using System;
using UnityEngine;
using UnityEngine.UI;
using LuaFrameworkCore;

public class UIRawImage : RawImage,IGrayMember
{
	/** static  */
	/** 图片加载方法 */
	public static Action<string,UIRawImage,CallBack<UIRawImage>> LoadTextureFunc = null;
	[Tooltip("当前路径")]
	public string nowPath;
	/** 带有alpha通道的分离贴图，如果是ETC2就不用了 */
	[HideInInspector]
	public Texture alphaTex;
	/** 是否是灰色 */
	private bool _isGray;
	/** 旧的颜色 */
	private Color oldColor;

	protected override void Awake ()
	{
		base.Awake ();
		if (this.texture == null) {
			Alpha = 0;
		}
	}
	/** 默认加载 */
	public void LoadImage(CallBack<UIRawImage> call = null)
	{
		LoadImage(nowPath,call);
	}
	/** 加载图片 */
	public void LoadImage (string Path, CallBack<UIRawImage> call = null)
	{
		nowPath = Path;
		if (string.IsNullOrEmpty (Path)) {
			Alpha = 0;
			return;
		}
		if(LoadTextureFunc!=null){
			LoadTextureFunc(Path,this,call);
		}
	}
	/** 置灰 */
	public void SetGray (bool isGray)
	{
		if (_isGray == isGray)
			return;
		switch (material.shader.name) {
		case "UI/Default":

			if (GrayManager.DefImageMaterial == null)
			{
				Debug.LogWarning("AorRawImage.setGray :: can not find the Shader<Custom/RawImage/RawImage Gray>");
				return;
			}
			material = GrayManager.DefImageMaterial;
			SetMaterialDirty();
			break;

		}
		_isGray = isGray;
		if (isGray) {
			oldColor = color;
			color = new Color (0, 0, 0, color.a);
		} else {
			color = new Color (oldColor.r, oldColor.g, oldColor.b, Alpha);
		}
	}

	public bool IsGray {
		get {
			return _isGray;
		}
	}
	public float Alpha {
		get { return color.a; }
		set {
			Color n = color;
			n.a = Mathf.Clamp (value, 0, 1);
			color = n;
		}
	}

}
