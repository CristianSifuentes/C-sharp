<div align="center">

# 16 · Guías de decisión

**Árboles para elegir la construcción correcta del lenguaje en cada situación**

[⬅ 15 · Bajo el capó](15-bajo-el-capo-y-rendimiento.md) · [🏠 Índice](../README.md) · [17 · Catálogo A–Z ➡](17-catalogo-a-z.md)

</div>

> [!TIP]
> Los árboles son puntos de partida: la decisión final depende de tu base de código, tu equipo y de mediciones reales.

---

## Contenido

1. [Modelar datos](#1-modelar-datos)
2. [Representar "uno de varios"](#2-representar-uno-de-varios)
3. [Tratar errores](#3-tratar-errores)
4. [Elegir una colección](#4-elegir-una-colección)
5. [Concurrencia y sincronización](#5-concurrencia-y-sincronización)
6. [Asincronía: qué tipo devolver](#6-asincronía-qué-tipo-devolver)
7. [Extender un tipo](#7-extender-un-tipo)
8. [Elegir versión de C# y TFM](#8-elegir-versión-de-c-y-tfm)
9. [Decisiones que conviene evitar](#9-decisiones-que-conviene-evitar)

---

## 1. Modelar datos

```mermaid
flowchart TD
    A{"¿Tiene identidad propia<br/>y comportamiento?"} -->|Sí| CL["class"]
    A -->|"No, solo son datos"| B{"¿Vive solo dentro de<br/>un método o clase?"}
    B -->|Sí| TU["Tupla con nombres"]
    B -->|No| C{"¿Es pequeño y<br/>muy numeroso?"}
    C -->|Sí| RS["readonly record struct"]
    C -->|No| D{"¿Es uno de un<br/>conjunto cerrado?"}
    D -->|Sí| U["Ver sección 2"]
    D -->|No| RC["record"]
    classDef r fill:#68217A,stroke:#3B0F47,color:#fff
    class CL,TU,RS,U,RC r
```

| Necesidad | Elección | Versión mínima |
|---|---|---|
| Entidad de EF Core con seguimiento | `class` | C# 1 |
| DTO, evento, comando | `record` | C# 9 |
| Objeto de valor (dinero, coordenada) | `readonly record struct` | C# 10 |
| Resultado interno con dos valores | Tupla | C# 7 |
| Propiedades obligatorias en inicializadores | `required` | C# 11 |
| Inyección de dependencias | Constructor primario | C# 12 |

---

## 2. Representar "uno de varios"

```mermaid
flowchart TD
    A{"¿Los casos son valores<br/>sin datos?"} -->|Sí| EN["enum"]
    A -->|No| B{"¿Terceros deben poder<br/>añadir casos?"}
    B -->|Sí| IF["interface o clase abstracta"]
    B -->|No| C{"¿C# 15 disponible?"}
    C -->|No| AB["clase abstracta + constructor privado<br/>o biblioteca"]
    C -->|Sí| D{"¿Comparten base y<br/>comportamiento?"}
    D -->|Sí| CH["closed class"]
    D -->|No| E{"¿Tipos de valor en<br/>ruta crítica?"}
    E -->|Sí| CU["Unión a medida<br/>con TryGetValue"]
    E -->|No| UN["union"]
    classDef r fill:#CA5010,stroke:#7A2E06,color:#fff
    class EN,IF,AB,CH,CU,UN r
```

---

## 3. Tratar errores

| Situación | Elección |
|---|---|
| Fallo inesperado o error de programación | Excepción |
| Argumento inválido | `ArgumentException` y sus métodos `ThrowIf…` |
| Fallo **esperado**, frecuente y parte del dominio | Patrón `TryX` o unión `Resultado` |
| Ruta crítica | Evita excepciones: son costosas |
| Reintento según datos del error | Filtro `when` |
| Limpieza | `using`, nunca un `catch` que solo relanza |

```csharp cs=15 run
Console.WriteLine(Mensaje(Parsear("7")));    // → valor 7
Console.WriteLine(Mensaje(Parsear("x")));    // → no numérico

static Lectura Parsear(string s) =>
    int.TryParse(s, out int n) ? new Valido(n) : new Invalido("no numérico");

static string Mensaje(Lectura l) => l switch
{
    Valido v => $"valor {v.Numero}",
    Invalido i => i.Motivo,
    null => "sin lectura",
};

record Valido(int Numero);
record Invalido(string Motivo);
union Lectura(Valido, Invalido);
```

---

## 4. Elegir una colección

| Necesidad | Tipo | Nota |
|---|---|---|
| Secuencia general | `List<T>` | La opción por defecto |
| Búsqueda por clave | `Dictionary<TKey, TValue>` | |
| Pertenencia | `HashSet<T>` | |
| Orden por clave | `SortedDictionary<,>` | |
| Fijo, inmutable, muchas lecturas | `FrozenDictionary<,>`, `FrozenSet<T>` | Construcción más cara, lectura más rápida |
| Inmutable con versiones | `ImmutableList<T>`, `ImmutableArray<T>` | |
| Varios hilos | `ConcurrentDictionary<,>` | |
| Productor/consumidor | `Channel<T>` | Usado en este repositorio |
| Prioridades | `PriorityQueue<,>` | |
| API de entrada | `IEnumerable<T>`, `IReadOnlyList<T>` | Exige lo mínimo |
| API de salida | `IReadOnlyList<T>` o el tipo concreto | Promete lo máximo |

---

## 5. Concurrencia y sincronización

```mermaid
flowchart TD
    A{"¿Hay await dentro de<br/>la sección crítica?"} -->|Sí| SS["SemaphoreSlim.WaitAsync"]
    A -->|No| B{"¿Es una operación<br/>atómica simple?"}
    B -->|Sí| IL["Interlocked"]
    B -->|No| C{"¿Muchas lecturas,<br/>pocas escrituras?"}
    C -->|Sí| RW["ReaderWriterLockSlim<br/>o estructuras inmutables"]
    C -->|No| D{"¿.NET 9 o superior?"}
    D -->|Sí| LK["lock con System.Threading.Lock"]
    D -->|No| LO["lock con object privado"]
    classDef r fill:#00B7C3,stroke:#006B74,color:#04121a
    class SS,IL,RW,LK,LO r
```

| Trabajo | Herramienta |
|---|---|
| E/S concurrente | `Task.WhenAll` |
| CPU por elementos | `Parallel.ForEach`, `Parallel.ForEachAsync` |
| Tubería con contrapresión | `Channel<T>` acotado |
| Limitar la concurrencia | `SemaphoreSlim`, `MaxDegreeOfParallelism` |

---

## 6. Asincronía: qué tipo devolver

| Situación | Tipo |
|---|---|
| Opción por defecto | `Task` / `Task<T>` |
| Ruta crítica que suele completar de forma síncrona | `ValueTask<T>` |
| Muchos elementos que llegan con el tiempo | `IAsyncEnumerable<T>` |
| Manejador de evento | `async void` (único caso) |
| El resultado ya existe | `Task.FromResult`, sin `async` |

---

## 7. Extender un tipo

| Situación | Opción | Versión |
|---|---|---|
| El tipo es tuyo | Añade un miembro | C# 1 |
| Necesitas estado | Composición o herencia | C# 1 |
| Evolucionar una interfaz publicada | Miembro por defecto | C# 8 |
| Tipo ajeno, solo métodos | Método de extensión `this` | C# 3 |
| Tipo ajeno, propiedades, estáticos u operadores | Bloque `extension` | C# 14 |
| Tipo ajeno, indexador | Bloque `extension` | C# 15 |

---

## 8. Elegir versión de C# y TFM

```mermaid
flowchart TD
    A{"¿Debes ejecutar en<br/>.NET Framework?"} -->|Sí| F["C# 7.3<br/>netstandard2.0 o net48"]
    A -->|No| B{"¿Es una biblioteca para<br/>consumidores variados?"}
    B -->|Sí| M["Multidestino:<br/>netstandard2.0 + net10.0"]
    B -->|No| C{"¿Priorizas soporte<br/>largo?"}
    C -->|Sí| L[".NET 10 LTS · C# 14<br/>hasta nov-2028"]
    C -->|No| D[".NET 11 · C# 15<br/>tras la GA"]
    classDef r fill:#512BD4,stroke:#2A1272,color:#fff
    class F,M,L,D r
```

---

## 9. Decisiones que conviene evitar

| Antipatrón | Por qué | Alternativa |
|---|---|---|
| `LangVersion=latest` | No reproducible | Omitir o fijar |
| C# más nuevo que el TFM | Sin soporte | Sube el TFM |
| `.Result` y `.Wait()` en código asíncrono | Interbloqueos | `await` |
| `async void` en lógica | Excepciones que no se pueden capturar | `async Task` |
| `dynamic` por comodidad | Sin comprobación | Tipos concretos |
| `!` por todo el código | Promesas sin validar | Modela el `null` |
| `record` con colecciones mutables | Igualdad y copia engañosas | Colecciones inmutables |
| `struct` grande y mutable | Copias y errores sutiles | `class` o `readonly struct` |
| `_` en un `switch` sobre una unión | Oculta casos nuevos | Brazo `null` y cobertura explícita |
| `ToList()` antes de `Where` en EF Core | Trae toda la tabla | Filtra en `IQueryable<T>` |
| Interpolar SQL | Inyección de SQL | Parámetros |
| Capturar `Exception` sin relanzar | Oculta errores | Captura lo que puedes tratar |
