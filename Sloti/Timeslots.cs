using Sloti.Util;

namespace Sloti;

public class Timeslots : Slots<DateTime>
{
    public static Timeslots ForYear(int utcYear, int value)
    {
        var from = Utc.Create(utcYear);
        var to = from.AddYears(1);
        return new Timeslots(from, to, value);
    }

    public static Slots<DateTime> ForCurrentYear(int value)
    {
        return ForYear(DateTime.UtcNow.Year, value);
    }

    public Timeslots(DateTime from, DateTime to, int value) : base(from, to, value)
    {
        if (from.Kind != DateTimeKind.Utc) throw new ArgumentException($"{nameof(from)} must be of kind Utc");
        if (to.Kind != DateTimeKind.Utc) throw new ArgumentException($"{nameof(to)} must be of kind Utc");
    }
}

public record TimeSlot(DateTime From, DateTime To, int Value) : ISlot<DateTime>;
