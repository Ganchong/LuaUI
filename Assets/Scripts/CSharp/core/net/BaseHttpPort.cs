using System;
using System.Collections;
/// <summary>
/// Http请求
/// </summary>
public class BaseHttpPort<T> where T : class,new()
{
	/* static fields */
	/** 单例 */
	private static T instance;

	/* fields */

	/* methods */
	/** 请求方法 */
	public void access(string url)
	{
		MaskWindow.lockNet ();
		url = HttpPathHelper.HttpHost + url;
		HttpConnect.access (url, (ans,result) => {
			if(ans == HttpConnect.OK)
			{
				string[] splits = {":"};
				if (result.StartsWith ("ok:")) {
					string[] strs = result.Split (splits, StringSplitOptions.None);
					readData(strs);
				} else {
					string errorCode = result.Split (splits, StringSplitOptions.None) [1];
					readException(errorCode);
				}
			}
			else
			{
				connectException();
				MaskWindow.unLockUI();
			}
			MaskWindow.unLockNet();
		});
	}

	/** 读取正常 */
	protected virtual void readData(string[] results)
	{

	}

	/** 读取异常 */
	protected virtual void readException(string result)
	{
//		Tips.Show (result);
		MaskWindow.unLockUI();
	}
	/** 网络异常处理 */
	protected virtual void connectException()
	{
//		Tips.Show("@loginWindow_3");
	}

	/** 获取单例 */
	public static T Instance
	{
		get
		{
			if (BaseHttpPort<T>.instance == null)
			{
				BaseHttpPort<T>.instance = Activator.CreateInstance<T>();
			}
			return BaseHttpPort<T>.instance;
		}
	}
	/** 销毁单例 */
	public static void DestroyInstance()
	{
		if (BaseHttpPort<T>.instance != null)
		{
			BaseHttpPort<T>.instance = null;
		}
	}
}

