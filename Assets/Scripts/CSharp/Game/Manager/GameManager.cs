using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using ABSystem;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : SingletonBehaviour<GameManager>{
	/** 游戏目标帧率*/
	public static int TARGET_FRAMERATE = 30;
	/** 默认加载资源 */
	public static string[] BASERESOURCES;
	/** Root节点 */
	const string COREROOT = "_CoreUISystem#";

	[Tooltip("默认日志级别")]
	public LogLevel logLevel;
	[Tooltip("资源版本")]
	public int streamingAssetVersion;
	[Tooltip("是否使用AB包")]
	public bool isUseAB = false;
	[Tooltip("是否快速跳过闪屏")]
	public bool isSkipFlash = false;
	[Tooltip("是否开启SDK")]
	public bool openSDK = false;
	[Tooltip("是否可以更新")]
	public bool update = false;


	public GameObject splash;


	/** 当前状态 */
	StateBase curState = null;
	List<StateBase> states = null;
	void Awake()
	{
		Log.level = logLevel;
		Application.targetFrameRate = TARGET_FRAMERATE;
	}

	void Start()
	{
		BASERESOURCES = Resources.Load<TextAsset>("resourcesConfig").text.Replace("\r","").Split(new string[]{"\n"},StringSplitOptions.RemoveEmptyEntries);
		int count = 0;
		Action loadFinish = ()=>{
			count++;
			if(count<2)return;
			LoadResourcesFinish();
		};
		ResourcesManager.Instance.cache(BASERESOURCES,loadFinish);
		ResourceHelper.Instance.InitManifest(loadFinish);
	}
	/** 加载默认资源结束 */
	void LoadResourcesFinish()
	{
		if(splash==null)splash = GameObject.Find("Camera");
		splash.GetComponent<SplashAlpha>().setCallBack(()=>{
			InitRoot();
			AFRManager.Instance.RegisterFunc();
			states = new List<StateBase>();
			this.ChangeState<InitState>();
			Destroy(splash);
		});
		splash.GetComponent<SplashAlpha> ().start ();
	}

	/** 初始化Root */
	void InitRoot()
	{
		GameObject _coreRoot = GameObject.Find (COREROOT);
		if(_coreRoot!=null)DestroyImmediate(_coreRoot);
		_coreRoot = Instantiate(ResourcesManager.Instance.getObjectFromCache(COREROOT).LoadMainAsset<GameObject>())as GameObject;
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

	/** 切换状态 */
	public void ChangeState<T>() where T: StateBase,new()
	{
		if(curState!=null)curState.Exit();
		curState = GetState<T>();
		curState.Enter();
	}
	/** 获取状态 */
	private T GetState<T>() where T : StateBase,new()
	{
		T t = null;
		foreach (var s in states) {
			t = s as T;
			if(t!=null){
				return t;
			}
		}
		t = new T();
		states.Add(t);
		return t;
	}
}
