using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorButtonScript_ex05 : MonoBehaviour {

	public GameObject[]	redDoors;
	public GameObject[] yellowDoors;
	public GameObject[] blueDoors;

	public Material	LineMaterial;

	private LineRenderer[]	_redLines;
	private	LineRenderer[]	_yellowLines;
	private LineRenderer[]	_blueLines;

	private bool[]	_openDoors = new bool[]{ false, false, false };
	private bool[]	_closeDoors = new bool[]{ false, false, false };

	private float[]	_timer = new float[]{0, 0, 0};

	void			Start () {
		_redLines = new LineRenderer[redDoors.Length];
		_yellowLines = new LineRenderer[yellowDoors.Length];
		_blueLines = new LineRenderer[blueDoors.Length];
		_initLines (redDoors, _redLines, "redLine (");
		_initLines (yellowDoors, _yellowLines, "yellowLine (");
		_initLines (blueDoors, _blueLines, "blueLine (");
	}

	private void	_setLines(GameObject[] doors, LineRenderer[] lines) {
		for (int i = 0; i < doors.Length; i++) {
			lines [i].material = LineMaterial;
			lines[i].startWidth = 0.009f;
			lines[i].endWidth = 0.009f;
			Vector3[] positions = new Vector3[3];
			float[]	spriteSizes = new float[]{GetComponent<SpriteRenderer> ().bounds.size.x / 2.0f, GetComponent<SpriteRenderer> ().bounds.size.y / 2.0f, doors [i].GetComponent<SpriteRenderer> ().bounds.size.x / 2.0f, doors [i].GetComponent<SpriteRenderer> ().bounds.size.y / 2.0f};
			positions[0] = new Vector3(transform.position.x + spriteSizes[0], transform.position.y + spriteSizes[1], 1);
			if (doors[i].transform.rotation.eulerAngles.z > 45.0f || doors[i].transform.rotation.eulerAngles.z < -45.0f) {
				positions [1] = new Vector3 (doors [i].transform.position.x - spriteSizes[2], transform.position.y + spriteSizes[1], 1);
				positions [2] = new Vector3 (doors [i].transform.position.x - spriteSizes[2], doors [i].transform.position.y + spriteSizes[3], 1);
			} else {
				positions [1] = new Vector3 (transform.position.x + spriteSizes[0], doors [i].transform.position.y + spriteSizes[3], 1);
				positions [2] = new Vector3 (doors [i].transform.position.x + spriteSizes[2], doors [i].transform.position.y + spriteSizes[3], 1);
			}
			lines[i].numPositions = positions.Length;
			lines[i].SetPositions (positions);
		}
	}

	private void	_initLines(GameObject[] doors, LineRenderer[] lines, string NameOfChildren) {
		GameObject[] children = new GameObject[doors.Length];
		for (int i = 0; i < doors.Length; i++) {
			children [i] = new GameObject (NameOfChildren + i + ")");
			children [i].transform.parent = transform;
			lines [i] = children [i].AddComponent<LineRenderer> ();
		}
		_setLines (doors, lines);
	}

	void	OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.layer == 11) {
			_closeDoors [0] = false;
			_openDoors [0] = true;
		} else if (coll.gameObject.layer == 12) {
			_closeDoors [1] = false;
			_openDoors [1] = true;
		} else if (coll.gameObject.layer == 13) {
			_closeDoors [2] = false;
			_openDoors [2] = true;
		}
	}

	void	OnTriggerExit2D(Collider2D coll) {
		if (coll.gameObject.layer == 11) {
			_openDoors [0] = false;
			_closeDoors [0] = true;
		} else if (coll.gameObject.layer == 12) {
			_openDoors [1] = false;
			_closeDoors [1] = true;
		} else if (coll.gameObject.layer == 13) {
			_openDoors [2] = false;
			_closeDoors [2] = true;
		}
	}

	private void	_moveDoors(GameObject[] doors, LineRenderer[] lines, int sign) {
		_setLines (doors, lines);
		Vector3 vecMove;
		for (int i = 0; i < doors.Length; i++) {
			if (doors [i].transform.rotation.eulerAngles.z > 45.0f || doors [i].transform.rotation.eulerAngles.z < -45.0f)
				vecMove = new Vector3 (0, doors [i].GetComponent<SpriteRenderer> ().bounds.size.y * 2.0f, 0);
			else
				vecMove = new Vector3 (doors [i].GetComponent<SpriteRenderer> ().bounds.size.x * 2.0f, 0, 0);
			doors [i].GetComponent<Rigidbody2D> ().MovePosition (doors [i].transform.position + sign * vecMove * Time.fixedDeltaTime);
		}
	}

	void			FixedUpdate() {
		for (int i = 0; i < 3; i++) {
			if (_openDoors [i] == true) {
				if (i == 0)
					_moveDoors (redDoors, _redLines, 1);
				else if (i == 1)
					_moveDoors (yellowDoors, _yellowLines, 1);
				else if (i == 2)
					_moveDoors (blueDoors, _blueLines, 1);
				_timer [i] += Time.fixedDeltaTime;
				if (_timer[i] >= 0.5f) {
					_timer[i] = 0.5f;
					_openDoors[i] = false;
				}
			} else if (_closeDoors [i] == true) {
				if (i == 0)
					_moveDoors (redDoors, _redLines, -1);
				else if (i == 1)
					_moveDoors (yellowDoors, _yellowLines, -1);
				else if (i == 2)
					_moveDoors (blueDoors, _blueLines, -1);
				_timer [i] -= Time.fixedDeltaTime;
				if (_timer[i] <= 0.0f) {
					_timer[i] = 0.0f;
					_closeDoors[i] = false;
				}
			}
		}
	}
}
