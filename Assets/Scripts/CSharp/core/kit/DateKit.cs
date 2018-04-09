using System;
using UnityEngine;

public class DateKit
{
	public static string toString (int seconds,bool showDay = true)
	{
//		if (seconds < 0)
//			seconds = 0;
//		int hour = (int)(seconds / 3600);
//		int minute = ((int)(seconds / 60)) % 60;
//		int second = seconds % 60;
//		int day = 0;
//		if(showDay){
//			if(hour>24)day = hour/24;
//			hour -= day*24;
//		}
//		string hourstr = (hour >= 10 ? hour.ToString () + ":" : "0" + hour + ":");
//		string minutestr = (minute >= 10 ? minute.ToString () + ":" : "0" + minute + ":");
//		string secondstr = (second >= 10 ? second.ToString () : "0" + second);
//		return day>0?Language.get("str_147",day,hourstr + minutestr + secondstr):hourstr + minutestr + secondstr;
		return null;
	}

	public static string toString (int seconds, string format)
	{
		return TimeKit.getDateTime (seconds).ToString (format);
	}
	/** 是否在同一天 */
	public static bool isInSameDay (int secondTime1, int secondTime2)
	{
		DateTime dateTime1 = TimeKit.getDateTime (secondTime1);
		int year1 = dateTime1.Year;
		int day1 = dateTime1.DayOfYear;
		DateTime dateTime2 = TimeKit.getDateTime (secondTime2);
		int year2 = dateTime2.Year;
		int day2 = dateTime2.DayOfYear;
		return year1 == year2 && day1 == day2;
	}
	/** 是否在同一小时 */
	public static bool isInSameHour(int secondTime1, int secondTime2)
	{
		DateTime dateTime1 = TimeKit.getDateTime (secondTime1);
		int year1 = dateTime1.Year;
		int day1 = dateTime1.DayOfYear;
		int hour1 = dateTime1.Hour;
		DateTime dateTime2 = TimeKit.getDateTime (secondTime2);
		int year2 = dateTime2.Year;
		int day2 = dateTime2.DayOfYear;
		int hour2 = dateTime2.Hour;

		return year1 == year2 && day1 == day2 && hour1 == hour2;
	}

	/** 获得零点时间 */
	public static int getZeroTime(int time)
	{
		DateTime dateTime=TimeKit.getDateTime(time);
		dateTime=dateTime.AddHours (-dateTime.Hour);
		dateTime=dateTime.AddMinutes (-dateTime.Minute);
		dateTime=dateTime.AddSeconds (-dateTime.Second);
		return TimeKit.timeSecond(TimeKit.getTimeMillis(dateTime));
	}
	
	public static string getLastTime (int nowTime, int lastTime)
	{
//		string dateDiff = "";
//		try {
//			TimeSpan ts1 = new TimeSpan (TimeKit.getDateTime (nowTime).Ticks);
//			TimeSpan ts2 = new TimeSpan (TimeKit.getDateTime (lastTime).Ticks);
//			TimeSpan ts = ts1.Subtract (ts2).Duration ();
//
//			if (ts.Days >= 365) {
//				return Language.get ("time6", (ts.Days / 365).ToString ());
//			} else if (ts.Days >= 30 && ts.Days < 365) {
//				return Language.get ("time5", (ts.Days / 30).ToString ());
//			} else if (ts.Days >= 1 && ts.Days < 30) {
//				return Language.get ("time4", (ts.Days / 1).ToString ());
//			} else if (ts.Days < 1 && ts.Hours >= 1) {
//				return Language.get ("time3", (ts.Hours / 1).ToString ());
//			} else if (ts.Hours < 1 && ts.Minutes >= 1) {
//				return Language.get ("time2", (ts.Minutes / 1).ToString ());
//			} else if (ts.Minutes < 1) {
//				return Language.get ("time1", (ts.Seconds / 1).ToString ());
//			}
//		} catch(Exception ex) {
//			Debug.LogError(ex);
//		}
//		return dateDiff;
		return null;
	}
	public static string getLeftTime (int nowTime, int lastTime)
	{
		string dateDiff = "";
//		try {
//			TimeSpan ts1 = new TimeSpan (TimeKit.getDateTime (nowTime).Ticks);
//			TimeSpan ts2 = new TimeSpan (TimeKit.getDateTime (lastTime).Ticks);
//			TimeSpan ts = ts1.Subtract (ts2).Duration ();
//			
//			if (ts.Days >= 30 && ts.Days < 365) {
//				return Language.get ("leftTime5", (ts.Days / 30).ToString ());
//			} else if (ts.Days >= 1 && ts.Days < 30) {
//				return Language.get ("leftTime4", (ts.Days / 1).ToString ());
//			} else if (ts.Days < 1 && ts.Hours >= 1) {
//				return Language.get ("leftTime3", (ts.Hours / 1).ToString ());
//			} else if (ts.Hours < 1 && ts.Minutes >= 1) {
//				return Language.get ("leftTime2", (ts.Minutes / 1).ToString ());
//			} else if (ts.Minutes < 1) {
//				return Language.get ("leftTime1", (ts.Seconds / 1).ToString ());
//			}
//		} catch(Exception ex) {
//			Debug.LogError(ex);
//		}
		return dateDiff;
	}
}

