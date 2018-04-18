---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/4/18 15:13
--- 提示条
local Time = Time

TipWindow = class("TipWindow", WindowBase)

local this = TipWindow
this.activeTime = -1

function this:OnWDAwake(uiObj)
    self:BindWindow(uiObj)
end

function this:BindWindow(uiObj)
    self.content = LuaUtil.GetChildComponent(uiObj, "root/content", ComponentName.UIText)
end

function this:OnWDStart(param)
    self:StartUpdate()
    self.activeTime = Time.GetTimestamp()
    self:ShowContent(param[1])
end

function this:Update()
    if self.activeTime < 0 then
        return
    end
    if Time.GetTimestamp() - self.activeTime > 1 then
        self.activeTime = -1
        self:FinishWindow()
    end
end

function this:ShowContent(param)
    local key = tostring(param)
    local index, _ = string.find(key, '@')
    if index == 1 then
        key, _ = string.gsub(key, '@', '', 1)
        self.content.text = Language[key]
    else
        self.content.text = key
    end
end

function this:IsStatic()
    return false
end

return this