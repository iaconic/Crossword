using UnityEngine;
using System.Collections;

public class PageButton : MonoBehaviour {
	
	public CameraRotation cameraRotation;
	public string buttonName;
	
	void OnClick()
	{
		cameraRotation.ButtonPress(buttonName);
	}
}
