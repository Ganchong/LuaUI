﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UIRawImageWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UIRawImage), typeof(UnityEngine.UI.RawImage));
		L.RegFunction("LoadImage", LoadImage);
		L.RegFunction("SetGray", SetGray);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("LoadTextureFunc", get_LoadTextureFunc, set_LoadTextureFunc);
		L.RegVar("nowPath", get_nowPath, set_nowPath);
		L.RegVar("alphaTex", get_alphaTex, set_alphaTex);
		L.RegVar("IsGray", get_IsGray, null);
		L.RegVar("Alpha", get_Alpha, set_Alpha);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadImage(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				UIRawImage obj = (UIRawImage)ToLua.CheckObject<UIRawImage>(L, 1);
				obj.LoadImage();
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<string>(L, 2))
			{
				UIRawImage obj = (UIRawImage)ToLua.CheckObject<UIRawImage>(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				obj.LoadImage(arg0);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<LuaFrameworkCore.CallBack<UIRawImage>>(L, 2))
			{
				UIRawImage obj = (UIRawImage)ToLua.CheckObject<UIRawImage>(L, 1);
				LuaFrameworkCore.CallBack<UIRawImage> arg0 = (LuaFrameworkCore.CallBack<UIRawImage>)ToLua.ToObject(L, 2);
				obj.LoadImage(arg0);
				return 0;
			}
			else if (count == 3)
			{
				UIRawImage obj = (UIRawImage)ToLua.CheckObject<UIRawImage>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				LuaFrameworkCore.CallBack<UIRawImage> arg1 = (LuaFrameworkCore.CallBack<UIRawImage>)ToLua.CheckDelegate<LuaFrameworkCore.CallBack<UIRawImage>>(L, 3);
				obj.LoadImage(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UIRawImage.LoadImage");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetGray(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UIRawImage obj = (UIRawImage)ToLua.CheckObject<UIRawImage>(L, 1);
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.SetGray(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_LoadTextureFunc(IntPtr L)
	{
		try
		{
			ToLua.Push(L, UIRawImage.LoadTextureFunc);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_nowPath(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIRawImage obj = (UIRawImage)o;
			string ret = obj.nowPath;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index nowPath on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_alphaTex(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIRawImage obj = (UIRawImage)o;
			UnityEngine.Texture ret = obj.alphaTex;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index alphaTex on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsGray(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIRawImage obj = (UIRawImage)o;
			bool ret = obj.IsGray;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index IsGray on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Alpha(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIRawImage obj = (UIRawImage)o;
			float ret = obj.Alpha;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Alpha on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_LoadTextureFunc(IntPtr L)
	{
		try
		{
			System.Action<string,UIRawImage,LuaFrameworkCore.CallBack<UIRawImage>> arg0 = (System.Action<string,UIRawImage,LuaFrameworkCore.CallBack<UIRawImage>>)ToLua.CheckDelegate<System.Action<string,UIRawImage,LuaFrameworkCore.CallBack<UIRawImage>>>(L, 2);
			UIRawImage.LoadTextureFunc = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_nowPath(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIRawImage obj = (UIRawImage)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.nowPath = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index nowPath on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_alphaTex(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIRawImage obj = (UIRawImage)o;
			UnityEngine.Texture arg0 = (UnityEngine.Texture)ToLua.CheckObject<UnityEngine.Texture>(L, 2);
			obj.alphaTex = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index alphaTex on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Alpha(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIRawImage obj = (UIRawImage)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.Alpha = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Alpha on a nil value");
		}
	}
}
