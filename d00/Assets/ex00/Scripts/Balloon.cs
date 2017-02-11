using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {

	private float	balloon_lifetime = 0.0f;
	private float	refresh_breath = 0.0f;


	void	DestroyBalloon() {
		Destroy (gameObject);
	}

	void	GameOver() {
		Debug.Log ("Balloon life time: " + Mathf.RoundToInt(balloon_lifetime) + "s");
	}

	void	Update () {
		balloon_lifetime += Time.deltaTime;
		if (transform.localScale.x <= 0.3f) {
			GameOver ();
			DestroyBalloon ();
		}
		if (Input.GetKeyDown ("space") && refresh_breath <= 3.0f) {
			if (transform.localScale.x >= 1.6f) {
				GameOver ();
				DestroyBalloon ();
			}
			refresh_breath += 1.0f;
			transform.localScale *= 1.1f;//Debug.Log ("LMAO");
		}
		transform.localScale *= 0.995f;
		refresh_breath -= 0.06f;
	}

}
