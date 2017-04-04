using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class footman : MonoBehaviour, ISelectable {

	public enum orientation
	{
		E, N, NE, NW, S, SE, SW, W
	};

	/* public attributes start */
	public bool	_enemyUnit;

	public float	_speedOfUnit = 2.0f;
	/* public attributes end */

	/* private attributes start */
	private	Animator	_footmanAnimator;

	private Image	_selectedImage;
	/* private attributes end */

	/* properties start */
	public bool	IsWalking { get; set; }
	public bool	IsSelected { get; set; }

	public Vector3	TargetPos { get; set; }

	//si l'on veut que toutes les unites selectionnes soient orientees dans la meme direction a la fin du mouvement:
	//creer _footmanWalkOrientation et _footmanIdleOrientation (a determiner dans le manager)
	public orientation FootmanOrientation { get; set; }
	/* properties end */

	void			OnValidate () {
		_speedOfUnit = Mathf.Clamp (_speedOfUnit, 0, 100.0f);
	}

	void			Start () {
		_footmanAnimator = GetComponent<Animator> ();
		FootmanOrientation = (footman.orientation)Random.Range (0, 7);
		_setCurrentAnimation ();
		_selectedImage = transform.GetChild(0).GetChild (0).gameObject.GetComponent<Image>();
	}

	public void		_selectedDisplay (bool show) {
		if (show) {
			if (_selectedImage)
				_selectedImage.enabled = true;
		} else {
			if (_selectedImage)
				_selectedImage.enabled = false;
		}
	}

	public void		_setCurrentAnimation () {
		int countParameters = _footmanAnimator.parameterCount;
		AnimatorControllerParameter param;
		for (int i = 0; i < countParameters; i++) {
			param = _footmanAnimator.GetParameter (i);
			_footmanAnimator.SetBool(param.name, false);
		}
		if (IsWalking)
			_footmanAnimator.SetBool ("footman_walk_" + FootmanOrientation.ToString(), true);
		else
			_footmanAnimator.SetBool ("footman_idle_" + FootmanOrientation.ToString(), true);
	}

	void			Update () {
		if (IsWalking) {
			transform.position = Vector3.MoveTowards (transform.position, TargetPos, _speedOfUnit * Time.deltaTime);
			if (TargetPos.y > transform.position.y - 0.2 && TargetPos.y < transform.position.y + 0.2 && TargetPos.x > transform.position.x - 0.2 && TargetPos.x < transform.position.x + 0.2) {
				IsWalking = false;
				_setCurrentAnimation ();
			}
		}
	}
}
