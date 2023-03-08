using System.Diagnostics;

namespace Sloti;

public partial class Slots<T> where T: IComparable<T>
{
    private readonly LinkedList<ISlot<T>> _internalList = new();
    public int Count => _internalList.Count;
    public Slots(T from, T to, int value)
    {
      _internalList.AddLast(new Slot<T>(from, to, value));
    }

    public void ApplySlot(T from, T to, int value)
    {
        var slot = new Slot<T>(from, to, value);
        ApplySlot(slot);
    }

    public void ApplySlot(ISlot<T> slot)
    {

        if (slot.From.CompareTo(slot.To) > 0) throw new ArgumentException("from must be before to argument");

        // never be null due to design
        var node = _internalList.First!;

        // early return if the slot is before the range
        if (node.Value.From.CompareTo(slot.To) > 0) return;

        // early return if the slot is behind the last node
        var last = _internalList.Last!;
        if (last.Value.To.CompareTo(slot.From) < 0) return;

        // skip all nodes that are before the beginning of the slot and are not affected
        while (node?.Value.To.CompareTo(slot.From) <= 0) node = node.Next;

        // makes the compiler happy
        Debug.Assert(node != null, nameof(node) + " != null");

        // first hit
        //if (node.Value.ToUtc <= slot.FromUtc) return;

        // split the front if necessary
        if (node.Value.From.CompareTo(slot.From) < 0)
        {
            var v = node.Value;
            var insertedNode = new Slot<T>(From: v.From, To: slot.From, Value: v.Value);
            var newCurrentNode = new Slot<T>(From: slot.From, To: v.To, Value: v.Value);
            _internalList.AddBefore(node: node, insertedNode);
            var newNode = _internalList.AddBefore(node, newCurrentNode);
            _internalList.Remove(node);
            node = newNode;
        }

        while (node?.Value.From.CompareTo(slot.To) < 0)
        {
            var v = node.Value;
            // split back if necessary
            if (slot.To.CompareTo(node.Value.To) < 0)
            {
                var leftPart = new Slot<T>(From: v.From, To: slot.To, Value: v.Value + slot.Value);
                var remainingRightPart = new Slot<T>(From: slot.To, To: v.To, Value: v.Value);
                var leftNode = _internalList.AddBefore(node, leftPart);
                _internalList.AddBefore(node, remainingRightPart);
                _internalList.Remove(node);
                MergeNodeWithNeighboursIfPossible(leftNode);
                return; // return because we're done
            }

            var newSlotValue = new Slot<T>(From: v.From, To: v.To, Value: v.Value + slot.Value);
            var newNode = _internalList.AddBefore(node, newSlotValue);
            _internalList.Remove(node);
            node = newNode.Next;

            MergeNodeWithNeighboursIfPossible(newNode);
        }
    }

    private void MergeNodeWithNeighboursIfPossible(LinkedListNode<ISlot<T>> node)
    {
        var prev = node.Previous;
        var next = node.Next;

        var prevVal   = prev?.Value.Value;
        var nextValue = next?.Value.Value;
        var nodeValue = node.Value.Value;

        if (prev is not null && next is not null && prevVal == nodeValue && nextValue == nodeValue)
        {
            // merge all 3 to a single Node
            var mergedSlot = new Slot<T>(prev.Value.From, next.Value.To, node.Value.Value);
            _internalList.AddBefore(prev, mergedSlot);
            _internalList.Remove(prev);
            _internalList.Remove(node);
            _internalList.Remove(next);
        }
        else if (prev?.Value.Value == node.Value.Value)
        {
            // merge only with previous node
            var mergedSlot = new Slot<T>(prev.Value.From, node.Value.To, node.Value.Value);
            _internalList.AddBefore(prev, mergedSlot);
            _internalList.Remove(prev);
            _internalList.Remove(node);
        }
        else if (next?.Value.Value == node.Value.Value)
        {
            // merge only with next node
            var mergedSlot = new Slot<T>(node.Value.From, next.Value.To, node.Value.Value);
            _internalList.AddBefore(node, mergedSlot);
            _internalList.Remove(node);
            _internalList.Remove(next);
        }
    }

}
