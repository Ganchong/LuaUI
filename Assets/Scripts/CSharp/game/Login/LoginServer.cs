using System;
/// <summary>
/// 登录服务器
/// 汪松民
/// </summary>
public class LoginServer
{
	/* fields */
	/** 区服ID */
	int id;
	/** 角色名称 */
	string roleName;
	/** 角色等级 */
	int roleLevel;
	/** 登录时间 */
	int loginTime;

	/* methods */
	/** 构造方法 */
	public LoginServer()
	{
	}
	public LoginServer(int id)
	{
		this.id = id;
	}
	/** 解析方法 */
	public void parseJson(IJsonNode node)
	{
		id = node ["gameServerId"].ToInt ();
		roleName = node ["roleName"].ToString ();
		roleLevel = node ["roleLevel"].ToInt ();
		loginTime = node ["loginTime"].ToInt ();
	}
	/** 获取角色名称 */
	public string getRoleName()
	{
		return roleName;
	}
	/** 设置角色名称 */
	public void setRoleName(string roleName)
	{
		this.roleName = roleName;
	}
	/** 获取角色等级 */
	public int getRoleLevel()
	{
		return roleLevel;
	}
	/** 设置角色等级 */
	public void setRoleLevel(int roleLevel)
	{
		this.roleLevel = roleLevel;
	}
	/** 获取服务器ID */
	public int getID()
	{
		return id;
	}
	/** 获取登录时间 */
	public int getLoginTime()
	{
		return loginTime;
	}
	/** 获取服务器 */
	public Server getServer()
	{
		return ServerManager.Instance.getServer(id);
	}
}

