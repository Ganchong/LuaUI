using System;
using System.Text;
using UnityEngine;
using System.IO;

public class NameKit
{
	/* static fields */
	/** 不用转换的字符 */
	private const string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

	/* static methods */
	/** 中文转码 */
	public static string encode(string value)
	{
		StringBuilder builder = new StringBuilder();
		byte[] bytes = Encoding.UTF8.GetBytes (value);
		for(int i=0; i<bytes.Length; i++)
		{
			if(bytes[i]>=97&&bytes[i]<=122||bytes[i]>=65&&bytes[i]<=90||bytes[i]>=48&&bytes[i]<=57||"=,+;.'-@&/$_()!~*:".IndexOf((char)bytes[i])>=0)
			{
				builder.Append((char)bytes[i]);
			}
			else
			{
				builder.Append('%');
				builder.Append(string.Format("{0:X}",(15 & UnSignRight(bytes[i],4))));
				builder.Append(string.Format("{0:X}",(15 & bytes[i])));
			}
		}
		return builder.ToString();
	}
	/** 无符号右移 */
	public static int UnSignRight(int value,int offset)
	{
		if (offset == 0) return value;
		int mask = int.MaxValue;
		value >>= 1;
		value &= mask;
		value >>= offset - 1;
		return value;
	}
	/** 解码名称 */
	public static string decode(string name)
	{
		return WWW.UnEscapeURL(name);
	}
}

