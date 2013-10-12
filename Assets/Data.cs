using UnityEngine;
using System.Collections.Generic;

public class Data : MonoBehaviour {
	protected int width = 20;
	protected int height = 20;
	protected int depth = 10;
	
	protected byte[] data;
	
	public static int Width {
		get {
			return Singleton.width;
		}
	}
	
	public static int Height {
		get {
			return Singleton.height;
		}
	}
	
	public static int Depth {
		get {
			return Singleton.depth;
		}
	}
	
	protected static Data singleton;
	public static Data Singleton {
		get {
			return singleton;
		}
	}
	
	public void Awake() {
		data = new byte[width * height * depth];
		singleton = this;
	}
	
	public byte this[int x, int y, int z] {
		get {
			return data[x + y * width + z * width * height];
		}
		set {
			data[x + y * width + z * width * height] = value;
		}
	}
}