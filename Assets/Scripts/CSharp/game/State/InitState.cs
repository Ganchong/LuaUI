using UnityEngine;
using System;
using LuaInterface;
using LuaFramework.Core;

/// <summary>
/// 初始化状态
/// </summary>
public class InitState : StateBase
{
	/** 初始化资源方法注入接口 */
	public static Action<CallBack> InitResFunc = null;

	public override void Enter ()
	{
		base.Enter ();
		if (InitResFunc != null) {
			InitResFunc (() => {
				LuaManager.Instance.LuaStart ();
				LuaManager.Instance.DoFile ("Main.lua");
			});
		}
	}
	/** 获取Lua方法 */
	public override LuaFunction GetLuaFunction (string funcName)
	{
		return LuaManager.Instance.GetLuaFunction(funcName);
	}

	public override void Update ()
	{
		base.Update ();
	}

	public override void Exit ()
	{
		base.Exit ();
	}
}
