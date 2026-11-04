<div align="center">

# 07 · C# 7.0–7.3: tuplas, patrones y `ref`

**Datos ligeros, primeros patrones y el camino hacia el rendimiento seguro (2017–2018)**

[⬅ 06 · C# 6](06-csharp-6-roslyn.md) · [🏠 Índice](../README.md) · [08 · C# 8 ➡](08-csharp-8-nulabilidad-y-streams.md)

</div>

<!-- cs=7 -->

| Ficha | C# 7.0 | C# 7.1 | C# 7.2 | C# 7.3 |
|---|---|---|---|---|
| **Publicación** | mar-2017 | ago-2017 | nov-2017 | may-2018 |
| **Visual Studio** | VS 2017 | VS 2017 15.3 | VS 2017 15.5 | VS 2017 15.7 |
| **`LangVersion`** | `7` | `7.1` | `7.2` | `7.3` |
| **Tema** | Tuplas y patrones | Pulido | `Span<T>` | Rendimiento seguro |

**Laboratorio:** [CristianSifuentes/CSharp7](https://github.com/CristianSifuentes/CSharp7)

> [!NOTE]
> **C# 7.3 sigue en producción.** Es la versión por defecto en .NET Framework y .NET Standard 2.0, de modo que gran parte del código
> empresarial y de las bibliotecas multiplataforma todavía se escribe con ella.

---

## Contenido

1. [Tuplas](#1-tuplas)
2. [Deconstrucción y descartes](#2-deconstrucción-y-descartes)
3. [Primeros patrones](#3-primeros-patrones)
4. [Funciones locales](#4-funciones-locales)
5. [`ref` locales y retornos](#5-ref-locales-y-retornos)
6. [Otras mejoras de C# 7.0](#6-otras-mejoras-de-c-70)
7. [C# 7.1](#7-c-71)
8. [C# 7.2: `Span<T>` y la familia `ref`](#8-c-72-spant-y-la-familia-ref)
9. [C# 7.3](#9-c-73)
10. [Notas de migración](#10-notas-de-migración)
11. [Referencias](#11-referencias)

---

## 1. Tuplas

### Antes y después

| Técnica previa | Problema |
|---|---|
| Parámetros `out` | No funcionan en `async` y resultan incómodos |
| `System.Tuple<T1, T2>` | Clase en el *heap*, miembros `Item1`, `Item2` |
| Clase auxiliar | Demasiado para un resultado interno |

```csharp run
class Programa
{
    static (int Minimo, int Maximo) Rango(int[] datos)
    {
        int min = datos[0], max = datos[0];
        foreach (int d in datos)
        {
            if (d < min) min = d;
            if (d > max) max = d;
        }
        return (min, max);
    }

    static void Main()
    {
        var r = Rango(new[] { 3, 1, 4 });
        Console.WriteLine(r.Minimo);    // → 1
        Console.WriteLine(r.Item2);     // → 4
    }
}
```

| Semántica | Detalle |
|---|---|
| Tipo real | `System.ValueTuple<…>`: `struct` **mutable**, con campos públicos |
| Nombres | **Solo existen en compilación**. En firmas públicas viajan en `TupleElementNamesAttribute`, y se pierden con la reflexión o el `dynamic` |
| Igualdad | `==` desde C# 7.3, elemento a elemento |
| Uso recomendado | Valores internos y privados. En una API pública estable, prefiere un `record` |

---

## 2. Deconstrucción y descartes

Cualquier tipo con un método `Deconstruct` admite deconstrucción, incluso por extensión. `_` descarta valores.

```csharp run
class Punto
{
    public int X = 3, Y = 4;
    public void Deconstruct(out int x, out int y) { x = X; y = Y; }
}

class Programa
{
    static void Main()
    {
        var (x, y) = new Punto();
        Console.WriteLine(x + y);                  // → 7

        (x, y) = (y, x);                           // intercambio sin variable temporal
        Console.WriteLine(x);                      // → 4

        var (_, soloY) = new Punto();
        Console.WriteLine(soloY);                  // → 4

        if (int.TryParse("42", out int n))         // variable out en línea
        {
            Console.WriteLine(n);                  // → 42
        }
    }
}
```

---

## 3. Primeros patrones

C# 7 introdujo los patrones de tipo y de constante, y `switch` admitió cualquier tipo y cláusulas `when`. Es la semilla del
[hilo evolutivo de patrones](14-hilos-evolutivos.md#2-pattern-matching).

```csharp run
class Programa
{
    static string Describir(object o)
    {
        switch (o)
        {
            case null:
                return "nulo";
            case int n when n < 0:
                return "negativo";
            case int n:
                return "entero " + n;
            case string s:
                return "texto de " + s.Length;
            default:
                return "otro";
        }
    }

    static void Main()
    {
        Console.WriteLine(Describir(-1));   // → negativo
        Console.WriteLine(Describir(5));    // → entero 5
        Console.WriteLine(Describir("ab")); // → texto de 2
        Console.WriteLine(Describir(null)); // → nulo
    }
}
```

> [!IMPORTANT]
> Con patrones, **el orden de los `case` importa**. El compilador rechaza un caso inalcanzable (CS8120). Antes de C# 7, los `case` eran
> constantes disjuntas y el orden no influía.

---

## 4. Funciones locales

| | Lambda | Función local |
|---|---|---|
| Asignación del delegado | Sí | **No**, si no se convierte a delegado |
| Clausura | Clase en el *heap* | `struct` pasado por referencia, si no escapa |
| Recursión | Incómoda | Natural |
| `yield` | No | Sí |
| Genéricos | No | Sí |

Su caso clásico es validar argumentos de inmediato en un iterador, resolviendo la limitación de [C# 2](03-csharp-2-genericos.md#3-iteradores-con-yield):

```csharp run
class Programa
{
    static IEnumerable<int> Primeros(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));  // inmediato
        return Iterar();

        IEnumerable<int> Iterar()
        {
            for (int i = 0; i < n; i++) yield return i;
        }
    }

    static void Main()
    {
        try
        {
            var s = Primeros(-1);    // falla aquí, no al enumerar
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Validación inmediata"); // → Validación inmediata
        }
    }
}
```

---

## 5. `ref` locales y retornos

Permiten referirse a una ubicación de memoria en vez de copiar su valor: crucial con `struct` grandes.

```csharp run
class Programa
{
    static ref int Mayor(int[] datos)
    {
        int indice = 0;
        for (int i = 1; i < datos.Length; i++)
        {
            if (datos[i] > datos[indice]) indice = i;
        }
        return ref datos[indice];
    }

    static void Main()
    {
        int[] valores = { 1, 9, 3 };
        ref int mayor = ref Mayor(valores);
        mayor = 0;                          // escribe en el arreglo
        Console.WriteLine(valores[1]);      // → 0
    }
}
```

El compilador impide devolver una referencia a una variable local: de ahí arranca el **análisis de seguridad de referencias**, que
madura en [C# 11](10-csharp-11-matematica-generica.md) con `scoped`.

---

## 6. Otras mejoras de C# 7.0

| Característica | Ejemplo |
|---|---|
| Literales binarios y separadores | `0b1010_1010`, `1_000_000` |
| Expresiones `throw` | `nombre ?? throw new ArgumentNullException()` |
| Más cuerpos de expresión | Constructores, finalizadores, `get`/`set` |
| Tipos de retorno `async` generalizados | `ValueTask<T>`: sin asignación si el resultado ya existe |

---

<!-- cs=7.1 -->

## 7. C# 7.1

Primera versión menor. Introdujo la selección de `LangVersion`.

```csharp run
class Programa
{
    static async Task Main()                    // async Main
    {
        await Task.Delay(1);
        int n = default;                        // literal default
        var nombre = "Ana";
        var edad = 30;
        var t = (nombre, edad);                 // nombres de tupla inferidos
        Console.WriteLine(n + " " + t.nombre);  // → 0 Ana
    }
}
```

También añadió la coincidencia de patrones sobre parámetros de tipo genérico.

---

<!-- cs=7.2 -->

## 8. C# 7.2: `Span<T>` y la familia `ref`

`Span<T>` representa una región contigua de memoria, sea arreglo, pila o memoria nativa, y permite segmentar **sin copiar ni asignar**.
Para que sea seguro, debe vivir en la pila: de ahí `ref struct`.

```csharp run
class Programa
{
    static int Sumar(ReadOnlySpan<int> datos)
    {
        int total = 0;
        foreach (int d in datos) total += d;
        return total;
    }

    static void Main()
    {
        int[] arreglo = { 1, 2, 3, 4 };
        Console.WriteLine(Sumar(arreglo.AsSpan(1, 2))); // → 5

        Span<int> pila = stackalloc int[3];             // sin unsafe y sin heap
        pila[0] = 10;
        Console.WriteLine(Sumar(pila));                 // → 10

        ReadOnlySpan<char> parte = "2026-11-04".AsSpan(0, 4);
        Console.WriteLine(int.Parse(parte));            // → 2026
    }
}
```

| Construcción | Significado |
|---|---|
| `in` | Parámetro por referencia y de solo lectura |
| `ref readonly` (retorno) | Devuelve una referencia que no se puede modificar |
| `readonly struct` | Inmutable: el compilador lo garantiza |
| `ref struct` | Solo en pila: no se puede poner en un campo de clase, ni convertir a `object` |
| `private protected` | Acceso desde clases derivadas **del mismo ensamblado** |
| Argumentos con nombre no finales | `M(a: 1, 2)` |
| Separador inicial | `0x_FF` |
| `ref` condicional | `ref var r = ref (c ? ref a : ref b);` |

### La trampa de la copia defensiva

Pasar un `struct` **no `readonly`** como `in` puede ser más lento que por valor. El compilador no sabe si un método modifica el valor, y lo
protege llamándolo sobre una copia:

```csharp run
struct Contador
{
    public int Valor;
    public void Incrementar() { Valor++; }
}

class Programa
{
    static void Tocar(in Contador c)
    {
        c.Incrementar();                // se ejecuta sobre una copia oculta
        Console.WriteLine(c.Valor);     // → 0
    }

    static void Main()
    {
        Tocar(new Contador());
    }
}
```

**Regla:** usa `in` solo con `readonly struct`, o con miembros `readonly` desde [C# 8](08-csharp-8-nulabilidad-y-streams.md).

---

<!-- cs=7.3 -->

## 9. C# 7.3

Lo que permite escribir código seguro casi tan rápido como el inseguro, y varios ajustes.

```csharp run unsafe
class Programa
{
    static int Tamano<T>() where T : unmanaged
    {
        unsafe { return sizeof(T); }
    }

    static string Nombre<T>(T valor) where T : System.Enum => valor.ToString();

    static void Main()
    {
        Console.WriteLine(Nombre(DayOfWeek.Monday));  // → Monday
        Console.WriteLine((1, "a") == (1, "a"));      // → True

        Span<int> datos = stackalloc[] { 1, 2, 3 };   // inicializador en stackalloc
        ref int r = ref datos[0];
        r = ref datos[2];                             // reasignación de ref
        Console.WriteLine(r);                         // → 3
    }
}
```

La tabla completa: restricciones `unmanaged`, `Enum` y `Delegate`; `==` en tuplas; reasignación de `ref`; inicializadores de `stackalloc`;
`fixed` por patrón; acceso a campos `fixed` sin fijar; variables de expresión en más lugares; y atributos sobre el campo oculto con
`[field: …]`.

---

## 10. Notas de migración

| Tema | Detalle |
|---|---|
| `ValueTuple` | Incluido desde .NET Framework 4.7; en versiones anteriores, paquete `System.ValueTuple` |
| `Span<T>` en .NET Framework | Paquete `System.Memory`, con una implementación más lenta y sin las sobrecargas de la biblioteca base |
| Tuplas públicas | Cambiar el nombre de un elemento no rompe los binarios, pero sí puede cambiar el código fuente |
| `ValueTask<T>` | **Solo se puede esperar una vez**: no lo guardes para reutilizarlo |

---

## 11. Referencias

- [Tuple types](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/value-tuples) · [Pattern matching](https://learn.microsoft.com/dotnet/csharp/fundamentals/functional/pattern-matching)
- [Memory and spans](https://learn.microsoft.com/dotnet/standard/memory-and-spans/) · [ref struct](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/ref-struct)
- [Write safe and efficient C# code](https://learn.microsoft.com/dotnet/csharp/advanced-topics/performance/)
