using UnityEngine;
using System;
using LuaFramework.Core;

/// <summary>
/// 初始化状态
/// </summary>
public class InitState : StateBase 
{
	public override void Enter ()
	{
		base.Enter ();
		if(InitResFunc!=null){
			InitResFunc(()=>{
				LuaManager.Instance.LuaStart();
				LuaManager.Instance.DoFile("Main.lua");
				LuaManager.Instance.GetLuaFunction("Start").Call();
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
