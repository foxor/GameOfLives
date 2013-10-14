using UnityEngine;
using System.Collections;
using System.Linq;

public class LayerSelector : MonoBehaviour {
	
	public const int TOP = 0, RIGHT = 1, BOTTOM = 2, LEFT = 3;
	protected const int PADDING = 5;
	protected const int MENU_WIDTH = 200;
	protected const int MIN_LAYER_BUTTON_HEIGHT = 30;
	
	protected int layerSelectorSelection;
	protected int LayerSelectorSelection;
	
	protected Rect layerSelectorRect;
	protected Rect layerSelectorHoverRect;
	
	protected int lastScreenWidth, lastScreenHeight;
	
	protected Vector2 scrollViewVector;
	
	protected Texture2D colorTex;

	void Start () {
		determineLayerToolbarRect();
		scrollViewVector = new Vector2();
		colorTex = new Texture2D(30 - PADDING, 30 - PADDING);
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
		
		scrollViewVector = GUI.BeginScrollView(layerSelectorRect, scrollViewVector, new Rect(layerSelectorRect.x, layerSelectorRect.y, layerSelectorRect.width, layers.Length*(30 + PADDING) + 20));
		
		Rect position = new Rect();
		for (int i = 0; i < layers.Length; i++) {
			position.Set (PADDING, i*30 + i*PADDING + 20, layerSelectorRect.width - 30 - PADDING*5, 30);
			if (GUI.Button(position, LayerManager.Layers.ElementAt(i).Name)) {
				toggleLayer(i);
			}
			position.Set (position.x + position.width + PADDING, position.y, 30, 30);
			fillTexture(colorTex, new Color(layers[i].Color.r, layers[i].Color.g, layers[i].Color.b, 1f));
			GUI.Box(position, colorTex);
		}
		
		GUI.EndScrollView();
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
	
	private static void fillTexture(Texture2D tex, Color color) {
		for (int x = 0; x < tex.width; x++) {
			for (int y = 0; y < tex.height; y++) {
				tex.SetPixel(x, y, color);
			}
		}
		tex.Apply ();
	}
}