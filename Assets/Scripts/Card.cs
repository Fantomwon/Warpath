using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Card : MonoBehaviour {

	public static GameObject selectedHero;
	public GameObject heroPrefab;
	public static GameObject selectedCard;
	public Text NameText, TypeText, PowerText, HealthText, SpeedText, RangeText;
	public string cardName;
	public string type;
	public int quantity;
	public int spellDamage;

	private PlayField playField;

	// Use this for initialization
	void Start () {
		NameText.text = cardName.ToString();
		TypeText.text = type.ToString();
		playField = FindObjectOfType<PlayField>();
		if (type == "Hero") {
			PowerText.text = heroPrefab.GetComponent<Hero>().power.ToString();
			HealthText.text = heroPrefab.GetComponent<Hero>().maxHealth.ToString();
			SpeedText.text = heroPrefab.GetComponent<Hero>().speed.ToString();
			RangeText.text = heroPrefab.GetComponent<Hero>().range.ToString();
		}
	}

	void OnMouseDown () {
		selectedCard = gameObject;
		if (type == "Hero") {
			selectedHero = heroPrefab;
		}
	}

	public void CastSpell () {
		if (cardName == "Fireball") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If there is an enemy in the square I clicked on then do spell damage to them
					if (hero.transform.position.x == playField.roundedPos.x && hero.transform.position.y == playField.roundedPos.y) {
						hero.GetComponent<Hero>().TakeDamage(spellDamage);
						Destroy(Card.selectedCard);
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If there is an enemy in the square I clicked on then do spell damage to them
					if (hero.transform.position.x == playField.roundedPos.x && hero.transform.position.y == playField.roundedPos.y) {
						hero.GetComponent<Hero>().TakeDamage(spellDamage);
						Destroy(Card.selectedCard);
						return;
					}
				}
			}
			Debug.LogWarning("NOT A VALID TARGET FOR SPELL");
		} else if (cardName == "Heal") {
			if (playField.player1Turn) {
				foreach (Transform hero in playField.player1.transform) {
					//If one of my heroes is in the square I clicked on AND they are below max health then heal them to full
					if (hero.transform.position.x == playField.roundedPos.x && hero.transform.position.y == playField.roundedPos.y && hero.GetComponent<Hero>().currentHealth < hero.GetComponent<Hero>().maxHealth) {
						hero.GetComponent<Hero>().HealFull();
						Destroy(Card.selectedCard);
						return;
					} 
				}
			} else if (!playField.player1Turn) {
				foreach (Transform hero in playField.player2.transform) {
					//If one of my heroes is in the square I clicked on AND they are below max health then heal them to full
					if (hero.transform.position.x == playField.roundedPos.x && hero.transform.position.y == playField.roundedPos.y && hero.GetComponent<Hero>().currentHealth < hero.GetComponent<Hero>().maxHealth) {
						hero.GetComponent<Hero>().HealFull();
						Destroy(Card.selectedCard);
						return;
					}
				}
			}
			Debug.Log("NOT A VALID TARGET FOR SPELL");
		}
	}
}
