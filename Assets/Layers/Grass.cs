using UnityEngine;
using System.Collections.Generic;

public class Grass : Layer {
	public const int MAX_HEIGHT = 50;
	
	public const int TOO_WET = 70;
	
	protected static Grass singleton;
	public static Grass Singleton {
		get {
			if (singleton == null) {
				singleton = new Grass(){Name = "Grass", Color = Color.green};
			}
			return singleton;
		}
	}
	
	public override void PerFrame () {}
	
	public override byte Process(byte val, int x, int y) {
		if (Data.Singleton[x, y, LayerManager.GetLayer<Water>()] > val &&
			Data.Singleton[x, y, LayerManager.GetLayer<Water>()] < TOO_WET &&
			val < MAX_HEIGHT
		) {
			if (Data.Singleton[x, y, LayerManager.GetLayer<SunLight>()] == 1) {
				return (byte)(val + 5);
			}
			return (byte)(val + 2);
		}
		if (Data.Singleton[x, y, LayerManager.GetLayer<Water>()] > TOO_WET &&
			val > 0
		) {
			return (byte)(val - 1);
		}
		return val;
	}
	
	public override byte MaxValue () {
		return MAX_HEIGHT;
	}
}