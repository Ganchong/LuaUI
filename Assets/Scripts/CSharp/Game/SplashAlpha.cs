using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashAlpha : MonoBehaviour 
{
	public Color startColor;
	public Color endColor;
	[Tooltip("闪屏1渐变时间")]
	public float time1;
	[Tooltip("闪屏2渐变阶段1")]
	public float time2;
	[Tooltip("闪屏2停顿时间")]
	public float spauseTime;
	[Tooltip("闪屏2渐变阶段2")]
	public float time3;
	public RawImage loginLoading1;
	public RawImage loginLoading2;
	float startTime=-1;
	Action action;
	bool flag=false;

	void Awake()
	{
		if(Application.platform==RuntimePlatform.IPhonePlayer||Application.platform==RuntimePlatform.OSXPlayer||Application.platform==RuntimePlatform.OSXEditor)
		{
			time1=0;
			loginLoading1.enabled=false;
		}
	}

	void Start () 
	{
		loginLoading1.color = endColor;
		loginLoading2.color = startColor;
	}

	void Update () 
	{
		if (startTime < 0) return;
		loginLoading1.color = endColor-(endColor-startColor)*((Time.time - startTime) / time1);
		if (Time.time > startTime + time1-0.1f) 
		{
			if(Time.time > startTime+ time1 + time2+spauseTime){
				if(!flag)
				{
					//SDKHelper.saveState(StateStep.STEP5);
					flag=true;
				}
				loginLoading2.color = endColor-(endColor-startColor)*((Time.time - startTime-time1-time2-spauseTime-0.1f) / time3);
				if(Time.time > startTime+ time1 + time2+time3+spauseTime-0.03f){
					if (action != null)
					{
						action ();
						action=null;
					}
				}
			}else if(Time.time <= startTime+ time1 + time2){
				loginLoading2.color = (endColor-startColor)*((Time.time - startTime-time1) / time2) + startColor;
			}
		}
	}

	public void start()
	{
		startTime = Time.time;
		#if UNITY_EDITOR
		if(GameManager.Instance.isSkipFlash)
		{
			action ();
		}
		#endif
	}

	/** 设置回调 */
	public void setCallBack(Action action)
	{
		this.action = action;
		if ((startTime>0&&Time.time > startTime+time1 + time2) || !isActiveAndEnabled) 
		{
			Destroy (this);
			action ();
			this.action=null;
		}
	}
}
