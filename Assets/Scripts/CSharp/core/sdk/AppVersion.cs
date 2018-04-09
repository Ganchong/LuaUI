using System;

/// <summary>
/// APPVersoin
/// </summary>
public class AppVersion
{
	/* fields */
	/** 版本号 */
	public string version="1.0.1";
	/** VC */
	public int versionCode=1;

	/* methods */
	/** JSON序列化读 */
	public void jsonRead(IJsonNode json)
	{
		version=json["version"].ToString();
		versionCode=json["versionCode"].ToInt();
	}
}