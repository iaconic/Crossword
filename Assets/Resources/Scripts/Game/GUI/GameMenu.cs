using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class GameMenu : MonoBehaviour {
	
	private Transform[] children;
	public UIButton solveButton;
	public UIButton keyboardButton;
	public UIButton menuButton;
	private bool solved;
	void Awake()
	{
		children = gameObject.GetComponentsInChildren<Transform>();
	}
	public void Solved()
	{
		solved = true;
		keyboardButton.isEnabled = false;
		solveButton.isEnabled = false;
		menuButton.isEnabled = false;
	}
	public void View()
	{
		menuButton.isEnabled = true;
	}
	void OnEnable()
	{
		keyboardButton.isEnabled = false;
		solveButton.isEnabled = false;
		foreach(Transform child in children)
		{
			child.gameObject.SetActive(true);
		}
	}
	void OnDisable()
	{
		if(!solved)
		{
			keyboardButton.isEnabled = true;
			solveButton.isEnabled = true;
		}
		foreach(Transform child in children)
		{
			child.gameObject.SetActive(false);
		}
	}
}
