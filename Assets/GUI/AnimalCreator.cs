using UnityEngine;
using System.Collections;
using System.Linq;

public class AnimalCreator : MonoBehaviour {
	
	protected static string[] ANIMAL_NAMES = new string[] {
		"Hippogryph",
		"Basalisk",
		"Grizzly bear",
		"Fox",
		"Hound",
		"Pidgeon",
		"Snake",
		"Worm",
		"Mouse",
		"Hawk",
		"Lion",
		"Cheetah",
		"Giraffe",
		"Rhino",
		"Tricerotops",
		"Whale",
		"Salmon",
		"Oyster",
		"Human",
		"Elephant",
		"Polar bear",
		"Housecat",
		"Eagle",
		"Tuna",
		"Octopus",
		"Cthulhu"
	};
	
	public static int PADDING = 5;
	public Point windowSize, expanderSize;
	
	protected Rect windowRect, windowHoverRect;
	protected Rect expanderRect, expanderHoverRect;
	protected Rect testRect;
	
	private int lastScreenWidth, lastScreenHeight;
	
	private bool expanded;
	
	private Vector2 scrollViewVector;
	
	private string nameSelection;
	
	protected Color colorSelection;
	protected Texture2D colorTex;
	
	protected bool eatsMeatSelection, eatsPlantSelection;
	
	protected bool walksSelection, swimsSelection;
	
	protected byte targetElevationSelection;
	
	protected byte breedingThresholdSelection;
	
	protected float activitySelection;
	
	protected float aggressionSelection;
	
	protected float birthRatioSelection;
	
	protected float combatAbilitySelection;
	
	protected float efficiencySelection;
	
	// Use this for initialization
	void Start () {
		Randomize();
		
		lastScreenWidth = Screen.width;
		lastScreenHeight = Screen.height;
		
		windowRect = new Rect();
		windowHoverRect = new Rect();
		
		expanderRect = new Rect();		
		expanderHoverRect = new Rect();
		
		windowSize = new Point(200, 300);
		expanderSize = new Point(100, 30);
		
		determineRectangles();
		
		colorTex = new Texture2D((int)(windowRect.width*.75 + 0.5), 16);
	}
	
	protected void Randomize() {
		nameSelection = ANIMAL_NAMES.OrderBy(x => Random.Range(0f, 1f)).First();
		expanded = false;
		colorSelection = new Color(
			Random.Range(0f, 1f),
			Random.Range(0f, 1f),
			Random.Range(0f, 1f),
			1f
		);
		
		eatsMeatSelection = Random.Range(0f, 1f) < 0.666f;
		eatsPlantSelection = !eatsMeatSelection || (Random.Range(0f, 1f) < 0.5f);
		
		
		targetElevationSelection = (byte)Random.Range(0, 255);
		
		swimsSelection = targetElevationSelection <= Water.SEA_LEVEL || Random.Range(0f, 1f) < 0.5f;
		walksSelection = targetElevationSelection > Water.SEA_LEVEL || (Random.Range(0f, 1f) < 0.5f);
		
		breedingThresholdSelection = (byte)Random.Range(0, 255);
		
		activitySelection = Random.Range(0f, 1f);
		
		aggressionSelection = Random.Range(0f, 1f);
		
		birthRatioSelection = Random.Range(0f, 1f);
		
		combatAbilitySelection = Random.Range(0f, 1f);
		
		efficiencySelection = Random.Range(0f, 1f);
	}
	
	
	// Update is called once per frame
	void Update () {
		if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height) {
			lastScreenWidth = Screen.width;
			lastScreenHeight = Screen.height;
			resizeEvent();
		}
		
		if (!walksSelection) {
			if (targetElevationSelection > Water.SEA_LEVEL) {
				targetElevationSelection = Water.SEA_LEVEL;
			}
		}
		if (!swimsSelection) {
			if (targetElevationSelection < Water.SEA_LEVEL) {
				targetElevationSelection = Water.SEA_LEVEL;
			}
		}
	
	}
	
	void OnGUI() {
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.y = Screen.height - mousePosition.y;
		
		if (expanderHoverRect.Contains(mousePosition)) {
			expanded = true;
		}
		else if (expanded && !windowHoverRect.Contains(mousePosition)) {
			expanded = false;
		}
		
		if (expanded) {
			windowRect = GUI.Window(0, windowRect, windowFunction, "");			
		}
		else {
			GUI.Box(expanderRect, "Make a creature");
		}
		
	}
	
	void windowFunction(int windowID) {
		GUI.skin.label.normal.textColor = Color.white;
		
		scrollViewVector = GUILayout.BeginScrollView(scrollViewVector, GUILayout.Width(windowRect.width), GUILayout.Height(windowRect.height));
		
		nameSelection = GUILayout.TextField(nameSelection);
		
		GUILayout.Label("Color");
		colorSelection.r = GUILayout.HorizontalSlider(colorSelection.r, 0f, 1f);
		colorSelection.g = GUILayout.HorizontalSlider(colorSelection.g, 0f, 1f);
		colorSelection.b = GUILayout.HorizontalSlider(colorSelection.b, 0f, 1f);
		colorSelection.a = 1f;
		fillTexture(colorTex, colorSelection);
		colorTex.Apply();
		GUILayout.Box(colorTex);
		
		GUILayout.Label("Diet");
		eatsPlantSelection = GUILayout.Toggle(eatsPlantSelection, "Eats Plants");
		eatsMeatSelection = GUILayout.Toggle(eatsMeatSelection, "Eats Meat");
		if (!(eatsMeatSelection || eatsPlantSelection)) {
			eatsPlantSelection = true;
		}
		
		GUILayout.Label("Habitat");
		walksSelection = GUILayout.Toggle(walksSelection, "Walks on Land");
		swimsSelection = GUILayout.Toggle(swimsSelection, "Swims in water");
		if (!(walksSelection || swimsSelection)) {
			walksSelection = true;
		}
		
		GUILayout.Label("Target Elevation: " + targetElevationSelection);
		targetElevationSelection = (byte)GUILayout.HorizontalSlider(targetElevationSelection, (swimsSelection) ? 0 : Water.SEA_LEVEL, (walksSelection) ? 255 : Water.SEA_LEVEL);
		
		GUILayout.Label("Breeding Threshold: " + breedingThresholdSelection);
		breedingThresholdSelection = (byte)GUILayout.HorizontalSlider(breedingThresholdSelection, 0, 255);
		
		GUILayout.Label("Activity: " + activitySelection);
		activitySelection = GUILayout.HorizontalSlider(activitySelection, 0, 1);
		activitySelection = (float)((int)(activitySelection*100 + 0.5f))/100;
		
		GUILayout.Label("Aggression: " + aggressionSelection);
		aggressionSelection = GUILayout.HorizontalSlider(aggressionSelection, 0, 1);
		aggressionSelection = (float)((int)(aggressionSelection*100 + 0.5f))/100;
		
		GUILayout.Label("Birth Ratio: " + birthRatioSelection);
		birthRatioSelection = GUILayout.HorizontalSlider(birthRatioSelection, 0, 1);
		birthRatioSelection = (float)((int)(birthRatioSelection*100 + 0.5f))/100;
		
		GUILayout.Label("Combat Ability: " + combatAbilitySelection);
		combatAbilitySelection = GUILayout.HorizontalSlider(combatAbilitySelection, 0, 1);
		combatAbilitySelection = (float)((int)(combatAbilitySelection*100 + 0.5f))/100;
		
		GUILayout.Label("Efficiency: " + efficiencySelection);
		efficiencySelection = GUILayout.HorizontalSlider(efficiencySelection, 0, 1);
		efficiencySelection = (float)((int)(efficiencySelection*100 + 0.5f))/100;
		
		if (GUILayout.Button("Create")) {
			LayerManager.AddLayer(new Animal() {
				Activity = activitySelection,
				Aggression = aggressionSelection,
				BirthWeight = birthRatioSelection,
				BreedingThreshold = breedingThresholdSelection,
				CombatAbility = combatAbilitySelection,
				Color = colorSelection,
				Digestion = 
					(eatsMeatSelection ? Animal.CARNIVOR_FLAG : 0) |
					(eatsPlantSelection ? Animal.HERBIVOR_FLAG : 0),
				Habitat = 
					(walksSelection ? Animal.TERRESTRIAL_FLAG : 0) |
					(swimsSelection ? Animal.AQUATIC_FLAG : 0),
				Inefficiency = efficiencySelection,
				TargetElevation = targetElevationSelection
			});
			
			Randomize();
		}
		
		GUI.EndScrollView();
	}
	
	private void resizeEvent() {
		determineRectangles();
	}
	
	private void determineRectangles() {
		expanderRect.Set(Screen.width - expanderSize.x - PADDING, Screen.height - expanderSize.y - PADDING, expanderSize.x, expanderSize.y);
		expanderHoverRect.Set(Screen.width - expanderSize.x - 2*PADDING, Screen.height - expanderSize.y - 2*PADDING, expanderSize.x + PADDING, expanderSize.y + PADDING);
		
		windowRect.Set(Screen.width - windowSize.x - PADDING, Screen.height - windowSize.y - PADDING, windowSize.x, windowSize.y);
		windowHoverRect.Set(Screen.width - windowSize.x - 2*PADDING, Screen.height - windowSize.y - 2*PADDING, windowSize.x + PADDING, windowSize.y + PADDING);
	}
	
	private static void fillTexture(Texture2D tex, Color color) {
		for (int x = 0; x < tex.width; x++) {
			for (int y = 0; y < tex.height; y++) {
				tex.SetPixel(x, y, color);
			}
		}
	}
}
