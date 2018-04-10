/// <summary>
/// 游戏事件
/// </summary>

public enum GameEvent
{
	/** 资源更新 */
	ResourceUpdate =1,
	/** 退出 */
	Exit,
	/** 读取进度 */
	EnterLoading,
	/** 登录 */
	Login,
	/** 断线重连 */
	ReLogin,
	/** 退出登录 */
	LogOut,
	MaxValue
}