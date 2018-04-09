using System;

/// <summary>
/// 状态步骤
/// </summary>
public enum StateStep : byte
{
	/** 初始化SDK */
	STEP1=1,
	/** 初始化完成 */
	STEP2,
	/** LoadResourceFinish */
	STEP3,
	/** Load1 */
	STEP4,
	/** Load2 */
	STEP5,
	/** 第一个界面 */
	STEP6,
	/** 开始连接SDK服务器 */
	STEP7,
	/** 连接SDK服务器成功 */
	STEP8,
	/** 连接失败 */
	STEP9,
	/** 开始检查更新 */
	STEP10,
	/** 有更新 */
	STEP11,
	/** 提示下载 */
	STEP12,
	/** 更新失败 */
	STEP13,
	/** 更新成功 */
	STEP14,
	/** 加载资源 */
	STEP15,
	/** 加载完成 */
	STEP16,
	/** 切换场景 */
	STEP17,
	/** 打开登录窗口完成 */
	STEP18,
	/** 打开公告 */
	STEP19,
	/** 开始登录 */
	STEP20,
	/** 登录验证 */
	STEP21,
	/** 验证成功 */
	STEP22,
	/** 验证失败 */
	STEP23,
	/** 登录成功 */
	STEP24,
	/** 登录失败 */
	STEP25,
	/** 登录取消 */
	STEP26,
	/** 开始认证 */
	STEP27,
	/** 认证成功 */
	STEP28,
	/** 认证失败 */
	STEP29,
	/** 获取PID */
	STEP30,
	/** 播放剧情文字 */
	STEP31,
	/** 进入创建角色 */
	STEP32,
	/** 输入名称 */
	STEP33,
	/** 开始创建 */
	STEP34,
	/** 开始连接服务器 */
	STEP35,
	/** 连接成功 */
	STEP36,
	/** 连接失败 */
	STEP37,
	/** 开始登录 */
	STEP38,
	/** 开始加载 */
	STEP39,
	/** 开始二次加载 */
	STEP40,
	/** 进入新手引导 */
	STEP41,
	/** 开始引导 */
	STEP42,
	/** 回到主界面 */
	STEP43
}