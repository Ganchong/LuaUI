using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UV frame animation.
/// </summary>
public class UVFrameAnimation : MonoBehaviour,IUVFrameProcess
{
	
	/** 每行图个数 */
	public int tileX = 1;
	/** 每列图个数 */
	public int tileY = 1;
	/** 目标帧数 */
	public int framerate = 16;
	/** 刷新帧率 */
	private float secPerFrame;
	/** 刷新时间 */
	private float cd = 0;
	/** 时间 */
	private float _time;
	/** 当前下标 */
	private int _currentIndex = 0;
	/** 渲染 */
	private Renderer _render;
	/** 偏移向量 */
	private List<Vector2> offsetArray;

	void Awake ()
	{
		_render = GetComponent<Renderer> ();
		_render.enabled = false;
	}

	void Start ()
	{
		_time = 0;
		_currentIndex = 0;
		offsetArray = new List<Vector2> ();
		secPerFrame = 1 / (float)framerate;

		if (Application.isPlaying)
			_render.material.SetTextureScale ("_MainTex", new Vector2 (1 / (float)tileX, 1 / (float)tileY));
		else
			_render.sharedMaterial.SetTextureScale ("_MainTex", new Vector2 (1 / (float)tileX, 1 / (float)tileY));

		for (int j = 0; j < tileY; j++) {
			for (int i = 0; i < tileX; i++) {
				offsetArray.Add (new Vector2 (i / (float)tileX, 1 - (j + 1) / (float)tileY));
			}
		}
		_render.enabled = true;
	}

	void Update ()
	{
		if (_render == null)
			return;

		Process (_time);
		_time += Time.deltaTime;
	}

	public void Process (float time)
	{
		if (cd >= secPerFrame) {
			_render.material.SetTextureOffset ("_MainTex", offsetArray [_currentIndex]);
			_currentIndex++;
			if (_currentIndex >= offsetArray.Count) {
				_currentIndex = 0;
			}
			cd = 0;
		}
		cd += Time.deltaTime;
	}
}
