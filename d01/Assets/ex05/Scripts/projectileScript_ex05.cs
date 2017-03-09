using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript_ex05 : MonoBehaviour {

	string	nameOfTurret;

	public void	setName(string name) {
		nameOfTurret = name;
	}

	void		OnTriggerEnter2D(Collider2D coll) {
		if ((coll.gameObject.layer == 0 || coll.gameObject.layer == gameObject.layer) && coll.gameObject.tag != "trap") {
			Transform turret;
			if ((turret = transform.parent.Find (nameOfTurret)) != null)
				turret.GetComponent<turretScript_ex05> ().removeProjectileFromList (gameObject);
			else if (coll.gameObject.tag == "teleporter") {
				Transform[] transform_tp = coll.gameObject.GetComponentsInChildren<Transform> ();
				gameObject.transform.position = transform_tp [1].position;
			} else
				Destroy (gameObject);
		}
	}
}
