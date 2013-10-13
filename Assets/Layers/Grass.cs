using UnityEngine;
using System.Collections.Generic;

public class Grass {
	public const int LAYER = 3;
	
	public const int MAX_HEIGHT = 50;
	
	public const int TOO_WET = 70;
	
	public static byte Process(byte val, int x, int y) {
		if (Data.Singleton[x, y, Water.LAYER] > val &&
			Data.Singleton[x, y, Water.LAYER] < TOO_WET &&
			val < MAX_HEIGHT
		) {
			if (Data.Singleton[x, y, SunLight.SUN_LAYER] == 1) {
				return (byte)(val + 20);
			}
			return (byte)(val + 5);
		}
		if (Data.Singleton[x, y, Water.LAYER] > TOO_WET &&
			val > 0
		) {
			return (byte)(val - 1);
		}
		return val;
	}
}