using UnityEngine;
using System.Collections.Generic;

public class Conway {
	public const int SUN_LAYER = 0;
	
	public int Layer {
		get; set;
	}
	
	public byte Process (byte val, int x, int y) {
		byte neighbors =  Data.Singleton.sumNeighbors(x, y, SUN_LAYER);
		int tooBig = ((neighbors & 4) >> 2) | ((neighbors & 8) >> 3);
		int isThree = ((neighbors & 1) & ((neighbors & 2) >> 1)) & ~tooBig;
		int tooSmall = (~tooBig & ~((neighbors & 2) >> 1)) & 1;
		return (byte)((val & ~(tooBig | tooSmall)) | isThree);
	}
}