---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/3/21 17:27
---
UIManager = class("UIManager")
local this = UIManager

function this:ctor()
    --require 的class 缓存
    self._uiClassMap = {}
    --打开过的UI名称列表，顺序数组
    self._openingNameList = {}
    --打开过的UI（绑定了UIObj的Class）
    self._openedUIMap = {}
    --加载过得预制
    self._uiObjMap = {}
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
    self._canvasLayerTrans[UILayer.BottomStatic] = root:Find("_Bottom#/_BottomStatic#")
    self._canvasLayerTrans[UILayer.BottomDyn] = root:Find("_Bottom#/_BottomDyn#")
    self._canvasLayerTrans[UILayer.MiddleStatic] = root:Find("_Middle#/_MiddleStatic#")
    self._canvasLayerTrans[UILayer.MiddleDyn] = root:Find("_Middle#/_MiddleDyn#")
    self._canvasLayerTrans[UILayer.TopStatic] = root:Find("_Top#/_TopStatic#")
    self._canvasLayerTrans[UILayer.TopDyn] = root:Find("_Top#/_TopStatic#")
end

--打开界面
function this:OpenWindow(name,param)
    local opening,index = self:IsOpening(name)
    --打开过
    if opening then
        local uiBase = self._openedUIMap[name]
        Util.SetAsLastSibling(uiBase.UIObj)
        table.remove(self._openingNameList,index)
        self:OnEnableUI(uiBase,param)
        --因为当前界面是全屏，隐藏所有其他UI
        if uiBase:GetUIType() == UIType.FullType then
            self:HideAllUI()
        end
        return
    end
    --未曾打开过
    local uiBase = self:GetUIClass(name).new()
    uiBase.name = name
    local layer = uiBase:GetUILayer()
    self:GetUIObj(name,function (uiObj)
        uiObj.transform.parent = self._canvasLayerTrans[layer]
        uiObj.transform.localPosition = Vector3.zero
        uiObj.transform.localScale = Vector3.one
        uiBase:InitUI(uiObj)
        self:OnEnableUI(uiBase,param)
        self._openedUIMap[name] = uiBase
        --因为当前界面是全屏，隐藏所有其他UI
        if uiBase:GetUIType() == UIType.FullType then
            self:HideAllUI()
        end
    end)
end

--关闭窗口
function this:CloseWindow(name)
    local uiBase = self._openedUIMap[name]
    if uiBase == nil then
        LogError("window you want to close is not exist,name is"..name)
        return
    end
    uiBase:SetVisible(false)
    local opening,index = self:IsOpening(uiBase.name)
    if opening then
        table.remove(self._openingNameList,index)
    end
    if uiBase:IsStatic() then
        self:CloseUI(uiBase)
    else
        self:DestroyUI(uiBase)
    end
end


--关闭UI
function this:CloseUI(uiBase)
    uiBase:CloseUI(function ()
        self:RestAllUI()
    end )
end

--销毁UI
function this:DestroyUI(uiBase)
    uiBase:CloseUI(function ()
        self:RestAllUI()
        --启动销毁程序
        uiBase:DestroyUI()
        Util.DestroyObject(self._openedUIMap[uiBase.name].UIObj)
        --这里的obj已经销毁，_uiObjMap[name]虽然此时不为nil，但是数据已经不存在了
        --也就是说不置为空,_uiObjMap[name]永远不为空，那么下次用的话会报错
        --比如说下一帧调用_uiObjMap[name].transform的时候C#端将会抛出异常，说他是nil
        --if self._uiObjMap[name].transform.parent==nil then
        --    Log("=======================")
        --end
        self._uiObjMap[uiBase.name] = nil
        self._openedUIMap[uiBase.name].UIObj = nil
        self._openedUIMap[uiBase.name] = nil
    end)
end

--重新调整UI
function this:RestAllUI()
    for i = #self._openingNameList, 1,-1 do
        local tmpName = self._openingNameList[i]
        local tmpUIBase = self._openedUIMap[tmpName]
        if tmpUIBase:GetUIType() == UIType.FullType then
            for j = 1, #self._openingNameList-i+1 do
                tmpName = self._openingNameList[i]
                tmpUIBase = self._openedUIMap[tmpName]
                local opening,index = self:IsOpening(tmpName)
                if opening then
                    table.remove(self._openingNameList,index)
                end
                self:OnEnableUI(tmpUIBase,"")
            end
            return
        end
    end
end


--获取UIObj
function this:GetUIObj(name,callback)
    local uiObj = self._uiObjMap[name]
    if uiObj~=nil then
        callback(uiObj)
        Log("getUIObj from cache")
    else
        self:LoadUIObj(name,callback)
        Log("getUIObj from new load")
    end
end
--加载UIObj
function this:LoadUIObj(name,callback)
    Util.InstantiateUIObj(name,function (uiObj)
        self._uiObjMap[name] = uiObj;
        callback(uiObj);
    end)
end

--获取UIClass
function this:GetUIClass(name)
    local uiClass = self._uiClassMap[name]
    if uiClass == nil then
        uiClass = require("UI/"..name)
        self._uiClassMap[name] = uiClass
        Log("get ui class from require")
    end
    return uiClass
end

--隐藏所有UI（除了当前显示的）
function this:HideAllUI()
    for i = 1, #self._openingNameList-1 do
        local uiBase = self._openedUIMap[self._openingNameList[i]]
        uiBase:SetVisible(false)
    end
end

--重新显示UI
function this:OnEnableUI(uiBase,param)
    uiBase:SetVisible(true)
    uiBase:OnEnableUI(param)
    Log("insert name is "..uiBase.name)
    table.insert(self._openingNameList,uiBase.name)
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
