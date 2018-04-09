using System;
using UnityEngine;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using ABSystem;

/// <summary>
/// 模板工厂
/// 汪松民
/// </summary>
public class SampleFactory
{
	/* static fields */
	/** 模板数量 */
	public const int COUNT = 0xffff;
	/** 类名位置 */
	public const int CLASSINDEX = 1;
	/** 域条 */
	public const string FIELD = "field:";
	/** 类 */
	public const string CLASS = "class";
	/** 空模板 */
	public static readonly Sample[] EMPTY_ARRAY = {};
	/** 注释行 */
	public static string NOTE = "#";
	/** 分隔符 */
	public static char SPLITCHAR = '|';
	/** 所有模板 */
	private static Dictionary<string,SampleFactory> factorys = new Dictionary<string, SampleFactory>();
	/** 配置资源路径 */
	public const string CONFIGPATH="Config/";
	/* fields */
	/** 模板数组 */
	Dictionary<int,Sample> sampleArray;
	/** 配置文件 */
	string configName;

	/* static methods */
	public static FieldInfo getField (Type type, string name)
	{
		Type temp = type;
		FieldInfo field = null;
		do {
			field = temp.GetField (name, BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			if (field != null) {
				return field;
			} else {
				temp = temp.BaseType;
			}
		} while (temp!=null);
		return field;
	}
	/** 重置所有配置 */
	public static IEnumerator ResetAll (Action action,List<string> updataList)
	{
		StringBuilder builder = new StringBuilder ();
		foreach (var item in factorys) 
		{
			builder.Remove(0,builder.Length);
			builder.Append(ResourceHelper.CONFIGPATH);
			builder.Append(item.Key);
			builder.Append(ResourceHelper.SUFFIX);
			if(updataList.Contains(builder.ToString().ToLower()))
			{
				item.Value.initData();
				yield return null;
			}
		}
		action ();
	}

	/* methods */
	/** 构造方法 */
	public SampleFactory (string configName)
	{
		this.configName = configName;
#if UNITY_EDITOR
		if(factorys.ContainsKey(configName))
		{
			Debug.LogError("check code,don't create the same config!",null);
			factorys[configName] = this;
		}
		else factorys.Add(configName,this);
#else
		factorys.Add(configName,this);
#endif
		initData();
	}
	/** 加载资源 */
	private void initData()
	{
		ResData resData=ResourceHelper.Instance.LoadResData(ResourceHelper.CONFIGPATH+configName);
		TextAsset asset=resData.LoadMainAsset<TextAsset>();
		string text = asset.text;
		resData.Release();
		int num = getConfigLength (text);
		sampleArray = new Dictionary<int, Sample> (num);
		loadConfig(text);
	}
	/** 获取配置条数 */
	private int getConfigLength(string text)
	{
		int num = 0;
		int i = 0,length=text.Length;
		char ch;
		while(i<length)
		{
			i = text.IndexOf ('\n', i);
			if (i < 0) break;
			i++;
			if (i >= length) break;
			ch = text [i];
			if (ch != '#' && ch != 'f')
				num++;
		}
		return num;
	}
	/** 读取配置 */
	private void loadConfig(string content)
	{
		int start = 0,end=0,length=0,totalLength=content.Length;
		bool field = true;
		string str = null;
		int[] values = new int[0];

		//当前类型
		Type curType=null;
		//当前名称
		string className=null;
		//当前域列表
		Dictionary<string,FieldInfo> fieldMap = new Dictionary<string, FieldInfo> ();

		while(true)
		{
			if(field)
			{
				if(start>=totalLength) break;
				end=content.IndexOf('\n',start);
				if(end<0) break;
				length=end-start;
				if(length>0&&content[end-1]=='\r') length--;
				str=content.Substring(start,length);
				start=end+1;
				if (string.IsNullOrEmpty(str) || !str.StartsWith(FIELD)) continue;
			}
			else field=true;
			string[] fields = str.Substring(FIELD.Length).Split(SPLITCHAR);
			if(values.Length<=fields.Length) values=new int[fields.Length+1];
			while(true)
			{
				if(start>=totalLength) break;
				end=content.IndexOf('\n',start);
				if(end<0) end=totalLength;
				length=end-start;
				if(length>0&&content[end-1]=='\r') length--;
				if(length<=0||content[start]=='#')
				{
					start=end+1;
					continue;
				}
				if(content[start]=='f')
				{
					field=false;
					str=content.Substring(start,length);
					start=end+1;
					break;
				}
				length=TextKit.splitIntArray(content,start,start+length,SPLITCHAR,values);
				start=end+1;
				if(!loadSample(fields,content,values,length,ref className,ref curType,fieldMap))
				{
					Debug.LogError("config parse error");
					return;
				}
			}
		}
	}
	/** 获取类名 */
	private string getClassName(string[] fields,string str,int[] values,int length)
	{
		if(fields.Length != length-1)
		{
			Debug.LogError(GetType().FullName + ",config is error,fields length not equal values",null);
			return string.Empty;
		}
		for(int i = 0; i < fields.Length; i ++)
		{
			if(CLASS.Equals(fields[i],StringComparison.CurrentCultureIgnoreCase)) return str.Substring(values[i]+1,values[i+1]-values[i]-1);
		}
		return string.Empty;
	}
	/** 解析sample */
	private bool loadSample(string[] fields,string str,int[] values,int length,ref string curClassName,ref Type curType,Dictionary<string,FieldInfo> fieldMap)
	{
		string className = getClassName(fields,str,values,length);
		if (string.IsNullOrEmpty (className)) {
			Debug.LogError("className is null" + TextKit.toString(fields) + "\n" + TextKit.toString(values),null);
			return false;
		}
		if (className != curClassName) 
		{
			curType=DomainAccess.getType(className);
			curClassName=className;
			fieldMap.Clear();
		}
		Sample sample = null;
		try
		{
			sample = (Sample)DomainAccess.LoadObject(curType);
		}
		catch(Exception ex)
		{
			Debug.LogError("className:"+className+ ex.ToString());
			return false;
		}
		if(sample == null)
		{
			Debug.LogError("Sample:" + className + ",is not exist!",null);
			return false;
		}
		FieldInfo field = null;
		for(int i = 0; i < fields.Length; i ++)
		{
			if(CLASS.Equals(fields[i],StringComparison.CurrentCultureIgnoreCase))
				continue;
			if(values[i+1]-values[i]<=1)
			   continue;
			if(!fieldMap.TryGetValue(fields[i],out field))
			{
				field = getField(sample.GetType(),fields[i]);
				fieldMap.Add(fields[i],field);
			}
			if(field==null)
			{
				Debug.LogError(typeof(SampleFactory)+","+sample.GetType()+" field is error,"+fields[i],null);
				return false;
			}
			try
			{
				field.SetValue (sample,changeType(str,values[i]+1,values[i+1],field.FieldType), BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, null);
			}
			catch(Exception ex)
			{
				Debug.LogError("class:" + className + ",field:" + fields[i] + ",value:" + str.Substring(values[i]+1,values[i+1])+ ex.ToString());
				return false;
			}
		}
		sample.initSample ();
#if UNITY_EDITOR
		if(sampleArray.ContainsKey(sample.Sid)) Debug.LogError(typeof(SampleFactory)+","+sample.GetType()+"sid repate:"+sample.Sid,null);
#endif
		sampleArray[sample.Sid] = sample;
		return true;
	}
	/** str to Obj */
	public static object changeType(string str,int startIndex,int endIndex,Type type)
	{
		if (type == typeof(int))
			return TextKit.parseInt (str, startIndex, endIndex);
		else if (type == typeof(float))
			return TextKit.parseFloat (str, startIndex, endIndex);
		else if (type == typeof(string))
			return str.Substring (startIndex, endIndex - startIndex);
		else if (type == typeof(int[]))
			return TextKit.parseIntArray (str,startIndex,endIndex,',');
		else if (type == typeof(string[]))
			return str.Substring(startIndex,endIndex-startIndex).Split ('#');
		else if (type == typeof(float[]))
			return TextKit.parseFloatArray (str.Substring(startIndex,endIndex-startIndex),',');
		else if (type == typeof(byte[]))
			return TextKit.parseByteArray (str.Substring(startIndex,endIndex-startIndex).Split (','));
		else if (type == typeof(long[]))
			return TextKit.parseLongArray (str.Substring(startIndex,endIndex-startIndex).Split (','));
		else if (type == typeof(bool))
			return TextKit.parseBoolean (str.Substring(startIndex,endIndex-startIndex));
		else if (type.BaseType == typeof(Enum))
			return TextKit.parseEnum (type, str.Substring(startIndex,endIndex-startIndex));
		else if (type == typeof(UnityEngine.Vector3)) {
			float[] fs = TextKit.parseFloatArray (str.Substring(startIndex,endIndex-startIndex),',');
			return new UnityEngine.Vector3 (fs [0], fs [1], fs [2]);
		} else if (type == typeof(int[][])) {
			string[] strs = str.Substring(startIndex,endIndex-startIndex).Split ('#');
			int[][] array = new int[strs.Length][];
			for (int i=0; i<array.Length; i++) {
				array [i] = TextKit.parseIntArray (strs [i],0,strs[i].Length,',');
			}
			return array;
		} else if (type == typeof(float[][])) {
			string[] strs = str.Substring(startIndex,endIndex-startIndex).Split ('#');
			float[][] array = new float[strs.Length][];
			for (int i=0; i<array.Length; i++) {
				array [i] = TextKit.parseFloatArray (strs [i],',');
			}
			return array;
		}
		return Convert.ChangeType(str.Substring(startIndex,endIndex-startIndex),type);
	}
	/** 获取模板 */
	public Sample getSample (int sid)
	{
		Sample sample;
		if (sampleArray.TryGetValue (sid, out sample))
			return sample;
		return null;
	}
	/** 获取模板数组 */
	public Sample[] getSamples ()
	{
		Sample[] samples=new Sample[sampleArray.Count];
		sampleArray.Values.CopyTo (samples, 0);
		return samples;
	}
	/** 根据泛型获取模板 */
	public T getSample<T>(int sid) where T : Sample{
		return getSample (sid) as T;
	}

	/** 设置模板 */
	public void setSample (Sample sample)
	{
		sampleArray [sample.Sid] = sample;
	}
	/* methods */
	public Sample newSample (int sid)
	{
		Sample sample = getSample (sid);
		if (sample == null)
			return null;
		return (Sample)sample.Clone ();
	}

	public T newSample<T>(int sid)where T : Sample{
		return newSample (sid) as T;
	}

	public Sample[] newSamples (int start, int end)
	{
		if (start < 0)
			start = 0;
		if (start >= end)
			return EMPTY_ARRAY;
		Sample[] temp = new Sample[end - start];
		Sample s;
		for (int i=start,j=0; i<end; i++,j++) {
			s = getSample (i);
			if (s == null)
				continue;
			temp [j] = (Sample)s.Clone ();
		}
		return temp;
	}

	/** 选片 */
	public T[] newSamples<T>(int start,int end) where T : Sample{
		Sample [] samples = newSamples(start,end);
		if (samples == EMPTY_ARRAY)
			return new T[0];
		T[] ts = new T[samples.Length];
		for(int i=0;i<samples.Length;i++){
			ts[i] = samples[i] as T;
		}
		return ts;
	}
}
