using Sloti;
using Sloti.Util;

namespace UnitTests;

public class TimeslotMergeApplyTests
{
    [Fact]
    public void MergeTest1()
    {
        // From:         [        ]
        // Apply:        [11]     |
        // Apply:        |  [33]  |
        // Apply:        |     [33]
        // Expected:     [11|  33 ]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot1 = new TimeSlot(from.AddDays(0), from.AddDays(1), 11);
        var slot2 = new TimeSlot(from.AddDays(1), from.AddDays(2), 33);
        var slot3 = new TimeSlot(from.AddDays(2), from.AddDays(3), 33);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot1);
        sut.ApplySlot(slot2);
        sut.ApplySlot(slot3);

        var data = sut.ToArray();

        Assert.Equal(2, sut.Count);
        Assert.Equal(11, data[0].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[0].Duration());
        Assert.Equal(33, data[1].Value);
        Assert.Equal(TimeSpan.FromDays(2), data[1].Duration());
    }

    [Fact]
    public void MergeTest2()
    {
        // From:         [        ]
        // Apply:        [11]     |
        // Apply:        |  [22]  |
        // Apply:        |     [33]
        // Apply:        [11]     |
        // Apply:        [  11 ]  |
        // Expected:     [   33   ]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot1 = new TimeSlot(from.AddDays(0), from.AddDays(1), 11);
        var slot2 = new TimeSlot(from.AddDays(1), from.AddDays(2), 22);
        var slot3 = new TimeSlot(from.AddDays(2), from.AddDays(3), 33);
        var slot4 = new TimeSlot(from.AddDays(0), from.AddDays(1), 11);
        var slot5 = new TimeSlot(from.AddDays(0), from.AddDays(2), 11);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot1);
        sut.ApplySlot(slot2);
        sut.ApplySlot(slot3);
        sut.ApplySlot(slot4);
        sut.ApplySlot(slot5);

        var data = sut.ToArray();

        Assert.Equal(1, sut.Count);
        Assert.Equal(33, data[0].Value);
        Assert.Equal(TimeSpan.FromDays(3), data[0].Duration());
    }

    [Fact]
    public void TripleMergeTest()
    {
        // From:         [        ]
        // Apply:        [11]     |
        // Apply:        |     [11]
        // Apply:        |  [11]  |
        // Expected:     [   11   ]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot1 = new TimeSlot(from.AddDays(0), from.AddDays(1), 11);
        var slot2 = new TimeSlot(from.AddDays(2), from.AddDays(3), 11);
        var slot3 = new TimeSlot(from.AddDays(1), from.AddDays(2), 11);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot1);
        sut.ApplySlot(slot2);
        sut.ApplySlot(slot3);

        var data = sut.ToArray();

        Assert.Equal(1, sut.Count);
        Assert.Equal(11, data[0].Value);
        Assert.Equal(TimeSpan.FromDays(3), data[0].Duration());
    }

    [Fact]
    public void LeftMergeTest()
    {
        // From:         [        ]
        // Apply:        [11]     |
        // Apply:        |  [11]  |
        // Expected:     [ 11  |  ]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot1 = new TimeSlot(from.AddDays(0), from.AddDays(1), 11);
        var slot2 = new TimeSlot(from.AddDays(1), from.AddDays(2), 11);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot1);
        sut.ApplySlot(slot2);

        var data = sut.ToArray();

        Assert.Equal(2, sut.Count);
        Assert.Equal(11, data[0].Value);
        Assert.Equal(TimeSpan.FromDays(2), data[0].Duration());
        Assert.Equal(0, data[1].Value);
        Assert.Equal(TimeSpan.FromDays(1), data[1].Duration());
    }

    [Fact]
    public void DoubleLeftMergeTest()
    {
        // From:         [        ]
        // Apply:        [22]     |
        // Apply:        |  [11]  |
        // Apply:        |     [11]
        // Apply:        |  [  11 ]
        // Expected:     [   22   ]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(3);

        var slot1 = new TimeSlot(from.AddDays(0), from.AddDays(1), 22);
        var slot2 = new TimeSlot(from.AddDays(1), from.AddDays(2), 11);
        var slot3 = new TimeSlot(from.AddDays(2), from.AddDays(3), 11);
        var slot4 = new TimeSlot(from.AddDays(1), from.AddDays(3), 11);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot1);
        sut.ApplySlot(slot2);
        sut.ApplySlot(slot3);
        sut.ApplySlot(slot4);

        var data = sut.ToArray();

        Assert.Equal(1, sut.Count);
        Assert.Equal(22, data[0].Value);
        Assert.Equal(TimeSpan.FromDays(3), data[0].Duration());
    }

    [Fact]
    public void ExtraTest()
    {
        // From:       [               0     ]
        // Apply:      [    1    ]
        // Apply:   [   2   ]
        // Apply:         [     3     ]
        // Apply:                         [  9 ]
        // Apply:   [             0               ]
        // Expected:   [3 |6|  4 | 3  | 0 | 9  ]
        var from = Utc.CurrentDay();
        var to   = from.AddDays(8);

        var slot1 = new TimeSlot(from.AddDays(0), from.AddDays(4), 1);
        var slot2 = new TimeSlot(from.AddDays(-2), from.AddDays(2), 2);
        var slot3 = new TimeSlot(from.AddDays(1), from.AddDays(5), 3);
        var slot4 = new TimeSlot(from.AddDays(7), from.AddDays(8), 9);
        var slot5 = new TimeSlot(from.AddDays(-1), from.AddDays(9), 0);

        var sut = new Timeslots(from, to, 0);
        sut.ApplySlot(slot1);
        sut.ApplySlot(slot2);
        sut.ApplySlot(slot3);
        sut.ApplySlot(slot4);
        sut.ApplySlot(slot5);

        var data = sut.ToArray();

        Assert.Equal(6, sut.Count);
        Assert.Equal(3, data[0].Value);
        Assert.Equal(6, data[1].Value);
        Assert.Equal(4, data[2].Value);
        Assert.Equal(3, data[3].Value);
        Assert.Equal(0, data[4].Value);
        Assert.Equal(9, data[5].Value);
    }

}
