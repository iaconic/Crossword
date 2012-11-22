using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour {

	Vector3 targetPosition = new Vector3(0f,0f,-10f);
	Vector3 currentPosition = new Vector3(0f,0f,-10f);
	
	void Start()
	{
		Time.timeScale = 1;
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)&&Application.platform!=RuntimePlatform.IPhonePlayer)
		{
			RaycastHit[] hits;
			hits = Physics.RaycastAll (camera.ScreenPointToRay(Input.mousePosition), 100.0f);
			foreach(RaycastHit hit in hits)
			{
				if(hit.collider.tag=="Level")
				{
					Application.LoadLevel(hit.collider.name);
				}
				if(hit.collider.tag=="Chapter")
				{
					targetPosition = hit.collider.transform.parent.position;
					targetPosition.z = -10;
				}
			}
		}
		else if(Input.touchCount==1)
		{
			foreach(Touch touch in Input.touches)
			{
				if(touch.tapCount>0&&touch.phase==TouchPhase.Began)
				{
   					RaycastHit[] hits;
    				hits = Physics.RaycastAll (camera.ScreenPointToRay(touch.position), 100.0f);
					foreach(RaycastHit hit in hits)
					{
						if(hit.collider.tag=="Level")
						{
							Application.LoadLevel(hit.collider.name);
						}
						if(hit.collider.tag=="Chapter")
						{
							targetPosition = hit.collider.transform.parent.position;
							targetPosition.z = -10;
						}
					}
				}
			}
		}
		if(currentPosition != targetPosition)
		{
			currentPosition += (targetPosition - currentPosition) * Time.deltaTime * 4f;
		}
		transform.position = currentPosition;
	}
}
