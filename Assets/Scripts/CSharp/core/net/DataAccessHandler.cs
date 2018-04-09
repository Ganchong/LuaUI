/**
 * Coypright 2013 by 刘耀鑫<xiney@youkia.com>.
 */
using System;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

/**
 * @author 刘耀鑫
 */
public class DataAccessHandler : TransmitHandler
{

	/* static fields */
	/** 默认的线程等待时间为10秒 */
	public const int TIMEOUT = 5;
	/** 访问结果的动作参数定义 */
	public const int ACCESS_ERROR = -1, ACCESS_EXCEPTION = 0, ACCESS_OK = 1;
	/** 数据标记 */
	public const int DATAMARK=1000;

	/** 当前的数据访问处理器 */
	static DataAccessHandler handler = new DataAccessHandler ();

	/** 错误信息 */
	public static string err2 = typeof(DataAccessHandler).ToString () + " access, timeout";
	public static string err3 = typeof(DataAccessHandler).ToString () + " access, io error";
	public static string err4 = typeof(DataAccessHandler).ToString () + " parseData, server data error";

	/* static methods */
	/** 获得当前的数据访问处理器 */
	public static DataAccessHandler instance 
	{
		get 
		{
			return handler;
		}
	}

	/* fields */
	/** 访问返回端口 */
	int accessReturnPort = PortService.ACCESS_RETURN_PORT;
	/** 线程等待时间 */
	int timeout = TIMEOUT;
	/** 对象池 */
	ObjectPool<Entity> pool=new ObjectPool<Entity>();
	/** 当前实体列表 */
	Dictionary<int,Entity> entities=new Dictionary<int, Entity>();
	/** 待移除实体 */
	List<Entity> removeEntities=new List<Entity>();

	/* methods */
	/**
	 * 消息处理方法， 参数connect为连接， 参数data是传送的消息，
	 */
	public void transmit (Connect connect, ByteBuffer data)
	{
		accessCall (data.readInt (), data);
	}
	/** 数据访问回调方法 */
	public void accessCall (int id, ByteBuffer data)
	{
		//MaskWindow.unLockNet();
		Entity entity;
		if(!entities.TryGetValue(id,out entity))
			return;
		ActionEvent ae = entity.actionEvent;
		ActionListener listener = (ActionListener)ae.source;
		DataAccessException e = parseData (data);
		if (e != null) {
			ae.type = ACCESS_EXCEPTION;
			ae.parameter = e;
		} else {
			ae.type = ACCESS_OK;
			ae.parameter = data;
		}
		listener.action (ae);
		if(entity.data.mark==DATAMARK) entity.data.recycle();
		entities.Remove(id);
		pool.Recycle(entity);
	}

	/** 向指定连接的服务进行数据访问 */
	public void access (int port, ByteBuffer data, ActionEvent action)
	{
		access (port, data, action, timeout);
	}
	/** 向指定连接的服务进行数据访问，可以设置超时值 */
	public void access (int port, ByteBuffer data,ActionEvent action, int timeout)
	{
		Entity entity = pool.Get ();
		int id = Entity.newID ();
		entity.id = id;
		entity.port = port;
		entity.actionEvent=action;
		entities [id] = entity;
		accessSend (entity, port, id, data);
	}
	
	/** 访问发送方法 */
	public void accessSend (Entity entity, int port, int id, ByteBuffer data)
	{
		if (data.offset () >= 8) 
		{
			byte[] bytes = data.getArray ();
			int position = data.offset ();
			position -= 8;
			data.setOffset (position);
			ByteKit.writeShort ((short)port, bytes, position);
			ByteKit.writeShort ((short)accessReturnPort, bytes, position + 2);
			ByteKit.writeInt (id, bytes, position + 4);
			entity.data = data;
		}
		else 
		{
			int length=ByteKit.getWriteLength(data.length()+8);
			ByteBuffer temp=ByteBuffer.GetByteBuffer(length);
			temp.mark=DATAMARK;
			temp.setCapacity(length+data.length()+8);
			temp.top=length+data.length()+8;

			int offset=temp.offset();

			byte[] bytes=temp.getArray();

			ByteKit.writeShort ((short)port, bytes, offset);
			ByteKit.writeShort ((short)accessReturnPort, bytes, offset+2);
			ByteKit.writeInt (id, bytes, offset+4);
			Buffer.BlockCopy (data.getArray (), data.offset (), bytes, offset+8, data.length ());

			entity.data = temp;
		}

		sendData(entity.data,false);
	}
	/** 发送数据包 */
	public void sendData(ByteBuffer data,bool reconnect)
	{
		Connect connect = new Connect();//=TcpManager.connect;
		if(connect!=null&&connect.Active)
		{
			connect.send(data);
		}
		//MaskWindow.lockNet();
	}
	/** 分析返回的响应消息数据 */
	DataAccessException parseData (ByteBuffer data)
	{
		try {
			int t = data.readUnsignedShort ();
			if (t == DataAccessException.OK)
				return null;
			return new DataAccessException (t, data.readUTF ());
		} catch (Exception e) {
			Debug.LogError (e);
			return new DataAccessException (
				DataAccessException.CLIENT_SDATA_ERROR, err4);
		}
	}
	/** 重新请求 */
	public void reaccess()
	{
		lock (entities)
		{
			Connect c = new Connect();//=TcpManager.connect;
			if(c==null) return;
			foreach(var item in entities.Values)
			{
				sendData(item.data,true);
			}
		}
	}
	/** 关闭 */
	public void close ()
	{
		entities.Clear ();
	}
	/** 移除重连放弃的请求 */
	public void removeAccessForReLogin()
	{
		lock (entities)
		{
			foreach(var item in entities.Values)
			{
				switch(item.port)
				{
				case PortService.LOGIN_PORT:
				case PortService.LOAD_PORT:
				case PortService.TIME_PORT:
					removeEntities.Add(item);
					break;
				}
			}
			for(int i=0;i<removeEntities.Count;i++)
				entities.Remove(removeEntities[i].id);
			removeEntities.Clear();
		}
	}
	private DataAccessHandler ()
	{
	}

	/* inner class */
	/** 实体类型 */
	public class Entity : IPoolObject<Entity>
	{
		/* static fields */
		/** 通讯ID */
		static int oid;
		/** 同步锁 */
		static readonly object mutex=new object();
		
		/* fields */
		/** 消息ID */
		public int id;
		/** 超时时间 */
		public int timeOut;
		/** 通讯子类型 */
		public int type;
		/** 端口 */
		public int port;
		/** 源 */
		public object source;
		/** 数据 */
		public ByteBuffer data;
		/** 回调 */
		public ActionEvent actionEvent;
		
		/* methods */
		/** 强转 */
		public Entity Cast()
		{
			id = 0;
			timeOut = 0;
			type = 0;
			data = null;
			source = null;
			return this;
		}
		
		/* static methods */
		/** 新的ID */
		public static int newID()
		{
			lock (mutex) 
			{
				if (oid == int.MaxValue) oid = 0;
				return ++oid;
			}
		}
	}
}
