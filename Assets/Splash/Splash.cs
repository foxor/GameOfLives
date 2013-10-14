using UnityEngine;
using System.Collections.Generic;

public class Splash : MonoBehaviour {
	
	protected float timer = 10;
	
	public void Awake() {
		transform.localScale = new Vector3(
			Camera.main.orthographicSize * 2f * Camera.main.aspect,
			Camera.main.orthographicSize * 2f,
			1f
		);
	}
	
	public void Update() {
		if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
			Destroy(gameObject);
		}
		if ((timer -= Time.deltaTime) < 0f) {
			Destroy(gameObject);
		}
	}
}