using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class LayerManager : MonoBehaviour {
	protected static LayerManager singleton;
	
	protected List<Layer> layers;
	protected Dictionary<Type, int> layerIndex;
	
	public static void AddLayer(Layer l) {
		l.OnStartup(
			singleton.layerIndex[l.GetType()] = singleton.layers.Count
		);
		singleton.layers.Add(l);
	}
	
	public static IEnumerable<Layer> Layers {
		get {
			return singleton.layers;
		}
	}
	
	public static int GetLayer<T>() {
		return singleton.layerIndex[typeof(T)];
	}
	
	public static Layer GetLayer(int depth) {
		return singleton.layers[depth];
	}
	
	public static int LayerDepth {
		get {
			return singleton.layers.Count;
		}
	}
	
	public void Awake() {
		layers = new List<Layer>();
		layerIndex = new Dictionary<Type, int>();
		singleton = this;
	}
	
	public void Start() {
		AddLayer(Topography.Singleton);
		AddLayer(Water.Singleton);
		AddLayer(SunLight.Singleton);
		AddLayer(Grass.Singleton);
		AddLayer(Animal.Bunny);
		AddLayer(Animal.Wolf);
	}
}