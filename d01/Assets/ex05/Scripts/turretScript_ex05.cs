using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class turretScript_ex05 : MonoBehaviour {

	public GameObject	projectilePrefab;

	[Tooltip ("in units/second")]
	public float	speedOfProjectiles;
	[Tooltip ("in seconds")]
	public float	delayBetweenProjectiles;

	private List<GameObject>	_projectiles = new List<GameObject>();

	private Vector2	_direction;

	private float	_timer = 0.0f;

	void		Start() {
		float rad = Mathf.Deg2Rad * transform.eulerAngles.z;
		_direction = new Vector2 (Mathf.Sin (rad), -Mathf.Cos (rad)); 
	}

	public void	removeProjectileFromList(GameObject projectile) {
		_projectiles.Remove (projectile);
		Destroy (projectile);
	}

	void		FixedUpdate () {
		if (_timer >= delayBetweenProjectiles) {
			GameObject projectile = GameObject.Instantiate (projectilePrefab, transform.position, transform.rotation);
			projectile.GetComponent<projectileScript_ex05> ().setName (gameObject.name);
			projectile.transform.parent = GameObject.Find ("platforms").transform;
			_projectiles.Add(projectile);
			_timer = 0.0f;
		}
		foreach (GameObject go in _projectiles) {
			if (go != null) {
				Rigidbody2D rb2d = go.GetComponent<Rigidbody2D> ();
				Vector2	newPos = new Vector2 (rb2d.position.x + _direction.x * speedOfProjectiles * Time.fixedDeltaTime, rb2d.position.y + _direction.y * speedOfProjectiles * Time.fixedDeltaTime);
				rb2d.MovePosition (newPos);
			}
		}
		_timer += Time.fixedDeltaTime;
	}
}
