using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Water {
	protected const int SIGN_SHIFT = 31;
	protected const int SEA_LEVEL = 100;
	
	public const int LAYER = 2;
	
	protected static int[][] watchedCoords;
	protected static int[][] WatchedCoords {
		get {
			if (watchedCoords == null) {
				watchedCoords = GetWatchedCoords().ToArray();
			}
			return watchedCoords;
		}
	}
	
	protected static IEnumerable<int[]> GetWatchedCoords() {
		for (int x = 0; x < Data.Width; x++) {
			for (int y = 0; y < Data.Height; y++) {
				if (Data.Singleton[x, y, Topography.LAYER] < SEA_LEVEL) {
					yield return new int[] {x, y};
				}
			}
		}
	}
	
	protected static int GetAdjustedNeighborValue(int x, int y) {
		if (Data.boundsOk(x, y, LAYER)) {
			return Data.Singleton[x, y, Topography.LAYER] + Data.Singleton[x, y, LAYER];
		}
		return SEA_LEVEL;
	}
	
	public static byte Process(byte val, int x, int y) {
		if (val == 0) {
			return 0;
		}
		int cVal = Data.Singleton[x, y, Topography.LAYER] + val;
		
		for (int dx = -1; dx <= 1; dx++) {
			for (int dy = -1; dy <= 1; dy++) {
				int n = GetAdjustedNeighborValue(x + dx, y + dy);
				if (cVal > n && val > 0) {
					val -= 1;
					cVal -= 1;
					Data.Singleton[x + dx, y + dy, LAYER] += 1;
					Data.Singleton.setNext(x + dx, y + dy, LAYER,
						(byte)(Data.Singleton.getNext(x + dx, y + dy, LAYER) + 1)
					);
				}
			}
		}
		
		return val;
	}
	
	public static void PerFrame() {
		int x = Random.Range(0, Data.Width);
		int y = Random.Range(0, Data.Height);
		for (int i = 0; i < 100; i++) {
			Data.Singleton[x, y, LAYER] = (byte)(Data.Singleton[x, y, LAYER] + 1);
		}
	}
}