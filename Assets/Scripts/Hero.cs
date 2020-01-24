using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour {

	public Text armorDisplay;
	public Image armorImage;
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
	public bool moveRightAndAttack = false;
	public bool moveLeftAndAttack = false;
	public bool moveRight = false;
	public bool moveLeft = false;
	public bool usingHaste = false;
	public GameObject cardPrefab;
	public ParticleSystem hitParticle, healParticle;
	public GameObject dodgeParticleGameObject;
	public AnimationClip attackRightAnim;
	public AnimationClip attackLeftAnim;
	public Transform buffList;
	public GameObject attackTellBox;
	public GameObject combatText;

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
		OnSpawnEffects ();
	}
	
	// Update is called once per frame
	void Update () {
		if (moveRightAndAttack) {
			if (transform.position.x < distToMove) {
				transform.Translate(Vector2.right * Time.deltaTime * 3);
				myAnimator.SetTrigger("isWalking");
			} else {
				moveRightAndAttack = false;
				myAnimator.ResetTrigger("isWalking");
				//This is where heroes ATTACK (or more specifically this is where heroes run through their targeting logic and do w/e it is they do to all of their valid targets)
				CheckForValidTargetsToAttack(myTransform);
                //Debug.Log("heroAttackedATarget is " + playField.heroAttackedATarget);
				CenterHero ();

                //WE CAN PROBABLY DELETE THIS SHIT BELOW
                //Check to see if the hero is sitting in the enemy's home row
                //ScoreCheckMove(gameObject);

                if (!playField.heroAttackedATarget && usingHaste) {
					usingHaste = !usingHaste;
				} else if (!playField.heroAttackedATarget && !usingHaste) {
					//Run MoveHeroes() again to check if there are any more heroes to move. MAKE SURE THIS IS RUN AFTER ScoreCheck() OR ELSE THERE WILL BE PROBLEMS!!! This is because
					//MoveHeroes() will change which player's turn it is and ScoreCheck() relies on it remaining the current players turn to work. Thanks future Derek!
					playField.MoveHeroes();
				}
			}
		} else if (moveLeftAndAttack) {
			if (transform.position.x > distToMove) {
				transform.Translate(Vector2.left * Time.deltaTime * 3);
				myAnimator.SetTrigger("isWalking");
			} else {
				moveLeftAndAttack = false;
				myAnimator.ResetTrigger("isWalking");
				//This is where heroes ATTACK (or more specifically this is where heroes run through their targeting logic and do w/e it is they do to all of their valid targets)
				CheckForValidTargetsToAttack(myTransform);
				//Debug.LogWarning("HERO ATTACKED A TARGET: " + playField.heroAttackedATarget);
				CenterHero ();

                //WE CAN PROBABLY DELETE THIS SHIT BELOW
                //Check to see if the hero is sitting in the enemy's home row
                //ScoreCheckMove(gameObject);

                if (!playField.heroAttackedATarget && usingHaste) {
					usingHaste = !usingHaste; 
				} else if (!playField.heroAttackedATarget && !usingHaste) {
					//Run MoveHeroes() again to check if there are any more heroes to move. MAKE SURE THIS IS RUN AFTER ScoreCheck() OR ELSE THERE WILL BE PROBLEMS!!! This is because
					//MoveHeroes() will change which player's turn it is and ScoreCheck() relies on it remaining the current players turn to work. Thanks future Derek!
					playField.MoveHeroes();
				}
			}
		} else if (moveRight) {
			if (transform.position.x < distToMove) {
				transform.Translate(Vector2.right * Time.deltaTime * 3);
				myAnimator.SetTrigger("isWalking");
			} else {
				moveRight = false;
				myAnimator.ResetTrigger("isWalking");
				CenterHero ();
			}
		} else if (moveLeft) {
			if (transform.position.x > distToMove) {
				transform.Translate(Vector2.left * Time.deltaTime * 3);
				myAnimator.SetTrigger("isWalking");
			} else {
				moveLeft = false;
				myAnimator.ResetTrigger("isWalking");
				CenterHero ();
			}
		}
	}

	//Checks to see if there are any valid targets for the hero to attack. If there are any valid targets then the hero plays the 'PlayAttackAnimation' coroutine.
	public void CheckForValidTargetsToAttack (Transform currentHero) {
        //Debug.Log("RUNNING CheckForValidTargetsToAttack()");
        if (currentHero.GetComponent<Hero>().id == "rogue" && playField.TargetCheckCardinalDirections(currentHero, "enemy").Count > 0) {
            StartCoroutine("PlayAttackAnimation");
        } else if (currentHero.GetComponent<Hero>().id == "tower" && playField.TargetCheckAllDirections(currentHero, "enemy", null).Count > 0) {
            StartCoroutine("PlayAttackAnimation");
        } else if (currentHero.GetComponent<Hero>().id == "archer" && playField.TargetCheckAllHeroesInRange(currentHero, "enemy").Count > 0) {
            StartCoroutine("PlayAttackAnimation");
        } else if (currentHero.GetComponent<Hero>().id == "sapper" && playField.TargetCheckAllDirections(currentHero, "enemy", null).Count > 0) {
            StartCoroutine("PlayAttackAnimation");
        } else if (currentHero.GetComponent<Hero>().id == "chaosmage" && playField.TargetCheckEntireBoardTwoRandomHeroes(currentHero, "enemy").Count > 0) {
            StartCoroutine("PlayAttackAnimation");
        } else if (playField.TargetCheckClosestHeroInRange(currentHero, "enemy").Count > 0) {
            StartCoroutine("PlayAttackAnimation");
        } else if ((playField.player1Turn && currentHero.GetComponent<Hero>().range >= playField.player2HomeColumn - currentHero.transform.position.x) || (!playField.player1Turn && currentHero.GetComponent<Hero>().range >= currentHero.transform.position.x - playField.player1HomeColumn)) {
            //We are attacking the enemy Summoner here
            playField.heroAttackedATarget = true;
            StartCoroutine("PlayAttackAnimation");
        }
        //WE CAN PROBABLY DELETE THIS SHIT BELOW
        //else if ((playField.player1Turn && currentHero.transform.position.x == playField.player2HomeColumn - 1) || (!playField.player1Turn && currentHero.transform.position.x == playField.player1HomeColumn + 1)) {
        //    //We are attacking the enemy Summoner here
        //    playField.heroAttackedATarget = true;
        //    StartCoroutine("PlayAttackAnimation");
        //}
    }

	IEnumerator PlayAttackAnimation () {
		TargetTellTurnOn();
		yield return new WaitForSeconds(0.5f);
		//Damage is dealt through an animation event
		if (playField.player1Turn) {
			myAnimator.SetTrigger("attackRight");
		} else if (!playField.player1Turn) {
			myAnimator.SetTrigger("attackLeft");
		}
		yield return new WaitForSeconds(0.5f);
		AfterAttackOperations ();
	}

	public void AfterAttackOperations () {
		//Debug.Log("HERO ATTACK FINISHED");
		playField.heroAttackedATarget = false;
		if (!usingHaste) {
			//Debug.Log("RUN MoverHeroes() - NOT USING HASTE");
			playField.MoveHeroes();
		} else if (usingHaste) {
			Debug.Log("RUN MoverHeroes() - USING HASTE");
			usingHaste = !usingHaste;
		}
		AttackTellTurnOff();
	}

	void TargetTellTurnOn () {
		//Turn on the tell boxes that show where the hero is trying to attack
		if (playField.player1Turn) {
			hero.transform.Find("TargetTellRight").GetComponent<CanvasGroup>().alpha = 1;
		} else if (!playField.player1Turn) {
			hero.transform.Find("TargetTellLeft").GetComponent<CanvasGroup>().alpha = 1;
		}

		//Spawn the tell boxes that show where the hero is actually going to attack
		if (hero.id == "rogue") {
			foreach (Transform enemy in playField.TargetCheckCardinalDirections(hero.transform, "enemy")) {
				GameObject spawnedAttackTellBox = Instantiate (attackTellBox, enemy.transform.position, Quaternion.identity, FindObjectOfType<AttackTellBoxes>().transform) as GameObject;
				spawnedAttackTellBox.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
			}
		} else if (hero.id == "tower") {
			foreach (Transform enemy in playField.TargetCheckAllDirections(hero.transform, "enemy", null)) {
				GameObject spawnedAttackTellBox = Instantiate (attackTellBox, enemy.transform.position, Quaternion.identity, FindObjectOfType<AttackTellBoxes>().transform) as GameObject;
				spawnedAttackTellBox.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
			}
		} else if (hero.id == "archer") {
			foreach (Transform enemy in playField.TargetCheckAllHeroesInRange(hero.transform, "enemy")) {
				GameObject spawnedAttackTellBox = Instantiate (attackTellBox, enemy.transform.position, Quaternion.identity, FindObjectOfType<AttackTellBoxes>().transform) as GameObject;
				spawnedAttackTellBox.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
			}
		} else if (hero.id == "chaosmage") {
			foreach (Transform enemy in playField.tempTransformList) {
				GameObject spawnedAttackTellBox = Instantiate (attackTellBox, enemy.transform.position, Quaternion.identity, FindObjectOfType<AttackTellBoxes>().transform) as GameObject;
				spawnedAttackTellBox.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
			}
		} else {
			foreach (Transform enemy in playField.TargetCheckClosestHeroInRange(hero.transform, "enemy")) {
				GameObject spawnedAttackTellBox = Instantiate (attackTellBox, enemy.transform.position, Quaternion.identity, FindObjectOfType<AttackTellBoxes>().transform) as GameObject;
				spawnedAttackTellBox.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
			}
		}
	}

	void AttackTellTurnOff () {
		hero.transform.Find("TargetTellRight").GetComponent<CanvasGroup>().alpha = 0;
		hero.transform.Find("TargetTellLeft").GetComponent<CanvasGroup>().alpha = 0;

		foreach (Transform attackTellBox in FindObjectOfType<AttackTellBoxes>().transform) {
			Destroy (attackTellBox.gameObject);
		}
	}

	//These are the things that we want heroes to do ONLY when they attack
	public void HeroAttackEffects () {
		if (id == "bloodmage") {
			hero.HealPartial(hero.healValue);
		}

//		if (id == "sapper") {
//			hero.TakeDamage(hero.GetComponent<Hero>().currentHealth);
//		}
	}

	public void TakeDamage (int dmg) {
		if (hero.GetComponent<Hero>().id == "ghost") {
			dmg = 1;
		}

		if (currentArmor > 0 && (currentArmor - dmg) >= 0) {
			hero.currentArmor -= dmg;
		} else if (currentArmor > 0) {
			hero.currentHealth -= (dmg - hero.currentArmor);
			hero.currentArmor = 0;
		} else {
			hero.currentHealth -= dmg;
		}

		//If the hero's health is zero or below check if their death causes the enemy to score a point and also destroy them.
		if (hero.currentHealth <= 0) {
//			Debug.Log("RUNNING HERO DEATH CODE");
            //DISABLING THIS FOR THE WIN CONDITION PROTOTYPE
			//ScoreCheckDeath(gameObject);
			Destroy(gameObject);
		}
		Instantiate (hitParticle, hero.transform.position, Quaternion.identity);
		UpdateArmorDisplay();
		healthDisplay.text = hero.currentHealth.ToString();
		myAnimator.SetTrigger("hitReact");

		GameObject x = Instantiate(combatText, new Vector3 (hero.transform.position.x, hero.transform.position.y + 0.5f, hero.transform.position.z), Quaternion.identity) as GameObject;
		x.GetComponentInChildren<TextMesh>().text = "-" + dmg.ToString();
		x.GetComponentInChildren<TextMesh>().color = Color.red;
	}

	public void DodgeDamage () {
		Debug.LogWarning("DODGING DAMAGE");
		Instantiate (dodgeParticleGameObject, hero.transform.position, Quaternion.identity);
		myAnimator.SetTrigger("dodge");

		GameObject x = Instantiate(combatText, new Vector3 (hero.transform.position.x, hero.transform.position.y + 0.5f, hero.transform.position.z), Quaternion.identity) as GameObject;
		x.GetComponentInChildren<TextMesh>().text = "Dodge!";
	}

	public void HealFull () {
		hero.currentHealth += (hero.maxHealth - hero.currentHealth);
		healthDisplay.text = hero.currentHealth.ToString();

		GameObject x = Instantiate(combatText, new Vector3 (hero.transform.position.x, hero.transform.position.y + 0.5f, hero.transform.position.z), Quaternion.identity) as GameObject;
		x.GetComponentInChildren<TextMesh>().text = "+" + (hero.maxHealth - hero.currentHealth);
		x.GetComponentInChildren<TextMesh>().color = Color.green;
	}

	public void HealPartial (int amt) {
		//Only play the heal particle if the hero is below max health/if they will actually be healed
		if (hero.currentHealth < hero.maxHealth) {
			Instantiate (healParticle, hero.transform.position, Quaternion.identity);
			GameObject x = Instantiate(combatText, new Vector3 (hero.transform.position.x, hero.transform.position.y + 0.5f, hero.transform.position.z), Quaternion.identity) as GameObject;
			x.GetComponentInChildren<TextMesh>().text = "+" + amt.ToString();
			x.GetComponentInChildren<TextMesh>().color = Color.green;
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

		GameObject x = Instantiate(combatText, new Vector3 (hero.transform.position.x, hero.transform.position.y + 0.5f, hero.transform.position.z), Quaternion.identity) as GameObject;
		x.GetComponentInChildren<TextMesh>().text = "+" + (amt);
		x.GetComponentInChildren<TextMesh>().color = Color.blue;
	}

	public void MoveSingleHeroRightAndAttack(float dist) {
		moveRightAndAttack = true;
		distToMove = dist + transform.position.x;
	}

	public void MoveSingleHeroLeftAndAttack(float dist) {
		moveLeftAndAttack = true;
		distToMove = transform.position.x - dist;
	}

    //Only used in specific instances (like the 'Wind Gust' spell). NOT used when we are iterating through heroes in an 'EndTurn' scenario b/c this will NOT continue to iterate through remaining heroes in a turn.
	public void MoveSingleHeroRight(float dist) {
		moveRight = true;
		distToMove = dist + transform.position.x;
	}

    //Only used in specific instances (like the 'Wind Gust' spell). NOT used when we are iterating through heroes in an 'EndTurn' scenario b/c this will NOT continue to iterate through remaining heroes in a turn.
    public void MoveSingleHeroLeft(float dist) {
		moveLeft = true;
		distToMove = transform.position.x - dist;
	}

    //Make sure that the hero is sitting on a rounded X position 
    void CenterHero () {
		Vector3 currentPos = transform.position;
		currentPos.x = Mathf.RoundToInt (transform.position.x);
		transform.position = currentPos;
	}

	public void SetTransform (Transform hero) {
		hero = myTransform;
	}

	void ScoreCheckMove (GameObject hero) {
		if (playField.player1Turn) {
			if (Mathf.RoundToInt(hero.transform.position.x) >= playField.player2HomeColumn) {
				playField.LosePlayerHealth(hero.GetComponent<Hero>().playerDamage);
				Destroy(hero);
				//Need to set this to false here or else the bool will be set to true if 'CheckForValidTargetsToAttack' returns any valid targets. CheckForValidTargetsToAttack still gets run even if the hero scores and doesn't attack.
				playField.heroAttackedATarget = false;
			}
		} else if (!playField.player1Turn) {
			if (Mathf.RoundToInt(hero.transform.position.x) <= playField.player1HomeColumn) {
				playField.LosePlayerHealth(hero.GetComponent<Hero>().playerDamage);
				Destroy(hero);
				//Need to set this to false here or else the bool will be set to true if 'CheckForValidTargetsToAttack' returns any valid targets. CheckForValidTargetsToAttack still gets run even if the hero scores and doesn't attack.
				playField.heroAttackedATarget = false;
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
			//hero.transform.Find("Armor").GetComponent<Text>().enabled = false;
			armorDisplay.enabled = false;
			if (armorImage != null) {
				armorImage.enabled = false;
			}
		} else if (currentArmor > 0) {
			//hero.transform.Find("Armor").GetComponent<Text>().enabled = true;
			armorDisplay.enabled = true;
			if (armorImage != null) {
				armorImage.enabled = true;
			}
		}

		armorDisplay.text = hero.currentArmor.ToString();
	}

	void OnSpawnEffects () {
		if (id == "wolf") {
			usingHaste = true;
			if (playField.player1Turn) {
				playField.Player1MoveHasteCheck(myTransform);
			} else if (!playField.player1Turn) {
				playField.Player2MoveHasteCheck(myTransform);
			}
		}
	}
}