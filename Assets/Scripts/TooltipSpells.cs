using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TooltipSpells : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler 
{

	private float holdTime = 0.5f;
	private bool spellSelected;

	public UnityEvent onLongPress = new UnityEvent();

	public void OnPointerDown(PointerEventData eventData) {
		Debug.Log("ON POINTER DOWN ");
		if (EventSystem.current.currentSelectedGameObject.GetComponent<Card>().type == "Spell") {
			Invoke("OnLongPress", holdTime);
		}
	}

	public void OnPointerUp(PointerEventData eventData) {
		if (spellSelected) {
			EventSystem.current.currentSelectedGameObject.transform.FindChild("Tooltip").GetComponent<CanvasGroup>().alpha = 0;
		}
		CancelInvoke("OnLongPress");
		spellSelected = false;
	}

	public void OnPointerExit(PointerEventData eventData) {
		if (spellSelected) {
			EventSystem.current.currentSelectedGameObject.transform.FindChild("Tooltip").GetComponent<CanvasGroup>().alpha = 0;
		}
		CancelInvoke("OnLongPress");
		spellSelected = false;
	}

	private void OnLongPress () {
		Debug.Log("LONG PRESS ACHIEVED");
		EventSystem.current.currentSelectedGameObject.transform.FindChild("Tooltip").GetComponent<CanvasGroup>().alpha = 1;
		spellSelected = true;
		onLongPress.Invoke();
	}

}