using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 状态管理器
/// </summary>
public class StateManager : Singleton<StateManager> {
	
	/** 当前状态 */
	StateBase curState;
	/** 状态列表 */
	List<StateBase> states = null;

	/** 切换状态 */
	public void ChangeState<T> () where T: StateBase, new()
	{
		if (curState != null)
			curState.Exit ();
		curState = GetState<T> ();
		curState.Enter ();
	}

	/** 获取状态 */
	private T GetState<T> () where T : StateBase, new()
	{
		if(states==null)
			states = new List<StateBase>();
		T t = null;
		foreach (var s in states) {
			t = s as T;
			if (t != null) {
				return t;
			}
		}
		t = new T ();
		states.Add (t);
		return t;
	}
}
