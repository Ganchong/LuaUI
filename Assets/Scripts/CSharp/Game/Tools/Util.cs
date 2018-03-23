using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using LuaInterface;
using LuaUIFramework;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LuaFrameworkCore {
	
    public class Util {
		public static void Log(string str)
		{
			Debug.Log(str);
		}
		public static void LogError(string str)
		{
			Debug.LogError (str);
		}
		public static void LogWarning(string str)
		{
			Debug.LogWarning (str);
		}
		private static List<string> luaPaths = new List<string>();

		public static int Int(object o) {
			return Convert.ToInt32(o);
		}

		public static float Float(object o) {
			return (float)Math.Round(Convert.ToSingle(o), 2);
		}

		public static long Long(object o) {
			return Convert.ToInt64(o);
		}

		public static float Random(float min, float max) {
			return UnityEngine.Random.Range(min, max);
		}

		public static string Uid(string uid) {
			int position = uid.LastIndexOf('_');
			return uid.Remove(0, position + 1);
		}

		public static long GetTime() {
			TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
			return (long)ts.TotalMilliseconds;
		}

		/// <summary>
		/// 时间戳转换时间
		/// </summary>
		public static DateTime TimeCover(long data)
		{
			DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			return dtStart.AddSeconds(data);        
		}

		/// <summary>
		/// 时间戳转换时间 yyyy-MM-dd
		/// </summary>
		public static string TimeToString_01(long data)
		{
			DateTime go = Util.TimeCover(data);
			return go.ToString("yyyy-MM-dd");
		}

		/// <summary>
		/// 时间戳转换时间 HH:mm:ss
		/// </summary>
		public static string TimeToString_02(long data)
		{
			DateTime go = Util.TimeCover(data);
			return go.ToString("HH:mm:ss");
		}

		public static Component GetChildComponent(GameObject go, string subnode, int compName)
		{
			if (go != null)
			{
				return GetChildComponent(go.transform, subnode, compName);
			}
			return null;
		}
		public static Component GetChildComponent(Transform tran, string subnode, int compName)
		{
			if (tran != null)
			{

				Transform sub = tran.Find(subnode);
				if (sub == null)
					return null;
				return GetComponent(sub, compName);
			}
			return null;
		}
		public static Component GetComponent(GameObject go, int compName)
		{
			if (go == null)
			{
				return null;
			}
			return GetComponent(go.transform, compName);
		}

		public static Component GetComponent(Transform trans, int compName)
		{
			return new Component();
		}

		/// <summary>
		/// 将物体的层级置于当前层最下方
		/// </summary>
		public static void SetAsLastSibling(GameObject go)
		{
			go.transform.SetAsLastSibling();
		}
		public static void SetAsLastSibling(Transform tran)
		{
			tran.SetAsLastSibling();
		}
		public static void SetSiblingIndex(Transform tran,int index)
		{
			tran.SetSiblingIndex(index);
		}
		public static void SetSiblingIndex(GameObject go, int index)
		{
			go.transform.SetSiblingIndex(index);
		}
		public static GameObject InstantiateGameObject(GameObject prefab, Transform parent)
		{
			return CommonFunction.InstantiateGameObject(prefab,parent);
		}

		public static void SetImage(Image img, string name, bool setNativeSize = false)
		{
			CommonFunction.SetImage( img, name, setNativeSize);
		}

		public static void SetTexColor(Text textobj, Color txtColor, Color outLineColor)
		{
			SetTexColor(textobj.gameObject, txtColor, outLineColor);
		}
		public static void SetTexColor(Text textobj, Color txtColor)
		{
			SetTexColor(textobj.gameObject, txtColor);
		}

		public static void SetTexColor(GameObject obj, Color txtColor, Color outLineColor)
		{
			Text tex = obj.GetComponent<Text>();
			if (tex == null)
				return;
			tex.color = txtColor;
			Outline outLine = obj.GetComponent<Outline>();
			if (outLine == null)
				return;
			outLine.effectColor = outLineColor;
		}

		public static void SetTexColor(GameObject obj, Color txtColor)
		{
			Text tex = obj.GetComponent<Text>();
			if (tex == null)
				return;
			tex.color = txtColor;
		}

		public static void SetUrlTex(Image img, uint accid, string url, int width)
		{
			CommonFunction.SetUrlImg(img, accid, url, width);
		}

		public static void InstantiateUIObj(string name,Transform parent,Action<GameObject> callback)
		{
			
		}
		public static void DestroyObject(UnityEngine.Object go)
		{
			UnityEngine.Object.Destroy(go);
		}
		/// <summary>
		/// 查找子对象
		/// </summary>
		public static GameObject Child(GameObject go, string subnode) {
			return Child(go.transform, subnode);
		}

		/// <summary>
		/// 查找子对象
		/// </summary>
		public static GameObject Child(Transform go, string subnode) {
			Transform tran = go.Find(subnode);
			if (tran == null) return null;
			return tran.gameObject;
		}

		/// <summary>
		/// 取平级对象
		/// </summary>
		public static GameObject Peer(GameObject go, string subnode) {
			return Peer(go.transform, subnode);
		}

		/// <summary>
		/// 取平级对象
		/// </summary>
		public static GameObject Peer(Transform go, string subnode) {
			Transform tran = go.parent.Find(subnode);
			if (tran == null) return null;
			return tran.gameObject;
		}

		/// <summary>
		/// 计算字符串的MD5值
		/// </summary>
		public static string md5(string source) {
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
			byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
			md5.Clear();

			string destString = "";
			for (int i = 0; i < md5Data.Length; i++) {
				destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
			}
			destString = destString.PadLeft(32, '0');
			return destString;
		}

		/// <summary>
		/// 计算文件的MD5值
		/// </summary>
		public static string md5file(string file) {
			try {
				FileStream fs = new FileStream(file, FileMode.Open);
				System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
				byte[] retVal = md5.ComputeHash(fs);
				fs.Close();

				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < retVal.Length; i++) {
					sb.Append(retVal[i].ToString("x2"));
				}
				return sb.ToString();
			} catch (Exception ex) {
				throw new Exception("md5file() fail, error:" + ex.Message);
			}
		}

		/// <summary>
		/// 清除所有子节点
		/// </summary>
		public static void ClearChild(Transform go) {
			if (go == null) return;
			for (int i = go.childCount - 1; i >= 0; i--) {
				GameObject.Destroy(go.GetChild(i).gameObject);
			}
		}

		/// <summary>
		/// 清理内存
		/// </summary>
		public static void ClearMemory() {
			GC.Collect(); Resources.UnloadUnusedAssets();
			LuaManager.Instance.LuaGC();
		}

		/// <summary>
		/// 取得行文本
		/// </summary>
		public static string GetFileText(string path) {
			return File.ReadAllText(path);
		}

		/// <summary>
		/// 网络可用
		/// </summary>
		public static bool NetAvailable {
			get {
				return Application.internetReachability != NetworkReachability.NotReachable;
			}
		}

		/// <summary>
		/// 是否是无线
		/// </summary>
		public static bool IsWifi {
			get {
				return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
			}
		}

		#region transform属性操作
		public static void SetLocalEulerAngles(GameObject go, float x, float y, float z)
		{
			go.transform.localEulerAngles = new Vector3(x, y, z);
		}
		public static void SetLocalEulerAngles(Transform trans, float x, float y, float z)
		{
			trans.localEulerAngles = new Vector3(x, y, z);
		}

		public static void SetLocalPosition(GameObject go, float x, float y, float z)
		{
			go.transform.localPosition = new Vector3(x, y, z);
		}
		public static void SetLocalPosition(Transform trans, float x, float y, float z)
		{
			trans.localPosition = new Vector3(x, y, z);
		}

		public static void SetLocalScale(GameObject go, float x, float y, float z)
		{
			go.transform.localScale = new Vector3(x, y, z);
		}
		public static void SetLocalScale(Transform trans, float x, float y, float z)
		{
			trans.localScale = new Vector3(x, y, z);
		}
		#endregion

		public static void SetObjLayer(GameObject go, string layer)
		{
			go.layer = LayerMask.NameToLayer(layer);
		}

		public static void SetObjMesh(GameObject go,Mesh mesh)
		{
			MeshFilter mt = go.GetComponent<MeshFilter>();
			if (mt)
			{
				mt.sharedMesh = mesh;
			}
			else
			{
				//TODO
			}
		}
		public static Mesh GetObjMesh(GameObject go)
		{
			MeshFilter mt = go.GetComponent<MeshFilter>();
			if (mt)
			{
				return mt.sharedMesh;
			}
			else
			{
				/*MDebug.LogError(go.name + ".GetComponent<MeshFilter>() == null !");*/
				return null;
			}
		}

		public static void LoadScene(string sceneName)
		{
			SceneManager.LoadScene(sceneName);
		}
		public static void LoadSceneAsync(string sceneName,System.Action callback)
		{
			AsyncOperation opera = SceneManager.LoadSceneAsync(sceneName);
//			Main.Instance.StartCoroutine(LoadYourAsyncScene(opera, callback));
		}
		public static IEnumerator LoadYourAsyncScene(AsyncOperation opera, System.Action callback)
		{
			while (!opera.isDone)
			{
				yield return null;
			}
			if (callback != null)
			{
				callback();
			}
		}

		//字符串匹配 英文，中文，数字
		public static bool StringMagex(string data)
		{
			Regex P_regex = new Regex("^[\u4e00-\u9fa5a-zA-Z0-9]+$");
			bool go = P_regex.IsMatch(data);
			return go;
		}
			
		public static byte[] ReadAllByte(string fileName)
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			if (CallAndroidManager.Instance.FileExist(fileName))
			{
			return CallAndroidManager.Instance.ReadFile(fileName);
			}
			#else
			if (File.Exists(fileName))  //Android下 Stream文件夹无法进行IO操作
			{
				return File.ReadAllBytes(fileName);
			}
			#endif
			return null;
		}
		/// <summary>
		/// 将一个文件复制到另外一个文件（覆盖）
		/// </summary>
		/// <param name="sourceFile">源文件</param>
		/// <param name="destFile">目标文件，不能stream目录下</param>
		public static void CopyFile(string sourceFile, string destFile)
		{
			byte[] tmp = Util.ReadAllByte(sourceFile);
			if (tmp != null)
			{
				string dir = Path.GetDirectoryName(destFile);
				if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
				File.WriteAllBytes(destFile, tmp);
			}
		}
		/// <summary>
		/// 读取配置表中字符串
		/// 换行符的转换
		/// </summary>
		public static string ReplaceNewLineSymbol(string str)
		{
			string result = str.Replace("\\n", "\n");
			result = result.Replace("/n", "\n");
			result = result.Replace("\t", "\t");
			result = result.Replace("\\t", "\t");
			result = result.Replace("\\u3000", "\u3000");
			result = result.Replace("\u3000", "\u3000");
			return result;
		}

		public static void AddDropdownoption(Dropdown com ,string str )
		{
			Dropdown.OptionData data = new Dropdown.OptionData(str);
			com.options.Add(data);
		}
    }
}