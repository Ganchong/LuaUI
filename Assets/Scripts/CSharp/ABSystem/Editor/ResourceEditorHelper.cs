using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace ABSystem{
	/// <summary>
	/// Resource editor helper.
	/// </summary>
	public static class ResourceEditorHelper
	{
		/* static fields */
		/** 不标记文件夹后缀 */
		public static string SUFFIX="_";
		/** 主资源路径 */
		public static string RESROOT;
		/** Lua文件主路径*/
		public static string LUAROOT;
		/** 主路径 */
		public static string ASSETROOT;
		/** 资源后缀 */
		public static string ABSUFFIX="unity3d";
		/** 流媒体路劲 */
		public static string STREAMPATH;
		/** 依赖查询路径 */
		public static string MANIFEASTPATH;

		/* static constructor */
		/** 构造方法 */
		static ResourceEditorHelper()
		{
			ASSETROOT = Replace (Application.dataPath);
			RESROOT = ASSETROOT + "/Art/";
			LUAROOT = ASSETROOT + "/temp/";
			MANIFEASTPATH = ASSETROOT+"/../Manifests";
			ASSETROOT = ASSETROOT.Substring (0, ASSETROOT.Length - "Assets".Length);
			STREAMPATH = Replace (Application.streamingAssetsPath);
		}

		/* static methods */
		/** 清除标记 */
		[MenuItem ("AssetBundle/ClearMarks")]
		public static void ClearMarks()
		{
			string[] names=AssetDatabase.GetAllAssetBundleNames ();
			if (names.Length < 1) return;
			int startIndex = 0;
			string[] paths = null;
			string name = string.Empty;
			EditorApplication.update = delegate()
			{
				for(int i=0;i<500;i++)
				{
					EditorUtility.DisplayProgressBar("清理标记中", name, (float)startIndex / (float)names.Length);

					paths=AssetDatabase.GetAssetPathsFromAssetBundle(names[startIndex]);

					if(paths==null)
					{
						name=names[startIndex];
						AssetDatabase.RemoveAssetBundleName (name, true);
					}
					else if(paths.Length==2)
					{
						for(int j=0;j<paths.Length;j++)
						{
							if(!paths[j].Contains(ResData.ALPHAPREFIX)) continue;
							AssetImporter importer = AssetImporter.GetAtPath(paths[j]);
							importer.assetBundleName=string.Empty;
							name=names[startIndex];
							break;
						}
					}
					startIndex++;
					if (startIndex >= names.Length)
					{
						EditorUtility.ClearProgressBar();
						EditorApplication.update = null;
						startIndex = 0;
						Debug.Log("清理完毕");
						break;
					}
				}
			};
			AssetDatabase.Refresh ();
		}

		/** 标记资源 */
		[MenuItem ("AssetBundle/MarkAndroidAssets")]
		public static void MarkAndroidAssets()
		{
			AssetDatabase.RemoveUnusedAssetBundleNames();
			if (!Directory.Exists (RESROOT)) 
			{
				Debug.LogWarning (RESROOT+", isn't exist!");
			}
			DirectoryInfo dicectory = new DirectoryInfo (RESROOT);
			MarkDirectory(dicectory,"Android");
			AssetDatabase.Refresh ();
		}
		/** 标记资源 */
		[MenuItem ("AssetBundle/MarkIosAssets")]
		public static void MarkIosAssets()
		{
			AssetDatabase.RemoveUnusedAssetBundleNames();
			if (!Directory.Exists (RESROOT)) 
			{
				Debug.LogWarning (RESROOT+", isn't exist!");
			}
			DirectoryInfo dicectory = new DirectoryInfo (RESROOT);
			MarkDirectory(dicectory,"iPhone");
			AssetDatabase.Refresh ();
		}
		/** 标记资源 */
		[MenuItem ("AssetBundle/MarkAssetsWithFolder")]
		public static void MarkAssets()
		{
			AssetDatabase.RemoveUnusedAssetBundleNames();
			if (Selection.activeObject) 
			{
				string path=AssetDatabase.GetAssetPath(Selection.activeObject);
				path=(Application.dataPath.Replace("Assets",string.Empty)+path).Replace('/','\\');
				if(Directory.Exists(path))
				{
					DirectoryInfo dicectory = new DirectoryInfo (path);
					MarkDirectory (dicectory, "iPhone");
					AssetDatabase.Refresh ();
				}
			}
		}


		[MenuItem("AssetBundle/BuildAndroid")]
		public static void BuildAndroid()
		{
			AssetDatabase.Refresh ();
			BuildAssetBundle (BuildTarget.Android);
		}

		[MenuItem("AssetBundle/BuildIOS")]
		public static void BuildIOS()
		{
			AssetDatabase.Refresh ();
			BuildAssetBundle (BuildTarget.iOS);
		}
		[MenuItem("AssetBundle/Lua/MarkLuaAssets")]
		public static void MarkLuaAssets()
		{
			if (!Directory.Exists (LUAROOT)) {
				Debug.Log (LUAROOT+", isn't exist!,will to create..");
			}
			CopyLuaFilesToTemp ();
			DirectoryInfo dicectoryLua = new DirectoryInfo (LUAROOT);
			MarkLuaDirectory(dicectoryLua);
		}
		[MenuItem("AssetBundle/Lua/CopyLuaFilesToTemp")]
		public static void CopyLuaFilesToTemp()
		{
			ToLuaMenu.ClearAllLuaFiles();
			#if !UNITY_5 && !UNITY_2017 && !UNITY_2018
			string tempDir = CreateStreamDir("Lua");
			#else
			string tempDir = Application.dataPath + "/temp/Lua";

			if (!File.Exists(tempDir))
			{
				Directory.CreateDirectory(tempDir);
			}        
			#endif
			ToLuaMenu.CopyLuaBytesFiles(LuaConst.luaDir, tempDir);
			ToLuaMenu.CopyLuaBytesFiles(LuaConst.toluaDir, tempDir);
			AssetDatabase.Refresh();
			Debug.Log ("拷贝Lua文件到临时目录成功！");
		}
		[MenuItem("AssetBundle/Lua/ClearAllTempLuaFiles")]
		public static void ClearAllTempLuaFiles()
		{
			ToLuaMenu.ClearAllLuaFiles();
			AssetDatabase.Refresh ();
			Debug.Log ("清理Lua临时文件成功！");
		}


		[MenuItem("AssetBundle/BuildAllSelectAndroid")]
		public static void BuildAllSelectAndroid()
		{
			List<string> filePathList = new List<string>();
			UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object),SelectionMode.DeepAssets);
			string name = selection[0].name;
			for (int i = 0; i < selection.Length; i++) {
				UnityEngine.Object obj = selection[i];
				string filePath = AssetDatabase.GetAssetOrScenePath(obj);
				Debug.LogError(filePath);
				AddDependencies(filePath,filePathList);
			}
			Dictionary<string,List<string>> list_dict = new Dictionary<string, List<string>>();
			for (int j = 0; j < filePathList.Count; j++) {
				string f_path = filePathList[j];
				if(string.IsNullOrEmpty(f_path))
					continue;
				string endText = Path.GetExtension(f_path).Trim('.');
				if(!list_dict.ContainsKey(endText))
					list_dict[endText] = new List<string>();
				List<string> list = list_dict[endText];
				list.Add(f_path);
			}
			AssetBundleBuild[] abb_array = new AssetBundleBuild[list_dict.Count];
			List<string> key_list = new List<string> (list_dict.Keys);
			for (int k = 0; k < key_list.Count; k++) {
				string key = key_list[k];
				if(string.IsNullOrEmpty(key)||!list_dict.ContainsKey(key)||list_dict[key]==null)
					continue;
				List<string> list = list_dict[key];
				abb_array[k].assetNames = list.ToArray();
				abb_array[k].assetBundleName = string.Format("{0}{1}{2}",name,key,".unity3d").ToLower();
			}
			BuildPipeline.BuildAssetBundles(STREAMPATH,abb_array,BuildAssetBundleOptions.DeterministicAssetBundle,BuildTarget.Android);
			AssetDatabase.Refresh();
		}

		/** 循环添加路径及资源相关列表 */
		static void AddDependencies(string path,List<string> list)
		{
			if(list.Contains(path)){
				return;
			}
			list.Add(path);
			string[] fileDependencies = AssetDatabase.GetDependencies(path);
			for (int i = 0; i < fileDependencies.Length; i++) {
				string fd = fileDependencies[i];
				if(string.IsNullOrEmpty(fd))
					continue;
				AddDependencies(fd,list);
			}
		}
		/** 生产AB文件 */
		static void BuildAssetBundle(BuildTarget target)
		{
			if (Directory.Exists (STREAMPATH)) 
			{
				Directory.Delete (STREAMPATH, true);
			}
			Directory.CreateDirectory (STREAMPATH);

			BuildPipeline.BuildAssetBundles (STREAMPATH, BuildAssetBundleOptions.DeterministicAssetBundle,target);

			MoveManifest ();

			string manifestPath = STREAMPATH + FileHelper.FILESPEARATOR + FileHelper.GetFileName (STREAMPATH);
			File.Move(manifestPath,STREAMPATH + FileHelper.FILESPEARATOR + ResourceHelper.MANIFESTPATH + ResourceHelper.SUFFIX);

			AssetDatabase.Refresh ();
		}
		/** 移动manifest文件 */
		private static void MoveManifest()
		{
			DirectoryInfo info = new DirectoryInfo (STREAMPATH);
			if (Directory.Exists (MANIFEASTPATH))
			{
				Directory.Delete(MANIFEASTPATH, true);
			}
			FileComparator.ListFile (info, (file) => {
				if(!file.FullName.EndsWith(".manifest")) return;
				string targetPath=Replace(file.FullName).Replace(STREAMPATH,MANIFEASTPATH);
				FileHelper.CheckPath(targetPath);
				File.Move(file.FullName,targetPath);
			});
		}
		/** 标记Lua文件夹下文件 */
		private static void MarkLuaDirectory(DirectoryInfo dicectory)
		{
			if (dicectory.Name.EndsWith (SUFFIX)) return;
			string directoryFullName = Replace (dicectory.FullName);
			FileInfo[] files=dicectory.GetFiles ();
			for(int i=0;i<files.Length;i++)
			{
				if(!files[i].Name.EndsWith(".bytes")) continue;
				AssetImporter importer = AssetImporter.GetAtPath (Replace(files[i].FullName).Replace(ASSETROOT,string.Empty));
				string assetName=FileHelper.StripSuffix(Replace (files[i].FullName).Replace(LUAROOT,string.Empty).Replace(".lua",""));
				if(importer.assetBundleName==assetName) return;
				importer.assetBundleName=assetName;
				importer.assetBundleVariant=ABSUFFIX;
			}
			DirectoryInfo[] dicectorys = dicectory.GetDirectories ();
			for (int i = 0; i < dicectorys.Length; i++) 
			{
				MarkLuaDirectory (dicectorys [i]);
			}
		}
		/** 标记文件夹下文件 */
		private static void MarkDirectory(DirectoryInfo dicectory,string platform)
		{
			if (dicectory.Name.EndsWith (SUFFIX)) return;
			string directoryFullName = Replace (dicectory.FullName);
			string prefabPath=directoryFullName+FileHelper.FILESPEARATOR+dicectory.Name+".prefab";
			if (System.IO.File.Exists (prefabPath)) 
			{
				AssetImporter importer = AssetImporter.GetAtPath (prefabPath.Replace(ASSETROOT,string.Empty));
				string assetName=directoryFullName.Replace(RESROOT,string.Empty);
				if(importer.assetBundleName==assetName) return;
				importer.assetBundleName=assetName;
				importer.assetBundleVariant=ABSUFFIX;
				return;
			}
			FileInfo[] files=dicectory.GetFiles ();
			if (dicectory.Name == "Config") 
			{
				for(int i=0;i<files.Length;i++)
				{
					if(!files[i].Name.EndsWith(".txt")) continue;
					AssetImporter importer = AssetImporter.GetAtPath (Replace(files[i].FullName).Replace(ASSETROOT,string.Empty));
					string assetName=FileHelper.StripSuffix(Replace (files[i].FullName).Replace(RESROOT,string.Empty));
					if(importer.assetBundleName==assetName) return;
					importer.assetBundleName=assetName;
					importer.assetBundleVariant=ABSUFFIX;
				}
				return;
			}
			if (dicectory.Name == "Views") 
			{
				for(int i=0;i<files.Length;i++)
				{
					if(!files[i].Name.EndsWith(".prefab")) continue;
					AssetImporter importer = AssetImporter.GetAtPath (Replace(files[i].FullName).Replace(ASSETROOT,string.Empty));
					string assetName=FileHelper.StripSuffix(directoryFullName.Replace(RESROOT,string.Empty));
					if(importer.assetBundleName==assetName) return;
					importer.assetBundleName=assetName;
					importer.assetBundleVariant=ABSUFFIX;
				}
				return;
			}
			string fileName=null;
			for (int i = 0; i < files.Length; i++)
			{
				if(files[i].Name.EndsWith(".meta")) continue;
				if (files [i].Name.EndsWith (".DS_Store"))continue;
				fileName=Replace (files [i].FullName);
				if(files[i].Name.EndsWith(".shader"))
				{
					var shader = AssetImporter.GetAtPath (fileName.Replace(ASSETROOT,string.Empty));
					if (shader.assetBundleName == "Shaders") continue;
					shader.assetBundleName="Shaders";
					shader.assetBundleVariant=ABSUFFIX;
					continue;
				}
				string assetName = FileHelper.StripSuffix(fileName.Replace (RESROOT, string.Empty));
				AssetImporter importer = AssetImporter.GetAtPath (fileName.Replace(ASSETROOT,string.Empty));
				if(files[i].Name.StartsWith(ResData.ALPHAPREFIX)) //如果是alpha通道图,若原始图无alpha通道则标记，若有则不标记
				{
					TextureImporter textureImporter=AssetImporter.GetAtPath(fileName.Replace(ASSETROOT,string.Empty).Replace(ResData.ALPHAPREFIX,string.Empty)) as TextureImporter;
					TextureImporterFormat format;
					int maxSize;
					textureImporter.GetPlatformTextureSettings(platform,out maxSize,out format);
					if(TextureAssetImporter.IsHaveAlpha(format,textureImporter.DoesSourceTextureHaveAlpha()))
						continue;
					assetName=assetName.Replace(ResData.ALPHAPREFIX,string.Empty);
				}
				if (importer.assetBundleName == assetName) continue;
				importer.assetBundleName = assetName;
				importer.assetBundleVariant = ABSUFFIX;
			}
			DirectoryInfo[] dicectorys = dicectory.GetDirectories ();
			for (int i = 0; i < dicectorys.Length; i++) 
			{
				MarkDirectory (dicectorys [i],platform);
			}
		}
		/** 修正windows路径 */
		private static string Replace(string path)
		{
			return path.Replace ('\\', '/');
		}
		/** 标记资源 */
		[MenuItem ("AssetBundle/CreateUpdateList")]
		public static void CreateMoveList ()
		{
			string AssetsPath = EditorUtility.SaveFolderPanel ("Select Floder To Bundle", "StreamingAssets", "StreamingAssets");
			string path = AssetsPath + "/list.settings";
			if (File.Exists (path))
				File.Delete (path);

			List<string> list = null;
			if (string.IsNullOrEmpty (AssetsPath))
				return;
			list = GetAllFilePath (new DirectoryInfo (AssetsPath));

			FileStream fs = File.OpenWrite (path);
			int subLen = new DirectoryInfo (AssetsPath).FullName.Length;

			for(int i=0;i<list.Count;i++)
			{
				string name=list[i];
				long fileSize = 0;
				string fileMD5 = Utils.GetMD5Hash (name);
				if (string.IsNullOrEmpty (fileMD5)) {
					continue;
				}
				string str = name.Substring (subLen + 1);
				str = str.Replace ('\\', '/');

				FileInfo info=new FileInfo(name);
				fileSize=info.Length;

				string all = (i+1) + "|" + fileMD5  + "|" + fileSize+ "|" + str + "\n";

				byte[] data = System.Text.Encoding.UTF8.GetBytes (all);
				fs.Write (data, 0, data.Length);
			}
			fs.Flush ();
			fs.Close ();
		}
		private static List<string> GetAllFilePath (DirectoryInfo dir)
		{
			List<string> list = new List<string> ();
			foreach (FileInfo info in dir.GetFiles()) 
			{
				if (info.FullName.EndsWith (".meta"))
					continue;
				list.Add (info.FullName);
			}
			foreach (DirectoryInfo info in dir.GetDirectories()) 
			{
				list.AddRange(GetAllFilePath (info));
			}
			return list;
		}
	}
}
