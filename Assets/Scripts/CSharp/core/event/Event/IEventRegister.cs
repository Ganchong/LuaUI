using System;
using System.Collections.Generic;

/// <summary>
/// 注册接口
/// </summary>
public interface IEventRegister
{
	/* properties */
	/** 已注册列表 */
	Dictionary<int,List<Delegate>> eventMap {
		get;
		set;
	}

	/* methods(扩展方法中实现) */
}