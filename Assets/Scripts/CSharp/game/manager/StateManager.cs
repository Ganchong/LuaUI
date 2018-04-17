using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using System;
/// <summary>
/// 状态管理器
/// </summary>
public class StateManager : Singleton<StateManager> {
	
	/** 当前状态 */
	StateBase curState;
	/** 状态列表 */
	List<StateBase> states = null;

	/** 启动状态 */
	public void LauncherState<T> () where T: StateBase, new()
	{
		if (curState != null)
			curState.Exit ();
		curState = GetState<T> ();
		curState.Enter ();
	}
	/** 执行Lua方法 */
	public void DoLuaFunction(string funcName)
	{
		if(curState==null){
			Debug.LogWarning("attempt to call a null state");
			return;
		}
		LuaFunction func = curState.GetLuaFunction(funcName);
		if(func==null){
			Debug.LogWarning("attempt to call a null luaFunction");
			return;
		}
		func.Call();
	}
	/** 执行Lua方法 */
	public void DoLuaFunction(string funcName,string msg,Action callback,bool changeBack = false)
	{
		if(curState==null){
			Debug.LogWarning("attempt to call a null state");
			return;
		}
		LuaFunction func = curState.GetLuaFunction(funcName);
		if(func==null){
			Debug.LogWarning("attempt to call a null luaFunction");
			return;
		}
		func.Call<string,Action,bool>(msg,callback,changeBack);
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
