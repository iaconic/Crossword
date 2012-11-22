using UnityEngine;
using System.Collections;

public class Word : MonoBehaviour {
	
	public int index;
	public Vector2 position;
	public string direction;
	public string answer;
	public string clue;
	public Letter firstLetter;
	public Clue clueScript;
	public int length;
	
	public bool CheckAnswer()
	{
		bool solved = true;
		foreach(Letter letter in GetComponentsInChildren<Letter>())
		{
			if(!letter.CheckAnswer())
				solved = false;
			StartCoroutine(letter.ClearCheckAnswer());
		}
		return solved;
	}
	public void SetFocus(bool f)
	{
		firstLetter.SetFocus(f);
	}
	public void EnableButtons(bool enabled)
	{
		firstLetter.EnableButtons(enabled);
		clueScript.EnableButtons(enabled);
	}
	public void Init(int index, Vector2 position, string direction, string answer, string clue)
	{
		this.index = index;
		this.position = position;
		this.direction = direction;
		this.answer = answer;
		this.clue = clue;
		
		//Instantiate letters
		Object letterResource = Resources.Load("Objects/Letter");
		Letter lastLetter = null;
		for(int i = 0; i < answer.Length; i ++)
		{
			if(answer.Substring(i,1).CompareTo(" ")!=0)
			{
				GameObject letterPrefab;
				
				if(direction.CompareTo("across")==0)
					letterPrefab = Instantiate(letterResource,transform.position+Vector3.right*i*24f,Quaternion.identity) as GameObject;
				else
					letterPrefab = Instantiate(letterResource,transform.position+Vector3.down*i*24f,Quaternion.identity) as GameObject;
				
				letterPrefab.transform.parent = transform;
				
				Letter letter = letterPrefab.GetComponent<Letter>();
				letter.Init(answer.Substring(i,1));
				if(lastLetter!=null)
					letter.SendMessage("SetPrevious", lastLetter);
				lastLetter = letter;
				
				if(i == 0)
					firstLetter = letter;
				
			}
		}
		lastLetter = null;
		Destroy(lastLetter);
	}
	
}
