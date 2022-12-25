namespace Sloti;

public static class SlotRelated
{
    /// <summary>
    /// Returns the duration of the slot
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public static TimeSpan Duration(this ISlot<DateTime> slot)
    {
        return slot.To - slot.From;
    }

}
