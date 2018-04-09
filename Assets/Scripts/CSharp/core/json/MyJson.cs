using System;


/**
 * json主类
 * 
 * @author 汪松民
 */
public static class MyJson 
{
	/** 解析json */
	public static IJsonNode Parse(String json)
	{
		try
		{
			ScanObj obj = new ScanObj();
			obj.json = json;
			obj.seed = 0;
			IJsonNode node = Scan(obj);
			return node;
		}
		catch (Exception err)
		{
			throw new Exception("parse err:" + json, err);
		}
	}
	/** 浏览第一个字符确定类型 */
	public static IJsonNode ScanFirst(char c)
	{
		if (c == ' ' || c == '\n' || c == '\r' || c == '\t')
		{
			return null;
		}
		if (c == '{')
		{
			return new JsonObject();
		}
		else if (c == '[')
		{
			return new JsonArray();
		}
		else if (c == '"')
		{
			return new JsonString();
		}
		else
		{
			return new JsonNumber();
		}
	}
	/** 浏览对象 */
	public static IJsonNode Scan(ScanObj scan)
	{
		for (int i = 0; i < scan.json.Length; i++)
		{
			IJsonNode node = ScanFirst(scan.json[i]);
			if (node != null)
			{
				scan.seed = i;
				node.Scan(scan);
				return node;
			}
		}
		return null;
	}
	/** 扩展类 */
	public static IJsonNode ToJsonNode(this string json)
	{
		return Parse (json);
	}
}