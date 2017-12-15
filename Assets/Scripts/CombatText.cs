using UnityEngine;
using System.Collections;

public class CombatText : MonoBehaviour {

	public AnimationClip floatUp;

	private Animator myAnimator;

	// Use this for initialization
	void Start () {
		myAnimator = GetComponent<Animator>();
		StartCoroutine("OnSpawnAnimation");
	}
	
	IEnumerator OnSpawnAnimation () {
		myAnimator.SetTrigger("floatUp");
		yield return new WaitForSeconds(floatUp.length);
		Destroy(transform.parent.gameObject);
	}
}
