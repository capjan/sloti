using Sloti.Util;

namespace UnitTests;

public class UtcTest
{
    [Fact]
    public void KindTest()
    {
        // Utc
        Assert.Equal(DateTimeKind.Utc, DateTime.UtcNow.Kind);
        Assert.Equal(DateTimeKind.Utc, DateTime.UtcNow.AddHours(2).Kind);
        Assert.Equal(DateTimeKind.Utc, DateTime.UtcNow.AddYears(2).Kind);
        Assert.Equal(DateTimeKind.Utc, new DateTime(2022, 1,1, 0,0, 0, DateTimeKind.Utc).Kind);

        // Local
        Assert.Equal(DateTimeKind.Local, DateTime.Now.Kind);

        // Unspecified
        Assert.Equal(DateTimeKind.Unspecified, new DateTime(2022, 1, 1).Kind);
    }

    [Fact]
    public void ConvenienceFactoryMethodTests()
    {
        // hopefully it runs fast enough to run in a single test
        var month = Utc.CurrentMonth();
        var day = Utc.CurrentDay();
        var hour = Utc.CurrentHour();
        var minute = Utc.CurrentMinute();
        var second = Utc.CurrentSecond();
        Assert.Equal(month.Month, day.Month);
        Assert.Equal(day.Day, hour.Day);
        Assert.Equal(hour.Hour, minute.Hour);
        Assert.Equal(minute.Minute, second.Minute);
    }


}
