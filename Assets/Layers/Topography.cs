using UnityEngine;
using System.Collections.Generic;

public class Topography : Layer {
	
	protected static Topography singleton;
	public static Topography Singleton {
		get {
			if (singleton == null) {
				singleton = new Topography(){Color = Color.black, Name = "Topography"};
			}
			return singleton;
		}
	}
	
	public override void OnStartup(int layer) {
		base.OnStartup(layer);
		byte[] heights = Noise2d.GenerateNoiseMap(Data.Width, Data.Height, 8);
		for (int x = 0; x < Data.Width; x++) {
			for (int y = 0; y < Data.Height; y++) {
				Data.Singleton[x, y, LAYER] = heights[x + y * Data.Width];
			}
		}
	}
	
	public override byte Process(byte val, int x, int y) {
		return val;	
	}
	
	public override void PerFrame() {
	}
}