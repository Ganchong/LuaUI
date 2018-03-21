using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaFrameWorkCore;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : MonoSingleton<GameManager>{
	/** 游戏目标帧率*/
	public static int TARGET_FRAMERATE = 30;

	[Tooltip("是否使用AB包")]
	public bool isUseAB = false;

	void Awake()
	{
		Application.targetFrameRate = TARGET_FRAMERATE;
	}

	void Start()
	{
		GameObject _coreRoot = GameObject.Find ("_CoreRoot#");
		if (_coreRoot == null) {
			_coreRoot = Resources.Load ("_CoreRoot#") as GameObject;
			_coreRoot = GameObject.Instantiate (_coreRoot);
			_coreRoot.name = "_CoreRoot#";
			_coreRoot.transform.SetParent (null);
		}
		DontDestroyOnLoad (this);
		DontDestroyOnLoad (_coreRoot);
	}
}
