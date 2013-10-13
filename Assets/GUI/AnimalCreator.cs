using UnityEngine;
using System.Collections;

public class AnimalCreator : MonoBehaviour {
	
	public static int PADDING = 5;
	public Point windowSize, expanderSize;
	
	protected Rect windowRect, windowHoverRect;
	protected Rect expanderRect, expanderHoverRect;
	
	private int lastScreenWidth, lastScreenHeight;
	
	private bool expanded;
	
	private string nameString;
	
	private Color color;
	
	private Texture2D colorRect;
	
	// Use this for initialization
	void Start () {
		lastScreenWidth = Screen.width;
		lastScreenHeight = Screen.height;
		
		windowRect = new Rect();
		windowHoverRect = new Rect();
		
		expanderRect = new Rect();		
		expanderHoverRect = new Rect();
		
		windowSize = new Point(200, 600);
		expanderSize = new Point(100, 30);
		
		determineRectangles();
		
		expanded = false;
		
		nameString = "Name";
		
		color = new Color(255, 0, 0);
		
		colorRect = new Texture2D(1, 1);
		colorRect.SetPixel(0, 0, color);
		colorRect.Apply();
	}
	
	// Update is called once per frame
	void Update () {
		if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height) {
			lastScreenWidth = Screen.width;
			lastScreenHeight = Screen.height;
			resizeEvent();
		}
	
	}
	
	void OnGUI() {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.y = Screen.height - mousePosition.y;
		
		if (expanderHoverRect.Contains(mousePosition)) {
			expanded = true;
		}
		else if (expanded && !windowHoverRect.Contains(mousePosition)) {
			expanded = false;
		}
		
		if (expanded) {
			windowRect = GUI.Window(0, windowRect, windowFunction, "Window");			
		}
		else {
			GUI.Box(expanderRect, "Expander");
		}
		
	}
	
	void windowFunction(int windowID) {
		nameString = GUILayout.TextField(nameString);
		GUILayout.Label("Color");
		color.r = GUILayout.HorizontalSlider(color.r, 0, 255);
		color.g = GUILayout.HorizontalSlider(color.g, 0, 255);
		color.b = GUILayout.HorizontalSlider(color.b, 0, 255);
	}
	
	private void resizeEvent() {
		determineRectangles();
	}
	
	private void determineRectangles() {
		expanderRect.Set(Screen.width - expanderSize.x - PADDING, Screen.height - expanderSize.y - PADDING, expanderSize.x, expanderSize.y);
		expanderHoverRect.Set(Screen.width - expanderSize.x - 2*PADDING, Screen.height - expanderSize.y - 2*PADDING, expanderSize.x + PADDING, expanderSize.y + PADDING);
		
		windowRect.Set(Screen.width - windowSize.x - PADDING, Screen.height - windowSize.y - PADDING, windowSize.x, windowSize.y);
		windowHoverRect.Set(Screen.width - windowSize.x - 2*PADDING, Screen.height - windowSize.y - 2*PADDING, windowSize.x + PADDING, windowSize.y + PADDING);
		
	}
}
