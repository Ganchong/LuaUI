using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ABSystem{
	/// <summary>
	/// 贴图导入器
	/// </summary>
	public class TextureAssetImporter : AssetPostprocessor
	{
		/* static fields */
		/** 是否处理中 */
		static bool handling = false;

		/* methods */

		/* static methods */
		/** 导入处理节点(只处理删除图片时，删除alpha贴图) */
		static void OnPostprocessAllAssets (string[] imported, string[] deleted, string[] moved, string[] movedFromAssetPaths) 
		{
			if (handling||deleted.Length < 1) return;
			handling = true;
			for (int i=0; i<deleted.Length; i++) 
			{
				if(!deleted[i].ToLower().EndsWith(".png")) continue;
				if(FileHelper.GetFileName(deleted[i]).StartsWith(ResData.ALPHAPREFIX)) continue;

				string fileName = FileHelper.GetFileName(deleted[i]);
				string assetPath=deleted[i].Substring (0, deleted[i].Length - fileName.Length) + ResData.ALPHAPREFIX + fileName;

				string filePath=Application.dataPath.Replace ("Assets", string.Empty) + assetPath;
				if(File.Exists(filePath))
				{
					AssetDatabase.DeleteAsset(assetPath);
				}
			}
			handling = false;
		}
		/** 初始导入贴图 */
		static void handleTextures(List<string> importList)
		{
			Texture texture;
			TextureImporter importer;
			int maxSize;
			TextureImporterFormat format;
			for (int i=0; i<importList.Count; i++) 
			{
				importer=AssetImporter.GetAtPath(importList[i]) as TextureImporter;
				bool alpha=importer.DoesSourceTextureHaveAlpha()||importer.grayscaleToAlpha;
				if(!alpha) {FixTextureImporterFormat(importer); continue;}

				texture=AssetDatabase.LoadAssetAtPath<Texture>(importList[i]);
				importer.GetPlatformTextureSettings("Android",out maxSize,out format);
				if(IsHaveAlpha(format,alpha))
				{
					DeleteAlphaTexture(texture);
					var andriodformat=format;
					importer.GetPlatformTextureSettings("iPhone",out maxSize,out format);
					if(!IsHaveAlpha(format,alpha))
					{
						if(andriodformat==TextureImporterFormat.ETC2_RGBA8||andriodformat==TextureImporterFormat.ARGB16
							||andriodformat==TextureImporterFormat.RGBA16) format=TextureImporterFormat.PVRTC_RGBA4;
						else format=andriodformat;
						importer.SetPlatformTextureSettings("iPhone",maxSize,format);
					}
					continue;
				}
				//			if(texture.width!=texture.height) continue;
				//			int width=texture.width;
				//			if((width&(width-1))!=0) continue;
				//			handleTexture(texture,importList[i],format);
			}
		}
		/** 导入贴图 */
		static void handleTexture(Texture texture,string assetPath,TextureImporterFormat format)
		{
			string saveAssetPath = GetAlphaSavePath(texture);
			string fullPath = Application.dataPath.Replace ("Assets", string.Empty) + saveAssetPath;
			if (File.Exists (fullPath)) return;

			texture=MakeTextureReadable (assetPath);
			CreateAlphaTexture (texture as Texture2D, false);
			setImportSetting (assetPath, Mathf.Max (texture.width, 32), format, format==TextureImporterFormat.ETC_RGB4?TextureImporterFormat.PVRTC_RGB4:format);
		}
		/** 设置指定图片导入格式 */
		static void setImportSetting(string assetPath,int maxSize,TextureImporterFormat androidFormat, TextureImporterFormat iosFormat)
		{
			var importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
			{
				importer.npotScale = TextureImporterNPOTScale.ToNearest;
				importer.isReadable = false;
				importer.mipmapEnabled = false;
				importer.alphaIsTransparency = true;
				importer.wrapMode = TextureWrapMode.Clamp;
				importer.filterMode = FilterMode.Bilinear;
				importer.anisoLevel = 0;
				importer.SetPlatformTextureSettings("Android", maxSize, androidFormat, 100, true);
				importer.SetPlatformTextureSettings("iPhone", maxSize, iosFormat, 100, true);
			}
			AssetDatabase.ImportAsset(assetPath);
		}
		[MenuItem("Texture/图片可读")]
		/** 让图片可读 */
		public static void MakeTextureReadable()
		{
			if (Selection.activeObject == null)	return;
			if (!Selection.activeObject is Texture)	return;
			MakeTextureReadable(AssetDatabase.GetAssetPath(Selection.activeObject));
		}
		[MenuItem("Texture/自动压缩")]
		/** 自动压缩 */
		public static void AutoCompress()
		{
			if (Selection.activeObject == null)	return;
			if (!Selection.activeObject is Texture)	return;
			string assetPath=AssetDatabase.GetAssetPath(Selection.activeObject);
			var importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);

			int maxSize = importer.maxTextureSize;
			bool alpha = importer.DoesSourceTextureHaveAlpha ();

			importer.isReadable = false;
			importer.SetPlatformTextureSettings("Android", maxSize, TextureImporterFormat.RGBA16, 100, true);
			importer.SetPlatformTextureSettings("iPhone", maxSize, TextureImporterFormat.PVRTC_RGBA4, 100, true);

			AssetDatabase.ImportAsset(assetPath,ImportAssetOptions.ForceUpdate|ImportAssetOptions.ForceSynchronousImport);

		}

		/** 让图片可读 */
		static Texture MakeTextureReadable(string assetPath)
		{
			var importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);

			int maxSize = importer.maxTextureSize;

			importer.isReadable = true;
			importer.SetPlatformTextureSettings("Android", maxSize, TextureImporterFormat.ARGB32, 100, true);
			importer.SetPlatformTextureSettings("iPhone", maxSize, TextureImporterFormat.ARGB32, 100, true);

			AssetDatabase.ImportAsset(assetPath,ImportAssetOptions.ForceUpdate|ImportAssetOptions.ForceSynchronousImport);

			return AssetDatabase.LoadAssetAtPath<Texture> (assetPath);
		}
		/** 创建alpha图片 */
		static string CreateAlphaTexture(Texture2D texture, bool alphaHalfSize)
		{
			var srcPixels = texture.GetPixels();
			var tarPixels = new Color[srcPixels.Length];
			for (int i = 0; i < srcPixels.Length; i++)
			{
				float r = srcPixels[i].a;
				tarPixels[i] = new Color(r, r, r);
			}
			Texture2D alphaTex = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
			alphaTex.SetPixels(tarPixels);
			alphaTex.Apply();

			string saveAssetPath = GetAlphaSavePath(texture);
			string fullPath = Application.dataPath.Replace ("Assets", string.Empty) + saveAssetPath;
			var bytes = alphaTex.EncodeToPNG ();
			File.WriteAllBytes(fullPath, bytes);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

			return saveAssetPath;
		}
		/** 创建左乘图片 */
		static string CreatePrimaryTexture(Texture2D texture, bool alphaHalfSize)
		{
			var srcPixels = texture.GetPixels();
			var tarPixels = new Color[srcPixels.Length];
			for (int i = 0; i < srcPixels.Length; i++)
			{
				tarPixels[i] = new Color(srcPixels[i].r*srcPixels[i].a, srcPixels[i].g*srcPixels[i].a, srcPixels[i].b*srcPixels[i].a,srcPixels[i].a);
			}
			Texture2D alphaTex = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
			alphaTex.SetPixels(tarPixels);
			alphaTex.Apply();

			string saveAssetPath = GetPrimarySavePath(texture);
			string fullPath = Application.dataPath.Replace ("Assets", string.Empty) + saveAssetPath;
			var bytes = alphaTex.EncodeToPNG ();
			File.WriteAllBytes(fullPath, bytes);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

			return saveAssetPath;
		}

		/** 删除Alpha通道图 */
		static void DeleteAlphaTexture(Texture texture)
		{
			string saveAssetPath = GetAlphaSavePath (texture);
			string fullPath = Application.dataPath.Replace ("Assets", string.Empty) + saveAssetPath;
			if(File.Exists(fullPath)) AssetDatabase.DeleteAsset (saveAssetPath);
		}

		/** 获取Alpha贴图保存位置 */
		static string GetAlphaSavePath(Texture src)
		{
			string path = AssetDatabase.GetAssetPath(src);
			string fileName = FileHelper.GetFileName(path);

			return path.Substring (0, path.Length - fileName.Length) + ResData.ALPHAPREFIX + fileName;
		}
		/** 获取左乘贴图保存位置 */
		static string GetPrimarySavePath(Texture src)
		{
			string path = AssetDatabase.GetAssetPath(src);
			string fileName = FileHelper.GetFileName(path);

			return path.Substring (0, path.Length - fileName.Length) + "primary_" + fileName;
		}

		/** 修正贴图压缩方式 */
		public static void FixTextureImporterFormat(TextureImporter importer)
		{
			int maxSize;
			TextureImporterFormat format;

			importer.GetPlatformTextureSettings ("Android", out maxSize, out format);
			if (format == TextureImporterFormat.ARGB32) {
				format=TextureImporterFormat.RGB24;
				importer.SetPlatformTextureSettings ("Android", maxSize, format);
			}
			if (format == TextureImporterFormat.ARGB16) {
				format=TextureImporterFormat.RGB16;
				importer.SetPlatformTextureSettings ("Android", maxSize, format);
			}

			importer.GetPlatformTextureSettings ("iPhone", out maxSize, out format);
			if (format == TextureImporterFormat.ARGB32) {
				format=TextureImporterFormat.RGB24;
				importer.SetPlatformTextureSettings ("iPhone", maxSize, format);
			}
			if (format == TextureImporterFormat.ARGB16) {
				format=TextureImporterFormat.RGB16;
				importer.SetPlatformTextureSettings ("iPhone", maxSize, format);
			}
		}
		/** 是否拥有alpha */
		public static bool IsHaveAlpha(TextureImporterFormat format,bool alpha)
		{
			switch (format) 
			{
			case TextureImporterFormat.ARGB16:
			case TextureImporterFormat.ARGB32:
			case TextureImporterFormat.ETC2_RGBA8:
			case TextureImporterFormat.RGBA16:
			case TextureImporterFormat.RGBA32:
			case TextureImporterFormat.PVRTC_RGBA4:
				return true;
			case TextureImporterFormat.AutomaticCompressed:
			case TextureImporterFormat.AutomaticTruecolor:
			case TextureImporterFormat.Automatic16bit:
				return alpha;
			}
			return false;
		}
		[MenuItem("Texture/分离Alpha通道")]
		public static void FindFiles()
		{
			if (Selection.objects == null || Selection.objects.Length < 1)
				return;
			UnityEngine.Object[] objects = Selection.objects;
			TextureImporter importer = null;
			string assetPath = null;
			int index = 0;
			EditorApplication.update = () => {
				assetPath = AssetDatabase.GetAssetPath (objects [index]);
				EditorUtility.DisplayProgressBar("分离Alpha通道",assetPath,index*1.0f/objects.Length);
				if (!objects [index] is Texture2D) {
					Debug.LogError (assetPath);
				}
				else
				{

					MakeTextureReadable (assetPath);
					importer = AssetImporter.GetAtPath (assetPath) as TextureImporter;
					if (!importer.isReadable) { 
						Debug.LogError (assetPath);
					}
					else CreateAlphaTexture (objects [index] as Texture2D, false);
				}
				//setImportSetting(assetPath,importer.maxTextureSize,TextureImporterFormat.ETC_RGB4,TextureImporterFormat.PVRTC_RGB4);
				index++;
				if(index>=objects.Length)
				{
					EditorApplication.update=null;
					EditorUtility.ClearProgressBar();
					Debug.Log("分离完成");
				}
			};
		}
		[MenuItem("Texture/导出左乘图")]
		public static void CreatePrimaryTextureFindFiles()
		{
			if (Selection.objects == null || Selection.objects.Length < 1)
				return;
			UnityEngine.Object[] objects = Selection.objects;
			TextureImporter importer = null;
			string assetPath = null;
			int index = 0;
			EditorApplication.update = () => {
				assetPath = AssetDatabase.GetAssetPath (objects [index]);
				EditorUtility.DisplayProgressBar("导出左乘图",assetPath,index*1.0f/objects.Length);
				if (!objects [index] is Texture2D) {
					Debug.LogError (assetPath);
				}
				else
				{
					MakeTextureReadable (assetPath);
					importer = AssetImporter.GetAtPath (assetPath) as TextureImporter;
					if (!importer.isReadable) { 
						Debug.LogError (assetPath);
					}
					else CreatePrimaryTexture (objects [index] as Texture2D, false);
				}
				//setImportSetting(assetPath,importer.maxTextureSize,TextureImporterFormat.ETC_RGB4,TextureImporterFormat.PVRTC_RGB4);
				index++;
				if(index>=objects.Length)
				{
					EditorApplication.update=null;
					EditorUtility.ClearProgressBar();
					Debug.Log("分离完成");
				}
			};
		}
		[MenuItem("Texture/自动处理特效图片")]
		public static void AutoCompressTexture() 
		{
			if (Selection.activeObject == null) return;
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (!string.IsNullOrEmpty(path))
			{
				path = (Application.dataPath.Replace ("Assets", string.Empty) + path).Replace ('/', '\\');
				List<string> withExtensions = new List<string>(){".png",".tga",".jpg"};
				string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
					.Where(s => withExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
				int startIndex = 0;

				EditorApplication.update = delegate()
				{
					string file = files[startIndex];

					bool isCancel = EditorUtility.DisplayCancelableProgressBar("处理贴图中", file, (float)startIndex / (float)files.Length);

					string assetPath=GetRelativeAssetsPath(file);

					var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

					int maxSize = importer.maxTextureSize;
					bool alpha = importer.DoesSourceTextureHaveAlpha ();

					importer.textureType = TextureImporterType.Default;
					importer.npotScale = TextureImporterNPOTScale.ToNearest;
					importer.isReadable = false;
					importer.mipmapEnabled = false;

					if(alpha||importer.grayscaleToAlpha)
					{
						importer.SetPlatformTextureSettings("Android", maxSize, TextureImporterFormat.RGBA16, 50, true);
						importer.SetPlatformTextureSettings("iPhone", maxSize, TextureImporterFormat.PVRTC_RGBA4, 50, true);
					}
					else
					{
						importer.SetPlatformTextureSettings("Android", maxSize, TextureImporterFormat.ETC_RGB4, 50, true);
						importer.SetPlatformTextureSettings("iPhone", maxSize, TextureImporterFormat.PVRTC_RGB4, 50, true);
					}

					AssetDatabase.ImportAsset(assetPath,ImportAssetOptions.ForceUpdate|ImportAssetOptions.ForceSynchronousImport);

					startIndex++;
					if (isCancel || startIndex >= files.Length)
					{
						EditorUtility.ClearProgressBar();
						EditorApplication.update = null;
						startIndex = 0;
						Debug.Log("处理结束");
					}
				};
			}
		}
		/** 修正windows路径 */
		private static string Replace(string path)
		{
			return path.Replace ('\\', '/');
		}
		static private string GetRelativeAssetsPath(string path)
		{
			return Replace (path).Replace (ResourceEditorHelper.ASSETROOT, string.Empty);
		}
	}
}
