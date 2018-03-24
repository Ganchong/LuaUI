---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/3/23 21:37
---注意：只要子类拥有相同的方法，那么将实现重载；
---     切记不要在子类父类同时拥有相同方法时，在父类中再通过self.的形式调用子类方法
---

-- UI基础类
UIBase = class("UIBase")

function UIBase:ctor()
    --开启Update方法，请不要在UIBase中声明Update方法
    if self.Update ~= nil then
        local handle = UpdateBeat:CreateListener(self.Update,self)
        UpdateBeat:AddListener(handle)
    end

    --开启LateUpdate方法，请不要在UIBase中声明LateUpdate方法
    if self.LateUpdate ~= nil then
        local handle = LateUpdateBeat:CreateListener(self.LateUpdate,self)
        LateUpdateBeat:AddListener(handle)
    end
end

function UIBase:StartFixedUpdate()
    --开启FixedUpdate方法，请不要在UIBase中声明FixedUpdate方法
    if self.FixedUpdate ~= nil then
        local handle = FixedUpdateBeat:CreateListener(self.FixedUpdate,self)
        FixedUpdateBeat:AddListener(handle)
    end
end

---#region UI控制

--初始化UI
function UIBase:InitUI(uiObj)
    self.UIObj = uiObj;
end

--打开UI
function UIBase:OnEnableUI(param)

end

--关闭UI
function UIBase:CloseUI(callback)

end

--UI打开动画
function UIBase:DoShowAnimation()

end

--UI关闭动画
function UIBase:DoCloseAnimation(callback)

end

--销毁UI
function UIBase:DestroyUI()

end

--设置UI显示情况
function UIBase:SetVisible(show)
    self.UIObj:SetActive(show);
end

--获取UI类型，默认普通，如有需要在子类实现相同方法
function UIBase:GetUIType()
    return UIType.None
end

--窗口是否是静态窗口，静态窗口在关闭时会缓存，否则直接销毁，如有需要在子类实现相同方法
function UIBase:IsStatic()
    return true
end
--窗口层级，默认为MiddleStatic#层，如有需要在子类实现相同方法
function UIBase:GetUILayer()
    return UILayer.MiddleStatic
end
---#endRegion UI控制


---#region 逻辑流程控制
function UIBase:OnInit()
end

function UIBase:OnShow()
end

function UIBase:OnClose()
end

function UIBase:OnDestroy()
end
---#endRegion 逻辑流程控制


---#region UI事件

---#endRegion UI事件