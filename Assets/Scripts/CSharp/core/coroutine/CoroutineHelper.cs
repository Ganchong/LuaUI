using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 协程帮助器
/// </summary>
public static class CoroutineHelper
{
	/* static methods */
	/** 延迟等待执行 */
	public static void DelayRun(this MonoBehaviour behaviour,float time,Action action)
	{
		behaviour.StartCoroutine (WaitSecond (time, action));
	}
	/** 延迟等待帧执行 */
	public static void DelayRun(this MonoBehaviour behaviour,int frame,Action action)
	{
		behaviour.StartCoroutine (WaitFrame (frame, action));
	}
	/** 延迟等待帧结束 */
	public static void EndOfFrame(this MonoBehaviour behaviour,Action action)
	{
		behaviour.StartCoroutine (WaitEndOfFrame (action));
	}

	/** 等待枚举器 */
	private static IEnumerator WaitSecond(float time,Action action)
	{
		yield return new WaitForSeconds(time);
		object target = action.Target;
		if (target != null) 
		{
			UnityEngine.Object obj = target as UnityEngine.Object;
			if (object.ReferenceEquals (obj, null) || obj != null)
				action.Invoke();
		}
		else action.Invoke();
	}
	/** 等待枚举器 */
	private static IEnumerator WaitFrame(int frame,Action action)
	{
		while (frame>0) 
		{
			yield return null;
			frame--;
		}
		object target = action.Target;
		if (target != null) 
		{
			UnityEngine.Object obj = target as UnityEngine.Object;
			if (object.ReferenceEquals (obj, null) || obj != null)
				action.Invoke();
		}
		else action.Invoke();
	}
	/** 等待帧结束 */
	private static IEnumerator WaitEndOfFrame(Action action)
	{
		yield return new WaitForEndOfFrame ();
		object target = action.Target;
		if (target != null) 
		{
			UnityEngine.Object obj = target as UnityEngine.Object;
			if (object.ReferenceEquals (obj, null) || obj != null)
				action.Invoke();
		}
		else action.Invoke();
	}
}