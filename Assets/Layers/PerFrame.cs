using UnityEngine;
using System.Collections.Generic;

public class PerFrame {
	public static void Tick() {
		SunLight.PerFrame();
		Water.PerFrame();
		Animal.Bunny.PerFrame();
	}
}