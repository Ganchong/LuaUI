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
			//ResourceHelper.Instance.LoadResData(TEXTUREPATH+"loginback_3");
			callBack();
		};
		/** Lua bytes加载方法注册 */
		LuaLoader.LoadLuaFuc = (fileName,call)=>{
			byte[] buffer = null;
			fileName = fileName.Replace (".lua","");
			LuaPool.Instance.Load (fileName,1,(luaCode)=>{
				if (luaCode != null)
				{
					buffer = luaCode.bytes;
					Resources.UnloadAsset(luaCode);
				}
			});
			if(call!=null)call(buffer);
		};
		/** UI预制加载方法 */
		Util.LoadUIObjFuc = (name,call)=>{
			ResourceLoaderManager.Instance.LoadUIObj (name,null,call);
		};

		/** UIRawImage纹理加载方法注册 */
		UIRawImage.LoadTextureFunc = (path,rawImage,call)=>{

			if(rawImage==null||rawImage.IsDestroyed())return;
			if(string.IsNullOrEmpty(path)){
				rawImage.Alpha = 0;
				return;
			}
			TexturePool.Instance.Load(TEXTUREPATH+path,(mainTex,alphaTex)=>{
				if(rawImage == null||rawImage.IsDestroyed())return;
				if(mainTex==null){
					Debug.LogWarning("Load texture failed,path is :"+path);
					return;
				}
				rawImage.Alpha = 1;
				rawImage.texture = mainTex;
				rawImage.alphaTex = alphaTex;
				if(call!=null)call(rawImage);
			});
		};
		/** UIRawImage默认Shader加载方法注册 */
		UIRawImage.LoadUIDefaultShaderFunc = (shaderName,callBack)=>{
			callBack(ResourcesManager.Instance.GetShader(shaderName));
		};

		MaterialManager.CustomGetShaderFunc = (shaderName)=>{
			return ResourcesManager.Instance.GetShader(shaderName);
		};

	}
}
