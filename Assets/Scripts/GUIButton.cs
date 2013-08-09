using UnityEngine;
using System.Collections;

public class GUIButton : MonoBehaviour
{
	public GameObject receiver;
	public string buttonText;
	
	private Rect buttonRect;
	GUIStyle buttonStyle;
	
	void Start()
	{
		Bounds bounds = renderer.bounds;
		
		Vector3 buttonPos = Camera.main.WorldToScreenPoint( new Vector3(bounds.min.x, bounds.max.y, bounds.min.z) );
		buttonPos.y = Screen.height - buttonPos.y;

		Vector3 buttonMax = Camera.main.WorldToScreenPoint( bounds.max );
		Vector3 buttonMin = Camera.main.WorldToScreenPoint( bounds.min );
		Vector3 buttonSize = buttonMax-buttonMin;
		
		
		buttonRect = new Rect(buttonPos.x, buttonPos.y, buttonSize.x, buttonSize.y);
		renderer.enabled = false;
	}
	
	private void SendMessage(object parameter)
	{
		if(receiver) receiver.SendMessage("OnButtonClicked", parameter, SendMessageOptions.DontRequireReceiver);
	}
	
	void OnGUI()
	{
		GUIStyle buttonStyle = new GUIStyle( GUI.skin.button );
		buttonStyle.wordWrap = true;
		buttonStyle.alignment = TextAnchor.UpperCenter;

		if( GUI.Button(buttonRect, "\n"+buttonText, buttonStyle) )
		{
			SendMessage(buttonText);
		}
	}
}
