using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LuaFramework.Core;
using ABSystem;
using LuaInterface;

/// <summary>
/// 登录状态
/// </summary>
public class LoginState : StateBase
{
	/** 初始化资源方法注入接口 */
	public static Action<CallBack> InitResFunc = null;

	public override void Enter ()
	{
		base.Enter ();
		LuaManager.Instance.Clear ();
		if (InitResFunc != null) {
			InitResFunc (() => {
				LuaManager.Instance.LuaStart ();
				LuaManager.Instance.DoFile ("Main.lua");
			});
		}
	}
	/** 执行Lua方法 */
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
