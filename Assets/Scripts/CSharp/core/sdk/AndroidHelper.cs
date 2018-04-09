using System;
using UnityEngine;
/// <summary>
/// SDK工具
/// 汪松民
/// </summary>
public class AndroidHelper
{
	/* static fields */
	#if UNITY_ANDROID || UNITY_STANDALONE_WIN
	/** SDK服务 */
	private const string SDK_JAVA_CLASS = "com.youmi.sdk.SDKService";
	/** 变量名称 */
	private const string FIELDNAME = "sdkService";
	/** 方法名称 */
	private const string TRANSMIT = "transmit",ACCESS="access",INITSDK="initSDK";

	/** 类型常量 */
	private const string LOGINSUCCESS = "loginSuccess";

	/* fields */

	/* methods */
	/** 初始化SDK */
	public static void initSDK()
	{
		Log.debug("initSDK");
		try
		{
			using (AndroidJavaClass javaClass = new AndroidJavaClass(SDK_JAVA_CLASS))
			{
				using (AndroidJavaObject javaObject = javaClass.GetStatic<AndroidJavaObject>(FIELDNAME))
				{
					javaObject.Call(INITSDK);
				}
			}
		}
		catch(Exception ex)
		{
			Debug.LogError(ex.ToString());
		}
	}
	/** 调用SDK方法 */
	public static void onFuncCall(int port,string json)
	{
		Log.debug("onFuncCall calling...port=" + port + ",json="+json);
		try
		{
			using (AndroidJavaClass javaClass = new AndroidJavaClass(SDK_JAVA_CLASS))
			{
				using (AndroidJavaObject javaObject = javaClass.GetStatic<AndroidJavaObject>(FIELDNAME))
				{
					javaObject.Call(TRANSMIT,port, json);
				}
			}
		}
		catch(Exception ex)
		{
			Debug.LogError(ex.ToString());
		}
	}
	/** 有返回的方法 */
	public static string platformHelper(int port, string json)
	{
		Log.debug("platformHelper calling...port=" + port + ",json="+json);
		try
		{
			using (AndroidJavaClass javaClass = new AndroidJavaClass(SDK_JAVA_CLASS))
			{
				using (AndroidJavaObject javaObject = javaClass.GetStatic<AndroidJavaObject>(FIELDNAME))
				{
					return javaObject.Call<string>(ACCESS,port,json);
				}
			}
		}
		catch(Exception ex)
		{
			Debug.LogError(ex.ToString());
			return string.Empty;
		}
	}
	/** SDK发过来的 */
	public static void OnGetMessage (string msg)
	{

	}

	public static void update()
	{

	}
#endif
}

