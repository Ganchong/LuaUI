using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LuaFramework.Core;
using ABSystem;

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
				LuaManager.Instance.GetLuaFunction ("Main").Call ();
			});
		}
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
