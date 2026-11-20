<div align="center">

# 13 · C# 15: uniones y jerarquías cerradas

**El lenguaje cierra por fin el conjunto de casos: `union`, `closed`, `with(...)`, indexadores de extensión y saltos con etiqueta (2026)**

[⬅ 12 · C# 14](12-csharp-14-extensiones.md) · [🏠 Índice](../README.md) · [14 · Hilos evolutivos ➡](14-hilos-evolutivos.md)

</div>

<!-- cs=15 -->

| Ficha | C# 15 🟢 |
|---|---|
| **Estado al 20-nov-2026** | **Estable.** Última versión del lenguaje |
| **Publicación** | 10 de noviembre de 2026, junto con .NET 11 |
| **Visual Studio** | VS 2026 18.8 o posterior |
| **Runtime** | .NET 11 STS (número impar), soporte hasta el **14-nov-2028** |
| **`LangVersion` actual** | `15.0`, por defecto en `net11.0` |
| **Compilador de este capítulo** | SDK `11.0.100` (GA) |
| **Laboratorio** | [CristianSifuentes/CSharp15](https://github.com/CristianSifuentes/CSharp15) |

> [!NOTE]
> **C# 15 es estable desde el 10 de noviembre de 2026**, con la GA de .NET 11. Este capítulo se escribió sobre el RC3 y se volvió a verificar con el SDK `11.0.100`:
> todos los ejemplos dan el mismo resultado. Solo la seguridad de memoria (sección 9) sigue en *preview*.

---

## Contenido

1. [Panorama](#1-panorama)
2. [Uniones](#2-uniones)
3. [Uniones a medida](#3-uniones-a-medida)
4. [Jerarquías cerradas](#4-jerarquías-cerradas)
5. [Argumentos en expresiones de colección](#5-argumentos-en-expresiones-de-colección)
6. [Indexadores de extensión](#6-indexadores-de-extensión)
7. [`break` y `continue` con etiqueta](#7-break-y-continue-con-etiqueta)
8. [Miembros estáticos de interfaz en runtimes antiguos](#8-miembros-estáticos-de-interfaz-en-runtimes-antiguos)
9. [Seguridad de memoria (preview)](#9-seguridad-de-memoria-preview)
10. [Lo que viene](#10-lo-que-viene)
11. [Notas de migración](#11-notas-de-migración)
12. [Referencias](#12-referencias)

---

## 1. Panorama

| Característica | Problema | ¿Requiere `preview`? |
|---|---|---|
| **Uniones** | Sin forma nativa de decir "A o B" con exhaustividad | No |
| **Jerarquías cerradas** | Un `switch` sobre una clase base siempre necesitaba `_` | No |
| **Argumentos de colección** | `[...]` no aceptaba capacidad ni comparador | No |
| **Indexadores de extensión** | Faltaban en C# 14 | No |
| **`break`/`continue` con etiqueta** | Banderas o `goto` en bucles anidados | No |
| **Estáticos en interfaces en runtimes antiguos** | Un método auxiliar exigía soporte de DIM | No |
| **Seguridad de memoria** | `unsafe` marca los punteros, no los accesos peligrosos | **Sí** |

---

## 2. Uniones

### El problema

Durante años, modelar "uno de varios tipos" exigía una interfaz marcador, una clase base abstracta o bibliotecas como `OneOf`. Ninguna garantizaba que el conjunto estuviera cerrado, de modo que todo `switch`
necesitaba un `_` que se tragaba en silencio los casos nuevos.

```csharp cs=14
// Antes: nada impide un tercer implementador, y siempre hace falta el brazo _
interface IResultado { }
record Exito(int Valor) : IResultado;
record Fallo(string Motivo) : IResultado;

static class Uso
{
    static string Texto(IResultado r) => r switch
    {
        Exito e => $"ok {e.Valor}",
        Fallo f => f.Motivo,
        _ => throw new InvalidOperationException(),
    };
}
```

### La solución

```csharp run
Console.WriteLine(Texto(new Exito(5)));       // → ok 5
Resultado r = new Fallo("sin saldo");         // conversión implícita desde cada caso
Console.WriteLine(Texto(r));                  // → sin saldo

static string Texto(Resultado r) => r switch
{
    Exito e => $"ok {e.Valor}",
    Fallo f => f.Motivo,                      // exhaustivo: sin brazo _
};

record Exito(int Valor);
record Fallo(string Motivo);
union Resultado(Exito, Fallo);
```

Los tipos de caso **no necesitan parentesco**: son tipos existentes, incluso de otro ensamblado.

```csharp run
EnteroOTexto a = 5;
EnteroOTexto b = "hola";
Console.WriteLine(a.Value);   // → 5
Console.WriteLine(b.Value);   // → hola

union EnteroOTexto(int, string);
```

### Qué genera el compilador

Comprobado con reflexión sobre la GA de .NET 11:

| Hecho | Detalle |
|---|---|
| Forma | Un `struct` marcado con `[Union]` que implementa `IUnion` |
| Contenido | Un `object?` en la propiedad `Value` |
| Constructores | Uno público por cada tipo de caso |
| Costo | **Un tipo de valor como `int` se encajona** en la unión generada |
| Dependencia | `UnionAttribute` e `IUnion`, incluidos en .NET 11 |
| JSON | `System.Text.Json` serializa el contenido: `{"Valor":5}`, no un envoltorio |

### Cuerpo de una unión

Admite miembros, como un `struct`, pero **sin campos de instancia, propiedades automáticas ni eventos de estilo campo**.

```csharp run
Longitud a = new Metros(1);
Console.WriteLine(a.Sumar(new Pies(10)).EnMetros);  // → 4.048

record Metros(double Valor);
record Pies(double Valor);

union Longitud(Metros, Pies)
{
    public double EnMetros => this switch
    {
        Metros m => m.Valor,
        Pies p => p.Valor * 0.3048,
        _ => throw new InvalidOperationException("Sin valor"),
    };

    public Longitud Sumar(Longitud otra) => new Metros(EnMetros + otra.EnMetros);
}
```

### Patrones: contenido o unión

| Patrón | Se aplica a |
|---|---|
| Tipo o declaración: `Perro`, `Perro p` | **Contenido** |
| Propiedad o posicional **con tipo**: `Perro { Raza: "x" }` | **Contenido** |
| Propiedad o posicional **sin tipo**: `{ Nombre: "Rex" }` | **La unión** |
| Lista | **La unión** |
| `var`, `_` | **La unión**, sin desenvolver |
| Constante, relacional, `null` | **Contenido** |

### La trampa de `default`

Una unión generada es un `struct`, y su valor por defecto contiene `null`. Si todos los casos son no anulables, el compilador **no necesariamente avisa**:

```csharp run
Mascota[] jaula = new Mascota[1];   // cada elemento es default

try
{
    Console.WriteLine(Nombre(jaula[0]));
}
catch (System.Runtime.CompilerServices.SwitchExpressionException)
{
    Console.WriteLine("Sin brazo para null"); // → Sin brazo para null
}

Console.WriteLine(NombreSeguro(jaula[0]));  // → vacía

static string Nombre(Mascota m) => m switch { Perro p => p.N, Gato g => g.N };

static string NombreSeguro(Mascota m) => m switch
{
    Perro p => p.N,
    Gato g => g.N,
    null => "vacía",       // prefiere null a _: conserva la exhaustividad
};

record Perro(string N);
record Gato(string N);
union Mascota(Perro, Gato);
```

> [!TIP]
> **Maneja `null` si `default` puede llegar a tu `switch`** (campos sin inicializar, elementos de arreglo). Y usa `null`, no `_`: un brazo `_` también
> aceptaría un caso añadido más tarde, y perderías el aviso del compilador.

---

## 3. Uniones a medida

La forma generada es deliberadamente simple: siempre `struct`, siempre `object?`. Para evitar el *boxing* o para adaptar un tipo existente, se puede implementar el **patrón de unión**:

```csharp run
using System.Runtime.CompilerServices;

EnteroOBool v = new EnteroOBool((int?)42);
Console.WriteLine(v switch
{
    int i => $"entero {i}",   // usa TryGetValue: sin boxing
    bool b => $"bool {b}",
}); // → entero 42

[Union]
public struct EnteroOBool : IUnion
{
    private readonly int _entero;
    private readonly bool _logico;
    private readonly byte _etiqueta;   // 0 = vacío, 1 = int, 2 = bool

    public EnteroOBool(int? valor)
    {
        if (valor.HasValue) { _entero = valor.Value; _etiqueta = 1; }
    }

    public EnteroOBool(bool? valor)
    {
        if (valor.HasValue) { _logico = valor.Value; _etiqueta = 2; }
    }

    public object? Value => _etiqueta switch { 1 => _entero, 2 => _logico, _ => null };

    public bool HasValue => _etiqueta != 0;

    public bool TryGetValue(out int valor) { valor = _entero; return _etiqueta == 1; }
    public bool TryGetValue(out bool valor) { valor = _logico; return _etiqueta == 2; }
}
```

| Requisito del patrón | Detalle |
|---|---|
| `[Union]` | Atributo en el tipo |
| Constructores públicos | Un parámetro por valor o `in`: su tipo **define** un caso |
| `Value` | Propiedad pública `object?` u `object` |
| Opcional: `TryGetValue` | Evita el *boxing* en patrones de tipo |
| Opcional: `HasValue` | Optimiza el patrón `null` |
| Opcional: `IUnionMembers` | Interfaz anidada con métodos `Create`, para constructores privados |

El compilador **confía** en que el tipo cumple cuatro reglas: solidez, estabilidad, equivalencia de creación y coherencia de acceso. También son posibles las uniones basadas en `class`.

---

## 4. Jerarquías cerradas

`closed` limita la derivación **directa** al ensamblado que declara la clase. Así, el compilador conoce todos los descendientes directos.

```csharp run
Console.WriteLine(Describir(new Abierta(50)));  // → abierta al 50%
Console.WriteLine(Describir(new Cerrada()));    // → cerrada

static string Describir(Compuerta c) => c switch
{
    Cerrada => "cerrada",
    Abierta(var p) => $"abierta al {p}%",       // sin brazo por defecto
};

public closed record class Compuerta;
public record class Cerrada : Compuerta;
public record class Abierta(float Porcentaje) : Compuerta;
```

| Regla | Detalle |
|---|---|
| Implícito | Una clase `closed` es **implícitamente `abstract`** |
| Incompatible | No se combina con `sealed`, `static` ni un `abstract` explícito |
| **No transitivo** | Un hijo no cerrado puede derivarse desde otros ensamblados |
| Metadatos | El compilador emite `IsClosedTypeAttribute` |
| Palabra clave | Contextual; un tipo llamado `closed` necesita `@` |

### ¿`union` o `closed`?

| | `union` | `closed` |
|---|---|---|
| Parentesco entre casos | No es necesario | Herencia de una base |
| Casos ajenos (`int`, `string`, tipos de terceros) | ✅ | ❌ |
| Comportamiento común | En el cuerpo, con `switch` | En la clase base, con virtuales |
| Representación | `struct` con `object?` | Referencia normal |
| Casos en otro ensamblado | ✅ | ❌ |

Árbol de decisión completo en [16 § Modelar datos](16-guias-de-decision.md#1-modelar-datos).

---

## 5. Argumentos en expresiones de colección

`with(...)` debe ser el **primer** elemento, y pasa argumentos al constructor o método de fábrica de la colección.

```csharp run
string[] valores = ["uno", "dos"];

List<string> nombres = [with(capacity: valores.Length * 2), .. valores];
Console.WriteLine(nombres.Capacity);  // → 4

HashSet<string> conjunto = [with(StringComparer.OrdinalIgnoreCase), "Hola", "HOLA"];
Console.WriteLine(conjunto.Count);    // → 1
```

Es la pieza que faltaba para usar `[...]` con diccionarios ordenados, comparadores personalizados y capacidad precalculada. Si tenías un método llamado `with`, usa `@with`.

---

## 6. Indexadores de extensión

Completa los [miembros de extensión de C# 14](12-csharp-14-extensiones.md#1-miembros-de-extensión). Como un indexador siempre es de instancia, el receptor debe tener nombre.

```csharp run
IEnumerable<int> numeros = Enumerable.Range(1, 10);
Console.WriteLine(numeros[2]);  // → 3

public static class Secuencias
{
    extension(IEnumerable<int> origen)
    {
        public int this[int indice] => origen.ElementAt(indice);
    }
}
```

---

## 7. `break` y `continue` con etiqueta

```csharp cs=14
// Antes: bandera manual o goto
int[,] rejilla = { { 1, 0 }, { 2, 3 } };
bool hallado = false;
for (int f = 0; f < 2 && !hallado; f++)
{
    for (int c = 0; c < 2; c++)
    {
        if (rejilla[f, c] == 0) { hallado = true; break; }
    }
}
```

```csharp run
int[,] rejilla = { { 1, 0 }, { 2, 3 } };

externo: for (int f = 0; f < 2; f++)
{
    for (int c = 0; c < 2; c++)
    {
        if (rejilla[f, c] == 0) continue externo;   // pasa a la siguiente fila
        if (rejilla[f, c] == 3) break externo;      // abandona ambos bucles
        Console.WriteLine(rejilla[f, c]);
        // → 1
        // → 2
    }
}
```

La etiqueta debe ir **directamente** sobre el bucle o `switch`. La regla de estilo IDE0410 señala las banderas y los `goto` que se pueden sustituir.

---

## 8. Miembros estáticos de interfaz en runtimes antiguos

Los miembros estáticos **no virtuales** de una interfaz ya no exigen un runtime con soporte de implementaciones por defecto: útil para bibliotecas que apuntan a .NET Standard 2.0 o .NET Framework. Las
implementaciones de instancia, `static abstract`, `static virtual` y los miembros `protected` siguen necesitándolo.

```csharp run
Console.WriteLine(IMedida.AMetros(250));  // → 2.5

public interface IMedida
{
    public static double AMetros(double centimetros) => centimetros / 100;
}
```

---

## 9. Seguridad de memoria (preview)

El SDK de .NET 11 incluye el inicio de un esfuerzo que durará varias versiones: **ligar el contexto `unsafe` a las operaciones que acceden a memoria**, no a la existencia de un puntero.

```csharp preview run
int numero = 42;
int* puntero = &numero;                  // ya no requiere unsafe
int valor = unsafe(*puntero);            // la desreferencia sí: expresión unsafe
Console.WriteLine(valor);                // → 42
```

| Sin `unsafe` en *preview* | Sigue requiriendo `unsafe` |
|---|---|
| Declarar punteros y usar `&` | Indirección `*p` |
| `fixed` | `p->miembro` |
| `stackalloc` convertido a puntero | `p[i]` |
| `sizeof` de tipos no administrados | Invocar punteros a función |

```xml
<PropertyGroup>
  <LangVersion>preview</LangVersion>
  <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
  <!-- Reglas completas: "requires-unsafe" se propaga a quien llama -->
  <Features>$(Features);updated-memory-safety-rules</Features>
</PropertyGroup>
```

> [!CAUTION]
> **No lo uses en producción.** El modelo y su sintaxis pueden cambiar. La lista de cambios incompatibles de Roslyn ya sitúa varias de estas reglas en **C# 16**, por ejemplo la palabra clave `safe` y las nuevas ambigüedades
> en la resolución de sobrecargas.

---

## 10. Lo que viene

Según *Language Feature Status* de Roslyn. **Trabajo en curso, sin versión ni garantía de publicación:**

| Propuesta | Idea |
|---|---|
| Expresiones de diccionario | Sintaxis `[clave: valor]` |
| `await` condicional nulo | `await?` |
| Comparaciones relacionales encadenadas | Comparar en cadena, como `a < b < c` |
| Acceso a miembros estáticos con tipo de destino | Omitir el nombre del tipo cuando se infiere |
| Modificadores en cualquier orden | Reglas más flexibles |
| Asignación compuesta en inicializadores | `new T { Lista += … }` |
| Extensiones en receptores sin tipo y constantes de extensión | Más lugares para `extension` |
| Inferencia desde restricciones | Inferir parámetros de tipo a partir de `where` |
| Runtime Async Streams | Flujos asíncronos implementados por el runtime |

---

## 11. Notas de migración

| Cambio | Síntoma | Solución |
|---|---|---|
| `closed` contextual | CS9380 o CS1519 con tipos llamados `closed` | `@closed` |
| `union` contextual | CS9370 | `@union` |
| `with(...)` en colección | Se interpreta como argumentos, no como un método `with` | `@with(...)` |
| Colecciones `Span` en bucles | `safe-context` pasa a ser *declaration-block* | Usa un arreglo o sube la expresión de ámbito |
| `(X.Y) when` en `switch` | Ahora es patrón constante con `when` | Generalmente corrige código |
| `ref readonly` en delegados sintetizados y funciones locales | CS0518 si falta `InAttribute` | Añade la referencia |
| `&&`/`||` dinámicos con interfaz a la izquierda | CS7083 | Usa un tipo concreto o `dynamic` |
| `nameof(this.P)` en atributos | Ya no compila | `nameof(P)` |

### ¿Debo adoptar C# 15?

| Escenario | Recomendación |
|---|---|
| Producción con LTS | Permanece en **.NET 10 / C# 14** hasta 2028 si valoras el soporte a largo plazo |
| Biblioteca | Solo si apunta a .NET 11 o posterior: C# 15 no está soportado en runtimes anteriores |
| Producto que se actualiza con frecuencia | Adóptalo ya; planifica el salto a .NET 12 LTS antes del 14-nov-2028 |
| Aprendizaje | Hoy mismo, con el SDK `11.0.100` |

---

## 12. Referencias

- [What's new in C# 15](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-15) · [Union types](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/union)
- [Breaking changes since C# 14](https://learn.microsoft.com/dotnet/csharp/whats-new/breaking-changes/compiler%20breaking%20changes%20-%20dotnet%2011) · [What's new in .NET 11](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-11/overview)
- [Language Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md) · [.NET support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core)
