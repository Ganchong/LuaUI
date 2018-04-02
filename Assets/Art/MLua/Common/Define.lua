---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/3/22 16:10
---
--引入全局表定义
require("Common/GlobalDefine")
--引入lua Class
require("Common/DefinitionClass")
--引入UI管理器
require("Manager/UIManager")
--引入lua APP
require("LuaApp")
--引入UIBase
require("Base/UIBase")
--引入窗口基类
require("Base/WindowBase")
--引入有序列表
require("Common/YMList")
--引入字典
require("Common/YMDictionary")
--引入定时器
require("Common/Timer")
--引入定时器管理器
require("Manager/TimerManager")
--引入语言表
require("Config/Language")
--引入Lua工具
require("Common/LuaUtil")


GameObject = UnityEngine.GameObject
WWW = UnityEngine.WWW
Util = LuaFrameworkCore.Util


---#全局方法定义#--
--日志打印
Log = function(m)
    local traceBack = string.split(debug.traceback("",2),"\n")
    local logStr = tostring(m).."\n"
    for i = 3, #traceBack do
        logStr = logStr..traceBack[i].."\n"
    end
    Util.Log(logStr)
end
--警告日志打印
LogWarn = function(w)
    local traceback = string.split(debug.traceback("", 2), "\n")
    Util.LogWarning(traceback[3].."\n"..tostring(w))
end
--错误日志打印
LogError = function(e)
    local traceback = string.split(debug.traceback("", 2), "\n")
    local logStr = tostring(e).."\n";
    for i = 3, #traceback do
        logStr = logStr..traceback[i].."\n";
    end
    Util.LogError(logStr);
end