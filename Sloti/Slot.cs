namespace Sloti;

public record Slot<T>(T From, T To, int Value) : ISlot<T> where T: IComparable<T>;
