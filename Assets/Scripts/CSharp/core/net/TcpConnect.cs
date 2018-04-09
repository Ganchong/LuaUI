using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
/// <summary>
/// Tcp连接(单个数据包超过缓冲区大小时未处理)
/// </summary>
public class TcpConnect : Connect
{
	/* static fields */
	/** 最大包长度 */
	public const int MAX_DATA_LENGTH = 400 * 1024;
	/** 数据包发送超时时间(2s) */
	public const int SENDTIMEOUT=2*1000;

	/* fields */
	/** 连接 */
	internal Socket socket;
	/** 超时时间(3s) */
	int timeOut=3;
	/** 超时定时器 */
	Timer timer=null;
	/** 头信息 */
	byte[] head=new byte[4];
	/** 头下标 */
	int headIndex = 0;
	/** 消息 */
	ByteBuffer data;

	/* methods */
	/** 打开连接 */
	public void open (URL url)
	{
		this.url = url;
		if (Active)
			throw new Exception (this.GetType () + ", open, connect is active");
		if (url == null)
			throw new Exception (this.GetType () + ", open, null url");
		try {
			IPAddress[] ips = Dns.GetHostAddresses (url.IPAddress);
			IPEndPoint ipEndPoint = new IPEndPoint (ips [0], url.Port); 
			socket = new Socket (ips[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);

			timer=TimerManager.Instance.getTimer (1000L * timeOut);
			timer.addOnTimer (onTimeOut);
			timer.start ();

			IAsyncResult result = socket.BeginConnect (ipEndPoint, new AsyncCallback (connectCallback), socket);
			if(Log.isInfoEnable()) Log.info ("socket connect result = " + result.ToString ());
		} catch (System.Exception ex) {
			Debug.LogError (ex);
			if(socket!=null) socket.Close();
			CallBack (this, Connect.OPEN_FAIL_CHANGED);
		}
	}
	/** 超时 */
	public void onTimeOut()
	{
		if(socket!=null) socket.Close ();
		timer.stop ();
		timer = null;
		CallBack (this, Connect.OPEN_FAIL_CHANGED);
		Log.warning (this.GetType () + ", open is timeout," + this);
	}

	/** 连接回调 */
	private void connectCallback (IAsyncResult asyncConnect)
	{
		if (timer != null) {
			timer.stop ();
			timer = null;
		} else {
			return;
		}
		if (socket.Connected) {
			if(Log.isInfoEnable()) Log.info (this.GetType () + ", open is success," + this);
			socket.SendTimeout = SENDTIMEOUT;
			setLocalAddress (socket.LocalEndPoint.ToString ());
			opened ();
		} else {
			Log.warning (this.GetType () + ", open is fail," + this);
			CallBack (this, Connect.OPEN_FAIL_CHANGED);
		}
	}
	/** 发送数据 */
	public override void send(ByteBuffer data)
	{
		this.send(data.getArray(),data.offset(),data.length());
	}
	/** 发送数据 */
	public void send (byte[] data, int offset, int len)
	{
		if (!Active) return;
		if (len > MAX_DATA_LENGTH)
			throw new Exception (this.GetType () + ", send, data overflow:"
			                     + len + ", " + this);
		try {
			int length = ByteKit.getWriteLength (len);
			if(offset>=length) 
			{
				offset-=length;
				ByteKit.writeLength (len, data, offset);
				socket.Send(data, offset, len + length, SocketFlags.None);
			}
			else
			{
				length += len;
				ByteBuffer newData=ByteBuffer.GetByteBuffer();
				newData.setCapacity(length);
				int postion = ByteKit.writeLength(len, newData.getArray(), 0);
				Buffer.BlockCopy (data, offset, newData.getArray(), postion, len);
				socket.Send(newData.getArray(), 0, length, SocketFlags.None);
				newData.recycle();
			}
		} catch (SocketException e) {
			Debug.LogError (e);
			Close (Connect.CLOSEBYSEND);
		} catch (Exception e) {
			Debug.LogError (this.GetType () + ", send error, " + this+ e.ToString());
		}
	}
	/** 接收方法 */
	public override void receive ()
	{
		if(Active&&socket.Connected&&socket.Available>0)
		{
			receiveData();
		}
	}
	/** 接收数据 */
	private void receiveData()
	{
		try 
		{
			while (readBuffer() > 0) ;
		}
		catch (Exception ex) 
		{
			Debug.LogError (this.GetType () + ", receive error, available="+socket.Available + "," + this+ ex.ToString());
		}
	}
	/** 从块中读取数据，返回块中还可读取的长度 */
	protected int readBuffer()
	{
		if(data==null)
		{
			createData();
			if(data==null) return 0;
		}
		int len1=socket.Available;
		if(len1==0) return 0;
		int top=data.top;
		int len2=data.mark-data.top;
		if(len2>len1)
		{
			socket.Receive(data.getArray(),top,len1,SocketFlags.None);
			data.setTop(top+len1);
			return 0;
		}
		socket.Receive(data.getArray(),top,len2,SocketFlags.None);
		data.setTop(top+len2);
		receive(data);
		data.recycle();
		data=null;
		return len1-len2;
	}
	/** 创建字节缓存对象，有时需要二次读取才能获得字节缓存的长度 */
	private void createData()
	{
		if(headIndex==0)
		{
			if(socket.Available==0) return;
			socket.Receive(head,headIndex++,1,SocketFlags.None);
		}
		int n=getHeadLength(head,headIndex);
		while(headIndex<n)
		{
			if(socket.Available==0) return;
			socket.Receive(head,headIndex++,1,SocketFlags.None);
		}
		headIndex=0;
		data=createDataByHead(head,n);
	}
	/** 根据头信息创建字节缓存对象 */
	protected ByteBuffer createDataByHead(byte[] head,int length)
	{
		int len=ByteKit.readLength(head,0);
		if(len>MAX_DATA_LENGTH)
			throw new Exception(this+" createDataByHead, data overflow:"
			                    +len);
		ByteBuffer data = ByteBuffer.GetByteBuffer();
		data.setCapacity(len);
		data.mark=len;
		return data;
	}
	/** 根据当前的数据头获得头信息的长度 */
	protected int getHeadLength(byte[] head,int headIndex)
	{
		return ByteKit.getReadLength(head[0]);
	}

	/** 销毁方法 */
	public override void Close (int closeType)
	{
		base.Close (closeType);
		try {
			if (socket != null) 
			{
				if (socket.Connected)
				{
					if(closeType!=Connect.CLOSEBYUSER&&socket.Available>0)
						receiveData();
					socket.Shutdown (SocketShutdown.Both);
				}
				socket.Close ();
			}
		} catch (SocketException e) {
			Debug.LogError (e);
		} finally {
			try {
				if (socket != null)
					socket.Close ();
				socket = null;
				if(this.data!=null) 
					this.data.recycle();
			} catch (Exception ex) {
				Debug.LogError (ex);
			}
		}
	}
}