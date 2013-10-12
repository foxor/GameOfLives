using UnityEngine;
using System.Collections;

public class TimeMaster {
	
	public const uint FRAMES_PER_DAY = 1000;
	
	private uint frameCount = 0;
	
	public void step() {
		frameCount++;
	}
	
	public float getTimeRatio() {
		return (float)(frameCount % FRAMES_PER_DAY)/FRAMES_PER_DAY;
	}
	
	public int getTimeHours() {
		return (int)(getTimeRatio()*24);
	}
	
	public float getTimeDegrees() {
		return getTimeRatio()*360;
	}
	
	public float getTimeRadians() {
		return getTimeRatio()*Mathf.PI/2;
	}
}
