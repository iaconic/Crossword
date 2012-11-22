using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	
	Game game;
	public string buttonName;
	
	void Awake () {
		game = GameObject.FindWithTag("Game").GetComponent<Game>() as Game;
	}
	
	void OnClick()
	{
		game.ButtonPress(buttonName);
	}
}
