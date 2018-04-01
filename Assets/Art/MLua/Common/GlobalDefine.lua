---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/3/30 17:21
---这里存放了全局表的定义

--状态名称
StateNames = {
    InitState = "State/InitState",
    LoginState = "State/LoginState",
}
--UI层级 bottom{静态，动态} middle{静态，动态} top{静态，动态} 实现动静分离
WindowLayer = {
    BottomStatic = "_BottomStatic#",
    BottomDyn = "_BottomDyn#",
    MiddleStatic = "_MiddleStatic#",
    MiddleDyn = "_MiddleDyn",
    TopStatic = "_TopStatic#",
    TopDyn = "_TopDyn#",
}
--UI类型 普通， 全屏（关闭时均按返回处理）
WindowType = {
    None = "None",
    FullType = "FullType",
}
--组件名
ComponentName = {
    Image = 1,
    UIRawImage = 2,
    Button = 3,
    Text = 4,
    Mask = 5,
    Transform = 6,
    Toggle = 7,
    ToggleGroup = 8,
    Slider = 9,
    Scrollbar = 10,
    DropDown = 11,
    ScrollRect = 12,
    Selectable = 13,
    InputField = 14,
    RectTransform = 15,
    LayoutElement = 16,
    Outline = 17,
    Camera = 18,
    Animation = 19,
    UIButton = 20,
    UIImage = 21,
}

