using System;
using UnityEngine;

/// <summary>
/// 认证管理器
/// 汪松民
/// </summary>
public class CertifyManager : Singleton<CertifyManager>,IEventSender
{
	/* static fields */
	/** 认证地址 */
	public const string CERTIFYURL = "https://%1:%2/certify?id=%3&name=%4&trust=%5";


	/* fields */
	/** 当前选择服务器 */
	Server server;
	/** 角色UID */
	string uid;

	/* methods */
	/** 初始化 */
	public void init(Server server,string uid)
	{
		this.server = server;
		this.uid = uid;
		if (GameManager.Instance.openSDK) 
		{
			SDKHelper.certify (server.ID, (message)=>{
				IJsonNode node = message;
				string error = node["error"].ToString ();
				if (string.IsNullOrEmpty (error) || "null".Equals (error)) {
					if (node["update"].ToBoolean())
					{
						if(node["serverVersion"].ToInt () > GameManager.Instance.streamingAssetVersion) 
						{
							StateManager.Instance.DoLuaFunction(LuaFuncName.CheckVersionUpdate);
							return;
						}
					}
					string sid = node["sid"].ToString ();
					uid = node["uid"].ToString ();
					IJsonNode goods = node["goods"];
					//ChargeManager.Instance.praseGoods (goods);
					SDKHelper.saveState (StateStep.STEP28);
					LoginManager.Instance.login (uid, sid, server);
				} else {
					if(error=="version")
					{
						StateManager.Instance.DoLuaFunction(LuaFuncName.ShowAlert,"connect_version",()=>{
							string downUrl=SDKHelper.getDownUrl();
							if(!string.IsNullOrEmpty(downUrl))
								Application.OpenURL(downUrl);
							Application.Quit();
						});
						return;
					}
					SDKHelper.saveState (StateStep.STEP29);
					//Tips.Show (Language.get(error));
					MaskWindow.unLockUI();
				}
			});
		}
		else certify();
	}

	/** Unity认证认证函数 */
	private void certify ()
	{
		string certifyUrl = TextKit.parse(CERTIFYURL,server.IpAddress,server.HttpPort,uid,uid,GameManager.Instance.gm);
		HttpConnect.access (certifyUrl,(ans,result)=>{
			if(ans == HttpConnect.OK)
			{
				if(Log.isInfoEnable()) Log.info ("certify is back, result=" + result);
				string[] splits = {":"};
				if (result.StartsWith ("ok:")) {
					string[] strings = result.Split (splits, StringSplitOptions.None);
					string certifyID = strings [1];
					if(Log.isInfoEnable()) Log.info ("certify is ok,certifyId=" + certifyID);
					certifyBack(certifyID);
				} else {
					Debug.LogError(result);
					string errorCode = result.Split (splits, StringSplitOptions.None) [1];
					if(Log.isInfoEnable()) Log.info ("certify is fail,errCode=" + errorCode);
					//Tips.Show(errorCode);
					MaskWindow.unLockUI();
				}
			}
			else
			{
				Debug.LogError ("certify is error:" + result);
				StateManager.Instance.DoLuaFunction("ShowAlert","loginWindow_3",()=>{
					MaskWindow.unLockUI();
					//certify();
				});
			}
		});
	}
	/** 认证回调 */
	public void certifyBack(string certifyID)
	{
		LoginManager.Instance.login(uid,certifyID,server);
	}
	/** 重新认证 */
	public void reCertify (Action<string> action)
	{
		string certifyUrl = TextKit.parse(CERTIFYURL,server.IpAddress,server.HttpPort,uid,uid,GameManager.Instance.gm);
		HttpConnect.access (certifyUrl,(ans,result)=>{
			if(ans == HttpConnect.OK)
			{
				if(Log.isInfoEnable()) Log.info ("certify is back, result=" + result);
				string[] splits = {":"};
				if (result.StartsWith ("ok:")) {
					string[] strings = result.Split (splits, StringSplitOptions.None);
					string certifyID = strings [1];
					if(Log.isInfoEnable()) Log.info ("certify is ok,certifyId=" + certifyID);
					action(certifyID);
				} else {
					string errorCode = result.Split (splits, StringSplitOptions.None) [1];
					if(Log.isInfoEnable()) Log.info ("certify is fail,errCode=" + errorCode);
					//Tips.Show(errorCode);
					MaskWindow.unLockUI();
				}
			}
			else
			{
				Debug.LogError ("certify is error:" + result);
				StateManager.Instance.DoLuaFunction("ShowAlert","loginWindow_3",()=>{
					certify();
				});
			}
		});
	}
}

