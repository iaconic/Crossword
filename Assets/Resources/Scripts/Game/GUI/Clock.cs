using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour {

	public float currentTime = 0f;
	public bool go = true;
	public UILabel uiLabel;
	
	void Awake()
	{
		uiLabel = gameObject.GetComponent<UILabel>();
	}
	
	void GameOver ()
	{
		go = false;
	}

	void Update () {
		if(go)
		{
		   	currentTime += Time.deltaTime;
			int hours = (int) (currentTime / 3600);
		   	int mins = (int) (currentTime / 60) % 60;
		   	int secs = (int) (currentTime % 60);
			uiLabel.text = string.Format ("{0:00}:{1:00}:{2:00}", hours, mins, secs);
		}
	}
}
