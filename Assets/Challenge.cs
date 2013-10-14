using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Challenge : MonoBehaviour {
	protected const int CHALLENGE_TIMER = 10000;
	protected const int THRIVING_REQUIREMENT = 10;
	
	protected int DiversityThreshold = 2;
	protected int timer = CHALLENGE_TIMER;
	protected bool hasWon = false;
	protected Rect reportArea;
	protected int thriving;
	
	public void Start() {
		reportArea = new Rect(
			Screen.width * 7f / 8f,
			5f,
			Screen.width / 8f,
			130f
		);
	}
	
	public void Update() {
		thriving = 0;
		foreach (Layer l in LayerManager.Layers.Where(
			x => x.GetType() == typeof(Animal))
		) {
			if (((Animal)l).NumSurviving() >= THRIVING_REQUIREMENT) {
				thriving++;
			}
		}
		
		if (thriving >= DiversityThreshold) {
			if (--timer <= 0) {
				hasWon = true;
				Debug.Log("Amazing!");
			}
		}
		else {
			timer = CHALLENGE_TIMER;
		}
	}
	
	public void OnGUI() {
		GUILayout.BeginArea(reportArea);
		GUILayout.Label("Level " + (DiversityThreshold - 1) + ": ");
		if (thriving < DiversityThreshold) {
			GUILayout.Label(thriving + " species currently thriving, " + DiversityThreshold + " required");
			if (GUILayout.Button("Restart")) {
				Application.LoadLevel(Application.loadedLevel);
			}
		}
		else if (timer > 0) {
			GUILayout.Label("Countdown: " + timer);
		}
		else if (hasWon) {
			GUILayout.Label("You Win!  Yaaaay!");
			if (GUILayout.Button("Level Up")) {
				DiversityThreshold += 1;
				timer = CHALLENGE_TIMER;
			}
		}
		GUILayout.EndArea();
	}
}