using System;

/// <summary>
/// 时间工具
/// </summary>
public static class TimeHelper
{
	/* static fields */
	/** 时间纪元 */
	private static readonly DateTime initTime = new DateTime (1970, 1, 1, 0, 0, 0);
	/** 时间的修正值 */
	private static long timeFix;

	/* static methods */
	/** 得到校正后时间，秒为单位 */
	public static long GetMillis (this object obj)
	{
		return GetMillis ();
	}
	/** 得到校正后时间，毫秒为单位 */
	public static long GetMillis ()
	{
		return CurrentMillis () - timeFix;
	}
	/** 得到校正后时间，秒为单位 */
	public static int GetSecond (this object obj)
	{
		return GetSecond ();
	}
	public static int GetSecond ()
	{
		return (int)((CurrentMillis () - timeFix) / 1000);
	}
	/** 通过指定datatime得到当前时间的时间戳 */
	public static int GetSecond(DateTime dateTime)
	{
		return (int)(GetMillis (dateTime) / 1000);
	}
	/** 校正时间 */
	public static void Reset (long time)
	{
		timeFix = CurrentMillis () - time;
	}
	/** 将指定的毫秒数转换成秒数，毫秒数除1000 */
	public static int MillisToSecond (long timeMillis)
	{
		return (int)(timeMillis / 1000);
	}
	/** 将指定的秒数转换成毫秒数，秒数乘1000 */
	public static long SecondToMillis (long timeSecond)
	{
		return timeSecond * 1000;
	}
	/** 得到DateTime */
	public static DateTime GetDateTime (int time)
	{
		return initTime.AddSeconds (time);
	}
	/** 得到DateTime */
	public static DateTime GetDateTime (long time)
	{
		return initTime.AddMilliseconds (time);
	}
	/** 通过指定datatime得到当前时间的时间戳 */
	public static long GetMillis (DateTime date)
	{
		return Convert.ToInt64 (date.Subtract (initTime).TotalMilliseconds);
	}
	/** 当前时间长度 */
	private static long CurrentMillis ()
	{
		return Convert.ToInt64 (DateTime.UtcNow.Subtract (initTime).TotalMilliseconds);
	}
	/** 指定时间长度 */
	public static long CurrentMillis (DateTime value)
	{
		return Convert.ToInt64 (value.Subtract (initTime).TotalMilliseconds);
	}
}