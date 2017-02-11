using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeSpawner : MonoBehaviour {

	public int	spawnTime;

	public static float	timer = 0;

	public Cube[]	prefabs;
	public TextMove[]	textPrefab;
	public Transform[]	obj_begin;
	public Transform[]	obj_end;

	private int	_multiplier = 0;
	private int	_multiplier_tmp = -1;

	private float	_multiplier_hue = 0.150f;
	private float	precision;

	private Color	_tmp_clr;
	private GameObject	canvas;
	private Cube[]	objects = new Cube[3];
	private List<TextMove>	text_obj = new List<TextMove>();
	private TextMove	_multiplier_text;
	private string[]	keycodes = new string[]{"a", "s", "d"};
	private bool[]	_miss = new bool[]{false, false, false};

	void Start () {
		canvas = GameObject.Find ("Canvas");
		_multiplier_text = (TextMove)GameObject.Instantiate (textPrefab [4]);
		_multiplier_text.transform.SetParent (canvas.transform, false);
	}

	void Update () {
		if (_multiplier != _multiplier_tmp) {
			if (_multiplier_hue < 0.02f)
				_multiplier_hue = 0.02f;
			_tmp_clr = Color.HSVToRGB (_multiplier_hue, 0.89f, 0.937f);
			_multiplier_text.GetComponent<Text>().color = _tmp_clr;
			_multiplier_text.GetComponent<Text> ().text = "x" + _multiplier;
			_multiplier_tmp = _multiplier;
		}
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
					objects [1] = (Cube)GameObject.Instantiate (prefabs [1], obj_begin [1].position, Quaternion.identity);
					_miss [0] = false;
					_miss [1] = false;
				}
			} else if (rand == 4) {
				if (!objects [0] && !objects[2]) {
					objects [0] = (Cube)GameObject.Instantiate (prefabs [0], obj_begin [0].position, Quaternion.identity);
					objects [2] = (Cube)GameObject.Instantiate (prefabs [2], obj_begin [2].position, Quaternion.identity);
					_miss [0] = false;
					_miss [2] = false;
				}
			} else if (rand == 5) {
				if (!objects [1] && !objects[2]) {
					objects [1] = (Cube)GameObject.Instantiate (prefabs [1], obj_begin [1].position, Quaternion.identity);
					objects [2] = (Cube)GameObject.Instantiate (prefabs [2], obj_begin [2].position, Quaternion.identity);
					_miss [1] = false;
					_miss [2] = false;
				}
			} else {
				if (!objects[0] && !objects [1] && !objects[2]) {
					objects [0] = (Cube)GameObject.Instantiate (prefabs [0], obj_begin [0].position, Quaternion.identity);
					objects [1] = (Cube)GameObject.Instantiate (prefabs [1], obj_begin [1].position, Quaternion.identity);
					objects [2] = (Cube)GameObject.Instantiate (prefabs [2], obj_begin [2].position, Quaternion.identity);
					_miss [0] = false;
					_miss [1] = false;
					_miss [2] = false;
				}
			}
		}
		int index = 0;
		while (index < 3) {
			if (objects [index] && Input.GetKeyDown (keycodes [index]) && _miss[index] == false) {
				precision = Mathf.Abs (obj_end [index].position.y - objects [index].transform.localPosition.y);
				if (text_obj.Count == 4) {
					text_obj [3].fade_n_destroy ();
					text_obj.RemoveAt (3);
				}
				if (precision <= 0.1f) {
					text_obj.Insert (0, (TextMove)GameObject.Instantiate (textPrefab [0]));
					_multiplier_hue -= 0.008f;
					_multiplier += 1;
				} else if (precision <= 0.3f) {
					text_obj.Insert (0, (TextMove)GameObject.Instantiate (textPrefab [1]));
					_multiplier_hue -= 0.008f;
					_multiplier += 1;
				} else {
					text_obj.Insert (0, (TextMove)GameObject.Instantiate (textPrefab [2]));
					_multiplier_hue = 0.150f;
					_multiplier = 0;
				}
				text_obj[0].transform.SetParent (canvas.transform, false);
				for (int i = 1; i < text_obj.Count; ++i)
					text_obj [i].move_up ();
				objects [index].switch_status ();
				Debug.Log ("Precision: " + precision);
				_miss [index] = true;
			} else if (_miss[index] == false && objects[index] && objects [index].transform.position.y < (obj_end [index].position.y - (objects [index].transform.localScale.y / 2.0f))) {
				if (text_obj.Count == 4) {
					text_obj [3].fade_n_destroy ();
					text_obj.RemoveAt (3);
				}
				text_obj.Insert(0, (TextMove)GameObject.Instantiate (textPrefab [3]));
				text_obj[0].transform.SetParent (canvas.transform, false);
				for (int i = 1; i < text_obj.Count; ++i)
					text_obj [i].move_up ();
				Debug.Log ("Miss!");
				objects [index].fade_away ();
				_multiplier_hue = 0.150f;
				_multiplier = 0;
				_miss[index] = true;
			}
			index += 1;
		}
	}

}