using UnityEngine;
using System.Collections;

public class GUI_Initial : MonoBehaviour {
	
	public const int TOP = 0, RIGHT = 1, BOTTOM = 2, LEFT = 3;
	
	protected string[] layerToolbarNames = {"Sun", "Topo", "Water"};
	protected int layerSelectorSelection, layerSelectorLocation, layerSelectorMinWidth, layerSelectorMinHeight, layerSelectorNumColumns;
	protected int hoverRectPadding, edgePadding;
	protected string textAreaString;
	
	protected Rect layerSelectorRect;
	protected Rect layerSelectorHoverRect;
	
	public int lastScreenWidth, lastScreenHeight;

	// Use this for initialization
	void Start () {
			
		hoverRectPadding = 5;
		edgePadding = 5;
			
		layerSelectorSelection = 0;
		layerSelectorLocation = LEFT;
		layerSelectorMinWidth = 100;
		layerSelectorMinHeight = 30;
		layerSelectorNumColumns = (layerSelectorLocation == TOP || layerSelectorLocation == BOTTOM) ? layerToolbarNames.Length : 1;
			
		determineLayerToolbarRect();
		
		textAreaString = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (lastScreenWidth != Screen.width || lastScreenWidth != Screen.height) {
			lastScreenWidth = Screen.width;
			lastScreenHeight = Screen.height;
			resizeEvent();
		}
		
		BoxManager.DisplayLayer = layerSelectorSelection;
		
		print (textAreaString);
	}
	
	void OnGUI() {
		
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.y = Screen.height - mousePosition.y;
		
		GUI.Label(new Rect(Screen.width - 100, Screen.height - 30, 100, 20), "W: " + Screen.width + ", H: " + Screen.height);
		GUI.Label(new Rect(0, Screen.height - 30, 200, 20), "MX: " + mousePosition.x + ", MY: " + mousePosition.y);
		
		if (layerSelectorHoverRect.Contains(mousePosition)) {
			layerSelectorSelection = GUI.SelectionGrid(layerSelectorRect, layerSelectorSelection, layerToolbarNames, layerSelectorNumColumns);
		}
	}
	
	private void resizeEvent() {
		determineLayerToolbarRect();
	}
		
	private void determineLayerToolbarRect() {	
		int x, y, w, h;
		switch (layerSelectorLocation) {
			case (TOP) : {
				x = edgePadding;
			 	y = edgePadding;
				w = Screen.width - edgePadding*2;
				h = layerSelectorMinHeight;
				break;
			}
			case (RIGHT) : {
				x = Screen.width - layerSelectorMinWidth - edgePadding;
			 	y = edgePadding;
				w = layerSelectorMinWidth;
				h =	Screen.height - 2*edgePadding;
				break;
			}
			case (BOTTOM) : {
				x = edgePadding;
			 	y = Screen.height - layerSelectorMinHeight - edgePadding;
				w = Screen.width - edgePadding*2;
				h =	layerSelectorMinHeight;
				break;
			}
			case (LEFT) : {
				x = edgePadding;
			 	y = edgePadding;
				w = layerSelectorMinWidth;
				h =	Screen.height - 2*edgePadding;
				break;
			}
			default : {
				x = edgePadding;
			 	y = edgePadding;
				w = Screen.width - edgePadding*2;
				h = layerSelectorMinHeight;
				break;
			}
		}
		layerSelectorRect.Set(x, y, w, h);
		layerSelectorHoverRect.Set(x - hoverRectPadding, y - hoverRectPadding, w + hoverRectPadding*2, h + hoverRectPadding*2);
	}
}
