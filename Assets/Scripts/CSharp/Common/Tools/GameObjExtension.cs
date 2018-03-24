using System;
using UnityEngine;

public static class GameObjExtension {

	public static T AddMissCompoent<T>(this GameObject go) where T:Component
	{
		T t = go.GetComponent<T> ();
		if (t == null)
			t = go.AddComponent<T>();
		return t;
	}

	public static void DestroySelf(this GameObject go)
	{
		GameObject.Destroy (go);
	}
}
