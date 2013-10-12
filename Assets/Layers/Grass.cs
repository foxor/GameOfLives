using UnityEngine;
using System.Collections.Generic;

public class Grass {
	public const int LAYER = 3;
	
	public const int MAX_HEIGHT = 16;
	
	protected const int TOO_WET = 25;
	
	public static byte Process(byte val, int x, int y) {
		if (Data.Singleton[x, y, Water.LAYER] > val &&
			Data.Singleton[x, y, Water.LAYER] < TOO_WET &&
			Data.Singleton[x, y, SunLight.SUN_LAYER] == 1 &&
			val < MAX_HEIGHT
		) {
			return (byte)(val + 1);
		}
		return val;
	}
}