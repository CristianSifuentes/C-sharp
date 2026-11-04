<div align="center">

# 02 · C# 1.0 y 1.2: los cimientos

**Tipos, propiedades, eventos, delegados y atributos sobre un runtime administrado (2002–2003)**

[⬅ 01 · Modelo de versiones](01-modelo-de-versiones.md) · [🏠 Índice](../README.md) · [03 · C# 2 ➡](03-csharp-2-genericos.md)

</div>

<!-- cs=1 -->

| Ficha | C# 1.0 | C# 1.2 |
|---|---|---|
| **Publicación** | enero de 2002 | abril de 2003 |
| **Visual Studio** | VS .NET 2002 | VS .NET 2003 |
| **Runtime** | .NET Framework 1.0 | .NET Framework 1.1 |
| **`LangVersion` actual** | `1` o `ISO-1` | `1` o `ISO-1` |
| **Laboratorio** | [CristianSifuentes/CSharp1](https://github.com/CristianSifuentes/CSharp1) | — |

---

## Contenido

1. [El problema que resolvía](#1-el-problema-que-resolvía)
2. [El sistema de tipos unificado](#2-el-sistema-de-tipos-unificado)
3. [Propiedades e indexadores](#3-propiedades-e-indexadores)
4. [Delegados y eventos](#4-delegados-y-eventos)
5. [Herencia, interfaces y polimorfismo](#5-herencia-interfaces-y-polimorfismo)
6. [Atributos y reflexión](#6-atributos-y-reflexión)
7. [Recursos: `using`, `lock` y `unsafe`](#7-recursos-using-lock-y-unsafe)
8. [C# 1.2: `Dispose` en `foreach`](#8-c-12-dispose-en-foreach)
9. [Lo que faltaba](#9-lo-que-faltaba)
10. [Referencias](#10-referencias)

---

## 1. El problema que resolvía

A finales de los noventa, escribir software para Windows implicaba elegir entre la productividad de Visual Basic y el
control de C++ con COM. C# nació como un lenguaje "simple, moderno y de propósito general", con tres apuestas:

| Apuesta | Antes | C# 1.0 |
|---|---|---|
| Memoria | `new`/`delete` y conteo de referencias COM | Recolector de basura |
| Componentes | IDL, registro y `IUnknown` | Metadatos en el ensamblado, propiedades y eventos en el lenguaje |
| Seguridad de tipos | Punteros y conversiones sin comprobar | IL verificable; `unsafe` solo por decisión explícita |

---

## 2. El sistema de tipos unificado

Todo deriva de `System.Object`, incluidos los tipos de valor. La diferencia esencial está en la **semántica de copia**:

| | `class` (referencia) | `struct` (valor) |
|---|---|---|
| Asignación | Copia la referencia | Copia los datos |
| Almacenamiento | Objeto en el *heap* administrado | Allí donde esté declarado: pila, campo o elemento de arreglo |
| `null` | Permitido | No permitido (hasta C# 2 con `T?`) |
| Herencia | Sí | No, salvo interfaces |

```csharp run
struct PuntoValor { public int X; }
class PuntoRef { public int X; }

class Programa
{
    static void Main()
    {
        PuntoValor a = new PuntoValor();
        PuntoValor b = a;          // copia los datos
        b.X = 10;
        Console.WriteLine(a.X);    // → 0

        PuntoRef c = new PuntoRef();
        PuntoRef d = c;            // copia la referencia
        d.X = 10;
        Console.WriteLine(c.X);    // → 10
    }
}
```

### Boxing: el precio de la unificación

Convertir un tipo de valor en `object` asigna un objeto en el *heap* y **copia** el valor. Sin genéricos, toda colección
lo hacía de forma constante:

```csharp run
using System.Collections;

class Programa
{
    static void Main()
    {
        ArrayList lista = new ArrayList();
        lista.Add(42);                 // boxing: asigna un objeto para guardar el int
        lista.Add("hola");             // nada impide mezclar tipos

        int n = (int)lista[0];         // unboxing y conversión explícita
        Console.WriteLine(n);          // → 42

        try
        {
            int m = (int)lista[1];     // compila, y falla en ejecución
        }
        catch (InvalidCastException)
        {
            Console.WriteLine("Error descubierto en producción"); // → Error descubierto en producción
        }
    }
}
```

> [!NOTE]
> Este ejemplo resume por qué [C# 2](03-csharp-2-genericos.md) fue inevitable: costo de asignación por elemento **y**
> errores de tipos que solo aparecen al ejecutar.

---

## 3. Propiedades e indexadores

Java usaba convenciones (`getX`/`setX`). C# hizo de la propiedad un concepto del lenguaje y de los metadatos, lo que
permitió a diseñadores visuales y a la serialización descubrirlas de forma uniforme.

```csharp run
class Inventario
{
    private string[] _nombres = new string[3];
    private int _total;

    public int Total                // propiedad: get_Total y set_Total en IL
    {
        get { return _total; }
    }

    public string this[int i]       // indexador: la propiedad Item
    {
        get { return _nombres[i]; }
        set
        {
            if (_nombres[i] == null) _total++;
            _nombres[i] = value;
        }
    }
}

class Programa
{
    static void Main()
    {
        Inventario inv = new Inventario();
        inv[0] = "teclado";
        Console.WriteLine(inv[0]);    // → teclado
        Console.WriteLine(inv.Total); // → 1
    }
}
```

Compatibilidad binaria: **cambiar un campo público por una propiedad rompe a quien ya compiló contra él**, porque el IL pasa de
acceder a un campo a llamar a un método. Por eso la buena práctica desde 2002 es exponer propiedades.

---

## 4. Delegados y eventos

Un delegado es un puntero a función seguro: un objeto que guarda destino y método. Son **multidifusión**: `+=` crea una
nueva lista de invocación, porque los delegados son inmutables.

Un `event` restringe ese delegado: desde fuera del tipo solo se permite `+=` y `-=`. Sin esa palabra clave, cualquiera
podría invocar o sobrescribir la lista.

```csharp run
delegate void Notificador(string mensaje);

class Sensor
{
    public event Notificador Alerta;   // desde fuera: solo += y -=

    public void Medir(int valor)
    {
        if (valor > 100 && Alerta != null)
        {
            Alerta("Valor alto: " + valor);
        }
    }
}

class Programa
{
    static void Registrar(string m) { Console.WriteLine("LOG " + m); }
    static void Avisar(string m) { Console.WriteLine("SMS " + m); }

    static void Main()
    {
        Sensor s = new Sensor();
        s.Alerta += new Notificador(Registrar);
        s.Alerta += new Notificador(Avisar);
        s.Medir(120);
        // → LOG Valor alto: 120
        // → SMS Valor alto: 120
    }
}
```

> [!WARNING]
> La comprobación `Alerta != null` seguida de la invocación tiene una carrera entre hilos: otro hilo puede quitar el último
> suscriptor entre ambas. C# 1 obligaba a copiar el delegado a una variable local; desde [C# 6](06-csharp-6-roslyn.md)
> `Alerta?.Invoke(...)` lo resuelve.

Los eventos también son una **fuente clásica de fugas de memoria**: el publicador mantiene viva una referencia a cada suscriptor
hasta que este ejecuta `-=`.

---

## 5. Herencia, interfaces y polimorfismo

| Decisión de diseño | C++ | Java (2002) | C# 1.0 |
|---|---|---|---|
| Herencia múltiple de clases | Sí | No | No |
| Métodos virtuales | Opt-in (`virtual`) | Por defecto | **Opt-in** (`virtual`) y explícito (`override`) |
| Ocultar un método | Implícito | No aplica | Explícito con `new` |

Exigir `override` protege frente al problema de la **clase base frágil**: si una nueva versión de la base añade un método con
el mismo nombre que uno tuyo, tu código no lo sobrescribe sin querer.

```csharp run
class Animal
{
    public virtual string Sonido() { return "..."; }
    public string Tipo() { return "animal"; }
}

class Perro : Animal
{
    public override string Sonido() { return "guau"; }
    public new string Tipo() { return "perro"; }    // oculta, no sobrescribe
}

class Programa
{
    static void Main()
    {
        Animal a = new Perro();
        Console.WriteLine(a.Sonido()); // → guau
        Console.WriteLine(a.Tipo());   // → animal
    }
}
```

La segunda línea es la lección: `override` se resuelve con el **tipo en ejecución**; `new` se resuelve con el **tipo estático**.

---

## 6. Atributos y reflexión

Los atributos añaden metadatos declarativos que viajan en el ensamblado. La idea sigue viva en cada versión: `[Obsolete]` en 2002,
`[CallerMemberName]` en C# 5, `[Experimental]` en C# 12 y `[OverloadResolutionPriority]` en C# 13.

```csharp run
using System.Reflection;

[AttributeUsage(AttributeTargets.Class)]
class TablaAttribute : Attribute
{
    public string Nombre;
    public TablaAttribute(string nombre) { Nombre = nombre; }
}

[Tabla("clientes")]
class Cliente { }

class Programa
{
    static void Main()
    {
        object[] atributos = typeof(Cliente).GetCustomAttributes(typeof(TablaAttribute), false);
        Console.WriteLine(((TablaAttribute)atributos[0]).Nombre); // → clientes
    }
}
```

---

## 7. Recursos: `using`, `lock` y `unsafe`

El recolector libera memoria, no archivos ni conexiones. `using` garantiza `Dispose` con un `try/finally` generado, y `lock`
hace lo propio con `Monitor.Enter` y `Monitor.Exit`.

| Construcción | Se traduce a | Evolución |
|---|---|---|
| `using (r) { }` | `try { } finally { r.Dispose(); }` | Declaración `using` en C# 8 · `await using` en C# 8 |
| `lock (o) { }` | `Monitor.Enter` + `try/finally` | `System.Threading.Lock` en C# 13 |
| `unsafe { }` | IL no verificable, con punteros | `Span<T>` en C# 7.2 · nuevo modelo en *preview* en C# 15 |

---

## 8. C# 1.2: `Dispose` en `foreach`

La única mejora relevante: desde esta versión, el código generado por `foreach` llama a `Dispose` sobre el enumerador cuando
implementa `IDisposable`. Parece menor, pero es la base que permitió a los iteradores de C# 2 ejecutar bloques `finally` al
abandonar un bucle antes de tiempo.

```csharp omitir
// Forma aproximada de lo que genera foreach desde C# 1.2
IEnumerator e = coleccion.GetEnumerator();
try
{
    while (e.MoveNext())
    {
        Elemento x = (Elemento)e.Current;  // conversión implícita en el foreach
        // cuerpo del bucle
    }
}
finally
{
    IDisposable d = e as IDisposable;
    if (d != null) d.Dispose();
}
```

---

## 9. Lo que faltaba

| Carencia en 2002 | Consecuencia | Se resuelve en |
|---|---|---|
| Sin genéricos | *Boxing* y conversiones | [C# 2](03-csharp-2-genericos.md) |
| Delegados verbosos | Un método con nombre por cada *callback* | [C# 2](03-csharp-2-genericos.md) y [C# 3](04-csharp-3-linq.md) |
| Sin `int?` | Valores centinela como `-1` | [C# 2](03-csharp-2-genericos.md) |
| Enumeradores escritos a mano | Clases auxiliares con estado | [C# 2](03-csharp-2-genericos.md) |
| Consultas como cadenas SQL | Errores solo en ejecución | [C# 3](04-csharp-3-linq.md) |
| Asincronía con `BeginX`/`EndX` | Lógica fragmentada en *callbacks* | [C# 5](05-csharp-4-5-dinamismo-y-asincronia.md) |

> [!TIP]
> **Herencia viva.** Propiedades, eventos, delegados y atributos siguen casi intactos. Si un concepto de C# moderno parece
> mágico, casi siempre es azúcar sobre uno de estos cimientos.

---

## 10. Referencias

- [The history of C#](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-version-history)
- [Value types](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/value-types) · [Boxing and unboxing](https://learn.microsoft.com/dotnet/csharp/programming-guide/types/boxing-and-unboxing)
- Repositorios de apoyo: [tipos de clases](https://github.com/CristianSifuentes/typesOfClassesCSh) · [clases abstractas e interfaces](https://github.com/CristianSifuentes/abstract-classes-vs-interfaces-csh)
