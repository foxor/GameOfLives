using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Animal {
	
	protected static Animal bunny;
	
	public static Animal Bunny {
		get {
			if (bunny == null) {
				bunny = new Animal(4){
					Activity = 0.4f,
					BreedingThreshold = 30,
					CombatAbility = 0.1f,
					Diet = new List<int>(){Grass.LAYER},
					DisplayColor = Color.white,
					Habitat = TERRESTRIAL_FLAG,
					Name = "Bunny",
					TargetElevation = 45
				};
				bunny.AddAnimal(
					Random.Range(0, Data.Width),
					Random.Range(0, Data.Height)
				);
			}
			return bunny;
		}
	}	
	
	public static Dictionary<int, Animal> LayerMapping;
		
	
	protected const int TERRESTRIAL_FLAG = 1;
	protected const int AQUATIC_FLAG = 2;
	
	protected const int HERBIVOR_FLAG = 1;
	protected const int CARNIVOR_FLAG = 2;
	
	protected const int SWIM_DEPTH = 3;
	protected const int TERRITORY_DEAD_ZONE = 50;
	
	protected static byte[] flowField;
	
	public string Name;
	public Color DisplayColor;
	public int TargetElevation;
	public int Habitat;
	public List<int> Diet;
	public float Activity;
	public int BreedingThreshold;
	public float CombatAbility;
	
	protected int layer;
	
	protected Dictionary<int, int> nextAnimalPositions;
	
	public Animal(int layer) {
		nextAnimalPositions = new Dictionary<int, int>();
		
		if (LayerMapping == null) {
			LayerMapping = new Dictionary<int, Animal>();
		}
		LayerMapping[layer] = this;
		
		flowField = new byte[Data.Width * Data.Height];
	}
	
	public bool canSwim() {
		return (Habitat | AQUATIC_FLAG) > 0;
	}
	
	public bool canWalk() {
		return (Habitat | TERRESTRIAL_FLAG) > 0;
	}
	
	protected static int posClamp(int x, int y) {
		return Mathf.Clamp(x, 0, Data.Width - 1) + Data.Width * Mathf.Clamp(y, 0, Data.Height - 1);
	}
	
	protected List<int> shuffleSpace = new List<int>();
	protected bool movement(int x, int y, ref int[] delta) {
		bool foundPrey = false;
		shuffleSpace.Clear();
		int xMin = x - 1;
		int xMax = x + 1;
		int yMin = y - 1;
		int yMax = y + 1;
		
		float roll = Random.Range(0f, 1f);
		if (roll > Activity) {
			return false;
		}
		
		for (int xPos = xMin; xPos <= xMax; xPos++) {
			for (int yPos = yMin; yPos <= yMax; yPos++) {
				int pos = posClamp(xPos, yPos);
				if (nextAnimalPositions.ContainsKey(pos)) {
					continue;
				}
				if (!canSwim() && Data.Singleton[xPos, yPos, Water.LAYER] >= SWIM_DEPTH) {
					continue;
				}
				else if (!canWalk() && Data.Singleton[xPos, yPos, Water.LAYER] < SWIM_DEPTH) {
					continue;
				}
				if (!Data.boundsOk(xPos, yPos, layer)) {
					continue;
				}
				foreach (int prey in Diet) {
					if (Data.Singleton[xPos, yPos, prey] > 0) {
						if (!foundPrey) {
							foundPrey = true;
							shuffleSpace.Clear();
						}
						shuffleSpace.Add((xPos + 1 - x) + (yPos + 1 - y) * 3);
					}
				}
				if (!foundPrey) {
					shuffleSpace.Add((xPos + 1 - x) + (yPos + 1 - y) * 3);
				}
			}
		}
		if (shuffleSpace.Count == 0) {
			return false;
		}
		if (flowField[posClamp(x, y)] > 0) {
			//TODO
		}
		int direction = shuffleSpace.OrderBy(key => Random.Range(0f, 1f)).First();
		delta[0] = (direction % 3) - 1;
		delta[1] = (direction / 3) - 1;
		return true;
	}
	
	public byte Process(byte val, int x, int y) {
		int pos = posClamp(x, y);
		if (nextAnimalPositions.ContainsKey(pos)) {
			return (byte)nextAnimalPositions[pos];
		}
		return 0;
	}
	
	public void AddAnimal(int x, int y) {
		nextAnimalPositions[posClamp(x, y)] = 255;
	}
	
	public void PerFrame() {
		int[] delta = new int[2];
		foreach (int key in nextAnimalPositions.Keys.ToArray()) {
			int x = key % Data.Width;
			int y = key / Data.Width;
			if (movement(x, y, ref delta)) {
				x += delta[0];
				y += delta[1];
				if (nextAnimalPositions[key] > BreedingThreshold) {
					nextAnimalPositions[x + y * Data.Width] = nextAnimalPositions[key] / 2;
					nextAnimalPositions[key] /= 2;
				}
				else {
					int nextVal = nextAnimalPositions[key] - 1;
					if (nextVal > 0) {
						nextAnimalPositions[x + y * Data.Width] = nextVal;
					}
					nextAnimalPositions.Remove(key);
				}
			}
		}
	}
}