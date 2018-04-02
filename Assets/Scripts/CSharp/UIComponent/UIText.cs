﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIText : Text
{
	
	public static Action<string, UIText> OnAwake = null;
	/** 字间距 */
	[HideInInspector]
	public float m_spacing = 0f;
	/** 间距是否改变 */

	protected override void Awake ()
	{
		base.Awake ();
		if (string.IsNullOrEmpty (text)) {
			if (Application.isPlaying && OnAwake != null) {
				OnAwake (text.Replace ("@", ""), this);			
			}
		}
	}
	public override string text {
		set {
			base.text = value;
		}
		get {
			return base.m_Text;
		}
	}
	public float spacing {
		get { return m_spacing; }
		set {
			if (m_spacing == value)
				return;
			m_spacing = value;
			this.SetVerticesDirty ();
		}
	}

	protected override void OnPopulateMesh (VertexHelper toFill)
	{
		base.OnPopulateMesh (toFill);
		//if(!m_spacingChanged)return;
		int count = toFill.currentVertCount;
		if (!IsActive ())return;

		float alignmentFactor = 0;

		switch (alignment) {
		case TextAnchor.LowerLeft:
		case TextAnchor.MiddleLeft:
		case TextAnchor.UpperLeft:
			alignmentFactor = 0f;
			break;
		case TextAnchor.LowerCenter:
		case TextAnchor.MiddleCenter:
		case TextAnchor.UpperCenter:
			alignmentFactor = 0.5f;
			break;
		case TextAnchor.LowerRight:
		case TextAnchor.MiddleRight:
		case TextAnchor.UpperRight:
			alignmentFactor = 1f;
			break;
		}
		string[] lines = text.Split ('\n');
		UIVertex tmp = new UIVertex();
		Vector3 pos;
		float letterOffset = spacing * (float)fontSize / 100f;
		int glyphIdx = 0;

		for (int lineIdx = 0; lineIdx < lines.Length; lineIdx++) {
			string line = lines [lineIdx];
			float lineOffset = (line.Length - 1) * letterOffset * alignmentFactor;

			for (int charIdx = 0; charIdx < line.Length; charIdx++) {
				int idx1 = glyphIdx * 4 + 0;
				int idx2 = glyphIdx * 4 + 1;
				int idx3 = glyphIdx * 4 + 2;
				int idx4 = glyphIdx * 4 + 3;

				if (idx4 > count - 1)
					return;
				toFill.PopulateUIVertex(ref tmp,idx1);
				UIVertex vert1 = tmp;
				toFill.PopulateUIVertex(ref tmp,idx2);
				UIVertex vert2 = tmp;
				toFill.PopulateUIVertex(ref tmp,idx3);
				UIVertex vert3 = tmp;
				toFill.PopulateUIVertex(ref tmp,idx4);
				UIVertex vert4 = tmp;

				pos = Vector3.right * (letterOffset * charIdx - lineOffset);

				vert1.position += pos;
				vert2.position += pos;
				vert3.position += pos;
				vert4.position += pos;

				toFill.SetUIVertex(vert1,glyphIdx*4);
				toFill.SetUIVertex(vert2,glyphIdx*4+1);
				toFill.SetUIVertex(vert3,glyphIdx*4+2);
				toFill.SetUIVertex(vert4,glyphIdx*4+3);

				glyphIdx++;
			}
			//回车符占的6个点
			glyphIdx++;
		}
	}
}
