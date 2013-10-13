using UnityEngine;
using System.Collections.Generic;

public class ColorManager : MonoBehaviour {
	
	protected static ColorManager singleton;
	
	public void Awake() {
		singleton = this;
	}
	
	public static Color Convert(byte val, int layer) {
		Layer l = LayerManager.GetLayer(layer);
		return Color.Lerp(Color.grey, l.Color, ((float)val) / ((float)l.MaxValue()));
	}
}