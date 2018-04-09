using System;
using System.Net;
using System.IO;
using System.Collections;
using UnityEngine;
using System.Text;
/// <summary>
/// Http连接
/// 汪松民
/// </summary>
public class HttpConnect:IDisposable
{
	/* static fields */
	/** 类型常量OK成功，ERROR失败 */
	public const int OK = 1, ERROR = 2;
	/** https协议头 */
	public const string HTTP="http://",HTTPS="https://";
	
	/* fields */
	/** 地址 */
	private string url;
	/** 下载器 */
	private WWW www;
	/** 定时器 */
	private Timer timer;
	/** 是否超时 */
	private bool timeout;
	/** 回调 */
	private Action<int,string> action;
	
	/* methods */
	/** 构造方法 */
	private HttpConnect()
	{
	}

	/** 请求方法 */
	public static HttpConnect access(string url,Action<int,string> action)
	{
		HttpConnect httpConnect = new HttpConnect();
#if UNITY_ANDROID || UNITY_EDITOR
		url=url.Replace(HTTPS,HTTP);
#endif
		CoroutineCenter.Instance.excute(httpConnect.connect(url,action));
		return httpConnect;
	}

	private IEnumerator connect (string url, Action<int,string> action)
	{
		MaskWindow.lockNet ();
		this.action = action;
		this.url = url;
		if (string.IsNullOrEmpty (this.url)) {
			throw new ArgumentNullException (this + " url is null");
		}
		timer=TimerManager.Instance.getTimer (1000L * DataAccessHandler.TIMEOUT);
		timer.addOnTimer (timeOut);
		timer.start ();
		www = new WWW (this.url);
		yield return www;
		timer.stop ();
		if (timeout) yield break;
		if (string.IsNullOrEmpty (www.error)) {
			string resultStr = www.text;
			action(OK,resultStr);
		} else {
			action(ERROR,this + " www is error," + www.error);
		}
		MaskWindow.unLockNet ();
	}
	/** 超时 */
	public void timeOut()
	{
		timeout = true;
		action(ERROR,this + " www is error, timeout");
		www.Dispose ();
		timer.stop ();
		MaskWindow.unLockNet ();
	}
	
	public void Dispose ()
	{
		if (www != null) {
			www.Dispose ();
		}
	}
}