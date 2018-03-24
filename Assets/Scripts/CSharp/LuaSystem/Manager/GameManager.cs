using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaFrameworkCore;
using UnityEngine.EventSystems;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : SingletonBehaviour<GameManager>{
	/** 游戏目标帧率*/
	public static int TARGET_FRAMERATE = 30;

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
		GameObject _coreRoot = GameObject.Find ("_CoreRoot#");
		if (_coreRoot == null) {
			_coreRoot = Resources.Load ("_CoreRoot#") as GameObject;
			_coreRoot = GameObject.Instantiate (_coreRoot);
			_coreRoot.name = "_CoreRoot#";
			_coreRoot.transform.SetParent (null);
		}
		DontDestroyOnLoad (this);
		DontDestroyOnLoad (_coreRoot);
		GameObject eventSystem = GameObject.Find ("EventSystem");
		if (eventSystem == null) {
			eventSystem = new GameObject ();
		}
		eventSystem.AddMissCompoent<EventSystem> ();
		DontDestroyOnLoad (eventSystem);

		AFRManager.Instance.RegisterFunc();
		states = new List<StateBase>();
		this.ChangeState<InitState>();
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
