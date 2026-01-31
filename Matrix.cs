using System;
using System;
using System.Buffers;
using System.Numerics;

namespace C_sharp;

#region Matrix
// ---------- Matrix type with methods (generics + value semantics-ish) ----------
// Notes:
// - We use T : unmanaged, INumber<T> (generic math) to allow + and * on float/double/decimal/etc.
// - Row-major layout in a single flat array for performance.
// - At() is bounds-checked by runtime; if you want the Go-style "panic for speed", you can add Unsafe access. 
public sealed class Matrix<T> where T : unmanaged, INumber<T>
{
    public int Rows { get; }
    public int Cols { get; }
    internal T[] Data { get; } // internal so Mul can use fast path without virtual calls

    public Matrix(int rows, int cols)
    {
        if (rows <= 0 || cols <= 0) throw MatrixException.EmptyMatrix();
        Rows = rows;
        Cols = cols;
        Data = new T[rows * cols];
    }

    /*
    Provides a type-safe and memory-safe read-only representation of a contiguous region of arbitrary memory.

    C#
    [System.Runtime.InteropServices.Marshalling.NativeMarshalling(typeof(System.Runtime.InteropServices.Marshalling.ReadOnlySpanMarshaller<,>))]
    public readonly ref struct ReadOnlySpan<T>
    */

    /*
    ReadOnlySpan<T> is a ref struct that is allocated on the stack and can never escape to the managed heap. Ref struct types have a number of restrictions to ensure they cannot be promoted to the managed heap, including that they cannot be boxed, captured in lambda expressions, assigned to variables of type Object, assigned to dynamic variables, and cannot implement any interface types.

    An instance of ReadOnlySpan<T> is often used to reference the elements of an array or a portion of an array. Unlike an array, however, a ReadOnlySpan<T> instance can point to managed memory, native memory, or stack-allocated managed memory.
    */
    public static Matrix<T> FromRowMajor (int rows, int cols, ReadOnlySpan<T> rowMajor)
    {
        if (rows <= 0 || cols <= 0) throw MatrixException.EmptyMatrix();
        if (rowMajor.Length != rows * cols)
            throw new ArgumentException($"invalid data length: got {rowMajor.Length} want {rows * cols}");

        var m = new Matrix<T>(rows, cols);
        rowMajor.CopyTo(m.Data);
        return m;
    }

    public T At(int i, int j) => Data[(i * Cols) +  j];
    public void Set(int i, int j, T v) => Data[(i * Cols) +  j] = v;

    public Matrix<T> Clone()
    {
        var m = new Matrix<T>(Rows, Cols);
        Array.Copy(Data, m.Data, Data.Length);
        return m;     
    }

    // Transpose returns a new matrix with transposed data (cache-friendly for multiplication)
    public Matrix<T> Transpose()
    {
        var outM = new Matrix<T>(Cols, Rows);
        for (int i = 0; i < Rows; i++)
        {
            int rowOff = i * Cols;
            for (int j= 0; j < Cols; j++)
            {
                outM.Data[(j * outM.Cols) + i] = Data[rowOff + j];
            }
        }
        return outM;
    }
}

#endregion

