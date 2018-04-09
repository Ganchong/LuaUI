using System;
using System.Collections;
using UnityEngine;

public class Connect
{
	/* static fields */
	/** 空字节数组 */
	public static readonly byte[] NULL_BYTE = {};
	/** 超时时间 */
	public const int TIMEOUT = 180000;
	/** 状态 */
	public const int OPEN_CHANGED = 1, OPEN_FAIL_CHANGED = 2, CLOSE_CHANGED = 3, USERCLOSE_CHANGED = 4;
	/** 关闭类型 */
	public const int CLOSEBYAUTO = 1,CLOSEBYSEND = 2,CLOSEBYUSER = 3;

	/* fields */
	/** 连接 */
	protected URL url;
	/** 本地地址 */
	string localAddress;
	/** 是否活动 */
	volatile bool active;
	/** 开始时间 */
	long startTime;
	/** 活动时间 */
	long activeTime;
	/** ping值 */
	int ping = -1;
	/** 超时时间 */
	int timeout = TIMEOUT;
	/** 源 */
	object source;
	/** 消息处理器 */
	TransmitHandler handler;
	/** 监听回调 */
	Action<Connect,int> callBack;
	/** ping码 */
	int pingCode;
	/** ping时间 */
	long pingTime;

	/* properties */
	/** 远程连接 */
	public URL Url
	{
		get
		{
			return url;
		}
	}
	/** 本地地址 */
	public string LoaclAddress
	{
		get
		{
			return localAddress;
		}
	}
	/** 是否活动 */
	public bool Active
	{
		get
		{
			return active;
		}
	}
	/** 开始时间 */
	public long StartTime
	{
		get
		{
			return startTime;
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
	/** 获取ping值 */
	public int Ping
	{
		get
		{
			return ping;
		}
		set
		{
			ping = value;
		}
	}
	/** 超时时间 */
	public int Timeout
	{
		get
		{
			return timeout;
		}
		set
		{
			timeout = value;
		}
	}
	/** 绑定源 */
	public object Source
	{
		get
		{
			return source;
		}
		set
		{
			source = value;
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
	/** 状态回调 */
	public Action<Connect,int> CallBack
	{
		get{
			return callBack;
		}
		set
		{
			callBack=value;
		}
	}
	/** 获取ping码 */
	public int PingCode
	{
		get
		{
			return pingCode;
		}
		set
		{
			pingCode = value;
		}
	}
	/** ping时间 */
	public long PingTime
	{
		get
		{
			return pingTime;
		}
		set
		{
			pingTime = value;
		}
	}

	/* methods */
	/** 打开完成 */
	protected void opened ()
	{
		active = true;
		activeTime = startTime = DateTime.Now.ToFileTime ();
		Log.debug (this.GetType () + ", opened, " + this);

		if (callBack != null)
		{
			callBack (this, OPEN_CHANGED);
		}
	}
	/** 设置本地连接地址 */
	protected void setLocalAddress (string address)
	{
		localAddress = address;
		url.localAddress = address;
	}
	/** 发送数据方法 */
	public virtual void send (ByteBuffer data)
	{
	}
	/** 接收数据方法 */
	public virtual void receive ()
	{
	}
	/** 处理数据方法 */
	public void receive (ByteBuffer data)
	{
		if (handler == null)
			return;
		activeTime = TimeKit.getMillisTime ();
		try {
			handler.transmit (this, data);
		} catch (Exception e) {
			Debug.LogError (this.GetType () + ", receive error, " + this+ e.ToString());
		}
	}
	/** 关闭连接方法 */
	public virtual void Close (int closeType)
	{
		lock (this) {
			if (!active)
				return;
			active = false;
		}
		if(Log.isInfoEnable()) Log.info (this.GetType () + ", close, " + this);
		if (callBack != null)
			callBack (this, closeType==CLOSEBYSEND ?CLOSE_CHANGED : USERCLOSE_CHANGED);
	}

	/* common methods */
	/** 字符串 */
	public override string ToString ()
	{
		return base.ToString () + "[" + url + ", localAddress=" + localAddress
			+ ", active=" + active + ", startTime="
			+ startTime + ", activeTime=" + activeTime + ", ping=" + ping
			+ ", timeout=" + timeout + "]";
	}
}