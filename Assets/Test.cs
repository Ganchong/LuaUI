using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Test : MonoBehaviour {

	public Sprite tes;
	void Awake()
	{
		init ();
	}
	// Use this for initialization
	[ContextMenu("一键引用(数组引用例外)")]
	void init () {
		Image image = GetComponent<Image> ();
		var atlas = Resources.Load<SpriteAtlas> ("LoginWindow");
		tes = atlas.GetSprite ("button2");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void test(System.Action action)
	{
		action();
	}


}
