---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/3/21 17:24
---
require("Define/Define")

--Lua端主入口，从这里开始lua逻辑
function Main()
    LuaAPP.Init()
    LuaAPP.ChangeState(StateNames.LoginState)
end

function CheckVersionUpdate()
    LuaAPP.Init()
    LuaAPP.ChangeState(StateNames.CheckVersionState)
end

--切换场景通知
function OnLevelWasLoad(level)
    collectgarbage("collect")
    Time.timeSinceLevelLoad = 0
end