using System;
using UnityEngine;
using LuaInterface;
using LuaFrameWorkCore;
using System.Collections.Generic;
using LuaFrameWorkCore;

namespace LuaUIFrameWork
{
	/// <summary>
	/// Lua manager.
	/// </summary>
	public class LuaManager : MonoSingleton<LuaManager>{
		
		/** Lua文件加载根目录 */
		public static string LUAFILEPATH = "Scripts/MLua";
		/** Lua AB包加载方法 */
		public static Action<CallBack<Dictionary<string,AssetBundle>>> LoadLuaABFunc = null;

		[Tooltip("是否使用AB包")]
		public bool isUseAB = false;

		/** lua虚拟机 */
		private LuaState luaState = null;
		/** lua脚本文件加载器 */
		private LuaLoader luaLoader = null;
		/** lua协程驱动器（C#的每一帧都会去驱动lua的协同完成协同功能） */
		private LuaLooper luaLooper = null;

		void Awake()
		{
		}

		/** 启动lua */
		public void LuaStart()
		{
			luaLoader = new LuaLoader();
			luaState = new LuaState();
			this.OpenLibs();
			luaState.LuaSetTop(0);

			LuaBinder.Bind(luaState);
			DelegateFactory.Init();
			LuaCoroutine.Register(luaState,this);

			InitLuaLoadPath();


		}


		/** 初始化Lua文件加载路径 */
		void InitLuaLoadPath()
		{
			if(!isUseAB){
				luaState.AddSearchPath(Application.dataPath+LUAFILEPATH);
			}else{
				if(LoadLuaABFunc!=null){
					LoadLuaABFunc((abDic)=>{
						string[] keys = abDic.Keys;
						for (int i = 0,max = abDic.Count; i < max; ++i) {
							if(string.IsNullOrEmpty(keys[i])||abDic[i]==null){
								Debug.Log("luaAB load error,name is null or assetBundle is null");
								continue;
							}
							luaLoader.AddSearchBundle(keys[i],abDic);
						}
					});
				}
			}
		}


		/// <summary>
		/// 初始化加载第三方库
		/// </summary>
		void OpenLibs() {
			luaState.OpenLibs(LuaDLL.luaopen_pb);
			luaState.OpenLibs(LuaDLL.luaopen_lpeg);
			luaState.OpenLibs(LuaDLL.luaopen_bit);
			luaState.OpenLibs(LuaDLL.luaopen_socket_core);

			#region cjson 只new了一个table 没有注册库，这里单独注册一下
			luaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
			luaState.OpenLibs(LuaDLL.luaopen_cjson);
			luaState.LuaSetField(-2, "cjson");
			luaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
			luaState.LuaSetField(-2, "cjson.safe");
			#endregion
		}
	}
}

