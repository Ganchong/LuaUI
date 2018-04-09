using System;
using UnityEngine;

/// <summary>
/// 单例类脚本
/// </summary>
public class SingletonBehaviour<T>:MonoBehaviour where T : MonoBehaviour
{
	/* static fields */
	/** 单例 */
	private static T instance;

	/* static methods */
	/** 获取单例 */
	public static T Instance 
	{
		get {
			if (instance == null) {
				instance = FindObjectOfType(typeof(T)) as T;
				if(instance==null)
				{
					GameObject obj=new GameObject();
					obj.hideFlags=HideFlags.HideAndDontSave;
					instance=obj.AddComponent(typeof(T)) as T;
				}
			}
			return instance;
		}
	}
	protected virtual void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		if (instance == null)
		{
			instance = this as T;
		}
		else
		{
			Destroy(gameObject);
		}
	}
}