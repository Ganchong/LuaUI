using System;
using UnityEngine;
using LuaInterface;

/// <summary>
/// 状态基类
/// </summary>
public class StateBase {

	/** 进入状态 */
	public virtual void Enter()
	{
		
	}
	/** 执行Lua方法 */
	public virtual LuaFunction GetLuaFunction(string funcName)
	{
		return null;
	}

	/** 更新状态 */
	public virtual void Update()
	{
		
	}
	/** 退出状态 */
	public virtual void Exit()
	{
		
	}

}
