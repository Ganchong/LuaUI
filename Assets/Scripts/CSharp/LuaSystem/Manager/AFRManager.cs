using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaFrameworkCore;
using LuaUIFramework;
using ABSystem ;
/// <summary>
/// 全局委托注册管理器
/// </summary>
public class AFRManager : Singleton<AFRManager>{
	/** 贴图 */
	public const string TEXTUREPATH="Texture/";

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
//			ResourceHelper.Instance.LoadResDataAsync(TEXTUREPATH+"loginback_3",(re)=>{
//			});
			ResourceHelper.Instance.LoadResData(TEXTUREPATH+"loginback_3");
			callBack();
		};
	}
}
