using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaFrameworkCore;
using LuaUIFramework;

/// <summary>
/// 全局委托注册管理器
/// </summary>
public class AFRManager : MonoSingleton<AFRManager>{

	public void RegisterFunc()
	{
		Debug.Log("AFRManager");
		/** LuaAB加载注册 */
		LuaManager.LoadLuaABFunc = (callBack)=>{
			Dictionary<string,AssetBundle> abDic = new Dictionary<string, AssetBundle>();
			callBack(abDic);
		};
		/** 切换状态资源准备 */
		StateBase.InitResFunc = (callBack)=>{
			callBack();
		};
	}
}
