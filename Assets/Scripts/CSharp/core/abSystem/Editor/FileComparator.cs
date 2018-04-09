using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace ABSystem{
	
	/// <summary>
	/// 文件比较器是否一样
	/// </summary>
	public class FileComparator
	{
		/* static fields */
		/** 公用资源路径 */
		public static string BASERESPATH;

		/* static constructor */
		/** 构造方法 */
		static FileComparator()
		{
			BASERESPATH=Replace (Application.dataPath)+"/Art/Base/";
		}

		/* static methods */
		[MenuItem("Tools/查找依赖")]
		public static void FindDependencies()
		{
			if (Selection.objects == null || Selection.objects.Length < 1) return;
			string assetPath=AssetDatabase.GetAssetPath(Selection.objects [0]);
			string[] dependencies=AssetDatabase.GetDependencies (new string[]{assetPath});
			StringBuilder builder = new StringBuilder ();
			for (int i=0; i<dependencies.Length; i++) 
			{
				builder.Append(dependencies[i]);
				builder.AppendLine();
			}
			Debug.LogError (builder.ToString ());
		}
		[MenuItem("Tools/查找没有贴图的mat")]
		public static void FindNullMat()
		{
			List<string> withoutExtensions = new List<string>(){".mat"};
			string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
				.Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

			int startIndex = 0;

			EditorApplication.update = delegate()
			{
				string file = files[startIndex];

				bool isCancel = EditorUtility.DisplayCancelableProgressBar("查找资源中", file, (float)startIndex / (float)files.Length);
				file = Replace(file).Replace(ResourceEditorHelper.ASSETROOT,string.Empty);
				Material material=AssetDatabase.LoadAssetAtPath<Material>(file);
				if(material.mainTexture==null)
				{
					Debug.Log(files[startIndex]);
				}

				startIndex++;
				if (isCancel || startIndex >= files.Length)
				{
					EditorUtility.ClearProgressBar();
					EditorApplication.update = null;
					startIndex = 0;
					Debug.Log("匹配结束");
				}
			};
		}

		[MenuItem("Assets/Find References",false,10)]
		public static void FindReference()
		{
			if (Selection.activeObject == null) return;
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (!string.IsNullOrEmpty(path))
			{
				string guid = AssetDatabase.AssetPathToGUID(path);
				List<string> withoutExtensions = new List<string>(){".prefab",".unity",".mat",".asset"};
				string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
					.Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
				int startIndex = 0;

				EditorApplication.update = delegate()
				{
					string file = files[startIndex];

					bool isCancel = EditorUtility.DisplayCancelableProgressBar("匹配资源中", file, (float)startIndex / (float)files.Length);

					if (Regex.IsMatch(File.ReadAllText(file), guid))
					{
						Debug.Log(file, AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(GetRelativeAssetsPath(file)));
					}

					startIndex++;
					if (isCancel || startIndex >= files.Length)
					{
						EditorUtility.ClearProgressBar();
						EditorApplication.update = null;
						startIndex = 0;
						Debug.Log("匹配结束");
					}

				};
			}
		}
		static private string GetRelativeAssetsPath(string path)
		{
			return Replace (path).Replace (ResourceEditorHelper.ASSETROOT, string.Empty);
		}

		[MenuItem("Tools/文件对比相同")]
		public static void FindFiles()
		{
			if (Selection.objects == null || Selection.objects.Length < 1) return;
			string filePath = AssetDatabase.GetAssetPath (Selection.objects [0]);
			filePath = filePath.Replace ("Assets", "");
			filePath = Application.dataPath + filePath;

			Dictionary<string,string> hashKeyBaseMap = new Dictionary<string, string> ();
			if (Directory.Exists (BASERESPATH)) 
			{
				DirectoryInfo info = new DirectoryInfo (BASERESPATH);
				ListFile (info, (file) => {
					if (file.Name.ToLower ().EndsWith (".fbx"))
						return;
					if (file.Name.ToLower ().EndsWith (".mat"))
						return;
					if (file.Name.ToLower ().EndsWith (".meta"))
						return;
					if (file.Name.ToLower ().EndsWith (".obj"))
						return;
					if (file.Name.ToLower ().EndsWith (".anim"))
						return;
					if (file.Name.ToLower ().EndsWith (".controller"))
						return;
					string md5 = Utils.GetMD5Hash (file.FullName);
					if (!hashKeyBaseMap.ContainsKey (md5)) {
						hashKeyBaseMap.Add (md5, file.FullName);
						return;
					}
					Debug.LogError ("重复的资源:" + file.FullName);
					hashKeyBaseMap.Add (md5, file.FullName);
				});
			}

			Dictionary<string,List<string>> hashKeyMap = new Dictionary<string, List<string>> ();
			if (Directory.Exists (filePath)) {
				DirectoryInfo info = new DirectoryInfo (filePath);
				ListFile (info, (file) => {
					if(file.Name.ToLower().EndsWith(".fbx")) return;
					if(file.Name.ToLower().EndsWith(".mat")) return;
					if(file.Name.ToLower().EndsWith(".meta")) return;
					if(file.Name.ToLower().EndsWith(".obj")) return;
					if (file.Name.ToLower ().EndsWith (".anim")) return;
					if (file.Name.ToLower ().EndsWith (".controller")) return;
					string md5 = Utils.GetMD5Hash (file.FullName);
					if (!hashKeyMap.ContainsKey (md5)) {
						List<string> list = new List<string> ();
						list.Add (file.FullName);
						hashKeyMap.Add (md5, list);
						return;
					}
					hashKeyMap [md5].Add (file.FullName);
				});

				foreach (var keyValue in hashKeyMap) {
					var item=keyValue.Value;
					if (!hashKeyBaseMap.ContainsKey(keyValue.Key) && item.Count < 2)
						continue;
					string targetPath;
					if(!hashKeyBaseMap.TryGetValue(keyValue.Key,out targetPath))
					{
						string fileName=FileHelper.GetFileNameNoSuffix(item[0]);
						string fileSuffix=FileHelper.GetSuffix(item[0]);
						targetPath=BASERESPATH+fileName+fileSuffix;
						int i=1;
						while(File.Exists(targetPath))
						{
							targetPath=BASERESPATH+fileName+"("+i+")"+fileSuffix;
							i++;
						}
						FileHelper.CheckPath(targetPath);
						AssetDatabase.CopyAsset(Replace(item[0]).Replace(ResourceEditorHelper.ASSETROOT,string.Empty),Replace(targetPath).Replace(ResourceEditorHelper.ASSETROOT,string.Empty));
						AssetDatabase.ImportAsset(Replace(targetPath).Replace(ResourceEditorHelper.ASSETROOT,string.Empty));
						AssetDatabase.SaveAssets();
						AssetDatabase.Refresh();
					}
					string targetGuid=AssetDatabase.AssetPathToGUID(Replace(targetPath).Replace(ResourceEditorHelper.ASSETROOT,string.Empty));
					bool have=false;
					for(int i=0;i<item.Count;i++)
					{
						string guid=AssetDatabase.AssetPathToGUID(Replace(item[i]).Replace(ResourceEditorHelper.ASSETROOT,string.Empty));
						List<string> list=GetAllMat(item[i],guid);
						for(int j=0;j<list.Count;j++)
						{
							FileHelper.ReplaceFile(list[j],guid,targetGuid);
							AssetDatabase.ImportAsset(Replace(list[j]).Replace(ResourceEditorHelper.ASSETROOT,string.Empty));
						}
						AssetDatabase.DeleteAsset(Replace(item[i]).Replace(ResourceEditorHelper.ASSETROOT,string.Empty));
						if(list.Count>0) have=true;
					}
					if(!have)
					{
						//AssetDatabase.DeleteAsset(Replace(targetPath).Replace(ResourceEditorHelper.ASSETROOT,string.Empty));
						Debug.LogError(targetPath);
					}
				}
			} else {
				Debug.LogError ("请选择一个文件夹");
			}
		}

		/// <summary>
		/// 遍历文件
		/// </summary>
		public static void ListFile(FileSystemInfo info,Action<FileInfo> action)
		{
			if(info is DirectoryInfo){
				if(info.Name.EndsWith("_")) return;
				FileSystemInfo[] infos = (info as DirectoryInfo).GetFileSystemInfos();
				for(int i=0;i<infos.Length;i++){
					ListFile(infos[i],action);				
				}
			}
			else if(info is FileInfo)
			{
				FileInfo file = info as FileInfo;
				action(file);
			}
		}

		/** 修正windows路径 */
		private static string Replace(string path)
		{
			return path.Replace ('\\', '/');
		}

		/** 提取指定文件夹下的包含指定guid文件mat文件 */
		private static List<string> GetAllMat(string filePath,string guid)
		{
			DirectoryInfo directory = new DirectoryInfo (ResourceEditorHelper.RESROOT);
			FileInfo fileInfo = new FileInfo (filePath);
			DirectoryInfo current = fileInfo.Directory;
			while (true) 
			{
				if(current==null)
				{
					Debug.LogError(filePath);
				}
				string directoryFullName = Replace (current.FullName);
				string prefabPath=directoryFullName+FileHelper.FILESPEARATOR+current.Name+".prefab";
				if (System.IO.File.Exists (prefabPath)) break;
				if(current.FullName==directory.FullName) break;
				current = current.Parent;
			}
			if (current.FullName == directory.FullName) return new List<string> ();
			List<string> list = new List<string> ();
			GetAllMat (current, list, guid);
			return list;
		}
		/** 获取材质 */
		private static void GetAllMat(DirectoryInfo current,List<string> list,string guid)
		{
			FileInfo[] files=current.GetFiles();
			for(int i=0;i<files.Length;i++)
			{
				if(!files[i].Name.EndsWith(".mat")) continue;
				string content;
				if(FileHelper.GetFileString(files[i].FullName,out content))
				{
					if(content.Contains(guid))
					{
						list.Add(files[i].FullName);
					}
				}
			}
			DirectoryInfo[] directorys=current.GetDirectories();
			for (int i=0; i<directorys.Length; i++) 
			{
				GetAllMat(directorys[i],list,guid);
			}
		}

		[MenuItem("Tools/删除未使用的mat")]
		public static void FindNoUseMat()
		{
			if (Selection.objects == null || Selection.objects.Length < 1) return;
			string filePath = AssetDatabase.GetAssetPath (Selection.objects [0]);
			filePath = filePath.Replace ("Assets", "");
			filePath = Application.dataPath + filePath;
			if (Directory.Exists (filePath)) {
				DirectoryInfo info = new DirectoryInfo (filePath);
				FindNoUseInAllDirectory (info);
				AssetDatabase.Refresh();
			} else {
				Debug.LogError ("请选择一个文件夹");
			}
		}
		/** 是否是带查找文件夹 */
		public static void FindNoUseInAllDirectory(DirectoryInfo directory)
		{
			if (directory.Name.EndsWith (ResourceEditorHelper.SUFFIX)) return;
			string directoryFullName = Replace (directory.FullName);
			string prefabPath=directoryFullName+FileHelper.FILESPEARATOR+directory.Name+".prefab";
			if (System.IO.File.Exists (prefabPath)) 
			{
				List<string> nouseMats=new List<string>();
				FindNoUseMat(directory, nouseMats);
				string[] dependencies=AssetDatabase.GetDependencies (new string[]{prefabPath.Replace(ResourceEditorHelper.ASSETROOT,string.Empty)});
				foreach(var str in dependencies)
				{
					if(!nouseMats.Contains(str)) continue;
					nouseMats.Remove(str);
				}
				if(nouseMats.Count<1) return;
				for(int i=0;i<nouseMats.Count;i++)
				{
					AssetDatabase.DeleteAsset(nouseMats[i]);
				}
				Debug.LogError(directoryFullName);
				return;
			}
			var directories = directory.GetDirectories ();
			foreach (var dir in directories) 
			{
				FindNoUseInAllDirectory(dir);
			}
		}
		/** 查找没有使用的mat */
		public static void FindNoUseMat(DirectoryInfo directory,List<string> mats)
		{
			var files=directory.GetFiles ();
			foreach(var file in files)
			{
				if(file.Name.ToLower().EndsWith(".mat"))
				{
					string assetPath=Replace(file.FullName).Replace(ResourceEditorHelper.ASSETROOT,string.Empty);
					mats.Add(assetPath);
				}
			}
			var directories = directory.GetDirectories ();
			foreach (var dir in directories) 
			{
				FindNoUseMat(dir,mats);
			}
		}
	}
}
