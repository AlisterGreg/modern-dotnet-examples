namespace TimeProviders;

#region fastDateTime
public static class FastDateTime
{
	private static readonly TimeSpan LocalUtcOffset;

	public static DateTime Now => DateTime.SpecifyKind(DateTime.UtcNow + LocalUtcOffset, DateTimeKind.Local);
	public static DateTime UtcNow => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

	static FastDateTime()
	{
		LocalUtcOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
	}
}
#endregion

#region variant one
public interface IClock_1
{
	DateTime Today();
	DateTime Now();
	DateTime UtcNow();
}

public sealed class Clock_1 : IClock_1
{
	public DateTime Today() => DateTime.Today;
	public DateTime Now() => FastDateTime.Now;
	public DateTime UtcNow()
	{
		return DateTime.UtcNow;

	}
}
#endregion
#region variant two
public interface IDateTimeGetter
{
	DateTime UtcNow();
}

public class DateTimeGetter : IDateTimeGetter
{
	public DateTime UtcNow()
	{
		return DateTime.UtcNow;
	}
}
#endregion
#region variant three
public interface IClock_2
{
	DateTime Now();
	DateTimeOffset UtcNow();
}

public class Clock_2 : IClock_2
{
	public DateTime Now() => FastDateTime.Now;

	public DateTimeOffset UtcNow() => DateTimeOffset.UtcNow;
}
#endregion
#region variant four
public sealed class FakeDateTime : IDateTimeGetter
{
	private DateTime _utcNow;
	private DateTime _now;

	public FakeDateTime()
	{
		_utcNow = new DateTime(2023, 01, 01);
		_now = new DateTime(2023, 01, 01);
	}

	public FakeDateTime(DateTime dateTime)
	{
		_utcNow = dateTime;
		_now = dateTime;
	}

	public void AdvanceTime(TimeSpan time)
	{
		_utcNow += time;
		_now += time;
	}

	public DateTime UtcNow() => _utcNow;

	public DateTime Now() => _now;
}
#endregion