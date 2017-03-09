using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_button : MonoBehaviour {

	public float	offset = 16.25f;

	public bool	permanent;

	private float	_const_x;
	private float	_y_hide;
	private float	_y_show;

	private bool	_hide = false;
	private bool	_show = false;

	void Start() {
		_const_x = GetComponent<RectTransform> ().anchoredPosition.x;
		_y_hide = GetComponent<RectTransform> ().anchoredPosition.y;
		_y_show = _y_hide + offset;
	}

	public void	ShowButton() {
		_hide = false;
		_show = true;
	}

	public void	HideButton() {
		_show = false;
		_hide = true;
	}

	void Update () {
		/* (60.0f * Time.deltaTime) */
		if (_show && permanent) {
			GetComponent<RectTransform> ().Translate (0, (offset / 400.0f), 0);
			if (offset >= 0.0f && GetComponent<RectTransform> ().anchoredPosition.y >= _y_show)
				GetComponent<RectTransform> ().anchoredPosition = new Vector2 (_const_x, _y_show);
			else if (offset < 0.0f && GetComponent<RectTransform> ().anchoredPosition.y <= _y_show)
				GetComponent<RectTransform> ().anchoredPosition = new Vector2 (_const_x, _y_show);
		}
		else if (_hide) {
			GetComponent<RectTransform> ().Translate (0, -(offset / 400.0f), 0);
			if (-offset >= 0.0f && GetComponent<RectTransform> ().anchoredPosition.y >= _y_hide)
				GetComponent<RectTransform> ().anchoredPosition = new Vector2 (_const_x, _y_hide);
			else if (-offset < 0.0f && GetComponent<RectTransform> ().anchoredPosition.y <= _y_hide)
				GetComponent<RectTransform> ().anchoredPosition = new Vector2 (_const_x, _y_hide);
		}
	}
}
