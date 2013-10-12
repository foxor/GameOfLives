using UnityEngine;
using System.Collections.Generic;

public class Topography : MonoBehaviour {
	public const int LAYER = 1;
	
	public void Start() {
		byte[] heights = Noise2d.GenerateNoiseMap(Data.Width, Data.Height, 8);
		for (int x = 0; x < Data.Width; x++) {
			for (int y = 0; y < Data.Height; y++) {
				Data.Singleton[x, y, LAYER] = heights[x + y * Data.Width];
			}
		}
	}
	
	public static byte Process(byte val, int x, int y) {
		return val;	
	}
}