<div align="center">

# 19 · Referencia rápida y glosario

**CLI de .NET, varios puntos de entrada, operadores, plantilla de proyecto, el código de este repositorio y términos clave**

[⬅ 18 · Migración](18-migracion-y-ciclo-de-vida.md) · [🏠 Índice](../README.md)

</div>

---

## Contenido

1. [Crear un proyecto con la CLI](#1-crear-un-proyecto-con-la-cli)
2. [Varios puntos de entrada](#2-varios-puntos-de-entrada)
3. [Plantilla de proyecto recomendada](#3-plantilla-de-proyecto-recomendada)
4. [Chuleta de operadores](#4-chuleta-de-operadores)
5. [Comandos esenciales](#5-comandos-esenciales)
6. [El código de este repositorio](#6-el-código-de-este-repositorio)
7. [Glosario](#7-glosario)
8. [Para seguir aprendiendo](#8-para-seguir-aprendiendo)

---

## 1. Crear un proyecto con la CLI

```bash
# 1. Comprobar el SDK
dotnet --list-sdks

# 2. Crear la solución y un proyecto de consola
dotnet new sln -n MiSolucion
dotnet new console -n MiApp

# 3. Añadir el proyecto a la solución
dotnet sln add MiApp/MiApp.csproj

# 4. Restaurar, compilar y ejecutar
dotnet restore
dotnet build
dotnet run --project MiApp

# 5. Abrir en Visual Studio Code (extensión C# Dev Kit)
code .
```

`dotnet run` ya restaura y compila; los pasos separados son útiles sobre todo en integración continua. Sin un proyecto, también basta con
[un solo archivo](12-csharp-14-extensiones.md#9-aplicaciones-de-un-solo-archivo): `dotnet run app.cs`.

---

## 2. Varios puntos de entrada

Un proyecto ejecutable solo puede tener **un** punto de entrada. Si varias clases declaran `Main`, el compilador emite **CS0017**.

```csharp err=CS0017
class Programa1 { public static void Main() => Console.WriteLine("Uno"); }
class Programa2 { public static void Main() => Console.WriteLine("Dos"); }
```

| Solución | Cómo |
|---|---|
| Fijar en el proyecto | `<StartupObject>MiApp.Programa2</StartupObject>` |
| Elegir al compilar | `dotnet run -p:StartupObject=MiApp.Programa2` |
| Con `csc` | Opción `-main:Programa2` |
| Un `Main` que despacha | Según `args`, llama a cada programa |
| Separar en proyectos | Lo más limpio a largo plazo |

> [!NOTE]
> La versión anterior de este README recomendaba `--launch-profile`, pero los perfiles de inicio configuran variables y argumentos, **no** el punto de entrada. Y las instrucciones de nivel superior
> solo pueden aparecer en un archivo por proyecto.

---

## 3. Plantilla de proyecto recomendada

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>   <!-- C# 14 por defecto: sin LangVersion -->
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <AnalysisLevel>latest-recommended</AnalysisLevel>
  </PropertyGroup>
</Project>
```

---

## 4. Chuleta de operadores

| Operador | Significado | Desde |
|---|---|---|
| `a ?? b` | `b` si `a` es nulo | C# 2 |
| `T?` | Tipo de valor anulable | C# 2 |
| `x => y` | Lambda | C# 3 |
| `a?.b`, `a?[i]` | Acceso condicional nulo | C# 6 |
| `nameof(x)` | Nombre en compilación | C# 6 |
| `$"{x}"` | Interpolación | C# 6 |
| `x is T y` | Patrón de tipo | C# 7 |
| `_` | Descarte | C# 7 |
| `a ??= b` | Asigna si es nulo | C# 8 |
| `^1`, `a..b` | Índice desde el final y rango | C# 8 |
| `x!` | Suprime la advertencia de nulo | C# 8 |
| `x with { }` | Copia no destructiva | C# 9 |
| `and`, `or`, `not` | Patrones lógicos | C# 9 |
| `"""` | Cadena sin formato | C# 11 |
| `>>>` | Desplazamiento sin signo | C# 11 |
| `u8` | Literal UTF-8 | C# 11 |
| `[a, ..b]` | Expresión de colección | C# 12 |
| `\e` | Carácter de escape | C# 13 |
| `a?.b = c` | Asignación condicional | C# 14 |
| `[with(…), …]` | Argumentos de colección | C# 15 |
| `break etiqueta` | Salto con etiqueta | C# 15 |

---

## 5. Comandos esenciales

| Comando | Función |
|---|---|
| `dotnet new list` | Plantillas disponibles |
| `dotnet add package X` | Añade un paquete |
| `dotnet test` | Ejecuta las pruebas |
| `dotnet publish -c Release` | Publica |
| `dotnet format` | Aplica estilo |
| `dotnet watch` | Recompila al cambiar |
| `dotnet tool install -g X` | Herramienta global |

---

## 6. El código de este repositorio

Junto al atlas, la raíz contiene una multiplicación concurrente de matrices, analizada en [15 § Caso real](15-bajo-el-capo-y-rendimiento.md#8-caso-real-la-multiplicación-de-matrices-de-este-repositorio).

| Archivo | Contenido |
|---|---|
| `Matrix.cs` | Matriz genérica sobre `INumber<T>` |
| `MatrixMultiply.cs` | Multiplicación con canal y trabajadores |
| `MulOption.cs`, `MulOptions.cs`, `MulOptionsBuilder.cs` | Patrón de opciones |
| `MatrixFill.cs`, `MatrixException.cs` | Utilidades y errores |
| `MatrixBeanchmarks.cs`, `BenchmarkProgram.cs` | Medición con BenchmarkDotNet |
| `Matrix.Test/` | Pruebas con xUnit |

> [!WARNING]
> **Estado a 4-nov-2026: el proyecto no compila.** El refactor que separó el código en varios archivos dejó pendientes:
>
> - `MatrixBeanchmarks.cs:35` tiene una llave de cierre y un `Main` sobrantes (CS1519). Es el único error de compilación actual.
> - Una vez corregido, `Program.cs` y `BenchmarkProgram.cs` declaran dos `Main`: hará falta `<StartupObject>`.
> - `C-sharp.csproj` referencia `xunit` 1.7.0.1540, un paquete para .NET Framework (NU1701).
>
> El código de los capítulos del atlas no depende de estos archivos.

---

## 7. Glosario

| Término | Definición |
|---|---|
| **Argumento de tipo** | El tipo concreto que sustituye a `T` |
| **Árbol de expresión** | Código representado como datos |
| ***Boxing*** | Copiar un tipo de valor a un objeto en el *heap* |
| **Caso** | Cada tipo posible de una unión |
| **CLR** | *Common Language Runtime*: ejecuta el IL |
| **Clausura** | Función que captura variables de su entorno |
| **Compilación por niveles** | El JIT compila rápido y luego reoptimiza lo que más se usa |
| **Contravarianza** | Aceptar un tipo menos derivado (`in`) |
| **Copia defensiva** | Copia oculta de un `struct` para proteger un valor de solo lectura |
| **Covarianza** | Aceptar un tipo más derivado (`out`) |
| **Descarte** | `_`, un valor que se ignora |
| **Ejecución diferida** | El trabajo ocurre al enumerar |
| **Exhaustividad** | Un `switch` cubre todos los valores |
| **Generador de código** | Componente que añade código durante la compilación |
| **Go-live** | Licencia para usar una versión preliminar en producción, con soporte |
| ***Heap* administrado** | Memoria gestionada por el recolector |
| **IL** | Lenguaje intermedio que emite el compilador |
| **Interceptor** | Método que sustituye una llamada concreta en compilación |
| **JIT** | Compilador a código nativo en ejecución |
| **LDM** | *Language Design Meeting*, reuniones de diseño de C# |
| **LTS / STS** | Soporte a largo y a corto plazo |
| ***Lowering*** | Traducir construcciones de alto nivel a otras más simples |
| **Máquina de estados** | Clase o `struct` que genera el compilador para `yield` y `async` |
| **Native AOT** | Compilación anticipada a código nativo |
| **Operador elevado** | Operador de `T` aplicado a `T?` |
| **Palabra contextual** | Solo es reservada en ciertas posiciones |
| **Patrón** | Prueba sobre la forma de un valor |
| **Polyfill** | Tipo que declaras para usar una característica en un destino antiguo |
| **Reificación** | Los genéricos existen en ejecución |
| **Roslyn** | La plataforma de compiladores de C# y Visual Basic |
| **Seguridad de referencias** | Reglas que impiden que una referencia sobreviva a su destino |
| **TAP** | Patrón asíncrono basado en `Task` |
| **TFM** | *Target Framework Moniker*, por ejemplo `net10.0` |
| **Tipo producto / suma** | Combina A *y* B / A *o* B |
| **Tipo de referencia anulable** | Anotación estática `string?` |

---

## 8. Para seguir aprendiendo

| Recurso | Para qué |
|---|---|
| [Documentación de C#](https://learn.microsoft.com/dotnet/csharp/) | Referencia oficial |
| [Especificación del lenguaje](https://learn.microsoft.com/dotnet/csharp/language-reference/language-specification/readme) | Detalle normativo |
| [dotnet/csharplang](https://github.com/dotnet/csharplang) | Diseño del lenguaje |
| [dotnet/roslyn](https://github.com/dotnet/roslyn) | Compilador |
| [SharpLab](https://sharplab.io/) | Ver IL y código traducido |
| [Blog de .NET](https://devblogs.microsoft.com/dotnet/) | Novedades |

**Laboratorios por versión:** [C# 1](https://github.com/CristianSifuentes/CSharp1) · [2](https://github.com/CristianSifuentes/CSharp2) · [3](https://github.com/CristianSifuentes/CSharp3) · [4](https://github.com/CristianSifuentes/CSharp4) · [5](https://github.com/CristianSifuentes/CSharp5) · [6](https://github.com/CristianSifuentes/CSharp6) · [7](https://github.com/CristianSifuentes/CSharp7) · [8](https://github.com/CristianSifuentes/CSharp8) · [9](https://github.com/CristianSifuentes/CSharp9) · [10](https://github.com/CristianSifuentes/CSharp10) · [11](https://github.com/CristianSifuentes/CSharp11) · [12](https://github.com/CristianSifuentes/CSharp12) · [13](https://github.com/CristianSifuentes/CSharp13) · [14](https://github.com/CristianSifuentes/CSharp14) · [15](https://github.com/CristianSifuentes/CSharp15)
