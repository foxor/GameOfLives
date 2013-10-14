using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class PredatorHeirarchy : MonoBehaviour {
	
	private Rect windowRect;
	
	private int windowWidth = 200, windowHeight = 100;
	
	private List<Layer> sortedLayers;
	
	private Vector2 scrollViewVector;

	// Use this for initialization
	void Start () {
		windowRect = new Rect(Screen.width/2 - windowWidth/2, Screen.height - windowHeight, windowWidth, windowHeight);
		
		sortedLayers = new List<Layer>();
		
		scrollViewVector = new Vector2();
	}
	
	// Update is called once per frame
	void Update () {
		if (LayerManager.LayerDepth - 4 != sortedLayers.Count) {
			generateSortedLayers();
		}
	}
	
	void OnGUI() {
		windowRect = GUI.Window(1, windowRect, windowFunction, "ho");
	}
	
	void windowFunction(int id) {
		GUI.DragWindow();
		
		scrollViewVector = GUILayout.BeginScrollView(scrollViewVector, GUILayout.Width(windowRect.width - 10), GUILayout.Height(windowRect.height));
		
		foreach (Animal layer in sortedLayers) {
			GUILayout.TextField(layer.Name);
		}
		
		GUILayout.EndScrollView();
			
	}
	
	private void generateSortedLayers() {
		sortedLayers = LayerManager.Layers.ToList();
		Layer temp;
		bool inOrder = false;
		while (!inOrder) {
			inOrder = true;
			for (int i = 0; i < sortedLayers.Count - 1; i++) {
				if (!(sortedLayers[i].GetType() == (typeof(Animal)))) {
					inOrder = false;
					sortedLayers.RemoveAt(i);
					i--;
					break;
				}
				else if (((Animal)(sortedLayers[i])).Aggression < ((Animal)(sortedLayers[i+1])).Aggression) {
					inOrder = false;
					temp = sortedLayers[i];
					sortedLayers[i] = sortedLayers[i+1];
					sortedLayers[i+1] = temp;
				}
			}
		}
	}
}
