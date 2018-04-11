using System;
using System.Collections.Generic;
/// <summary>
/// 平台帮助工具
/// 汪松民
/// </summary>
public class SDKHelper
{
	/* static fields */
	/** 回调常量 */
	public const string INIT="init",CONNECT="connect",CERTIFY="certify",LOGIN="login",LOGOUT="logout",RELOGIN="switch",PAY="pay";
	/** 更新URL */
	public const string UPDATEURL = "http://192.168.1.15:1001/100/1";
	/** 是否更新 */
	public const string RESOURCEVERSION="-1";
	/**　默认平台ID */
	public const int PLANTID = 1;
	/** IP(域名) */
	public const string IPADRESS = "jiushi1234.6655.la";
	/** 端口(http,tcp) */
	public const int HTTPPORT = 843,TCPPORT = 1000;
	/** 服务器ID */
	public const int SERVERID = 1;
	
	/** 回调对象 */
	public static Action<IJsonNode> InitAction;
	public static Action<IJsonNode> LoginAction;
	public static Action<IJsonNode> LogoutAction;
	public static Action<IJsonNode> CertifyAction;
	public static Action<IJsonNode> ConnectAction;
	public static Action<IJsonNode> BuyAction;

	/* static methods */
	/** 初始化 */
	public static void initSDK(Action<IJsonNode> InitAction)
	{
		if (!GameManager.Instance.openSDK) 
		{
			InitAction(new JsonString("success"));
			return;
		}
		SDKHelper.InitAction = InitAction;
		#if UNITY_IPHONE
		IOSHelper.initSDK();
		#endif
		#if UNITY_ANDROID || UNITY_STANDALONE_WIN
		AndroidHelper.initSDK();
		#endif
	}
	/** 状态记录 */
	public static void saveState(StateStep state)
	{
		if (!GameManager.Instance.openSDK) return;
		onFuncCall(Handler.STATE,((int)state).ToString());
	}
	/** 状态记录 */
	public static void SaveState(int state)
	{
		if (!GameManager.Instance.openSDK) return;
		onFuncCall(Handler.STATE,(state).ToString());
	}

	/** 是否应许更新 */
	public static bool enableUpdate()
	{
		if (!GameManager.Instance.openSDK) return GameManager.Instance.update;
		return "true".Equals(platformHelper(Handler.UPDATE,null));
	}

	/** 获取更新路径 */
	public static string getUpdateDataUrl()
	{	
		if(!GameManager.Instance.openSDK) return UPDATEURL;
		return platformHelper(Handler.UPDATEURL,null);
	}
	/** 获取下载路径 */
	public static string getDownUrl()
	{
		if(!GameManager.Instance.openSDK) return string.Empty;
		return platformHelper (Handler.DOWNURL, null);
	}
	/** 获取信息 */
	public static string getInfo(int port)
	{
		if(!GameManager.Instance.openSDK)
		{
			if(port==Handler.RESOURCEVERSION)
				return RESOURCEVERSION;
			return string.Empty;
		}
		return platformHelper(port,null);
	}
	/** 连接SDK服务器 */
	public static void connectServer(Action<IJsonNode> connectBack)
	{
		ConnectAction = connectBack;
		onFuncCall(Handler.CONNECT,null);
	}
	/** 是否登录 */
	public static bool islogin()
	{
		if (!GameManager.Instance.openSDK) return false;
		string result=platformHelper(Handler.ISLOGIN,null);
		return "true".Equals(result.ToLower());
	}
	/** 登录 */
	public static void login(Action<IJsonNode> backAction)
	{
		SDKHelper.LoginAction = backAction;
		onFuncCall(Handler.LOGIN,null);
	}
	/** 获取物品 */
	public static string getGoods()
	{
		return platformHelper(Handler.GOODLIST,null);
	}
	/** 获取服务器列表 */
	public static Server[] getServers()
	{
		Server[] servers = null;
		if (!GameManager.Instance.openSDK) 
		{
			List<Server> list=new List<Server>();
			//list.Add(Server.newServer (8,"外网测试服", 1, "jiushi1234.6655.la", 1000, 843));
			list.Add(Server.newServer (1,"外网测试服", 1, 1, "120.24.152.209", 1100, 1101));
			list.Add(Server.newServer (2,"内网测试服", 1, 1, "192.168.1.15", 1000, 1001));
			list.Add(Server.newServer (3,"本地", 1, 1, "127.0.0.1", 1000, 843));
			list.Add(Server.newServer (4,"胖子", 1, 1, "192.168.1.19", 1000, 843));
			list.Add(Server.newServer (5,"黄海涛", 1, 1, "192.168.1.14", 1000, 843));
			list.Add(Server.newServer (6,"汪松民", 1, 1, "192.168.1.18", 1000, 1100));
			list.Add(Server.newServer (7,"test", 1, 1, "192.168.1.16", 8020, 8021));
			list.Add(Server.newServer (8,"大魔法时代", 1, 1, "120.92.27.168", 1100, 1101));
			servers=list.ToArray();
			return servers;
		}
		string serverStr = platformHelper(Handler.SERVERLIST,null);
		Log.debug("server:"+serverStr);
		IJsonNode node=serverStr.ToJsonNode();
		servers=new Server[node.Count];
		for(int i = 0; i < servers.Length; i ++)
		{
			servers[i] = new Server();
			servers[i].parse(node[servers.Length-1-i]);
		}
		return servers;
	}
	/** 获取最近登录服务器 */
	public static LoginServer[] getRecentLoginServer()
	{
		string serverStr = platformHelper(Handler.RECENT,null);
		if (!string.IsNullOrEmpty (serverStr)) 
		{
			IJsonNode node=serverStr.ToJsonNode();
			LoginServer[] servers=new LoginServer[node.Count];
			for(int i=0;i<servers.Length;i++)
			{
				servers[i]=new LoginServer();
				servers[i].parseJson(node[i]);
			}
			Array.Sort(servers,(s1,s2)=>{
				if(s1.getLoginTime() < s2.getLoginTime()) return 1;
				if(s1.getLoginTime() > s2.getLoginTime()) return -1;
				if(s1.getLoginTime() == s2.getLoginTime())
				{
					if(s1.getID() < s2.getID()) return 1;
					if(s1.getID() > s2.getID()) return -1;
				}
				return 0;
			});
			return servers;
		}
		return null;
	}
	/** 退出登录 */
	public static void logout()
	{
		if (!GameManager.Instance.openSDK) return;
		onFuncCall(Handler.LOGOUT,null);
	}
	/** 登录认证 */
	public static void certify(int serverSID,Action<IJsonNode> backAction)
	{
		SDKHelper.CertifyAction = backAction;
		JsonObject json=new JsonObject();
		json.Add("serverId",new JsonNumber(serverSID));
		onFuncCall(Handler.CERTIFY,json.ToString());
	}
	/** 获取第三方平台ID */
	public static string getSDKPlantID()
	{
		if (!GameManager.Instance.openSDK) return "0";
		return platformHelper (Handler.PLANTID, null);
	}
	/** 获取版本信息 */
	public static AppVersion getAppVersion()
	{
		if (!GameManager.Instance.openSDK) return new AppVersion();
		string json=platformHelper(Handler.APPINFO,null);
		AppVersion version=new AppVersion();
		version.jsonRead(json.ToJsonNode());
		return version;
	}
	/** 退出游戏 */
	public static void exit()
	{
		if (!GameManager.Instance.openSDK) return;
		JsonObject json = new JsonObject ();
		//TODO 
		onFuncCall(Handler.EXIT,json.ToString());
	}
	/** 购买 */
	public static void buy(Action<IJsonNode> BuyAction, string goodID)
	{
		SDKHelper.BuyAction = BuyAction;
		JsonObject json = new JsonObject ();
		json.Add ("goodId", goodID);
		//TODO
		onFuncCall(Handler.PAY,json.ToString());
	}
	/** 统计 */
	public static void statisticRoleInfo(int state)
	{
		if (!GameManager.Instance.openSDK) return;
		JsonObject json = new JsonObject ();
		//TODO
		onFuncCall(Handler.ROLEINFO,json.ToString());
	}
	/** 创建统计 */
	public static void createStatisticRoleInfo(string name,string pid)
	{
		if (!GameManager.Instance.openSDK) return;
		JsonObject json = new JsonObject ();
		//TODO
		onFuncCall(Handler.ROLEINFO,json.ToString());
	}

	public static string platformHelper(int handler,string json)
	{
		#if UNITY_ANDROID || UNITY_STANDALONE_WIN
		string result = AndroidHelper.platformHelper(handler,json);
		#endif
		#if UNITY_IPHONE
		string result = IOSHelper.platformHelper(handler,json);
		#endif
		Log.debug (handler+":"+result);
		return result;
	}
	public static void onFuncCall(int handler,string json)
	{
		#if UNITY_ANDROID || UNITY_STANDALONE_WIN
		AndroidHelper.onFuncCall(handler,json);
		#endif
		#if UNITY_IPHONE
		IOSHelper.onFuncCall(handler,json);
		#endif
	}

	/** SDK发过来的 */
	public static void OnGetMessage (string msg)
	{
		IJsonNode jsonNode = msg.ToJsonNode();
		string key = jsonNode["key"].ToString();
		IJsonNode value = jsonNode["value"];
		switch (key)
		{
		case SDKHelper.INIT:
			if(InitAction!=null)
			{
				InitAction(value);
				InitAction=null;
			}
			break;
		case SDKHelper.CONNECT:
			if(ConnectAction!=null)
			{
				ConnectAction(value);
				ConnectAction=null;
			}
			break;
		case SDKHelper.CERTIFY:
			if(CertifyAction!=null)
			{
				CertifyAction(value);
				CertifyAction=null;
			}
			break;
		case SDKHelper.LOGIN:
			if(LoginAction!=null)
			{
				LoginAction(value);
				LoginAction=null;
			}
			break;
		case SDKHelper.LOGOUT:
			
			break;
		case SDKHelper.PAY:
			if(BuyAction!=null)
			{
				BuyAction(value);
				BuyAction=null;
			}
			break;
		}
	}
	/** 更新 */
	public static void update()
	{
		if(!GameManager.Instance.openSDK) return;
		#if UNITY_ANDROID || UNITY_STANDALONE_WIN
		AndroidHelper.update();
		#endif
	}
}

