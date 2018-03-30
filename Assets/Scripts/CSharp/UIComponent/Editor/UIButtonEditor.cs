using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(UIButton),true)]
[CanEditMultipleObjects]
public class UIButtonEditor : ButtonEditor {

	public override void OnInspectorGUI ()
	{
		GUILayout.BeginVertical("box");
		serializedObject.Update();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("EnableClickSound"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("IgnoreClickInterval"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("ClickSoundPath"));
		serializedObject.ApplyModifiedProperties();
		GUILayout.EndVertical();

		GUILayout.Space(10);

		base.OnInspectorGUI();
	}
}
