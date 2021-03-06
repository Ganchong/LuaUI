using UnityEngine;
using System;

public class Log
{
	public static LogLevel level = LogLevel.debug;
	public static bool useUnityLog = true;

	public static void info (object message)
	{
		if ((level & LogLevel.info_level) != 0) {
			if (useUnityLog)
				Debug.Log ("Info : " + message);
			Console.WriteLine (message);
#if UNITY_EDITOR_WIN
			FileHelper.WriteStringToFile("F://a.txt",message.ToString(),System.IO.FileMode.Append);
#endif
		}
	}

	public static bool isInfoEnable()
	{
		return (level & LogLevel.info_level) != 0;
	}
	
	public static void warning (object message)
	{
		if ((level & LogLevel.warning_level) != 0) {
			if (useUnityLog)
				Debug.Log ("Warning : " + message);
			Console.WriteLine (message);
		}
	}
	
	public static void debug (object message)
	{
		if ((level & LogLevel.debug_level) != 0) {
			if (useUnityLog)
				Debug.Log ("Debug : " + message);
			Console.WriteLine (message);
		}
	}

	public static void error (object message, Exception e)
	{
		if ((level & LogLevel.error_level) != 0) {
			if (useUnityLog) {
				if (message != null) {
					Debug.Log ("Error : " + message);
				}
				if (e != null) {
					Debug.LogError (e);
				}
			}
			if (message != null) {
				Console.WriteLine (message);
			}
			if (e != null) {
				Console.WriteLine (e);
			}
			
		}
	}

	public static void error (object message, Exception e, bool dialog)
	{
		if (dialog) {
			//Alert.show ("error", message.ToString ());
		}
		error (message, e);
	}
}

public enum LogLevel
{
	debug			=	15,
	info 			= 	7,
	warning			=	3,
	error			=	1,
	
	debug_level		=	8,
	info_level		=	4,
	warning_level	=	2,
	error_level		=	1
}
