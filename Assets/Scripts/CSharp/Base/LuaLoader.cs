using UnityEngine;
using LuaInterface;
using LuaUIFramework;

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
			return base.ReadFile (fileName);
		}
		public override bool RemoveSearchPath (string path)
		{
			return base.RemoveSearchPath (path);
		}
	}

}
