using System;
using System.Numerics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MatrixMul;



#region RowBlock
// ---------- Concurrency: worker pool + channel + cancellation ----------

/*
A record in C# is a concise class type for immutable data models, emphasizing value-based equality (data is equal if its contents are equal, not if it is the same instance in memory) and automatically generating useful methods such as ToString() and equality comparisons, facilitating work with DTOs (Data Transfer Objects) and immutability, available since C# 9. It allows non-destructive mutation with the with expression, deconstruction, and can be defined as a reference type (record class) or a value type (record struct).
*/

/*
The `internal` keyword is an access modifier for types and type members.

This page is about the `internal` access modifier. The `internal` keyword is also part of the `protected internal` access modifier.

Internal types and members can only be accessed within files of the same assembly, as in this example:

C#
public class BaseClass
{
// Only accessible within the same assembly.
internal static int x = 0;

}

*/
internal readonly record struct RowBlock(int Start, int EndExclusive);

#endregion

