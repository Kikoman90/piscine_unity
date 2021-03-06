﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerScript_ex05 : MonoBehaviour {

	public int	playerIndex = 0;

	public float	speed = 2.0f;
	public float	jump = 5.0f;

	public static int	curIndex = 0;

	public Transform	limits;

	private int	_collisionCount = 0;

	private float	_respawnTime = 0.8f;

	private bool	_respawn = true;
	private bool	_canJump = false;

	private Rigidbody2D	_rb;

	static bool[]	hasReachedExit = new bool[]{ false, false, false };

	private IEnumerator	_coroutine;

	void				Start() {
		_coroutine = _blink();
		StartCoroutine (_coroutine);
	}

	private IEnumerator	_blink() {
		while (_respawn) {
			Color	clr = GetComponent<SpriteRenderer> ().color;
			if (clr.a == 1.0f)
				clr.a = 0.5f;
			else
				clr.a = 1.0f;
			GetComponent<SpriteRenderer> ().color = clr;
			yield return new WaitForSeconds (.1f);
		}
	}

	void				OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "red_exit" && playerIndex == 0) {
			Color clr = new Color (0.12f, 1, 0.12f, 1);
			hasReachedExit [0] = true;
			coll.gameObject.GetComponent<SpriteRenderer> ().color = clr;
		} else if (coll.gameObject.tag == "yellow_exit" && playerIndex == 1) {
			Color clr = new Color (0.12f, 1, 0.12f, 1);
			hasReachedExit [1] = true;
			coll.gameObject.GetComponent<SpriteRenderer> ().color = clr;
		} else if (coll.gameObject.tag == "blue_exit" && playerIndex == 2) {
			Color clr = new Color (0.12f, 1, 0.12f, 1);
			hasReachedExit [2] = true;
			coll.gameObject.GetComponent<SpriteRenderer> ().color = clr;
		} else if (coll.gameObject.tag == "teleporter") {
			Transform[] transform_tp = coll.gameObject.GetComponentsInChildren<Transform> ();
			gameObject.transform.position = transform_tp [1].position;
		} else if (coll.gameObject.tag == "trap")
			resetLevel ();
	}

	void				OnTriggerExit2D(Collider2D coll) {
		Color clr = new Color(1, 1, 1, 1);
		if (coll.gameObject.tag == "red_exit" && playerIndex == 0) {
			hasReachedExit [0] = false;
			coll.gameObject.GetComponent<SpriteRenderer> ().color = clr;
		} else if (coll.gameObject.tag == "yellow_exit" && playerIndex == 1) {
			hasReachedExit [1] = false;
			coll.gameObject.GetComponent<SpriteRenderer> ().color = clr;
		} else if (coll.gameObject.tag == "blue_exit" && playerIndex == 2) {
			hasReachedExit [2] = false;
			coll.gameObject.GetComponent<SpriteRenderer> ().color = clr;
		}
	}

	void				OnCollisionEnter2D(Collision2D coll) {
		if (coll.enabled && coll.contacts.Length > 0) {
			_collisionCount++;
			ContactPoint2D contact = coll.contacts [0];
			if (Vector2.Dot (contact.normal, Vector2.up) > 0.5)
				_canJump = true;
		}
	}

	void				OnCollisionExit2D(Collision2D coll) {
		_collisionCount--;
		if (coll.enabled && _collisionCount == 0)
			_canJump = false;

	}

	public void			resetLevel() {
		SceneManager.LoadScene ("ex05");
	}

	public void			loadNextLevel() {
		SceneManager.LoadScene ("ex00");
	}

	void				Update() {
		if (gameObject.transform.position.y < limits.position.y || Input.GetKeyDown ("r"))
			resetLevel ();
		if (_respawn) {
			if (_respawnTime <= 0) {
				Color	clr = GetComponent<SpriteRenderer> ().color;
				clr.a = 1.0f;
				GetComponent<SpriteRenderer> ().color = clr;
				StopCoroutine (_coroutine);
				_respawn = false;
			}
			_respawnTime -= Time.deltaTime;
		}
		_rb = GetComponent<Rigidbody2D> ();
		_rb.constraints = RigidbodyConstraints2D.None;
		_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		float vertical = _rb.velocity.y;
		if (curIndex == playerIndex) {
			if (_canJump && (Input.GetKey ("space") || Input.GetKey ("up"))) {
				vertical = jump;
				_canJump = false;
			}
			_rb.velocity = new Vector2 (Input.GetAxis ("Horizontal") * speed, vertical);
		} else {
			_rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
		}
		if ((hasReachedExit [0] == true && hasReachedExit [1] == true && hasReachedExit [2] == true) || Input.GetKeyDown ("n")) {
			hasReachedExit [0] = false;
			hasReachedExit [1] = false;
			hasReachedExit [2] = false;
			loadNextLevel ();
		}
	}
}
