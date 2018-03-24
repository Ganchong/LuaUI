using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ABSystem{
	/// <summary>
	/// 资源加载器
	/// </summary>
	public sealed class ResourceHelper : SingletonBehaviour<ResourceHelper>
	{
		/* static fields */
		/** 更新资源路径 */
		public static string ASEETPATH;
		/** 流媒体路径 */
		public static string STREAMPATH; 
		/** 后缀 */
		public const string SUFFIX=".unity3d";
		/** 每10秒钟检测一次资源释放 */
		public const float RELEASETIME=5f;
		/** 每2分钟尖刺一次资源回收 */
		public const float UNLOADTIME=120;
		/** 资源路径 */
		public const string MANIFESTPATH = "manifest";
		/** 配置资源路径 */
		public const string CONFIGPATH="Config/";
		/** 阵法路径 */
		public const string FORMATION="Formation/";
		/** 特效路径 */
		public const string EFFECTPATH="Effect/";
		/** UI特效路径 */
		public const string UIEFFECTPATH="Effect/UiEffect/";
		/** 模型路径 */
		public const string MODELPATH="Model/";
		/** 音效 */
		public const string AUDIOPATH="Audio/";
		/** 贴图 */
		public const string TEXTUREPATH="Texture/";

		/* fields */
		/** 回调列表 */
		Dictionary<string,List<Action<ResData>>> actionMap=new Dictionary<string, List<Action<ResData>>>();
		/** 当前资源列表 */
		Dictionary<string,ResData> resDataMap=new Dictionary<string, ResData>();
		/** 资源依赖关系 */
		AssetBundleManifest manifest;

		/** 是否使用AB */
		bool useAssetBundle;

		/* methods */
		protected sealed override void Awake ()
		{
			base.Awake ();
			STREAMPATH = Application.streamingAssetsPath + FileHelper.FILESPEARATOR;
			if (Application.platform == RuntimePlatform.Android)
				STREAMPATH = Application.dataPath + "!assets/";
			#if UNITY_EDITOR
			useAssetBundle = GameManager.Instance.isUseAB;
			ASEETPATH=Application.dataPath+"/AssetBundle/";
			if(!File.Exists(STREAMPATH+MANIFESTPATH+SUFFIX))
				useAssetBundle=false;
			#else
			ASEETPATH=Application.persistentDataPath+"/AssetBundle/";
			useAssetBundle=true;
			#endif
			InvokeRepeating ("releaseResource", RELEASETIME, RELEASETIME);
			InvokeRepeating ("unloadUnUseAssets", UNLOADTIME, UNLOADTIME);
		}
		/** 初始化 */
		public void InitManifest(Action action)
		{
			if (!useAssetBundle) { action (); return; }
			manifest = null;
			LoadResDataAsync (MANIFESTPATH, (data) => {
				manifest=data.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
				data._object=null;
				data.assetBundle.Unload(false);
				resDataMap.Remove(MANIFESTPATH);
				action();
			});
		}
		/** 加载主资源(在资源加载时若调用UnLoadUnUse卸载时会导致加载资源引用丢失) */
		public void LoadMainAssetAsync<T>(string path,Action<T> action) where T : UnityEngine.Object
		{
			LoadResDataAsync (path, (resData) => {
				if(typeof(T) == typeof(GameObject))
					action(resData.LoadMainAsset<T>());
				else 
					resData.LoadMainAssetAsync<T>(action);
			});
		}
		/** 加载主资源同步 */
		public T LoadMainAsset<T>(string path) where T : UnityEngine.Object
		{
			ResData resData = LoadResData (path);
			return resData.LoadMainAsset<T> ();
		}
		/** 加载资源(返回值为未加载完成对象) */
		public ResData LoadResDataAsync(string path,Action<ResData> action)
		{
			ResData resData;
			path = path.ToLower ();
			if (resDataMap.TryGetValue (path,out resData)) 
			{
				if (resData.isDone) {
					if (resData.retainCount >= 0) {
						resData.useTime = Time.unscaledTime;
						if(action!=null) action (resData);
						return resData;
					}
					resDataMap.Remove (path);
					resData = null;
				}
			}
			if (action != null) 
			{
				List<Action<ResData>> list;
				if (actionMap.TryGetValue (path, out list)) {
					list.Add (action);
				} else {
					list = new List<Action<ResData>> ();
					list.Add (action);
					actionMap.Add (path, list);
				}
			}
			if (resData != null) return resData;
			resData = new ResData (path);
			LoadResDataAsync (resData, action);
			return resData;
		}
		/** 同步加载资源 */
		public ResData LoadResData(string path)
		{
			ResData resData;
			path = path.ToLower ();
			if (resDataMap.TryGetValue (path,out resData)) 
			{
				if (resData.isDone) {
					resData.useTime = Time.unscaledTime;
					return resData;
				}
				else
				{
					throw new Exception("must be async load or cache res:"+path);
				}
			}
			resData = new ResData (path);
			loadRes.Clear ();
			loadRes.Add (resData.path,resData);
			getDependencies (resData, loadRes, true);
			foreach(var res in loadRes)
			{
				resDataMap.Add (res.Key,res.Value);
				LoadResData(res.Value);
			}
			return resData;
		}

		/** 加载临时列表 */
		Dictionary<string,ResData> loadRes=new Dictionary<string,ResData>();
		/** 加载资源 */
		private void LoadResDataAsync(ResData resData,Action<ResData> action)
		{
			if (!useAssetBundle) { loadFinish(resData); return; }

			loadRes.Clear ();
			loadRes.Add (resData.path,resData);
			getDependencies (resData, loadRes, false);
			foreach(var res in loadRes){
				resDataMap.Add (res.Key,res.Value);
				StartCoroutine (LoadResDataAsync (res.Value));
			}
		}


		/** 加载多个资源(返回值为未加载完成资源) */
		public ResData[] LoadResDatasAsync(string[] paths,Action<ResData[]> action)
		{
			paths = paths.RemoveSamePath ();
			ResData[] resdatas=new ResData[paths.Length];
			int num = 0;
			for (int i = 0; i < paths.Length; i++) 
			{
				resdatas[i]=LoadResDataAsync (paths[i], (data) => 
					{
						num++;
						if(num==paths.Length)
						{
							if(action!=null) action(resdatas);
						}
					});
			}
			return resdatas;
		}
		/** 加载资源(单个) */
		private IEnumerator LoadResDataAsync(ResData data)
		{
			string path = data.path + SUFFIX;
			string url = ASEETPATH + path;
			if (!File.Exists (url)) url = STREAMPATH + path;
			AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync (url);
			while (!request.isDone)
			{
				data._progress = request.progress;
				yield return 0;
			}
			data.assetBundle=request.assetBundle;
			data._isDone = true;
			while (!data.isDone) 
			{
				yield return 0;
			}
			loadFinish (data);
		}
		/** 加载资源同步(用于单个资源加载,依赖资源长缓) */
		private void LoadResData(ResData data)
		{
			if (!useAssetBundle) 
			{
				data._isDone=true;
				return;
			}
			string path = data.path + SUFFIX;
			string url = ASEETPATH + path;
			if (!File.Exists (url)) url = STREAMPATH + path;
			data.assetBundle = AssetBundle.LoadFromFile (url);
			data._isDone = true;
		}

		/** 加载完成 */
		private void loadFinish(ResData resData)
		{
			resData.Finish ();
			resData.useTime = Time.unscaledTime;

			List<Action<ResData>> list;
			if (actionMap.TryGetValue (resData.path, out list)) 
			{
				for (int i = 0; i < list.Count; i++) 
				{
					list [i].Invoke (resData);
				}
				list.Clear ();
			}
		}
		/** 获取所有依赖包 */
		private void getDependencies(ResData resData,Dictionary<string,ResData> dic,bool checkAysnc)
		{
			if (manifest == null) return;
			string[] dependencies = manifest.GetAllDependencies (resData.path+SUFFIX);
			if (dependencies == null || dependencies.Length < 1) return;
			ResData[] references=new ResData[dependencies.Length];
			ResData temp;
			string path;
			for (int i = 0; i < dependencies.Length; i++) 
			{
				path = dependencies [i];
				path = path.Substring(0,path.Length-SUFFIX.Length);
				if(resDataMap.TryGetValue(path,out temp))
				{
					references [i] = temp;
					if(checkAysnc&&!temp.isDone)
					{
						throw new Exception("must be load aysnc or cache res:"+resData.path);
					}
					continue;
				}
				if(!dic.TryGetValue(path,out temp)){
					temp = new ResData (path);
					dic.Add (temp.path,temp);
					getDependencies (temp, dic, checkAysnc);
				}else Debug.LogError("path:"+path+",res:"+resData.path);
				references [i] = temp;
			}
			resData.references = references;
			for (int i = 0; i < references.Length; i++) 
			{
				references [i].Retain ();
			}
		}
		/** 主动释放 */
		public void Release(string path)
		{
			ResData data;
			if (resDataMap.TryGetValue (path, out data)) 
			{
				Release (data);
			}
		}
		/** 释放 */
		public void Release(ResData resData)
		{
			if (resData.retainCount > 0) 
			{
				resData.release ();
				if (resData.retainCount < 1)
					resDataMap.Remove (resData.path);
			}
		}
		/** 释放 */
		public void Release(ResData[] resDatas)
		{
			for (int i = 0; i < resDatas.Length; i++) 
			{
				if (resDatas [i].retainCount < 1) continue;
				resDatas [i].release ();
				if (resDatas [i].retainCount < 1)
					resDataMap.Remove (resDatas [i].path);
			}
		}
		/** 资源释放 */
		private void releaseResource()
		{
			float nowTime= Time.unscaledTime;
			ResData[] datas=new ResData[resDataMap.Count];
			resDataMap.Values.CopyTo (datas, 0);
			ResData item = null;
			for(int i=0;i<datas.Length;i++)
			{
				item=datas[i];
				if (!item.isDone) continue;
				if (item.retainCount > 0) continue;
				if (item.useTime > nowTime - RELEASETIME) continue;
				item.release ();
				resDataMap.Remove (item.path);
			}
		}
		/** 卸载资源 */
		private void unloadUnUseAssets()
		{
			Resources.UnloadUnusedAssets ();
		}

		/* static methods */
		/** 进度 */
		public static void Progress(ResData[] resDatas,Action<float> progress)
		{
			Instance.StartCoroutine(Instance.GetProgress(resDatas,progress));
		}
		/** 普通进度 */
		public static void Progress(ResData resData,Action<float> progress)
		{
			Instance.StartCoroutine(Instance.GetProgress(resData,progress));
		}
		/** 普通进度协程 */
		private IEnumerator GetProgress(ResData resData,Action<float> progress)
		{
			while (!resData.isDone) 
			{
				progress (resData.progress);
				yield return new WaitForSeconds (0.1f);
			}
		}
		/** 进度协程 */
		private IEnumerator GetProgress(ResData[] resDatas,Action<float> progress)
		{
			while (true) 
			{
				bool isDone = true;
				float temp = 0;
				for (int i = 0; i < resDatas.Length; i++) 
				{
					temp += resDatas [i].progress;
					if (isDone&&!resDatas [i].isDone)
						isDone = false;
				}
				if (isDone) break;
				temp /= resDatas.Length;
				progress (temp);
				yield return new WaitForSeconds (0.1f);
			}
		}
	}
}
