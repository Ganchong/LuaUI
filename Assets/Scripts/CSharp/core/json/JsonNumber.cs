using System;
using System.Text;

/**
 * 值数据
 * 
 * @author 汪松民
 */
public sealed class JsonNumber : IJsonNode
{
	/* static fields */
	/** 类型常量 */
	public const int INT=0,BOOL=1,NULL=2,FLOAT=3;

	/* fields */
	/** 值 */
	long value;
	/** 类型 */
	int type;

	/* methods */
	/** 构造方法 */
	public JsonNumber ()
	{
	}

	public JsonNumber (long value)
	{
		this.value = value;
		type = INT;
	}
	public JsonNumber(double value)
	{
		this.value = BitConverter.DoubleToInt64Bits (value);
		type=FLOAT;
	}
	public JsonNumber(bool value)
	{
		setBoolean (value);
	}

	public Jsontype getType ()
	{
		return Jsontype.Value_Number;
	}

	/** 大小 */
	public int Count
	{
		get {
			return 1;
		}
	}

	public void setBoolean (bool v)
	{
		this.value = v ? 1 : 0;
		type=BOOL;
	}

	public void setNull ()
	{
		type = NULL;
		this.value = 0;
	}

	/** 设置值 */
	public void setValue ()
	{

	}

	/** 是不是空 */
	public bool isNull ()
	{
		return type==NULL;
	}

	/** 是不是BOOL */
	public bool isBoolean ()
	{
		return type==BOOL;
	}

	/** 浏览对象 */
	public void Scan (ScanObj scan)
	{
		String number = "";
		for (int i = scan.seed; i < scan.json.Length; i++) {
			char c = scan.json [i];
			if (c != ',' && c != ']' && c != '}' && c != ' ') {
				if (c != '\n')
					number += c;
			} else {
				scan.seed = i;
				break;
			}
		}
		if ("true".Equals (number.ToLower ())) {
			value = 1;
			type=BOOL;
		} else if ("false".Equals (number.ToLower ())) {
			value = 0;
			type=BOOL;
		} else if ("null".Equals (number.ToLower ())) {
			value = 0;
			type=NULL;
		} else {
			if (number.IndexOf (".") < 0)
			{
				value = long.Parse (number);
				type=INT;
			}
			else
			{
				value = BitConverter.DoubleToInt64Bits (double.Parse (number));
				type=FLOAT;
			}
		}
	}

	/** 获取BOOL */
	public bool ToBoolean ()
	{
		return value != 0;
	}
	/** 获取double */
	public double ToDouble ()
	{
		if(type==FLOAT)	return BitConverter.Int64BitsToDouble (value);
		return value;
	}
	/** 获取float */
	public float ToFloat ()
	{
		return (float)ToDouble ();
	}
	/** 获取int */
	public int ToInt ()
	{
		if (type == FLOAT) return (int)ToDouble ();
		return (int)value;
	}

	public IJsonNode this[int index] 
	{
		get {
			return this;
		}
	}

	public IJsonNode this[string key] 
	{
		get {
			return this;
		}
	}

	/* common methods */
	/** 字符串 */
	public override string ToString ()
	{
		if (type == BOOL) {
			return value != 0 ? "true" : "false";
		} else if (type == NULL) {
			return "null";
		} else if (type == FLOAT) {
			return ToDouble().ToString();
		} else {
			return value.ToString ();
		}
	}

	/** 添加到字符串 */
	public void ConvertToString (StringBuilder sb)
	{
		sb.Append (ToString ());
	}
}