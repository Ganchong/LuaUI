using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 基类
/// </summary>
public class BaseBehaviour:MonoBehaviour,IEventRegister,IEventSender{
	protected virtual void Awake(){}
	protected virtual void OnDestroy(){
		this.UnRegisterAll ();
	}
	protected virtual void Start(){}
	protected virtual void OnEnable(){}
	protected virtual void OnDisable(){}
	protected virtual void Update(){}
	protected virtual void FixedUpdate(){}
	protected virtual void LateUpdate(){}

	public Dictionary<int,List<Delegate>> eventMap 
	{
		get;
		set;
	}

}
