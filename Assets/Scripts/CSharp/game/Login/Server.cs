using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 服务器
/// 汪松民
/// </summary>
public class Server
{
	/* fields */
	/** id */
	int id;
	/** 名称 */
	string name;
	/** sid */
	int sid;
	/** 平台ID */
	int plantId=1;
	/** ip(域名) */
	string ipAddress;
	/** 端口 */
	int tcpPort;
	/** http端口 */
	int httpPort;
	/** 其他参数 */
	Dictionary<string,string> map;


	/* static methods */
	/** 构建一个服务器 */
	public static Server newServer(int id,string name,int sid,int plantId,string ipAddress,int tcpPort,int httpPort)
	{
		Server server = new Server();
		server.id = id;
		server.name = name;
		server.sid = sid;
		server.plantId = plantId;
		server.ipAddress = ipAddress;
		server.tcpPort = tcpPort;
		server.httpPort = httpPort;
		return server;
	}

	/* methods */
	public int Sid {
		get {
			return this.sid;
		}
		set {
			this.sid=value;
		}
	}
	public int PlantId {
		get {
			return this.plantId;
		}
		set {
			this.plantId=value;
		}
	}

	public string IpAddress {
		get {
			return this.ipAddress;
		}
		set {
			this.ipAddress=value;
		}
	}

	public int TcpPort {
		get {
			return this.tcpPort;
		}
		set {
			this.tcpPort=value;
		}
	}

	public int HttpPort {
		get {
			return this.httpPort;
		}
		set {
			this.httpPort=value;
		}
	}

	public string Name {
		get {
			return this.name;
		}
	}
	public int ID
	{
		get{
			return this.id;
		}
		set{
			this.id=value;
		}
	}
	/** 解析方法 */
	public void parse(IJsonNode node)
	{
		id = node["id"].ToInt();
		name = node["serverName"].ToString();
		sid = node["serverId"].ToInt ();
		plantId = node["plantId"].ToInt ();
		ipAddress = node["ipAddress"].ToString ();
		tcpPort = node["tcpPort"].ToInt ();
#if UNITY_ANDROID
		httpPort = node["httpPort"].ToInt ();
#else 
		httpPort = node["httpsPort"].ToInt ();
#endif
	}

	/** 获取键值 */
	public string getValue(string key)
	{
		if(map.ContainsKey(key))
			return map[key];
		return string.Empty;
	}

	/* common methods */
	public override string ToString ()
	{
		return string.Format ("[Server: name={0}, sid={1}, ipAddress={2}, tcpPort={3}, httpPort={4}]", name, sid, ipAddress, tcpPort, httpPort);
	}
}
