using System;
using System.Text;

/**
 * Json节点
 * 
 * @author 汪松民
 */
public interface IJsonNode 
{
	/* properties */
	/** 获取节点 */
	IJsonNode this[int index]
	{
		get;
	}
	IJsonNode this[string key]
	{
		get;
	}
	/** 大小 */
	int Count 
	{
		get;
	}

	/* methods */
	/** 获取类型 */
	Jsontype getType();
	/** 添加到字符串 */
	void ConvertToString(StringBuilder sb);
	/** 浏览对象 */
	void Scan(ScanObj scan);
	/** 获取BOOL */
	bool ToBoolean();
	/** 获取double */
	double ToDouble();
	/** 获取float */
	float ToFloat();
	/** 获取int */
	int ToInt();
}
