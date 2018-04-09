using System;
using UnityEngine;

/// <summary>
/// Unity类扩展工具
/// </summary>
public static class UnityExtension
{
	/* static methods */
	/** 添加子节点 */
	public static GameObject AddChild(this GameObject @object,GameObject prefab)
	{
		prefab = UnityEngine.Object.Instantiate (prefab) as GameObject;
		Transform transform = prefab.transform;
		transform.SetParent(@object.transform);
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
		return prefab;
	}
	/** 获取脚本 */
	public static T GetOrAddComponent<T>(this GameObject @object) where T : Component
	{
		T component=@object.GetComponent<T> ();
		if (component) return component;
		return @object.AddComponent<T> ();
	}
	/** 查找指定组件 */
	public static T FindInParent<T>(this GameObject @object) where T : Component
	{
		Transform transform = @object.transform;
		T component = null;
		while (transform!=null) 
		{
			component=transform.GetComponent<T>();
			if(component!=null) return component;
			transform=transform.parent;
		}
		return null;
	}
	/** 改变Shader */
	public static void ChangeShader(this Transform transform)
	{
		for(int i=0,count=transform.childCount;i<count;i++)
		{
			transform.GetChild(i).ChangeShader();
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
}