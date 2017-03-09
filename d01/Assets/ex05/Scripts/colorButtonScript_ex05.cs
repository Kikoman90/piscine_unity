using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum ColorState : int {
	defaultColor = 0, red = 8, yellow = 9, blue = 10
}*/

public class colorButtonScript_ex05 : MonoBehaviour {
	
	public GameObject[]	platforms;

	[Header("Color switches:")]
	public ColorState	defaultColorTo;
	public ColorState	redTo;
	public ColorState	yellowTo;
	public ColorState	blueTo;

	public Color	defaultColorTo_color;
	public Color	redTo_color;
	public Color	yellowTo_color;
	public Color	blueTo_color;

	public Material	LineMaterial;

	private LineRenderer[]	_lines;

	private bool	_switchColors = false;
	private bool	_unswitchColors = false;

	private List<int[]>	_layerList = new List<int[]> ();
	private List<Color[]>	_colorList = new List<Color[]>();

	void			Start () {
		_lines = new LineRenderer[platforms.Length];
		_initLines (platforms, _lines, "line (");
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
				positions [2] = new Vector3 (doors [i].transform.position.x - spriteSizes[2], doors [i].transform.position.y, 1);
			} else {
				positions [1] = new Vector3 (transform.position.x + spriteSizes[0], doors [i].transform.position.y + spriteSizes[3], 1);
				positions [2] = new Vector3 (doors [i].transform.position.x, doors [i].transform.position.y + spriteSizes[3], 1);
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
		if (coll.gameObject.tag == "player") {
			_unswitchColors = false;
			_switchColors = true;
		}
	}

	void	OnTriggerExit2D(Collider2D coll) {
		if (coll.gameObject.tag == "player") {
			_switchColors = false;
			_unswitchColors = true;
		}
	}

	void			Update() {
		if (_switchColors) {
			int	i = 0;
			int[] intTab = new int[transform.childCount];
			Color[]	colorTab = new Color[transform.childCount];
			foreach (GameObject go in platforms) {
				intTab [i] = go.layer;
				colorTab [i] = go.GetComponent<SpriteRenderer> ().color;
				if (go.layer == 0) {
					go.layer = (int)defaultColorTo;
					go.GetComponent<SpriteRenderer> ().color = defaultColorTo_color;
				} else if (go.layer == 8) {
					go.layer = (int)redTo;
					go.GetComponent<SpriteRenderer> ().color = redTo_color;
				} else if (go.layer == 9) {
					go.layer = (int)yellowTo;
					go.GetComponent<SpriteRenderer> ().color = yellowTo_color;
				} else if (go.layer == 10) {
					go.layer = (int)blueTo;
					go.GetComponent<SpriteRenderer> ().color = blueTo_color;
				}
				i++;
			}
			_layerList.Add (intTab);
			_colorList.Add (colorTab);
			_switchColors = false;
		}
		if (_unswitchColors) {
			int i = _layerList.Count - 1;
			int j = 0;
			foreach (GameObject go in platforms) {
				go.layer = _layerList [i][j];
				go.GetComponent<SpriteRenderer> ().color = _colorList [i][j];
				j++;
			}
			_layerList.RemoveAt(i);
			_colorList.RemoveAt (i);
			_unswitchColors = false;
		}
		_setLines (platforms, _lines);
	}

}
