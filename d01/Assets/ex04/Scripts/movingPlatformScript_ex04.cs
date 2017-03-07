using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class movingPlatformScript_ex04 : MonoBehaviour {

	[Tooltip("horizontal distance in units")]
	public float	horizontalDistance = 0;
	[Tooltip("vertical distance in units")]
	public float	verticalDistance = 0;

	[Tooltip("time it takes to translate the platform over the specified distance")]
	[Range(1.0f, 5.0f)]
	public float	time = 1.0f;

	public bool	IsTriggered = false;

	private float	_timer = 0;

	private bool	_movePositive = true;

	private Vector3	_vecMove;

	private Rigidbody2D	_platformRB;

	private GameObject	_player = null;

	void Start () {
		_platformRB = GetComponent<Rigidbody2D>();
		_platformRB.isKinematic = true;
		_vecMove = new Vector3 (horizontalDistance / time, verticalDistance / time, 0);
	}

	void	OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "player")
			IsTriggered = false;
	}

	void	OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "player")
			_player = coll.gameObject;
	}

	void	OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "player")
			_player = null;
	}

	void FixedUpdate () {
		if (IsTriggered == false) {
			if (_movePositive) {
				_platformRB.MovePosition (transform.position + _vecMove * Time.deltaTime);
				if (_player)
					_player.GetComponent<Rigidbody2D> ().velocity += new Vector2 (horizontalDistance / time, 0);
				if (_timer >= time) {
					_timer = 0;
					_movePositive = false;
				}
				_timer += Time.fixedDeltaTime;
			} else {
				_platformRB.MovePosition (transform.position - _vecMove * Time.deltaTime);
				if (_player)
					_player.GetComponent<Rigidbody2D> ().velocity += new Vector2 (-horizontalDistance / time, 0);
				if (_timer >= time) {
					_timer = 0;
					_movePositive = true;
				}
				_timer += Time.fixedDeltaTime;
			}
		}
	}
}
