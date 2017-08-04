using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

	public Transform hero;
	public int spellDamage;

	private Card card;

	// Use this for initialization
	void Start () {
		card = FindObjectOfType<Card>();
//		gameObject.transform.parent = null;
	}

	public void EndOfSpellEffects () {
		if (gameObject.name == "Fireball") {
			//TODO Think about referencing the 'TakeDamage' method in hero.cs directly here instead of going through card.cs
			card.DoSpellDamage(hero,spellDamage);
		} else if (gameObject.name == "FlameStrike") {
			card.DoSpellDamage(hero,spellDamage);
		}
	}

	public void DestroyGameObject () {
		Destroy(transform.parent.gameObject);
	}

}
