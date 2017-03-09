using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript_ex02 : MonoBehaviour {

	public GameObject[]	players;

	public GameObject	platforms;

	private int		_index = 0;

	private float	_cam_size;

	private bool	_ctrlKey = false;

	void			Start() {
		_cam_size = Camera.main.orthographicSize;
		_setLayerTransparency (9, 10);
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

	private void	_setLayerTransparency(int layer_index1, int layer_index2) {
		foreach (Transform tr in platforms.transform) {
			if (tr.gameObject.layer == layer_index1 || tr.gameObject.layer == layer_index2) {
				Color clr = tr.GetComponent<SpriteRenderer> ().color;
				clr.a = 0.3f;
				tr.GetComponent<SpriteRenderer> ().color = clr;
			} else {
				Color clr = tr.GetComponent<SpriteRenderer>().color;
				clr.a = 1.0f;
				tr.GetComponent<SpriteRenderer>().color = clr;
			}
		}
	}

	void			Update () {
		if (Input.GetKeyDown ("1")) {
			_index = 0;
			_setLayerTransparency (9, 10);
		}
		if (Input.GetKeyDown ("2")) {
			_index = 1;
			_setLayerTransparency (8, 10);
		}
		if (Input.GetKeyDown ("3")) {
			_index = 2;
			_setLayerTransparency (8, 9);
		}
		if (Input.GetKeyDown ("r")) {
			_index = 0;
			_setLayerTransparency (9, 10);
		}
		playerScript_ex02.curIndex = _index;
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