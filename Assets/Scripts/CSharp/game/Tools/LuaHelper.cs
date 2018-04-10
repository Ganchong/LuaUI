using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using LuaFramework.Core;

public static class LuaHelper {
	

	/** 资源更新管理器 */
	public static ResourceUpdateManager GetResourceUpdateManager()
	{
		return ResourceUpdateManager.Instance;
	}
	/** 游戏管理器 */
	public static GameManager GetGameManager()
	{
		return GameManager.Instance;
	}
}



