using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	private float	speed;
	private int	_framesCount = 15;
	private float	_dx;
	private float	_initScale;
	private float	_targetScale;
	private bool	_hasToBeDestroyed = false;
	private bool	_fade_away = false;
	private Vector4	s_p;

	void	Start () {
		_initScale = gameObject.transform.localScale.x;
		_targetScale = _initScale * 1.6f;
		_dx = (_targetScale - _initScale) / _framesCount;
		speed = Random.Range (1.7f, 3.0f);
		s_p = gameObject.GetComponent<SpriteRenderer>().color;
	}

	void OnBecameInvisible() {
		Destroy (gameObject);
	}

	public void	fade_away() {
		_fade_away = true;
	}

	public void	switch_status() {
		_hasToBeDestroyed = true;
	}

	void	Update () {
		if (_hasToBeDestroyed) {
			_initScale += _dx;
			s_p.w -= 0.1f;
			if (_initScale > _targetScale) {
				_initScale = _targetScale;
				Destroy (gameObject);
			}
			gameObject.GetComponent<SpriteRenderer> ().color = s_p;
			gameObject.transform.localScale = Vector3.one * _initScale;
		}
		else {
			if (_fade_away) {
				s_p.w -= (0.5f / 10.0f) * speed;
				gameObject.GetComponent<SpriteRenderer> ().color = s_p;
			}
			float dist = Time.deltaTime * speed;
			transform.Translate (0, -dist, 0);
		}
	}
}
