using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	string tes;
	// Use this for initialization
	void Start () {
		int ss = 0;
		test(()=>{
			Debug.Log("汪总是傻逼");
			tes = 1.ToString();
		});
		test(()=>{
			ss = 1;
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void test(System.Action action)
	{
		action();
	}


}
