using UnityEngine;
using System.Collections;
public class UIUtils
{
	/** 添加子物体 */
	public static void AddChild(GameObject parent,GameObject child)
	{
		AddChild(parent.transform,child.transform,true);
	}

	/** 添加子物体 */
	public static void AddChild(GameObject parent,Transform child)
	{
		AddChild(parent.transform,child,true);
	}

	/** 添加子物体 */
	public static void AddChild(Transform parent,GameObject child)
	{
		AddChild(parent,child.transform,true);
	}

	/** 添加子物体 */
	public static void AddChild(Transform parent,Transform child)
	{
		AddChild(parent,child,true);
	}


	public static void AddChild(Transform parent,Transform child,bool resetChild)
	{
		child.parent=parent;
		if(resetChild)
		{
			child.localPosition=Vector3.zero;
			child.localScale=Vector3.one;
			child.localRotation=Quaternion.Euler(Vector3.zero);
		}
	}

	/// <summary>
	/// M_adds the child.
	/// </summary>
	/// <param name="parent">parent.</param>
	/// <param name="child">child.</param>
	/// <param name="_position">_position.</param>
	/// <param name="_isLocalPosition">If set to <c>true</c> _is local position.</param>
	/// <param name="_resetScale">If set to <c>true</c> _reset scale.</param>
	public static void AddChild(GameObject parent,GameObject child,Vector3 _position,bool _isLocalPosition,bool _resetScale)
	{
		Transform childTransform=child.transform;
		childTransform.parent=parent.transform;
		if(_isLocalPosition)
		{
			childTransform.localPosition=_position;
		}else
		{
			childTransform.position=_position;
		}
		if(_resetScale)
		{
			child.transform.localScale=Vector3.one;
		}
		child.transform.localRotation=Quaternion.Euler(Vector3.zero);
	}
	public static GameObject AddChild(GameObject parent,GameObject child,Vector3 _position,bool _isLocalPosition,bool _resetScale,string restName)
	{
		Transform childTransform=child.transform;
		childTransform.parent=parent.transform;
		if(_isLocalPosition)
		{
			childTransform.localPosition=_position;
		}else
		{
			childTransform.position=_position;
		}
		if(_resetScale)
		{
			child.transform.localScale=Vector3.one;
		}
		child.name = restName;
		child.transform.localRotation=Quaternion.Euler(Vector3.zero);
		return child;
	}


	/** 移除所有子物体 */
	public static void RemoveChilds(Transform parent)
	{
		int childCount=parent.childCount;
		for(int i=childCount-1;i>=0;i--)
		{
			parent.GetChild(i).gameObject.SetActive(false);
			MonoBehaviour.DestroyImmediate(parent.GetChild(i).gameObject);
		}
	}

	/** 改变某个物体的层 */
	public static void setLayer(GameObject gameObj,int layer){
		for(int i=0;i<gameObj.transform.childCount;i++){
			gameObj.layer = layer;
			gameObj.transform.GetChild(i).gameObject.layer = layer;
			setLayer(gameObj.transform.GetChild(i).gameObject,layer);
		}
	}

	/** 移除所有子物体 */
	public static void RemoveChilds(GameObject parent)
	{
		RemoveChilds(parent.transform);
	}


	public static void Trace(params object[] _parameters)
	{
		foreach(object item in _parameters)
		{
			Log.debug("==:"+item.ToString());
		}
	}
}

