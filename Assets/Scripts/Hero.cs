using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hero : MonoBehaviour {

	public Text healthDisplay;
	public int health = 10;
	public int damage = 3;
	public int speed = 1;
	public int range = 1;
	public bool movingRight = false;
	public bool movingLeft = false;
	public GameObject cardPrefab;

	private Animator animator;
	private float distToMove = 1f;
	private PlayField playField;
	private Transform myTransform;

	// Use this for initialization
	void Start () {
		healthDisplay.text = GetComponent<Hero>().health.ToString();
		playField = GameObject.FindObjectOfType<PlayField>();
		myTransform = GetComponent<Transform>();
		animator = GetComponent<Animator>();
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
		GetComponent<Hero>().health -= dmg;
		if (GetComponent<Hero>().health <= 0) {
			Destroy(gameObject);
		}
		healthDisplay.text = GetComponent<Hero>().health.ToString();
	}

	public void MoveSingleHeroRight(float dist) {
		movingRight = true;
		distToMove = dist + transform.position.x;
	}

	public void MoveSingleHeroLeft(float dist) {
		movingLeft = true;
		distToMove = transform.position.x - dist;
	}

	public void SetTransform (Transform hero) {
		hero = myTransform;
	}
}