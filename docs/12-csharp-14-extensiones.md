<div align="center">

# 12 · C# 14: miembros de extensión

**Bloques `extension`, palabra clave `field`, conversiones de `Span`, asignación condicional y operadores compuestos (2025)**

[⬅ 11 · C# 12 y 13](11-csharp-12-13-colecciones.md) · [🏠 Índice](../README.md) · [13 · C# 15 ➡](13-csharp-15-uniones.md)

</div>

<!-- cs=14 -->

| Ficha | C# 14 🟢 |
|---|---|
| **Publicación** | noviembre de 2025 |
| **Visual Studio** | VS 2026 18.0 |
| **Runtime** | .NET 10 LTS (11-nov-2025), soporte hasta el **14-nov-2028** |
| **`LangVersion` actual** | `14.0` |
| **Estado** | **Última versión estable** a noviembre de 2026 |
| **Laboratorio** | [CristianSifuentes/CSharp14](https://github.com/CristianSifuentes/CSharp14) |

> [!CAUTION]
> **Cuidado con lo que circula.** Versiones antiguas de este README, corregidas en mayo de 2026, atribuían a C# 14 "partial parameters", un operador
> `filter`, "async main pipelines" e "interface type forwarding". **Ninguna de esas características existe.** Este capítulo se basa en la documentación
> oficial y sus ejemplos se han compilado.

---

## Contenido

1. [Miembros de extensión](#1-miembros-de-extensión)
2. [La palabra clave `field`](#2-la-palabra-clave-field)
3. [Conversiones implícitas de `Span`](#3-conversiones-implícitas-de-span)
4. [Asignación condicional nula](#4-asignación-condicional-nula)
5. [Operadores de asignación compuesta](#5-operadores-de-asignación-compuesta)
6. [Modificadores en lambdas simples](#6-modificadores-en-lambdas-simples)
7. [`nameof` con genéricos sin enlazar](#7-nameof-con-genéricos-sin-enlazar)
8. [Constructores y eventos parciales](#8-constructores-y-eventos-parciales)
9. [Aplicaciones de un solo archivo](#9-aplicaciones-de-un-solo-archivo)
10. [Notas de migración](#10-notas-de-migración)
11. [Referencias](#11-referencias)

---

## 1. Miembros de extensión

### El problema

Desde [C# 3](04-csharp-3-linq.md#4-métodos-de-extensión) solo se podían extender **métodos de instancia**. No había propiedades de extensión, ni miembros estáticos, ni operadores. Cada método repetía además el
receptor y sus parámetros de tipo.

### La solución

```csharp cs=13
// Antes: solo métodos, con el receptor repetido en cada uno
public static class Texto
{
    public static bool EstaVacio(this string s) => s.Length == 0;
    public static string Primero(this string s) => s[..1];
}
```

```csharp run
int[] datos = [1, 2, 3];
Console.WriteLine(datos.EstaVacia);                 // → False
Console.WriteLine(datos.Segundo());                 // → 2

var todos = IEnumerable<int>.Combinar([1], [2]);    // miembro estático de extensión
Console.WriteLine(todos.Count());                   // → 2

var suma = new[] { 1 }.AsEnumerable() + [2, 3];     // operador de extensión
Console.WriteLine(suma.Count());                    // → 3

public static class Secuencias
{
    extension<T>(IEnumerable<T> origen)             // receptor con nombre: miembros de instancia
    {
        public bool EstaVacia => !origen.Any();
        public T Segundo() => origen.Skip(1).First();
    }

    extension<T>(IEnumerable<T>)                    // sin nombre: miembros estáticos
    {
        public static IEnumerable<T> Combinar(IEnumerable<T> a, IEnumerable<T> b) => a.Concat(b);
        public static IEnumerable<T> operator +(IEnumerable<T> a, IEnumerable<T> b) => a.Concat(b);
    }
}
```

| Miembro | ¿Permitido en C# 14? |
|---|---|
| Métodos de instancia y estáticos | ✅ |
| Propiedades de instancia y estáticas | ✅ |
| Operadores definidos por el usuario | ✅ |
| Indexadores | ❌ Llegan en [C# 15](13-csharp-15-uniones.md#6-indexadores-de-extensión) |
| Campos, eventos, constructores | ❌ |

### Compatibilidad

| Pregunta | Respuesta |
|---|---|
| ¿Debo reescribir mis extensiones? | No: la sintaxis `this` sigue siendo válida y las dos pueden convivir |
| ¿Se pierde la llamada estática? | No: `Secuencias.Segundo(datos)` sigue funcionando |
| ¿Tienen acceso a privados? | No, igual que antes |
| ¿Prioridad frente a miembros reales? | Los miembros del tipo siempre ganan |

```csharp run
int[] datos = [5, 6];
Console.WriteLine(Secuencias.Segundo(datos)); // → 6

public static class Secuencias
{
    extension<T>(IEnumerable<T> origen)
    {
        public T Segundo() => origen.Skip(1).First();
    }
}
```

> [!IMPORTANT]
> `extension` es ahora una palabra clave contextual. Un tipo, alias o parámetro de tipo no puede llamarse `extension` sin `@`.

---

## 2. La palabra clave `field`

Una propiedad automática con un poco de lógica obligaba a escribir el campo a mano. `field` da acceso al campo sintetizado.

```csharp cs=13
// Antes
class Cliente
{
    private string _nombre = "";
    public string Nombre
    {
        get => _nombre;
        set => _nombre = value ?? throw new ArgumentNullException(nameof(value));
    }
}
```

```csharp run
var c = new Cliente { Correo = "  ANA@EJEMPLO.COM " };
Console.WriteLine(c.Correo);       // → ana@ejemplo.com
Console.WriteLine(c.Etiqueta);     // → sin etiqueta

class Cliente
{
    public string Correo
    {
        get;
        set => field = value.Trim().ToLowerInvariant();  // un accesor automático y otro con cuerpo
    } = "";

    public string Etiqueta => field ??= "sin etiqueta";   // inicialización diferida
}
```

La trampa es el nombre: si el tipo ya tiene un miembro llamado `field`, dentro de un accesor la palabra pasa a referirse al campo sintetizado (advertencia CS9258).
Usa `this.field` o `@field`.

```csharp warn=CS9258
class Registro
{
    private int field = 0;
    public int Valor => field;   // advertencia CS9258
}
```

---

## 3. Conversiones implícitas de `Span`

C# 14 da **soporte de primera clase** a `Span<T>` y `ReadOnlySpan<T>`: nuevas conversiones implícitas entre `T[]`, `Span<T>` y `ReadOnlySpan<T>`, que también aplican a los receptores
de extensión y a la inferencia de tipos.

```csharp run
int[] datos = [1, 2, 3];
Console.WriteLine(datos.Total());                   // → 6

static class Extensiones
{
    public static int Total(this ReadOnlySpan<int> s)  // antes hacía falta .AsSpan()
    {
        int t = 0;
        foreach (int v in s) t += v;
        return t;
    }
}
```

> [!WARNING]
> **Es el cambio incompatible más delicado de C# 14.** Las sobrecargas de `Span` entran en juego en más casos, y un arreglo covariante puede lanzar excepción donde antes no lo hacía.

El mismo programa con dos versiones del lenguaje:

```csharp cs=13 run
string[] textos = ["a"];
object[] objetos = textos;   // covarianza de arreglos

C.R(objetos);                // → IEnumerable

static class C
{
    public static void R<T>(IEnumerable<T> e) => Console.WriteLine("IEnumerable");
    public static void R<T>(Span<T> s) => Console.WriteLine("Span");
}
```

```csharp cs=14 run
string[] textos = ["a"];
object[] objetos = textos;

try
{
    C.R(objetos);            // ahora elige Span<T>, cuyo constructor rechaza el arreglo covariante
}
catch (ArrayTypeMismatchException)
{
    Console.WriteLine("ArrayTypeMismatchException");  // → ArrayTypeMismatchException
}

static class C
{
    public static void R<T>(IEnumerable<T> e) => Console.WriteLine("IEnumerable");
    public static void R<T>(Span<T> s) => Console.WriteLine("Span");
}
```

---

## 4. Asignación condicional nula

```csharp run
Pedido? p = null;
p?.Total = Calcular();              // Calcular no se ejecuta
p = new Pedido();
p?.Total = Calcular();              // → calculado
p?.Total += 1;
Console.WriteLine(p?.Total);        // → 11

int Calcular()
{
    Console.WriteLine("calculado");
    return 10;
}

class Pedido { public int Total { get; set; } }
```

| Regla | Detalle |
|---|---|
| Lado derecho | Se evalúa **solo** si el izquierdo no es nulo |
| Operadores | `=`, `+=`, `-=` y demás compuestos |
| No permitidos | `++` y `--` |

---

## 5. Operadores de asignación compuesta

Antes, `x += y` siempre era `x = x + y`, de modo que un tipo grande debía **asignar un resultado nuevo**. Ahora un tipo puede modificarse en su lugar.

```csharp run
var a = new Acumulador();
var referencia = a;
a += 5;              // llama a operator +=, sin crear otro objeto
a++;                 // llama al operator ++ de instancia
Console.WriteLine(a.Total);                     // → 6
Console.WriteLine(ReferenceEquals(a, referencia)); // → True

class Acumulador
{
    public int Total;
    public void operator +=(int x) => Total += x;   // instancia, void, sin static
    public void operator ++() => Total++;
}
```

| Regla | Detalle |
|---|---|
| Forma | Método de instancia, retorno `void`, un parámetro (ninguno para `++` y `--`) |
| Prioridad | Si `x` es una variable, se prefiere `+=`; si no existe, se recurre a `+` |
| Coherencia | El compilador **no** comprueba que `+` y `+=` concuerden |
| `checked` | `operator checked +=` requiere su par normal |
| Uso previsto | Tipos con datos grandes: `BigInteger`, tensores |

---

## 6. Modificadores en lambdas simples

```csharp run
TryParse<int> leer = (texto, out resultado) => int.TryParse(texto, out resultado);
Console.WriteLine(leer("42", out int n) ? n : -1);  // → 42

delegate bool TryParse<T>(string texto, out T resultado);
```

Antes, usar `out`, `ref`, `in`, `scoped` o `ref readonly` obligaba a declarar **todos** los tipos. `params` todavía los exige.

---

## 7. `nameof` con genéricos sin enlazar

```csharp run
Console.WriteLine(nameof(List<>));          // → List
Console.WriteLine(nameof(Dictionary<,>));   // → Dictionary
```

---

## 8. Constructores y eventos parciales

Completan la serie iniciada en [C# 13](11-csharp-12-13-colecciones.md#9-c-13-prioridad-de-resolución-y-propiedades-parciales), pensada para los generadores de código.

```csharp run
var m = new Modelo(3);
m.Cambio += () => Console.WriteLine("evento");  // → evento
Console.WriteLine(m.Valor);                     // → 3

public partial class Modelo
{
    public int Valor { get; }
    public partial Modelo(int valor);            // declaración
    public partial event Action Cambio;
}

public partial class Modelo
{
    public partial Modelo(int valor) => Valor = valor;   // implementación

    public partial event Action Cambio
    {
        add => value();
        remove { }
    }
}
```

| Regla | Detalle |
|---|---|
| Declaraciones | Exactamente una de definición y una de implementación |
| `this()` y `base()` | Solo en la implementación de un constructor |
| Constructor primario | Solo en una de las declaraciones parciales del tipo |
| Evento | La definición es de estilo campo; la implementación tiene `add` y `remove` |

---

## 9. Aplicaciones de un solo archivo

Es una función del **SDK de .NET 10**, con directivas de preprocesador reconocidas por el lenguaje. Un archivo `.cs` se compila y ejecuta sin `.csproj`.

```csharp omitir
#!/usr/bin/env -S dotnet --
#:package Spectre.Console@*
#:property PublishAot=false

using Spectre.Console;

AnsiConsole.MarkupLine("[green]Hola desde un solo archivo[/]");
```

| Directiva | Función |
|---|---|
| `#:package Nombre@Versión` | Referencia NuGet |
| `#:project ruta` | Referencia a otro proyecto |
| `#:property Clave=Valor` | Propiedad de MSBuild |
| `#:sdk Nombre` | SDK alternativo, como `Microsoft.NET.Sdk.Web` |
| `#:include ruta` | Más archivos (SDK 10.0.300 y .NET 11) |

| Comando | Efecto |
|---|---|
| `dotnet run app.cs` o `dotnet app.cs` | Compila y ejecuta |
| `dotnet publish app.cs` | Publica con **Native AOT por defecto** |
| `dotnet project convert app.cs` | Genera un proyecto normal |

> [!WARNING]
> Dentro de una carpeta con un `.csproj`, `dotnet run app.cs` ejecuta **ese proyecto** y pasa `app.cs` como argumento. Usa `--file` o mantén los
> scripts fuera del árbol del proyecto.

---

## 10. Notas de migración

| Tema | Síntoma | Acción |
|---|---|---|
| Sobrecargas de `Span` | Ambigüedad o `ArrayTypeMismatchException` | `.AsSpan()` o `.AsEnumerable()` explícitos |
| `field` | CS9258, CS9272 | `@field` o renombrar |
| `extension` | No se puede usar como nombre de tipo | `@extension` |
| `partial` como tipo de retorno | Error de sintaxis | `@partial` |
| `scoped` en lambdas | Siempre es modificador | `@scoped` en la posición del tipo |
| `Enumerable.Reverse` con C# 14 en TFM antiguos | Se invierte **en el sitio** y devuelve `void` | No uses C# 14 en un TFM anterior a `net10.0` |
| Propiedades parciales de interfaz | Ahora son implícitamente `virtual` y `public` | Añade `private` o `sealed` para conservar lo anterior |

Lista completa en [18 § C# 14](18-migracion-y-ciclo-de-vida.md#c-14).

---

## 11. Referencias

- [What's new in C# 14](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14) · [Breaking changes since C# 13](https://learn.microsoft.com/dotnet/csharp/whats-new/breaking-changes/compiler%20breaking%20changes%20-%20dotnet%2010)
- [Extension members](https://learn.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/extension-methods) · [field keyword](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/field)
- [First-class span types](https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-14.0/first-class-span-types) · [User-defined compound assignment](https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-14.0/user-defined-compound-assignment)
- [File-based apps](https://learn.microsoft.com/dotnet/core/sdk/file-based-apps)
