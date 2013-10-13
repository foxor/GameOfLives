using UnityEngine;
using System.Collections;
using System.Linq;

public class LayerSelector : MonoBehaviour {
	
	public const int TOP = 0, RIGHT = 1, BOTTOM = 2, LEFT = 3;
	protected const int PADDING = 5;
	protected const int MENU_WIDTH = 100;
	
	protected int layerSelectorSelection;
	protected int LayerSelectorSelection;
	
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
	}
	
	void OnGUI() {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.y = Screen.height - mousePosition.y;
		
		if (layerSelectorHoverRect.Contains(mousePosition)) {	
			GUI.Window(0, layerSelectorRect, windowFunction, "Layers");
		}
	}
	
	void windowFunction(int windowID) {
		Layer[] layers = LayerManager.Layers.ToArray();
		for (int i = 0; i < layers.Length; i++) {
			if (GUILayout.Button(LayerManager.Layers.ElementAt(i).Name)) {
				toggleLayer(i);
			}
		}
	}
	
	private void resizeEvent() {
		determineLayerToolbarRect();
	}
		
	private void determineLayerToolbarRect() {
		layerSelectorRect.Set(PADDING, PADDING, MENU_WIDTH, Screen.height - 2*PADDING);
		layerSelectorHoverRect.Set(0, 0, MENU_WIDTH + PADDING*2, Screen.height);
	}
	
	public void toggleLayer(int layerIndex) {
		BoxManager.DisplayLayer ^= 1 << layerIndex;
	}
}