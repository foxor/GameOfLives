using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

public class BoxManager : MonoBehaviour {
	protected static GameObject prefab;
	
	protected int displayLayer;
	public static int DisplayLayer {
		get {
			return singleton.displayLayer;
		}
		set {
			singleton.displayLayer = value;
		}
	}
	
	protected GameObject[] cubes;
	protected GameObject this[int x, int y] {
		get {
			return cubes[x + y * Data.Width];
		}
		set {
			cubes[x + y * Data.Width] = value;
		}
	}
	
	protected static BoxManager singleton;
	
	public void Awake() {
		singleton = this;
		displayLayer = -1;
	}
	
	public void Start() {
		prefab = (GameObject)Resources.Load("Cube");
		cubes = new GameObject[Data.Width * Data.Height];
		float cameraHeight = Camera.main.orthographicSize * 2f;
		float cameraWidth = cameraHeight * Camera.main.aspect;
		float widthSpan =  cameraWidth / ((float)Data.Width);
		float heightSpan = cameraHeight  / ((float)Data.Height);
		// make them a little too big to get the benifit of the doubt on LSB calculations
		Vector3 localScale = new Vector3(widthSpan * 1.00000001f, heightSpan * 1.00000001f, 1f);
		float startX = cameraWidth / -2f + widthSpan / 2f;
		float endX = cameraWidth / 2f + widthSpan / 2f;
		float startY = cameraHeight / -2f + heightSpan / 2f;
		float endY = cameraHeight / 2f + heightSpan / 2f;
		for (int x = 0; x < Data.Width; x++) {
			for (int y = 0; y < Data.Height; y++) {
				GameObject cube = (GameObject)Instantiate(prefab);
				cube.transform.position = new Vector3(
					Mathf.Lerp(startX, endX, ((float)x) / ((float)Data.Width)),
					Mathf.Lerp(startY, endY, ((float)y) / ((float)Data.Height)),
					transform.position.z
				);
				cube.transform.localScale = localScale;
				cube.transform.parent = transform;
				cube.renderer.material.color = Color.blue;
				this[x, y] = cube;
			}
		}
	}
	
	public void Update() {
		for (int x = 0; x < Data.Width; x++) {
			for (int y = 0; y < Data.Height; y++) {
				int bestLayer = 0;
				float bestVal = 0;
				byte bestValOrig = 0;
				for (int z = 0; z < LayerManager.LayerDepth; z++) {
					if (((1 << z) & displayLayer) == 0) {
						continue;
					}
					byte orig = Data.Singleton[x, y, z];
					float val = ((float)orig) / ((float)(LayerManager.GetLayer(z).MaxValue()))  * (z + 1f);
					if (val > bestVal) {
						bestLayer = z;
						bestVal = val;
						bestValOrig = orig;
					}
				}
				this[x, y].renderer.material.color = 
					ColorManager.Convert(bestValOrig, bestLayer);
			}
		}
	}
}