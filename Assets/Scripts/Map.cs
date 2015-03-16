using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class Map : MonoBehaviour {

	public Camera camera;
	public Tile tile;
	public static int HEIGHT = 20;
	public static int WIDTH = 20;
	public static int NOISE = WIDTH/5; //Higher numbers mean sharper changes in terrain.
	public Tile[,] map = new Tile[WIDTH,HEIGHT];
	private LinkedList<Tile> highlighted = new LinkedList<Tile>();
	public Controller controller;

	// Use this for initialization
	void Start () {

		//intialize chunks.
		int h = WIDTH/NOISE; //distance between chunks
		Chunk[] chunks = new Chunk[NOISE*NOISE]; //does not allow for non-square worlds.
		for (int y = 0; y<HEIGHT ; y+=h){
			for (int x = 0; x<WIDTH ; x+=h){
			//map[x,y] = Instantiate(tile,new Vector3(x,y,0),Quaternion.identity) as Tile;
				chunks[((y/h)*NOISE)+(x/h)] = new Chunk((h/2)+x,(h/2)+y);//Should not matter that chunks can be outside the borders since they are never placed on map
				//print ("made chunk at: " + chunks[((y/h)*NOISE)+(x/h)].x + "," + chunks[((y/h)*NOISE)+(x/h)].y);
			}
		}

		makeTerrain(chunks);

		for(int i = 0; i<20; i++){
			int xcor = (int)(Random.value * WIDTH);
			int ycor = (int)(Random.value * HEIGHT);
			gameObject.GetComponent<Unitmanager>().createUnit(xcor,ycor,map[xcor,ycor]); //test. not important, can delete.
		}
	}

	private void makeTerrain(Chunk[] chunks){
		//make terrain
		float forest = 0,grass = 0,water = 0;
		Chunk chunk;
		float r; //the random value.
		float total;
		float modifier;
		for (int y=0;y<HEIGHT;y++){
			for (int x=0;x<WIDTH;x++){ //for each tile

				map[x,y] = Instantiate(tile,new Vector3(x,y,0),Quaternion.identity) as Tile; //make the tile
				map[x,y].controller = controller;
				map[x,y].setPosition(x,y); //gives the tile information of its posistion.
				grass = 0; water = 0; forest = 0;
				for(int i=0;i<NOISE*NOISE;i++){ //then go through every chunk to adjust the probabilities for the tile being a certain terrain type.
					chunk = chunks[i];
					modifier = chunk.strength/((Mathf.Pow(distance(x,chunk.x,y,chunk.y), Chunk.FALLOFF[chunk.type]))+0.01f);
					if (chunk.type == "grassland") {grass += modifier;}
					else if (chunk.type == "water") {water += modifier;}
					else if (chunk.type == "forest") {forest += modifier;}
					else {print ("was not any of the terrain types");}
				}
				//make the total sum to 1
				total = grass + water + forest;
				grass /= total;
				water /= total;
				forest /= total;
				r = Random.value; //0-1

				if (r < grass) {map[x,y].MakeGrass();}
				else if (r < water + grass) {map[x,y].MakeWater();}
				else if (r < forest + water + grass) {map[x,y].MakeForest();}
				else {print("error: did not pick a terrain type");print ("grass: "+grass+" water: "+water+" forest: "+forest+" on: "+x+","+y);}
			}
		}
	}

	public float distance(int xOne, int xTwo, int yOne, int yTwo){
		float d = (xTwo - xOne)*(xTwo - xOne) + (yTwo - yOne)*(yTwo - yOne);
		return Mathf.Pow(d,0.5f);
	}

	public int gridDistance(int xOne, int xTwo, int yOne, int yTwo){
		int distance = Mathf.Abs(xTwo - xOne);
		distance += Mathf.Abs(yTwo - yOne);
		return distance;
	}
	/*
	void Update(){
		//Vector3 position = GetComponent<Transform>().position;
		Vector3 pixelCor = new Vector3( Input.mousePosition.x, Input.mousePosition.y, 0f); //get mouse position in pixels
		Vector3 worldCor = camera.ScreenToWorldPoint(pixelCor);	//translate to game coordinates.
		worldCor.x += .16f; //further conversion to adjust for tiles being aligned centrally rather than at the lower left
		worldCor.y += .16f;
		
		if (Input.GetMouseButtonDown(0))
		{
			int x = Mathf.FloorToInt(worldCor.x);	//rounding to the nearest tile coordinate.
			int y = Mathf.FloorToInt(worldCor.y);
			print ("at: " + x + "," + y);
			if ( x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT){ // if the tile is within the map bounds.

				//map[x,y].highlight(new Color(.09f,.180f,.220f)); //blue highlight.
				if (map[x,y].hasUnit()){
					print ("has unit");
					int move = map[x,y].unit.mov;
					findOptions(x,y,move);
				}
			}
		}
	} */

	public static void worldToMapCor(Vector3 worldCor){ //further conversion to adjust for tiles being aligned centrally rather than at the lower left
		worldCor.x += .16f;
		worldCor.y += .16f;
	}

	public bool onMap(Vector3 worldCor){ //returns true if the coordinate is on the map.

		int x = Mathf.RoundToInt(worldCor.x);	//converting to int
		int y = Mathf.RoundToInt(worldCor.y);

		if ( x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT){ // if the tile is within the map bounds.
			return true;
		}
		else return false;
	}

	public bool onMap(int x, int y){
		if ( x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT){ // if the tile is within the map bounds.
			return true;
		}
		else return false;
	}

	public bool hasUnit(Vector3 worldCor){
		int x = Mathf.RoundToInt(worldCor.x);	//converting to int
		int y = Mathf.RoundToInt(worldCor.y);
		if (map[x,y].hasUnit()){
			return true;
		}
		else return false;
	}

	public void deselect(int x,int y,int mov,TileCosts costs){ //deselects all selected (highlighted) tiles.
		List<Tile> tiles = getTilesInMovement(x,y,costs,mov);
		foreach (Tile tile in tiles){
			tile.clearHighlight();
		}
		/*Tile currentTile;
		while(highlighted.Count > 0){
			currentTile = highlighted.First.Value;
			highlighted.RemoveFirst();
			currentTile.clearHighlight();
		}*/
	}

	public Unit selectUnit(Vector3 worldCor){ //selects the unit at the coordinates and returns it.
		if (hasUnit(worldCor)){
			int x = Mathf.RoundToInt(worldCor.x);	//converting to int
			int y = Mathf.RoundToInt(worldCor.y);

		//	int move = map[x,y].unit.mov;
		//	findOptions(x,y,move);

			return map[x,y].unit;
		}
		return null;
	}

	public bool isHighlighted(Vector3 worldCor){ //returns true if the tile at the given coordinates is highlighted. No longer Needed?
		int x = Mathf.RoundToInt(worldCor.x);	//converting to int
		int y = Mathf.RoundToInt(worldCor.y);

		Tile currentTile;
		while(highlighted.Count > 0){
			currentTile = highlighted.First.Value;
			currentTile.clearHighlight();
			highlighted.RemoveFirst();
			if (currentTile.Equals(map[x,y])){
				//deselect();
				return true;
			}
		}
		return false;
	}

	public Tile moveUnit(Unit unit, int x, int y){//, GameOptions options){

		int origX = (int)unit.transform.position.x; //original position.
		int origY = (int)unit.transform.position.y;

		unit.mov = searchForTile(unit.tile.x,unit.tile.y,map[x,y],unit.mov,unit.moveCosts);//options.get(x,y); //reduces the units movement
		
		map[origX,origY].unit = null; //remove the unit from the old tile.
		map[x,y].unit = unit; //and put him on the other tile.

		unit.setTile(map[x,y]);

		unit.transform.position = new Vector3((float) x, (float) y, 0f);

		return map[origX,origY]; //returns the tile so it can be returned to if the move is reversed.
	}

	public void revertUnitToTile(Tile tile, Unit unit) //returns a unit to a tile.
	{
		int x = (int) unit.transform.position.x;
		int y = (int) unit.transform.position.y;

		map[x,y].unit = null; //remove the unit from the old tile.
		tile.unit = unit; //and put him on the other tile.

		unit.setTile(tile); //tell the unit its new position.

		unit.transform.position = new Vector3((float) tile.x, (float) tile.y, 0f);

		unit.restoreMove();

	}
	
	private GameOptions findOptionsPartTwo(int x, int y, int move, GameOptions options){ //highlights all tiles that the unit can move to.
		if (options.get (x,y) != null && options.get (x,y) > move){
			return options;
		}
		else {
			options.set(x,y,move);
		}
		Tile tile = map[x,y];
		
		tile.highlight (new Color(.09f,.180f,.220f)); //highlight blue
		highlighted.AddFirst(tile);

		if (x-1 >= 0 && move - map[x-1,y].moveCost >= 0) //check that the next space is in the map and that it does not exceed movement cost.
			{findOptionsPartTwo (x-1,y,move - map[x-1,y].moveCost, options);}
		if (x+1 < WIDTH && move - map[x+1,y].moveCost >= 0)
			{findOptionsPartTwo (x+1,y,move - map[x+1,y].moveCost, options);}
		if (y-1 >= 0 && move - map[x,y-1].moveCost >= 0)
			{findOptionsPartTwo (x,y-1,move - map[x,y-1].moveCost, options);}
		if (y+1 < HEIGHT && move - map[x,y+1].moveCost >= 0)
			{findOptionsPartTwo (x,y+1,move - map[x,y+1].moveCost, options);}

		return options;
	}


	public GameOptions findOptions(int x, int y, int move)
	{
		GameOptions options = new GameOptions();
		return findOptionsPartTwo(x,y,move,options);
	}

	public int searchForTile(int x,int y,Tile tile, int mov, TileCosts costs){
		if (mov < gridDistance(x,tile.x,y,tile.y)) return -100; //failure
		if (tile.Equals(map[x,y])) return mov; //success

		int left,right,up,down,final;
		right = gridDistance(x+1,tile.x,y,tile.y) + (int)costs.costs[map[x+1,y].type];
		left = gridDistance(x-1,tile.x,y,tile.y) + (int)costs.costs[map[x-1,y].type];
		up = gridDistance(x,tile.x,y+1,tile.y) + (int)costs.costs[map[x,y+1].type];
		down = gridDistance(x,tile.x,y-1,tile.y) + (int)costs.costs[map[x,y-1].type];

		if(up < down && up < right && up < left){
			final = searchForTile(x,y+1,tile,mov - (int)costs.costs[map[x,y+1].type],costs);
			if (final >= 0) return final;
		}
		if (down < left && down < right){
			final = searchForTile(x,y-1,tile,mov - (int) costs.costs[map[x,y-1].type],costs);
			if (final >= 0) return final;
		}
		if (left < right){
			final = searchForTile(x-1,y,tile,mov - (int) costs.costs[map[x-1,y].type],costs);
			if (final >= 0) return final;
		}
		final = searchForTile(x+1,y,tile,mov - (int) costs.costs[map[x+1,y].type],costs);
		return final;
	}

	//returns the tiles surrounding around x and y.
	private List<Tile> getTilesFromCircle(int x, int y, int inner, int outer){
		List<Tile> tiles,verticalTiles;
		List<Tile> finalTiles = new List<Tile>();
		tiles = getTilesFromPath(x,y,outer,-1,0,0,outer); //if bug, check parameters
		tiles.AddRange(getTilesFromPath(x,y,outer,1,0,0,outer));
		int distanceFromcenter;
		foreach(Tile tile in tiles){
			distanceFromcenter = Mathf.Abs(x - tile.x);
			verticalTiles = getTilesFromPath(tile.x,y,outer - distanceFromcenter,0,1,inner - distanceFromcenter,outer-distanceFromcenter);
			verticalTiles.AddRange(getTilesFromPath(tile.x,y-1,outer - distanceFromcenter-1,0,-1,inner - distanceFromcenter-1,outer-distanceFromcenter));// the -1s should avoid duplicates.
			finalTiles.AddRange(verticalTiles);
		}
		return finalTiles;
	}
	public List<Tile> getUnitMove(Unit unit){
		return getTilesInMovement(unit.tile.x,unit.tile.y,unit.moveCosts,unit.mov);
	}

	public List<Tile> getTilesInMovement(int x, int y,TileCosts costs,int distance){
		List<Tile> tiles = new List<Tile>();
		GameOptions options = new GameOptions();
		tiles = gtim (x,y,costs,distance,options,tiles);
		return tiles;
	}
	//method called by wrapper getTilesInMovement
	private List<Tile> gtim(int x,int y,TileCosts costs, int distance, GameOptions explored,List<Tile> tiles){
		if (distance < 0) return new List<Tile>();
		if (!onMap(x,y)) return new List<Tile>();
		if (tiles.Contains(map[x,y])){
			if (explored.get(x,y) < distance){
				explored.set (x,y,distance);
			}
			else return new List<Tile>();//there was a more efficient route
		}
		else{
			tiles.Add (map[x,y]);
			explored.set(x,y,distance);
		}
		if (onMap(x+1,y))gtim (x+1,y,costs,distance - (int)costs.costs[map[x+1,y].type],explored,tiles);
		if (onMap(x-1,y))gtim (x-1,y,costs,distance - (int)costs.costs[map[x-1,y].type],explored,tiles);
		if (onMap(x,y+1))gtim (x,y+1,costs,distance - (int)costs.costs[map[x,y+1].type],explored,tiles);
		if (onMap(x,y-1))gtim (x,y-1,costs,distance - (int)costs.costs[map[x,y-1].type],explored,tiles);
		return tiles;
	}

	//returns tiles from starting at a point and moving a direction a certain distance. The first and last let you set a range
	//of tiles to select. If you want all tiles in the path, choose first = 0; last = distance;
	private List<Tile> getTilesFromPath(int startingx, int startingy, int distance, int dx, int dy,int first, int last){
		List<Tile> tiles = new List<Tile>();
		int x= startingx;
		int y= startingy;
		for (int i = 0; i<=distance;i++){
			x=startingx + dx*i;
			y=startingy + dy*i;
			if (!onMap (x,y)){
				continue;
			}
			else if (i >= first && i <= last){
				tiles.Add(map[x,y]);
			}
		}
		return tiles;
	}
	//takes a list of tiles and highlights them all.
	private void highlightTiles(LinkedList<Tile> tiles, Color color){
		Tile tile;
		while (tiles.Count > 0){
			tile = tiles.First.Value;
			tiles.RemoveFirst();
			tile.highlight(color);
		}
	}

	//highlight an area starting from the lowerRadius and ending with the higherRadius.
	public void highlight(int x, int y, int lowerRadius,int higherRadius, Color color){
		foreach(Tile tile in getTilesFromCircle(x,y,lowerRadius,higherRadius)){
			tile.highlight(color);
		}
	}
	
}


public class Chunk{

	public static float STRENGTHVARIANCE = 10; //determines by how much the strengths of chunks can vary
	
	//chance is the weight of that type being chosen. Strength effects how far it spreads linearly. Falloff affects how sharply it cuts out.
	//If you want sharp borders between the terrain types while keeping the area about the same size, increase falloff and strength.
	//If you want gradual borders reduce strength and reduce falloff.
	//falloff is more powerfull than strength (exponential compared to linear).
	public static float WATERCHANCE = 7;
	public static float WATERSTRENGTH = 3;
	public static float GRASSCHANCE = 1;
	public static float GRASSSTRENGTH = 7;
	public static float FORESTCHANCE = 3;
	public static float FORESTSTRENGTH = 1;
												//water,grass,forest
	//public static float[] FALLOFFS = new float[3] {4	,1,		1};
	public static Dictionary<string,float> FALLOFF = new Dictionary<string,float> {{"water",4},{"grassland",1.3f},{"forest",2}};

	public static float TOTALWEIGHTS = WATERCHANCE + GRASSCHANCE + FORESTCHANCE;

	public string type;
	public float strength;
	public int x;
	public int y;
	public static string[] types = new string[3] {"water","forest","grassland"};

	public Chunk(int xcor, int ycor){
		x = xcor;
		y = ycor;
		float random = Random.value;
		if (random < GRASSCHANCE/TOTALWEIGHTS) {type = "grassland"; strength = GRASSSTRENGTH;}
		else if (random < (GRASSCHANCE + WATERCHANCE)/TOTALWEIGHTS) {type = "water"; strength = WATERSTRENGTH;}
		else if (random < (GRASSCHANCE + WATERCHANCE + FORESTCHANCE)/TOTALWEIGHTS) {type = "forest"; strength = FORESTSTRENGTH;}
	}

	public void printvalues(){
		//print("xcor: " + x + ". ycor: " + y + ". type: " + type + ". strength: " + strength + ".");
	}
}