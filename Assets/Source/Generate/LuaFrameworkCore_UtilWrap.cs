﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class LuaFrameworkCore_UtilWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(LuaFrameworkCore.Util), typeof(System.Object));
		L.RegFunction("Log", Log);
		L.RegFunction("LogError", LogError);
		L.RegFunction("LogWarning", LogWarning);
		L.RegFunction("Int", Int);
		L.RegFunction("Float", Float);
		L.RegFunction("Long", Long);
		L.RegFunction("Random", Random);
		L.RegFunction("Uid", Uid);
		L.RegFunction("GetTime", GetTime);
		L.RegFunction("TimeCover", TimeCover);
		L.RegFunction("TimeToString_01", TimeToString_01);
		L.RegFunction("TimeToString_02", TimeToString_02);
		L.RegFunction("GetChildComponent", GetChildComponent);
		L.RegFunction("GetComponent", GetComponent);
		L.RegFunction("GetChildByCount", GetChildByCount);
		L.RegFunction("SetAsLastSibling", SetAsLastSibling);
		L.RegFunction("SetSiblingIndex", SetSiblingIndex);
		L.RegFunction("InstantiateGameObject", InstantiateGameObject);
		L.RegFunction("SetImage", SetImage);
		L.RegFunction("SetTexColor", SetTexColor);
		L.RegFunction("SetUrlTex", SetUrlTex);
		L.RegFunction("InstantiateUIObj", InstantiateUIObj);
		L.RegFunction("DestroyObject", DestroyObject);
		L.RegFunction("Child", Child);
		L.RegFunction("Peer", Peer);
		L.RegFunction("md5", md5);
		L.RegFunction("md5file", md5file);
		L.RegFunction("ClearChild", ClearChild);
		L.RegFunction("ClearMemory", ClearMemory);
		L.RegFunction("GetFileText", GetFileText);
		L.RegFunction("SetLocalEulerAngles", SetLocalEulerAngles);
		L.RegFunction("SetLocalPosition", SetLocalPosition);
		L.RegFunction("SetLocalScale", SetLocalScale);
		L.RegFunction("SetObjLayer", SetObjLayer);
		L.RegFunction("SetObjMesh", SetObjMesh);
		L.RegFunction("GetObjMesh", GetObjMesh);
		L.RegFunction("LoadScene", LoadScene);
		L.RegFunction("LoadSceneAsync", LoadSceneAsync);
		L.RegFunction("LoadYourAsyncScene", LoadYourAsyncScene);
		L.RegFunction("StringMagex", StringMagex);
		L.RegFunction("ReplaceNewLineSymbol", ReplaceNewLineSymbol);
		L.RegFunction("AddDropdownoption", AddDropdownoption);
		L.RegFunction("New", _CreateLuaFrameworkCore_Util);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("LoadUIObjFuc", get_LoadUIObjFuc, set_LoadUIObjFuc);
		L.RegVar("NetAvailable", get_NetAvailable, null);
		L.RegVar("IsWifi", get_IsWifi, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLuaFrameworkCore_Util(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				LuaFrameworkCore.Util obj = new LuaFrameworkCore.Util();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: LuaFrameworkCore.Util.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Log(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			LuaFrameworkCore.Util.Log(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogError(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			LuaFrameworkCore.Util.LogError(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogWarning(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			LuaFrameworkCore.Util.LogWarning(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Int(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			object arg0 = ToLua.ToVarObject(L, 1);
			int o = LuaFrameworkCore.Util.Int(arg0);
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Float(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			object arg0 = ToLua.ToVarObject(L, 1);
			float o = LuaFrameworkCore.Util.Float(arg0);
			LuaDLL.lua_pushnumber(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Long(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			object arg0 = ToLua.ToVarObject(L, 1);
			long o = LuaFrameworkCore.Util.Long(arg0);
			LuaDLL.tolua_pushint64(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Random(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 1);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 2);
			float o = LuaFrameworkCore.Util.Random(arg0, arg1);
			LuaDLL.lua_pushnumber(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Uid(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			string o = LuaFrameworkCore.Util.Uid(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTime(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			long o = LuaFrameworkCore.Util.GetTime();
			LuaDLL.tolua_pushint64(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TimeCover(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			long arg0 = LuaDLL.tolua_checkint64(L, 1);
			System.DateTime o = LuaFrameworkCore.Util.TimeCover(arg0);
			ToLua.PushValue(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TimeToString_01(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			long arg0 = LuaDLL.tolua_checkint64(L, 1);
			string o = LuaFrameworkCore.Util.TimeToString_01(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TimeToString_02(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			long arg0 = LuaDLL.tolua_checkint64(L, 1);
			string o = LuaFrameworkCore.Util.TimeToString_02(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetChildComponent(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 3 && TypeChecker.CheckTypes<UnityEngine.Transform, string, int>(L, 1))
			{
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
				UnityEngine.Component o = LuaFrameworkCore.Util.GetChildComponent(arg0, arg1, arg2);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes<UnityEngine.GameObject, string, int>(L, 1))
			{
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
				UnityEngine.Component o = LuaFrameworkCore.Util.GetChildComponent(arg0, arg1, arg2);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFrameworkCore.Util.GetChildComponent");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetComponent(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<UnityEngine.Transform, int>(L, 1))
			{
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.ToObject(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				UnityEngine.Component o = LuaFrameworkCore.Util.GetComponent(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<UnityEngine.GameObject, int>(L, 1))
			{
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				UnityEngine.Component o = LuaFrameworkCore.Util.GetComponent(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFrameworkCore.Util.GetComponent");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetChildByCount(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.CheckObject<UnityEngine.Transform>(L, 1);
			int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
			UnityEngine.Transform[] o = LuaFrameworkCore.Util.GetChildByCount(arg0, arg1);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAsLastSibling(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1 && TypeChecker.CheckTypes<UnityEngine.Transform>(L, 1))
			{
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.ToObject(L, 1);
				LuaFrameworkCore.Util.SetAsLastSibling(arg0);
				return 0;
			}
			else if (count == 1 && TypeChecker.CheckTypes<UnityEngine.GameObject>(L, 1))
			{
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				LuaFrameworkCore.Util.SetAsLastSibling(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFrameworkCore.Util.SetAsLastSibling");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetSiblingIndex(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<UnityEngine.GameObject, int>(L, 1))
			{
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				LuaFrameworkCore.Util.SetSiblingIndex(arg0, arg1);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<UnityEngine.Transform, int>(L, 1))
			{
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.ToObject(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				LuaFrameworkCore.Util.SetSiblingIndex(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFrameworkCore.Util.SetSiblingIndex");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InstantiateGameObject(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
			UnityEngine.Transform arg1 = (UnityEngine.Transform)ToLua.CheckObject<UnityEngine.Transform>(L, 2);
			UnityEngine.GameObject o = LuaFrameworkCore.Util.InstantiateGameObject(arg0, arg1);
			ToLua.PushSealed(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetImage(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UnityEngine.UI.Image arg0 = (UnityEngine.UI.Image)ToLua.CheckObject<UnityEngine.UI.Image>(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				LuaFrameworkCore.Util.SetImage(arg0, arg1);
				return 0;
			}
			else if (count == 3)
			{
				UnityEngine.UI.Image arg0 = (UnityEngine.UI.Image)ToLua.CheckObject<UnityEngine.UI.Image>(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				bool arg2 = LuaDLL.luaL_checkboolean(L, 3);
				LuaFrameworkCore.Util.SetImage(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFrameworkCore.Util.SetImage");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTexColor(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<UnityEngine.GameObject, UnityEngine.Color>(L, 1))
			{
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				UnityEngine.Color arg1 = ToLua.ToColor(L, 2);
				LuaFrameworkCore.Util.SetTexColor(arg0, arg1);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<UnityEngine.UI.Text, UnityEngine.Color>(L, 1))
			{
				UnityEngine.UI.Text arg0 = (UnityEngine.UI.Text)ToLua.ToObject(L, 1);
				UnityEngine.Color arg1 = ToLua.ToColor(L, 2);
				LuaFrameworkCore.Util.SetTexColor(arg0, arg1);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<UnityEngine.UI.Text, UnityEngine.Color, UnityEngine.Color>(L, 1))
			{
				UnityEngine.UI.Text arg0 = (UnityEngine.UI.Text)ToLua.ToObject(L, 1);
				UnityEngine.Color arg1 = ToLua.ToColor(L, 2);
				UnityEngine.Color arg2 = ToLua.ToColor(L, 3);
				LuaFrameworkCore.Util.SetTexColor(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<UnityEngine.GameObject, UnityEngine.Color, UnityEngine.Color>(L, 1))
			{
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				UnityEngine.Color arg1 = ToLua.ToColor(L, 2);
				UnityEngine.Color arg2 = ToLua.ToColor(L, 3);
				LuaFrameworkCore.Util.SetTexColor(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFrameworkCore.Util.SetTexColor");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetUrlTex(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			UnityEngine.UI.Image arg0 = (UnityEngine.UI.Image)ToLua.CheckObject<UnityEngine.UI.Image>(L, 1);
			uint arg1 = (uint)LuaDLL.luaL_checknumber(L, 2);
			string arg2 = ToLua.CheckString(L, 3);
			int arg3 = (int)LuaDLL.luaL_checknumber(L, 4);
			LuaFrameworkCore.Util.SetUrlTex(arg0, arg1, arg2, arg3);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InstantiateUIObj(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			string arg0 = ToLua.CheckString(L, 1);
			UnityEngine.Transform arg1 = (UnityEngine.Transform)ToLua.CheckObject<UnityEngine.Transform>(L, 2);
			System.Action<UnityEngine.GameObject> arg2 = (System.Action<UnityEngine.GameObject>)ToLua.CheckDelegate<System.Action<UnityEngine.GameObject>>(L, 3);
			LuaFrameworkCore.Util.InstantiateUIObj(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyObject(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject<UnityEngine.Object>(L, 1);
			LuaFrameworkCore.Util.DestroyObject(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Child(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<UnityEngine.Transform, string>(L, 1))
			{
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				UnityEngine.GameObject o = LuaFrameworkCore.Util.Child(arg0, arg1);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<UnityEngine.GameObject, string>(L, 1))
			{
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				UnityEngine.GameObject o = LuaFrameworkCore.Util.Child(arg0, arg1);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFrameworkCore.Util.Child");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Peer(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<UnityEngine.Transform, string>(L, 1))
			{
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				UnityEngine.GameObject o = LuaFrameworkCore.Util.Peer(arg0, arg1);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<UnityEngine.GameObject, string>(L, 1))
			{
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				UnityEngine.GameObject o = LuaFrameworkCore.Util.Peer(arg0, arg1);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFrameworkCore.Util.Peer");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int md5(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			string o = LuaFrameworkCore.Util.md5(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int md5file(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			string o = LuaFrameworkCore.Util.md5file(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearChild(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.CheckObject<UnityEngine.Transform>(L, 1);
			LuaFrameworkCore.Util.ClearChild(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearMemory(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			LuaFrameworkCore.Util.ClearMemory();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFileText(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			string o = LuaFrameworkCore.Util.GetFileText(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLocalEulerAngles(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 4 && TypeChecker.CheckTypes<UnityEngine.Transform, float, float, float>(L, 1))
			{
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.ToObject(L, 1);
				float arg1 = (float)LuaDLL.lua_tonumber(L, 2);
				float arg2 = (float)LuaDLL.lua_tonumber(L, 3);
				float arg3 = (float)LuaDLL.lua_tonumber(L, 4);
				LuaFrameworkCore.Util.SetLocalEulerAngles(arg0, arg1, arg2, arg3);
				return 0;
			}
			else if (count == 4 && TypeChecker.CheckTypes<UnityEngine.GameObject, float, float, float>(L, 1))
			{
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				float arg1 = (float)LuaDLL.lua_tonumber(L, 2);
				float arg2 = (float)LuaDLL.lua_tonumber(L, 3);
				float arg3 = (float)LuaDLL.lua_tonumber(L, 4);
				LuaFrameworkCore.Util.SetLocalEulerAngles(arg0, arg1, arg2, arg3);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFrameworkCore.Util.SetLocalEulerAngles");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLocalPosition(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 4 && TypeChecker.CheckTypes<UnityEngine.Transform, float, float, float>(L, 1))
			{
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.ToObject(L, 1);
				float arg1 = (float)LuaDLL.lua_tonumber(L, 2);
				float arg2 = (float)LuaDLL.lua_tonumber(L, 3);
				float arg3 = (float)LuaDLL.lua_tonumber(L, 4);
				LuaFrameworkCore.Util.SetLocalPosition(arg0, arg1, arg2, arg3);
				return 0;
			}
			else if (count == 4 && TypeChecker.CheckTypes<UnityEngine.GameObject, float, float, float>(L, 1))
			{
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				float arg1 = (float)LuaDLL.lua_tonumber(L, 2);
				float arg2 = (float)LuaDLL.lua_tonumber(L, 3);
				float arg3 = (float)LuaDLL.lua_tonumber(L, 4);
				LuaFrameworkCore.Util.SetLocalPosition(arg0, arg1, arg2, arg3);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFrameworkCore.Util.SetLocalPosition");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLocalScale(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 4 && TypeChecker.CheckTypes<UnityEngine.Transform, float, float, float>(L, 1))
			{
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.ToObject(L, 1);
				float arg1 = (float)LuaDLL.lua_tonumber(L, 2);
				float arg2 = (float)LuaDLL.lua_tonumber(L, 3);
				float arg3 = (float)LuaDLL.lua_tonumber(L, 4);
				LuaFrameworkCore.Util.SetLocalScale(arg0, arg1, arg2, arg3);
				return 0;
			}
			else if (count == 4 && TypeChecker.CheckTypes<UnityEngine.GameObject, float, float, float>(L, 1))
			{
				UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				float arg1 = (float)LuaDLL.lua_tonumber(L, 2);
				float arg2 = (float)LuaDLL.lua_tonumber(L, 3);
				float arg3 = (float)LuaDLL.lua_tonumber(L, 4);
				LuaFrameworkCore.Util.SetLocalScale(arg0, arg1, arg2, arg3);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: LuaFrameworkCore.Util.SetLocalScale");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetObjLayer(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
			string arg1 = ToLua.CheckString(L, 2);
			LuaFrameworkCore.Util.SetObjLayer(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetObjMesh(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
			UnityEngine.Mesh arg1 = (UnityEngine.Mesh)ToLua.CheckObject(L, 2, typeof(UnityEngine.Mesh));
			LuaFrameworkCore.Util.SetObjMesh(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetObjMesh(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.CheckObject(L, 1, typeof(UnityEngine.GameObject));
			UnityEngine.Mesh o = LuaFrameworkCore.Util.GetObjMesh(arg0);
			ToLua.PushSealed(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadScene(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			LuaFrameworkCore.Util.LoadScene(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadSceneAsync(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			string arg0 = ToLua.CheckString(L, 1);
			System.Action arg1 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 2);
			LuaFrameworkCore.Util.LoadSceneAsync(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadYourAsyncScene(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.AsyncOperation arg0 = (UnityEngine.AsyncOperation)ToLua.CheckObject<UnityEngine.AsyncOperation>(L, 1);
			System.Action arg1 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 2);
			System.Collections.IEnumerator o = LuaFrameworkCore.Util.LoadYourAsyncScene(arg0, arg1);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StringMagex(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			bool o = LuaFrameworkCore.Util.StringMagex(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReplaceNewLineSymbol(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			string o = LuaFrameworkCore.Util.ReplaceNewLineSymbol(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddDropdownoption(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.UI.Dropdown arg0 = (UnityEngine.UI.Dropdown)ToLua.CheckObject<UnityEngine.UI.Dropdown>(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			LuaFrameworkCore.Util.AddDropdownoption(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LoadUIObjFuc(IntPtr L)
	{
		try
		{
			ToLua.Push(L, LuaFrameworkCore.Util.LoadUIObjFuc);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NetAvailable(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, LuaFrameworkCore.Util.NetAvailable);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsWifi(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, LuaFrameworkCore.Util.IsWifi);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_LoadUIObjFuc(IntPtr L)
	{
		try
		{
			System.Action<string,System.Action<UnityEngine.GameObject>> arg0 = (System.Action<string,System.Action<UnityEngine.GameObject>>)ToLua.CheckDelegate<System.Action<string,System.Action<UnityEngine.GameObject>>>(L, 2);
			LuaFrameworkCore.Util.LoadUIObjFuc = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

