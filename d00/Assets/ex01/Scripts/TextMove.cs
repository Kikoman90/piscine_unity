using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMove : MonoBehaviour {

	private int	_framesCount = 15;

	private float	_speed = 300.0f;
	private float	_dx_scale;
	private float	_initScale;
	private float	_targetScale;
	private float	_dx_pos;
	private float	_initPos;
	private float	_targetPos;

	private bool	_init = true;
	private bool	_move = false;
	private bool	_destroy = false;

	private Vector4	s_p;

	void	Start () {
		_initScale = gameObject.transform.localScale.x;
		_targetScale = _initScale * 0.3f;
		_dx_scale = (_targetScale - _initScale) / _framesCount;
		s_p = gameObject.GetComponent<Text>().color;
	}

	public void	move_up() {
		_initPos = transform.localPosition.y;
		_targetPos = _initPos + 1.5f;
		_dx_pos = (_targetPos - _initPos) / _framesCount;
		_move = true;
	}

	public void fade_n_destroy() {
		_destroy = true;
	}

	void	Update () {
		if (_init) {
			_initScale += _dx_scale;
			if (s_p.w < 1.0f)
				s_p.w += 0.029f;
			else
				_init = false;
			if (_initScale < _targetScale) {
				_initScale = _targetScale;
				_init = false;
			}
			gameObject.GetComponent<Text> ().color = s_p;
			gameObject.transform.localScale = Vector3.one * _initScale;
		}
		if (_move) {
			_initPos += _dx_pos;
			if (_initPos > _targetPos)
				_move = false;
			float dist = Time.deltaTime * _speed;
			transform.Translate (0, dist, 0);
		}
		if (_destroy) {
			GameObject.Destroy(gameObject);
		}
	}
}
