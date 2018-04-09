using System;
using UnityEngine;

/// <summary>
/// URL
/// </summary>
public class URL
{
	/* static fields */
	/** 协议分隔符 */
	public const string HOST_SEPARATOR = "://";
	/** 端口分隔符 */
	public const char PORT_SEPARATOR = ':';
	/** 文件分隔符 */
	public const char FILE_SEPARATOR = '/';

	/* static methods */
	/** 地址IPv4 */
	public static string IntToIPv4 (int addr)
	{
		byte[] bytes = BitConverter.GetBytes (addr);
		Array.Reverse (bytes);
		return ((bytes [0]) & 0xff) + "." + ((bytes [2]) & 0xff) + "."
			+ ((bytes [3]) & 0xff) + "." + (bytes [4] & 0xff);
	}

	/** 地址转int */
	public static int IPv4ToInt (string addr)
	{
		int offset = 0;
		int i = addr.IndexOf ('.');
		if (i < 0)
			return 0;
		try {
			int t0 = Convert.ToInt32 (addr.Substring (offset, i));
			offset = i + 1;
			i = addr.IndexOf ('.', offset);
			if (i < 0)
				return 0;
			int t1 = Convert.ToInt32 (addr.Substring (offset, i));
			offset = i + 1;
			i = addr.IndexOf ('.', offset);
			if (i < 0)
				return 0;
			int t2 = Convert.ToInt32 (addr.Substring (offset, i));
			int t3 = Convert.ToInt32 (addr.Substring (i + 1));
			return ((t0 << 24) + (t1 << 16) + (t2 << 8) + t3);
		} catch (Exception e) {
			Debug.LogError (e);
		}
		return 0;
	}

	/* fields */
	/** 远程IP(域名) */
	string ipAddress;
	/** 端口 */
	int port;
	/** 地址 */
	string filePath;
	/** 协议 */
	string protocol;
	/** 本地地址连接 */
	public string localAddress;

	/* constructors */
	/** 构造方法 */
	public URL()
	{
	}
	/** 构造方法(IP,端口) */
	public URL (string remoteIP, int remotePort)
	{
		if (remoteIP == null)
			throw new Exception (this
				+ " <init>, null remoteIP");
		this.ipAddress = remoteIP.ToLower ();
		this.port = (remotePort >= 0) ? remotePort : 0;
	}
	/** 构造方法（根据地址） */
	public URL (URL url)
	{
		this.ipAddress = url.ipAddress;
		this.port = url.port;
		this.filePath = url.filePath;
	}
	/** 构造方法 */
	public URL (string url)
	{
		try
		{
			int index = url.IndexOf (HOST_SEPARATOR);
			protocol = url.Substring (0, index);
			int index1 = url.IndexOf (FILE_SEPARATOR, index+3);
			if(index1>0) filePath = url.Substring (index1 + 1);
			url = url.Substring (index+3, index1);
			index = url.LastIndexOf (PORT_SEPARATOR);
			ipAddress = url.Substring (0, index);
			port = int.Parse (url.Substring (index + 1));
		}
		catch(Exception ex)
		{
			throw new Exception(ex.ToString());
		}
	}

	/* properties */
	/** IP域名 */
	public string IPAddress
	{
		get
		{
			return ipAddress;
		}
	}
	/** 文件 */
	public string FilePath
	{
		get{
			return filePath;
		}
		set{
			filePath=value;
		}
	}
	/** 端口 */
	public int Port
	{
		get{
			return port;
		}
	}
	/** 协议 */
	public string Protocol
	{
		get
		{
			return protocol;
		}
	}

	/* common methods */
	/** equal */
	public override bool Equals (object obj)
	{
		if (this == obj)
			return true;
		if (!(obj is URL))
			return false;
		URL url = (URL)obj;
		if (!ipAddress.Equals (url.ipAddress))
			return false;
		if (!filePath.Equals (url.filePath))
			return false;
		if (port != url.port)
			return false;
		return true;
	}
	/** hashCode */
	public override int GetHashCode ()
	{
		return (ipAddress + port + filePath).GetHashCode ();
	}
	/** 字符串 */
	public override string ToString ()
	{
		return protocol+HOST_SEPARATOR+ipAddress+PORT_SEPARATOR+port+(string.IsNullOrEmpty(filePath)?string.Empty:FILE_SEPARATOR+filePath);
	}
}