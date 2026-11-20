#!/usr/bin/env python3
"""Genera el catálogo A–Z, la tabla periódica y el banner de C# Atlas.

Fuente única de verdad: data/caracteristicas.csv
Salidas:
  - docs/17-catalogo-a-z.md
  - assets/csharp-atlas-tabla-periodica.svg
  - assets/csharp-atlas-banner.svg

Uso:  python3 tools/generar_atlas.py   (solo biblioteca estándar, Python 3.9+)
"""
from __future__ import annotations

import csv
import unicodedata
from collections import Counter, defaultdict
from pathlib import Path
from xml.sax.saxutils import escape

RAIZ = Path(__file__).resolve().parent.parent
CSV = RAIZ / "data" / "caracteristicas.csv"
CATALOGO = RAIZ / "docs" / "17-catalogo-a-z.md"
POSTER = RAIZ / "assets" / "csharp-atlas-tabla-periodica.svg"
BANNER = RAIZ / "assets" / "csharp-atlas-banner.svg"

EDICION = "noviembre 2026"
FUENTE = "Segoe UI, Inter, Helvetica Neue, Arial, sans-serif"

FAMILIAS = [
    ("tipos", "Tipos y genéricos", "#A78BFA"),
    ("datos", "Modelado de datos", "#F472B6"),
    ("funcional", "Funciones y LINQ", "#22D3EE"),
    ("patrones", "Patrones", "#FB923C"),
    ("nulos", "Nulabilidad", "#FACC15"),
    ("async", "Asincronía", "#34D399"),
    ("memoria", "Memoria y rendimiento", "#F87171"),
    ("colecciones", "Colecciones", "#2DD4BF"),
    ("ergonomia", "Ergonomía", "#60A5FA"),
    ("meta", "Metaprogramación e interop", "#CBD5E1"),
]
NOMBRE_FAMILIA = {k: n for k, n, _ in FAMILIAS}
COLOR_FAMILIA = {k: c for k, _, c in FAMILIAS}

# Versión -> (año, capítulo)
VERSIONES = {
    "1.0": ("2002", "02-csharp-1-cimientos.md"),
    "1.2": ("2003", "02-csharp-1-cimientos.md"),
    "2": ("2005", "03-csharp-2-genericos.md"),
    "3": ("2007", "04-csharp-3-linq.md"),
    "4": ("2010", "05-csharp-4-5-dinamismo-y-asincronia.md"),
    "5": ("2012", "05-csharp-4-5-dinamismo-y-asincronia.md"),
    "6": ("2015", "06-csharp-6-roslyn.md"),
    "7.0": ("2017", "07-csharp-7-tuplas-patrones-y-ref.md"),
    "7.1": ("2017", "07-csharp-7-tuplas-patrones-y-ref.md"),
    "7.2": ("2017", "07-csharp-7-tuplas-patrones-y-ref.md"),
    "7.3": ("2018", "07-csharp-7-tuplas-patrones-y-ref.md"),
    "8": ("2019", "08-csharp-8-nulabilidad-y-streams.md"),
    "9": ("2020", "09-csharp-9-10-records.md"),
    "10": ("2021", "09-csharp-9-10-records.md"),
    "11": ("2022", "10-csharp-11-matematica-generica.md"),
    "12": ("2023", "11-csharp-12-13-colecciones.md"),
    "13": ("2024", "11-csharp-12-13-colecciones.md"),
    "14": ("2025", "12-csharp-14-extensiones.md"),
    "15": ("2026", "13-csharp-15-uniones.md"),
}
# Columnas del póster: las versiones menores se agrupan con su versión mayor.
COLUMNAS = [
    ("1", ["1.0", "1.2"], "2002"), ("2", ["2"], "2005"), ("3", ["3"], "2007"), ("4", ["4"], "2010"),
    ("5", ["5"], "2012"), ("6", ["6"], "2015"), ("7", ["7.0", "7.1", "7.2", "7.3"], "2017"), ("8", ["8"], "2019"),
    ("9", ["9"], "2020"), ("10", ["10"], "2021"), ("11", ["11"], "2022"), ("12", ["12"], "2023"),
    ("13", ["13"], "2024"), ("14", ["14"], "2025"), ("15", ["15"], "2026"),
]
CLASE = {"C": "Compilador", "B": "Biblioteca", "R": "Runtime"}
ESTADO = {"GA": "🟢", "RC": "🟡 RC", "Preview": "🟣 Preview"}


def clave_orden(texto: str) -> str:
    sin_tildes = unicodedata.normalize("NFKD", texto).encode("ascii", "ignore").decode()
    return sin_tildes.lower()


def leer() -> list[dict]:
    with CSV.open(encoding="utf-8") as f:
        filas = list(csv.DictReader(f))
    desconocidas = {r["familia"] for r in filas} - set(NOMBRE_FAMILIA)
    versiones = {r["version"] for r in filas} - set(VERSIONES)
    if desconocidas or versiones:
        raise SystemExit(f"Valores desconocidos en el CSV: {desconocidas} {versiones}")
    repetidos = [k for k, n in Counter(r["simbolo"] for r in filas).items() if n > 1]
    if repetidos:
        raise SystemExit(f"Símbolos repetidos en el CSV: {repetidos}")
    return filas


def celda(texto: str) -> str:
    return texto.replace("|", "\\|")


def generar_catalogo(filas: list[dict]) -> str:
    total = len(filas)
    lineas = [
        '<div align="center">', "",
        "# 17 · Catálogo A–Z de características de C#", "",
        f"**{total} características con su versión, familia, requisito y capítulo · edición {EDICION}**", "",
        "[⬅ 16 · Guías de decisión](16-guias-de-decision.md) · [🏠 Índice](../README.md) · "
        "[18 · Migración ➡](18-migracion-y-ciclo-de-vida.md)", "",
        "</div>", "",
        "> [!NOTE]",
        "> Este archivo se **genera automáticamente** desde [`data/caracteristicas.csv`](../data/caracteristicas.csv) con",
        "> `python3 tools/generar_atlas.py`. Para corregir o añadir una característica, edita el CSV y ejecuta el script.", "",
        '<a href="../assets/csharp-atlas-tabla-periodica.svg"><img src="../assets/csharp-atlas-tabla-periodica.svg" '
        'alt="Tabla periódica de características de C#" width="100%"></a>', "",
        "## Resumen por versión", "",
        "| Versión | Año | Características | Compilador | Biblioteca | Runtime |",
        "|---|---|---:|---:|---:|---:|",
    ]
    por_version: dict[str, Counter] = defaultdict(Counter)
    for r in filas:
        por_version[r["version"]][r["clase"]] += 1
    for v, (anio, capitulo) in VERSIONES.items():
        c = por_version[v]
        lineas.append(f"| [C# {v}]({capitulo}) | {anio} | {sum(c.values())} | {c['C']} | {c['B']} | {c['R']} |")
    c = Counter(r["clase"] for r in filas)
    lineas += [f"| **Total** | | **{total}** | **{c['C']}** | **{c['B']}** | **{c['R']}** |", "",
               "## Resumen por familia", "", "| Familia | Características |", "|---|---:|"]
    familias = Counter(r["familia"] for r in filas)
    lineas += [f"| {n} | {familias[k]} |" for k, n, _ in FAMILIAS]
    lineas += ["",
               "**Leyenda.** Estado: 🟢 estable · 🟣 *preview*. "
               "Requisito: lo que exige por debajo del compilador; consulta "
               "[01 § Qué necesita cada característica](01-modelo-de-versiones.md#3-qué-necesita-cada-característica).", "",
               "## Catálogo", ""]
    letra = ""
    for r in sorted(filas, key=lambda x: clave_orden(x["nombre"])):
        inicial = clave_orden(r["nombre"])[0].upper()
        if inicial != letra:
            letra = inicial
            lineas += ["", f"### {letra}", "", "| Característica | C# | Familia | Requisito | Estado | Descripción |",
                       "|---|---|---|---|---|---|"]
        cap = VERSIONES[r["version"]][1]
        lineas.append(f"| {celda(r['nombre'])} | [{r['version']}]({cap}) | {NOMBRE_FAMILIA[r['familia']]} | "
                      f"{CLASE[r['clase']]} | {ESTADO[r['estado']]} | {celda(r['descripcion'])} |")
    return "\n".join(lineas) + "\n"


# --------------------------------------------------------------------------- póster
TILE, GAP, MARGEN, CABECERA = 104, 8, 40, 190


def tile(r: dict, x: int, y: int) -> list[str]:
    color = COLOR_FAMILIA[r["familia"]]
    simbolo = r["simbolo"]
    borde = 'stroke-dasharray="6 4"' if r["estado"] != "GA" else ""
    partes = [
        f'<g transform="translate({x},{y})">',
        f'<rect width="{TILE}" height="{TILE}" rx="10" fill="{color}" fill-opacity="0.13" stroke="{color}" stroke-width="1.5" {borde}/>',
        f'<text x="9" y="17" font-size="11" fill="#CBD5E1">{escape(r["version"])}</text>',
        f'<text x="{TILE // 2}" y="50" font-size="30" font-weight="700" fill="{color}" text-anchor="middle">{escape(simbolo)}</text>',
    ]
    lineas = r["etiqueta"].split("|")
    y0 = 72 if len(lineas) == 2 else 80
    for i, l in enumerate(lineas):
        partes.append(f'<text x="{TILE // 2}" y="{y0 + i * 14}" font-size="12" fill="#F8FAFC" text-anchor="middle">{escape(l)}</text>')
    partes.append("</g>")
    return partes


def generar_poster(filas: list[dict]) -> str:
    columnas = []
    for nombre, versiones, anio in COLUMNAS:
        items = [r for v in versiones for r in filas if r["version"] == v and r["poster"] == "1"]
        columnas.append((nombre, anio, items))
    alto_max = max(len(i) for _, _, i in columnas)
    ancho = MARGEN * 2 + len(columnas) * (TILE + GAP) - GAP
    alto = CABECERA + 44 + alto_max * (TILE + GAP) + 110
    total = sum(len(i) for _, _, i in columnas)
    s = [
        f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 {ancho} {alto}" width="{ancho}" height="{alto}" font-family="{FUENTE}" role="img" aria-labelledby="pt">',
        f'<title id="pt">Tabla periódica de las características de C#, edición {EDICION}</title>',
        '<defs><linearGradient id="pf" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#0B0620"/><stop offset="0.6" stop-color="#160B33"/><stop offset="1" stop-color="#21104A"/></linearGradient></defs>',
        f'<rect width="{ancho}" height="{alto}" fill="url(#pf)"/>',
        f'<text x="{MARGEN}" y="74" font-size="46" font-weight="700" fill="#F8FAFC">Tabla periódica de C#</text>',
        f'<text x="{MARGEN}" y="112" font-size="20" fill="#C4B5FD">{total} características clave, de C# 1 a C# 15 · cada columna es una versión del lenguaje · edición {EDICION}</text>',
    ]
    for i, (k, n, c) in enumerate(FAMILIAS):
        x = MARGEN + (i % 5) * 300
        y = 138 + (i // 5) * 26
        s.append(f'<rect x="{x}" y="{y}" width="16" height="16" rx="4" fill="{c}"/>')
        s.append(f'<text x="{x + 24}" y="{y + 13}" font-size="15" fill="#E2E8F0">{escape(n)}</text>')
    for ci, (nombre, anio, items) in enumerate(columnas):
        x = MARGEN + ci * (TILE + GAP)
        y = CABECERA + 14
        s.append(f'<text x="{x + TILE // 2}" y="{y + 8}" font-size="20" font-weight="700" fill="#F8FAFC" text-anchor="middle">C# {nombre}</text>')
        s.append(f'<text x="{x + TILE // 2}" y="{y + 27}" font-size="13" fill="#A5B4FC" text-anchor="middle">{anio}</text>')
        for ti, r in enumerate(items):
            s += tile(r, x, CABECERA + 52 + ti * (TILE + GAP))
    s.append(f'<text x="{MARGEN}" y="{alto - 34}" font-size="14" fill="#94A3B8">Borde discontinuo: características en preview al 20 de noviembre de 2026. Fuente: data/caracteristicas.csv. Guía no oficial.</text>')
    s.append("</svg>")
    return "\n".join(s) + "\n"


# --------------------------------------------------------------------------- banner
HITOS = [("2", "Genéricos"), ("3", "LINQ"), ("5", "async"), ("7", "Tuplas"), ("8", "string?"),
         ("9", "record"), ("11", "INumber"), ("12", "[ ]"), ("14", "extension"), ("15", "union")]


def generar_banner(filas: list[dict]) -> str:
    w, h = 1600, 420
    s = [
        f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 {w} {h}" width="{w}" height="{h}" font-family="{FUENTE}" role="img" aria-labelledby="bt">',
        '<title id="bt">C# Atlas · Guía técnica de la evolución del lenguaje C#</title>',
        '<defs>'
        '<linearGradient id="bf" x1="0" y1="0" x2="1" y2="1"><stop offset="0" stop-color="#0B0620"/><stop offset="0.6" stop-color="#1A0B3B"/><stop offset="1" stop-color="#3B1A7A"/></linearGradient>'
        '<radialGradient id="bh" cx="0.8" cy="0.42" r="0.5"><stop offset="0" stop-color="#9B4F96" stop-opacity="0.45"/><stop offset="1" stop-color="#9B4F96" stop-opacity="0"/></radialGradient>'
        '<pattern id="bp" width="22" height="22" patternUnits="userSpaceOnUse"><circle cx="1.5" cy="1.5" r="1.1" fill="#FFFFFF" fill-opacity="0.05"/></pattern>'
        '<linearGradient id="bm" x1="0" y1="0" x2="1" y2="0"><stop offset="0" stop-color="#C4B5FD"/><stop offset="1" stop-color="#9B4F96"/></linearGradient>'
        '</defs>',
        f'<rect width="{w}" height="{h}" fill="url(#bf)"/><rect width="{w}" height="{h}" fill="url(#bh)"/><rect width="{w}" height="{h}" fill="url(#bp)"/>',
        '<g transform="translate(72,70)"><path d="M46 0 L92 26 L92 78 L46 104 L0 78 L0 26 Z" fill="#68217A" fill-opacity="0.55" stroke="url(#bm)" stroke-width="3.5"/>'
        '<text x="46" y="68" font-size="42" font-weight="700" fill="#F8FAFC" text-anchor="middle">C#</text></g>',
        '<text x="190" y="118" font-size="72" font-weight="700" fill="#F8FAFC">C# Atlas</text>',
        '<text x="192" y="162" font-size="26" fill="#DDD6FE">La evolución del lenguaje, versión por versión</text>',
        f'<text x="192" y="198" font-size="19" fill="#A5B4FC">De C# 1 a C# 15 · {len(filas)} características · ejemplos compilados · {EDICION}</text>',
    ]
    x0, x1, y = 90, 1510, 310
    s.append(f'<line x1="{x0}" y1="{y}" x2="{x1}" y2="{y}" stroke="url(#bm)" stroke-width="3"/>')
    paso = (x1 - x0) / (len(HITOS) - 1)
    for i, (v, nombre) in enumerate(HITOS):
        x = x0 + i * paso
        ultimo = i == len(HITOS) - 1
        color = "#FACC15" if ultimo else "#C4B5FD"
        s.append(f'<circle cx="{x:.0f}" cy="{y}" r="11" fill="#1A0B3B" stroke="{color}" stroke-width="3"/>')
        s.append(f'<text x="{x:.0f}" y="{y - 24}" font-size="19" font-weight="700" fill="#F8FAFC" text-anchor="middle">C# {v}</text>')
        s.append(f'<text x="{x:.0f}" y="{y + 38}" font-size="16" fill="{color}" text-anchor="middle">{escape(nombre)}</text>')
    s.append("</svg>")
    return "\n".join(s) + "\n"


def main() -> None:
    filas = leer()
    CATALOGO.write_text(generar_catalogo(filas), encoding="utf-8")
    POSTER.write_text(generar_poster(filas), encoding="utf-8")
    BANNER.write_text(generar_banner(filas), encoding="utf-8")
    print(f"{len(filas)} características → {CATALOGO.name}, {POSTER.name}, {BANNER.name}")


if __name__ == "__main__":
    main()
