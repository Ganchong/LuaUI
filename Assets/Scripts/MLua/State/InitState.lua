---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/3/22 14:47
---
require("State/BaseState")

local InitSate = class("InitSate",BaseState)
local this = InitSate

function this:Enter()
    self:InitManagers()

end

--初始化各类管理器
function this.InitManagers()

end

function this:Update()

end

function this:Exit()

end

return this