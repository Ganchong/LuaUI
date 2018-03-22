using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using LuaInterface;
using LuaUIFramework;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LuaFrameworkCore {
	
    public class Util {
		public static void Log(string str)
		{
			Debug.Log(str);
		}
		public static void LogError(string str)
		{
			Debug.LogError (str);
		}
		public static void LogWarning(string str)
		{
			Debug.LogWarning (str);
		}

    }
}