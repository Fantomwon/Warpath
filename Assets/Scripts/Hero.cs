using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hero : MonoBehaviour {

	public Text healthDisplay;
	public int health = 10;
	public int damage = 3;
	public int speed = 1;
	public int range = 1;

	// Use this for initialization
	void Start () {
		healthDisplay.text = GetComponent<Hero>().health.ToString();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void TakeDamage (int dmg) {
		GetComponent<Hero>().health -= dmg;
		if (GetComponent<Hero>().health <= 0) {
			Destroy(gameObject);
		}
		healthDisplay.text = GetComponent<Hero>().health.ToString();
	}
}
