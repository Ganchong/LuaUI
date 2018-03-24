---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/3/24 19:06
---
LoginWindow = class("LoginWindow",UIBase)

local this = LoginWindow
this.num = 0

function this:InitUI(uiObj)
    self.UIObj = uiObj
    self:BindWindow(uiObj)
end

function this:BindWindow(uiObj)
    Log("bind loingwindow")
    self.TimeLabel = Util.GetChildComponent(uiObj,"time",4)
    self.TimeLabel.text = Util.TimeToString_02(Time.GetTimestamp())
    Log(type(self.TimeLabel))
    self:StartFixedUpdate()

end

function this:OnEnableUI(param)
end

function this:Update1()
    Log(Time.GetTimestamp())
end

function this:FixedUpdate()
    self.TimeLabel.text = Util.TimeToString_02(Time.GetTimestamp())
end

function this:GetUILayer()
    return UILayer.MiddleStatic
end
return this

