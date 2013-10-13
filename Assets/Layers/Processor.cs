using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Processor : MonoBehaviour {
	
	protected Layer[] layers;
	
	protected void Process() {
		for (int x = 0; x < Data.Width; x++) {
			for (int y = 0; y < Data.Height; y++) {
				for (int z = 0; z < LayerManager.LayerDepth; z++) {
					Data.Singleton.setNext(x, y, z, 
						layers[z].Process(Data.Singleton[x, y, z], x, y)
					);
				}
			}
		}
	}
	
	public void Update() {
		layers = LayerManager.Layers.ToArray();
		foreach (Layer l in layers) {
			l.PerFrame();
		}
		Process();
		Data.Flip();
	}
}