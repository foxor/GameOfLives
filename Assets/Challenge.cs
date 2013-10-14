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
			80f
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
		
		if (thriving >= THRIVING_REQUIREMENT) {
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
		if (thriving < THRIVING_REQUIREMENT) {
			GUILayout.Label(thriving + " species currently thriving, " + THRIVING_REQUIREMENT + " required");
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