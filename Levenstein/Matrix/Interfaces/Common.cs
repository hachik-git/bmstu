п»їnamespace Common.Interfaces;

public interface ICompressed<T>
{
    public float Compression { get; }
    public T Decompress();
}
