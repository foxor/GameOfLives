using UnityEngine;
using System.Collections.Generic;

public class FlowField {
	protected const byte ELEVATION_CLAMP = 20;
	
	public static byte[] Generate(byte targetElevation) {
		HashSet<int> explored = new HashSet<int>();
		HashSet<int> border = new HashSet<int>();
		byte[] values = new byte[Data.Width * Data.Height];
		return values;
	}
}