using UnityEngine;
using System.Collections;

public class Clue : MonoBehaviour {
	
	public Word word;
	public int lineLength = 40;
	private UILabel uiLabel;
	private Game game;
	
	public float Init (string clue, Word word) {
		string[] substrings = clue.Split(char.Parse(" "));
		clue = "";
		int position = 0;
		float size = 22f;
		for(int i = 0; i < substrings.Length; i ++)
		{
			if(position + substrings[i].Length > lineLength)
			{
				position = substrings[i].Length + 1;
				clue += "\n" + substrings[i] + " ";
				size += 20f;
			}
			else
			{
				position += substrings[i].Length + 1;
				clue += substrings[i] + " ";
			}
		}
		uiLabel.text = clue;
		if(word!=null)
		{
			this.word = word;
			word.clueScript = this;
		}
		return size;
				
	}
	
	// Use this for initialization
	void Awake () {
		uiLabel = gameObject.GetComponentInChildren<UILabel>() as UILabel;
		game = GameObject.FindWithTag("Game").GetComponent<Game>();
	}
	public void EnableButtons(bool enabled)
	{
		this.GetComponent<UIButton>().isEnabled = enabled;
	}
	void OnClick() {
		game.SetLetterFocus(word.firstLetter.transform,2);
	}
}
