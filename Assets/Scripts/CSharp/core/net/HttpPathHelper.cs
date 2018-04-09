using System;
using System.Text;

/// <summary>
/// HTTP路径
/// 汪松民
/// </summary>
public class HttpPathHelper
{
	/* static fields */
	/** URL */
	public const string ServerUrl = "https://%1:%2/";
	/** 当前URL */
	public static string HttpHost = string.Empty;

	/** 获取PID地址 */
	public const string GETPIDURL = "getRolePid?certifyID=%1&uid=%2&plantID=%3&serverID=%4";
	/** 创建角色地址 */
	public const string CreateRoleURL = "createRole?certifyID=%1&uid=%2&plantID=%3&serverID=%4&name=%5&headStyle=%6";
	/** 错误日志地址 */
	public const string ErrorLogUrl = "clienterror?certifyID=%1&uid=%2&pid=%3&name=%4&type=%5&error=%6";

	/* static methods */
	public static string UrlEncode(string str)
	{
		return NameKit.encode (str);
	}
}