using System;
using UnityEngine;
using System.Collections.Generic;

namespace ABSystem{
	/// <summary>
	/// 资源管理器
	/// </summary>
	public class ResourcesManager : Singleton<ResourcesManager>
	{
		/* static fields */
		/** Shader路径 */
		private const string SHADERPATH="shaders";
		/** 默认Alpha分离的Shader名称 */
		public const string DefaultAlphaShaderName = "Unlit/Transparent Colored two";
		/** 默认Alpha分离的Shader名称 */
		public const string DefaultShaderName = "Unlit/Transparent Colored";

		/* fields */
		/** 缓存对象 */
		ResData[] resDatas;
		/** Shader */
		Dictionary<string,Shader> shaderMap = new Dictionary<string, Shader>();
		/** Shader资源 */
		ResData shaderData;

		/* methods */
		/** 基础缓存 */
		public void cache(string[] paths,Action action)
		{
			if (resDatas != null) resDatas.Release ();
			int count = 0;
			Action action1 = () => {count++;if(count>1) action();};
			resDatas=ResourceHelper.Instance.LoadResDatasAsync (paths, (datas)=>{
				action1();
			});
			resDatas.Retain ();
			if (shaderData != null) shaderData.Release ();
			shaderData = ResourceHelper.Instance.LoadResDataAsync (SHADERPATH, (data) => {
				#if UNITY_EDITOR
				if (!GameManager.Instance.isUseAB) {action ();return;}
				#endif
				ResourceHelper.Instance.StartCoroutine(LoadAllAssetsAsync<Shader>(data.assetBundle,(shaders)=>{
					Shader shader = null;
					for (int j=0; j<shaders.Length; j++) 
					{
						shader = shaders [j] as Shader;
						if (shaderMap.ContainsKey (shader.name))
							shaderMap [shader.name] = shader;
						else
							shaderMap.Add (shader.name, shader);
					}
					action1 ();
				}));
			});
			shaderData.Retain ();
		}
		/** 异步加载所有资源 */
		private System.Collections.IEnumerator LoadAllAssetsAsync<T>(AssetBundle assetBundle,Action<UnityEngine.Object[]> action) where T : UnityEngine.Object
		{
			var request=assetBundle.LoadAllAssetsAsync<T> ();
			yield return request;
			action (request.allAssets);
		}
		public Shader findShader(string name)
		{
			#if UNITY_EDITOR
			return Shader.Find(name);
			#else
			Shader shader = null;
			if (!shaderMap.TryGetValue(name,out shader))
			shader = Shader.Find (name);
			if (shader == null) {
			Debug.LogError(name + " "+ shaderMap.Count);
			}
			return shader;
			#endif
		}
		#if UNITY_EDITOR
		/** 改变Shader */
		public static void ChangeShader(Transform transform)
		{
			for(int i=0,count=transform.childCount;i<count;i++)
			{
				ChangeShader(transform.GetChild(i));
			}
			Renderer renderer=transform.GetComponent<Renderer>();
			if (renderer == null) return;
			Material material = renderer.material;
			if (material != null) 
			{
				Shader shader = material.shader;
				if (shader != null)
					material.shader = Shader.Find (shader.name);
			}
			material = renderer.sharedMaterial;
			if (material != null) 
			{
				Shader shader = material.shader;
				if (shader != null)
					material.shader = Shader.Find (shader.name);
			}
			Material[] shareMaterials = renderer.sharedMaterials;
			int length = shareMaterials == null ? 0 : shareMaterials.Length;
			for (int i=0; i<length; i++) 
			{
				material = shareMaterials[i];
				if (material != null) 
				{
					Shader shader = material.shader;
					if (shader != null)
						material.shader = Shader.Find (shader.name);
				}
			}
			shareMaterials = renderer.materials;
			length = shareMaterials == null ? 0 : shareMaterials.Length;
			for (int i=0; i<length; i++) 
			{
				material = shareMaterials[i];
				if (material != null) 
				{
					Shader shader = material.shader;
					if (shader != null)
						material.shader = Shader.Find (shader.name);
				}
			}
		}
		#endif
		/** 从缓存对象中拿游戏对象 */
		public ResData getObjectFromCache(string name)
		{
			name = name.ToLower ();
			int length=resDatas==null?0:resDatas.Length;
			for(int i=0;i<length;i++)
			{
				if (resDatas [i].path == name)
					return resDatas[i];
			}
			Debug.LogError("ResData:" + name + "is not exits!",null);
			return null;
		}
		/** 异步加载资源 */
		public System.Collections.IEnumerator LoadAsync<T>(string path,Action<T> action) where T : UnityEngine.Object
		{
			yield return null;
			var request=Resources.LoadAsync<T> (path);
			yield return request;
			action ((T)request.asset);
		}

		public Shader GetShader(string name)
		{
			Shader shader = null;
			shaderMap.TryGetValue(name,out shader);
			return shader;
		}
	}

}
