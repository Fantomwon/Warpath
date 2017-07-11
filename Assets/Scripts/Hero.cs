using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hero : MonoBehaviour {

	public Text healthDisplay;
	public int maxHealth;
	public int currentHealth;
	public int damage;
	public int speed;
	public int range;
	public bool movingRight = false;
	public bool movingLeft = false;
	public GameObject cardPrefab;

	private float distToMove = 1f;
	private PlayField playField;
	private Transform myTransform;
	private Hero hero;

	// Use this for initialization
	void Start () {
		hero = GetComponent<Hero>();
		healthDisplay.text = hero.currentHealth.ToString();
		playField = GameObject.FindObjectOfType<PlayField>();
		myTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		if (movingRight) {
			if (transform.position.x < distToMove) {
				transform.Translate(Vector2.right * Time.deltaTime * 3);
			} else {
				movingRight = false;
				playField.MoveHeroes();
				playField.Player1EnemyCheck(myTransform);

				//Make sure that the hero is sitting on a rounded X position
				Vector3 currentPos = transform.position;
				currentPos.x = Mathf.RoundToInt(transform.position.x);
				transform.position = currentPos;
			}
		} else if (movingLeft) {
			if (transform.position.x > distToMove) {
				transform.Translate(Vector2.left * Time.deltaTime * 3);
			} else {
				movingLeft = false;
				playField.MoveHeroes();
				playField.Player2EnemyCheck(myTransform);

				//Make sure that the hero is sitting on a rounded X position
				Vector3 currentPos = transform.position;
				currentPos.x = Mathf.RoundToInt(transform.position.x);
				transform.position = currentPos;
			}
		}
	}

	public void TakeDamage (int dmg) {
		hero.currentHealth -= dmg;
		if (hero.currentHealth <= 0) {
			Destroy(gameObject);
		}
		healthDisplay.text = hero.currentHealth.ToString();
	}

	public void HealFull () {
		hero.currentHealth += (hero.maxHealth - hero.currentHealth);
		healthDisplay.text = hero.currentHealth.ToString();
	}

	public void MoveSingleHeroRightAndAttack(float dist) {
		movingRight = true;
		distToMove = dist + transform.position.x;
	}

	public void MoveSingleHeroLeftAndAttack(float dist) {
		movingLeft = true;
		distToMove = transform.position.x - dist;
	}

	public void SetTransform (Transform hero) {
		hero = myTransform;
	}
}