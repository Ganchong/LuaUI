﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LuaFrameworkCore;
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
	const string COREROOT = "_CoreRoot#";


	[Tooltip("是否使用AB包")]
	public bool isUseAB = false;

	/** 当前状态 */
	StateBase curState = null;
	List<StateBase> states = null;
	void Awake()
	{
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
		InitRoot();
		AFRManager.Instance.RegisterFunc();
		states = new List<StateBase>();
		this.ChangeState<InitState>();
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
