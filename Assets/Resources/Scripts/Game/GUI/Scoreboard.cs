using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Scoreboard : MonoBehaviour {
	
	public GameObject background;
	public UILabel timeUILabel;
	public UILabel nameUILabel;
	public UILabel congratulationsUILabel;
	private int playTime = 3000;
	public string playerName = "IACONIC";
	private Game game;
	
	void Awake()
	{
		game = GameObject.FindWithTag("Game").GetComponent<Game>() as Game;
	}
	void View()
	{
		gameObject.SetActive(false);
	}
	void AppendName(string l)
	{
		if(playerName.Length < 8)
			playerName += l.ToUpper();
		nameUILabel.text = playerName;
	}
	void Backspace()
	{
		if(playerName.Length>0)
			playerName = playerName.Substring(0,playerName.Length-1);
		nameUILabel.text = playerName;
	}
	void SetTime(float time)
	{
		playTime = (int)time;
	}
	string TimeToString(int time)
	{
		int hours = (int) (time / 3600);
		int mins = (int) (time / 60) % 60;
		int secs = (int) (time % 60);
		return string.Format ("{0:00}:{1:00}:{2:00}", hours, mins, secs);
	}
	void GameOver() {
		timeUILabel.text = TimeToString(playTime);
		playerName = PlayerPrefs.GetString("name", "IACONIC");
		nameUILabel.text = playerName;
	}
	void NameEntered () {
		congratulationsUILabel.text = "";
		PlayerPrefs.SetString("name", playerName);
	
		StringReader reader = new StringReader(PlayerPrefs.GetString(Application.loadedLevel+"","600 IACONIC\n1200 IACONIC\n1800 IACONIC\n2400 IACONIC\n3000 IACONIC\n3600 IACONIC\n4200 IACONIC\n4800 IACONIC\n5400 IACONIC\n6000 IACONIC"));
		string output = "";
		bool counted = false;
		timeUILabel.text = "";
		nameUILabel.text = "";
		for(int i = 0; i < 10; i ++)
		{
			string input = reader.ReadLine();
			int timer = int.Parse(input.Split(' ')[0]);
			if(playTime<timer&&!counted)
			{
				timeUILabel.text += TimeToString(playTime) + "\n";
				nameUILabel.text += playerName + "\n";
				output += playTime + " " + playerName + "\n";
				i ++;
				counted = true;
			}
			timeUILabel.text += TimeToString(timer);
			nameUILabel.text += input.Split(' ')[1];
			if(i!=9)
			{
				timeUILabel.text +=  "\n";
				nameUILabel.text +=  "\n";
			}
			output += input + "\n";
		}
		reader.Close();
		PlayerPrefs.SetString(Application.loadedLevel+"",output);
	}
	void OnClick()
	{
		game.ButtonPress("Scoreboard");
	}
	void OnEnable()
	{
		background.SetActive(true);
		timeUILabel.gameObject.SetActive(true);
		nameUILabel.gameObject.SetActive(true);
		congratulationsUILabel.gameObject.SetActive(true);
	}
	void OnDisable()
	{
		background.SetActive(false);
		timeUILabel.gameObject.SetActive(false);
		nameUILabel.gameObject.SetActive(false);
		congratulationsUILabel.gameObject.SetActive(false);
	}
}
