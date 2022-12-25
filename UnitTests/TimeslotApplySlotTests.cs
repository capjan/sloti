using Sloti;
using Sloti.Util;

namespace UnitTests;

public class TimeslotApplySlotTests
{
    [Fact]
    public void Test1()
    {
        // From:     [     0     ]
        // Apply:    |   [ 2 ]   |
        // Expected: [ 0 | 2 | 0 ]
        var from = Utc.CurrentDay();
        var to = from.AddDays(4);

        var slotFrom = from.AddDays(1);
        var slotTo = slotFrom.AddDays(1);

        var sut = new Slots<DateTime>(from, to, 0);
        sut.ApplySlot(slotFrom, slotTo, 2);

        Assert.Equal(3, sut.Count);
        var data = sut.ToArray();
        Assert.Equal(0, data[0].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[0].Duration());
        Assert.Equal(2, data[1].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[1].Duration());
        Assert.Equal(0, data[2].Value);
        Assert.Equal(TimeSpan.FromDays(2), data[2].Duration());
    }

    [Fact]
    public void Test2()
    {
        // From:         [   0   ]
        // Apply:    [    2  ]   |
        // Expected:     [ 2 | 0 ]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(4);

        var slotFrom = from.AddDays(-2);
        var slotTo =   from.AddDays(2);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slotFrom, slotTo, 2);

        Assert.Equal(2, sut.Count);
        var data = sut.ToArray();
        Assert.Equal(2, data[0].Value);
        Assert.Equal(TimeSpan.FromDays(2), data[0].Duration());
        Assert.Equal(0, data[1].Value);
        Assert.Equal(TimeSpan.FromDays(2), data[1].Duration());
    }

    [Fact]
    public void Test3()
    {
        // From:     [       ]
        // Apply:    |   [   2   ]
        // Expected: [ 0 | 2 ]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(4);

        var slotFrom = from.AddDays(2);
        var slotTo =   from.AddDays(6);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slotFrom, slotTo, 2);

        Assert.Equal(2, sut.Count);
        var data = sut.ToArray();
        Assert.Equal(0, data[0].Value);
        Assert.Equal(TimeSpan.FromDays(2), data[0].Duration());
        Assert.Equal(2, data[1].Value);
        Assert.Equal(TimeSpan.FromDays(2), data[1].Duration());
    }

    [Fact]
    public void Test4()
    {
        // From:     [   0    ]
        // Apply:    [11]     |
        // Apply:    |  [22]  |
        // Apply:    |     [33]
        // Expected: [11|22|33]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot1 = new TimeSlot(from.AddDays(0), from.AddDays(1), 11);
        var slot2 = new TimeSlot(from.AddDays(1), from.AddDays(2), 22);
        var slot3 = new TimeSlot(from.AddDays(2), from.AddDays(3), 33);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot1);
        sut.ApplySlot(slot2);
        sut.ApplySlot(slot3);

        var data = sut.ToArray();

        Assert.Equal(3, sut.Count);
        Assert.Equal(11, data[0].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[0].Duration());
        Assert.Equal(22, data[1].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[1].Duration());
        Assert.Equal(33, data[2].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[2].Duration());
    }

    [Fact]
    public void Test5()
    {
        // From:     [        ]
        // Apply:    |  [11]  |
        // Apply:    [22]     |
        // Apply:    |     [33]
        // Expected: [22|11|33]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot1 = new TimeSlot(from.AddDays(1), from.AddDays(2), 11);
        var slot2 = new TimeSlot(from.AddDays(0), from.AddDays(1), 22);
        var slot3 = new TimeSlot(from.AddDays(2), from.AddDays(3), 33);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot1);
        sut.ApplySlot(slot2);
        sut.ApplySlot(slot3);

        var data = sut.ToArray();

        Assert.Equal(3, sut.Count);
        Assert.Equal(22, data[0].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[0].Duration());
        Assert.Equal(11, data[1].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[1].Duration());
        Assert.Equal(33, data[2].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[2].Duration());
    }

    [Fact]
    public void Test6()
    {
        // From:     [    0   ]
        // Apply:    |     [11]
        // Apply:    |  [22]  |
        // Apply:    [33]     |
        // Expected: [33|22|11]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot1 = new TimeSlot(from.AddDays(2), from.AddDays(3), 11);
        var slot2 = new TimeSlot(from.AddDays(1), from.AddDays(2), 22);
        var slot3 = new TimeSlot(from.AddDays(0), from.AddDays(1), 33);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot1);
        sut.ApplySlot(slot2);
        sut.ApplySlot(slot3);

        var data = sut.ToArray();

        Assert.Equal(3, sut.Count);
        Assert.Equal(33, data[0].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[0].Duration());
        Assert.Equal(22, data[1].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[1].Duration());
        Assert.Equal(11, data[2].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[2].Duration());
    }

    [Fact]
    public void Test7()
    {
        // From:         [    0    ]
        // Apply:        |      [  11  ]
        // Apply:    [  22  ]      |
        // Expected:     [22| 0 |11]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot1 = new TimeSlot(from.AddDays(2), from.AddDays(4), 11);
        var slot2 = new TimeSlot(from.AddDays(-1), from.AddDays(1), 22);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot1);
        sut.ApplySlot(slot2);

        var data = sut.ToArray();

        Assert.Equal(3, sut.Count);
        Assert.Equal(22, data[0].Value);
        Assert.Equal(from, data[0].From);
        Assert.Equal(TimeSpan.FromDays(1), data[0].Duration());
        Assert.Equal(0, data[1].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[1].Duration());
        Assert.Equal(11, data[2].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[2].Duration());
        Assert.Equal(to, data[2].To);
    }

    [Fact]
    public void TestEarlyReturnBefore()
    {
        // From:              [ 0 ]
        // Apply:      [ 11 ] |   |
        // Expected:          [ 0 ]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot = new TimeSlot(from.AddDays(-4), from.AddDays(-2), 11);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot);

        var data = sut.ToArray();

        Assert.Equal(1, sut.Count);
        Assert.Equal(0, data[0].Value);
    }

    [Fact]
    public void TestEarlyReturnAfter()
    {
        // From:      [ 0 ]
        // Apply:            [ 11 ]
        // Expected:  [ 0 ]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(2);

        var slot = new Slot<DateTime>(from.AddDays(4), from.AddDays(6), 11);

        var sut = new Slots<DateTime>(from, to, 0);
        sut.ApplySlot(slot);

        var data = sut.ToArray();

        Assert.Equal(1, sut.Count);
        Assert.Equal(0, data[0].Value);
    }

    [Fact]
    public void TestBigInsert()
    {
        // From:           [        ]
        // Apply:      [       11        ]
        // Expected:       [   11   ]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot = new Slot<DateTime>(from.AddDays(-2), from.AddDays(5), 11);

        var sut = new Slots<DateTime>(from, to, 0);
        sut.ApplySlot(slot);

        var data = sut.ToArray();

        Assert.Equal(1, sut.Count);
        Assert.Equal(11, data[0].Value);
    }

    private class CustomSlot: ISlot<DateTime>
    {
        public CustomSlot(DateTime fromUtc, DateTime toUtc, int value)
        {
            From = fromUtc;
            To = toUtc;
            Value = value;
        }

        public DateTime From { get; }
        public DateTime To { get; }
        public int Value { get; }
    }

    [Fact]
    public void TestForcingToRecordSlots()
    {
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot = new CustomSlot(from.AddDays(0), from.AddDays(1), 11);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot);

        var data = sut.ToArray();

        Assert.Equal(2, sut.Count);
        Assert.Equal(11, data[0].Value);
        Assert.Equal(0, data[1].Value);
        Assert.True(data[0] is Slot<DateTime>);
        Assert.True(data[1] is Slot<DateTime>);
    }
}
