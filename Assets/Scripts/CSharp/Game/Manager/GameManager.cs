using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using ABSystem;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : SingletonBehaviour<GameManager>
{
	/** 游戏目标帧率*/
	public static int TARGET_FRAMERATE = 30;
	/** 默认加载资源 */
	public static string[] BASERESOURCES;
	/** Root节点 */
	const string COREROOT = "_CoreUISystem#";

	[Tooltip ("默认日志级别")]
	public LogLevel logLevel;
	[Tooltip ("资源版本")]
	public int streamingAssetVersion;
	[Tooltip ("是否使用AB包")]
	public bool isUseAB = false;
	[Tooltip ("是否快速跳过闪屏")]
	public bool isSkipFlash = false;
	[Tooltip ("是否开启SDK")]
	public bool openSDK = false;
	[Tooltip ("是否可以更新")]
	public bool update = false;
	[Tooltip("是否托管")]
	public bool gm = false;
	/** 是否加载完成 */
	private bool sdkInited,resourceInited,initFailed;
	/** 闪屏 */
	public GameObject splash;

	void Awake ()
	{
		Log.level = logLevel;
		Application.targetFrameRate = TARGET_FRAMERATE;
		if (splash == null)
			splash = GameObject.Find ("Camera");
		SDKHelper.initSDK ((node)=>{
			if("success".Equals(node.ToString()))
			{
				initFailed=false;
				SDKHelper.saveState(StateStep.STEP2);
				if(resourceInited) splash.GetComponent<SplashAlpha>().start();
				else sdkInited=true;
			}
			else
			{
				initFailed=true;
				if(resourceInited) splash.GetComponent<SplashAlpha>().start();
				else sdkInited=true;
			}
		});
		SDKHelper.saveState(StateStep.STEP1);
	}

	void Start ()
	{
		CoroutineCenter.Instance.stop();
		BASERESOURCES = Resources.Load<TextAsset> ("resourcesConfig").text.Replace ("\r", "").Split (new string[]{ "\n" }, StringSplitOptions.RemoveEmptyEntries);
		int count = 0;
		Action loadFinish = () => {
			count++;
			if (count < 2)
				return;
			LoadResourcesFinish ();
		};
		ResourcesManager.Instance.cache (BASERESOURCES, loadFinish);
		ResourceHelper.Instance.InitManifest (loadFinish);
	}

	/** 加载默认资源结束 */
	void LoadResourcesFinish ()
	{
		SDKHelper.saveState(StateStep.STEP3);
		splash.GetComponent<SplashAlpha> ().setCallBack (() => {
			InitRoot ();
			AFRManager.Instance.RegisterFunc ();
			StateManager.Instance.LauncherState<InitState> ();
			if(initFailed){
				#if UNITY_ANDROID
				StateManager.Instance.DoLuaFunction("ShowAlert","TIP_11",()=>{
					Application.Quit();
				},true);
				#endif
				Destroy (splash);
			}else{
				StateManager.Instance.DoLuaFunction("CheckVersionUpdate");
				StartCoroutine(CoroutineCenter.delayRunFrameIE(()=>{Destroy(splash);},1));
			}
		});
		splash.GetComponent<SplashAlpha> ().start ();
	}

	/** 初始化Root */
	void InitRoot ()
	{
		GameObject _coreRoot = GameObject.Find (COREROOT);
		if (_coreRoot != null)
			DestroyImmediate (_coreRoot);
		_coreRoot = Instantiate (ResourcesManager.Instance.getObjectFromCache (COREROOT).LoadMainAsset<GameObject> ())as GameObject;
		_coreRoot.name = COREROOT;
		_coreRoot.transform.SetParent (null);
		DontDestroyOnLoad (this);
		DontDestroyOnLoad (_coreRoot);
		GameObject eventSystem = GameObject.Find ("EventSystem");
		if (eventSystem == null) {
			eventSystem = new GameObject ();
		}
		eventSystem.AddMissCompoent<EventSystem> ();
		DontDestroyOnLoad (eventSystem);
	}
}
