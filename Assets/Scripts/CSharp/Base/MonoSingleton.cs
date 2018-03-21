using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuaFrameWorkCore
{
	/** 单例 */
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {

		private static T instance = null;
		public static T Instance {
			get{
				if(instance == null){
					instance = GameObject.FindObjectOfType(typeof(T)) as T;
					if(instance == null){
						instance = new GameObject(typeof(T).ToString(),typeof(T)).GetComponent<T>();
						GameObject gameManager = GameObject.Find("GameManager");
						instance.transform.SetParent(gameManager==null?null:gameManager.transform);
					}
				}
				return instance;
			}
		}
	}
}


