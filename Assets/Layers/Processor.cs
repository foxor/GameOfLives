using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Processor : MonoBehaviour {
	
	protected const int PROCESS_PER_FRAME = 2;
	
	protected Layer[] layers;
	protected int frameCounter;
	
	protected void Process(int z) {
		for (int x = 0; x < Data.Width; x++) {
			for (int y = 0; y < Data.Height; y++) {
				Data.Singleton.setNext(x, y, z, 
					layers[z].Process(Data.Singleton[x, y, z], x, y)
				);
			}
		}
	}
	
	public void Start() {
		StartCoroutine(Frame());
	}
	
	public IEnumerator Frame() {
		while (LayerManager.LayerDepth == 0) {
			yield return 0;
		}
		while (true) {
			layers = LayerManager.Layers.ToArray();
			int z;
			for (z = 0; z < LayerManager.LayerDepth; z++) {
				layers[z].PerFrame();
			}
			for (z = 0; z < LayerManager.LayerDepth; z++) {
				Process(z);
				if (--frameCounter <= 0) {
					yield return 0;
					frameCounter = PROCESS_PER_FRAME;
				}
			}
			Data.Flip();
		}
	}
}