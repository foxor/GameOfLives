using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

public class Processor : MonoBehaviour {
	
	protected delegate byte cellProcessor (byte val, int x, int y);
	
	protected Dictionary<int, cellProcessor> layerProcessors = new Dictionary<int, cellProcessor> {
		{0, Conway}
	};
	
	protected bool isDead;
	protected Thread lockManager;
	protected void LockManager() {
		while (!isDead) {
			Process();
			Data.Flip();
			Thread.Sleep(20);
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
		lockManager = new Thread(new ThreadStart(LockManager));
		lockManager.Start(this);
	}
	
	protected static byte Conway(byte val, int x, int y) {
		byte neighbors =  Data.Singleton.sumNeighbors(x, y, 0);
		int tooBig = ((neighbors & 4) >> 2) | ((neighbors & 8) >> 3);
		int isThree = ((neighbors & 1) & ((neighbors & 2) >> 1)) & ~tooBig;
		int tooSmall = ~tooBig & ((neighbors & 2) >> 1);
		return (byte)((val ^ (tooBig | tooSmall)) | isThree);
	}
	
	public void OnApplicationExit() {
		isDead = true;
		Thread.Sleep(100);
		try {
			if (lockManager.IsAlive) {
				lockManager.Abort();
			}
		}
		catch (Exception e) {}
	}
}