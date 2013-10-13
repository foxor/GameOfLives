using UnityEngine;
using System.Collections.Generic;

public abstract class Layer {
	public string Name {
		get; set;
	}
	public Color Color {
		get; set;
	}
	public int LAYER {
		get; set;
	}
	
	public virtual void OnStartup(int layer) {
		LAYER = layer;
	}
	public abstract void PerFrame();
	public abstract byte Process(byte val, int x, int y);
	
	public virtual byte MaxValue() {
		return byte.MaxValue;
	}
}