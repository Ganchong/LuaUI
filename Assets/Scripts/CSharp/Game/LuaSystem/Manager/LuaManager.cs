using System;
using UnityEngine;
using LuaInterface;
using LuaFrameworkCore;
using System.Collections.Generic;

namespace LuaUIFramework
{
	/// <summary>
	/// Lua manager.
	/// </summary>
	public class LuaManager : SingletonBehaviour<LuaManager>{
		
		/** Lua文件加载根目录 */
		public static string LUAFILEPATH = "/Art/MLua";
		/** Lua AB包加载方法 */
		public static Action<CallBack<Dictionary<string,AssetBundle>>> LoadLuaABFunc = null;

		/** lua虚拟机 */
		private LuaState luaState = null;
		/** lua脚本文件加载器 */
		private LuaLoader luaLoader = null;
		/** lua协程驱动器（C#的每一帧都会去驱动lua的协同完成协同功能） */
		private LuaLooper luaLooper = null;


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

			this.InitLuaLoadPath();
			luaState.Start ();
			this.InitLuaLooper ();
		}

		/** 加载并执行Lua文件 */
		public void DoFile(string fileName)
		{
			luaState.DoFile(fileName);
		}

		/** 调用Lua文件中的方法 */
		public LuaFunction GetLuaFunction(string funcName)
		{
			return luaState.GetFunction(funcName);
		}

		public void LuaGC()
		{
			luaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
		}
		/** 初始化Lua文件加载路径 */
		void InitLuaLoadPath()
		{
			if(!GameManager.Instance.isUseAB){
				luaState.AddSearchPath(Application.dataPath+LUAFILEPATH);
			}else{
//				if(LoadLuaABFunc!=null){
//					LoadLuaABFunc((abDic)=>{
//						foreach (KeyValuePair<string, AssetBundle> iter in abDic)
//						{
//							if(string.IsNullOrEmpty(iter.Key)||iter.Value==null){
//								Debug.Log("luaAB load error, assetBundle name is null or assetBundle is null");
//								continue;
//							}
//							luaLoader.AddSearchBundle(iter.Key,iter.Value);
//						}
//					});
//				}
			}
		}

		/** 初始化Lua携程驱动器 */
		void InitLuaLooper()
		{
			if (luaLooper == null)
				luaLooper = gameObject.AddMissCompoent<LuaLooper> ();
			luaLooper.luaState = this.luaState;
		}

		/** 初始化加载第三方库 */
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

		/** 清理 */
		public void Clear()
		{
			luaLooper.Destroy ();
			luaLooper = null;
			luaLoader = null;
			luaState.Dispose ();
			luaState = null;
		}
	}
}

