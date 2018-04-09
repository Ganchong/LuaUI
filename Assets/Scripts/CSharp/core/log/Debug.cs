using System;
using UnityEngine;
using System.Collections.Generic;

#if !UNITY_EDITOR
public static class Debug
{
	/* static fields */
	/* 是否打印日志 */
	public static bool DEBUG=false;
	/** 空 */
	public static string NULL="null";
	/** 异常 */
	public static string EXCEPTION="exception:";
	/** 错误 */
	public static string ERROR="error:";
	/** 警告 */
	public static string WARNING="warning:";

	/* static methods */
	static Debug()
	{
		Application.logMessageReceived += UnityLog;
		Application.logMessageReceivedThreaded += UnityLog;
		AppDomain.CurrentDomain.UnhandledException += UnhandledException;
	}
	public static void UnityLog(string condition,string stackTrace,LogType logType)
	{
		ClientErrorHttpPort.Instance.access (condition, stackTrace);
	}
	public static void UnhandledException(object sender,UnhandledExceptionEventArgs eventArgs)
	{
		ClientErrorHttpPort.Instance.access (sender.GetType().FullName+":"+eventArgs.IsTerminating, eventArgs.ExceptionObject.ToString());
	}
	public static void Log (object message)
	{
		if (!DEBUG) return;
		LogOnWindow (message == null ? NULL : message.ToString ());
	}
	public static void Log (object message,object context)
	{
		if (!DEBUG) return;
		LogOnWindow (message == null ? NULL : message.ToString ());
	}
	public static void LogError (object message)
	{
		if (!DEBUG) return;
		LogOnWindow (ERROR+(message == null ? NULL : message.ToString ()));
	}
	public static void LogError (object message, object context)
	{
		if (!DEBUG) return;
		LogOnWindow (ERROR+(message == null ? NULL : message.ToString ()));
	}
	public static void LogErrorFormat (string format, params object[] args)
	{
		if (!DEBUG) return;
		LogOnWindow (ERROR+string.Format(format,args));
	}
	public static void LogException (Exception exception)
	{
		if (!DEBUG) return;
		LogOnWindow (EXCEPTION+exception);
	}
	public static void LogFormat (string format, params object[] args)
	{
		if (!DEBUG) return;
		LogOnWindow (string.Format(format,args));
	}
	public static void LogWarning (object message)
	{
		if (!DEBUG) return;
		LogOnWindow (WARNING+(message==null?NULL:message.ToString()));
	}
	public static void LogWarning (object message,object context)
	{
		if (!DEBUG) return;
		LogOnWindow (WARNING+(message==null?NULL:message.ToString()));
	}
	public static void LogWarningFormat (string format, params object[] args)
	{
		if (!DEBUG) return;
		LogOnWindow (WARNING+string.Format(format,args));
	}
	public static void DrawLine (Vector3 start, Vector3 end, Color color)
	{

	}
	/** 打印日志 */
	private static void LogOnWindow(string str)
	{
		DebugWindow.Instance.Log (str);
	}
}

public class DebugWindow : SingletonBehaviour<DebugWindow> 
{
	/* static fields */

	/* fields */
	/** 日志 */
	List<string> logs = new List<string>();
	/** 日志窗口大小 */
	Rect windowRect = new Rect(80*Screen.width/960f, 20*Screen.height/640f, 800*Screen.width/960f,560*Screen.height/640f);
	/** 日志窗口开始位置 */
	Vector2 logPos = Vector2.zero;
	/** 是否显示界面 */
	bool show;

	/* methods */
	/** 打印日志 */
	public void Log(string str)
	{
		logs.Add(str);
	}
	/** 显示界面 */
	private void OnGUI()
	{
		GUILayout.Space(40*Screen.width/960f);
		if (GUILayout.Button("SHOW",GUILayout.Width(80*Screen.width/960f),GUILayout.Height(40*Screen.height/640f)))
		{
			show = !show;
		}
		else if (GUILayout.Button("CLEAR",GUILayout.Width(80*Screen.width/960f),GUILayout.Height(40*Screen.height/640f)))
		{
			logs.Clear();
		}
		if(show)
		{
			windowRect=GUILayout.Window(1, windowRect, showLogs, "Debug Error Window");            
		}
	}
	/** 显示日志 */
	private void showLogs(int windowID)
	{
		logPos = GUILayout.BeginScrollView(logPos, false, true, GUILayout.Width(800*Screen.width/960f), GUILayout.Height(560*Screen.height/640f));

		GUILayout.Space(50*Screen.height/640f);
		GUILayout.BeginVertical();

		for(int i=0;i<logs.Count;i++)
		{
			GUILayout.Label(logs[i], GUILayout.Width(800*Screen.width/960f));
		}

		GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}
}
#endif