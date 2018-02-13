﻿using UnityEngine;
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
			GameObject buff = Instantiate (buffMight, hero.transform.Find("Buffs").transform.localPosition, Quaternion.identity, hero.transform.Find("Buffs").transform) as GameObject;
			buff.GetComponent<Buff>().myHero = hero;
		}
		if (buffId == "shroud") {
			GameObject buff = Instantiate (buffShroud, hero.transform.Find("Buffs").transform.localPosition, Quaternion.identity, hero.transform.Find("Buffs").transform) as GameObject;
			buff.GetComponent<Buff>().myHero = hero;
		} else if (buffId == "root") {
			GameObject buff = Instantiate (debuffRoot, hero.transform.Find("Buffs").transform.localPosition, Quaternion.identity, hero.transform.Find("Buffs").transform) as GameObject;
			buff.GetComponent<Buff>().myHero = hero;
		}

	}

	public void DecrementBuffDurations (Transform hero) {
		if (hero.Find("Buffs").childCount > 0) {
			Debug.Log("RUNNING DecrementBuffDurations");
			foreach (Transform buff in hero.Find("Buffs").transform) {
				buff.GetComponent<Buff>().duration -= 1;
				if (buff.GetComponent<Buff>().duration <= 0) {
					buff.gameObject.GetComponent<Buff>().RemoveBuff(buff.gameObject);
				}
			}
		}

	}
}
