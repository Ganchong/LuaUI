using System;
using System.Text;
using System.Collections.Generic;


/**
 * 数组
 * 
 * @author 汪松民
 */
public sealed class JsonArray : List<IJsonNode>,IJsonNode
{
	/* fields */

	/* methods */
	/** 获取类型 */
	public Jsontype getType()
	{
		return Jsontype.Array;
	}
	/** 根据下标获取节点 */
	public new IJsonNode this[int index] 
	{
		get {
			return base[index];
		}
	}
	public IJsonNode this[string key] 
	{
		get {
			return this;
		}
	}
	/** 大小 */
	public new int Count
	{
		get
		{
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
	/* common */
	/** 字符串 */
	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		ConvertToString(sb);
		return sb.ToString();
	}
	/** 添加到字符串 */
	public void ConvertToString(StringBuilder sb)
	{
		sb.Append('[');
		for (int i=0;i<Count;i++)
		{
			this[i].ConvertToString(sb);
			if (i!=Count-1)
				sb.Append(',');
		}
		sb.Append(']');
	}
	/** 浏览方法 */
	public void Scan(ScanObj scan)
	{
		for (int i=scan.seed+1;i<scan.json.Length;i++)
		{
			char c=scan.json[i];
			if (c==',')
				continue;
			if (c==']')
			{
				scan.seed=i+1;
				break;
			}
			IJsonNode node = MyJson.ScanFirst(c);
			if (node != null)
			{
				scan.seed = i;
				node.Scan(scan);
				i = scan.seed - 1;
				this.Add(node);
			}

		}
	}

	public int getFirstKey02(String path, int start, StringCache nextpath)
	{
		int _path = -1;
		for (int i = start + 1; i < path.Length; i++)
		{
			if (path[i] == '[')
			{
				_path = getFirstKey02(path, i, nextpath);
			}
			if (path[i] == ']')
			{
				nextpath.value = path.Substring(i + 1);
				if (_path == -1)
				{
					_path = int.Parse(path.Substring(start + 1, i - start - 1));
				}
				return _path;
			}
		}
		nextpath.value = null;
		return -1;
	}
	public int getFirstKey(String path, StringCache nextpath)
	{
		nextpath.value = null;
		for (int i = 0; i < path.Length; i++)
		{
			if (path[i]== '.'||path[i]==' ')
			{
				continue;
			}
			if (path[i] == '[')
			{
				return getFirstKey02(path, i, nextpath);
			}
		}
		return -1;
	}
}