using UnityEngine;
using System.Collections;

public class BuffManager : MonoBehaviour {

	public GameObject buffMight;
	public GameObject buffShroud;
	public GameObject debuffRoot;

	// Use this for initialization
	void Start () {
	
	}

	public void ApplyBuff (string buffId, Transform hero) {
		if (buffId == "might") {
			GameObject buff = Instantiate (buffMight, hero.GetComponent<Hero>().buffList.transform.localPosition, Quaternion.identity, hero.GetComponent<Hero>().buffList.transform) as GameObject;
			buff.GetComponent<Buff>().myHero = hero;
		} else if (buffId == "shroud") {
			GameObject buff = Instantiate (buffShroud, hero.GetComponent<Hero>().buffList.transform.localPosition, Quaternion.identity, hero.GetComponent<Hero>().buffList.transform) as GameObject;
			buff.GetComponent<Buff>().myHero = hero;
		} else if (buffId == "root") {
			GameObject buff = Instantiate (debuffRoot, hero.GetComponent<Hero>().buffList.transform.localPosition, Quaternion.identity, hero.GetComponent<Hero>().buffList.transform) as GameObject;
			buff.GetComponent<Buff>().myHero = hero;
		}
	}

	public void DecrementBuffDurations (Transform hero) {
		if (hero.GetComponent<Hero>().buffList.childCount > 0) {
			//Debug.Log("RUNNING DecrementBuffDurations");
			foreach (Transform buff in hero.GetComponent<Hero>().buffList.transform) {
				buff.GetComponent<Buff>().duration -= 1;
				if (buff.GetComponent<Buff>().duration <= 0) {
					buff.gameObject.GetComponent<Buff>().RemoveBuff(buff.gameObject);
				}
			}
		}
	}

	public bool CheckForExistingBuff (Transform hero, string buffId) {
		foreach (Transform buff in hero.GetComponent<Hero>().buffList.transform) {
			//If I can find the given buffId in the list of the hero's buffs, return true
			if (buff.GetComponent<Buff>().id == buffId) {
				Debug.Log("Found an existing buff on a hero: " + buffId);
				return true;
			}
		}
		//If I CANNOT find the given buffId in the list of the hero's buffs, return false
		return false;
	}
}
