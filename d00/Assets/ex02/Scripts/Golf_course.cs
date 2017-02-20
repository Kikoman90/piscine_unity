using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golf_course : MonoBehaviour {

	/* set alpha of sprite to 1.0f */
	public void	setCourseActive() {
		Color tmp_clr = Color.HSVToRGB (0, 0, 1.0f);
		tmp_clr.a = 1.0f;
		GetComponent<SpriteRenderer> ().color = tmp_clr;
	}

	/* set alpha of sprite to 0.604f */
	public void	setCourseInactive() {
		Color tmp_clr = Color.HSVToRGB (0, 0, 0.773f);
		tmp_clr.a = 0.604f;
		GetComponent<SpriteRenderer> ().color = tmp_clr;
	}
}
