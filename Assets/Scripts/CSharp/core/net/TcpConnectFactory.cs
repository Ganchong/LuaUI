using System;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

public class TcpConnectFactory : MonoBehaviour
{
	/* static fields */
	/** ping时间间隔 */
	public const int PING_TIME = 20 * 1000;

	/* fields */
	/** 连接数组 */
	List<Connect> connects=new List<Connect>();
	/** 处理器 */
	TransmitHandler handler;
	/** ping时间间隔 */
	int pingTime = PING_TIME;
	/** 最后一次ping的时间 */
	long lastPingTime = 0;
	/** 状态改变回调 */
	Action<Connect,int> callBack;
	/** ping数据 */
	ByteBuffer pingData=new ByteBuffer();

	/* properties */
	/** 连接数量 */
	public int Count
	{
		get
		{
			return connects.Count;
		}
	}
	/** 说有连接 */
	public Connect[] Connects
	{
		get
		{
			return connects.ToArray();
		}
	}
	/** 消息处理器 */
	public TransmitHandler Handler
	{
		get
		{
			return handler;
		}
		set
		{
			handler = value;
		}
	}
	/** ping时间 */
	public int PingTime
	{
		get
		{
			return pingTime;
		}
		set
		{
			pingTime=value;
		}
	}
	/** 回调 */
	public Action<Connect,int> CallBack
	{
		get
		{
			return callBack;
		}
		set
		{
			callBack = value;
		}
	}

	/* constructors */
	/** 构造方法 */


	/* methods */
	/** 移出连接 */
	public bool removeConnect(Connect connect)
	{
		return connects.Remove(connect);
	}
	/** 创建Tcp连接 */
	public void createTcpConnect (URL url)
	{
		TcpConnect c = new TcpConnect ();
		init (c);
		c.open (url);
	}
	/** 初始化连接 */
	protected void init (TcpConnect c)
	{
		connects.Add (c);
		c.Handler = handler;
		c.CallBack = callBack;
	}
	/** 运行 */
	private void run ()
	{
		receive ();
		long time = TimeKit.getMillisTime ();
		if (pingTime > 0 && time - lastPingTime > pingTime) {
			ping (time);
			lastPingTime = time;
		}
	}
	/** 接收 */
	private void receive ()
	{
		try {
			TcpConnect c;
			List<Connect> connects=this.connects;
			for (int i=connects.Count-1; i>=0; i--) 
			{
				c = connects [i] as TcpConnect;
				c.receive ();
			}
		} catch (Exception ex) {
			Debug.LogError (ex);
		}
	}
	/** ping */
	private void ping (long time)
	{
		try {
			int code = (int)time;
			Connect c;
			ByteBuffer data=this.pingData;
			data.clear(1);
			data.writeShort(PortService.ECHO_PORT);
			data.writeShort(PortService.PING_PORT);
			data.writeInt(code);
			List<Connect> connects=this.connects;
			for (int i=connects.Count-1; i>=0; i--) 
			{
				c = connects [i];
				if (!c.Active) continue;
				c.PingCode = code;
				c.PingTime = time;
				c.send (data);
			}
		} catch (Exception ex) {
			Debug.LogError (ex);
		}
	}
	/** 更新方法 */
	void FixedUpdate ()
	{
		run ();
	}
	/** 关闭所有连接 */
	public void close (bool dispose)
	{
		List<Connect> connects=this.connects;
		for (int i=connects.Count-1; i>=0; i--)
			connects [i].Close (dispose?Connect.CLOSEBYUSER:Connect.CLOSEBYAUTO);
		connects.Clear ();
		Log.debug (this.GetType () + " close, size=" + connects.Count + " " + this);
	}

	/* common methods */
	/** 字符串 */
	public override string ToString ()
	{
		return base.ToString () + "[size=" + connects.Count + ", pingTime=" + pingTime + "]";
	}
}