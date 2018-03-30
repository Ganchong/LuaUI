using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIImage : Image,IGrayMember {

	public static Action<object[], UIImage, Action<UIImage>> LoadSprite;
	public Action<object[], UIImage, Action<UIImage>> CustomLoadSpriteFunc;

	public bool CanRaycast = true;
	private Collider2D collider;

	public object textureRef;
	public object materialRef;

	public object[] data = new object[3];

	public void LoadImage(string Path, string spName, Action<UIImage> call = null, string materialPath = "")
	{
		data[0] = Path;
		data[1] = spName;
		data[2] = materialPath;
		if (null != LoadSprite)
		{
			LoadSprite(data, this, call);
			return;
		}
		if (null != CustomLoadSpriteFunc)
		{
			CustomLoadSpriteFunc(data, this, call);
			return;
		}
		if (!string.IsNullOrEmpty(materialPath))
		{
			Material mt = Resources.Load<Material>(materialPath);
			if (mt != null)
			{
				material = mt;
			}
		}
		Sprite[] sprites = Resources.LoadAll<Sprite>(Path);
		if (sprites != null && sprites.Length > 0)
		{
			int i, len = sprites.Length;
			for (i = 0; i < len; i++)
			{
				if (sprites[i].name == spName)
				{
					sprite = sprites[i];

					if (null != call)
					{
						call(this);
					}
					return;
				}
			}
		}
	}

	private bool _isGray;

	public bool IsGray
	{
		get
		{
			return _isGray;
		}
	}
	private Color oldColor;

	/// <summary>
	/// 变灰
	/// </summary>
	/// <param name="button"></param>
	/// <param name="bo"></param>
	public  void SetGray(bool isGray)
	{
		if (_isGray == isGray)
			return;
		switch (material.shader.name)
		{
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
		if (isGray)
		{
			oldColor = color;
			color = new Color(0, 0, 0, color.a);
		}
		else
		{
			color = new Color(oldColor.r, oldColor.g, oldColor.b, Alpha);
		}
	}

	protected override void Awake()
	{
		base.Awake();

		if (sprite == null)
		{
			this.Alpha = 0;
		}
	}

	protected override void Start()
	{

		Collider2D collider2D = GetComponent<Collider2D>();
		if (collider2D != null)
		{
			setCollider(collider2D);
		}

		base.Start();
	}


	public float Alpha
	{
		get
		{
			return color.a;
		}
		set
		{
			Color n = color;
			n.a = Mathf.Clamp(value, 0, 1);
			color = n;
		}
	}

	public void setCollider(Collider2D collider2D)
	{
		collider = collider2D;
	}

	override public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		if (!CanRaycast)
		{
			return false;
		}
		else if (collider != null)
		{
			var worldPoint = Vector3.zero;
			var isInside = RectTransformUtility.ScreenPointToWorldPointInRectangle(
				rectTransform,
				sp,
				eventCamera,
				out worldPoint
			);
			if (isInside)
				isInside = collider.OverlapPoint(worldPoint);
			return isInside;
		}
		else
		{
			return base.IsRaycastLocationValid(sp, eventCamera);
		}
	}

}
