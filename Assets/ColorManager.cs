using UnityEngine;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour {
	
	protected static ColorManager singleton;
	
	public void Awake() {
		singleton = this;
	}
	
	public static Color Convert(byte val, int layer) {
		switch (layer) {
		case 0:
			if (val == 1) {
				return Color.yellow;
			}
			return Color.grey;
		case 1:
			return Color.Lerp(Color.black, Color.white, ((float)val) / ((float)byte.MaxValue));
		case 2:
			return Color.Lerp(Color.grey, Color.blue, ((float)val) / ((float)byte.MaxValue));
		case 3:
			return Color.Lerp(Color.grey, Color.green, ((float)val) / ((float)Grass.MAX_HEIGHT));
		}
		return Color.black;
	}
}