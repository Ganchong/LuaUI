using System;
using System.Collections;

/// <summary>
/// 单例类
/// </summary>
public class Singleton<T> where T : class,new()
{
	/* static fields */
	/** 单例 */
	private static T instance;

	/* static methods */
	/** 获取单例 */
	public static T Instance
	{
		get
		{
			if (Singleton<T>.instance == null)
			{
				Singleton<T>.instance = Activator.CreateInstance<T>();
			}
			return Singleton<T>.instance;
		}
	}
	/** 销毁单例 */
	public static void DestroyInstance()
	{
		if (Singleton<T>.instance != null)
		{
			Singleton<T>.instance = null;
		}
	}
}
