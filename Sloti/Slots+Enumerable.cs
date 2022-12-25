using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Sloti;

public partial class Slots<T> : IEnumerable<ISlot<T>> where T: IComparable<T>
{
    public IEnumerator<ISlot<T>> GetEnumerator()
    {
        return _internalList.GetEnumerator();
    }

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _internalList.GetEnumerator();
    }
}
