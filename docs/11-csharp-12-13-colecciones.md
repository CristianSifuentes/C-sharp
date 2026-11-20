<div align="center">

# 11 · C# 12 y 13: colecciones y `ref struct`

**Constructores primarios, expresiones de colección, `params` generalizado, `Lock` y la expansión de `ref struct` (2023–2024)**

[⬅ 10 · C# 11](10-csharp-11-matematica-generica.md) · [🏠 Índice](../README.md) · [12 · C# 14 ➡](12-csharp-14-extensiones.md)

</div>

| Ficha | C# 12 | C# 13 |
|---|---|---|
| **Publicación** | noviembre de 2023 | noviembre de 2024 |
| **Visual Studio** | VS 2022 17.8 | VS 2022 17.12 |
| **Runtime** | .NET 8 LTS (14-nov-2023) | .NET 9 STS (12-nov-2024) |
| **`LangVersion` actual** | `12.0` | `13.0` |
| **Fin de soporte del runtime** | ⛔ 10-nov-2026 | ⛔ 10-nov-2026 |
| **Laboratorio** | [CristianSifuentes/CSharp12](https://github.com/CristianSifuentes/CSharp12) | [CristianSifuentes/CSharp13](https://github.com/CristianSifuentes/CSharp13) |

> [!CAUTION]
> Las versiones anteriores de este README afirmaban que C# 13 añadió los parámetros opcionales en lambdas. Son de **C# 12**. Y `var numeros = [1, 2, 3];` **no
> compila**: una expresión de colección necesita un tipo de destino.

---

## Contenido

1. [C# 12: constructores primarios](#1-c-12-constructores-primarios)
2. [C# 12: expresiones de colección](#2-c-12-expresiones-de-colección)
3. [C# 12: lambdas, alias y `ref readonly`](#3-c-12-lambdas-alias-y-ref-readonly)
4. [C# 12: arreglos en línea](#4-c-12-arreglos-en-línea)
5. [C# 12: `Experimental` e interceptores](#5-c-12-experimental-e-interceptores)
6. [C# 13: colecciones `params`](#6-c-13-colecciones-params)
7. [C# 13: `System.Threading.Lock`](#7-c-13-systemthreadinglock)
8. [C# 13: `ref struct` en todas partes](#8-c-13-ref-struct-en-todas-partes)
9. [C# 13: prioridad de resolución y propiedades parciales](#9-c-13-prioridad-de-resolución-y-propiedades-parciales)
10. [Notas de migración](#10-notas-de-migración)
11. [Referencias](#11-referencias)

---

<!-- cs=12 -->

## 1. C# 12: constructores primarios

### Antes y después

```csharp cs=11
// Antes: el nombre aparece tres veces por dependencia
public class Servicio
{
    private readonly HttpClient _http;
    public Servicio(HttpClient http) { _http = http; }
    public Task<string> Leer(string url) => _http.GetStringAsync(url);
}
```

```csharp
// Después
public class Servicio(HttpClient http)
{
    public Task<string> Leer(string url) => http.GetStringAsync(url);
}
```

### Semántica: no es lo mismo que en un `record`

| | `record` posicional | `class` o `struct` con constructor primario |
|---|---|---|
| Genera propiedades públicas | ✅ | ❌ |
| El parámetro es | Propiedad | **Parámetro** visible en todo el cuerpo |
| Si se usa en un método | — | Se captura en un **campo oculto** |
| ¿Es `readonly`? | Propiedad `init` | **No: es mutable** |

```csharp run
var c = new Contador(1);
c.Sumar();
Console.WriteLine(c.Total);  // → 2

public class Contador(int total)
{
    public int Total => total;
    public void Sumar() => total++;   // ¡compila! el parámetro capturado es mutable
}
```

Trampa común: usar el parámetro para inicializar una propiedad **y además** capturarlo crea dos copias que divergen. El compilador avisa:

```csharp warn=CS9124
public class Cuenta(decimal saldo)
{
    public decimal Saldo { get; } = saldo;       // primera copia
    public void Abonar(decimal m) => saldo += m; // segunda copia, CS9124
}
```

> [!TIP]
> Si necesitas `readonly` real, asígnalo: `private readonly HttpClient _http = http;`. Los constructores adicionales deben encadenar con `: this(...)`.

---

## 2. C# 12: expresiones de colección

```csharp run
int[] a = [1, 2, 3];
List<string> b = ["x", "y"];
Span<int> c = [4, 5];
HashSet<int> d = [1, 1, 2];
IEnumerable<int> e = [.. a, .. c, 9];       // elemento de propagación

Console.WriteLine(d.Count);       // → 2
Console.WriteLine(e.Sum());       // → 24

int[][] tabla = [[1, 2], [3]];
Console.WriteLine(tabla[0][1]);   // → 2

int[] vacio = [];
Console.WriteLine(vacio.Length);  // → 0
```

```csharp err=CS9176
var numeros = [1, 2, 3];  // error CS9176: no hay tipo de destino
```

| Destino | Qué genera el compilador |
|---|---|
| Arreglo, `Span<T>` | Construcción directa |
| `ReadOnlySpan<T>` con constantes | Datos en el ensamblado, sin asignar en muchos casos |
| Tipos con inicializador de colección | `new T()` + `Add` |
| `IEnumerable<T>`, `IReadOnlyList<T>` | Una implementación de solo lectura a elección del compilador |
| Tipos con `[CollectionBuilder]` | Un método de fábrica, como `ImmutableArray.Create` |

`[]` en un arreglo reutiliza una instancia vacía cuando puede. La propagación sustituye a `Concat(...).ToArray()` y conoce el tamaño final cuando
los orígenes son contables.

La idea continúa en [C# 15](13-csharp-15-uniones.md#5-argumentos-en-expresiones-de-colección), con `with(...)`.

---

## 3. C# 12: lambdas, alias y `ref readonly`

```csharp run
using Punto = (int X, int Y);    // alias de cualquier tipo, incluso una tupla

Punto p = (1, 2);
Console.WriteLine(p.Y);          // → 2

var saludar = (string nombre = "mundo") => $"hola {nombre}"; // parámetro por defecto
Console.WriteLine(saludar());    // → hola mundo

int valor = 5;
Console.WriteLine(Leer(in valor)); // → 5

static int Leer(ref readonly int x) => x;
```

`ref readonly` aclara la intención: como `in`, no permite modificar, pero **exige una variable**, de modo que no acepta un valor temporal sin
advertencia. Sirve para migrar parámetros `ref` antiguos sin romper a los llamadores.

---

## 4. C# 12: arreglos en línea

Un búfer de tamaño fijo dentro de un `struct`, sin `unsafe` y con cualquier tipo de elemento. Son para autores de bibliotecas: lo normal es que los consumas sin
saberlo a través de `Span<T>`.

```csharp run
using System.Runtime.CompilerServices;

var b = new Bufer();
for (int i = 0; i < 4; i++) b[i] = i * i;
int total = 0;
foreach (int v in b) total += v;
Console.WriteLine(total);  // → 14

[InlineArray(4)]
struct Bufer
{
    private int _elemento0;
}
```

> [!NOTE]
> El README anterior presentaba `int[] array = [1, 2, 3, 4];` como arreglo en línea, pero eso es una expresión de colección. Un *inline array* es
> el atributo `[InlineArray]`, con un solo campo. Requiere .NET 8 o posterior.

---

## 5. C# 12: `Experimental` e interceptores

`[Experimental("DIAG01")]` marca una API de modo que su uso provoca un error del compilador hasta que se suprime ese identificador de forma expresa:

```csharp err=DIAG01
using System.Diagnostics.CodeAnalysis;

Beta.Probar();

public static class Beta
{
    [Experimental("DIAG01")]
    public static void Probar() { }
}
```

**Interceptores.** Permiten a un generador de código sustituir **en compilación** una llamada concreta por otra. Se publicaron de forma experimental en .NET 8. Según la documentación de Roslyn,
tienen **soporte estable desde el SDK 9.0.2xx**.

| Aspecto | Estado actual |
|---|---|
| Propiedad de MSBuild | `<InterceptorsNamespaces>` (la antigua `<InterceptorsPreviewNamespaces>` sigue como alias) |
| Atributo | `[InterceptsLocation(version, data)]`, con datos opacos en base64 que obtiene el generador |
| Público | **Autores de generadores**, no código escrito a mano |
| Uso real | Generadores de ASP.NET Core para API mínimas y del enlace de configuración: eliminan reflexión para Native AOT |

---

<!-- cs=13 -->

## 6. C# 13: colecciones `params`

```csharp run
Console.WriteLine(Sumar(1, 2, 3));   // → 6
Console.WriteLine(Sumar());          // → 0
Console.WriteLine(Contar("a", "b")); // → 2

static int Sumar(params ReadOnlySpan<int> datos)   // sin asignar un arreglo
{
    int total = 0;
    foreach (int d in datos) total += d;
    return total;
}

static int Contar(params IEnumerable<string> datos) => datos.Count();
```

| Tipo admitido | Ejemplo |
|---|---|
| `Span<T>`, `ReadOnlySpan<T>` | Argumentos en la pila cuando el compilador puede |
| Interfaces | `IEnumerable<T>`, `IReadOnlyCollection<T>`, `IReadOnlyList<T>`, `ICollection<T>`, `IList<T>` |
| Tipos concretos | Colecciones con `IEnumerable<T>` y `Add` |

Beneficio inmediato: **recompilar basta**. .NET 9 añadió sobrecargas de `params ReadOnlySpan<T>` en métodos como `string.Join`, y el compilador las
prefiere a las de arreglo.

---

## 7. C# 13: `System.Threading.Lock`

`lock (obj)` sobre un `object` usa `Monitor`. Con un `Lock`, la **misma instrucción** se compila a `EnterScope()`, que devuelve un `ref struct` desechable.

```csharp run
var c = new Caja();
Parallel.For(0, 1000, _ => c.Incrementar());
Console.WriteLine(c.Valor);   // → 1000

class Caja
{
    private readonly Lock _cerrojo = new();
    public int Valor { get; private set; }

    public void Incrementar()
    {
        lock (_cerrojo)   // la migración consiste en cambiar el tipo del campo
        {
            Valor++;
        }
    }
}
```

> [!WARNING]
> Convertir un `Lock` a `object` hace que `lock` vuelva a usar `Monitor`, sin la protección que se buscaba. El compilador lo detecta:

```csharp warn=CS9216
Lock cerrojo = new();
object o = cerrojo;    // advertencia CS9216
lock (o) { }
```

`lock` no admite `await` en su interior, y `Lock` no cambia eso. Para exclusión mutua asíncrona, usa `SemaphoreSlim.WaitAsync`.

---

## 8. C# 13: `ref struct` en todas partes

Tres características que amplían lo que pueden hacer `Span<T>` y compañía:

| Antes | C# 13 |
|---|---|
| Sin `ref` locales en `async` e iteradores | Permitidos, **si no cruzan** un `await` o `yield` |
| `Span<T>` no podía ser argumento genérico | Antirrestricción `where T : allows ref struct` |
| `ref struct` sin interfaces | Pueden implementarlas, pero no convertirse a ellas |

```csharp run
Console.WriteLine(await Medir("hola"));  // → 4
Console.WriteLine(Longitud<Lector>(new Lector(3))); // → 3

static async Task<int> Medir(string s)
{
    await Task.Yield();
    ReadOnlySpan<char> parte = s.AsSpan();   // ref struct en un método async
    return parte.Length;                     // sin cruzar otro await
}

static int Longitud<T>(T v) where T : ILongitud, allows ref struct => v.Largo;

interface ILongitud { int Largo { get; } }

ref struct Lector(int n) : ILongitud
{
    public int Largo => n;
}
```

La biblioteca base ya lo aprovecha: en .NET 10, `IEnumerable<T>` y `Func<,>` declaran esta antirrestricción, de modo que `Func<ReadOnlySpan<char>, int>` compila.

---

## 9. C# 13: prioridad de resolución y propiedades parciales

### `OverloadResolutionPriority`

Un autor de biblioteca añade un método mejor y quiere que el código recompilado lo adopte sin romper binarios antiguos:

```csharp run
using System.Runtime.CompilerServices;

Api.Log("a", "b");  // → span

static class Api
{
    public static void Log(params string[] v) => Console.WriteLine("arreglo");

    [OverloadResolutionPriority(1)]
    public static void Log(params ReadOnlySpan<string> v) => Console.WriteLine("span");
}
```

### Propiedades e indexadores parciales

```csharp run
var c = new Modelo { Nombre = "ok" };
Console.WriteLine(c.Nombre);  // → ok

public partial class Modelo
{
    public partial string Nombre { get; set; }     // declaración: la escribes tú
}

public partial class Modelo
{
    private string _nombre = "";
    public partial string Nombre                   // implementación: la genera una herramienta
    {
        get => _nombre;
        set => _nombre = value;
    }
}
```

Su principal usuario es `[GeneratedRegex]` y los generadores de MVVM, que pueden aplicarse a propiedades.

### Otros cambios

| Característica | Ejemplo |
|---|---|
| Secuencia de escape `\e` | `"\e[1m"`, en lugar del ambiguo `\x1b` |
| Índice desde el final en inicializadores | `new T { Buffer = { [^1] = 0 } }` |
| Tipo natural de grupos de métodos | El compilador descarta candidatos inaplicables por ámbito |
| `field` | En *preview* en esta versión; publicado en [C# 14](12-csharp-14-extensiones.md) |

```csharp run
var t = new Temporizador { Datos = { [^1] = 9, [^2] = 8 } };
Console.WriteLine(t.Datos[2]);   // → 9

class Temporizador { public int[] Datos { get; } = new int[3]; }
```

---

## 10. Notas de migración

| Versión | Tema | Acción |
|---|---|---|
| C# 12 | `ref` pasado a `in` | Puede cambiar la sobrecarga elegida |
| C# 12 | Cambios en `DisposeAsync` | `await using` prefiere el patrón a la interfaz |
| C# 13 | Sobrecargas con expresiones de colección | `M([1])` puede elegir otra sobrecarga por el tipo exacto de elemento |
| C# 13 | `[InlineArray]` en `record struct` | Ya no se permite (CS9259) |
| C# 13 | `unsafe` heredado en iteradores | Marca la función local como `unsafe` |
| Ambas | **.NET 8 y .NET 9 no tienen soporte desde el 10-nov-2026** | Migra a .NET 10 LTS: ver [18](18-migracion-y-ciclo-de-vida.md) |

---

## 11. Referencias

- [What's new in C# 12](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-12) · [What's new in C# 13](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-13)
- [Collection expressions](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/collection-expressions) · [Primary constructors](https://learn.microsoft.com/dotnet/csharp/whats-new/tutorials/primary-constructors)
- [Interceptors](https://github.com/dotnet/roslyn/blob/main/docs/features/interceptors.md) · [lock statement](https://learn.microsoft.com/dotnet/csharp/language-reference/statements/lock)
