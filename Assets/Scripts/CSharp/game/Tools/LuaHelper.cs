﻿using System.Collections;
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

	/** 协程中心 */
	public static CoroutineCenter GetCoroutineCenter()
	{
		return CoroutineCenter.Instance;
	}

	/** 游戏管理器 */
	public static GameManager GetGameManager()
	{
		return GameManager.Instance;
	}

	/** 认证管理器 */
	public static CertifyManager GetCertifyManager()
	{
		return CertifyManager.Instance;
	}

	/** 服务器管理器 */
	public static ServerManager GetServerManager()
	{
		return ServerManager.Instance;
	}
}



