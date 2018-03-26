---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 大杰哥.
--- DateTime: 2018/3/26 17:27
---
Alert = class("Alert",UIBase)
local this = Alert

function this:InitUI(uiObj)
    self.UIObj = uiObj
    self:BindWindow(uiObj)
    self:AddButtonEvent()
end

function this:BindWindow(uiObj)
    self.SureButton = Util.GetChildComponent(uiObj,"root/SureButton",3)
    self.CancelButton = Util.GetChildComponent(uiObj,"root/CancelButton",3)
end

function this:AddButtonEvent()
    self.SureButton.onClick:AddListener(function() self:SureEvent() end)
    self.CancelButton.onClick:AddListener(function() self:CancelEvent() end)
end

function this:SureEvent()
    LuaAPP.GetUIManager():OpenWindow("CreateRoleWindow","")
end

function this:CancelEvent()
    LuaAPP.GetUIManager():CloseWindow("Alert")
end

function this:IsStatic()
    return false
end

return this