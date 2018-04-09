using System;
using UnityEngine;

public class TextKit
{
	public static readonly char FIRST_ASCII=' ';
	public static string toString (bool[] array)
	{
		CharBuffer cb = new CharBuffer (array.Length * 2 + 2);
		cb.append ('{');
		toString (array, ",", cb);
		cb.append ('}');
		return cb.getString ();
	}
	/** 将指定的布尔数数组转换成字符串 */
	public static string toString (bool[] array, string separator)
	{
		CharBuffer cb = new CharBuffer (array.Length * (separator.Length + 1));
		toString (array, separator, cb);
		return cb.getString ();
	}

	public static void toString (bool[] array, string separator,
		CharBuffer cb)
	{
		int n = array.Length - 1;
		for (int i=0; i<n; i++)
			cb.append ((array [i]) ? '1' : '0').append (separator);
		if (n >= 0)
			cb.append (array [n]);
	}

	public static string toString (byte[] array)
	{
		CharBuffer cb = new CharBuffer (array.Length * 5 + 2);
		cb.append ('{');
		toString (array, ",", cb);
		cb.append ('}');
		return cb.getString ();
	}

	public static string toString (byte[] array, string separator)
	{
		CharBuffer cb = new CharBuffer (array.Length * (separator.Length + 4));
		toString (array, separator, cb);
		return cb.getString ();
	}

	public static void toString (byte[] array, string separator, CharBuffer cb)
	{
		int n = array.Length - 1;
		for (int i=0; i<n; i++)
			cb.append (array [i]).append (separator);
		if (n >= 0)
			cb.append (array [n]);
	}

	public static string toString (short[] array)
	{
		CharBuffer cb = new CharBuffer (array.Length * 6 + 2);
		cb.append ('{');
		toString (array, ",", cb);
		cb.append ('}');
		return cb.getString ();
	}

	public static string toString (short[] array, string separator)
	{
		CharBuffer cb = new CharBuffer (array.Length * (separator.Length + 5));
		toString (array, separator, cb);
		return cb.getString ();
	}

	public static void toString (short[] array, string separator, CharBuffer cb)
	{
		int n = array.Length - 1;
		for (int i=0; i<n; i++)
			cb.append (array [i]).append (separator);
		if (n >= 0)
			cb.append (array [n]);
	}

	public static string toString (char[] array)
	{
		CharBuffer cb = new CharBuffer (array.Length * 2 + 2);
		cb.append ('{');
		toString (array, ",", cb);
		cb.append ('}');
		return cb.getString ();
	}

	public static string toString (char[] array, string separator)
	{
		CharBuffer cb = new CharBuffer (array.Length * (separator.Length + 1));
		toString (array, separator, cb);
		return cb.getString ();
	}

	public static void toString (char[] array, string separator, CharBuffer cb)
	{
		int n = array.Length - 1;
		for (int i=0; i<n; i++)
			cb.append (array [i]).append (separator);
		if (n >= 0)
			cb.append (array [n]);
	}

	public static string toString (int[] array)
	{
		CharBuffer cb = new CharBuffer (array.Length * 9 + 2);
		cb.append ('{');
		toString (array, ",", cb);
		cb.append ('}');
		return cb.getString ();
	}

	public static string toString (int[] array, string separator)
	{
		CharBuffer cb = new CharBuffer (array.Length * (separator.Length + 8));
		toString (array, separator, cb);
		return cb.getString ();
	}

	public static void toString (int[] array, string separator, CharBuffer cb)
	{
		int n = array.Length - 1;
		for (int i=0; i<n; i++)
			cb.append (array [i]).append (separator);
		if (n >= 0)
			cb.append (array [n]);
	}

	public static string toString (long[] array)
	{
		CharBuffer cb = new CharBuffer (array.Length * 16 + 2);
		cb.append ('{');
		toString (array, ",", cb);
		cb.append ('}');
		return cb.getString ();
	}

	public static string toString (long[] array, string separator)
	{
		CharBuffer cb = new CharBuffer (array.Length * (separator.Length + 15));
		toString (array, separator, cb);
		return cb.getString ();
	}

	public static void toString (long[] array, string separator, CharBuffer cb)
	{
		int n = array.Length - 1;
		for (int i=0; i<n; i++)
			cb.append (array [i]).append (separator);
		if (n >= 0)
			cb.append (array [n]);
	}

	public static string toString (float[] array)
	{
		CharBuffer cb = new CharBuffer (array.Length * 10 + 2);
		cb.append ('{');
		toString (array, ",", cb);
		cb.append ('}');
		return cb.getString ();
	}

	public static string toString (float[] array, string separator)
	{
		CharBuffer cb = new CharBuffer (array.Length * (separator.Length + 9));
		toString (array, separator, cb);
		return cb.getString ();
	}

	public static void toString (float[] array, string separator, CharBuffer cb)
	{
		int n = array.Length - 1;
		for (int i=0; i<n; i++)
			cb.append (array [i]).append (separator);
		if (n >= 0)
			cb.append (array [n]);
	}

	public static string toString (double[] array)
	{
		CharBuffer cb = new CharBuffer (array.Length * 16 + 2);
		cb.append ('{');
		toString (array, ",", cb);
		cb.append ('}');
		return cb.getString ();
	}

	public static string toString (double[] array, string separator)
	{
		CharBuffer cb = new CharBuffer (array.Length * (separator.Length + 15));
		toString (array, separator, cb);
		return cb.getString ();
	}

	public static void toString (double[] array, string separator, CharBuffer cb)
	{
		int n = array.Length - 1;
		for (int i=0; i<n; i++)
			cb.append (array [i]).append (separator);
		if (n >= 0)
			cb.append (array [n]);
	}

	public static string toString (object[] array)
	{
		CharBuffer cb = new CharBuffer (array.Length * 25 + 2);
		cb.append ('{');
		toString (array, ",", cb);
		cb.append ('}');
		return cb.getString ();
	}

	public static string toString (object[] array, string separator)
	{
		CharBuffer cb = new CharBuffer (array.Length * (separator.Length + 24));
		toString (array, separator, cb);
		return cb.getString ();
	}

	public static void toString (object[] array, string separator, CharBuffer cb)
	{
		int n = array.Length - 1;
		for (int i=0; i<n; i++)
			cb.append (array [i]).append (separator);
		if (n >= 0)
			cb.append (array [n]);
	}

	public static int[] parseIntArray (string[] strs)
	{
		if (strs == null)
			return null;
		int[] array = new int[strs.Length];
		for (int i=0; i<strs.Length; i++)
			array [i] = parseInt (strs [i]);
		return array;
	}
	public static int splitIntArray(string str,int startIndex,int endIndex,char speparator,int[] array)
	{
		array [0] = startIndex-1;
		int num = 1;
		startIndex = str.IndexOf (speparator, startIndex);
		while (startIndex>=0&&startIndex<endIndex) 
		{
 			array[num++]=startIndex;
			startIndex = str.IndexOf (speparator, startIndex+1);
		}
		array [num] = endIndex;
		return num+1;
	}
	public static int[] parseIntArray (string str,int startIndex,int endIndex,char speparator)
	{
		int[] array;
		parseIntArray (str, endIndex, speparator, out array, startIndex, 0);
		return array;
	}
	private static void parseIntArray(string str,int endIndex,char spearator,out int[] array,int startIndex,int num)
	{
		int index = str.IndexOf (spearator, startIndex);
		if (index < 0||index>endIndex) 
		{
			array=new int[num+1];
			if (startIndex >= str.Length)
				return;
			int result=0;
			bool flag = str[startIndex]=='-';
			for (int i = flag?startIndex+1:startIndex; i < endIndex; i++) 
				result = result * 10 + (str [i] - '0');
			array[num]=flag?-result:result;
			return;
		}
		parseIntArray (str, endIndex, spearator, out array, index + 1, num + 1);
		int temp=0;
		bool tempFlag = str[startIndex]=='-';
		for (int i = tempFlag?startIndex+1:startIndex; i < index; i++)
			temp = temp * 10 + (str [i] - '0');
		array[num]=tempFlag?-temp:temp;
	}
	public static float[] parseFloatArray(string str,char speparator)
	{
		float[] array;
		parseFloatArray (str, speparator, out array, 0, 0);
		return array;
	}
	private static void parseFloatArray(string str,char spearator,out float[] array,int startIndex,int num)
	{
		int endIndex = str.IndexOf (spearator, startIndex);
		if (endIndex < 0) 
		{
			array=new float[num+1];
			str=str.Substring(startIndex,str.Length-startIndex);
			float.TryParse(str,out array[num]);
			return;
		}
		parseFloatArray (str, spearator, out array, endIndex + 1, num + 1);
		float.TryParse(str.Substring (startIndex, endIndex-startIndex),out array[num]);
	}

	public static float[] parseFloatArray(string[] strs)
	{
		if(strs == null) return null;
		float[] array = new float[strs.Length];
		for(int i = 0; i < strs.Length; i++)
			array [i] = parseFloat(strs[i]);
		return array;
	}

	public static byte[] parseByteArray (string[] strs)
	{
		if(strs==null) return null;
		byte[] array = new byte[strs.Length];
		for (int i=0; i<strs.Length; i++) {
			array[i] = (byte)parseInt(strs[i]);
		}
		return array;
	}

	public static long[] parseLongArray (string[] strs)
	{
		if (strs == null)
			return null;
		long[] array = new long[strs.Length];
		for (int i=0; i<strs.Length; i++)
			array [i] = parseLong (strs [i]);
		return array;
	}

	public static float parseFloat (string str)
	{
		return float.Parse(str);
	}
	public static float parseFloat (string str,int startIndex,int endIndex)
	{
		double result=0.0;
		bool flag = str[startIndex]=='-';
		int i = flag?startIndex+1:startIndex;
		for (;i<endIndex;i++)
		{
			if(str[i]=='.') break;
			result=result * 10 + (str [i] - '0');
		}
		double temp = 0.1;
		i++;
		for(;i<endIndex; i++)
		{
			result+=temp*(str [i] - '0');
			temp*=0.1;
		}
		return (float)(flag?-result:result);
	}

	public static int parseInt (string str)
	{
		return (int)parseLong (str);
	}
	public static int parseInt (string str,int startIndex,int endIndex)
	{
		int result=0;
		bool flag = str [startIndex] == '-';
		for(int i=flag?startIndex+1:startIndex;i<endIndex;i++)
			result=result*10+(str[i]-'0');
		return flag?-result:result;
	}
	public static long parseLong (string str)
	{
		if (string.IsNullOrEmpty (str))
			return 0;
		if (str.StartsWith("#"))
			return Convert.ToInt64 (str.Substring (1), 16);
		if (str.Length > 1 && str.StartsWith("0x"))
			return Convert.ToInt64 (str.Substring (2), 16);
		return Convert.ToInt64 (str);
	}
	
	public static bool parseBoolean (string str)
	{
		if (string.IsNullOrEmpty (str))
			return false;
		char[] chars = str.ToCharArray ();
		if (str.Length == 1)
			return chars [0] == '1';
		return str.Equals ("true", StringComparison.CurrentCultureIgnoreCase);
	}
	public static object parseEnum(Type type,string str)
	{
		try
		{
			return Enum.Parse (type, str, true);
		}
		catch(Exception ex)
		{
			return Activator.CreateInstance(type);
		}
	}


	public static char valid(string str,char[] charRangeSet)
	{
		char c;
		int len=str.Length;
		char[] chars = str.ToCharArray();
		for(int i=0;i<len;i++)
		{
			c=chars[i];
			if(c<FIRST_ASCII) return c;
			if(!valid(c,charRangeSet)) return c;
		}
		return Convert.ToChar(0);
	}
	public static bool valid(char c,char[] charRangeSet)
	{
		if(c<FIRST_ASCII) return false;
		if(charRangeSet==null) return true;
		for(int i=0,n=charRangeSet.Length-1;i<n;i+=2)
		{
			if(c>=charRangeSet[i]&&c<=charRangeSet[i+1]) return true;
		}
		return false;
	}
	
	/** 字符串替换方法 */
	public static string replace(string str, string target, string swap) {
		return replace(str, target, swap, false, null);
	}
	/**
	 * 字符串替换方法， 把str字符串中的从start到start+count的子字符串替换为swap
	 */
	public static string replace(string str, string swap, int start, int count) {
		int len = str.Length + swap.Length - count;
		CharBuffer cb = new CharBuffer(len);
		cb.append(str.Substring(0, start)).append(swap);
		cb.append(str.Substring(start + count));
		return cb.getString();
	}
	/**
	 * 字符串替换方法， 把str字符串中的从start到start+count的子字符串替换为swap， cb为临时使用的字符串缓存
	 */
	public static string replace(string str, string swap, int start, int count,
			CharBuffer cb) {
		int len = str.Length + swap.Length - count;
		cb.clear();
		cb.setCapacity(len);
		cb.append(str.Substring(0, start)).append(swap);
		cb.append(str.Substring(start + count));
		return cb.getString();
	}
	/**
	 * 字符串替换方法，使用java默认的字符串查找方法， str为字符串，target为目标字符串，
	 * swap为替换字符串，caseless为忽略大小写， cb为临时使用的字符串缓存
	 */
	public static string replace(string str, string target, string swap,
			bool caseless, CharBuffer cb) {
		string str1 = str;
		string target1 = target;
		if (caseless) {
			str1 = str.ToLower();
			target1 = target.ToLower();
		}
		int index = str1.IndexOf(target1);
		if (index < 0)
			return str;
		if (cb != null) {
			cb.clear();
			cb.setCapacity(str.Length + swap.Length - target.Length);
		} else
			cb = new CharBuffer(str.Length + swap.Length - target.Length);
		return replace(str, swap, index, target.Length, cb);
	}
	/**
	 * 替换全部的字符串替换方法，使用java默认的字符串查找方法， str为字符串，target为目标字符串，swap为替换字符串
	 */
	public static string replaceAll(string str, string target, string swap) {
		return replaceAll(str, target, swap, false, null);
	}
	/**
	 * 替换全部的字符串替换方法，使用java默认的字符串查找方法， str为字符串，target为目标字符串，
	 * swap为替换字符串，caseless为忽略大小写
	 */
	public static string replaceAll(string str, string target, string swap,
			bool caseless) {
		return replaceAll(str, target, swap, caseless, null);
	}
	/**
	 * 替换全部的字符串替换方法，使用java默认的字符串查找方法， str为字符串，target为目标字符串，
	 * swap为替换字符串，caseless为忽略大小写， cb为临时使用的字符串缓存
	 */
	public static string replaceAll(string str, string target, string swap,
			bool caseless, CharBuffer cb) {
		int len = target.Length;
		if (len == 0)
			return str;
		string str1 = str;
		string target1 = target;
		if (caseless) {
			str1 = str.ToLower();
			target1 = target.ToLower();
		}
		int start = 0;
		int end = str1.IndexOf(target1, start);
		if (end < 0)
			return str;
		if (cb != null) {
			cb.clear();
			cb.setCapacity(str.Length);
		} else {
			cb = CharBuffer.GetInstance(str.Length);
		}
		while (end >= 0) {
			if(start!=end) cb.append(str.Substring(start, end-start)).append(swap);
			else cb.append(swap);
			start = end + len;
			end = str1.IndexOf(target1, start);
		}
		cb.append(str.Substring(start));
		return cb.getString();
	}
	/** 文字替换 */
	public static string parse(string src,params object[] objects)
	{
		if(objects == null || objects.Length < 1) return src;
		string rep = string.Empty;
		for(int i = 0; i < objects.Length; i ++)
		{
			rep = "%" + (i + 1);
			src = replaceAll(src,rep,objects[i]==null?string.Empty:objects[i].ToString());
		}
		return src;
	}
}
