using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;
public static class CommonFunction
{
    #region 所有项目可用
    /// <summary>
    /// 获取指定个数的子物体，其它隐藏
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static List<Transform> GetChildByCount(Transform parent,int count)
    {
        List<Transform> tmp = new List<Transform>(count + 1);
        if (parent.childCount <= 0)
            return null;
        for (int i = 0; i < parent.childCount; ++i)
        {
            Transform tt = parent.GetChild(i);
            if (tt == null)
                continue;
            if (count < 1)
            {
                tt.gameObject.SetActive(false);
            }
            else
            {
                tt.gameObject.SetActive(true);
                --count;
                tmp.Add(tt);
            }
        }
        if (count > 0 && tmp.Count > 0)
        {
            Transform tt = tmp[tmp.Count - 1];
            if (tt != null)
            {
                for (int i = 0; i < count; ++i)
                {
                    GameObject clone = GameObject.Instantiate(tt.gameObject, parent);
                    if (clone != null)
                        tmp.Add(clone.transform);
                }
            }
        }
        return tmp;
    }
    /// <summary>
    /// 生成物体并返回需要的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static T InstantiateObject<T>(GameObject source, Transform parent) where T : MonoBehaviour
    {
        GameObject go = GameObject.Instantiate(source);
        if (parent != null)
            go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        T tmp = go.GetComponent<T>();
        if (tmp == null)
        {
            tmp = go.AddComponent<T>();
        }
        return tmp;
    }

    /// <summary>
    /// 生成物体并返回物体
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject InstantiateGameObject(GameObject prefab, Transform parent)
    {
        if (prefab == null)
            return null;
        GameObject clone = GameObject.Instantiate(prefab) as GameObject;
        if (clone != null && parent != null)
        {
            clone.layer = parent.gameObject.layer;
            clone.transform.SetParent(parent);
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localRotation = Quaternion.identity;
            clone.transform.localScale = prefab.transform.localScale;
        }
        return clone;
    }

    /// <summary>
    //文件写入//
    /// </param>
    public static void WriterLog(string sInfo)
    {
        string datapath;
        string name = "/" + System.DateTime.Now.ToString("yyyy-MM-dd") + "Log.txt";
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            datapath = Application.persistentDataPath + name;
        }
        else
        {
            datapath = Application.dataPath + name;
        }
        try
        {
            FileStream fs = new FileStream(@datapath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(System.DateTime.Now.ToString() + ":  " + sInfo);
            sw.Flush();
            sw.Close();
            fs.Close();
            fs.Dispose();
        }
        catch (IOException ex)
        {
        }
    }

    /// <summary>
    /// 获取格式化时间【时：分：秒】
    /// </summary>
    /// <param name="vSeconds"></param>
    /// <returns></returns>
    public static string ObtainFormatTimeFromSecond(uint vSeconds)
    {
        uint tmpHour = vSeconds / 3600;
        uint tmpMinute = (vSeconds % 3600) / 60;
        uint tmpSecond = vSeconds % 60;
        string tmpResult = string.Format("{0:D2}:{1:D2}", tmpMinute, tmpSecond);
        if (tmpHour > 0)
        {
            tmpResult = string.Format("{0:D2}:{1}", tmpHour, tmpResult);
        }
        return tmpResult;
    }

    /// <summary>
    /// 将毫秒转换为秒
    /// </summary>
    /// <param name="vTime"></param>
    /// <returns></returns>
    public static float ObtainSeconds(float vTime)
    {
        return vTime / 1000;
    }

    /// <summary>
    /// 根据时间戳获取时间
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static System.DateTime GetDateTime(long time)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        DateTime dt = startTime.AddSeconds(time);
        return dt;
    }
    /// <summary>
    /// 根据时间戳获取时间字符串
    /// 2017-05-03 11:02:09
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string GetDateTimeStr(long time)
    {
        DateTime dt = GetDateTime(time);
        return string.Format("{0}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}",
            dt.Year,dt.Month,dt.Day,dt.Hour,dt.Minute,dt.Second);
    }
    /// <summary>
    /// 数字转中文
    /// </summary>
    /// <param name="number">eg: 22</param>
    /// <returns></returns>
    public static string NumberToChinese(int number)
    {
        string res = string.Empty;
        string str = number.ToString();
        string schar = str.Substring(0, 1);
        switch (schar)
        {
            case "1":
                res = "一";
                break;
            case "2":
                res = "二";
                break;
            case "3":
                res = "三";
                break;
            case "4":
                res = "四";
                break;
            case "5":
                res = "五";
                break;
            case "6":
                res = "六";
                break;
            case "7":
                res = "七";
                break;
            case "8":
                res = "八";
                break;
            case "9":
                res = "九";
                break;
            default:
                res = "零";
                break;
        }
        if (str.Length > 1)
        {
            switch (str.Length)
            {
                case 2:
                case 6:
                    res += "十";
                    break;
                case 3:
                case 7:
                    res += "百";
                    break;
                case 4:
                    res += "千";
                    break;
                case 5:
                    res += "万";
                    break;
                default:
                    res += "";
                    break;
            }
            res += NumberToChinese(int.Parse(str.Substring(1, str.Length - 1)));
        }
        return res;
    }

    /// <summary>
    /// 截取指定字符串长度  并以..结尾
    /// </summary>
    /// <param name="nickname"></param>
    /// <returns></returns>
    public static string CutNickName(string nickname)
    {
        int length = 12;
        //ServerCommon data = ConfigManager.Instance.GeneralCommonData.GetData(ServerCommonID.PLAYERNICKNAMELENGTH);
        //if (data != null)
        //{
        //    int.TryParse(data.value, out length);
        //}
        string result = CommonFunction.CutString(nickname, length);
        return result;
    }

    /// <summary>
    /// 截取指定字符串长度  并以..结尾
    /// </summary>
    /// <param name="strInput"></param>
    /// <param name="intLen"></param>
    /// <returns></returns>
    public static string CutString(string strInput, int intLen)
    {
        strInput = strInput.Trim();
        byte[] myByte = System.Text.Encoding.Default.GetBytes(strInput);
        if (myByte.Length > intLen)
        {
            string resultStr = "";
            for (int i = 0; i < strInput.Length; i++)
            {
                byte[] tempByte = System.Text.Encoding.Default.GetBytes(resultStr);
                if (tempByte.Length < (intLen - 2))
                {
                    resultStr += strInput.Substring(i, 1);
                }
                else
                {
                    break;
                }
            }
            return resultStr + "..";
        }
        else
        {
            return strInput;
        }
    }

    /// <summary>
    /// 字符串是否为空或0
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsStringNullOrEmptyOrZero(string str)
    {
        return string.IsNullOrEmpty(str) | (str == "0");
    }

    [System.Diagnostics.DebuggerHidden]
    [System.Diagnostics.DebuggerStepThrough]
    static public bool GetActive(Behaviour mb)
    {
        return mb && mb.enabled && mb.gameObject.activeInHierarchy;
    }
    static public float ParticleSystemLength(Transform transform)
	{
		ParticleSystem []particleSystems = transform.GetComponentsInChildren<ParticleSystem>();
		float maxDuration = 0;
		foreach(ParticleSystem ps in particleSystems){
			if(ps.emission.enabled){
				if(ps.main.loop){
					return -1f;
				}
                float dunration = ps.main.duration;
                //if(ps.emission.rateOverDistanceMultiplier <=0){
                //    dunration = ps.main.startDelay.constant + ps.main.startLifetime.constant;
                //}else{
                //    dunration = ps.main.startDelay.constant + Mathf.Max(ps.main.duration, ps.main.startLifetime.constant);
                //}
				if (dunration > maxDuration) {
					maxDuration = dunration;
				}
			}
		}
		return maxDuration;
	}

    #endregion

    #region  仅此项目可用
    /// <summary>
    /// 设置物件层级
    /// </summary>
    /// <param name="vTrans"></param>
    /// <param name="vLayer"></param>
    public static void SetObjLayer(Transform vTrans, int vLayer)
    {
        if (vTrans == null)
            return;
        vTrans.gameObject.layer = vLayer;
        for (int i = 0; i < vTrans.childCount; i++)
            SetObjLayer(vTrans.GetChild(i), vLayer);
    }
    /// <summary>
    /// 设置物件层级
    /// </summary>
    /// <param name="vTrans"></param>
    /// <param name="vLayer"></param>
    public static void SetSortOrderLayer(Transform vTrans, string vSortLayer, int vOrderLayer)
    {
        if (vTrans == null)
            return;
        Renderer tmp = vTrans.GetComponent<Renderer>();
        if (tmp != null)
        {
            tmp.sortingLayerName = vSortLayer;
            tmp.sortingOrder = vOrderLayer;
        }
        for (int i = 0; i < vTrans.childCount; i++)
            SetSortOrderLayer(vTrans.GetChild(i), vSortLayer, vOrderLayer);
    }
    /// <summary>
    /// 获取整型数值某一位的值[0-false 1-true]
    /// </summary>
    /// <param name="vValue">需要查看的数值</param>
    /// <param name="vIndex">需要查看的位值,从右到左: 11101[0-1;1-0;2-1;3-1;4-1]</param>
    /// <returns></returns>
    public static bool GetBitStatus(int vValue, byte vIndex)
    {
        return (vValue & (1 << vIndex)) > 0;
    }

    /// <summary>
    /// 设置整型数值某一位的值[0-false 1-true]
    /// </summary>
    /// <param name="vValue">需要改变的数值</param>
    /// <param name="vIndex">需要改变的位值,从右到左: 11101[0-1;1-0;2-1;3-1;4-1]</param>
    /// <param name="vStatus">改变之后的状态</param>
    /// <returns></returns>
    public static int SetBitStatus(int vValue, int vIndex, bool vStatus)
    {
        if (vStatus)
        {
            return vValue | (1 << vIndex);
        }
        else
        {
            return ~((~vValue) | 1 << vIndex);
        }
    }
    /// <summary>
    /// 设置图片纹理，对应RawImage
    /// </summary>
    /// <param name="rimg"></param>
    /// <param name="name"></param>
    public static void SetRawIamge(RawImage rimg, string name)
    {
//        ResourceManager.Instance.RequestImage(name, rimg, (texture) =>
//        {
//            rimg.texture = texture;
//        });
    }
    /// <summary>
    /// 设置图片精灵，对应Image
    /// </summary>
    /// <param name="img"></param>
    /// <param name="name"></param>
    public static void SetImage(Image img, string name, bool setNativeSize = false)
    {
        if (CommonFunction.IsStringNullOrEmptyOrZero(name))
            return;
//        ResourceManager.Instance.RequestSprite(name, img, (sprite) =>
//        {
//            img.sprite = sprite;
//            if (setNativeSize)
//            {
//                img.SetNativeSize();
//            }
//        });
    }

    /// <summary>
    /// 设置按钮状态
    /// </summary>
    public static void SetBtnState(GameObject btn, bool state, bool disableClick = true)
    {
        Image img = btn.GetComponent<Image>();
        if (img != null && disableClick)
            img.raycastTarget = state;
        Image[] imgs = btn.GetComponentsInChildren<Image>();
        for (int i = 0; i < imgs.Length; i++)
        {
            SetImgGray(imgs[i], !state);
        }
    }
    /// <summary>
    /// 将图片设置为黑白图
    /// </summary>
    /// <param name="isGray">是否灰，否则恢复正常颜色</param>
    public static void SetImgGray(Image img, bool isGray)
    {
        if (img.material == null || img.material.name != "SpritesNeedGray")
        {
//            Material mat = ResourceManager.Instance.LoadResource("Material/SpritesNeedGray") as Material;
//            img.material = mat;
        }
        img.color = isGray ? Color.black : Color.white;
    }

    /// <summary>
    /// 匹配字符和数字
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNaturalAndNumber(string str)
    {
        System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
        return reg1.IsMatch(str);
    }


    public static void SetUrlImg(Image img,uint accid, string url, int width)
    {
//        RequestIconByPath(accid, url, width, (Sprite) =>
//        {
//            img.sprite = Sprite;
//        });
    }



    /// <summary>
    /// 动态创建一个Sprite
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static Sprite CreateSprite(int width, int height, byte[] bytes)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);
        texture.anisoLevel = 10;
        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
    /// <summary>
    /// 通过Texture动态创建一个Sprite  
    /// </summary>
    /// <param name="texture">texture</param>
    /// <param name="scale">scale</param>
    /// <returns></returns>
    public static Sprite CreateSpriteByTexture(Texture2D texture, int width)
    {
        if (texture == null)
            return null;
        //Texture2D target = TextureUtility.RescaleAndKeepAspect(texture, width);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }

    #region 图文混排
    /// 临时版本使用，有缺陷如下：
    /// 1.文本必须左上对齐
    /// 2.当<sprite=图片名/>字段跨行时，空格占位后，因为Text的机制可能会出现换行，导致图片后空白一段，所以拼UI时最好先运行下调整好Text宽度
    /// 3.其他富文本不能 跨图片使用

    /// <summary>
    /// 图文混排函数，主要给策划用的，在文本中填写<sprite=图片名/>来添加图片(按原图尺寸等比缩放至和字符同高)，需要混排的Text组件需要调用该函数处理
    /// </summary>
    /// <param name="m_text">需要混排的文本</param>
    public static void TextWithImage(Text m_text)
    {
        string val = m_text.text;
        m_text.text = "";
        CalculateText(val, m_text);
    }
    private static void CalculateText(string val, Text m_text)
    {
        if (val.Contains("<sprite="))
        {
            int iNum = val.IndexOf("<sprite=");
            m_text.text += val.Substring(0, iNum);
            Vector2 pos = new Vector2(m_text.preferredWidth, m_text.preferredHeight);
            val = val.Substring(iNum, val.Length - iNum);
            iNum = val.IndexOf("/>");
            string picName = val.Substring(0, iNum).Replace("<sprite=", "").Trim();
            int count = DrawTextPic(pos, m_text, picName);
            for (int i = 0; i < count; i++)
            {
                m_text.text += " ";
            }
            val = val.Substring(iNum + 2, val.Length - iNum - 2);
            CalculateText(val, m_text);
        }
        else
        {
            m_text.text += val;
        }
    }
    private static int DrawTextPic(Vector2 pos, Text m_text, string name)
    {
        Sprite spt = new Sprite();
//        ResourceManager.Instance.RequestSprite(name, m_text, (s) =>
//        {
//            spt = s;
//        });
        if (spt == null)
            return 0;
        TextGenerator g = m_text.cachedTextGeneratorForLayout;
        int num = g.lineCount;
        int xNum = g.lines[num - 1].startCharIdx;
        float x = g.GetPreferredWidth(m_text.text.Substring(xNum, m_text.text.Length - xNum), m_text.GetGenerationSettings(g.rectExtents.size));
        Vector2 vpos = new Vector2(x, -pos.y);
        GameObject gobj = new GameObject();
        Image img = GameObject.Instantiate(gobj, m_text.transform).AddComponent<Image>();
        img.transform.localPosition = Vector3.zero;
        img.transform.localScale = Vector3.one;
        img.gameObject.name = name;
        float ysize = pos.y / num;
        float xsize = spt.rect.size.x * ysize / spt.rect.size.y;
        img.rectTransform.sizeDelta = new Vector2(xsize, ysize);
        img.sprite = spt;
        img.rectTransform.anchorMax = Vector2.up;
        img.rectTransform.anchorMin = Vector2.up;
        img.rectTransform.pivot = Vector2.zero;
        img.rectTransform.anchoredPosition = vpos;
        float blank = g.GetPreferredWidth(" ", m_text.GetGenerationSettings(g.rectExtents.size));
        return Mathf.CeilToInt(xsize / blank);
    }
    #endregion

    public static DateTime LongToDateTime(long timeStamp)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        DateTime dt = startTime.AddSeconds(timeStamp);
        return dt;
    }
    /// <summary>
    /// </summary>
    /// <param name="DateTime1">结束时间</param>
    /// <param name="DateTime2">当前时间</param>
    /// <returns></returns>
    public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
    {
        string dateDiff = null;
        TimeSpan ts = DateTime1.Subtract(DateTime2).Duration();
        dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + ":" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString();
        return dateDiff;
    }

    public delegate bool LoopDoPlayerControlMode<T>(int index, T v);
    /// <summary>
    /// 向前循环
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="call"></param>
    public static void ListLooper_Forward<T>(List<T> list, LoopDoPlayerControlMode<T> call)
    {
        if (list == null)
            return;
        for (int i = 0; i < list.Count; ++i)
        {
            if (call(i, list[i]))
                break;
        }
    }
    /// <summary>
    /// 向后循环
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="call"></param>
    public static void ListLooper_Retreat<T>(List<T> list, LoopDoPlayerControlMode<T> call)
    {
        if (list == null)
            return;
        for (int i = list.Count - 1; i >= 0; --i)
        {
            if (call(i, list[i]))
                break;
        }
    }
    #endregion
}