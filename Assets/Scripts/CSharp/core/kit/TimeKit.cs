/**
 * Coypright 2013 by 刘耀鑫<xiney@youkia.com>.
 */
using System;

/**
 * @author 刘耀鑫
 */
public class TimeKit
{

	/* static fields */
	private static readonly DateTime initTime = new DateTime (1970, 1, 1, 08, 00, 00);
	/** 时间的修正值 */
	private static long timeFix;

	/* static methods */
	/** 得到校正后时间，毫秒为单位 */
	public static long getMillisTime ()
	{
		return currentTimeMillis () - timeFix;
	}
	/** 得到校正后时间，秒为单位 */
	public static int getSecondTime ()
	{
		return (int)((currentTimeMillis () - timeFix) / 1000);
	}
	/** 校正时间 */
	public static void resetTime (long time)
	{
		timeFix = currentTimeMillis () - time;
	}
	/** 将指定的毫秒数转换成秒数，毫秒数除1000 */
	public static int timeSecond (long timeMillis)
	{
		return (int)(timeMillis / 1000);
	}
	/** 将指定的秒数转换成毫秒数，秒数乘1000 */
	public static long timeMillis (long timeSecond)
	{
		return timeSecond * 1000;
	}
	/** 得到DateTime */
	public static DateTime getDateTime (int time)
	{
		return initTime.AddSeconds (time);
	}
	/** 得到DateTime */
	public static DateTime getDateTime (long time)
	{
		return initTime.AddMilliseconds (time);
	}
	/// <summary>
	/// 通过指定datatime得到当前时间的时间戳
	/// </summary>
	public static long getTimeMillis (DateTime date)
	{
		return Convert.ToInt64 (date.Subtract (initTime).TotalMilliseconds);
	}
	private static long currentTimeMillis ()
	{
		return Convert.ToInt64 (DateTime.UtcNow.Subtract (initTime).TotalMilliseconds);
	}

	public static long currentTimeMillis (DateTime value)
	{
		return Convert.ToInt64 (value.Subtract (initTime).TotalMilliseconds);
	}
	/// <summary>
	/// 获取星期数（1-7）
	/// </summary>
	public static int getWeekDay()
	{
		long time = getMillisTime();
		DateTime dateTime = getDateTime(time);
		DayOfWeek week=dateTime.DayOfWeek;
		return (((int)week-1)<0?6:((int)week-1))+1;
	}


	/* constructors */
	private TimeKit ()
	{
	}

}
