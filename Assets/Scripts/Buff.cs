using UnityEngine;
using System.Collections;

public class Buff : MonoBehaviour {

	public string id;
	public int duration;
	public GameObject particle;
	public Transform myHero;

	private GameObject spawnedParticle;
	private int originalSpeed;
	private int originalPower;

	// Use this for initialization
	void Start () {
		//We store a hero's original speed in a variable in case a particular buff/debuff needs to reset a hero back to their original speed after changing it (e.g. the 'Root' spell)
		originalSpeed = myHero.GetComponent<Hero>().speed;
		//We store a hero's original power in a variable in case a particular buff/debuff needs to reset a hero back to their original power after changing it (e.g. the 'Might' spell)
		originalPower = myHero.GetComponent<Hero>().power;

		//This id is set in the editor
		if (id == "might") {
			BuffMight();
		} else if (id == "shroud") {
			BuffShroud();
		} else if (id == "root") {
			DebuffRoot();
		}
	
	}

	public void BuffMight () {
		myHero.GetComponent<Hero>().power += 2;
		//We're currently spawning this particle in a bit of a roundabout way b/c parenting it to the buff itself while also maintaining its 
		//desired position in relation to the hero was very difficult, albeit doing it that way would be preferable from an organizational standpoint
		//b/c it would keep the particle parented under the Buff GameObject itself. But alas, doing things this way seems to work, so we'll roll with it for now.
		spawnedParticle = Instantiate (particle, myHero.transform.localPosition, Quaternion.identity, myHero.transform) as GameObject;
	}

	public void BuffShroud () {
		myHero.GetComponent<Hero>().dodgeChance = 0.5f;
		//We're currently spawning this particle in a bit of a roundabout way b/c parenting it to the buff itself while also maintaining its 
		//desired position in relation to the hero was very difficult, albeit doing it that way would be preferable from an organizational standpoint
		//b/c it would keep the particle parented under the Buff GameObject itself. But alas, doing things this way seems to work, so we'll roll with it for now.
		spawnedParticle = Instantiate (particle, myHero.transform.localPosition, Quaternion.identity, myHero.transform) as GameObject;
	}

	public void DebuffRoot () {
		myHero.GetComponent<Hero>().speed = 0;
		//We're currently spawning this particle in a bit of a roundabout way b/c parenting it to the buff itself while also maintaining its 
		//desired position in relation to the hero was very difficult, albeit doing it that way would be preferable from an organizational standpoint
		//b/c it would keep the particle parented under the Buff GameObject itself. But alas, doing things this way seems to work, so we'll roll with it for now.
		spawnedParticle = Instantiate (particle, myHero.transform.localPosition, Quaternion.identity, myHero.transform) as GameObject;
	}

	public void RemoveBuff (GameObject buff) {
		Debug.Log("DESTROYING BUFF");
		if (buff.GetComponent<Buff>().id == "might") {
			myHero.GetComponent<Hero>().power -= 2;
		} else if (buff.GetComponent<Buff>().id == "shroud") {
			myHero.GetComponent<Hero>().dodgeChance = 0.0f;
		} else if (buff.GetComponent<Buff>().id == "root") {
			myHero.GetComponent<Hero>().speed = originalSpeed;
		}
		Destroy (spawnedParticle);
		Destroy (buff);
	}

}