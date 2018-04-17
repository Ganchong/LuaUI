using System;
using UnityEngine;
using System.IO;
/// <summary>
/// 登录管理器
/// 汪松民
/// </summary>
public class LoginManager : Singleton<LoginManager>
{	
	/* static fields */
	/** 重连次数 */
	public static int CONNECTNUM=2;
	public static int REPORTSID =16;

	public static int FIRSTCHAPTERSID = 1;
	public static int FIRSTSTORYMISSIONSID = 48;
	/* fields */
	/** 选择的服务器 */
	Server server;
	/** uid */
	string uid;
	/** 认证ID */
	string certifyID;
	/** 玩家ID */
	string pid;
	/** 是否重链中 */
	bool relogining = false;
	/** 是否是重新登录 */
	bool isRelogin=false;
	/** 是否为重新加载 */
	bool isReLoad = false;
	/** 重试次数 */
	int reconnectNum;
	/** 是否被挤下线 */
	public bool isAdvise;
	bool isCreate = false;
	/* methods */
	/** 登录游戏 */
	public void login (string uid,string certifyID,Server server)
	{
		relogining = false;
		reconnectNum = 0;
		isAdvise = false;
		this.uid = uid;
		this.server = server;
		this.certifyID = certifyID;
		HttpPathHelper.HttpHost = TextKit.parse (HttpPathHelper.ServerUrl, server.IpAddress, server.HttpPort);
		getRolePid();
	}
	/** 连接回调 */
	private void connectBack (object source, int type)
	{
		Log.debug ("socket connectBack code = " + type);
		if (type == Connect.OPEN_CHANGED) {
			isRelogin=false;
			CoroutineCenter.delayRun(0.01f,()=>{
				SDKHelper.saveState (StateStep.STEP36);
                login (certifyID, pid);
			});
		} else if (type == Connect.OPEN_FAIL_CHANGED) {
			CoroutineCenter.delayRun(0.01f,()=>{
				SDKHelper.saveState (StateStep.STEP37);
				StateManager.Instance.DoLuaFunction("ShowAlert","loginWindow_3",()=>{
					TcpManager.Instance.initConnet (server.IpAddress, server.TcpPort, connectBack);
				});
				Log.debug("connect failed!!!!!!");
			});
		} else if (type == Connect.CLOSE_CHANGED) {
//			if(GameManager.Instance.ScenceID == LoadingWindow.LOGIN)
//			{
//				StateManager.Instance.DoLuaFunction("ShowAlert","loginWindow_3",()=>{
//					TcpManager.Instance.initConnet (server.IpAddress, server.TcpPort, connectBack);
//				});
//			}
//			else reLogin();
		}
	}
	/** 重新登录 */
	public bool reLogin ()
	{
//		if(GameManager.Instance.ScenceID == LoadingWindow.LOGIN||GameManager.Instance.ScenceID == LoadingWindow.LOADING) return false;
//		if (isAdvise || relogining) 
//		{
//			if (isAdvise){ MaskWindow.unLockNet (); MaskWindow.unLockUI();}
//			return false;
//		}
//		if(Log.isInfoEnable()) Log.info("relogining");
//		relogining = true;
//		if (TimeKit.getSecondTime () < TokenKeepPort.getActiveTime ()) {
//			closeConnect();
//			reconnect();
//		} else {
//			Alert.Show ((ans) => {
//				GameManager.Instance.logOut();
//			}, Language.get ("loginWindow_3"), false);
//			MaskWindow.unLockNet ();
//		}
		return true;
	}
	/** 重新连接 */
	public void reconnect()
	{
		if (!TcpManager.Instance.Connecting) {
			TcpManager.Instance.initCommunication ();
			TcpManager.Instance.initConnet (server.IpAddress, server.TcpPort, reconnectBack);
		} else
			relogining = false;
	}
	/** 重新连接回调 */
	private void reconnectBack(object source, int type)
	{
		Log.debug ("socket reconnectBack code = " + type);
		if (type == Connect.OPEN_CHANGED) {
			reconnectNum=0;
			isRelogin=true;
			CoroutineCenter.delayRun(0.01f,()=>{
				login (certifyID, pid);
			});
		} else if (type == Connect.OPEN_FAIL_CHANGED) {
			CoroutineCenter.delayRun(0.01f,()=>{
//				GameManager.Instance.SendEvent((int)NetExceptionEvent.ConnectException);
//				Alert.Show ((ans) => {
//					reconnectNum++;
//					if(reconnectNum>CONNECTNUM) GameManager.Instance.logOut();
//					else {relogining=false; MaskWindow.lockNet(); reLogin();}
//				}, Language.get ("loginWindow_3"), false);
				Log.debug("connect failed!!!!!!");
				MaskWindow.unLockNet ();
			});
		} else if (type == Connect.CLOSE_CHANGED) {
			relogining=false;
			reLogin();
		} else if (type == Connect.USERCLOSE_CHANGED) {
			relogining=false;
		}
	}
	/** 获取PID */
	private void getRolePid ()
	{
		SDKHelper.saveState(StateStep.STEP30);
		GetRoleIDHttpPort.Instance.access ((_pid) => {
			if (!string.IsNullOrEmpty (_pid)) {
				this.pid = _pid;
				TcpManager.Instance.initCommunication ();
				TcpManager.Instance.initConnet (server.IpAddress, server.TcpPort, connectBack);
			} else {
				SDKHelper.saveState(StateStep.STEP31);
//				LoginWindow window = UiManager.Instance.getWindow<LoginWindow>();
//				if(window!=null){
//					if(GameManager.Instance.OpenFirstStoryGuide){
//						window.storyPlay(playReport);
//					}else{
//						window.storyPlay(()=>{
//							LoadingWindow.loadScence(LoadingWindow.CREATE_ROLE,(Action)null);
//						});
//					}
//				}
			}
		},certifyID, uid, server.PlantId, server.Sid);
	}

	/** 登录服务器 */
	private void login (string certifyId, string pid)
	{
		long activeTime=TcpManager.Instance.ActiveTime;
		bool relogin=isRelogin&&Player.Instance!=null;
		if (TimeKit.getMillisTime () - activeTime > 55000) 
		{
			relogin = false;
			DataAccessHandler.instance.close();
		}
		SDKHelper.saveState (StateStep.STEP38);
//		LoginPort.Instance.access(()=>{
//			loadPlayer(!relogin);
//		},certifyId,pid,relogin);
	}
	/** 加载玩家信息 */
	private void loadPlayer (bool needLoad)
	{
	}
	/** 加载完成处理 */
	private void loadedHandler ()
	{
		Player.Instance.Pid = TextKit.parseLong(pid);
		ServerManager.Instance.addLoginServer (server, Player.Instance);
		SDKHelper.saveState (StateStep.STEP40);
//		SecondLoadAccessPort.Instance.loadMust(loadMustFinish);
	}
	/** 加载完成 */
	private void loadMustFinish ()
	{
	}

	/** 创建角色 */
	public void createRole (string name,string headStyle)
	{
		
	}

	public Server Server {
		get {
			return server;
		}
		set {
			server = value;
		}
	}
	public bool IsCreate{
		get{
			return isCreate;
		}
		set{
			this.isCreate = value;
		}
	}
	/** 是否重连中 */
	public bool IsReLogining()
	{
		return relogining;
	}
	/** 设置是否重连中 */
	public void setIsReLogining(bool relogining)
	{
		this.relogining=relogining;
	}
	/** 是否重新加载 */
	public bool IsReLoaded()
	{
		return isReLoad;
	}

	/** 获取认证ID */
	public string getCertifyID()
	{
		return certifyID;
	}
	/** 获取UID */
	public string getUid()
	{
		return uid;
	}
	/** 销毁 */
	public void destroy()
	{
		Player.Instance = null;
		DestroyInstance ();
	}
	public void closeConnect()
	{

		TcpManager.Instance.closeConnect ();
	}
}

