using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Processor : MonoBehaviour {
	
	protected delegate byte cellProcessor (byte val, int x, int y);
	
	protected Dictionary<int, cellProcessor> layerProcessors = new Dictionary<int, cellProcessor> {
		{0, new Conway(){Layer = 0}.Process}
	};
	
	protected bool isDead;
	protected IEnumerator LockManager() {
		while (!isDead) {
			Process();
			Data.Flip();
			yield return 0;
		}
	}
	
	protected void Process() {
		for (int x = 0; x < Data.Width; x++) {
			for (int y = 0; y < Data.Height; y++) {
				for (int z = 0; z < Data.Depth; z++) {
					Data.Singleton.setNext(x, y, z, 
						layerProcessors[z](Data.Singleton[x, y, z], x, y)
					);
				}
			}
		}
	}
	
	public void Start() {
		StartCoroutine(LockManager());
	}
}