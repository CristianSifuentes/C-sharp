<div align="center">

# 09 · C# 9 y 10: `record` y menos ceremonia

**Datos inmutables, programas sin plantilla, patrones lógicos, `global using` y manejadores de interpolación (2020–2021)**

[⬅ 08 · C# 8](08-csharp-8-nulabilidad-y-streams.md) · [🏠 Índice](../README.md) · [10 · C# 11 ➡](10-csharp-11-matematica-generica.md)

</div>

| Ficha | C# 9 | C# 10 |
|---|---|---|
| **Publicación** | noviembre de 2020 | noviembre de 2021 |
| **Visual Studio** | VS 2019 16.8 | VS 2022 17.0 |
| **Runtime** | .NET 5 (10-nov-2020) | .NET 6 LTS (8-nov-2021) |
| **`LangVersion` actual** | `9.0` | `10.0` |
| **Laboratorio** | [CristianSifuentes/CSharp9](https://github.com/CristianSifuentes/CSharp9) | [CristianSifuentes/CSharp10](https://github.com/CristianSifuentes/CSharp10) |

---

## Contenido

1. [C# 9: `record`](#1-c-9-record)
2. [C# 9: `init`](#2-c-9-init)
3. [C# 9: instrucciones de nivel superior](#3-c-9-instrucciones-de-nivel-superior)
4. [C# 9: patrones relacionales y lógicos](#4-c-9-patrones-relacionales-y-lógicos)
5. [C# 9: rendimiento e interoperabilidad](#5-c-9-rendimiento-e-interoperabilidad)
6. [C# 9: ajustes y generadores de código](#6-c-9-ajustes-y-generadores-de-código)
7. [C# 10: organización del código](#7-c-10-organización-del-código)
8. [C# 10: `record struct` y mejoras en `struct`](#8-c-10-record-struct-y-mejoras-en-struct)
9. [C# 10: manejadores de interpolación](#9-c-10-manejadores-de-interpolación)
10. [C# 10: lambdas y otras mejoras](#10-c-10-lambdas-y-otras-mejoras)
11. [Notas de migración](#11-notas-de-migración)
12. [Referencias](#12-referencias)

---

<!-- cs=9 -->

## 1. C# 9: `record`

### Antes y después

Un DTO correcto con igualdad por valor requería decenas de líneas: constructor, propiedades, `Equals`, `GetHashCode`, `==`, `!=`,
`ToString` y un método de copia. Cada campo nuevo era una oportunidad de olvidar algo.

```csharp run
var a = new Pedido(1, "MXN");
var b = new Pedido(1, "MXN");

Console.WriteLine(a == b);                    // → True
Console.WriteLine(ReferenceEquals(a, b));     // → False
Console.WriteLine(a);                         // → Pedido { Id = 1, Moneda = MXN }

var c = a with { Moneda = "USD" };            // copia no destructiva
Console.WriteLine(c.Moneda);                  // → USD

var (id, moneda) = a;                         // deconstrucción generada
Console.WriteLine(id);                        // → 1

public record Pedido(int Id, string Moneda);
```

### Lo que genera el compilador

| Miembro generado | Función |
|---|---|
| Propiedades `init` y constructor | A partir de los parámetros posicionales |
| `Equals`, `GetHashCode`, `==`, `!=` | Igualdad por valor |
| `EqualityContract` | Dos *records* solo son iguales si su tipo en ejecución coincide |
| `ToString` y `PrintMembers` | Representación legible |
| Constructor de copia y `<Clone>$` | Soporte para `with` |
| `Deconstruct` | Solo para *records* posicionales |

### Trampas

```csharp run
var a = new Lista(new[] { 1 });
var b = new Lista(new[] { 1 });
Console.WriteLine(a == b);            // → False

var copia = a with { };
copia.Datos[0] = 99;                  // with es una copia SUPERFICIAL
Console.WriteLine(a.Datos[0]);        // → 99

public record Lista(int[] Datos);
```

| Trampa | Motivo |
|---|---|
| Arreglos y colecciones en un `record` | La igualdad compara **referencias** de esas propiedades |
| `with` | Copia superficial: los objetos internos se comparten |
| No es inmutable por decreto | `record` admite `{ get; set; }`, y una propiedad mutable en un `HashSet` rompe la búsqueda |
| Herencia | Solo entre `record`, y la comparación considera el tipo exacto |
| EF Core | Las entidades con seguimiento encajan mal con `record`: mejor reservarlos para DTO y objetos de valor |

---

## 2. C# 9: `init`

```csharp err=CS8852
var c = new Config { Ruta = "/tmp" };   // permitido en la inicialización
c.Ruta = "/otro";                       // error CS8852

public class Config { public string Ruta { get; init; } = ""; }
```

`init` requiere el tipo `System.Runtime.CompilerServices.IsExternalInit`, presente desde .NET 5. En destinos anteriores se resuelve declarándolo tú
mismo, un *polyfill* habitual. La evolución sigue en [C# 11](10-csharp-11-matematica-generica.md) con `required`.

---

## 3. C# 9: instrucciones de nivel superior

```csharp run
// Un archivo completo. El compilador genera la clase y el método de entrada.
Console.WriteLine(args.Length);  // → 0
return 0;
```

| Regla | Detalle |
|---|---|
| Un archivo por proyecto | Solo uno puede tener instrucciones de nivel superior |
| Orden | Las instrucciones **preceden** a los tipos y espacios de nombres (CS8803) |
| Disponibles | `args` y `await`; el método devuelve `int`, `Task` o `Task<int>` según el código |

---

## 4. C# 9: patrones relacionales y lógicos

```csharp run
Console.WriteLine(Tarifa(4));      // → infantil
Console.WriteLine(Tarifa(30));     // → adulto
Console.WriteLine(EsVocal('e'));   // → True

object o = 5;
Console.WriteLine(o is not null);  // → True
Console.WriteLine(o is int and > 3); // → True

static string Tarifa(int edad) => edad switch
{
    < 0 => throw new ArgumentOutOfRangeException(nameof(edad)),
    < 12 => "infantil",
    >= 12 and < 65 => "adulto",
    _ => "senior",
};

static bool EsVocal(char c) => c is 'a' or 'e' or 'i' or 'o' or 'u';
```

> [!CAUTION]
> `not` tiene más prioridad que `or`. `x is not null or 42` significa `(not null) or 42`, y el `42` sobra. Lo que probablemente querías es
> `x is not (null or 42)`. Desde VS 2022 17.13, el compilador advierte de este error frecuente.

---

## 5. C# 9: rendimiento e interoperabilidad

| Característica | Ejemplo | Para qué |
|---|---|---|
| Enteros nativos | `nint`, `nuint` | Tamaño de la plataforma, para interoperabilidad y matemática de punteros |
| Punteros a función | `delegate* unmanaged<int, int>` | Llamadas sin asignar delegados |
| `[SkipLocalsInit]` | Sobre un método | Evita poner a cero la memoria local, por ejemplo tras `stackalloc` |
| `[ModuleInitializer]` | Un método estático | Código al cargar el ensamblado, útil para generadores |

```csharp unsafe run
using System.Runtime.CompilerServices;

unsafe
{
    delegate*<int, int> doble = &Doble;   // sin objeto delegado
    Console.WriteLine(doble(21));         // → 42
}

static int Doble(int x) => x * 2;
```

---

## 6. C# 9: ajustes y generadores de código

| Característica | Antes | Después |
|---|---|---|
| `new` con tipo de destino | `List<int> a = new List<int>();` | `List<int> a = new();` |
| Lambda `static` | Captura accidental | `static x => x + 1`: error si captura |
| Condicional con tipo de destino | Conversión manual | `int? x = c ? 1 : null;` |
| Retornos covariantes | Conversión en el llamador | `override Hijo Clonar()` (requiere .NET 5) |
| `GetEnumerator` por extensión | No admitido | `foreach` sobre más tipos |
| Descartes en lambdas | `(a, b) =>` | `(_, _) => 0` |
| Atributos en funciones locales | No admitido | `[Conditional("DEBUG")] void F() { }` |
| Métodos parciales | Solo `void` y privados | Con accesibilidad y retorno, y entonces la implementación es obligatoria |

Los **generadores de código** llegaron con el SDK de .NET 5. Corren dentro del compilador, inspeccionan el código y **añaden** archivos: la
alternativa moderna a la reflexión en ejecución, necesaria para Native AOT. `System.Text.Json`, el registro de alto rendimiento y `[GeneratedRegex]` se
apoyan en ellos.

---

<!-- cs=10 -->

## 7. C# 10: organización del código

```csharp omitir
// GlobalUsings.cs: una sola vez para todo el proyecto
global using System.Text.Json;
global using static System.Math;
```

```csharp
// Espacio de nombres de archivo: un nivel menos de sangría en toda la base de código
namespace Tienda.Ventas;

public class Factura { }
```

`<ImplicitUsings>enable</ImplicitUsings>` es una función del **SDK**: genera un archivo con `global using` según el tipo de proyecto. Esa es la
razón por la que `dotnet new console` produce un `Console.WriteLine` sin ningún `using`.

---

## 8. C# 10: `record struct` y mejoras en `struct`

```csharp run
var a = new Punto(1, 2);
var b = a with { X = 5 };             // with también en struct y tipos anónimos
Console.WriteLine(a == new Punto(1, 2)); // → True
Console.WriteLine(b);                 // → Punto { X = 5, Y = 2 }

var anonimo = new { A = 1 } with { A = 2 };
Console.WriteLine(anonimo.A);         // → 2

Console.WriteLine(new Config().Puerto);     // → 443
Console.WriteLine(default(Config).Puerto);  // → 0

public readonly record struct Punto(int X, int Y);

public struct Config
{
    public int Puerto = 443;
    public Config() { }
}
```

| Forma | Propiedades generadas | Igualdad |
|---|---|---|
| `record` / `record class` | `init` | Por valor, en el *heap* |
| `record struct` | **`get; set;` mutables** | Por valor, sin reflexión |
| `readonly record struct` | `init` | Por valor, inmutable |

La última línea del ejemplo es la trampa: **`default` no ejecuta constructores ni inicializadores de campo**; tampoco lo hace crear un arreglo.

Un `struct` con inicializadores **debe** declarar un constructor, un cambio introducido en VS 2022 17.1:

```csharp err=CS8983
struct Malo { public int Puerto = 443; }  // error CS8983
```

---

## 9. C# 10: manejadores de interpolación

Antes, `log.Debug($"…{costoso}")` construía la cadena **aunque** el nivel estuviera desactivado. Un manejador deja a la API decidir.

```csharp run
using System.Runtime.CompilerServices;
using System.Text;

var log = new Registro { Activo = false };
log.Debug($"valor {Costoso()}");     // Costoso() NO se llama
log.Activo = true;
log.Debug($"valor {Costoso()}");     // → valor 42

static int Costoso() => 42;

class Registro
{
    public bool Activo;
    public void Debug([InterpolatedStringHandlerArgument("")] ref Manejador m)
    {
        if (Activo) Console.WriteLine(m.Texto);
    }
}

[InterpolatedStringHandler]
ref struct Manejador
{
    private readonly StringBuilder? _b;
    public string Texto => _b?.ToString() ?? "";

    public Manejador(int literal, int huecos, Registro r, out bool activo)
    {
        activo = r.Activo;                      // false: se omite todo lo demás
        _b = activo ? new StringBuilder(literal) : null;
    }

    public void AppendLiteral(string s) => _b!.Append(s);
    public void AppendFormatted<T>(T v) => _b!.Append(v);
}
```

La biblioteca base lo aprovecha en `DefaultInterpolatedStringHandler`: desde C# 10 con .NET 6, la interpolación corriente evita el *boxing* y las
cadenas temporales de `string.Format`. Basta recompilar.

---

## 10. C# 10: lambdas y otras mejoras

```csharp run
using System.Runtime.CompilerServices;

var doble = (int x) => x * 2;                 // tipo natural: Func<int, int>
var parsear = object (string s) => int.Parse(s);  // retorno explícito
Console.WriteLine(doble(4));                  // → 8
Console.WriteLine(parsear("1"));              // → 1

const string Base = "api";
const string Ruta = $"/{Base}/v1";            // const interpolado
Console.WriteLine(Ruta);                      // → /api/v1

var p = new Persona(new Direccion("MX"));
Console.WriteLine(p is { Direccion.Pais: "MX" }); // → True

int a = 1;
Validar(a > 5, Mensaje);

static void Validar(bool c, Action<string> f, [CallerArgumentExpression(nameof(c))] string e = "") => f(e);
static void Mensaje(string s) => Console.WriteLine(s); // → a > 5

record Direccion(string Pais);
record Persona(Direccion Direccion);
```

Además: atributos en lambdas, `sealed` en `ToString` de un *record*, asignación y declaración mezcladas en una deconstrucción,
`[AsyncMethodBuilder]` por método y un formato nuevo de `#line`. Los patrones de propiedad extendidos convierten `{ A: { B: 1 } }` en `{ A.B: 1 }`.

**Lo que quedó en *preview*:** los atributos genéricos y los miembros `static abstract` en interfaces, publicados en C# 11.

---

## 11. Notas de migración

| Versión | Tema | Acción |
|---|---|---|
| C# 9 | Clase anémica con igualdad a mano | Evaluar `record` |
| C# 9 | Retornos covariantes | Requieren runtime de .NET 5+ |
| C# 10 | `struct` con inicializadores | Declarar un constructor (CS8983) |
| C# 10 | `{{` en especificadores de formato | `{{{12:X}}}` imprime `{C}`: una llave cierra la interpolación |
| C# 10 | `global using` | Centralizar en un archivo y mantenerlo corto |
| Ambas | `.NET 5`, `.NET 6` | **Sin soporte**: ver [18 · Ciclo de vida](18-migracion-y-ciclo-de-vida.md) |

---

## 12. Referencias

- [Records](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/record) · [init](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/init)
- [Top-level statements](https://learn.microsoft.com/dotnet/csharp/fundamentals/program-structure/top-level-statements) · [Source generators](https://learn.microsoft.com/dotnet/csharp/roslyn-sdk/#source-generators)
- [Interpolated string handlers](https://learn.microsoft.com/dotnet/csharp/advanced-topics/performance/interpolated-string-handler) · [Breaking changes since C# 10](https://learn.microsoft.com/dotnet/csharp/whats-new/breaking-changes/compiler%20breaking%20changes%20-%20dotnet%207)
