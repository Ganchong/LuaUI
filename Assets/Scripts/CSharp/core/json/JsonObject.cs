using System;
using System.Text;
using System.Collections.Generic;

/**
 * 对象
 * 
 * @author 汪松民
 */
public sealed class JsonObject : Dictionary<string, IJsonNode> , IJsonNode
{
	/* fields */

	/* methods */
	/** 获取类型 */
	public Jsontype getType()
	{
		return Jsontype.Object;
	}
	/** 根据下标获取节点 */
	public IJsonNode this[int index] 
	{
		get {
			return this;
		}
	}
	public new IJsonNode this[string key] 
	{
		get {
			IJsonNode node;
			if (TryGetValue (key,out node))
				return node;
			return null;
		}
	}
	/** 大小 */
	public new int Count
	{
		get {
			return base.Count;
		}
	}
	public bool ToBoolean() {
		return false;
	}
	public double ToDouble() {
		return 0;
	}
	public float ToFloat() {
		return 0;
	}
	public int ToInt() {
		return 0;
	}
	/** 添加字符串 */
	public void Add(string key,string value)
	{
		Add(key,new JsonString(value));
	}
	public void Add(string key,bool value)
	{
		JsonNumber number = new JsonNumber ();
		number.setBoolean (value);
		Add(key,number);
	}
	/** 添加整数 */
	public void Add(string key,int value)
	{
		Add(key,new JsonNumber(value));
	}
	/** 添加long */
	public void Add(string key,long value)
	{
		Add (key,new JsonNumber(value));
	}
	/** 添加double */
	public void Add(string key,double value)
	{
		Add (key,new JsonNumber(value));
	}
	/** 添加float */
	public void Add(string key,float value)
	{
		Add (key,new JsonNumber((double)value));
	}

	/** common methods */
	/** 字符串方法 */
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		ConvertToString(sb);
		return sb.ToString();
	}
	/** 追加字符串 */
	public void ConvertToString(StringBuilder sb)
	{
		sb.Append('{');
		int i = Count;
		foreach(var item in this)
		{
			sb.Append('\"');
			sb.Append(item.Key);
			sb.Append("\":");
			item.Value.ConvertToString(sb);
			i--;
			if (i != 0) sb.Append(',');
		}
		sb.Append('}');
	}
	/** 浏览方法 */
	public void Scan(ScanObj scan)
	{
		String key = null;
		int keystate = 0;//0 nokey 1scankey 2gotkey
		for (int i=scan.seed+1;i<scan.json.Length;i++)
		{
			char c=scan.json[i];
			if (keystate!=1&&(c==','||c==':'))
				continue;
			if (c=='}') {
				scan.seed=i+1;
				break;
			}
			if (keystate==0) {
				if (c=='\"')
				{
					keystate = 1;
					key="";
				}
			} else if (keystate==1) {
				if (c=='\"') {
					keystate=2;
					continue;
				} else {
					key+=c;
				}
			} else {
				IJsonNode node = MyJson.ScanFirst(c);
				if (node != null) {
					scan.seed=i;
					node.Scan(scan);
					i=scan.seed-1;
					if(ContainsKey(key))
						base[key]=node;
					else Add(key, node);
					keystate=0;
				}
			}
		}
	}
	public String getFirstKey01(String path, int start, StringCache nextpath)
	{
		for (int i=start+1;i<path.Length;i++)
		{
			if (path[i] == '\\') continue;
			if (path[i] == '\"')
			{
				nextpath.value=path.Substring(i + 1);
				return path.Substring(start + 1, i - start - 1);
			}
		}
		nextpath.value = null;
		return null;
	}
	public String getFirstKey02(String path, int start, StringCache nextpath)
	{
		String _path = null;
		for (int i=start+1;i<path.Length;i++)
		{
			if (path[i]=='[')
			{
				_path=getFirstKey02(path, i,nextpath);
			}
			if (path[i]=='\"')
			{
				_path = getFirstKey01(path, i,nextpath);
				i+=_path.Length + 2;
			}
			if (path[i] == ']')
			{
				nextpath.value=path.Substring(i+1);
				if (_path == null)
				{
					_path = path.Substring(start+1,i-start-1);
				}
				return _path;
			}
		}
		nextpath.value=null;
		return null;
	}
	public String getFirstKey(String path,StringCache nextpath)
	{
		nextpath.value = null;
		int istart = 0;
		for (int i=0;i<path.Length;i++)
		{
			if (path[i]=='.'||path[i]==' ')
			{
				istart++;
				continue;
			}
			if (path[i]=='[')
			{
				return getFirstKey02(path, i,nextpath);
			}
			else if (path[i]== '\"')
			{
				return getFirstKey01(path, i,nextpath);
			}
			else
			{
				int iend1 = path.IndexOf('[', i + 1);
				if (iend1 == -1) iend1 = path.Length;
				int iend2 = path.IndexOf('.', i + 1);
				if (iend2 == -1) iend2 = path.Length;
				int iss = iend1>iend2?iend2:iend1;
				nextpath.value = path.Substring(iss);
				return path.Substring(istart, iss - istart);
			}
		}
		return null;
	}
}