using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ABSystem;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace UGUIFrame
{
	[CustomEditor(typeof(UIImage))]
	public class UIImageEditor : Editor {

		const string mainPath = "Assets/Art/Atlas/";
		const string spriteDirectory = "texture";
		const string atlasSuffix = ".spriteatlas";

		UIImage uiImage;
		private SerializedObject obj;
		private SerializedProperty sourceSprite;
		private SerializedProperty atlasName;
		private SerializedProperty spriteName;
		private Object oldReferenceObj;
		private string oldAtlasName;
		private string oldSpriteName;

		void OnEnable()
		{
			uiImage = (UIImage)target;
			obj = new SerializedObject (target);
			sourceSprite = obj.FindProperty ("sourceSprite");
			atlasName = obj.FindProperty ("atlasName");
			spriteName = obj.FindProperty ("spriteName");
			oldReferenceObj = sourceSprite.objectReferenceValue;
			oldAtlasName = atlasName.stringValue;
			oldSpriteName = spriteName.stringValue;
		}

		public override void OnInspectorGUI ()
		{
			GUILayout.BeginVertical("box");
			EditorGUILayout.PropertyField (sourceSprite);
			EditorGUILayout.PropertyField (atlasName);
			EditorGUILayout.PropertyField (spriteName);
			Object referenceObj = sourceSprite.objectReferenceValue;
			if (referenceObj == null) {
				if (oldReferenceObj != null) {
					oldReferenceObj = null;
					uiImage.atlasName = null;
					uiImage.spriteName = null;
					uiImage.sprite = null;
				}

			}else if (!referenceObj.Equals (oldReferenceObj)) {
				oldReferenceObj = referenceObj;
				uiImage.sprite = sourceSprite.objectReferenceValue as Sprite;
				string path = AssetDatabase.GetAssetPath (referenceObj);
				int index = path.LastIndexOf (spriteDirectory);
				if (index < 0 || path.Length < index) {
					uiImage.atlasName = null;
					uiImage.spriteName = null;
					atlasName.stringValue = null;
					spriteName.stringValue = null;
				} else {
					string atlasname = path.Substring (0,index-1).Replace(mainPath,"");
					string atlaspath = mainPath+atlasname+"/"+atlasname+atlasSuffix;
					SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlaspath);
					if (atlas != null) {
						Sprite tmpsprite = atlas.GetSprite (referenceObj.name);
						if (tmpsprite != null) {
							uiImage.sprite = tmpsprite;
							atlasName.stringValue = atlasname;
							spriteName.stringValue = referenceObj.name;
						}
					}
				}
			}

			string nowAtlasName = atlasName.stringValue;
			if (string.IsNullOrEmpty (nowAtlasName)) {
				if (!string.IsNullOrEmpty (oldAtlasName)) {
					oldAtlasName = "";
					uiImage.sprite = null;
					atlasName.stringValue = null;
					spriteName.stringValue = null;
				}
			} else if(nowAtlasName!=oldAtlasName){
				oldAtlasName = nowAtlasName;
				string atlaspath = mainPath + nowAtlasName + "/" + nowAtlasName + atlasSuffix;
				SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlaspath);
				if (atlas == null) {
					uiImage.sprite = null;
					uiImage.atlasName = nowAtlasName;
				} else {
					Sprite tmpSprite = atlas.GetSprite (spriteName.stringValue);
					uiImage.sprite = tmpSprite;
					uiImage.atlasName = nowAtlasName;
				}
			}

			string nowSpriteName = spriteName.stringValue;
			if (string.IsNullOrEmpty (nowSpriteName)) {
				if (!string.IsNullOrEmpty (oldSpriteName)) {
					oldSpriteName = "";
					uiImage.sprite = null;
					spriteName.stringValue = null;
				}
			} else if(nowSpriteName!=oldSpriteName){
				oldSpriteName = nowSpriteName;
				string atlaspath = mainPath + nowAtlasName + "/" + nowAtlasName + atlasSuffix;
				SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlaspath);
				if (atlas == null) {
					uiImage.sprite = null;
				} else {
					Sprite tmpSprite = atlas.GetSprite (nowSpriteName);
					uiImage.sprite = tmpSprite;
					uiImage.spriteName = nowSpriteName;
				}
			}

			GUILayout.EndVertical();
			if (GUI.changed) {
				EditorUtility.SetDirty(target);
			}
			base.OnInspectorGUI ();
		}
	}

}

