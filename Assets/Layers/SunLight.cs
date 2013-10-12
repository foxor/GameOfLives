using UnityEngine;
using System.Collections.Generic;

public class SunLight {
	public const int SUN_LAYER = 0;
	
	protected const float LEFT = 0.25f;
	protected const float TOP = 0.50f;
	protected const float RIGHT = 0.75f;
	protected static int[,] GLIDER_POSITIONS = new int[,] {
		{0, 1},
		{1, 2},
		{2, 0},
		{2, 1},
		{2, 2}
	};
	protected const int FRAMES_BETWEEN_GLIDERS = 18;
	protected const float BEAM_SEPERATION = 0.05f;
	
	protected static int gliderTimer;
	
	protected static TimeMaster time = new TimeMaster();
	
	public static byte Process (byte val, int x, int y) {
		byte neighbors =  Data.Singleton.sumNeighbors(x, y, SUN_LAYER);
		int tooBig = ((neighbors & 4) >> 2) | ((neighbors & 8) >> 3);
		int isThree = ((neighbors & 1) & ((neighbors & 2) >> 1)) & ~tooBig;
		int tooSmall = (~tooBig & ~((neighbors & 2) >> 1)) & 1;
		return (byte)((val & ~(tooBig | tooSmall)) | isThree);
	}
	
	protected static void spawnSunlight(float ratio) {
		int startX, startY = 0;
		if (ratio < LEFT) {
			startX = 0;
			startY = Mathf.RoundToInt(Mathf.Lerp(0f, (float)(Data.Height - 1), ratio / LEFT));
		}
		else if (ratio < TOP) {
			startX = Mathf.RoundToInt(Mathf.Lerp(0f, (float)(Data.Width - 1), (ratio - LEFT) / (TOP - LEFT)));
			startY = Data.Height - 1;
		}
		else if (ratio < RIGHT) {
			startX = Data.Width - 1;
			startY = Mathf.RoundToInt(Mathf.Lerp((float)(Data.Height - 1), 0, (ratio - TOP) / (RIGHT - TOP)));
		}
		else {
			return;
		}
		
		bool flipX = startX > (Data.Width / 2);
		bool flipY = startY > (Data.Height / 2);
		
		for (int position = 0; position < GLIDER_POSITIONS.GetLength(0); position++) {
			int x = startX + (flipX ? -GLIDER_POSITIONS[position, 0] : GLIDER_POSITIONS[position, 0]);
			int y = startY + (flipY ? -GLIDER_POSITIONS[position, 1] : GLIDER_POSITIONS[position, 1]);
			Data.Singleton[x, y, SunLight.SUN_LAYER] = (byte)1;
		}
	}
	
	public static void PerFrame() {
		time.step();
		if ((--gliderTimer) <= 0) {
			spawnSunlight(time.getTimeRatio());
			spawnSunlight((time.getTimeRatio() + BEAM_SEPERATION) % 1f);
			spawnSunlight(((time.getTimeRatio() - BEAM_SEPERATION) + 1f) % 1f);
			gliderTimer = FRAMES_BETWEEN_GLIDERS;
		}
	}
}