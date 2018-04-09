using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
/// <summary>
/// IOS帮助工具
/// </summary>
public class IOSHelper
{
	/* static methods */
	#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern void _transmit(int port,string json);
	[DllImport("__Internal")]
	private static extern string _access(int port,string json);
	[DllImport("__Internal")]
	private static extern void _initSDK();

	/** 请求 */
	/** 调用SDK方法 */
	public static void onFuncCall(int port,string json)
	{
		Log.debug("onFuncCall calling...port=" + port + ",json="+json);
		_transmit(port,json);
	}

	/** 有返回的方法 */
	public static string platformHelper(int port, string json)
	{
		Log.debug("platformHelper calling...port=" + port + ",json="+json);
		return _access(port,json);
	}
	/** 初始化 */
	public static void initSDK()
	{
		_initSDK();
	}
	#endif
}