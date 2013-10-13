using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Animal : Layer {
	
	protected static Animal bunny;
	public static Animal Bunny {
		get {
			if (bunny == null) {
				bunny = new Animal(){
					Activity = 0.4f,
					Aggression = 0.4f,
					BreedingThreshold = 60,
					CombatAbility = 0.1f,
					Diet = new List<Layer>(){Grass.Singleton},
					Color = Color.white,
					Habitat = TERRESTRIAL_FLAG,
					Name = "Bunny",
					TargetElevation = 45,
					Rarity = 5
				};
			}
			return bunny;
		}
	}
	
	protected static Animal wolf;
	public static Animal Wolf {
		get {
			if (wolf == null) {
				wolf = new Animal(){
					Activity = 0.05f,
					Aggression = 0.3f,
					BreedingThreshold = 90,
					CombatAbility = 0.6f,
					Diet = new List<Layer>(){Bunny},
					Color = Color.black,
					Habitat = TERRESTRIAL_FLAG,
					Name = "Wolf",
					TargetElevation = 45,
					Rarity = 1000
				};
			}
			return wolf;
		}
	}
	
	public static Dictionary<int, Animal> LayerMapping;
		
	
	protected const int TERRESTRIAL_FLAG = 1;
	protected const int AQUATIC_FLAG = 2;
	
	protected const int HERBIVOR_FLAG = 1;
	protected const int CARNIVOR_FLAG = 2;
	
	protected const int SWIM_DEPTH = Grass.TOO_WET;
	protected const int TERRITORY_DEAD_ZONE = 50;
	protected const int MOVEMENT_ENERGY = 2;
	protected const float STATIONARY_ENERGY = 0.5f;
	
	protected static byte[] flowField;
	
	public int TargetElevation;
	public int Habitat;
	public List<Layer> Diet;
	public float Activity;
	public float Aggression;
	public int BreedingThreshold;
	public float CombatAbility;
	public int Rarity;
	
	protected int lastSpawn;
	protected int layer;
	
	protected Dictionary<int, int> nextAnimalPositions;
	
	public Animal() {
		nextAnimalPositions = new Dictionary<int, int>();
		
		flowField = new byte[Data.Width * Data.Height];
	}
	
	public override void OnStartup (int layer) {
		base.OnStartup (layer);
		
		if (LayerMapping == null) {
			LayerMapping = new Dictionary<int, Animal>();
		}
		LayerMapping[layer] = this;
	}
	
	public bool canSwim() {
		return (Habitat & AQUATIC_FLAG) > 0;
	}
	
	public bool canWalk() {
		return (Habitat & TERRESTRIAL_FLAG) > 0;
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
		
		for (int xPos = xMin; xPos <= xMax; xPos++) {
			for (int yPos = yMin; yPos <= yMax; yPos++) {
				int pos = posClamp(xPos, yPos);
				if (nextAnimalPositions.ContainsKey(pos)) {
					continue;
				}
				if (!canSwim() && Data.Singleton[xPos, yPos, LayerManager.GetLayer<Water>()] >= SWIM_DEPTH) {
					continue;
				}
				else if (!canWalk() && Data.Singleton[xPos, yPos, LayerManager.GetLayer<Water>()] < SWIM_DEPTH) {
					continue;
				}
				if (!Data.boundsOk(xPos, yPos, layer)) {
					continue;
				}
				if (xPos == x && yPos == y) {
					continue;
				}
				foreach (int prey in Diet.Select(d => d.LAYER)) {
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
		
		float roll = Random.Range(0f, 1f);
		if (!foundPrey) {
			if (roll > Activity) {
				return false;
			}
		}
		else {
			if (roll > Aggression) {
				return false;
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
	
	public override byte Process(byte val, int x, int y) {
		int pos = posClamp(x, y);
		if (!canSwim() && Data.Singleton[x, y, LayerManager.GetLayer<Water>()] > SWIM_DEPTH) {
			return 0;
		}
		if (nextAnimalPositions.ContainsKey(pos)) {
			int lastVal = nextAnimalPositions[pos];
			foreach (int prey in Diet.Select(d => d.LAYER)) {
				if (LayerMapping.ContainsKey(prey)) {
					if (!LayerMapping[prey].nextAnimalPositions.ContainsKey(pos)) {
						continue;
					}
					// Do that combat resolution
					float myAttack = CombatAbility * lastVal;
					float theirAttack = LayerMapping[prey].CombatAbility * LayerMapping[prey].nextAnimalPositions[pos];
					float myAttacks = LayerMapping[prey].nextAnimalPositions[pos] / myAttack;
					float theirAttacks = lastVal / theirAttack;
					if (myAttacks < theirAttacks) {
						lastVal += LayerMapping[prey].nextAnimalPositions[pos];
						lastVal = Mathf.Clamp(lastVal, 0, 255);
						nextAnimalPositions[pos] = (byte)lastVal;
						LayerMapping[prey].nextAnimalPositions.Remove(pos);
						Data.Singleton.setNext(x, y, prey, 0);
					}
					else {
						int theirVal = Mathf.Clamp(
							lastVal + LayerMapping[prey].nextAnimalPositions[pos],
							0, 255
						);
						LayerMapping[prey].nextAnimalPositions[pos] = (byte)theirVal;
						return 0;
					}
				}
				else if (Data.Singleton[x, y, prey] > 0) {
					lastVal = Mathf.Clamp(lastVal + Data.Singleton[x, y, prey] * 20, 0, 255);
					Data.Singleton[x, y, prey] /= 2;
					Data.Singleton.setNext(x, y, prey, Data.Singleton[x, y, prey]);
				}
			}
			return (byte)lastVal;
		}
		return 0;
	}
	
	public void AddAnimal(int x, int y) {
		nextAnimalPositions[posClamp(x, y)] = 255;
	}
	
	public override void PerFrame() {
		if (--lastSpawn <= 0) {
			AddAnimal(
				Random.Range(0, Data.Width),
				Random.Range(0, Data.Height)
			);
			lastSpawn = Rarity;
		}
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
					
					int nextVal = nextAnimalPositions[key] - MOVEMENT_ENERGY;
					if (nextVal > 0) {
						nextAnimalPositions[x + y * Data.Width] = nextVal;
					}
					nextAnimalPositions.Remove(key);
				}
			}
			else {
				nextAnimalPositions[key] -= Mathf.FloorToInt(STATIONARY_ENERGY);
				if (Random.Range(0f, 1f) < STATIONARY_ENERGY % 1f) {
					nextAnimalPositions[key] -= 1;
				}
			}
		}
	}
	
	public override byte MaxValue () {
		return (byte)BreedingThreshold;
	}
}