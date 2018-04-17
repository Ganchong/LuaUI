using System;
using System.Collections;
/// <summary>
/// 获取PID端口
/// 汪松民
/// </summary>
public class GetRoleIDHttpPort : BaseHttpPort<GetRoleIDHttpPort>
{
	/* static fields */

	/* fields */
	/** 回调 */
	Action<string> action;

	/* methods */
	/** 请求方法 */
	public void access(Action<string> action,string certifyID,string uid,int plantID,int serverID)
	{
		this.action = action;
		string url = TextKit.parse (HttpPathHelper.GETPIDURL, certifyID, uid, plantID, serverID);
		access (url);
	}

	/** 返回结果 */
	protected override void readData (string[] results)
	{
		base.readData (results);
		action (results [1]);
		DestroyInstance ();
	}
}

