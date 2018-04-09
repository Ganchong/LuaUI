using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class KeyWordKit
{
	/* static fields */
	/** language key */
	private const string REPLACEKEY="invalidText";

	private static TextValidity textValidity = new TextValidity ();

	public static void setString (string[] strs)
	{
		textValidity.setInvalidTexts(strs);
		Log.debug ("The keywords length is :" + strs.Length);
	}

	public static string isKeyWords (string str)
	{
		return textValidity.valid (str);
	}
	
	/** 关键字替换 */
	public static string replaceKeyWords(string str)
	{
		string[] invalidTexts=textValidity.getInvalidTexts();
		if (invalidTexts == null)
			return str;
		for (int i=0,n=invalidTexts.Length; i<n; i++) {
			str=TextKit.replaceAll(str,invalidTexts [i],REPLACEKEY);
		}
		return str;
	}
}
