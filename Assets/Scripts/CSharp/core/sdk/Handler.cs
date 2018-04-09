using System;
/// <summary>
/// Handler.SDK处理端口
/// 汪松民
/// </summary>
public class Handler
{
	/* 标准端口常量定义 */
	/** 回显 */
	public const int ECHO=1;
	/** APP信息 */
	public const int APPINFO=2;
	/** 状态统计 */
	public const int STATE=4;
	/** 平台ID */
	public const int PLANTID=5;
	
	/* 功能流程端口定义 */
	/** 连接服务器 */
	public const int CONNECT=11;
	/** 认证 */
	public const int CERTIFY=12;
	/** 获取物品  */
	public const int GOODLIST=13;
	/** 获取服务器列表 */
	public const int SERVERLIST=14;
	/** UID */
	public const int UID=15;
	/** UUID */
	public const int UUID=16;
	/** 是否能更新 */
	public const int UPDATE=17;
	/** 资源版本 */
	public const int RESOURCEVERSION=18;
	/** 更新地址 */
	public const int UPDATEURL=19;
	/** 平台ID */
	public const int GAMEPLANTID=20;
	/** 最近 */
	public const int RECENT=21;
	/** 公告 */
	public const int AFFICHE=22;
	/** 角色信息统计 */
	public const int ROLEINFO=23;
	/** 下载地址 */
	public const int DOWNURL=24;

	
	/* 自定义端口定义 */
	/** 初始化 */
	public const int INIT=51;
	/** 登录 */
	public const int LOGIN=52;
	/** 注销 */
	public const int LOGOUT=53;
	/** 支付 */
	public const int PAY=54;
	/** 退出 */
	public const int EXIT=55;
	/** 是否登录 */
	public const int ISLOGIN=56;
	/** 分享 */
	public const int SHARE=57;
}

