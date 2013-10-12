using UnityEngine;
using System.Collections;

public class GUI_Initial : MonoBehaviour {
	
	protected string[] testToolbarNames;
	protected int testToolbarSelection;
	protected string textAreaString;
	
	protected Rect topToolbarRect,
	               topHoverRect;
	
	public int lastScreenWidth, lastScreenHeight;

	// Use this for initialization
	void Start () {
		testToolbarNames = new string[4];
		testToolbarNames[0] = "Layer 1";
		testToolbarNames[1] = "Layer 2";
		testToolbarNames[2] = "Layer 3";
		testToolbarNames[3] = "Layer 4";
		testToolbarSelection = 0;
		
		topToolbarRect = new Rect(5, 5, Screen.width - 10, 30);
		topHoverRect = new Rect(0, 0, Screen.width, 40);
		
		textAreaString = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (lastScreenWidth != Screen.width || lastScreenWidth != Screen.height) {
			lastScreenWidth = Screen.width;
			lastScreenHeight = Screen.height;
			resizeEvent();
		}
		
		print (textAreaString);
	}
	
	void OnGUI() {
		
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.y = Screen.height - mousePosition.y;
		
		GUI.Label(new Rect(Screen.width - 100, Screen.height - 30, 100, 20), "W: " + Screen.width + ", H: " + Screen.height);
		GUI.Label(new Rect(0, Screen.height - 30, 200, 20), "MX: " + mousePosition.x + ", MY: " + mousePosition.y);
		
		if (GUI.Button(new Rect(120, 120, 100, 100), "Hmmm")) {
			print("Ouch!");
		}
		
		textAreaString = GUI.TextArea(new Rect(200, 200, 100, 100), textAreaString);
		
		if (topHoverRect.Contains(mousePosition)) {
			testToolbarSelection = GUI.Toolbar(topToolbarRect, testToolbarSelection, testToolbarNames);
		}
	}
	
	private void resizeEvent() {
		topToolbarRect.Set(topToolbarRect.x, topToolbarRect.y, Screen.width - 10, topToolbarRect.height);
		topHoverRect.Set(0, 0, Screen.width, 40);
	}
}
