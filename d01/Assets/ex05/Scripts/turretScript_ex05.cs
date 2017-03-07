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

	private List<GameObject>	_projectiles = null;

	private Vector2	_direction;

	private float	_timer = 0.0f;

	void	Start() {
		_direction = new Vector2 (Mathf.Cos (transform.rotation.z * Mathf.PI), Mathf.Sin (transform.rotation.z * Mathf.PI));
	}

	void	FixedUpdate () {
		if (_timer >= delayBetweenProjectiles) {
			_projectiles.Add(Instantiate<GameObject>(projectilePrefab, transform.position, Quaternion.identity));
			_timer = 0.0f;
		}
		foreach (GameObject go in _projectiles) {
			go.GetComponent<Rigidbody2D> ().MovePosition (go.transform.position.x + speedOfProjectiles * _direction.x * Time.fixedDeltaTime, go.transform.position.y * speedOfProjectiles * _direction.y * Time.fixedDeltaTime);
		}
		_timer += Time.fixedDeltaTime;
	}
}
