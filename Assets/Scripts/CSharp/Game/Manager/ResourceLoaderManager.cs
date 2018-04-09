using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ABSystem;
using System;
using ABSystem;

/// <summary>
/// 资源加载器,依赖于ABSystem
/// </summary>
public class ResourceLoaderManager : Singleton<ResourceLoaderManager>
{
	/** UI资源路径 */
	public const string UIPATH = "UI/";

	/** 缓存资源 */
	Dictionary<string ,ResData> cacheResData = new Dictionary<string, ResData> ();
	/** 缓存GameObj*/
	Dictionary<string ,GameObject> objectMap = new Dictionary<string, GameObject> ();

	/** 加载UIObj */
	public void LoadUIObj (string name,Transform parent, Action<GameObject> callback)
	{
		ResData resData;
		GameObject obj = null;
		bool exist = objectMap.TryGetValue (name, out obj);
		if (exist && obj == null)
			objectMap.Remove (name);
		if (!cacheResData.TryGetValue (name, out resData)) {
			resData = ResourceHelper.Instance.LoadResData (UIPATH + name);
			resData.Retain ();
			cacheResData.Add (name, resData);
		}
		obj = resData.LoadMainAsset<GameObject> ();
		obj = GameObject.Instantiate (obj) as GameObject;
		obj.name = name;
		if (parent != null) {
			obj.transform.parent = parent;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = Vector3.one;
		}
		objectMap.Add (name, obj);
		obj.SetActive (true);
		if (callback != null)
			callback (obj);
	}
}

