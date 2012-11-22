using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	
	public Word word;
	public Letter letter;
	string key;
	public GameObject letterFocus;
	public GameObject clueFocus;
	public GameObject clock;
	public GameObject scoreboard;
	public GameObject menu;
	
	const int _PlayGame = 0;
	const int _EnterName = 1;
	const int _ScoreBoard = 2;
	const int _Menu = 3;
	const int _View = 4;
	public int gameMode = 0;
	int lastMode = 0;
	private TouchScreenKeyboard touchScreenKeyboard;
	
	void Awake()
	{
		TouchScreenKeyboard.hideInput = true;
	}
	void GridSize (Vector2 gridSize) {
		Menu();
		Menu();
	}
	/* modes
	 * Type 0
	 * Click 1
	 * Clue 2
	 */
	public void SetLetterFocus(Transform l, int mode)
	{
		if(mode==2)
		{
			if(word!=null)
				if(word!=l.parent.GetComponent<Word>())
					word.SetFocus(false);
			letter = l.GetComponent<Letter>();
			word = l.parent.GetComponent<Word>();
			if(gameMode!=_View)
				letterFocus.transform.position = l.position;
			word.SetFocus(true);
			clueFocus.transform.position = word.clueScript.transform.position;
			clueFocus.transform.localScale = (word.clueScript.GetComponent<BoxCollider>()).size/0.002604167f*20f;
		}
		if(letter==null&&word!=null)
		{
			word.SetFocus(false);
			word = null;
		}
		else
		{
			Letter newLetter = l.GetComponent<Letter>();
			// if clicked/pressed
			if(mode==1)
			{
				// if you click another letter that isn't in the word
				if(newLetter.transform.parent.GetComponent<Word>() != word)
				{
					// and it has a twin
					if(newLetter.twin != null)
					{
						// if the twin of clicked is current letter, swap to clicked from new word
						if(newLetter.twin != letter)
						{
							newLetter = newLetter.twin;
						}
					}
				}
				// if you click the same letter
				else if(newLetter == letter)
				{
					if(letter.twin != null)
						newLetter = letter.twin;
				}
			}
			// set letter and focus
			letter = newLetter;
			if(gameMode!=_View)
				letterFocus.transform.position = l.position;
			
			// sets word and clue focus if selecting another word
			if(word!=null)
				if(letter.transform.parent.GetComponent<Word>() != word)
					word.SetFocus(false);
			word = letter.transform.parent.GetComponent<Word>();
			word.SetFocus(true);
			clueFocus.transform.position = word.clueScript.transform.position;
			clueFocus.transform.localScale = (word.clueScript.GetComponent<BoxCollider>()).size/0.002604167f*20f;
		}
	}
	void Backspace()
	{
		if(letter!=null&&gameMode==_PlayGame)
		{
			if(letter.guess.CompareTo("")==0)
			{
				if(letter.previous!=null)
				{
					letter = letter.previous;
					letter.SetLetter("");
				}
			}
			else
			{
				letter.SetLetter("");
			}
		}
		else if(gameMode==_EnterName)
		{
			scoreboard.SendMessage("Backspace");
		}
	}
	void Enter()
	{
		if(gameMode==_EnterName)
		{
			gameMode = _ScoreBoard;
			scoreboard.SendMessage("NameEntered");
		}
		else if(gameMode==_ScoreBoard)
		{
			gameMode = _View;
			scoreboard.SendMessage("View");
			menu.GetComponent<GameMenu>().View();
		}
		else
		{
			if(word!=null)
				word.firstLetter.SetFocus(false);
			letter = null;
		}
	}
	void MoveLeft()
	{
		if(letter!=null)
		{
			if(letter.previous!=null&&word.direction.CompareTo("across")==0)
				letter = letter.previous;
			else if(letter.twin!=null)
			{
				if(letter.twin.previous!=null&&word.direction.CompareTo("down")==0)
				{	
					letter = letter.twin.previous;
					//word = letter.transform.parent.GetComponent<Word>();
				}
			}
		}
	}
	void MoveRight()
	{
		if(letter!=null)
		{
			if(letter.next!=null&&word.direction.CompareTo("across")==0)
				letter = letter.next;
			else if(letter.twin!=null)
			{
				if(letter.twin.next!=null&&word.direction.CompareTo("down")==0)
				{	
					letter = letter.twin.next;
					//word = letter.transform.parent.GetComponent<Word>();
				}
			}
		}
	}
	void MoveUp()
	{
		if(letter!=null)
		{
			if(letter.previous!=null&&word.direction.CompareTo("down")==0)
				letter = letter.previous;
			else if(letter.twin!=null)
			{
				if(letter.twin.previous!=null&&word.direction.CompareTo("across")==0)
				{	
					letter = letter.twin.previous;
					//word = letter.transform.parent.GetComponent<Word>();
				}
			}
		}
	}
	void MoveDown()
	{
		if(letter!=null)
		{
			if(letter.next!=null&&word.direction.CompareTo("down")==0)
				letter = letter.next;
			else if(letter.twin!=null)
			{
				if(letter.twin.next!=null&&word.direction.CompareTo("across")==0)
				{	
					letter = letter.twin.next;
					//word = letter.transform.parent.GetComponent<Word>();
				}
			}
		}
	}
	public void Solve()
	{
		if(gameMode==_PlayGame)
		{
			letter = null;
			bool solved = true;
			foreach(Word solverWord in GetComponentsInChildren<Word>())
			{
				if(!solverWord.CheckAnswer())
				{
					solved = false;
				}
			}
			if(solved)
			{
				foreach(Word word in GetComponentsInChildren<Word>())
				{
					word.SetFocus(false);
				}
				letter = null;
				SetMainFocus();
				gameMode = _EnterName;
				menu.GetComponent<GameMenu>().Solved();
				clock.BroadcastMessage("GameOver");
				scoreboard.SetActive(true);
				scoreboard.SendMessage("SetTime",clock.GetComponentInChildren<Clock>().currentTime);
				scoreboard.SendMessage("GameOver");
			}
		}
	}
	public void Menu()
	{
		if(Time.timeScale==1)
		{
			lastMode = gameMode;
			gameMode = _Menu;
			Time.timeScale = 0;
			menu.SetActive(true);
			foreach(Word word in GetComponentsInChildren<Word>())
			{
				word.EnableButtons(false);
			}
		}
		else
		{
			gameMode = lastMode;
			Time.timeScale = 1;
			menu.SetActive(false);
			foreach(Word word in GetComponentsInChildren<Word>())
			{
				word.EnableButtons(true);
			}
		}
	}
	void SetCurrentLetter(string l)
	{
		if(gameMode==_PlayGame)
		{
			l = l.ToUpper();
			if(letter!=null)
			{
				letter.SetLetter(l);
				if(letter.next!=null)
					letter = letter.next;
			}
		}
		else if(gameMode==_EnterName)
		{
			scoreboard.SendMessage("AppendName",l);
		}
	}
	void SetMainFocus()
	{
		if(letter!=null)
		{
			SetLetterFocus(letter.transform,0);
		}
		else
		{
			letterFocus.transform.position =  new Vector3(1000f,1000f,-100f);
			clueFocus.transform.position = new Vector3(1000f,1000f,-100f);
		}
	}
	void KeyboardType()
	{
		if(gameMode!=_Menu&&gameMode!=_View)
		{
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				MoveLeft();
				SetMainFocus();
			}
			else if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				MoveUp();
				SetMainFocus();
			}
			else if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				MoveRight();
				SetMainFocus();
			}
			else if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				MoveDown();
				SetMainFocus();
			}
			else if(Input.GetKeyDown(KeyCode.Return))
			{
				Enter();
				SetMainFocus();
			}
			else if(Input.GetKeyDown(KeyCode.Backspace))
			{
				Backspace();
				SetMainFocus();
			}
			else if(Input.anyKeyDown)
			{
				foreach(char c in Input.inputString)
				{
					if((c>='a'&&c<='z')||(c>='A'&&c<='Z'))
					{
						SetCurrentLetter(c+"");
						SetMainFocus();
					}
				}
			}
		}
	}
	void OnScreenKeyboardType()
	{
		if(gameMode!=_Menu&&gameMode!=_View)
		{
			if(touchScreenKeyboard!=null)
			{
				if(touchScreenKeyboard.text.Length>1)
				{
					char c = touchScreenKeyboard.text.ToCharArray()[1];
					if((c>='a'&&c<='z')||(c>='A'&&c<='Z'))
					{
						touchScreenKeyboard.text = "0";
						SetCurrentLetter(c+"");
						SetMainFocus();
					}
				}
				if(touchScreenKeyboard.text.Length<1)
				{
					touchScreenKeyboard.text = "0";
					Backspace();
					SetMainFocus();
				}
				if(touchScreenKeyboard.done)
				{
					touchScreenKeyboard = null;
					Enter();
					SetMainFocus();
				}
			}
		}
	}
	public void ButtonPress(string buttonName)
	{
		if(buttonName.CompareTo("Menu")==0)
		{
			if(gameMode == _PlayGame || gameMode == _Menu || gameMode == _View)
				Menu();
		}
		else if(buttonName.CompareTo("Solve")==0)
		{
			Solve();
		}
		else if(buttonName.CompareTo("Confirm")==0)
		{
			if(gameMode == _Menu)
			{
				Time.timeScale = 1;
				Application.LoadLevel("menu");
			}
		}
		else if(buttonName.CompareTo("Cancel")==0)
		{
			if(gameMode == _Menu)
			{
				Menu();
			}
		}
		else if(buttonName.CompareTo("Keyboard")==0)
		{
			if(touchScreenKeyboard == null && letter != null)
			{
    			touchScreenKeyboard = TouchScreenKeyboard.Open("0", TouchScreenKeyboardType.Default);
			}
			else if(touchScreenKeyboard == null && gameMode == _EnterName)
			{
    			touchScreenKeyboard = TouchScreenKeyboard.Open("0", TouchScreenKeyboardType.Default);
			}
		}
		else if(buttonName.CompareTo("Scoreboard")==0)
		{
			if(gameMode == _EnterName)
			{
				if(touchScreenKeyboard == null)
				{
	    			touchScreenKeyboard = TouchScreenKeyboard.Open("0", TouchScreenKeyboardType.Default);
				}
				else
				{
					touchScreenKeyboard.active = false;
					touchScreenKeyboard = null;
					Enter();
				}
			}
			else if(gameMode == _ScoreBoard)
			{
				Enter();
			}
		}
	}
	void Update () {
		KeyboardType();
		OnScreenKeyboardType();
	}
}
