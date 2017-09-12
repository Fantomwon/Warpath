using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour {

	public Text armorDisplay;
	public Text healthDisplay;
	public string id;
	public int maxHealth;
	public int currentHealth;
	public int currentArmor;
	public int power;
	public int speed;
	public int range;
	public int healValue;
	public float dodgeChance;
	public int playerDamage;
	public bool movingRight = false;
	public bool movingLeft = false;
	public bool usingHaste = false;
	public GameObject cardPrefab;
	public ParticleSystem hitParticle, healParticle;
	public AnimationClip attackRightAnim;
	public AnimationClip attackLeftAnim;
	public List<GameObject> buffList;

	private float distToMove = 1f;
	private PlayField playField;
	private Transform myTransform;
	private Hero hero;
	private Animator myAnimator;

	// Use this for initialization
	void Start () {
		hero = GetComponent<Hero>();
		UpdateArmorDisplay();
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

				if (playField.heroAttackedATarget) {
					StartCoroutine( "PlayAttackAnimation");
				} else if (usingHaste) {
					usingHaste = !usingHaste;
				} else if (!playField.heroAttackedATarget && !usingHaste) {
					//Run MoveHeroes() again to check if there are any more heroes to move. MAKE SURE THIS IS RUN AFTER ScoreCheck() OR ELSE THERE WILL BE PROBLEMS!!! This is because
					//MoveHeroes() will change which player's turn it is and ScoreCheck() relies on it remaining the current players turn to work. Thanks future Derek!
					playField.MoveHeroes();
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
//				Debug.LogWarning("HERO ATTACKED A TARGET: " + playField.heroAttackedATarget);
				CenterHero ();

				//Check to see if the hero is sitting in the enemy's home row
				ScoreCheckMove(gameObject);

				if (playField.heroAttackedATarget) {
					StartCoroutine( "PlayAttackAnimation");
				} else if (usingHaste) {
					usingHaste = !usingHaste; 
				} else if (!playField.heroAttackedATarget && !usingHaste) {
					//Run MoveHeroes() again to check if there are any more heroes to move. MAKE SURE THIS IS RUN AFTER ScoreCheck() OR ELSE THERE WILL BE PROBLEMS!!! This is because
					//MoveHeroes() will change which player's turn it is and ScoreCheck() relies on it remaining the current players turn to work. Thanks future Derek!
					playField.MoveHeroes();
				}
			}
		}
	}

	IEnumerator PlayAttackAnimation () {
		if (playField.player1Turn) {
			myAnimator.SetTrigger("attackRight");
		} else if (!playField.player1Turn) {
			myAnimator.SetTrigger("attackLeft");
		}
		HeroAttackEffects ();
		yield return new WaitForSeconds(attackRightAnim.length + 0.2f);
		playField.heroAttackedATarget = false;
		if (!usingHaste) {
			playField.MoveHeroes();
		} else if (usingHaste) {
			usingHaste = !usingHaste;
		}
	}

	//These are the things that we want heroes to do ONLY when they attack
	void HeroAttackEffects () {
		if (id == "bloodmage") {
			hero.HealPartial(hero.healValue);
		}
	}

	public void TakeDamage (int dmg) {
		if (currentArmor > 0 && (currentArmor - dmg) >= 0) {
			hero.currentArmor -= dmg;
		} else if (currentArmor > 0) {
			hero.currentHealth -= (dmg - hero.currentArmor);
			hero.currentArmor = 0;
		} else {
			hero.currentHealth -= dmg;
		}
		if (hero.currentHealth <= 0) {
//			Debug.Log("RUNNING HERO DEATH CODE");
			ScoreCheckDeath(gameObject);
			Destroy(gameObject);
		}
		Instantiate (hitParticle, hero.transform.position, Quaternion.identity);
		UpdateArmorDisplay();
		healthDisplay.text = hero.currentHealth.ToString();
		myAnimator.SetTrigger("hitReact");
	}

	public void DodgeDamage () {
		Debug.LogWarning("DODGING DAMAGE");
		Instantiate (hitParticle, hero.transform.position, Quaternion.identity);
		myAnimator.SetTrigger("hitReact");
	}

//	public void ResetHitReactTrigger () {
//		myAnimator.ResetTrigger("hitReact");
//	}

	public void HealFull () {
		hero.currentHealth += (hero.maxHealth - hero.currentHealth);
		healthDisplay.text = hero.currentHealth.ToString();
	}

	public void HealPartial (int amt) {
		//Only play the heal particle if the hero is below max health/if they will actually be healed
		if (hero.currentHealth < hero.maxHealth) {
			Instantiate (healParticle, hero.transform.position, Quaternion.identity);
		}
		hero.currentHealth += amt;
		//Prevent from healing over a hero's max health
		if (hero.currentHealth > hero.maxHealth) {
			hero.currentHealth = hero.maxHealth;
		}
		healthDisplay.text = hero.currentHealth.ToString();
	}

	public void AddArmor (int amt) {
		hero.currentArmor += amt;
		UpdateArmorDisplay();
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
			if (hero.transform.position.x >= playField.player2HomeColumn) {
				playField.LosePlayerHealth(hero.GetComponent<Hero>().playerDamage);
				Destroy(hero);
			}
		} else if (!playField.player1Turn) {
			if (hero.transform.position.x <= playField.player1HomeColumn) {
				playField.LosePlayerHealth(hero.GetComponent<Hero>().playerDamage);
				Destroy(hero);
			}
		}
	}

	void ScoreCheckDeath (GameObject hero) {
		if (hero.tag == "player1") {
			if (hero.transform.position.x <= playField.player1HomeColumn) {
				playField.LosePlayerHealth(hero.GetComponent<Hero>().playerDamage);
			}
		} else if (hero.tag == "player2") {
			if (hero.transform.position.x >= playField.player2HomeColumn) {
				playField.LosePlayerHealth(hero.GetComponent<Hero>().playerDamage);
			}
		}
	}

	void UpdateArmorDisplay () {
		if (currentArmor <= 0) {
			hero.transform.FindChild("Armor").GetComponent<Text>().enabled = false;
		} else if (currentArmor > 0) {
			hero.transform.FindChild("Armor").GetComponent<Text>().enabled = true;
		}

		armorDisplay.text = hero.currentArmor.ToString();
	}
}