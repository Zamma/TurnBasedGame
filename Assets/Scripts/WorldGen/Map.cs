using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class Map : MonoBehaviour {

	public Camera camera;
	public Tile tile;
	public static int HEIGHT = 30;
	public static int WIDTH = 30;
	public static int NOISE = WIDTH/5; //Higher numbers mean sharper changes in terrain.
	public static int BIOMESEPERATION = 3; //distance between biomes
	public Tile[,] map = new Tile[WIDTH,HEIGHT];
	private LinkedList<Tile> highlighted = new LinkedList<Tile>();
	public Controller controller;

	// Use this for initialization
	void Start () {

		List<Biome> biomes = generateBiomes();
		makeMap(biomes);
		foreach (Biome biome in biomes){
			biome.makeFeatures();
		}
		/*
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
		*/
	}

	void Update(){
		print ("starting");
		makeAllFog();
		Grid.turnManager.startFactions();
		this.enabled = false;
	}

	private List<Biome> generateBiomes(){
		List<Biome> biomes = new List<Biome>();
		float total = Biome.WATERCHANCE + Biome.FORESTCHANCE + Biome.GRASSCHANCE;
		int biomesNeeded = (WIDTH/BIOMESEPERATION)*(HEIGHT/BIOMESEPERATION);
		float water = (Biome.WATERCHANCE/total)*biomesNeeded, grass = (Biome.GRASSCHANCE/total)*biomesNeeded, forest = (Biome.FORESTCHANCE/total)*biomesNeeded;
		float r;
		for (int y = 0; y<HEIGHT ; y+=BIOMESEPERATION){
			for (int x = 0; x<WIDTH ; x+=BIOMESEPERATION){
				r = UnityEngine.Random.value*(water+grass+forest);
				if (x == WIDTH/2 && y == HEIGHT/2) {biomes.Add(new GrassBiome(x,y));} ///center of the map.
				else if (r < water){water -= 1; biomes.Add(new WaterBiome(x,y));}
				else if (r<(water + grass)) {grass -= 1;biomes.Add(new GrassBiome(x,y));}
				else if (r<(water + grass + forest)) {forest -= 1;biomes.Add(new ForestBiome(x,y));}
			}
		}
		return biomes;
	}

	private void makeMap(List<Biome> biomes){
		float forest,grass,water;
		float r,total,modifier;
		for (int y = 0; y<HEIGHT;y++){
			for (int x = 0;x<WIDTH;x++){
				makeTile (x,y);
				grass = water = forest = 0;
				foreach(Biome biome in biomes){
					modifier = biome.strength/((Mathf.Pow(distance(x,biome.x,y,biome.y), biome.falloff))+0.01f);
					if (biome.type == "grass") {grass += modifier;}
					else if (biome.type == "water") {water += modifier;}
					else if (biome.type == "forest") {forest += modifier;}
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

	private Tile makeTile(int x,int y){
		Tile tile = Instantiate(Grid.prefabLoader.baseTile,new Vector3(x,y,0),Quaternion.identity) as Tile;
		tile.transform.parent = transform;
		tile.setPosition(x,y);
		map[x,y] = tile;
		return tile;
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
				map[x,y].transform.parent = transform;
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

		unit.mov = searchForTileInRange(unit.tile.x,unit.tile.y,map[x,y],unit.mov,unit.moveCosts);//options.get(x,y); //reduces the units movement
		
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
	/*
	private void findOptionsPartTwo(int x, int y, int move, GameOptions options,int n){ //highlights all tiles that the unit can move to.
		int max = 10;
		print ("depth of : " + n);
		//print (x + ", " + y);
		//print (options.get (x,y));
		if (options.get (x,y) != -1 && options.get (x,y) <= move){
			print ("return point a: " + options.get (x,y));
			return;
		}
		else {
			options.set(x,y,move);
		}
		Tile tile = map[x,y];

		if (x-1 >= 0 && move + (int)TileCosts.Basic.costs[map[x-1,y].type] < max) //check that the next space is in the map and that it does not exceed movement cost.
		{findOptionsPartTwo (x-1,y,move + (int)TileCosts.Basic.costs[map[x-1,y].type], options, n+1);} 
		if (x+1 < WIDTH && move + (int)TileCosts.Basic.costs[map[x+1,y].type] < max)
		{findOptionsPartTwo (x+1,y,move + (int)TileCosts.Basic.costs[map[x+1,y].type], options,n+1);}
		if (y-1 >= 0 && move + (int)TileCosts.Basic.costs[map[x,y-1].type] < max)
		{findOptionsPartTwo (x,y-1,move + (int)TileCosts.Basic.costs[map[x,y-1].type], options,n+1);}
		if (y+1 < HEIGHT && move + (int)TileCosts.Basic.costs[map[x,y+1].type] < max)
		{findOptionsPartTwo (x,y+1,move + (int)TileCosts.Basic.costs[map[x,y+1].type], options,n+1);}

		return;
	}
*/
	//method called by wrapper findOptions
	private void fo(int x,int y,TileCosts costs, int distance, GameOptions explored){
		int max = 50;
		if (distance > max) return;
		if (!onMap(x,y)) return;
		if (explored.get (x,y) != -1){ //has been explored.
			if (explored.get(x,y) > distance){
				explored.set (x,y,distance);
			}
			else return;//there was a more efficient route
		}
		else{
			explored.set(x,y,distance);
		}
		if (onMap(x+1,y))fo (x+1,y,costs,distance + (int)costs.costs[map[x+1,y].type],explored);
		if (onMap(x-1,y))fo (x-1,y,costs,distance + (int)costs.costs[map[x-1,y].type],explored);
		if (onMap(x,y+1))fo (x,y+1,costs,distance + (int)costs.costs[map[x,y+1].type],explored);
		if (onMap(x,y-1))fo (x,y-1,costs,distance + (int)costs.costs[map[x,y-1].type],explored);
		return;
	}


	public GameOptions findOptions(int x, int y, int move)
	{
		GameOptions options = new GameOptions();
		fo(x,y,TileCosts.Basic,0,options);
		options.finalize();
		return options;
	}
	//returns the distance to the tile
	public int searchForTile(int x,int y,Tile tile, int mov, TileCosts costs){
		int max = 20;
		//print ("doing " + mov);
		//if (mov < gridDistance(x,tile.x,y,tile.y)) return -100; //failure
		if (mov >= max) return int.MaxValue; //stop looking after some point
		if (tile.Equals(map[x,y])) return mov; //success

		int left,right,up,down,final;
		final = left = right = up = down = int.MaxValue; //high value so it wont be chosen
		if (onMap (x+1,y)) right = gridDistance(x+1,tile.x,y,tile.y) + (int)costs.costs[map[x+1,y].type];
		if (onMap (x-1,y)) left = gridDistance(x-1,tile.x,y,tile.y) + (int)costs.costs[map[x-1,y].type];
		if (onMap (x,y+1)) up = gridDistance(x,tile.x,y+1,tile.y) + (int)costs.costs[map[x,y+1].type];
		if (onMap (x,y-1)) down = gridDistance(x,tile.x,y-1,tile.y) + (int)costs.costs[map[x,y-1].type];

		while(up < int.MaxValue || down < int.MaxValue || left < int.MaxValue || right < int.MaxValue){
			if(up < down && up < right && up < left){
				final = searchForTile(x,y+1,tile,mov + (int)costs.costs[map[x,y+1].type],costs);
				if (final >= max) up = int.MaxValue; //this condition is so it will check the other options if it deadends
				else return final;
			}
			if (down < left && down < right){
				final = searchForTile(x,y-1,tile,mov + (int) costs.costs[map[x,y-1].type],costs);
				if (final >= max) down = int.MaxValue;
				else return final;
			}
			if (left < right){
				final = searchForTile(x-1,y,tile,mov + (int) costs.costs[map[x-1,y].type],costs);
				if (final >= max) left = int.MaxValue;
				else return final;
			}
			if (right < int.MaxValue){
				final = searchForTile(x+1,y,tile,mov + (int) costs.costs[map[x+1,y].type],costs);
				if (final >= max) right = int.MaxValue;
				else return final;
			}
		}
		return final;
	}
	//returns the remaining movement
	public int searchForTileInRange(int x, int y,Tile tile,int mov, TileCosts costs){
		int final = mov - searchForTile(x,y,tile,0,costs);
		if (final < 0) return 0;
		else return final;
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
	//unoptimized method to get tiles in a range which excludes tiles from a lower bound.
	//currently excludes the lower radius based on tile distance rather than how much movement it took to get there.
	//I think that's OK?
	public List<Tile> getTilesInRange(int x,int y,TileCosts costs,int lower,int upper){
		List<Tile> tiles = getTilesInMovement(x,y,costs,upper);
		List<Tile> matchedTiles = new List<Tile>();
		foreach(Tile tile in tiles){
			if (gridDistance(tile.x,x,tile.y,y) >= lower){
				matchedTiles.Add(tile);
			}
		}
		return matchedTiles;
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

	public List<Tile> getAllTiles(){
		List<Tile> tiles = new List<Tile>();
		for(int i = 0; i<WIDTH;i++){
			for(int j = 0;j<HEIGHT;j++){
				tiles.Add(map[i,j]);
			}
		}
		return tiles;
	}

	public List<Tile> getAllVisibleTiles(){
		List<Tile> tiles = getAllTiles();
		List<Tile> visible = new List<Tile>();
		foreach(Tile tile in tiles){
			if (!tile.getHighlight().Equals(Highlight.FOG)){
				visible.Add(tile);
			}
		}
		return visible;
	}

	//makes all tiles fogged
	public void makeAllFog(){
		for(int i = 0;i<WIDTH;i++){
			for(int j = 0;j<HEIGHT;j++){
				map[i,j].makeFog();
			}
		}
	}
	
}
