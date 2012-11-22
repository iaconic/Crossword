using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

	public UIAnchor uiAnchor;
	private Vector2 leftPosition = new Vector2(.38888888f,0f);
	private Vector2 rightPosition = new Vector2(-.38888888f,0f);
	private Vector3 landscapeScale = Vector3.one;
	private bool portraitLeft = false;
	private Vector2 targetPosition = Vector3.zero;
	private Vector3 targetScale = Vector3.one;
	private Vector2 currentPosition = Vector3.zero;
	private Vector3 currentScale = Vector3.one;
	public GameObject[] leftButton;
	public GameObject[] rightButton;
	public UILabel resolutionLabel;
	
	const int _Landscape = 0;
	const int _Portrait = 1;
	const int _Flat = 2;
	const int _Unknown = 3;
	private int orientation = _Flat;
	private float resolution;
	
	void Awake()
	{
		if(Screen.height > Screen.width)
			resolution = (float)Screen.height / (float)Screen.width;
		else
			resolution = (float)Screen.width / (float)Screen.height;
		resolutionLabel.text = "w:" + Screen.width + " h:" + Screen.height + " res:" + resolution;
		if(resolution == 16f/9f)
		{
			resolution = 1;
			for(int i = 0; i < 2; i ++)
			{
				leftButton[i].transform.Translate(Vector3.right*48);
				rightButton[i].transform.Translate(Vector3.left*48);
			}
			leftPosition = new Vector2(.5f,0f);
			rightPosition = new Vector2(-.5f,0f);
		}
		else if(resolution == 3f/2f)
		{
			resolution = 1;
			for(int i = 0; i < 2; i ++)
			{
				leftButton[i].transform.Translate(Vector3.right*48);
				rightButton[i].transform.Translate(Vector3.left*48);
			}
			leftPosition = new Vector2(.5f,0f);
			rightPosition = new Vector2(-.5f,0f);
		}
		else if(resolution == 4f/3f)
		{
			resolution = 1;
		}
		targetScale = Vector3.one;
		for(int i = 0; i < 2; i ++)
		{
			leftButton[i].SetActive(false);
			rightButton[i].SetActive(false);
		}
	}
	public void ButtonPress(string direction)
	{
		if(direction.CompareTo("Right")==0)
			portraitLeft = false;
		if(direction.CompareTo("Left")==0)
			portraitLeft = true;
	}
	void Update () {
		if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
		{
			orientation = _Landscape;
		}
		else if(Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
		{
			orientation = _Portrait;
		}
		if(orientation == _Portrait)
		{
			targetScale = new Vector3(resolution,resolution,1f);
			if(portraitLeft)
			{
				targetPosition = leftPosition;
				for(int i = 0; i < 2; i ++)
				{
					leftButton[i].SetActive(false);
					rightButton[i].SetActive(true);
				}
			}
			else
			{
				targetPosition = rightPosition;
				for(int i = 0; i < 2; i ++)
				{
					leftButton[i].SetActive(true);
					rightButton[i].SetActive(false);
				}
			}
		}
		else if(orientation == _Landscape)
		{
			targetPosition = Vector2.zero;
			targetScale = landscapeScale;
			for(int i = 0; i < 2; i ++)
			{
				leftButton[i].SetActive(false);
				rightButton[i].SetActive(false);
			}
		}
		currentPosition += (targetPosition - currentPosition) * Time.deltaTime * 4f;
		uiAnchor.relativeOffset = currentPosition;
		currentScale += (targetScale - currentScale) * Time.deltaTime * 2f;
		currentScale.z = 1f;
		uiAnchor.transform.localScale = currentScale;
	}
}
