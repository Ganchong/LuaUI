using UnityEditor;
using UnityEngine;

namespace UGUIFrame
{
	[CustomEditor(typeof(UIText))]
	public class UITextEditor : Editor {

		public override void OnInspectorGUI ()
		{
			GUILayout.BeginVertical("box");
			serializedObject.Update();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Text"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_spacing"));
			serializedObject.ApplyModifiedProperties();
			GUILayout.EndVertical();
			GUILayout.BeginVertical("box");
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FontData"));
			GUILayout.EndVertical();
			GUILayout.BeginVertical("box");
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Color"));
			GUILayout.EndVertical();
			GUILayout.BeginVertical("box");
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Material"));
			GUILayout.EndVertical();
			GUILayout.BeginVertical("box");
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_RaycastTarget"));
			GUILayout.EndVertical();
			serializedObject.ApplyModifiedProperties();
		}
	}

}
