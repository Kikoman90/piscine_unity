﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript_ex01 : MonoBehaviour {

	public GameObject[]	players;

	private int		_index = 0;

	private float	_cam_size;

	private bool	_ctrlKey = false;

	void			Start() {
		_cam_size = Camera.main.orthographicSize;
	}

	private void	_globalView() {
		float[]	tab_x = new float[] {
			players [0].transform.position.x,
			players [1].transform.position.x,
			players [2].transform.position.x
		};
		float[]	tab_y = new float[] {
			players [0].transform.position.y,
			players [1].transform.position.y,
			players [2].transform.position.y
		};
		float offset = 0.0f;
		float height = (Mathf.Max (tab_y) - Mathf.Min (tab_y)) + players[1].transform.localScale.y + offset;
		float width = (Mathf.Max (tab_x) - Mathf.Min (tab_x)) + players[2].transform.localScale.x + offset;
		if (width > height * Camera.main.aspect)
			height = width / Camera.main.aspect;
		Camera.main.orthographicSize = height / 2;
		gameObject.transform.position = new Vector3 ((Mathf.Max(tab_x) + Mathf.Min(tab_x)) / 2.0f,
			(Mathf.Max(tab_y) + Mathf.Min(tab_y)) / 2.0f,
			gameObject.transform.position.z);
	}

	void			Update () {
		if (Input.GetKeyDown ("1"))
			_index = 0;
		if (Input.GetKeyDown ("2"))
			_index = 1;
		if (Input.GetKeyDown ("3"))
			_index = 2;
		if (Input.GetKeyDown ("r"))
			_index = 0;
		playerScript_ex01.curIndex = _index;
		if (Input.GetKey ("left shift")) {
			_ctrlKey = true;
			_globalView ();
		} else {
			_ctrlKey = false;
			Camera.main.orthographicSize = _cam_size;
		}
	}

	void			LateUpdate() {
		if (_ctrlKey == false) {
			gameObject.transform.position = new Vector3 (players [_index].transform.position.x,
				players [_index].transform.position.y + 1.0f,
				players [_index].transform.position.z - 1.0f);
		}
	}
}