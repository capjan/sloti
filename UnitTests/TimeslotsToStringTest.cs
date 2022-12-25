using Sloti;
using Sloti.Util;

namespace UnitTests;

public class TimeslotsToStringTest
{

    [Fact]
    public void ToStringTest()
    {
        var from = Utc.CurrentDay();
        var to = from.AddDays(4);

        var slotFrom = from.AddDays(1);
        var slotTo = slotFrom.AddDays(1);


        var sut = new Timeslots(from, to, 0);
        // [ 4 days - 0 ]
        sut.ApplySlot(slotFrom, slotTo, 2);
        // [ 1 day - 0 ] [ 1 day - 2 ] [ 2 days - 0 ]

        Assert.Equal(3, sut.Count);
        var data = sut.ToArray();
        Assert.Equal(0, data[0].Value);
        Assert.Equal(2, data[1].Value);
        Assert.Equal(0, data[2].Value);
    }
}
