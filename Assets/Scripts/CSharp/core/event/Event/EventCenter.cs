using System;
using System.Collections.Generic;
/// <summary>
/// 事件中心
/// </summary>
public class EventCenter : Singleton<EventCenter>
{
	/* fields */
	/** 事件列表 */
	Dictionary<int,List<Delegate>> eventMap = new Dictionary<int, List<Delegate>> ();

	/* methods */
	/** 注册 */
	public void Register (int type, Action action)
	{
		register (type,action);
	}

	void register(int type,Delegate del){
		if (eventMap.ContainsKey (type))
			eventMap [type].Add (del);
		else {
			List<Delegate> list = new List<Delegate> ();
			list.Add (del);
			eventMap.Add (type, list);
		}
	}


	public void Register<T>(int type,Action<T> action) {
		register (type,action);
	}

	public void Register<T1,T2>(int type,Action<T1,T2> action) {
		register (type,action);
	}

	public void Register<T1,T2,T3>(int type,Action<T1,T2,T3> action) {
		register (type,action);
	}


	/** 取消注册 */
	public void Unregister (int type,Delegate action)
	{
		eventMap [type].Remove (action);
	}

	/** 触发消息 */
	public void TriggerEvent (int type)
	{
		if (eventMap.ContainsKey (type)) {
			List<Delegate> list = eventMap [type];
			Delegate[] array=list.ToArray();
			foreach (var each in array) {
				(each as Action).Invoke();
			}
		}
	}

	public void TriggerEvent<T>(int type,T t){
		if (eventMap.ContainsKey (type)) {
			List<Delegate> list = eventMap [type];
			Delegate[] array=list.ToArray();
			foreach (var each in array) {
				(each as Action<T>).Invoke(t);
			}
		}
	}

	public void TriggerEvent<T1,T2>(int type,T1 t1,T2 t2){
		if (eventMap.ContainsKey (type)) {
			List<Delegate> list = eventMap [type];
			Delegate[] array=list.ToArray();
			foreach (var each in array) {
				(each as Action<T1,T2>).Invoke(t1,t2);
			}
		}
	}

	public void TriggerEvent<T1,T2,T3>(int type,T1 t1,T2 t2,T3 t3){
		if (eventMap.ContainsKey (type)) {
			List<Delegate> list = eventMap [type];
			Delegate[] array=list.ToArray();
			foreach (var each in array) {
				(each as Action<T1,T2,T3>).Invoke(t1,t2,t3);
			}
		}
	}
}

