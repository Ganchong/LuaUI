/**
 * Coypright 2013 by 刘耀鑫<xiney@youkia.com>.
 */
using System;

/**
 * @author 刘耀鑫
 */
public class DataAccessException:Exception
{

	/* static fields */

	/** 操作成功的返回码常量 */
	public const int OK = 200;

	/** 服务器端重定向 */
	public const int SERVER_REDIRECT = 300;

	/** 客户端一般错误 */
	public const int CLIENT_INTERNAL_ERROR = 400;
	/** 客户端访问参数错误（如无效的协议，无效的地址） */
	public const int CLIENT_PARAMETER_ERROR = 410;
	/** 客户端IO错误（如无法建立连接，数据发送失败） */
	public const int CLIENT_IO_ERROR = 420;

	/** 客户端访问超时 */
	public const int CLIENT_TIMEOUT = 440;
	/**
	 * 客户端的服务器数据错误（在读取服务器端的返回数据时， 可能会产生该异常）
	 */
	public const int CLIENT_SDATA_ERROR = 450;
	/**
	 * 客户端的服务器消息错误（在读取服务器端的返回消息时， 可能会产生该异常）
	 */
	public const int CLIENT_SMSG_ERROR = 451;

	/** 服务器端一般错误 */
	public const int SERVER_INTERNAL_ERROR = 500;
	/**
	 * 服务器端的客户数据错误（在读取客户端发送的数据时， 可能会产生该异常）
	 */
	public const int SERVER_CDATA_ERROR = 550;
	/**
	 * 服务器端的客户消息错误（在处理客户端发送的消息时， 可能会产生该异常）
	 */
	public const int SERVER_CMSG_ERROR = 551;
	/** 服务器端的访问被拒绝（如没有认证） */
	public const int SERVER_ACCESS_REFUSED = 560;

	/** 服务器端的文件没有找到错误 */
	public const int SERVER_FILE_NOT_FOUND = 570;

	/** 自定义错误 */
	public const int CUSTOM_ERROR = 600;

	/* static methods */
	/** 得到异常类型的文字表示 */
	public static String typeMessage (int type)
	{
		switch (type) {
		case SERVER_REDIRECT:
			return "SERVER_REDIRECT";
		case CLIENT_INTERNAL_ERROR:
			return "CLIENT_INTERNAL_ERROR";
		case CLIENT_PARAMETER_ERROR:
			return "CLIENT_PARAMETER_ERROR";
		case CLIENT_IO_ERROR:
			return "CLIENT_IO_ERROR";
		case CLIENT_TIMEOUT:
			return "CLIENT_TIMEOUT";
		case CLIENT_SDATA_ERROR:
			return "CLIENT_SDATA_ERROR";
		case CLIENT_SMSG_ERROR:
			return "CLIENT_SMSG_ERROR";
		case SERVER_INTERNAL_ERROR:
			return "SERVER_INTERNAL_ERROR";
		case SERVER_CDATA_ERROR:
			return "SERVER_CDATA_ERROR";
		case SERVER_CMSG_ERROR:
			return "SERVER_CMSG_ERROR";
		case SERVER_ACCESS_REFUSED:
			return "SERVER_ACCESS_REFUSED";
		case SERVER_FILE_NOT_FOUND:
			return "SERVER_FILE_NOT_FOUND";
		case CUSTOM_ERROR:
			return "CUSTOM_ERROR";
		default:
			return null;
		}
	}

	/* fields */
	/** 异常类型 */
	private int type;
	/** 访问地址 */
	private String address;

	/* constructors */
	/**
	 * 用给定的异常类型和错误信息构造一个数据访问异常， 异常类型可以使用常量定义，也可以使用自定义类型（一般大于600），
	 */
	public DataAccessException (int type, String message):this(type,message,null)
	{
	}
	/**
	 * 用给定的异常类型、错误信息及访问地址构造一个数据访问异常， 异常类型可以使用常量定义，也可以使用自定义类型（一般大于600），
	 */
	public DataAccessException (int type, String message, String address):base(message)
	{
		this.type = type;
		this.address = address;
	}
	/* properties */
	/** 得到异常类型 */
	public int getType ()
	{
		return type;
	}
	/** 得到异常类型的文字表示 */
	public String getTypeMessage ()
	{
		return typeMessage (type);
	}
	/** 得到访问地址 */
	public String getAddress ()
	{
		return address;
	}
	/** 设置访问地址 */
	public void setAddress (String address)
	{
		this.address = address;
	}
	/* common methods */
	public String toString ()
	{
		String str = typeMessage (type);
		if (str == null)
			str = type.ToString ();
		return this + ":" + str + ", address=" + address + ", "
			+ Message;
	}

}
