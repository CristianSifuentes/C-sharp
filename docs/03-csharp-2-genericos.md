<div align="center">

# 03 · C# 2.0: los genéricos

**Genéricos reificados, iteradores, tipos anulables, métodos anónimos y tipos parciales (2005)**

[⬅ 02 · C# 1](02-csharp-1-cimientos.md) · [🏠 Índice](../README.md) · [04 · C# 3 ➡](04-csharp-3-linq.md)

</div>

<!-- cs=2 -->

| Ficha | C# 2.0 |
|---|---|
| **Publicación** | noviembre de 2005 |
| **Visual Studio** | VS 2005 |
| **Runtime** | .NET Framework 2.0 (CLR 2.0) |
| **`LangVersion` actual** | `2` o `ISO-2` |
| **Clase de cambio** | **Runtime**: el CLR se modificó para soportar genéricos |
| **Laboratorio** | [CristianSifuentes/CSharp2](https://github.com/CristianSifuentes/CSharp2) |

---

## Contenido

1. [Panorama](#1-panorama)
2. [Genéricos](#2-genéricos)
3. [Iteradores con `yield`](#3-iteradores-con-yield)
4. [Tipos de valor anulables](#4-tipos-de-valor-anulables)
5. [Métodos anónimos y clausuras](#5-métodos-anónimos-y-clausuras)
6. [Tipos parciales y clases estáticas](#6-tipos-parciales-y-clases-estáticas)
7. [Mejoras menores](#7-mejoras-menores)
8. [Notas de migración](#8-notas-de-migración)
9. [Referencias](#9-referencias)

---

## 1. Panorama

| Característica | Problema que resolvía | Sigue viva en |
|---|---|---|
| **Genéricos** | *Boxing* y conversiones de `ArrayList` | Todo el lenguaje |
| **Iteradores** | Clases enumeradoras escritas a mano | LINQ, `IAsyncEnumerable<T>` |
| **`Nullable<T>`** | Valores centinela (`-1`, `DateTime.MinValue`) | `T?` |
| **Métodos anónimos** | Un método con nombre por cada *callback* | Lambdas |
| **Tipos parciales** | Código de diseñador mezclado con el código humano | Generadores de código |

---

## 2. Genéricos

### 2.1 Antes y después

```csharp cs=1 cuerpo
// Antes (C# 1): sin seguridad de tipos y con boxing por elemento
System.Collections.ArrayList numeros = new System.Collections.ArrayList();
numeros.Add(1);
numeros.Add("dos");            // compila
int primero = (int)numeros[0]; // conversión manual
```

```csharp err=CS1503 cuerpo
// Después (C# 2): el error aparece al compilar
List<int> numeros = new List<int>();
numeros.Add(1);
numeros.Add("dos");            // error CS1503
```

### 2.2 Reificación: la decisión que separó a C# de Java

Java 5 (2004) implementó los genéricos con **borrado de tipos**: el parámetro desaparece tras compilar. El CLR los
**reifica**: `List<int>` existe de verdad en ejecución.

| Aspecto | CLR (C#) | JVM (Java) |
|---|---|---|
| `typeof(T)` en ejecución | ✅ | ❌ |
| Tipos de valor como argumento | ✅ sin *boxing* | ❌ requiere envoltorios (`Integer`) |
| `new T[n]` | ✅ | ❌ |
| Código generado | Especializado para cada tipo de valor; compartido para tipos de referencia | Uno solo |

```csharp run
class Programa
{
    static string Nombre<T>() { return typeof(T).Name; }

    static void Main()
    {
        Console.WriteLine(Nombre<int>());                 // → Int32
        Console.WriteLine(new List<int>().GetType() == new List<string>().GetType()); // → False
    }
}
```

> [!NOTE]
> **Rendimiento.** El JIT produce código nativo distinto para `List<int>` y `List<double>`, y comparte una versión para todos
> los tipos de referencia, porque un puntero mide lo mismo. Esa especialización es lo que hace posible la
> [matemática genérica de C# 11](10-csharp-11-matematica-generica.md) sin costo de abstracción.

### 2.3 Restricciones

```csharp run
interface IEntidad { int Id { get; } }

class Cliente : IEntidad
{
    private int _id = 7;
    public int Id { get { return _id; } }
}

class Repositorio<T> where T : class, IEntidad, new()
{
    private Dictionary<int, T> _datos = new Dictionary<int, T>();

    public T Crear()
    {
        T nuevo = new T();            // posible gracias a new()
        _datos[nuevo.Id] = nuevo;     // posible gracias a IEntidad
        return nuevo;
    }

    public T Buscar(int id)
    {
        T hallado;
        return _datos.TryGetValue(id, out hallado) ? hallado : default(T);
    }
}

class Programa
{
    static void Main()
    {
        Repositorio<Cliente> r = new Repositorio<Cliente>();
        Console.WriteLine(r.Crear().Id);          // → 7
        Console.WriteLine(r.Buscar(99) == null);  // → True
    }
}
```

| Restricción | Significado | Desde |
|---|---|---|
| `where T : struct` / `class` | Tipo de valor / de referencia | C# 2 |
| `where T : new()` | Constructor público sin parámetros | C# 2 |
| `where T : Base`, `IInterfaz`, `U` | Herencia, interfaz, otro parámetro | C# 2 |
| `where T : unmanaged`, `Enum`, `Delegate` | Sin referencias; enumeración; delegado | C# 7.3 |
| `where T : notnull` | No anulable | C# 8 |
| `where T : INumber<T>` | Operadores genéricos | C# 11 |
| `where T : allows ref struct` | Antirrestricción | C# 13 |

Lo que **no** se podía hacer: `a + b` con `T`. Los operadores son estáticos, y las interfaces de 2005 no los expresaban.
Hubo que esperar 17 años, hasta `static abstract` en C# 11.

---

## 3. Iteradores con `yield`

### Antes y después

Un enumerador manual exigía una clase con `Current`, `MoveNext`, `Reset` y estado explícito. Con `yield`, el compilador genera
esa clase, una **máquina de estados**:

```csharp run
class Programa
{
    static IEnumerable<int> Pares(int limite)
    {
        Console.WriteLine("inicio");     // no se ejecuta hasta el primer MoveNext
        for (int i = 0; i <= limite; i += 2)
        {
            yield return i;
        }
    }

    static void Main()
    {
        IEnumerable<int> secuencia = Pares(4);
        Console.WriteLine("creada");     // → creada
        foreach (int n in secuencia)     // → inicio
        {
            Console.WriteLine(n);
            // → 0
            // → 2
            // → 4
        }
    }
}
```

| Semántica | Detalle |
|---|---|
| **Ejecución diferida** | Nada corre hasta iterar: la validación de argumentos también se difiere |
| **Estado** | Variables locales convertidas en campos de la clase generada |
| **`finally`** | Se ejecuta al terminar o al llamar a `Dispose`, gracias a [C# 1.2](02-csharp-1-cimientos.md#8-c-12-dispose-en-foreach) |
| **Límites** | Sin `yield` dentro de `try` con `catch` |

> [!TIP]
> Si necesitas validar argumentos de inmediato, separa un método público que valide y un iterador privado. Desde
> [C# 7](07-csharp-7-tuplas-patrones-y-ref.md) el iterador puede ser una función local.

Esta máquina de estados es la **antepasada directa de `async`/`await`**: en C# 5 el compilador reutilizó la idea.

---

## 4. Tipos de valor anulables

`int?` es azúcar para `System.Nullable<int>`, un `struct` con `HasValue` y `Value`. El CLR tiene un caso especial al convertirlo a
`object`:

```csharp run
class Programa
{
    static void Main()
    {
        int? nada = null;
        object caja = nada;                  // boxing especial: produce null
        Console.WriteLine(caja == null);     // → True

        int? a = 5;
        int? b = null;
        Console.WriteLine((a + b).HasValue); // → False
        Console.WriteLine(a > b);            // → False
        Console.WriteLine(a < b);            // → False
        Console.WriteLine(b ?? 10);          // → 10
    }
}
```

**Operadores elevados** (*lifted*): si un operando es `null`, la aritmética da `null` y la comparación da `false`.
Por eso `a > b` y `a < b` son ambos falsos, y `!(a > b)` **no** equivale a `a <= b`.

El operador `??` también nace aquí. Esta línea de nulabilidad continúa en [C# 6](06-csharp-6-roslyn.md) con `?.`, en
[C# 8](08-csharp-8-nulabilidad-y-streams.md) con `??=` y los tipos de referencia anulables, y en
[C# 14](12-csharp-14-extensiones.md) con la asignación condicional.

---

## 5. Métodos anónimos y clausuras

```csharp run
class Programa
{
    static void Main()
    {
        List<int> numeros = new List<int>(new int[] { 1, 2, 3, 4 });
        int umbral = 2;

        List<int> mayores = numeros.FindAll(delegate(int n) { return n > umbral; });
        Console.WriteLine(mayores.Count); // → 2

        umbral = 3;  // la clausura captura la VARIABLE, no su valor
        Console.WriteLine(numeros.FindAll(delegate(int n) { return n > umbral; }).Count); // → 1
    }
}
```

Para capturar variables, el compilador las eleva a campos de una clase generada. Dos consecuencias duran hasta hoy: **una
asignación adicional** por cada ámbito capturado y **extensión del tiempo de vida** de los objetos capturados.

---

## 6. Tipos parciales y clases estáticas

`partial` nació para separar el código del diseñador de Windows Forms. Veinte años después es la base de los generadores de código:
los métodos parciales llegaron en C# 3 y se ampliaron en C# 9; las propiedades en C# 13; constructores y eventos en C# 14.

```csharp run
partial class Pedido { public int Id = 1; }
partial class Pedido { public string Etiqueta() { return "P-" + Id; } }

static class Formatos                 // sin instancias ni herencia
{
    public const string Prefijo = "#";
}

class Programa
{
    static void Main()
    {
        Console.WriteLine(new Pedido().Etiqueta()); // → P-1
        Console.WriteLine(Formatos.Prefijo);        // → #
    }
}
```

---

## 7. Mejoras menores

| Característica | Ejemplo |
|---|---|
| Accesibilidad distinta por accesor | `public int X { get { … } private set { … } }` |
| Conversión de grupos de métodos | `boton.Click += Manejador;` en lugar de `new EventHandler(Manejador)` |
| Covarianza y contravarianza de **delegados** | Asignar un método que devuelve un tipo más derivado |
| Calificador de alias `::` | `global::System.Console` |
| `extern alias` | Dos versiones de un ensamblado en un mismo proyecto |
| `#pragma warning` | Desactivar advertencias concretas |

---

## 8. Notas de migración

| Tema | Recomendación |
|---|---|
| `ArrayList`, `Hashtable` | Reemplazar por `List<T>`, `Dictionary<TKey, TValue>` |
| API públicas no genéricas | Conservar por compatibilidad y añadir sobrecargas genéricas |
| Compatibilidad | Los ensamblados 1.x siguen cargando, pero **los genéricos requieren CLR 2.0 o posterior** |
| Serialización | Revisar: el nombre de un tipo genérico contiene sus argumentos |

---

## 9. Referencias

- [Generics](https://learn.microsoft.com/dotnet/csharp/fundamentals/types/generics) · [Constraints on type parameters](https://learn.microsoft.com/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters)
- [Iterators](https://learn.microsoft.com/dotnet/csharp/iterators) · [Nullable value types](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/nullable-value-types)
