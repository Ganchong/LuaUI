using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object=UnityEngine.Object;

namespace ABSystem{
	/// <summary>
	/// 资源
	/// </summary>
	public sealed class ResData
	{
		/* static fields */
		/** 主资源路径 */
		public const string MAINPATH="Assets/Art/";
		/** alpha贴图前缀 */
		public const string ALPHAPREFIX="~";

		/* fields */
		/** 文件名称 */
		string fileName;

		/** 是否加载完成 */
		public bool _isDone;
		/** 进度 */
		public float _progress;
		/** 路径 */
		public string path;
		/** 引用次数 */
		public int retainCount;
		/** 当前资源 */
		public Object _object;
		/** 贴图alpha专用 */
		public Texture alpha;
		/** 依赖资源 */
		public ResData[] references;
		/** AB包 */
		public AssetBundle assetBundle;

		/** 上次使用时间 */
		public float useTime=float.MaxValue;

		/** 贴图对象 */
		public Texture[] textures;

		/* constructor */
		/** 构造方法 */
		public ResData(string path)
		{
			this.path = path;
			this.fileName = FileHelper.GetFileName (path);
		}

		/* properties */
		/** 是否完成 */
		public bool isDone
		{
			get { 
				if (!_isDone) return false;
				if (references != null) 
				{
					for (int i = 0; i < references.Length; i++) 
					{
						if (!references [i].isDone) return false;
					}
				}
				return true;
			}
		}
		/** 进度 */
		public float progress
		{
			get {
				float temp = _progress;
				int count = references == null ? 0 : references.Length;
				for (int i = 0; i < count; i++) 
				{
					temp+=references [i].progress;
				}
				return temp / (count + 1);
			}
		}

		/* methods */
		/** 增加引用 */
		public void Retain()
		{
			retainCount++;
			if (references != null) 
			{
				for (int i = 0; i < references.Length; i++) 
				{
					references [i].Retain ();
				}
			}
		}
		/** 释放资源 */
		public void Release()
		{
			ResourceHelper.Instance.Release (this);
		}
		/** 加载主资源 */
		public T LoadMainAsset<T> () where T : Object
		{
			#if UNITY_EDITOR
			if(retainCount<0) throw new Exception("Resource is Released");
			#endif
			if (_object != null) return (T)_object;
			if (assetBundle != null) 
			{
				_object = assetBundle.LoadAsset<T> (fileName);
				this.release (false);
				return (T)_object;
			}
			#if UNITY_EDITOR
			_object=UnityEditor.AssetDatabase.LoadAssetAtPath<T>(getFileSuffix<T>(MAINPATH+path));
			return (T)_object;
			#else
			return null;
			#endif
		}
		public void LoadMainAssetAsync<T>(Action<T> action) where T : Object
		{
			ResourceHelper.Instance.StartCoroutine(LoadMainAssetAsyncIE<T>(action));
		}
		/** 异步加载主资源 */
		private IEnumerator LoadMainAssetAsyncIE<T>(Action<T> action) where T : Object
		{
			#if UNITY_EDITOR
			if(retainCount<0) throw new Exception("Resource is Released");
			#endif
			if (_object != null) {
				action ((T)_object);
			} else {
				if (assetBundle != null) {
					AssetBundleRequest request = assetBundle.LoadAssetAsync<T> (fileName);
					while(!request.isDone)
					{
						yield return 0;
					}
					_object = request.asset;
					this.release (false);
					action ((T)_object);
				} else {
					#if UNITY_EDITOR
					_object=UnityEditor.AssetDatabase.LoadAssetAtPath<T>(getFileSuffix<T>(MAINPATH+path));
					action((T)_object);
					#else
					action(null);
					Debug.LogError("asset not find,path:"+path);
					#endif
				}
			}
		}
		/** 同步加载 */
		public T LoadAsset<T>(string name) where T : Object
		{
			#if UNITY_EDITOR
			if(retainCount<0) throw new Exception("Resource is Released");
			#endif
			if (assetBundle != null)
				return assetBundle.LoadAsset<T> (name);
			#if UNITY_EDITOR
			return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(getFileSuffix<T>(MAINPATH+path+FileHelper.FILESPEARATOR+name));
			#else
			Debug.LogError("asset not find,path:"+path+FileHelper.FILESPEARATOR+name);
			return null;
			#endif
		}
		/** 异步加载 */
		public void LoadAssetAsync<T>(string name,Action<T> action) where T : Object
		{
			ResourceHelper.Instance.StartCoroutine(LoadAssetAsyncIE<T>(name,action));
		}
		/** 异步加载IE */
		private IEnumerator LoadAssetAsyncIE<T>(string name,Action<T> action) where T : Object
		{
			#if UNITY_EDITOR
			if(retainCount<0) throw new Exception("Resource is Released");
			#endif
			if (assetBundle != null) {
				AssetBundleRequest request = assetBundle.LoadAssetAsync<T> (name);
				while(!request.isDone)
				{
					yield return 0;
				}
				action ((T)request.asset);
			} else {
				yield return 0;
				#if UNITY_EDITOR
				action(UnityEditor.AssetDatabase.LoadAssetAtPath<T>(getFileSuffix<T>(MAINPATH+path+FileHelper.FILESPEARATOR+name)));
				#else
				Debug.LogError("asset not find,path:"+path+FileHelper.FILESPEARATOR+name);
				action(null);
				#endif
			}
		}
		/** 贴图加载 */
		public void LoadTextureAsset(out Texture mainTex,out Texture alphaTex)
		{
			#if UNITY_EDITOR
			if(retainCount<0) throw new Exception("Resource is Released");
			#endif
			if (_object != null) 
			{
				mainTex=_object as Texture;
				alphaTex=alpha;
				return;
			}
			if (assetBundle != null)
				_object = assetBundle.LoadAsset<Texture> (fileName);
			#if UNITY_EDITOR
			else {
				_object=UnityEditor.AssetDatabase.LoadAssetAtPath<Texture> (getFileSuffix<Texture> (MAINPATH + path));
			}
			#endif
			string alphaName = ALPHAPREFIX + fileName;
			if (assetBundle != null && assetBundle.Contains (alphaName))
				alpha = LoadAsset<Texture> (alphaName);
			if (assetBundle !=null) this.release (false);
			#if UNITY_EDITOR
			if(alpha==null&&_object!=null)
			{
				string assetPath=getFileSuffix<Texture> (MAINPATH + path);
				UnityEditor.TextureImporter importer=UnityEditor.AssetImporter.GetAtPath(assetPath) as UnityEditor.TextureImporter;
				int maxSize;
				UnityEditor.TextureImporterFormat format;
			#if UNITY_ANDROID || UNITY_EDITOR_WIN
				importer.GetPlatformTextureSettings("Android",out maxSize,out format);
			#elif UNITY_IPHONE || UNITY_EDITOR_OSX
			importer.GetPlatformTextureSettings("iPhone",out maxSize,out format);
			#endif
				if(!IsHaveAlpha(importer.textureCompression,format,importer.DoesSourceTextureHaveAlpha()))
				{
					assetPath=MAINPATH+path.Substring(0,path.Length-fileName.Length) +alphaName;
					assetPath=getFileSuffix<Texture>(assetPath);
					string filePath=Application.dataPath + assetPath.Replace ("Assets", "");
					if(System.IO.File.Exists(filePath))
						alpha=UnityEditor.AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
				}
			}
			#endif
			mainTex=_object as Texture;
			alphaTex=alpha;
		}
		/** 异步加载贴图 */
		public void LoadTextureAssetAsync(Action<Texture[]> action)
		{
			ResourceHelper.Instance.StartCoroutine (LoadTextureAssetAsyncIE (action));
		}
		/** 异步加载贴图IE */
		private IEnumerator LoadTextureAssetAsyncIE(Action<Texture[]> action)
		{
			#if UNITY_EDITOR
			if(retainCount<0) throw new Exception("Resource is Released");
			#endif
			if (textures != null) {
				action (textures);
			} else {
				if (assetBundle != null)
				{
					AssetBundleRequest request=assetBundle.LoadAssetAsync<Texture> (fileName);
					while(!request.isDone)
					{
						yield return 0;
					}
					_object = request.asset;
				}
				#if UNITY_EDITOR
				else {
					yield return 0;
					_object=UnityEditor.AssetDatabase.LoadAssetAtPath<Texture> (getFileSuffix<Texture> (MAINPATH + path));
				}
				#endif
				string alphaName = ALPHAPREFIX + fileName;
				if (assetBundle != null && assetBundle.Contains (alphaName))
				{
					AssetBundleRequest request=assetBundle.LoadAssetAsync<Texture> (alphaName);
					while(!request.isDone)
					{
						yield return 0;
					}
					alpha = request.asset as Texture;
				}
				if (assetBundle !=null) this.release (false);
				#if UNITY_EDITOR
				if(alpha==null&&_object!=null)
				{
					yield return 0;
					string assetPath=getFileSuffix<Texture> (MAINPATH + path);
					UnityEditor.TextureImporter importer=UnityEditor.AssetImporter.GetAtPath(assetPath) as UnityEditor.TextureImporter;
					int maxSize;
					UnityEditor.TextureImporterFormat format;
				#if UNITY_ANDROID || UNITY_EDITOR_WIN
					importer.GetPlatformTextureSettings("Android",out maxSize,out format);
				#endif
				#if UNITY_IPHONE || UNITY_EDITOR_OSX
				importer.GetPlatformTextureSettings("iPhone",out maxSize,out format);
				#endif
					if(!IsHaveAlpha(importer.textureCompression,format,importer.DoesSourceTextureHaveAlpha()))
					{
						assetPath=MAINPATH+path.Substring(0,path.Length-fileName.Length) +alphaName;
						assetPath=getFileSuffix<Texture>(assetPath);
						string filePath=Application.dataPath + assetPath.Replace ("Assets", "");
						if(System.IO.File.Exists(filePath))
							alpha=UnityEditor.AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
					}
				}
				#endif
				textures = new Texture[]{_object as Texture,alpha};
				action(textures);
			}
		}
		public override string ToString ()
		{
			return string.Format ("[ResData: _isDone={0}, path={1}, retainCount={2}, _object={3}, assetBundle={4}]", _isDone, path, retainCount, _object, assetBundle);
		}


		#if UNITY_EDITOR
		/** 获取资源后缀 */
		public static string getFileSuffix<T>(string path)
		{
			string filePath = Application.dataPath + path.Replace ("Assets", "");
			int index=filePath.LastIndexOf (FileHelper.FILESPEARATOR);
			string name = filePath.Substring (index);
			bool isDictory = System.IO.Directory.Exists (filePath);

			string suffix = string.Empty;
			if (typeof(T) == typeof(GameObject))
				suffix=".prefab";
			if (typeof(T) == typeof(TextAsset)) 
			{
				if(System.IO.File.Exists (path + ".bytes"))
					suffix=".bytes";
				else suffix=".txt";
			}
			if (typeof(T) == typeof(Texture))
			{
				if(System.IO.File.Exists (path + ".jpg"))
					suffix=".jpg";
				else suffix=".png";
			}
			if (typeof(T) == typeof(AudioClip))
			{
				if(System.IO.File.Exists (path + ".ogg"))
					suffix=".ogg";
				else if(System.IO.File.Exists (path + ".mp3"))
					suffix=".mp3";
				else if(System.IO.File.Exists (path + ".wav"))
					suffix=".wav";
			}
			if (typeof(T) == typeof(UnityEngine.U2D.SpriteAtlas)) {
				if(System.IO.File.Exists (path + ".spriteatlas"))
					suffix=".spriteatlas";
			}
			return isDictory?path+name+suffix:path+suffix;
		}
		/** 是否拥有alpha */
		public static bool IsHaveAlpha(UnityEditor.TextureImporterCompression compression,UnityEditor.TextureImporterFormat format,bool alpha)
		{
			switch (format) 
			{
			case UnityEditor.TextureImporterFormat.ARGB16:
			case UnityEditor.TextureImporterFormat.ARGB32:
			case UnityEditor.TextureImporterFormat.ETC2_RGBA8:
			case UnityEditor.TextureImporterFormat.RGBA16:
			case UnityEditor.TextureImporterFormat.RGBA32:
			case UnityEditor.TextureImporterFormat.PVRTC_RGBA4:
				return true;
			}
			switch (compression) 
			{
			case UnityEditor.TextureImporterCompression.Compressed:
			case UnityEditor.TextureImporterCompression.CompressedHQ:
			case UnityEditor.TextureImporterCompression.CompressedLQ:
			case UnityEditor.TextureImporterCompression.Uncompressed:
				return alpha;
			}
			return false;
		}
		#endif

	}
}
