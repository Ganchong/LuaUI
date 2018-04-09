using System;
using System.Text;

/**
 * 字符串
 * 
 * @author 汪松民
 */
public sealed class JsonString : IJsonNode
{
	/* fields */
	/** 值 */
	string value;

	/* methods */
	/** 构造方法 */
	public JsonString()
	{
	}
	public JsonString(String value)
	{
		this.value = value;
	}
	/** 获取类型 */
	public Jsontype getType()
	{
		return Jsontype.Value_String;
	}
	/** 大小 */
	public int Count
	{
		get {
			return 1;
		}
	}
	/**　浏览方法 */
	public void Scan(ScanObj scan)
	{
		String _value="";
		for (int i=scan.seed+1;i<scan.json.Length;i++)
		{
			char c=scan.json[i];
			if (c=='\\')
			{
				i++;
				c=scan.json[i];
				_value+=c;
			}
			else if (c!='\"')
			{
				_value+=c;
			}
			else
			{
				scan.seed=i+1;
				break;
			}
		}
		value=_value;
	}
	/** 设置值 */
	public void setValue(string value)
	{
		this.value = value;
	}

	public bool ToBoolean()
	{
		return false;
	}
	public double ToDouble() 
	{
		return 0;
	}
	public float ToFloat() 
	{
		return 0;
	}
	public int ToInt() 
	{
		return 0;
	}
	public IJsonNode this[int index] 
	{
		get {
			return null;
		}
	}
	public IJsonNode this[string key] 
	{
		get {
			return null;
		}
	}

	/* common methods */
	public override string ToString()
	{
		return value;
	}
	public void ConvertToString(StringBuilder sb)
	{
		sb.Append('\"');
		if (value != null)
		{
			String v = value.Replace("\\", "\\\\");
			v = v.Replace("\"", "\\\"");
			sb.Append(v);
		}
		sb.Append('\"');
	}
}