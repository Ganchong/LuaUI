---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/4/3 14:38
--- 背景管理器
BackGroundManager = class("BackGroundManager")

function BackGroundManager:Init()
    local root = GameObject.Find("_CoreUISystem#").transform
    self.BackGround = LuaUtil.GetChildComponent(root,WindowLayer.CoreUIBG,ComponentName.UIRawImage)
   -- self.BackGround:LoadImage("loginBack")
end

function BackGroundManager:Change(backName)
    local path = "backGround/"
    if backName==nil then
        path = nil
    else
        path = path..backName
    end
    self.BackGround:LoadImage(path)
end

function BackGroundManager:Disable()
    self.BackGround.Alpha = 0
end

function BackGroundManager:Enable()
    self.BackGround.Alpha = 1
end