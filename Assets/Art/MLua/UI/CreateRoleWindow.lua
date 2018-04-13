---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/3/24 22:25
---

CreateRoleWindow = class("CreateRoleWindow", WindowBase)
local this = CreateRoleWindow

function this:InitUI(uiObj)
    Log("CreateRoleWindow InitUI")
    self.UIObj = uiObj
    self:BindWindow(uiObj)
    self:AddButtonEvent()
end

function this:BindWindow(uiObj)
    --self.headStyle = Util.GetChildComponent(root,"headStyle",1)
    self.ReturnButton = Util.GetChildComponent(uiObj,"root/ReturnButton",ComponentName.Button)
end

function this:OnEnableUI()
    LuaAPP.GetBackGroundManager():Change("createRoleBack")
end

function this:AddButtonEvent()
    self.ReturnButton.onClick:AddListener(function ()self:ReturnEvent() end)
end
function this:ReturnEvent()
    self:FinishWindow()
end
function this:GetUIType()
    return WindowType.FullType
end

return this