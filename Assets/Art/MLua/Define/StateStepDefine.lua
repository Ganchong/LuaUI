---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by 干冲.
--- DateTime: 2018/4/9 20:44
--- 状态步骤

StateStep = {
    --/** 初始化SDK */
    STEP1 = 1,
    -- /** 初始化完成 */
    STEP2 = 2,
    --/** LoadResourceFinish */
    STEP3 = 3,
    -- /** Load1 */
    STEP4 = 4,
    --/** Load2 */
    STEP5 = 5,
    --/** 第一个界面 */
    STEP6 = 6,
    --/** 开始连接SDK服务器 */
    STEP7 = 7,
    --/** 连接SDK服务器成功 */
    STEP8 = 8,
    --/** 连接失败 */
    STEP9 = 9,
    --/** 开始检查更新 */
    STEP10 = 10,
    -- /** 有更新 */
    STEP11 = 11,
    --/** 提示下载 */
    STEP12 = 12,
    --/** 更新失败 */
    STEP13 = 13,
    --/** 更新成功 */
    STEP14 = 14,
    --/** 加载资源 */
    STEP15 = 15,
    --/** 加载完成 */
    STEP16 = 16,
    --/** 切换场景 */
    STEP17 = 17,
    --/** 打开登录窗口完成 */
    STEP18 = 18,
    --/** 打开公告 */
    STEP19 = 19,
    --/** 开始登录 */
    STEP20 = 20,
    -- /** 登录验证 */
    STEP21 = 21,
    --/** 验证成功 */
    STEP22 = 22,
    --/** 验证失败 */
    STEP23 = 23,
    --/** 登录成功 */
    STEP24 = 24,
    --/** 登录失败 */
    STEP25 = 25,
    -- /** 登录取消 */
    STEP26 = 26,
    --/** 开始认证 */
    STEP27 = 27,
    --/** 认证成功 */
    STEP28 = 28,
    --/** 认证失败 */
    STEP29 = 29,
    -- /** 获取PID */
    STEP30 = 30,
    --/** 播放剧情文字 */
    STEP31 = 31,
    --/** 进入创建角色 */
    STEP32 = 32,
    -- /** 输入名称 */
    STEP33 = 33,
    --/** 开始创建 */
    STEP34 = 34,
    --/** 开始连接服务器 */
    STEP35 = 35,
    --/** 连接成功 */
    STEP36 = 36,
    --/** 连接失败 */
    STEP37 = 37,
    --/** 开始登录 */
    STEP38 = 38,
    --/** 开始加载 */
    STEP39 = 39,
    --/** 开始二次加载 */
    STEP40 = 40,
    --/** 进入新手引导 */
    STEP41 = 41,
    --/** 开始引导 */
    STEP42 = 42,
    --/** 回到主界面 */
    STEP43 = 43,
}

UpdateStep = {
    None = 1,				--//未开始
    CheckVersion = 2,		--//检查版本
    GetUpdateList = 3,		--//获取文件列表
    CompareData = 4,		--//比较列表
    MakeSureDownload = 5,	--//询问是否下载
    DownloadData = 6,		--//下载资源
    CheckData = 7,			--//校验资源
    CopyData = 8,			--//复制资源到加载目录
    CleanCache = 9,			--//清空临时目录
    Finish = 10,			--//完成
}

UpdateResult = {
    None = 1,						--//未开始
    Success = 2,					--//成功
    GetVersionFail = 3,				--//获取版本信息失败
    GetUpdateListFail = 4 ,			--//获取需更新列表失败
    CheckDataFail = 5,				--//检查资源失败
    DownloadFail = 6,				--//下载失败
    CopyDataFail = 7,				--//复制文件失败
    SaveVersionFileFail = 8,		--//保存版本信息
    SaveFileListFail = 9,			--//保存文件信息
    CleanCacheFail = 10,			--//清理缓存
    LoadServerFailListError = 11,	--//读取服务器文件信息
}