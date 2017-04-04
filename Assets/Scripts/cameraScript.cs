using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour {

	/* public attributes start */
	public int	_offsetToScreenEdge = 50;

	public float	_speedOfCamera = 5.0f;

	public Transform[]	_cameraLimits = new Transform[4];

	public Texture2D	_cursorTexture;
	/* public attributes end */

	/* private attributes start */
	private int	_width;
	private int	_height;

	private float	_camSize;

	private bool	_globalView = false;

	private Vector3	_saveCamPos;
	/* private attributes end */

	void			Start() {
		_camSize = Camera.main.orthographicSize;
		_width = Screen.width;
		_height = Screen.height;
		Cursor.SetCursor (_cursorTexture, Vector2.zero, CursorMode.Auto);
	}

	private void	_setGlobalView () {
		transform.position = new Vector3 (0.5f, 0, -10);
		Camera.main.orthographicSize = 14.65f;
	}

	void			Update() {
		if (_globalView == false) {
			float delta = Input.GetAxis ("Mouse ScrollWheel");
			if (delta > 0)
				_speedOfCamera += delta;
			else if (delta < 0)
				_speedOfCamera -= delta;
			_speedOfCamera = Mathf.Clamp (_speedOfCamera, 0.0f, 20.0f);
			if (transform.position.y + _camSize + (_speedOfCamera * Time.deltaTime) < _cameraLimits[0].position.y && Input.mousePosition.y > _height - _offsetToScreenEdge)
				transform.Translate (0, _speedOfCamera * Time.deltaTime, 0);
			if (transform.position.y - _camSize - (_speedOfCamera * Time.deltaTime) > _cameraLimits[1].position.y && Input.mousePosition.y < _offsetToScreenEdge)
				transform.Translate (0, -_speedOfCamera * Time.deltaTime, 0);
			if (transform.position.x + (_camSize * Camera.main.aspect) + (_speedOfCamera * Time.deltaTime) < _cameraLimits[2].position.x && Input.mousePosition.x > _width - _offsetToScreenEdge)
				transform.Translate (_speedOfCamera * Time.deltaTime, 0, 0);
			if (transform.position.x - (_camSize * Camera.main.aspect) - (_speedOfCamera * Time.deltaTime) > _cameraLimits[3].position.x && Input.mousePosition.x < _offsetToScreenEdge)
				transform.Translate (-_speedOfCamera * Time.deltaTime, 0, 0);
			_saveCamPos = transform.position;
		}
		if (Input.GetKey (KeyCode.KeypadMinus) && _camSize + 0.05f < 14.65f) {
			_camSize += 0.05f;
			Camera.main.orthographicSize = _camSize;
		}
		if (Input.GetKey (KeyCode.KeypadPlus) && _camSize - 0.05f > 2.0f) {
			_camSize -= 0.05f;
			Camera.main.orthographicSize = _camSize;
		}
		if (Input.GetKey ("left ctrl")) {
			_globalView = true;
			_setGlobalView ();
		} else if (_globalView) {
			transform.position = _saveCamPos;
			Camera.main.orthographicSize = _camSize;
			_globalView = false;
		}
	}
}
