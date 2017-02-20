using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonus_bunny : MonoBehaviour {

	private int	_incFrame = 0;
	private int	_totalFrames = 0;

	private float	_x_dir = 0.0f;
	private float	_y_dir = 0.0f;

	private Vector3	_positive_scale = new Vector3(1.0f, 1.0f, 1.0f);
	private Vector3	_negative_scale = new Vector3(-1.0f, 1.0f, 1.0f);

	private bool	_bunny_activated = false;

	public void	Activate() {
		_bunny_activated = true;
		GetComponent<SpriteRenderer> ().sortingOrder = 3;
	}

	void Update () {
		if (_bunny_activated) {
			gameObject.transform.Translate (4.0f * _x_dir * Time.deltaTime, 4.0f * _y_dir * Time.deltaTime, 0);
			if (gameObject.transform.position.x > 10.0f || gameObject.transform.position.x < -10.0f
				|| gameObject.transform.position.y > 8.0f || gameObject.transform.position.y < -8.0f)
				_incFrame = _totalFrames;
			if (_incFrame >= _totalFrames) {
				_incFrame = 0;
				_totalFrames = Random.Range (100, 250);
				if (gameObject.transform.position.x > 10.0f)
					_x_dir = Random.Range (-1.0f, 0.0f);
				else if (gameObject.transform.position.x < -10.0f)
					_x_dir = Random.Range (0.0f, 1.0f);
				else
					_x_dir = Random.Range (-1.0f, 1.0f);
				if (gameObject.transform.position.y > 8.0f)
					_y_dir = Random.Range (-1.0f, 0.0f);
				else if (gameObject.transform.position.y < -8.0f)
					_y_dir = Random.Range (0.0f, 1.0f);
				else
					_y_dir = Random.Range (-1.0f, 1.0f);
				if (_x_dir < 0.0f)
					transform.localScale = _negative_scale;
				else
					transform.localScale = _positive_scale;
			}
			_incFrame++;
		}
	}
}
