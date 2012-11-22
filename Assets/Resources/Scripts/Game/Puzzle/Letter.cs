using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour {
	
	public string guess;
	public string answer;
	public Letter previous;
	public Letter twin;
	public Letter next;
	public bool debug = true;
	public UILabel guessLabel;
	public UILabel answerLabel;
	public UISprite wordFocus;
	public UISprite background;
	private Game game;
	private bool hide;
	
	public void SetFocus(bool f)
	{
		if(f)
		{
			if(hide)
				twin.wordFocus.enabled = true;
			else
				wordFocus.enabled = true;
		}
		else
		{
			if(hide)
				twin.wordFocus.enabled = false;
			else
				wordFocus.enabled = false;
		}
		if(next!=null)
		{
			next.SetFocus(f);
		}
	}
	public void Awake()
	{
		guessLabel = GetComponentInChildren<UILabel>() as UILabel;
		game = GameObject.FindWithTag("Game").GetComponent<Game>();
	}
	public void Init(string answer)
	{
		answer = answer.ToUpper();
		this.answer = answer;
		if(debug)
		{
			this.guess = answer;
			guessLabel.text = answer;
		}
	}
	public void EnableButtons(bool enabled)
	{
		this.GetComponent<UIButton>().isEnabled = enabled;
		if(next!=null)
			next.EnableButtons(enabled);
	}
	public void OnClick()
	{
		game.SetLetterFocus(transform, 1);
	}
	public void SetPrevious(Letter previous)
	{
		this.previous = previous;
		previous.SendMessage("SetNext", this);
	}
	void OnTriggerEnter(Collider collider)
	{
		if(collider.GetComponent<Letter>()!=null)
		{
			twin = collider.transform.GetComponent<Letter>();
			twin.twin = this;
			if(transform.parent.GetComponent<Word>().direction.CompareTo("across")==0)
			{
				hide = true;
				guessLabel.enabled = false;
				background.enabled = false;
			}
			else
			{
				twin.hide = true;
				twin.guessLabel.enabled = false;
				twin.background.enabled = false;
			}
		}
	}
	public void SetNext(Letter next)
	{
		this.next = next;
		if(previous==null)
		{
			UILabel number = (Instantiate(Resources.Load("Objects/BlockNumber"),transform.position,Quaternion.identity) as GameObject).GetComponentInChildren<UILabel>();
			number.transform.parent.parent = transform;
			number.text = transform.parent.gameObject.GetComponent<Word>().index+"";
			number.transform.parent.parent = transform;
		}
	}
	
	public void SetLetter(string letter)
	{
		guess = letter;
		guessLabel.text = letter;
		if(twin!=null)
		{
			twin.SendMessage("SetTwinLetter",letter);
		}
	}
	public void SetTwinLetter(string letter)
	{
		guess = letter;
		guessLabel.text = letter;
	}
	
	public IEnumerator ClearCheckAnswer()
	{
		yield return new WaitForSeconds(10f);
		answerLabel.enabled = false;
	}
	
	public bool CheckAnswer()
	{
		if(guess.ToLower().CompareTo(answer.ToLower())==0)
		{
			answerLabel.enabled = true;
			return true;
		}
		else
		{
			
			answerLabel.enabled = false;
			return false;
		}
	}
}
