namespace Sloti;

public interface ISlot<out T> where T : IComparable<T>
{
    public T From { get; }
    public T To { get; }
    public int Value { get; }
}
