using System;
using UnityEngine;
using UnityEngine.UI;
using UGUIFrame;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 初始化驱动
/// 汪松民
/// </summary>
public class InitDriver : BaseBehaviour
{
	/* static fields */

	/* unity fields */
	/** 进度条 */
	public Slider slider;
	/** 提示语句 */
	public UIText tipLabel;
	/** 主节点 */
	public GameObject root;
	/** 版本号 */
	public UIText version;
	/* fields */
	/** 更新器 */
	UpdateHelper updateHelper;
	/** 最后更新步骤 */
	UpdateHelper.UpdateStep lastUpdateStep;
	/** 当前进度 */
	float process = -1;
	/** 是否更新 */
	bool update = false;
	/** 第一次 */
	bool firstTime = true;

	/* properties */

	/* methods */
	/** 初始化 */
	protected override void Awake ()
	{
		slider.value = 0;
		root.SetActive (false);
		this.Register ((int)GameEvent.ResourceUpdate, start);
	}
	/** 游戏开始 */
	public void start()
	{
		slider.value = 0;
		process = 0;
		enabled = false;
//		tipLabel.text = Language.get("TIP_0");
		version.text = SDKHelper.getAppVersion().version.ToString();
		root.SetActive (true);
//		BackGroundHelper.instance.change ("loginBack_3");
		CoroutineCenter.delayRunFrame (() => {
			enabled = true;
			if (Application.internetReachability == NetworkReachability.NotReachable) {
//				Alert.Show ((ans) => {
//					connect ();
//				}, "@network_0", false);
			} else
				connect ();
		}, 1);
	}
	/** 连接服务器 */
	private void connect()
	{
		SDKHelper.saveState(StateStep.STEP7);
		if(GameManager.Instance.openSDK)
		{
			connectSDKServer();
		}
		else
		{
			checkUpdate();
		}
	}
	/** 更新 */
	protected override void Update ()
	{
		if (process < 0) return;
		if(updateHelper != null && lastUpdateStep != updateHelper.CurUpdateStep)
		{
			lastUpdateStep = updateHelper.CurUpdateStep;
			switch(lastUpdateStep)
			{
			case UpdateHelper.UpdateStep.CheckVesion:
//				tipLabel.text = Language.get("TIP_2");
				update=false;
				process = 0;
				break;
			case UpdateHelper.UpdateStep.GetUpdateList:
//				tipLabel.text = Language.get("TIP_3");;
				process = 0.1f;
				break;
			case UpdateHelper.UpdateStep.CompareData:
//				tipLabel.text = Language.get("TIP_4");;
				process = 0.2f;
				break;
			case UpdateHelper.UpdateStep.MakeSureDownload:
				SDKHelper.saveState(StateStep.STEP12);
				if(Application.internetReachability==NetworkReachability.ReachableViaCarrierDataNetwork)
				{
//					Alert.Show((ans)=>{
//						updateHelper.downloadCurFileList();
//						MaskWindow.unLockUI();
//					},Language.get("network_1",(updateHelper.DownloadTotalSize*1f/(1024*1024)).ToString("F")),false);
				}
				else updateHelper.downloadCurFileList();
				update=true;
				process = 0.3f;
				break;
			case UpdateHelper.UpdateStep.DownloadData:
				break;
			case UpdateHelper.UpdateStep.CheckData:
//				tipLabel.text = Language.get("TIP_6");
				process = 0.8f;
				break;
			case UpdateHelper.UpdateStep.CopyData:
//				tipLabel.text = Language.get("TIP_7");
				process = 0.9f;
				break;
			case UpdateHelper.UpdateStep.CleanCache:
				break;
			case UpdateHelper.UpdateStep.Finish:
				if(updateHelper.CurUpdateResult != UpdateHelper.UpdateResult.Success)
				{
//					tipLabel.text = Language.get("TIP_9");
					SDKHelper.saveState(StateStep.STEP13);
				}
				else
				{
					SDKHelper.saveState(StateStep.STEP14);
//					tipLabel.text = Language.get("TIP_7");
					process=0.9f;
					Action initFinish=()=>{
						SDKHelper.saveState(StateStep.STEP16);
						process = 1;
//						tipLabel.text = Language.get("TIP_8");
					};
					if(update)
					{
						SDKHelper.saveState(StateStep.STEP15);
//						int count=0;
//						Action loadFinish=()=>{count++;if(count<4)return;initFinish();};
//						List<string> updateList=updateHelper.DownLoadList;
//						if(updateList.Contains((Language.LANGUAGE+ResourceHelper.SUFFIX).ToLower()))
//							Language.InitLanguage(loadFinish);
//						else loadFinish();
//						if(firstTime||updateList.Contains((ConfigHelper.NORMALCONFIG+ResourceHelper.SUFFIX).ToLower()))
//							ConfigHelper.Instance.InitConfig(loadFinish,setResourceProcess);
//						else loadFinish();
//						UiManager.Instance.initResDataAndLoad(loadFinish,updateList);
//						StartCoroutine(SampleFactory.ResetAll(loadFinish,updateList));
//						ResourceHelper.Instance.InitManifest(()=>{initResource(loadFinish);});
					}
					else
					{
						if(firstTime) initNoUpdate(initFinish);
						else initFinish();
					}
				}
				break;
			}
		}
		if(lastUpdateStep == UpdateHelper.UpdateStep.DownloadData) {
			tipLabel.text = (updateHelper.CurDownloadSize*1f/(1024*1024)).ToString("F")+"M/"+(updateHelper.DownloadTotalSize*1f/(1024*1024)).ToString("F")+"M";
			process = 0.3f + 0.4f * updateHelper.CurDownloadSize / updateHelper.DownloadTotalSize;
		} else if(lastUpdateStep == UpdateHelper.UpdateStep.GetUpdateList) {
			process = 0.1f + 0.1f * updateHelper.CurDownloadSize / 1000;
		}

		if(slider.value < process)
		{
			slider.value=Mathf.Lerp(slider.value,process,0.08f);
		}

#if UNITY_EDITOR
		if(GameManager.Instance.isSkipFlash)
		{
			slider.value=process;
		}
#endif
		if(slider.value >= 0.99f)
		{
			updateFinish();
			process = -1;
		}
	}
	/** 更新完成 */
	private void updateFinish()
	{
		firstTime = false;
//		tipLabel.text = Language.get("TIP_8");
		StartCoroutine (ToLoginScene());
		SDKHelper.saveState(StateStep.STEP17);
	}
	/** 切换场景 */
	private IEnumerator ToLoginScene()
	{
//		UpdateHelper helper=GetComponent<UpdateHelper> ();
//		if (helper != null) Destroy (helper);
//		yield return null;
//		if (GameManager.Instance.ScenceID != LoadingWindow.LOGIN) 
//		{
//			GameManager.Instance.ScenceID = LoadingWindow.LOGIN;
//			var operation=SceneManager.LoadSceneAsync(LoadingWindow.LOGIN);
//			yield return operation;
//		}
//		SDKHelper.saveState(StateStep.STEP18);
//		UiManager.Instance.openWindow<LoginWindow> ();
//		root.SetActive (false);
		yield return null;
	}
	/** 初始化配置 */
	private void initConfig(Action action)
	{
		int count = 0;
		Action action1 = ()=> {count++; if(count>1) action();};
//		ConfigHelper.Instance.InitConfig (action1, setResourceProcess);
//		StartCoroutine(ResourcesManager.Instance.LoadAsync<TextAsset>("keyword",(text)=>{
//			action1();
//			string str=text.text;
//			Resources.UnloadAsset(text);
//			Action action2=()=>{
//				KeyWordKit.setString (str.Split (','));
//			};
//			action2.BeginInvoke(null,null);
//		}));
	}
	/** 进度 */
	private void setResourceProcess(float process)
	{
		this.process = 0.9f + process / 10;
	}
	/** 初始化基础资源 */
	private void initResource(Action action)
	{
		int count = 0;
//		Action action1 = ()=> {count++; if(count>=3) action();};
//		ResourceHelper.Instance.LoadResDataAsync(ResourceHelper.UIEFFECTPATH+"LoginWindowEffect",(data)=>{action1();});
//		ResourceHelper.Instance.LoadMainAssetAsync<GameObject>(ResourceHelper.UIPATH+"LoginWindow",(data)=>{action1();});
//		ResourceHelper.Instance.LoadResDataAsync(ResourceHelper.TEXTUREPATH+TextureHelper.BACKGROUND+"loginBack",(data)=>{action1 ();});
//		LoadingWindow.CacheTexture ();
	}
	/** 没有更新初始化 */
	private void initNoUpdate(Action action)
	{
		int count = 0;
		Action action1=()=>{count++;if(count>=3) action();};
		initResource (action1);
//		UiManager.Instance.initResData (action1);
//		ConfigHelper.Instance.InitConfig (action1,setResourceProcess);
	}
	/** 连接服务器 */
	public void connectSDKServer()
	{
//		tipLabel.text = Language.get("TIP_0");
		SDKHelper.connectServer((message)=>{
			IJsonNode node=message;
			node=node["error"];
			if(node==null||string.IsNullOrEmpty(node.ToString()))
			{
				SDKHelper.saveState(StateStep.STEP8);
				int nowTime=message["nowTime"].ToInt();
				TimeKit.resetTime(nowTime*1000L);
				checkUpdate();
			}
			else
			{
//				tipLabel.text = Language.get("TIP_10");
//				Alert.Show((alert)=>{
//					if(node.ToString()=="version")
//					{
//						string downUrl=SDKHelper.getDownUrl();
//						if(!string.IsNullOrEmpty(downUrl))
//							Application.OpenURL(downUrl);
//						Application.Quit();
//						return;
//					}
//					connectSDKServer();
//				},Language.get("connect_"+node.ToString()),false);
				SDKHelper.saveState(StateStep.STEP9);
			}
		});
	}
	/** 检查更新 */
	public void checkUpdate()
	{
		if(SDKHelper.enableUpdate())
		{
			if(GameManager.Instance.openSDK)
			{
				SDKHelper.saveState(StateStep.STEP10);
				updateHelper = gameObject.AddComponent<UpdateHelper>();
				lastUpdateStep = updateHelper.CurUpdateStep;
				updateHelper.startCheckData(SDKHelper.getUpdateDataUrl(),SDKHelper.getInfo(Handler.RESOURCEVERSION));
			}
			else
			{
				string url=SDKHelper.getUpdateDataUrl();
				int index=url.IndexOf('/',8);
				url=url.Substring(0,index)+"/getVersion?1=1";
				if(Log.isInfoEnable()) Log.info("version url:"+url);
				HttpConnect.access(url,(ans,result)=>{
					if(ans==HttpConnect.OK)
					{
						if(Log.isInfoEnable()) Log.info("serverVersion:"+result);
						updateHelper = gameObject.AddComponent<UpdateHelper>();
						lastUpdateStep = updateHelper.CurUpdateStep;
						updateHelper.startCheckData(SDKHelper.getUpdateDataUrl(),result);
					}
					else
					{
						if(Log.isInfoEnable())	Log.info("update expcetion!");
//						tipLabel.text = Language.get("TIP_7");
						process=0.9f;
						initNoUpdate(()=>{
//							tipLabel.text = Language.get("TIP_8");
							process = 1;
						});
					}
				});
			}
		}
		else
		{
//			tipLabel.text = Language.get("TIP_7");
			process=0.9f;
			initNoUpdate(()=>{
//				tipLabel.text = Language.get("TIP_8");
				process = 1;
			});
		}
	}
}