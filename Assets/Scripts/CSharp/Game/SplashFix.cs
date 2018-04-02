using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 闪屏修正
/// </summary>
public class SplashFix : MonoBehaviour 
{
	/* inner enum */
	/** 闪屏类型 */
	public enum SplashScaling
	{
		Center,
		ScaleToFit,
		ScaleToFill,
	}
	/* unity fields */
	/** 闪屏类型 */
	public SplashScaling splash = SplashScaling.ScaleToFit;
	/** 图片 */
	public RawImage texture;
	/** 图的大小 */
	public int widthFix, heightFix;
	/** 提示图大小 */
	public int tipWidth, tipHeight;
	/** 下降像素 */
	public float tipDownPixel;

	/** 提示图片 */
	public RawImage tipTexture;

	/* methods */
	/** 初始化 */
	public void Awake()
	{
		Transform mTransform = transform;
		mTransform.position = Vector3.zero;
		mTransform.localScale = Vector3.one;

		if(Application.platform==RuntimePlatform.IPhonePlayer||Application.platform==RuntimePlatform.OSXPlayer||Application.platform==RuntimePlatform.OSXEditor)
		{
			splash = SplashScaling.ScaleToFill;
		}
	}
	/** 显示 */
	public void Start()
	{
		int width = Screen.width;
		int height = Screen.height;
		float x = width / 2f;
		float y = height / 2f;
		float w=0, h=0;
		float tw = 0, th = 0;
		if (splash == SplashScaling.Center) {
			w = widthFix-width;
			h = heightFix-height;
			tw = tipWidth-width;
			th = tipHeight-height;
		} else if (splash == SplashScaling.ScaleToFit) {
			float dpi1 = 1f * width / widthFix;
			float dpi2 = 1f * height / heightFix;
			dpi1 = Mathf.Min(dpi1, dpi2);
			w = widthFix * dpi1-width;
			h = heightFix * dpi1-height;
			tw = tipWidth * dpi1-width;
			th = tipHeight * dpi1-height;
			tipDownPixel *= dpi1;
		} else if (splash == SplashScaling.ScaleToFill) {
			w=0;
			h=0;
			tw = tipWidth-width;
			th = tipHeight-height;
		}
		//texture.SetClipRect(new Rect (x-w/2, y-h/2,w,h),true);
//		if (tipTexture != null) tipTexture.pixelInset = new Rect (x-tw/2, y-th/2-tipDownPixel,tw,th);
		//if (tipTexture != null)tipTexture.SetClipRect(new Rect (x-w/2, y-h/2,w,h),true);
	}
}