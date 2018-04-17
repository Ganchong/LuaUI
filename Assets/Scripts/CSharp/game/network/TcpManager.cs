using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/// <summary>
/// 连接管理器
/// 汪松民
/// </summary>
public class TcpManager : Singleton<TcpManager>,IDisposable
{
	/* static fields */
	/** 单例 */
	private static TcpManager _instance;
	/** 连接 */
	private static TcpConnect _connect;
	/** 会话活动时间 */
	private static long activeTime;

	/** 数据服务 */
	PortService service;
	/** 连接工厂 */
	TcpConnectFactory factory;
	/** 是否连接中 */
	bool connecting = false;
	/** 连接回调 */
	Action<object,int> callBack;

	/* static methods */
	/** 当前连接 */
	public static TcpConnect connect 
	{
		get 
		{
			return _connect;
		}
	}

	/* properties */
	/** 连接中 */
	public bool Connecting 
	{
		get 
		{
			return this.connecting || (_connect!=null&&_connect.Active);
		}
	}
	/** 活动时间 */
	public long ActiveTime
	{
		get
		{
			return activeTime;
		}
	}

	/* mehtods */
	/** 初始化连接工厂 */
	public void init (GameObject gameObject)
	{
		factory = gameObject.AddComponent (typeof(TcpConnectFactory)) as TcpConnectFactory;
	}
	/** 初始化服务 */
	public void initCommunication ()
	{
		Log.debug ("initCommunication start");
		if (factory == null) {
			throw new ArgumentNullException (this.GetType () + ",factory must init");
		}
		factory.CallBack = connectCallBack;
		if (service != null) return;
		service = new PortService ();
//		service.setPort (PortService.ECHO_PORT, new EchoPort ());
//		service.setPort (PortService.PING_PORT, new PingPort ());
//		service.setPort (PortService.ADVISE_OFFLINE_PORT, new AdviseOfflinePort ());
//		service.setPort (PortService.ACCESS_RETURN_PORT, DataAccessHandler.instance);
//		service.setPort (PortService.UPDATE_ACTIVE, new TokenKeepPort ());
//		service.setPort (PortService.MOVETIPS, new MoveTipPort ());
//		service.setPort (PortService.PLAYERPROT, new PlayerPort ());
//		service.setPort (PortService.MESSAGEPROT, new MessagePort());
//		service.setPort (PortService.UNIONPORT, new UnionServicePort ());
//		service.setPort (PortService.ONLINEPORT, new OnlinePlayerPort ());
//		service.setPort (PortService.PVPPORT, new PvpServicePort ());
//		service.setPort (PortService.RADIOPORT, new RadioServicePort());
//		service.setPort (PortService.KICKPORT, new KickPort());
//		service.setPort (PortService.OPENFUNC, new OpenFunctionPort ());
//		service.setPort (PortService.UNIONFIGHTPORT, new UnionFightServicePort());
//		service.setPort (PortService.DUELPORT, new DuelServicePort());
		factory.Handler = service;
	}
	/** 初始化连接 */
	public void initConnet (string url, int port,Action<object,int> callBack)
	{
		this.callBack = callBack;
		if(connecting) return;
		Log.debug ("initconnet start");
		connecting = true;
		if (_connect != null && _connect.Active) _connect.Close (Connect.CLOSEBYAUTO);
		factory.createTcpConnect (new URL (url, port));
	}
	/** 事件改变方法 */
	public void connectCallBack (Connect source, int type)
	{
		if (type == Connect.OPEN_CHANGED) {
			_connect = source as TcpConnect;
			connecting = false;
		} else if (type == Connect.OPEN_FAIL_CHANGED) {
			if(_connect !=null) activeTime=_connect.ActiveTime;
			_connect = null;
			connecting = false;
		} else if (type == Connect.CLOSE_CHANGED) {
			if(_connect !=null) activeTime=_connect.ActiveTime;
			_connect = null;
		}
		if(source is TcpConnect) callBack(source,type);
	}
	/** 销毁 */
	public void Dispose ()
	{
		connecting = false;
		if(_connect !=null) activeTime=_connect.ActiveTime;
		_connect = null;
		DataAccessHandler.instance.close ();
		if (factory != null) 
		{
			factory.CallBack = null;
			factory.close (true);
		}
	}
	/** 断开连接 */
	public void closeConnect()
	{
		connecting = false;
		if(_connect !=null) activeTime=_connect.ActiveTime;
		_connect = null;
		if (factory != null) 
		{
			factory.CallBack = null;
			factory.close (false);
		}
		DataAccessHandler.instance.removeAccessForReLogin();
	}
}
