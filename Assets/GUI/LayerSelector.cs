using UnityEngine;
using System.Collections;
using System.Linq;

public class LayerSelector : MonoBehaviour {
	
	public const int TOP = 0, RIGHT = 1, BOTTOM = 2, LEFT = 3;
	protected const int PADDING = 5;
	protected const int MENU_WIDTH = 100;
	
	protected int layerSelectorSelection;
	
	protected Rect layerSelectorRect;
	protected Rect layerSelectorHoverRect;
	
	protected int lastScreenWidth, lastScreenHeight;

	void Start () {
		determineLayerToolbarRect();
	}
	
	void Update () {
		if (lastScreenWidth != Screen.width || lastScreenWidth != Screen.height) {
			lastScreenWidth = Screen.width;
			lastScreenHeight = Screen.height;
			resizeEvent();
		}
		
		BoxManager.DisplayLayer = layerSelectorSelection;
	}
	
	void OnGUI() {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.y = Screen.height - mousePosition.y;
		
		if (layerSelectorHoverRect.Contains(mousePosition)) {
			layerSelectorSelection = GUI.SelectionGrid(
				layerSelectorRect, 
				layerSelectorSelection, 
				LayerManager.Layers.Select(x => x.Name).ToArray(), 
				1
			);
		}
	}
	
	private void resizeEvent() {
		determineLayerToolbarRect();
	}
		
	private void determineLayerToolbarRect() {
		layerSelectorRect.Set(PADDING, PADDING, MENU_WIDTH, Screen.height - 2*PADDING);
		layerSelectorHoverRect.Set(0, 0, MENU_WIDTH + PADDING*2, Screen.height);
	}
}