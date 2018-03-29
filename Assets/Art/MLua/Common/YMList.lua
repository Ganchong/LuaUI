---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/3/28 22:18
--- 有序列表

YMList = {}
YMList.__index = YMList

--初始化有序列表
function YMList:New(t)
    local o = {itemType = t}
    setmetatable(o, self)
    return o
end

--添加item
function YMList:Add(item)
    Log("insert item")
    if self:CheckType(item) then
        table.insert(self, item)
        Log("insert item")
    else
        LogError( string.format("type mismatch:you want to add a %s into a %s type list",tostring(type(item)),tostring(self.itemType)))
    end
end

--检查类型 userdata 自行考虑
function YMList:CheckType(item)
    if type(item) == self:ItemType()then
        return true
    end
    return false
end

--清除有序的列表值，itemType还在
function YMList:Clear()
    local count = self:Count()
    for i=count,1,-1 do
        table.remove(self)
    end
end

--是否包含至少一个item值
function YMList:Contains(item)
    local count = self:Count()
    Log(" YMList contains====== ")
    for i=1,count do
        if self[i] == item then
            Log(" YMList contains ")
            return true
        end
    end
    Log(" YMList not contains ")
    return false
end

--获取列表的有序长度
function YMList:Count()
    return table.getn(self)
end

--获取列表的真实长度
function YMList:RealCount()
    local len = 0
    for k, v in pairs(self) do
        len = len+1
    end
    return len
end

--通过自定义方法寻找合适item
function YMList:Find(predicate)
    if (predicate == nil or type(predicate) ~= 'function') then
        Log('predicate is invalid!')
        return
    end
    local count = self:Count()
    for i=1,count do
        if predicate(self[i]) then
            return self[i]
        end
    end
    return nil
end

--有序遍历
function YMList:ForEach(action)
    if (action == nil or type(action) ~= 'function') then
        Log('action is invalid!')
        return
    end
    local count = self:Count()
    for i=1,count do
        action(self[i])
    end
end

--获取第一个值为item的位置
function YMList:IndexOf(item)
    local count = self:Count()
    for i=1,count do
        if self[i] == item then
            return i
        end
    end
    return 0
end

--获取最后一个值为item的位置
function YMList:LastIndexOf(item)
    local count = self:Count()
    for i=count,1,-1 do
        if self[i] == item then
            return i
        end
    end
    return 0
end

--在index下标处插入item
function YMList:Insert(index, item)
    if self:CheckType(item) then
        table.insert(self, index, item)
    else
        LogError( string.format("type mismatch:you want to insert a %s into a %s type list",type(item),self.itemType))
    end

end

--获取当前列表类型
function YMList:ItemType()
    return self.itemType
end

--移除item
function YMList:Remove(item)
    local idx = self:LastIndexOf(item)
    if (idx > 0) then
        table.remove(self, idx)
        self:Remove(item)
    end
end

--移除指定位置
function YMList:RemoveAt(index)
    table.remove(self, index)
end

--排序
function YMList:Sort(comparison)
    if (comparison ~= nil and type(comparison) ~= 'function') then
        Log('comparison is invalid')
        return
    end
    if comparison == nil then
        table.sort(self)
    else
        table.sort(self, comparison)
    end
end

