#!/usr/bin/env python3
"""Compila (y opcionalmente ejecuta) cada bloque ```csharp del atlas con su versión exacta del lenguaje.

Cada ejemplo se compila con `-langversion:N`, de modo que un ejemplo de C# 7.3 demuestra que
solo usa sintaxis de C# 7.3. Se necesita un SDK de .NET 11 o posterior (el compilador acepta
todas las versiones del lenguaje, de la 1 a la 15).

Marcas en Markdown
  <!-- cs=N -->                 versión por defecto para los bloques siguientes del archivo
  ```csharp cs=N                versión de este bloque
  ```csharp cuerpo              envuelve el bloque en `class V { void M() { ... } }` (C# anterior a 9)
  ```csharp run                 ejecuta y compara la salida con los comentarios `// → texto`
  ```csharp err=CS1234          el bloque DEBE fallar con ese error
  ```csharp warn=CS1234         el bloque DEBE producir esa advertencia
  ```csharp preview             compila con -langversion:preview y AllowUnsafeBlocks
  ```csharp unsafe              compila con AllowUnsafeBlocks
  ```csharp omitir              no se compila (pseudocódigo o forma generada por el compilador)

Uso:
  python3 tools/verificar_ejemplos.py [--dotnet RUTA] [--solo 13] [--lista]
"""
from __future__ import annotations

import argparse
import concurrent.futures as cf
import json
import os
import re
import shutil
import subprocess
import sys
import tempfile
from dataclasses import dataclass, field
from pathlib import Path

RAIZ = Path(__file__).resolve().parent.parent
USINGS = [
    "System", "System.Collections.Generic", "System.IO", "System.Linq", "System.Net.Http",
    "System.Threading", "System.Threading.Tasks",
]
FLAGS = {"cuerpo", "run", "preview", "unsafe", "omitir"}


@dataclass
class Bloque:
    archivo: Path
    linea: int
    codigo: str
    cs: str
    flags: set[str] = field(default_factory=set)
    err: str | None = None
    warn: str | None = None

    @property
    def nombre(self) -> str:
        return f"{self.archivo.relative_to(RAIZ)}:{self.linea}"


def leer_bloques(archivo: Path) -> list[Bloque]:
    bloques: list[Bloque] = []
    cs = "15"
    lineas = archivo.read_text(encoding="utf-8").split("\n")
    i = 0
    while i < len(lineas):
        texto = lineas[i]
        directiva = re.fullmatch(r"\s*<!--\s*cs=([\d.]+)\s*-->\s*", texto)
        if directiva:
            cs = directiva.group(1)
        apertura = re.fullmatch(r"\s*```csharp(.*)", texto)
        if apertura:
            inicio = i + 1
            info = apertura.group(1).split()
            cuerpo: list[str] = []
            i += 1
            while not re.fullmatch(r"\s*```\s*", lineas[i]):
                cuerpo.append(lineas[i])
                i += 1
            b = Bloque(archivo, inicio, "\n".join(cuerpo), cs)
            for palabra in info:
                if palabra.startswith("cs="):
                    b.cs = palabra[3:]
                elif palabra.startswith("err="):
                    b.err = palabra[4:]
                elif palabra.startswith("warn="):
                    b.warn = palabra[5:]
                elif palabra in FLAGS:
                    b.flags.add(palabra)
                else:
                    raise SystemExit(f"{archivo}:{inicio}: marca desconocida {palabra!r}")
            bloques.append(b)
        i += 1
    return bloques


def localizar_sdk(dotnet: str) -> tuple[Path, Path, list[Path], str]:
    exe = Path(shutil.which(dotnet) or dotnet).resolve()
    raiz = exe.parent
    sdks = sorted((raiz / "sdk").glob("*/Roslyn/bincore/csc.dll"))
    candidatos = [s for s in sdks if int(s.parts[-4].split(".")[0]) >= 11]
    if not candidatos:
        raise SystemExit("Se necesita un SDK de .NET 11 o posterior. Usa --dotnet RUTA.")
    runtime = sorted((raiz / "shared" / "Microsoft.NETCore.App").glob("11.*"))[-1]
    refs = sorted((raiz / "packs" / "Microsoft.NETCore.App.Ref").glob("11.*/ref/net11.0/*.dll"))
    if not refs:
        refs = [p for p in runtime.glob("*.dll") if p.name.startswith(("System", "Microsoft", "netstandard", "mscorlib", "WindowsBase"))
                and not p.name.startswith(("Microsoft.DiaSymReader", "System.Private.DataContract"))]
    return exe, candidatos[-1], refs, runtime.name


def fuente(b: Bloque) -> tuple[str, str | None]:
    """Devuelve (código, archivo de usings globales o None)."""
    codigo = b.codigo
    if "cuerpo" in b.flags:
        codigo = "class V__ { void M__() {\n" + codigo + "\n} }"
    version = 99.0 if "preview" in b.flags else float(b.cs)
    if version >= 10:
        return codigo, "\n".join(f"global using {u};" for u in USINGS)
    prologo = "\n".join(f"using {u};" for u in USINGS)
    return prologo + "\n" + codigo, None


def verificar(b: Bloque, exe: Path, csc: Path, rsp: Path, runtime: str) -> tuple[Bloque, str | None, int]:
    with tempfile.TemporaryDirectory(prefix="atlas-") as tmp:
        d = Path(tmp)
        codigo, globales = fuente(b)
        (d / "Ejemplo.cs").write_text(codigo, encoding="utf-8")
        archivos = ["Ejemplo.cs"]
        if globales:
            (d / "Usings.cs").write_text(globales, encoding="utf-8")
            archivos.append("Usings.cs")
        version = "preview" if "preview" in b.flags else b.cs
        args = [str(exe), "exec", str(csc), "-nologo", f"@{rsp}", f"-langversion:{version}", "-out:Ejemplo.dll"]
        if float(99 if version == "preview" else version) >= 8:
            args.append("-nullable:enable")
        if b.flags & {"preview", "unsafe"}:
            args.append("-unsafe")

        def compilar(objetivo: str) -> tuple[int, str]:
            r = subprocess.run(args + [f"-t:{objetivo}"] + archivos, cwd=d, capture_output=True, text=True)
            return r.returncode, r.stdout + r.stderr

        rc, out = compilar("exe")
        errores = re.findall(r"error ([A-Z]+\d+)", out)
        if rc != 0 and set(errores) == {"CS5001"}:
            rc, out = compilar("library")
            errores = re.findall(r"error ([A-Z]+\d+)", out)
        avisos = len(re.findall(r"warning CS", out))

        if b.err:
            if b.err in errores:
                return b, None, avisos
            return b, f"se esperaba {b.err}; errores: {errores or 'ninguno'}", avisos
        if rc != 0:
            return b, "\n".join(l for l in out.splitlines() if "error" in l)[:900], avisos
        if b.warn and f"warning {b.warn}" not in out:
            return b, f"se esperaba la advertencia {b.warn}", avisos

        if "run" in b.flags:
            (d / "Ejemplo.runtimeconfig.json").write_text(json.dumps({"runtimeOptions": {
                "tfm": "net11.0", "framework": {"name": "Microsoft.NETCore.App", "version": runtime},
            }}), encoding="utf-8")
            try:
                r = subprocess.run([str(exe), "Ejemplo.dll"], cwd=d, capture_output=True, text=True, timeout=30)
            except subprocess.TimeoutExpired:
                return b, "la ejecución excedió 30 s", avisos
            esperado = re.findall(r"//\s*→\s*(.*)$", b.codigo, flags=re.M)
            obtenido = [l.rstrip() for l in r.stdout.splitlines()]
            esperado = [e.rstrip() for e in esperado]
            if r.returncode != 0 or obtenido != esperado:
                return b, f"salida distinta\n  esperado: {esperado}\n  obtenido: {obtenido}\n  {r.stderr[:300]}", avisos
    return b, None, avisos


def main() -> int:
    p = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    p.add_argument("--dotnet", default=os.environ.get("DOTNET_ATLAS", "dotnet"), help="ruta al ejecutable dotnet con SDK 11+")
    p.add_argument("--solo", help="filtra archivos que contengan este texto")
    p.add_argument("--lista", action="store_true", help="solo cuenta los bloques")
    p.add_argument("--hilos", type=int, default=os.cpu_count() or 4)
    a = p.parse_args()

    archivos = [RAIZ / "README.md", *sorted((RAIZ / "docs").glob("*.md"))]
    bloques = [b for f in archivos if not a.solo or a.solo in f.name for b in leer_bloques(f)]
    activos = [b for b in bloques if "omitir" not in b.flags]
    if a.lista:
        print(f"{len(bloques)} bloques, {len(activos)} verificables")
        return 0

    exe, csc, refs, runtime = localizar_sdk(a.dotnet)
    print(f"Compilador: {csc.parts[-4]} · runtime {runtime} · {len(activos)} bloques de {len(bloques)}")
    with tempfile.TemporaryDirectory(prefix="atlas-rsp-") as tmp:
        rsp = Path(tmp) / "refs.rsp"
        rsp.write_text("\n".join(f'-r:"{r}"' for r in refs) + "\n-nostdlib+\n-nowarn:CS1701,CS1702\n", encoding="utf-8")
        fallos = 0
        ejecutados = sum("run" in b.flags for b in activos)
        negativos = sum(bool(b.err) for b in activos)
        with cf.ThreadPoolExecutor(a.hilos) as ex:
            for b, error, _ in ex.map(lambda x: verificar(x, exe, csc, rsp, runtime), activos):
                if error:
                    fallos += 1
                    print(f"\n✗ {b.nombre} (C# {b.cs})\n{error}")
    print(f"\n{len(activos) - fallos}/{len(activos)} correctos · {ejecutados} ejecutados · {negativos} con error esperado · "
          f"{len(bloques) - len(activos)} omitidos")
    return 1 if fallos else 0


if __name__ == "__main__":
    sys.exit(main())
