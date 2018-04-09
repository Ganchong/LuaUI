using UnityEngine;
using System.Collections;

/// <summary>
/// 遮罩窗口
/// 汪松民
/// </summary>
public class MaskWindow : MonoBehaviour
{
	/* static fields */
	/** 单例 */
	private static MaskWindow instance;
	/** 展现Loading等待时间 */
	private const float SHOW_CD = 0.5f;
	/** 展现Loading最短时间 */
	private const float MINCD = 1f;

	/* unity fields */
	/** UI遮罩 */
	public GameObject maskUI;
	/** 网络遮罩 */
	public GameObject maskNet;
	/** 进度 */
	public GameObject progress;
	/** 点击特效 */
	public GameObject effect;

	/** 上次显示时间 */
	float lastShowTime;
	/** 网络锁开始的时间 */
	float netLockTime;
	/** 是否显示进度条 */
	bool showProgress;
	/** 是否显示点击特效 */
	bool showEffect;
	/** 展现网络锁时间 */
	float showNetLockTime;


	/* fields */

	/* methods */
	private void Awake()
	{
		instance = this;
	}
	/** 锁屏 */
	public static void lockUI()
	{
		if (instance == null) return;
		instance.maskUI.SetActive (true);
	}
	/** 解锁 */
	public static void unLockUI()
	{
		if (instance == null) return;
		instance.maskUI.SetActive (false);
	}

	/** 网络锁 */
	public static void lockNet()
	{
		if (instance == null) return;
		instance.maskNet.SetActive (true);
		instance.showProgress = true;
		instance.netLockTime = Time.unscaledTime;
	}

	/** 网络锁解 */
	public static void unLockNet()
	{
		if (instance == null) return;
		instance.maskNet.SetActive (false);
		instance.showProgress = false;
	}

	void Update(){
		if (showProgress && !progress.activeSelf) {
			if (Time.unscaledTime - netLockTime > SHOW_CD)
			{
				showNetLockTime=Time.unscaledTime;
				progress.gameObject.SetActive (true);
			}
		} else if (showProgress && Time.unscaledTime - netLockTime > DataAccessHandler.TIMEOUT+1F) {
			//if(!LoginManager.Instance.IsReLogining()) LoginManager.Instance.reLogin ();
		} else if((!showProgress)&&progress.activeSelf){
			if(Time.unscaledTime - showNetLockTime > MINCD)
				progress.gameObject.SetActive (false);
		}

		if (showEffect && Time.unscaledTime - lastShowTime > 0.5f) 
		{
			showEffect=false;
			effect.gameObject.SetActive (false);
		}
		if(Input.GetMouseButtonDown(0)){
			//effect.transform.localPosition = UiManager.ScreenToUIPos(Input.mousePosition,effect.transform.parent);
			if(showEffect) effect.gameObject.SetActive(false);
			else showEffect=true;
			effect.gameObject.SetActive(true);
			lastShowTime = Time.unscaledTime;
		}
	}
}

