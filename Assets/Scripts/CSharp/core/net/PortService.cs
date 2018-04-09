/**
 * Coypright 2013 by 刘耀鑫<xiney@youkia.com>.
 */
using System;
using UnityEngine;
using System.Collections.Generic;

/**
 * @author 刘耀鑫
 */
public class PortService : TransmitHandler
{

	/* static fields */
	/* 标准端口常量定义 */
	/** 反射端口 */
	public const int ECHO_PORT = 1;
	/** ping接收端口 */
	public const int PING_PORT = 2;
	/** 消息访问返回端口 */
	public const int ACCESS_RETURN_PORT = 4;
	/** 时间端口 */
	public const int TIME_PORT = 6;
	/** 属性端口 */
	public const int ATTRIBUTE_PORT = 11;
	/** 文件服务端口 */
	public const int FILE_PORT = 21;
	/** 授权文件服务端口 */
	public const int AUTHORIZED_FILE_PORT = 22;
	/* 认证中心标准端口常量定义 */
	/** 认证中心认证端口 */
	public const int CC_CERTIFY_PORT = 101;
	/** 认证中心加载端口 */
	public const int CC_LOAD_PORT = 102;
	/** 认证中心活动端口 */
	public const int CC_ACTIVE_PORT = 103;
	/** 认证中心退出端口 */
	public const int CC_EXIT_PORT = 104;
	/* 数据中心标准端口常量定义 */
	/** 数据中心登陆端口 */
	public const int DC_LOGIN_PORT = 111;
	/** 数据中心加载端口 */
	public const int DC_LOAD_PORT = 112;
	/** 数据中心保存端口 */
	public const int DC_SAVE_PORT = 113;
	/** 数据中心用户更新端口 */
	public const int DC_UPDATE_PORT = 121;
	/* 数据服务器标准端口常量定义 */
	/** 数据服务器认证码获取端口 */
	public const int CERTIFY_CODE_PORT = 201;
	/** 数据服务器认证代理端口 */
	public const int CERTIFY_PROXY_PORT = 202;
	/** 数据服务器登陆端口 */
	public const int LOGIN_PORT = 211;
	/** 数据服务器加载端口 */
	public const int LOAD_PORT = 212;
	/** 数据服务器退出端口 */
	public const int EXIT_PORT = 213;
	/** 数据服务器CLL端口（认证端口＋登陆端口＋加载端口） */
	public const int CLL_PORT = 214;
	/** 数据服务器session活动更新端口 */
	public const int UPDATE_ACTIVE=215;
	/* 代理服务器标准端口常量定义 */
	/** 代理反射端口 */
	public const int PROXY_ECHO_PORT = 301;
	/** 代理ping接收端口 */
	public const int PROXY_PING_PORT = 302;
	/** 代理时间端口 */
	public const int PROXY_TIME_PORT = 306;
	/** 代理状态端口 */
	public const int PROXY_STATE_PORT = 310;
	/** 代理登陆端口 */
	public const int PROXY_LOGIN_PORT = 402;
	/** 代理退出端口 */
	public const int PROXY_EXIT_PORT = 404;
	/* 其它标准端口常量定义 */
	/** 连接注册端口常量定义 */
	public const int CONNECT_REGISTER_PORT = 601;
	/** 用户通知断线端口定义 */
	public const int ADVISE_OFFLINE_PORT = 701;
	/** 服务器列表端口定义 */
	public const int SERVER_LIST_PORT = 801;
	/** 提示条 */
	public const int MOVETIPS = 1001;
	/** 用户端口 */
	public const int PLAYERPROT = 1002;
	/** 聊天端口 */
	public const int MESSAGEPROT = 1003;
	/** 联盟端口 */
	public const int UNIONPORT = 1004;
	/** 在线信息端口 */
	public const int ONLINEPORT = 1005;
	/** PVP端口 */
	public const int PVPPORT = 1006;
	/** 广播端口 */
	public const int RADIOPORT = 1007;
	/** 踢人端口 */
	public const int KICKPORT = 1008;
	/** 功能开放端口 */
	public const int OPENFUNC = 1009;
	/** 部落战服务端口 */
	public const int UNIONFIGHTPORT = 1010;
	/** 比武大会服务端口 */
	public const int DUELPORT = 1011;
	
	/** 端口改变标志常量 */
	public const int HANDLER_CHANGED = 0, PORT_CHANGED = 1;

	/* fields */
	/** 过滤器 */
	TransmitHandler filter;
	/** 缺省的消息传送处理接口 */
	TransmitHandler transmitHandler;
	/** 内部端口对应的消息传送处理接口数组 */
	Dictionary<int,TransmitHandler> handlers=new Dictionary<int, TransmitHandler>();

	/* properties */
	/** 获得过滤器 */
	public TransmitHandler getFilter ()
	{
		return filter;
	}
	/** 设置过滤器 */
	public void setFilter (TransmitHandler handler)
	{
		filter = handler;
	}
	/** 获得缺省的消息传送处理接口 */
	public TransmitHandler getTransmitHandler ()
	{
		return transmitHandler;
	}
	/** 设置缺省的消息传送处理接口 */
	public void setTransmitHandler (TransmitHandler handler)
	{
		TransmitHandler old = transmitHandler;
		transmitHandler = handler;
		Log.debug (this.GetType () + ", setTransmitHandler, " + handler);
	}
	/** 获得指定端口对应的消息传送处理接口 */
	public TransmitHandler getPort (int port)
	{
		TransmitHandler handler;
		if(handlers.TryGetValue(port,out handler))
			return handler;
		return null;
	}
	/** 设置指定端口对应的消息传送处理接口 */
	public void setPort (int port, TransmitHandler handler)
	{
		handlers [port] = handler;
	}
	/* methods */
	/**
	 * 消息处理方法， 参数connect为连接， 参数data是传送的消息，
	 */
	public void transmit (Connect connect, ByteBuffer data)
	{
		if (filter != null)
			filter.transmit (connect, data);
		int port = data.readUnsignedShort ();
		TransmitHandler handler = getPort (port);
		if (handler != null) {
			try {
				handler.transmit (connect, data);
			} catch (Exception e) {
				Debug.LogError (this.GetType () + ", transmit error, port=" + port + ", " + connect
					+ ", " + handler+e.ToString());
//				ClientErrorHttpPort.Instance.access(ClientErrorHttpPort.PORT,handler+e.ToString());
			}
		} else {
			data.setOffset (data.offset () - 2);
			handler = getTransmitHandler ();
			if (handler != null) {
				try {
					handler.transmit (connect, data);
				} catch (Exception e) {
					Debug.LogError (this.GetType () + ", default transmit error, port=" + port
						+ ", " + connect + ", " + handler+e.ToString());
//					ClientErrorHttpPort.Instance.access(ClientErrorHttpPort.PORT,"default,"+handler+e.ToString());
				}
			}
			Debug.LogError (this.GetType () + ", default transmit error, no handler, port="
				+ port + ", " + connect, null);
//			ClientErrorHttpPort.Instance.access(ClientErrorHttpPort.PORT,"no handler, port=" + port);
		}
	}
}
