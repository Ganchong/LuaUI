using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// 协程中心
/// 汪松民
/// </summary>
public class CoroutineCenter : SingletonBehaviour<CoroutineCenter>
{
	/* static fields */
	/** 帧结束 */
	public static WaitForEndOfFrame WaitForEndOfFrame=new WaitForEndOfFrame();

	/* fields */
	/** 协程池 */
	ArrayListYK<IEnumerator> list = new ArrayListYK<IEnumerator>();

	/* methods */
	/** 更新方法 */
	private void Update()
	{
		if(list.size() < 1) return;
		IEnumerator ie = list.removeAt(0);
		StartCoroutine(ie);
	}

	/** 执行方法 */
	public void excute(IEnumerator ie)
	{
		list.add(ie);
	}

	/** 停止所有 */
	public void stop()
	{
		list.clear();
	}
	/** 停止指定 */
	public void stop(IEnumerator ie)
	{
		if(!list.remove(ie))
		{
			StopCoroutine(ie);
		}
	}
	/** 延迟执行器(返回值用于终止协程) */
	public static IEnumerator delayRun(float delayTime,Action action)
	{
		if(delayTime <= 0)
		{
			action();
			return null;
		}
		IEnumerator ie = delayIEnumerator (delayTime, action);
		CoroutineCenter.Instance.excute(ie);
		return ie;
	}
	/** 延迟执行器帧率(返回值用于终止协程) */
	public static IEnumerator delayRunFrame(Action action,int frame)
	{
		IEnumerator ie = delayRunFrameIE (action, frame);
		CoroutineCenter.Instance.excute(ie);
		return ie;
	}

	/** 延迟帧率 */
	public static IEnumerator delayRunFrameIE(Action action,int frame)
	{
		for(int i=0;i<frame;i++)
			yield return null;
		action();
	}

	/** 延迟IEnumerator */
	public static IEnumerator delayIEnumerator(float delayTime,Action action)
	{
		yield return new WaitForSeconds(delayTime);
		action();
	}
	/** 帧结束后执行 */
	public static void RunEndOfFrame(Action action)
	{
		Instance.StartCoroutine(EndOfFrame(action));
	}
	private static IEnumerator EndOfFrame(Action action)
	{
		yield return WaitForEndOfFrame;
		if (action != null) action ();
	}
}

