using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour {

	public int	spawnTime;
	public static float	timer = 0;
	public Cube[]	prefabs;
	private Cube[]	objects = new Cube[3];
	public Transform[]	obj_begin;
	public Transform[]	obj_end;
	private string[]	keycodes = new string[]{"a", "s", "d"};
	private bool[]	_miss = new bool[]{false, false, false};

	void Update () {
		timer += Time.deltaTime;
		if (timer >= spawnTime) {
			timer = 0;
			int rand = Random.Range (0, 7);
			if (rand < 3) {
				if (!objects [rand]) {
					objects [rand] = (Cube)GameObject.Instantiate (prefabs [rand], obj_begin [rand].position, Quaternion.identity);
					_miss [rand] = false;
				}
			} else if (rand == 3) {
				if (!objects [0] && !objects[1]) {
					objects [0] = (Cube)GameObject.Instantiate (prefabs [0], obj_begin [0].position, Quaternion.identity);
					_miss [0] = false;
					objects [1] = (Cube)GameObject.Instantiate (prefabs [1], obj_begin [1].position, Quaternion.identity);
					_miss [1] = false;
				}
			} else if (rand == 4) {
				if (!objects [0] && !objects[2]) {
					objects [0] = (Cube)GameObject.Instantiate (prefabs [0], obj_begin [0].position, Quaternion.identity);
					_miss [0] = false;
					objects [2] = (Cube)GameObject.Instantiate (prefabs [2], obj_begin [2].position, Quaternion.identity);
					_miss [2] = false;
				}
			} else if (rand == 5) {
				if (!objects [1] && !objects[2]) {
					objects [1] = (Cube)GameObject.Instantiate (prefabs [1], obj_begin [1].position, Quaternion.identity);
					_miss [1] = false;
					objects [2] = (Cube)GameObject.Instantiate (prefabs [2], obj_begin [2].position, Quaternion.identity);
					_miss [2] = false;
				}
			} else {
				if (!objects[0] && !objects [1] && !objects[2]) {
					objects [0] = (Cube)GameObject.Instantiate (prefabs [0], obj_begin [0].position, Quaternion.identity);
					_miss [0] = false;
					objects [1] = (Cube)GameObject.Instantiate (prefabs [1], obj_begin [1].position, Quaternion.identity);
					_miss [1] = false;
					objects [2] = (Cube)GameObject.Instantiate (prefabs [2], obj_begin [2].position, Quaternion.identity);
					_miss [2] = false;
				}
			}
		}
		int index = 0;
		while (index < 3) {
			if (objects [index] && Input.GetKeyDown (keycodes [index]) && _miss[index] == false) {
				objects [index].switch_status ();
				Debug.Log ("Precision: " + Mathf.Abs (obj_end [index].position.y - objects [index].transform.localPosition.y));
			} else if (_miss[index] == false && objects[index] && objects [index].transform.position.y < (obj_end [index].position.y - (objects [index].transform.localScale.y / 2.0f))) {
				Debug.Log ("Miss!");
				objects [index].fade_away ();
				_miss[index] = true;
			}
			index += 1;
		}
	}

}