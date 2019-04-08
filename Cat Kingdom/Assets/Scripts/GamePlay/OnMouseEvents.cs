using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] UnityEvent onMouseOver;
	[SerializeField] UnityEvent onMouseExit;

	public UnityEvent OnMouseOver {
		get {
			return this.onMouseOver;
		}
		set {
			onMouseOver = value;
		}
	}

	public UnityEvent OnMouseExit {
		get {
			return this.onMouseExit;
		}
		set {
			onMouseExit = value;
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		onMouseOver.Invoke ();
	}

	public void OnPointerExit(PointerEventData eventData) {
		onMouseExit.Invoke ();
	}
}
