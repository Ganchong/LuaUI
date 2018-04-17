using System;
using System.Collections.Generic;

public class ServerManager : Singleton<ServerManager>
{
	/* fields */
	/** 服务器列表 */
	Server[] servers;
	/** 最近登录服务器 */
	ArrayListYK<LoginServer> loginServers=new ArrayListYK<LoginServer>();

	/* methods */
	/** 获取最近登录服务器 */
	public LoginServer getLastLoginServer()
	{
		if (loginServers.size () < 1) 
		{
			if(!GameManager.Instance.openSDK)
			{
				int serverID=UnityEngine.PlayerPrefs.GetInt(PlayerSetting.SERVERID,-1);
				if(serverID!=-1) return new LoginServer (serverID);
				return new LoginServer (getServers () [0].ID);
			}
			LoginServer[] recentServers=SDKHelper.getRecentLoginServer();
			int length=recentServers==null?0:recentServers.Length;
			for(int i=0;i<length;i++)
			{
				loginServers.add(recentServers[i]);
			}
			if(loginServers.size()>0) return loginServers[0];
			Server[] servers=getServers();
			if(servers!=null&&servers.Length>0)	return new LoginServer (servers [0].ID);
			return null;
		}
		return loginServers [0];
	}
	/** 获取最后登录的服务器信息 */
	public Server getLastServer()
	{
		LoginServer server = getLastLoginServer();
		if(server==null)return null;
		return server.getServer();
	}
	/** 添加最近登录服务器 */
	public void addLoginServer(Server server,Player player)
	{
		LoginServer loginServer = null;
		for (int i=0; i<loginServers.size(); i++) 
		{
			if(loginServers.get(i).getID()==server.ID)
			{
				loginServer=loginServers.get(i);
				break;
			}
		}
		if (loginServer == null) {
			loginServer = new LoginServer (server.ID);
			loginServer.setRoleName (player.Name);
			loginServer.setRoleLevel (player.Level);
			loginServers.add (loginServer, 0);
		} else {
			loginServers.remove(loginServer);
			loginServers.add(loginServer,0);
		}
	}
	/** 获取所有服务器 */
	public Server[] getServers()
	{
		if (servers == null) 
		{
			servers = SDKHelper.getServers ();
			SetKit.sort (servers, new ServerComparator ());
		}
		return servers;
	}
	/** 获取所有最近登录服务器 */
	public ArrayListYK<LoginServer> getLoginServers()
	{
		return loginServers;
	}
	/** 新服是否在登录服中 */
	public bool checkNewServer()
	{
		LoginServer[] array = loginServers.toArray ();
		for(int i=0;i<array.Length;i++)
		{
			if(array[i].getServer()==servers[0])
				return true;
		}
		return false;
	}

	/** 登录后记录服务器 */
	public void loginServerRecord(Server server)
	{
		servers = null;
	}
	/** 获取服务器 */
	public Server getServer(int id)
	{
		Server[] servers = getServers ();
		for (int i=0; i<servers.Length; i++) 
		{
			if(servers[i].ID==id)
				return servers[i];
		}
		return servers[0];
	}
}

