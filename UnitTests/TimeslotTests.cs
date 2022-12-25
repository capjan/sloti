using Sloti;
using Sloti.Util;

namespace UnitTests;

public class TimeslotTests
{
    [Fact]
    public void CtorTest()
    {
        var from = Utc.CurrentHour();
        var to = from.AddHours(1);
        var sut = new Timeslots(from, to, 250);
        Assert.True(sut != null);
        Assert.Throws<ArgumentException>(() => { _ = new Timeslots(DateTime.Now, to, 1); });
        Assert.Throws<ArgumentException>(() => { _ = new Timeslots(from, DateTime.Now, 5); });
    }

    [Fact]
    public void BasicTest()
    {
        var thisYear = Utc.CurrentYear();
        var nextYear = thisYear.AddYears(1);

        var range = Timeslots.ForCurrentYear(100);

        Assert.Equal(1, range.Count);
        var slots = range.ToList();
        Assert.Single(slots);
        Assert.Equal(100, slots[0].Value);
        Assert.Equal(thisYear, slots[0].From);
        Assert.Equal(nextYear, slots[0].To);
    }

    [Fact]
    public void BasicTest2()
    {
        var thisYear = Utc.CurrentYear();
        var nextYear = thisYear.AddYears(1);

        var sut = Timeslots.ForYear(thisYear.Year, 100);

        Assert.Equal(1, sut.Count);
        var slots = sut.ToList();

        Assert.Single(slots);
        Assert.Equal(100, slots[0].Value);
        Assert.Equal(thisYear, slots[0].From);
        Assert.Equal(nextYear, slots[0].To);
    }

    [Fact]
    public void ItsForbiddenToAddNonUtcDates()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new Timeslots(DateTime.Now, DateTime.Now.AddHours(1), 1);
        });
    }

    [Fact]
    public void EnumeratorTest()
    {
        var sut = Timeslots.ForCurrentYear(120);
        using var enumerator = sut.GetEnumerator();
        Assert.Null(enumerator.Current);
        enumerator.MoveNext();
        Assert.NotNull(enumerator.Current);
    }

}
