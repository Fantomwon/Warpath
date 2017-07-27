using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hero : MonoBehaviour {

	public Text healthDisplay;
	public string id;
	public int maxHealth;
	public int currentHealth;
	public int power;
	public int speed;
	public int range;
	public int healValue;
	public int playerDamage;
	public bool movingRight = false;
	public bool movingLeft = false;
	public bool usingHaste = false;
	public GameObject cardPrefab;
	public ParticleSystem hitParticle;

	private float distToMove = 1f;
	private PlayField playField;
	private Transform myTransform;
	private Hero hero;
	private int player1HomeColumn = 1;
	private int player2HomeColumn = 7;
	private Animator myAnimator;

	// Use this for initialization
	void Start () {
		hero = GetComponent<Hero>();
		healthDisplay.text = hero.currentHealth.ToString();
		playField = GameObject.FindObjectOfType<PlayField>();
		myTransform = GetComponent<Transform>();
		myAnimator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (movingRight) {
			if (transform.position.x < distToMove) {
				transform.Translate(Vector2.right * Time.deltaTime * 3);
				myAnimator.SetTrigger("isWalking");
			} else {
				movingRight = false;
				myAnimator.ResetTrigger("isWalking");
				//This is where heroes ATTACK (or more specifically this is where heroes run through their targeting logic and do w/e it is they do to all of their valid targets)
				playField.HeroTargetCheck(myTransform);
				CenterHero ();

				//Check to see if the hero is sitting in the enemy's home row
				ScoreCheckMove(gameObject);

				if (!usingHaste) {
					//Run MoveHeroes() again to check if there are any more heroes to move. MAKE SURE THIS IS RUN AFTER ScoreCheck() OR ELSE THERE WILL BE PROBLEMS!!! This is because
					//MoveHeroes() will change which player's turn it is and ScoreCheck() relies on it remaining the current players turn to work. Thanks future Derek!
					playField.MoveHeroes();
				}

				if (usingHaste) {
					usingHaste = !usingHaste;
				}
			}
		} else if (movingLeft) {
			if (transform.position.x > distToMove) {
				transform.Translate(Vector2.left * Time.deltaTime * 3);
				myAnimator.SetTrigger("isWalking");

			} else {
				movingLeft = false;
				myAnimator.ResetTrigger("isWalking");
				//This is where heroes ATTACK (or more specifically this is where heroes run through their targeting logic and do w/e it is they do to all of their valid targets)
				playField.HeroTargetCheck(myTransform);
				CenterHero ();

				//Check to see if the hero is sitting in the enemy's home row
				ScoreCheckMove(gameObject);

				if (!usingHaste) {
					//Run MoveHeroes() again to check if there are any more heroes to move. MAKE SURE THIS IS RUN AFTER ScoreCheck() OR ELSE THERE WILL BE PROBLEMS!!! This is because
					//MoveHeroes() will change which player's turn it is and ScoreCheck() relies on it remaining the current players turn to work. Thanks future Derek!
					playField.MoveHeroes();
				}

				if (usingHaste) {
					usingHaste = !usingHaste;
				}
			}
		}
	}

	public void TakeDamage (int dmg) {
		hero.currentHealth -= dmg;
		if (hero.currentHealth <= 0) {
//			Debug.Log("RUNNING HERO DEATH CODE");
			ScoreCheckDeath(gameObject);
			Destroy(gameObject);
		}
		healthDisplay.text = hero.currentHealth.ToString();
		Instantiate (hitParticle, hero.transform, false);
	}

	public void HealFull () {
		hero.currentHealth += (hero.maxHealth - hero.currentHealth);
		healthDisplay.text = hero.currentHealth.ToString();
	}

	public void HealPartial (int amt) {
		hero.currentHealth += amt;
		if (hero.currentHealth > hero.maxHealth) {
			hero.currentHealth = hero.maxHealth;
		}
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

	void CenterHero () //Make sure that the hero is sitting on a rounded X position 
	{
		Vector3 currentPos = transform.position;
		currentPos.x = Mathf.RoundToInt (transform.position.x);
		transform.position = currentPos;
	}

	public void SetTransform (Transform hero) {
		hero = myTransform;
	}

	void ScoreCheckMove (GameObject hero) {
		if (playField.player1Turn) {
			if (hero.transform.position.x >= player2HomeColumn) {
				playField.LosePlayerHealth(hero.GetComponent<Hero>().playerDamage);
				Destroy(hero);
			}
		} else if (!playField.player1Turn) {
			if (hero.transform.position.x <= player1HomeColumn) {
				playField.LosePlayerHealth(hero.GetComponent<Hero>().playerDamage);
				Destroy(hero);
			}
		}
	}

	void ScoreCheckDeath (GameObject hero) {
		if (hero.tag == "player1") {
			if (hero.transform.position.x <= player1HomeColumn) {
				playField.LosePlayerHealth(hero.GetComponent<Hero>().playerDamage);
			}
		} else if (hero.tag == "player2") {
			if (hero.transform.position.x >= player2HomeColumn) {
				playField.LosePlayerHealth(hero.GetComponent<Hero>().playerDamage);
			}
		}
	}
}