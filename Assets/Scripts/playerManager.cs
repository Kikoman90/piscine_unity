using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour {

	/* public attributes start */
	public Transform[]	_mapLimits = new Transform[4];
	/* public attributes end */

	/* private attributes start */
	private	List<footman>	_footmanSelection = new List<footman> ();
	private List<building>	_buildingSelection = new List<building> ();
	/* private attributes end */

	private void				_ClearSelection<T> (List<T> list) where T : ISelectable {
		foreach (T element in list)
			element._selectedDisplay (false);
		list.Clear ();
	}

	private footman.orientation	_setUnitOrientation (Vector2 movePos, Vector2 ftmPos) {
		Vector2	dirVec = movePos - ftmPos;
		float angle = Vector2.Angle (dirVec, Vector3.right);
		angle = (Mathf.Sign(Vector3.Cross(dirVec, Vector3.right).z) > 0) ? (360 - angle) % 360 : angle;
		angle *= Mathf.Deg2Rad;
		if (angle >= Mathf.PI / 8 && angle <= (3 * Mathf.PI) / 8)
			return (footman.orientation.NE);
		else if (angle > (3 * Mathf.PI) / 8 && angle < (5 * Mathf.PI) / 8)
			return (footman.orientation.N);
		else if (angle >= (5 * Mathf.PI) / 8 && angle <= (7 * Mathf.PI) / 8)
			return (footman.orientation.NW);
		else if (angle > (7 * Mathf.PI) / 8 && angle < (9 * Mathf.PI) / 8)
			return (footman.orientation.W);
		else if (angle >= (9 * Mathf.PI) / 8 && angle <= (11 * Mathf.PI) / 8)
			return (footman.orientation.SW);
		else if (angle > (11 * Mathf.PI) / 8 && angle < (13 * Mathf.PI) / 8)
			return (footman.orientation.S);
		else if (angle >= (13 * Mathf.PI) / 8 && angle <= (15 * Mathf.PI) / 8)
			return (footman.orientation.SE);
		else
			return (footman.orientation.E);
	}

	void						Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector2	camPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			if (camPos.y < _mapLimits[0].position.y && camPos.y > _mapLimits[1].position.y && camPos.x < _mapLimits[2].position.x && camPos.x > _mapLimits[3].position.x) {
				RaycastHit2D hit = Physics2D.Raycast (camPos, Vector2.zero);
				if (hit && hit.collider) {
					if (hit.collider.CompareTag ("playerUnit") && !_footmanSelection.Contains (hit.collider.gameObject.GetComponent<footman> ())) {
						_ClearSelection<building> (_buildingSelection);
						footman ftm = hit.collider.GetComponent<footman> ();
						if (!Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.RightShift))
							_ClearSelection<footman> (_footmanSelection);
						ftm._selectedDisplay (true);
						_footmanSelection.Add (ftm);
					} else if (hit.collider.CompareTag ("building") && !_buildingSelection.Contains (hit.collider.gameObject.GetComponent<building> ())) {
						_ClearSelection<footman> (_footmanSelection);
						building bldg = hit.collider.GetComponent<building> ();
						if (!Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.RightShift))
							_ClearSelection<building> (_buildingSelection);
						bldg._selectedDisplay (true);
						_buildingSelection.Add (bldg);
					}
					//else if (CompareTag("orc")...)
					//else if ("obstacle"...)
				} else {
					foreach (footman ftm in _footmanSelection) {
						ftm.IsWalking = true;
						ftm.FootmanOrientation = _setUnitOrientation (camPos, ftm.transform.position);
						ftm.TargetPos = camPos;//movePos
						ftm._setCurrentAnimation ();
					}
				}
			}
		} else if (Input.GetMouseButtonDown (1)) {
			_ClearSelection<footman> (_footmanSelection);
			_ClearSelection<building> (_buildingSelection);
		}
	}
}
