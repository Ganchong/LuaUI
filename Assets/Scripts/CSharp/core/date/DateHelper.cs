using System;

/// <summary>
/// 日期帮助器
/// </summary>
public static class DateHelper
{
	/** 将时间转化为指定格式 */
	public static string toString (int seconds, string format)
	{
		return TimeHelper.GetDateTime (seconds).ToString (format);
	}
	/** 是否在同一天 */
	public static bool isSameDay (int secondTime1, int secondTime2)
	{
		DateTime dateTime1 = TimeHelper.GetDateTime (secondTime1);
		int year1 = dateTime1.Year;
		int day1 = dateTime1.DayOfYear;
		DateTime dateTime2 = TimeHelper.GetDateTime (secondTime2);
		int year2 = dateTime2.Year;
		int day2 = dateTime2.DayOfYear;
		return year1 == year2 && day1 == day2;
	}
	/** 获得零点时间 */
	public static int getZeroTime(int time)
	{
		DateTime dateTime = TimeHelper.GetDateTime (time);
		dateTime=dateTime.AddHours (-dateTime.Hour);
		dateTime=dateTime.AddMinutes (-dateTime.Minute);
		dateTime=dateTime.AddSeconds (-dateTime.Second);
		return TimeHelper.MillisToSecond(TimeHelper.GetMillis(dateTime));
	}
}