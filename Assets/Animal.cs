using UnityEngine;
using System.Collections.Generic;

public class Animal {
	
	public static int HABITAT_TERRESTRIAL = 0, HABITAT_AQUATIC = 1, HABITAT_AMPHIBIUS = 2;
	public static int DIET_HERBIVOROUS = 0, DIET_CARNIVOROUS = 1, DIET_OMNIVOROUS = 2;
	public static int HEALTH_CAP = 0xFF;
	public static int NORTH = 0, EAST = 1, SOUTH = 2, WEST = 3;
	
	public string name;
	public Color color;
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
		
		currentAnimals = new List<int[]>();
	}
	
	public Animal() {
		currentAnimals = new List<int[]>();
	}
	
	public bool canSwim() {
		return habitat == HABITAT_AQUATIC || habitat == HABITAT_AMPHIBIUS;
	}
	
	public bool canWalk() {
		return habitat == HABITAT_AMPHIBIUS || habitat == HABITAT_TERRESTRIAL;
	}
	
	public bool eatsPlant() {
		return diet == DIET_HERBIVOROUS || diet == DIET_OMNIVOROUS;
	}
	
	public bool eatsMeat() {
		return diet == DIET_OMNIVOROUS || diet == DIET_CARNIVOROUS;
	}
	
	protected int determineDirection() {
		
	}
	
	public byte Process(byte val, int x, int y) {
		
	}
	
	public void PerFrame() {
		foreach (int[] position in currentAnimalPositions) {
			
		}
	}
	
}
