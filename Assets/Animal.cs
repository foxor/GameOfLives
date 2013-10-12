using UnityEngine;
using System.Collections.Generic;

public class Animal {
	
	public const int TERRESTRIAL_FLAG = 1;
	public const int AQUATIC_FLAG = 2;
	
	public const int HERBIVOR_FLAG = 1;
	public const int CARNIVOR_FLAG = 2;
	
	public string name;
	public Color color;
	public int target_elevation;
	public int habitat;
	public int diet;
	public int maxHealth;
	
	public List<int[]> currentAnimalPositions;
	
	public Animal(string name, Color color, int habitat, int diet, int maxHealth) {
		this.name = name;
		this.color = color;
		this.habitat = habitat;
		this.diet = diet;
		this.maxHealth = maxHealth;
		
		currentAnimalPositions = new List<int[]>();
	}
	
	public Animal() {
		currentAnimalPositions = new List<int[]>();
	}
	
	public bool canSwim() {
		return (habitat | AQUATIC_FLAG) > 0;
	}
	
	public bool canWalk() {
		return (habitat | TERRESTRIAL_FLAG) > 0;
	}
	
	public bool eatsPlant() {
		return (diet | HERBIVOR_FLAG) > 0;
	}
	
	public bool eatsMeat() {
		return (diet | CARNIVOR_FLAG) > 0;
	}
	
	protected int determineDirection() {
		return 0;
	}
	
	public byte Process(byte val, int x, int y) {
		return 0;
	}
	
	public void PerFrame() {
		foreach (int[] position in currentAnimalPositions) {
			
		}
	}
	
}
