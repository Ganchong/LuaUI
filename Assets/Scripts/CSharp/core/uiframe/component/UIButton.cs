using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UGUIFrame
{
	public class UIButton : Button,IGrayMember {
		/** 按钮点击音效 */
		public static Action<string> OnSoundPlay;
		/** 是否Disable所有按钮 */
		public static bool IsDisableAllBtn = false;
		/** 是否播放音效 */
		public bool EnableClickSound = true;
		/** 是否忽略点击间隔 */
		public bool IgnoreClickInterval = false;
		/** 音效路径 */
		public string ClickSoundPath = "Audio/SimpleButton";
		/** 点击间隔 */
		private float ClickInterval = 0.3f;
		/** 上次点击时间 */
		public static float lastClickTime = -1;

		public override void OnPointerClick(PointerEventData eventData)
		{
			if (IsDisableAllBtn) return;
			if (IgnoreClickInterval || Time.realtimeSinceStartup - lastClickTime >= ClickInterval)
			{
				base.OnPointerClick(eventData);
				lastClickTime = Time.realtimeSinceStartup;

				if (EnableClickSound && !string.IsNullOrEmpty(ClickSoundPath) && OnSoundPlay != null)
					OnSoundPlay(ClickSoundPath);
			}

		}
		public bool IsGray
		{
			get
			{
				if (targetGraphic != null&&targetGraphic is IGrayMember)
				{
					return ((IGrayMember)targetGraphic).IsGray;
				}
				return false;
			}
		}

		public void SetGrayWithInteractable(bool bo)
		{
			interactable = !bo;
			SetGray(bo);
		}

		/// <summary>
		/// 变灰
		/// </summary>
		public void SetGray(bool bo)
		{
			if (targetGraphic != null&& targetGraphic is IGrayMember)
				((IGrayMember)targetGraphic).SetGray(bo);
			GrayManager.SetGaryWithAllChildren(transform, bo);
		}
	}

}
