using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationEvents : MonoBehaviour {

	private PlayField playField;
	private Transform myTransform;

	// Use this for initialization
	void Start () {
		playField = FindObjectOfType<PlayField>();
		myTransform = this.transform.parent.transform;
	}

	public void RunHeroTargetCheck () {
		playField.HeroTargetCheck(myTransform);
	}

}