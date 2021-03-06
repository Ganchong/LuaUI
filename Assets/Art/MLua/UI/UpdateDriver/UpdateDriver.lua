---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/4/9 10:37
--- 更新驱动

UpdateDriver = class("UpdateDriver", WindowBase)

local this = UpdateDriver

function this:OnWDAwake(uiObj)
    self:BindWindow(uiObj)
    self:AddButtonEvent()
end

--绑定信息
function this:BindWindow(uiObj)
    self.tips = LuaUtil.GetChildComponent(uiObj, "root/tips", ComponentName.UIText)
    self.slider = LuaUtil.GetChildComponent(uiObj, "root/Slider", ComponentName.Slider)
    self.version = LuaUtil.GetChildComponent(uiObj, "root/version", ComponentName.UIText)
end

--添加时间信息
function this:AddButtonEvent()
    LuaAPP.GetGlobalEvent():AddEvent(EventName.UpdateDriverSetTips,
            function(t)
                self:SetValue(t[1], t[2])
            end)
    LuaAPP.GetGlobalEvent():AddEvent(EventName.UpdateDriverSetDownValue,
            function(t)
                self:SetTips(string.format(Language.TIP_5, t[1], t[2]))
            end)
end
--逻辑开始
function this:OnWDStart()
    self:StartUpdate()
    self.process = 0
    self.slider.value = 0
    self.tips.text = Language.TIP_0
    self.version.text = string.format(Language.Version, SDKHelper.getVersion())
    LuaAPP.GetBackGroundManager():Change("loginBack_3")
end

function this:Update()
    if self.process == nil or self.process < 0 then
        return
    end
    if self.slider.value < self.process then
        self.slider.value = LuaUtil.Lerp(self.slider.value, self.process, 0.08)
    end
    if self.slider.value > 0.99 then
        self.tips.text = Language.TIP_8
        LuaAPP.GetGlobalEvent():DispatchEvent(EventName.UpdateDriverFinish)
        self.process = -1
    end
end

--设置提示信息
function this:SetTips(tips)
    self.tips.text = tips
end

--设置提示信息和进度
function this:SetValue(tips, process)
    if tips ~= nil and tips ~= "" then
        self.tips.text = Language.Get(tips)
    end
    if self.process < process then
        self.process = process
    end
end

function this:GetUIType()
    return WindowType.FullType
end

function this:IsStatic()
    return false
end

return this