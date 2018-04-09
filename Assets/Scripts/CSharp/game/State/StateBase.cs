using System;
using UnityEngine;

/// <summary>
/// 状态基类
/// </summary>
public class StateBase {
	
	/** 加载资源方法注入接口 */
	public static Action<CallBack> InitResFunc = null;

	/** 进入状态 */
	public virtual void Enter()
	{
		
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
