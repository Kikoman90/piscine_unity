using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class building : MonoBehaviour, ISelectable {

	/* public attributes start */
	public GameObject	_unitPrefab = null;

	public bool	_enemyBuilding = false;
	public bool	_canSpawnUnits;

	public float	_spawnDelay = 10.0f;

	[Range (2.5f, 5.0f)]
	public float	_spawnRadius = 3.0f;

	[Range (1, 25)]
	public int	_maxNbOfUnits = 10;
	/* public attributes end */

	/* private attributes start */
	private int	_nbOfUnits = 0;

	private float	_timer;

	private float	_heightOfSprite;
	private float	_widthOfSprite;
	private float	_heightOfUnitSprite;
	private float	_widthOfUnitSprite;

	private bool	_hasSpawnedUnit = false;

	private Image	_selectedImage;
	private Image	_spawnZoneImage;
	/* private attributes end */

	void OnValidate () {
		if (_canSpawnUnits && _unitPrefab == null)
			_canSpawnUnits = false;
		_spawnDelay = Mathf.Clamp (_spawnDelay, 0, 3600.0f);	
	}

	void Start () {
		_timer = _spawnDelay;
		_heightOfSprite = GetComponent<SpriteRenderer> ().bounds.size.y;
		_widthOfSprite = GetComponent<SpriteRenderer> ().bounds.size.x;
		if (_unitPrefab != null && _canSpawnUnits) {
			_heightOfUnitSprite = _unitPrefab.GetComponent<SpriteRenderer> ().bounds.size.y;
			_widthOfUnitSprite = _unitPrefab.GetComponent<SpriteRenderer> ().bounds.size.x;
		}
		_spawnZoneImage = transform.GetChild (0).GetChild (0).gameObject.GetComponent<Image>();
		_selectedImage = transform.GetChild (0).GetChild (1).gameObject.GetComponent<Image>();
	}

	public void		_selectedDisplay (bool show) {
		if (show) {
			if (_selectedImage)
				_selectedImage.enabled = true;
			if (_canSpawnUnits && !_enemyBuilding && _spawnZoneImage)
				_spawnZoneImage.enabled = true;
		} else {
			if (_selectedImage)
				_selectedImage.enabled = false;
			if (_canSpawnUnits && !_enemyBuilding && _spawnZoneImage)
				_spawnZoneImage.enabled = false;
		}
	}

	void SpawnUnit () {
		while (!_hasSpawnedUnit) {
			float angle = Random.Range (0.0f, 360.0f);
			angle *= Mathf.Deg2Rad;
			// baseAngle: the angle in radians between rectangle diagonal and Ox axis
			float baseAngle = Mathf.Atan (_heightOfSprite / _widthOfSprite);
			// Which side we're on?
			bool left = (Mathf.Abs(angle - Mathf.PI) < baseAngle);
			bool right = ((angle > 2 * Mathf.PI - baseAngle) || (angle < baseAngle));
			bool top = (Mathf.Abs(angle - Mathf.PI / 2.0f) <= Mathf.Abs(Mathf.PI / 2.0f - baseAngle));
			bool bottom = (Mathf.Abs(angle - 3.0f * Mathf.PI / 2.0f) <= Mathf.Abs(Mathf.PI / 2.0f - baseAngle));
			int lr = (left ? -1 : 0) + (right ? 1 : 0);
			int tb = (bottom ? -1 : 0) + (top ? 1 : 0);
			Vector3	posOnSprite;
			if (lr != 0)
				posOnSprite = new Vector3 (transform.position.x + _widthOfSprite / 2.0f * lr, transform.position.y + _widthOfSprite / 2.0f * Mathf.Tan(angle) * lr, 0);
			else
				posOnSprite = new Vector3 (transform.position.x + _heightOfSprite / 2.0f * Mathf.Tan(Mathf.PI / 2 - angle) * tb, transform.position.y + _heightOfSprite / 2.0f * tb, 0);
			Vector3 posOnSpawnZone = new Vector3 (transform.position.x + _spawnRadius * Mathf.Cos(angle), transform.position.y + _spawnRadius * Mathf.Sin(angle),0);
			if (new Vector3(posOnSprite.x - transform.position.x, posOnSprite.y - transform.position.y, 0).magnitude >= new Vector3(posOnSpawnZone.x - transform.position.x, posOnSpawnZone.y - transform.position.y, 0).magnitude)
				break;
			Vector3 randPos = Vector3.Lerp (posOnSprite, posOnSpawnZone, Random.Range (0.0f, 1.0f));
			RaycastHit2D hit1 = Physics2D.Raycast (new Vector3(randPos.x - _widthOfUnitSprite / 2, randPos.y + _heightOfUnitSprite / 2, 0), Vector3.zero, 10.0f);
			RaycastHit2D hit2 = Physics2D.Raycast (new Vector3(randPos.x + _widthOfUnitSprite / 2, randPos.y + _heightOfUnitSprite / 2, 0), Vector3.zero, 10.0f);
			RaycastHit2D hit3 = Physics2D.Raycast (new Vector3(randPos.x - _widthOfUnitSprite / 2, randPos.y - _heightOfUnitSprite / 2, 0), Vector3.zero, 10.0f);
			RaycastHit2D hit4 = Physics2D.Raycast (new Vector3(randPos.x + _widthOfUnitSprite / 2, randPos.y - _heightOfUnitSprite / 2, 0), Vector3.zero, 10.0f);
			if (!hit1 && !hit2 && !hit3 && !hit4) {
				_nbOfUnits++;
				Instantiate (_unitPrefab, randPos, Quaternion.identity);
				_hasSpawnedUnit = true;
			}
		}
	}

	void Update () {
		if (_canSpawnUnits && _nbOfUnits < _maxNbOfUnits) {
			if (_timer >= _spawnDelay) {
				SpawnUnit ();
				_hasSpawnedUnit = false;
				_timer = 0;
			}
			_timer += Time.deltaTime;
		}
	}

	void OnDrawGizmosSelected ()
	{
		if (_canSpawnUnits) {
			UnityEditor.Handles.color = Color.green;
			UnityEditor.Handles.DrawWireDisc (transform.position, new Vector3 (0, 0, 1), _spawnRadius);
		}
	}
}
