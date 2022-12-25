namespace Sloti.Util;

public static class Utc
{
    public static DateTime Create(int year, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0)
    {
        return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
    }

    public static DateTime CurrentYear()
    {
        return Create(DateTime.UtcNow.Year);
    }

    public static DateTime CurrentMonth()
    {
        var utcNow = DateTime.UtcNow;
        return Create(utcNow.Year, utcNow.Month);
    }

    public static DateTime CurrentDay()
    {
        var utcNow = DateTime.UtcNow;
        return Create(utcNow.Year, utcNow.Month, utcNow.Day);
    }

    public static DateTime CurrentHour()
    {
        var utcNow = DateTime.UtcNow;
        return Create(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour);
    }

    public static DateTime CurrentMinute()
    {
        var utcNow = DateTime.UtcNow;
        return Create(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute);
    }

    public static DateTime CurrentSecond()
    {
        var utcNow = DateTime.UtcNow;
        return Create(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second);
    }
}
