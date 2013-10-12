using UnityEngine;
using System.Collections.Generic;

public class BoxManager : MonoBehaviour {
	protected static GameObject prefab = (GameObject)Resources.Load("Cube");
	
	protected int displayLayer;
	public static int DisplayLayer {
		get {
			return singleton.displayLayer;
		}
		set {
			singleton.displayLayer = value;
		}
	}
	
	protected static BoxManager singleton;
	
	public void Awake() {
		singleton = this;
	}
	
	public void Start() {
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
			}
		}
	}
}