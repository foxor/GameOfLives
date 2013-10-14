using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class Data : MonoBehaviour {
	protected int width = 30;
	protected int height = 30;
	protected int depth = 31;
	
	protected bool parity;
	protected byte[] data1;
	protected byte[] data2;
	protected byte[] data {
		get {
			return parity ? data1 : data2;
		}
	}
	
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
	
	public static int MaxDepth {
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
		data1 = new byte[width * height * depth];
		data2 = new byte[width * height * depth];
		singleton = this;
	}
	
	public static void Flip() {
		singleton.parity ^= true;
	}
	
	public static bool boundsOk(int x, int y, int z) {
		if (x < 0 || x >= Width || y < 0 || y >= Height || z < 0 || z >= Height) {
			return false;
		}
		return true;
	}
	
	public byte this[int x, int y, int z] {
		get {
			return boundsOk(x, y, z) ? data[x + y * width + z * width * height] : (byte)0;
		}
		set {
			if (boundsOk(x, y, z)) {
				data[x + y * width + z * width * height] = value;
			}
		}
	}
	
	public byte getNext(int x, int y, int z) {
		if (boundsOk(x, y, z)) {
			return (parity ? data2 : data1)[x + y * width + z * width * height];
		}
		return (byte)0;
	}
	
	public void setNext(int x, int y, int z, byte val) {
		if (boundsOk(x, y, z)) {
			(parity ? data2 : data1)[x + y * width + z * width * height] = val;
		}
	}
	
	public byte sumNeighbors(int x, int y, int z) {
		int sum = 0;
		sum += this[x - 1, y - 1, z];
		sum += this[x, y - 1, z];
		sum += this[x + 1, y - 1, z];
		sum += this[x - 1, y, z];
		sum += this[x + 1, y, z];
		sum += this[x - 1, y + 1, z];
		sum += this[x, y + 1, z];
		sum += this[x + 1, y + 1, z];
		return (byte)Mathf.Clamp(255, 0, sum);
	}
}