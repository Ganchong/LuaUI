using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UGUIFrame
{
	public class UIImage : Image,IGrayMember
	{
		/** 加载精灵方法*/
		public static Action<string,string, UIImage, Action<UIImage>> LoadSprite;
		/** 是否接受射线检测*/
		public bool CanRaycast = true;
		/** 碰撞体 */
		private Collider2D collider;
		[HideInInspector]
		public Sprite sourceSprite;
		[HideInInspector]
		public string atlasName;
		[HideInInspector]
		public string spriteName;
		private bool _isGray;
		private Color oldColor;


		//[ContextMenu("excute")]
		protected override void Awake ()
		{
			base.Awake ();
			if (!string.IsNullOrEmpty (atlasName)) {
				#if UNITY_EDITOR
				string mainPath = "Assets/Art/Atlas/";
				UnityEngine.U2D.SpriteAtlas atlas = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.U2D.SpriteAtlas> (mainPath + atlasName + "/" + atlasName + ".spriteatlas");
				if (atlas != null) {
					sprite = atlas.GetSprite (spriteName);
				}
				#else
				LoadImage (atlasName,spriteName,null);
				#endif
			}
			if (sprite == null) {
				this.Alpha = 0;
			}
		}

		protected override void Start ()
		{

			Collider2D collider2D = GetComponent<Collider2D> ();
			if (collider2D != null) {
				setCollider (collider2D);
			}

			base.Start ();
		}

		public void LoadImage (string atlasName, string spName, Action<UIImage> call = null)
		{
			if (null != LoadSprite) {
				LoadSprite (atlasName, spName, this, call);
			}
		}

		/// <summary>
		/// 变灰
		/// </summary>
		/// <param name="button"></param>
		/// <param name="bo"></param>
		public  void SetGray (bool isGray)
		{
			if (_isGray == isGray)
				return;
			switch (material.shader.name) {
			case "UI/Default":

				if (GrayManager.DefImageMaterial == null) {
					Debug.LogWarning ("AorRawImage.setGray :: can not find the Shader<Custom/RawImage/RawImage Gray>");
					return;
				}
				material = GrayManager.DefImageMaterial;
				SetMaterialDirty ();
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

		public void setCollider (Collider2D collider2D)
		{
			collider = collider2D;
		}

		override public bool IsRaycastLocationValid (Vector2 sp, Camera eventCamera)
		{
			if (!CanRaycast) {
				return false;
			} else if (collider != null) {
				var worldPoint = Vector3.zero;
				var isInside = RectTransformUtility.ScreenPointToWorldPointInRectangle (
					               rectTransform,
					               sp,
					               eventCamera,
					               out worldPoint
				               );
				if (isInside)
					isInside = collider.OverlapPoint (worldPoint);
				return isInside;
			} else {
				return base.IsRaycastLocationValid (sp, eventCamera);
			}
		}

		public float Alpha {
			get {
				return color.a;
			}
			set {
				Color n = color;
				n.a = Mathf.Clamp (value, 0, 1);
				color = n;
			}
		}

		public bool IsGray {
			get {
				return _isGray;
			}
		}
	}
}
