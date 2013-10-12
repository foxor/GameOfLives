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
			return Color.Lerp(Color.green, Color.red, ((float)val) / ((float)byte.MaxValue));
		}
		return Color.black;
	}
}