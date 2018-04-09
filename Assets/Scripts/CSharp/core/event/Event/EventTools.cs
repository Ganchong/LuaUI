using System;
using System.Collections.Generic;

/// <summary>
/// 事件工具
/// </summary>
public static class EventTools
{
	/* static methods */
	/** 触发事件 */
	public static void SendEvent(this IEventSender sender,int type)
	{
		EventCenter.Instance.TriggerEvent(type);
	}
	/** 触发事件 */
	public static void SendEvent<T>(this IEventSender sender,int type,T t)
	{
		EventCenter.Instance.TriggerEvent<T>(type,t);
	}

	/** 触发事件 */
	public static void SendEvent<T1,T2>(this IEventSender sender,int type,T1 t1,T2 t2)
	{
		EventCenter.Instance.TriggerEvent<T1,T2> (type,t1,t2);
	}

	/** 触发事件 */
	public static void SendEvent<T1,T2,T3>(this IEventSender sender,int type,T1 t1,T2 t2, T3 t3)
	{
		EventCenter.Instance.TriggerEvent<T1,T2,T3> (type, t1,t2,t3);
	}

	/** 注册事件(此方法不支持取消注册，若要取消只能取消注册全部) */
	public static void Register(this IEventRegister register,int type,Action action)
	{
		if(_register (register,type,action)) EventCenter.Instance.Register (type, action);
	}

	/** 注册事件(此方法不支持取消注册，若要取消只能取消注册全部) */
	public static void Register<T>(this IEventRegister register,int type,Action<T> action)
	{
		if(_register (register,type,action)) EventCenter.Instance.Register<T> (type, action);
	}

	/** 注册事件(此方法不支持取消注册，若要取消只能取消注册全部) */
	public static void Register<T1,T2>(this IEventRegister register,int type,Action<T1,T2> action)
	{
		if(_register (register,type,action)) EventCenter.Instance.Register<T1,T2> (type, action);
	}

	/** 注册事件(此方法不支持取消注册，若要取消只能取消注册全部) */
	public static void Register<T1,T2,T3>(this IEventRegister register,int type,Action<T1,T2,T3> action)
	{
		if(_register (register,type,action)) EventCenter.Instance.Register<T1,T2,T3> (type, action);
	}

	private static bool _register(IEventRegister register,int type,Delegate action){
		if (register.eventMap == null) register.eventMap = new Dictionary<int, List<Delegate>> ();
		List<Delegate> list;
		if (register.eventMap.TryGetValue (type, out list)) 
		{
			if(list.Contains(action))
			{
#if UNITY_EDITOR
				UnityEngine.Debug.LogError("same callBack add");
#endif
				try
				{
					throw new Exception("same callBack add");
				}
				catch(Exception e)
				{
					//ClientErrorHttpPort.Instance.access(ClientErrorHttpPort.SAMEEVENT,e.ToString());
				}
				return false;
			}
			list.Add(action);
			return true;
		}
		else 
		{
			list = new List<Delegate> ();
			list.Add (action);
			register.eventMap.Add (type, list);
		}
		return true;
	}

	/** 取消注册事件 */
	public static void UnRegister(this IEventRegister register,int type,Delegate action)
	{
		register.eventMap [type].Remove(action);
		EventCenter.Instance.Unregister(type, action);
	}
	/** 取消所有注册事件 */
	public static void UnRegisterAll(this IEventRegister register)
	{
		if (register.eventMap == null) return;
		foreach (var pair in register.eventMap) 
		{
			foreach (var each in pair.Value) {
				EventCenter.Instance.Unregister(pair.Key,each);
			}
		}
		register.eventMap.Clear ();
	}
}