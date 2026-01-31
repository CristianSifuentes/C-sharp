using System;

namespace C_sharp;

#region MatrixException
/// <summary>
/// Domain-specific exception type for matrix operation.
///
/// Why:
/// - Keeps matrix validation failures semantically distinct from generic ArgumentException. 
/// - Centralizes error messages so callers can reliably catch/recognize failures.
/// </summary>
// ---------- Errors (sentinel-style) ----------
public sealed class MatrixException: Exception
{
    public MatrixException(string message): base(message)
    {
        
    }
    public static MatrixException EmptyMatrix() => new ("matrix cannot be empty");
    public static MatrixException DimensionMismatch() => new ("matrix dimension mismatch");

}

#endregion
