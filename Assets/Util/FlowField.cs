using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FlowField {
	protected const byte ELEVATION_CLAMP = 20;
	
	protected static IEnumerable<int> neighborPositions(int pos) {
		int x = pos % Data.Width;
		int y = pos / Data.Width;
		for (int dx = -1; dx <= 1; dx++) {
			for (int dy = -1; dy <= 1; dy++) {
				if (dx == dy && dx == 0) {
					continue;
				}
				if (!Data.boundsOk(x + dx, y + dy, Topography.Singleton.LAYER)) {
					continue;
				}
				yield return (x + dx) + (y + dy) * Data.Width;
			}
		}
	}
	
	protected static byte getDirection(int fromPos, int toPos) {
		byte b = (byte)(
			((fromPos % Data.Width) - (toPos % Data.Width) + 1) +
			((fromPos / Data.Width) - (toPos / Data.Width) + 1) * 3
		);
		return b;
	}
	
	public static byte[] Generate(byte targetElevation) {
		HashSet<int> explored = new HashSet<int>();
		Queue<int> border = new Queue<int>();
		Queue<int> nextBorder = new Queue<int>();
		byte[] values = new byte[Data.Width * Data.Height];
		for (int x = 0; x < Data.Width; x++) {
			for (int y = 0; y < Data.Height; y++) {
				if (Mathf.Abs(
					Data.Singleton[x, y, Topography.Singleton.LAYER] - targetElevation)
					< ELEVATION_CLAMP
				) {
					int pos = x + y * Data.Width;
					border.Enqueue(pos);
					explored.Add(pos);
				}
			}
		}
		while (border.Any()) {
			foreach (int pos in border) {
				foreach (int neighbor in neighborPositions(pos)) {
					if (explored.Contains(neighbor)) {
						continue;
					}
					nextBorder.Enqueue(neighbor);
					explored.Add(neighbor);
					values[neighbor] = getDirection(pos, neighbor);
				}
			}
			
			Queue<int> tmp = border;
			border = nextBorder;
			nextBorder = tmp;
			nextBorder.Clear();
		}
		return values;
	}
}