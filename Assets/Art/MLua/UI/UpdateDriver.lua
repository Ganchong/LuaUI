---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/4/9 10:37
--- 更新驱动

UpdateDriver = class("UpdateDriver",WindowBase)

local this = UpdateDriver

function this:InitUI(uiObj)
    self.UIObj = uiObj
    self:BindWindow(uiObj)
    self:AddButtonEvent()
end

function this:BindWindow(uiObj)
    self.tips = LuaUtil.GetChildComponent(uiObj,"root/tips",ComponentName.UIText)
    self.slider = LuaUtil.GetChildComponent(uiObj,"root/Slider",ComponentName.slider)
    self.version = LuaUtil.GetChildComponent(uiObj,"root/version",ComponentName.UIText)
end

function this:AddButtonEvent()
    LuaAPP.GetGlobalEvent():AddEvent(EventName.UpdateDriverSetValue,
            function(value,curLoadSize,totalLoadSize)
                self:SetValue(value,curLoadSize,totalLoadSize)
            end)
end

function this:SetValue(value,curLoadSize,totalLoadSize)
    local tip = ""
    if value<=0 then
        tip = Language.UpdateDriver_03
    elseif value>0 and value<=0.1 then
        tip = Language.UpdateDriver_04
    elseif value>0.1 and value <=0.2 then
            tip = Language.UpdateDriver_05
    elseif value>0.1 and value <=0.2 then
        tip = Language.UpdateDriver_05
    elseif value>0.2 and value<0.8 then
        tip = Language.Get(Language.UpdateDriver_06,curLoadSize,totalLoadSize)
    elseif value>=0.8 and value<0.9 then
        tip = Language.UpdateDriver_07
    elseif value>=0.9 and value<1 then
        tip = Language.UpdateDriver_08
    elseif value>=1 then
        tip = Language.UpdateDriver_09
        value = 1
    end
    self.tips.text = tip
    self.slider.value = value
end

function this:OnEnableUI()
    self.slider.value = 0
    self.tips.text = Language.UpdateDriver_01
    self.version.text = Language.Get(Language.Version,"1.0.0")
    LuaAPP.GetBackGroundManager():Change("loginBack_3")
end

function this:GetUIType()
    return WindowType.FullType
end

return this