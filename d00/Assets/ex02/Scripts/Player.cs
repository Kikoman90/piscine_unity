using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public GameObject	club_prefab;
	public GameObject	ball_prefab;

	private GameObject	_club;
	private GameObject	_ball;

	private Transform	_hole;
	private Transform	_top_limit;
	private Transform	_bottom_limit;

	private string	_name;

	private float	_prevYpos;
	private float	_power;

	private int		_score = -15;
	private int		_direction = 1;
	private int		_ball_direction = 1;

	private uint	_framesToWait = 20;

	private bool	_ballInHole = false;
	private bool	_ballIsMoving = false;

	public void		InitPlayer(string setName, Transform hole, Transform top_limit, Transform bottom_limit) {
		_club = Instantiate (club_prefab, transform.position, Quaternion.identity);
		_ball = Instantiate (ball_prefab, transform.position, Quaternion.identity);
		_name = setName;
		_refreshName ();
		_hole = hole;
		_top_limit = top_limit;
		_bottom_limit = bottom_limit;
		_translateClubToBall ();
		switchRender ();
	}

	private void	_translateClubToBall() {
		Vector3	rev_scale = new Vector3 (_club.transform.localScale.x, -(_club.transform.localScale.y), _club.transform.localScale.z);
		if (_ball.transform.position.y > _hole.position.y && _direction == 1) {
			_direction = -1;
			_club.transform.localScale = rev_scale;
		} else if (_ball.transform.position.y < _hole.position.y && _direction == -1) {
			_direction = 1;
			_club.transform.localScale = rev_scale;
		}
		_club.transform.position = new Vector3 (_ball.transform.position.x - 0.24f, _ball.transform.position.y + (_direction * 0.16f) , _club.transform.position.z);
		_ball_direction = _direction;
		_prevYpos = _club.transform.position.y;
	}

	private void	_refreshName() {
		_ball.GetComponentInChildren<Text> ().text = _name + ": " + _score;
	}

	public int		getScore() {
		return (_score);
	}

	public string	getName() {
		return (_name);
	}

	public bool		ballMovement() {
		return (_ballIsMoving);
	}

	public bool		ballInHole() {
		return (_ballInHole);
	}
		
	public float	getBallYpos() {
		return (_ball.transform.position.y);
	}

	public void		switchRender() {
		SpriteRenderer	spRend;
		Text			text;
		Image			image;

		if ((spRend = _club.GetComponent<SpriteRenderer>()).enabled == true)
			spRend.enabled = false;
		else
			spRend.enabled = true;
		if ((spRend = _ball.GetComponent<SpriteRenderer>()).enabled == true)
			spRend.enabled = false;
		else
			spRend.enabled = true;
		if ((text = _ball.GetComponentInChildren<Text> ()).enabled == true)
			text.enabled = false;
		else
			text.enabled = true;
		if ((image = _ball.GetComponentInChildren<Image> ()).enabled == true)
			image.enabled = false;
		else
			image.enabled = true;
	}

	public void		shoot(float power) {
		float	curYpos = _club.transform.position.y;
		_club.transform.Translate (0, _prevYpos - curYpos, 0);
		_power = power;
		_score += 5;
		_refreshName ();
		_ballIsMoving = true;
	}

	public void		moveClub(bool powerUp) {
		if (powerUp == true)
			_club.transform.Translate (0, _direction * -(2.81f * Time.deltaTime), 0);
		else
			_club.transform.Translate (0, _direction * 2.81f * Time.deltaTime, 0);
	}

	public void		setPlayerActive() {

		CanvasRenderer[]	cvRend;

		Color tmp_clr = Color.HSVToRGB (0, 0, 1.0f);
		tmp_clr.a = 1.0f;
		_club.GetComponent<SpriteRenderer> ().color = tmp_clr;
		_ball.GetComponent<SpriteRenderer> ().color = tmp_clr;
		cvRend = _ball.GetComponentsInChildren<CanvasRenderer> ();
		foreach (CanvasRenderer cv in cvRend)
			cv.SetAlpha (1.0f);
	}

	public void		setPlayerInactive() {
		CanvasRenderer[]	cvRend;

		Color tmp_clr = Color.HSVToRGB (0, 0, 0.773f);
		tmp_clr.a = 0.604f;
		_club.GetComponent<SpriteRenderer> ().color = tmp_clr;
		_ball.GetComponent<SpriteRenderer> ().color = tmp_clr;
		cvRend = _ball.GetComponentsInChildren<CanvasRenderer> ();
		foreach (CanvasRenderer cv in cvRend)
			cv.SetAlpha (0.0f);
	}

	public void		Die() {
		GameObject.DestroyImmediate (_ball);
		GameObject.DestroyImmediate (_club);
		GameObject.DestroyImmediate (gameObject);
	}

	void 			Update () {
		if (_ballIsMoving) {
			_ball.transform.Translate (0, _ball_direction * 50.0f * Time.deltaTime * _power, 0);
			if (_ball.transform.position.y >= _hole.transform.position.y - (_hole.transform.localScale.y / 2) && _ball.transform.position.y <= _hole.transform.position.y + (_hole.transform.localScale.y / 2) && _power < 0.2f) {
				_ballInHole = true;
				_ballIsMoving = false;
			}
			else if (_ball.transform.position.y > _top_limit.position.y - (_ball.transform.localScale.y / 2)) {
				Vector3 revBall = new Vector3 (_ball.transform.position.x, _top_limit.position.y - (_ball.transform.localScale.y / 2), _ball.transform.position.z);
				_ball.transform.position = revBall;
				_ball_direction = -1;
			}
			else if (_ball.transform.position.y < _bottom_limit.position.y + (_ball.transform.localScale.y / 2)) {
				Vector3 revBall = new Vector3 (_ball.transform.position.x, _bottom_limit.position.y + (_ball.transform.localScale.y / 2), _ball.transform.position.z);
				_ball.transform.position = revBall;
				_ball_direction = 1;
			}
			if (_power > 0.0f)
				_power = (_power - 0.02f < 0.0f) ? 0.0f : _power - 0.02f;
			else {
				_power = 0.0f;
				_translateClubToBall ();
				_framesToWait--;
				if (_framesToWait == 0) {
					_framesToWait = 20;
					_ballIsMoving = false;
				}
			}
		}
	}
}
