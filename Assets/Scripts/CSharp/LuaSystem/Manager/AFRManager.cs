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
		Debug.Log("AFRManager start");
		/** LuaAB加载注册 */
		LuaManager.LoadLuaABFunc = (callBack)=>{
			Dictionary<string,AssetBundle> abDic = new Dictionary<string, AssetBundle>();
			callBack(abDic);
		};
		/** 切换状态资源准备 */
		StateBase.InitResFunc = (callBack)=>{
			ResourceHelper.Instance.LoadResData(TEXTUREPATH+"loginback_3");
			callBack();
		};
		/** Lua bytes加载方法注册 */
		LuaLoader.LoadLuaFuc = (fileName,callBack)=>{
			byte[] buffer = null;
			fileName = fileName.Replace (".lua","");
			LuaPool.Instance.Load (fileName,1,(luaCode)=>{
				if (luaCode != null)
				{
					buffer = luaCode.bytes;
					Resources.UnloadAsset(luaCode);
				}
			});
			if(callBack!=null)callBack(buffer);
		};

		Util.LoadUIObjFuc = (name,callback)=>{
			ResourceLoaderManager.Instance.LoadUIObj (name,null,callback);
		};
	}
}
