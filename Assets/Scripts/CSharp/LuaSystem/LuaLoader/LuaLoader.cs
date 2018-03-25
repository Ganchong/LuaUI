using UnityEngine;
using LuaInterface;
using LuaUIFramework;
using System.IO;
using ABSystem;

namespace LuaFrameworkCore
{
	/// <summary>
	/// Lua脚本文件加载器，重载自LuaFileUtils以实现自己的添加方法
	/// </summary>
	public class LuaLoader : LuaFileUtils {
		
		/** 构造 */
		public LuaLoader()
		{
			instance = this;
			beZip = GameManager.Instance.isUseAB;
		}

		/** 以AB包的形式添加Lua脚本文件 */
		public override void AddSearchBundle (string name, AssetBundle bundle)
		{
			base.AddSearchBundle (name, bundle);
		}
		public override byte[] ReadFile (string fileName)
		{
			if (!beZip)
			{
				string path = FindFile(fileName);
				byte[] str = null;

				if (!string.IsNullOrEmpty(path) && File.Exists(path))
				{
					#if !UNITY_WEBPLAYER
					str = File.ReadAllBytes(path);
					#else
					throw new LuaException("can't run in web platform, please switch to other platform");
					#endif
				}
				return str;
			}
			else
			{
				return ReadAssetBundleFile(fileName);
			}
		}

		public byte[] ReadAssetBundleFile(string fileName)
		{
			byte[] buffer = null;
			fileName = fileName.Replace (".lua","");
			LuaPool.Instance.Load (fileName,1,(luaCode)=>{
				if (luaCode != null)
				{
					buffer = luaCode.bytes;
					Resources.UnloadAsset(luaCode);
				}
			});

			return buffer;
		}
	}

}
