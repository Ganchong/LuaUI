using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UGUIFrame
{
	[CustomEditor(typeof(UIRawImage))]
	public class UIRawImageEditor : Editor {

		UIRawImage uiRawImage;

		void OnEnable()
		{
			uiRawImage = (UIRawImage)target;
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
		}
	}

}
