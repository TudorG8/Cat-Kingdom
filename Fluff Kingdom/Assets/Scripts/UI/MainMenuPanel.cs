using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour {
	[SerializeField] MainMenuController mainMenu;
	public virtual void MarkAsComplete() {
		mainMenu.CanClick = true;
	}
}
