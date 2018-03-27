---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/3/21 17:27
---
UIManager = class("UIManager")
local this = UIManager

function this:ctor()
    --require 的class 缓存
    self._windowClassMap = {}
    --打开过的UI名称列表，顺序数组
    self._openingNameList = {}
    --打开过的UI（绑定了UIObj的Class）
    self._openedWindowMap = {}
    --加载过得预制
    self._windowObjMap = {}
    --界面层级
    self._canvasLayerTrans = {}
end
--初始化
function this:Init()
    self:InitUIRoot()
end

--初始化UIRoot层级
function this:InitUIRoot()
    local root = GameObject.Find("_CoreRoot#").transform
    self._canvasLayerTrans[WindowLayer.BottomStatic] = root:Find("_Bottom#/_BottomStatic#")
    self._canvasLayerTrans[WindowLayer.BottomDyn] = root:Find("_Bottom#/_BottomDyn#")
    self._canvasLayerTrans[WindowLayer.MiddleStatic] = root:Find("_Middle#/_MiddleStatic#")
    self._canvasLayerTrans[WindowLayer.MiddleDyn] = root:Find("_Middle#/_MiddleDyn#")
    self._canvasLayerTrans[WindowLayer.TopStatic] = root:Find("_Top#/_TopStatic#")
    self._canvasLayerTrans[WindowLayer.TopDyn] = root:Find("_Top#/_TopStatic#")
end

--打开界面
---@name 窗口名
---@param 窗口需要的数据
function this:OpenWindow(name,param)
    local opening,index = self:IsOpening(name)
    --打开过
    if opening then
        local window = self._openedWindowMap[name]
        Util.SetAsLastSibling(window.UIObj)
        table.remove(self._openingNameList,index)
        --因为当前界面是全屏，隐藏所有其他UI
        if window:GetUIType() == WindowType.FullType then
            self:HideAllWindow()
        end
        self:OnEnableWindow(window,param)
        return
    end
    --未曾打开过
    local window = self:GetWindowClass(name).new()
    window.name = name
    local layer = window:GetUILayer()
    self:GetUIObj(name,function (uiObj)
        --因为当前界面是全屏，隐藏所有其他UI
        if window:GetUIType() == WindowType.FullType then
            self:HideAllWindow()
        end
        uiObj.transform.parent = self._canvasLayerTrans[layer]
        uiObj.transform.localPosition = Vector3.zero
        uiObj.transform.localScale = Vector3.one
        window:InitUI(uiObj)
        self:OnEnableWindow(window,param)
        self._openedWindowMap[name] = window
    end)
end

--关闭窗口
function this:CloseWindow(name)
    local window = self._openedWindowMap[name]
    if window == nil then
        LogError("window you want to close is not exist,name is"..name)
        return
    end
    window:SetVisible(false)
    local opening,index = self:IsOpening(window.name)
    if opening then
        table.remove(self._openingNameList,index)
    end
    if window:IsStatic() then
        window:CloseUI(function ()
            window:OnDisableUI()
            self:RestAllWindow(window)
        end )
    else
        self:DestroyWindow(window)
    end
end

--销毁窗口
function this:DestroyWindow(window)
    window:CloseUI(function ()
        window:OnDisableUI()
        window:DestroyUI()
        self:RestAllWindow(window)
        --启动销毁程序
        Util.DestroyObject(self._openedWindowMap[window.name].UIObj)
        --这里的obj已经销毁，_windowObjMap[name]虽然此时不为nil，但是数据已经不存在了
        --也就是说不置为空,_windowObjMap[name]永远不为空，那么下次用的话会报错
        --比如说下一帧调用_windowObjMap[name].transform的时候C#端将会抛出异常，说他是nil
        --if self._windowObjMap[name].transform.parent==nil then
        --    Log("=======================")
        --end
        self._windowObjMap[window.name] = nil
        self._openedWindowMap[window.name].UIObj = nil
        self._openedWindowMap[window.name] = nil
    end)
end


--重新调整窗口
function this:RestAllWindow(window)
    for i = #self._openingNameList, 1,-1 do
        local tmpName = self._openingNameList[i]
        local tmpWindow = self._openedWindowMap[tmpName]
        if tmpWindow:GetUIType() == WindowType.FullType then
            for j = 1, #self._openingNameList-i+1 do
                tmpName = self._openingNameList[i]
                tmpWindow = self._openedWindowMap[tmpName]
                --如果当前关闭的界面是全屏才处理
                if window.GetUIType() == WindowType.FullType then
                    local opening,index = self:IsOpening(tmpName)
                    if opening then
                        table.remove(self._openingNameList,index)
                    end
                    self:OnEnableWindow(tmpWindow,"")
                end
                --聚焦当前窗口
                if j == #self._openingNameList-i+1 then
                    if tmpWindow.OnFocus~=nil then
                        tmpWindow:OnFocus()
                    end
                end
            end
            return
        end
    end
end


--获取UIObj
function this:GetUIObj(name,callback)
    local uiObj = self._windowObjMap[name]
    if uiObj~=nil then
        callback(uiObj)
    else
        self:LoadUIObj(name,callback)
    end
end
--加载UIObj
function this:LoadUIObj(name,callback)
    Util.InstantiateUIObj(name,function (uiObj)
        self._windowObjMap[name] = uiObj;
        callback(uiObj);
    end)
end

--获取windowClass
function this:GetWindowClass(name)
    local windowClass = self._windowClassMap[name]
    if windowClass == nil then
        windowClass = require("UI/"..name)
        self._windowClassMap[name] = windowClass
    end
    return windowClass
end

--隐藏所有UI（除了当前显示的）
function this:HideAllWindow()
    for i = #self._openingNameList, 1,-1 do
        local window = self._openedWindowMap[self._openingNameList[i]]
        window:SetVisible(false)
        window:OnDisableUI()
        if window:GetUIType() == WindowType.FullType then
            return
        end
    end
end

--重新显示UI
function this:OnEnableWindow(window, param)
    window:SetVisible(true)
    window:OnEnableUI(param)
    table.insert(self._openingNameList,window.name)
end

--界面是处于打开状态（包括隐藏的和显示着的）
function this:IsOpening(name)
    for i = 1, #self._openingNameList do
        if self._openingNameList[i] == name then
            return true,i
        end
    end
    return false,-1
end

return this
