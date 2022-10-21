п»їusing System.Runtime.CompilerServices;

namespace Matrices;

public interface IMatrix
{
    public int RowCount { get; }
    public int ColCount { get; }
    public string? Name { get; set; }
    public int[,] ToArray();

    public int this[int r, int c] { get; set; }
    public bool IsSymmetric { get; }
    public bool IsSquare { get ; }
}
