using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TooltipHeroes : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler 
{

	private float holdTime = 1f;
	private PlayField playField;
	private Vector2 roundedPos;
	private GameObject currentHero;

	public UnityEvent onLongPress = new UnityEvent();

	void Start () {
		playField = GetComponent<PlayField>();
	}

	public void OnPointerDown(PointerEventData eventData) {
//		Debug.Log("ON POINTER DOWN");
		Invoke("OnLongPress", holdTime);
	}

	public void OnPointerUp(PointerEventData eventData) {
		CancelInvoke("OnLongPress");
		if (currentHero) {
			currentHero.transform.FindChild("Tooltip").GetComponent<CanvasGroup>().alpha = 0;
		}

	}

	public void OnPointerExit(PointerEventData eventData) {
		CancelInvoke("OnLongPress");
		if (currentHero) {
			currentHero.transform.FindChild("Tooltip").GetComponent<CanvasGroup>().alpha = 0;
		}
	}

	private void OnLongPress () {
		Debug.Log("LONG PRESS ACHIEVED");

		//Use CalculateWorldPointOfMouseClick method to get the 'raw position' (position in pixels) of where the player clicked in the world
		Vector2 rawPos = playField.CalculateWorldPointOfMouseClick();
		//Use SnapToGrid method to turn rawPos into rounded integer units in world space coordinates
		roundedPos = playField.SnapToGrid(rawPos);

		foreach (Transform hero in playField.player1.transform) {
			if (hero.transform.position.x == roundedPos.x && hero.transform.position.y == roundedPos.y) {
				currentHero = hero.gameObject;
//				Debug.Log("There's a hero here named: " + hero.GetComponent<Hero>().name);
				hero.transform.FindChild("Tooltip").GetComponent<CanvasGroup>().alpha = 1;
			}
		}
		foreach (Transform hero in playField.player2.transform) {
			if (hero.transform.position.x == roundedPos.x && hero.transform.position.y == roundedPos.y) {
				currentHero = hero.gameObject;
				Debug.Log("There's a hero here named: " + hero.GetComponent<Hero>().name);
				hero.transform.FindChild("Tooltip").GetComponent<CanvasGroup>().alpha = 1;
			}
		}

		onLongPress.Invoke();
	}

}