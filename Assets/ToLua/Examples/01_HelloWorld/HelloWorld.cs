using UnityEngine;
using LuaInterface;
using System;

public class HelloWorld : MonoBehaviour
{
    void Awake()
    {
        LuaState lua = new LuaState();
        lua.Start();
        string hello =
            @"   
				UINames = {TestUI = 'TestUI',LoadingUI = 'LoadingUI',LoginUI = 'loginUI' }
				name = UINames.LoginUI
             
                print('hello tolua#')

				mtable = {'loginUI','mainUI'}

				for i = 1,#mtable do 
					if mtable[i] == name then print('equals index '..i) end
				end	
                                  
            ";
        
        lua.DoString(hello, "HelloWorld.cs");
        lua.CheckTop();
        lua.Dispose();
        lua = null;
    }
}
