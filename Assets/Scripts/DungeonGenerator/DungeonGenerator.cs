using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour {

    //================ ROOM LOGIC =================================================//
    [SerializeField] GameObject[] Rooms;                //Types of room prefabs
    [SerializeField] List<GameObject> RoomsSpawned;     //List of rooms spawned
    [SerializeField] int roomNum;                       //number of rooms to spawn
    [SerializeField] Vector3 areaCenter;                //Center of where the area should be
	[SerializeField] Vector3 rectSize;                  //Length,Width,Depth of the spawn area
	[SerializeField] float rectMagnitude = 2f;          //Magnitude size of the spawn area
    [SerializeField] LayerMask RoomMask;                //Layer mask for the checking of rooms


    //=============== TILE LOGIC ==================================================//
    [SerializeField] GameObject BlankTile;              //Blank tile for the map
    [SerializeField] GameObject PassageTile;            //Passage tile for passgeways
    [SerializeField] List<GameObject> TilesSpawned;     //List of tiles spawned
    bool hasSpawned;
   



    //============== VARIABLES =====================================================//
    int numOfTries;                                     //Try limit is set to 1000 in spawning code
    int floorSize;                                      //Size of the floor for the map
    [SerializeField] Vector2 MapSize;

    

	// Use this for initialization
	void Start () {
        RoomsSpawned = new List<GameObject>();
        TilesSpawned = new List<GameObject>();
        MapSize = new Vector2(rectSize.x * rectMagnitude, rectSize.y * rectMagnitude);
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // GenerateMaze();
            GenerateMap();
            GenerateRooms();
            GeneratePassages();
        }
	
	}
    void GenerateMap() //Fills in the space with blank tiles first. 
    {
        //int sizeX = Mathf.RoundToInt(rectSize.x * rectMagnitude);
        //int sizeY = Mathf.RoundToInt(rectSize.y * rectMagnitude);

       if(!hasSpawned)
        {
            for (short y = 0; y < (rectSize.y * rectMagnitude); y += 4) //goes by Y axis times the magnitude of the thing
            {
                for (short x = 0; x < (rectSize.x * rectMagnitude); x += 4) //goes by X axis times the magnitude
                {
                    Vector3 gridPos = new Vector3(x, y, 0);  //sets blank space positions

                    GameObject BlankTileClone = Instantiate(BlankTile, gridPos, Quaternion.identity) as GameObject; //spawns blank space
                    BlankTileClone.transform.SetParent(gameObject.transform);   //helps organise the hierarchy better
                    BlankTileClone.name = "Tile " + x / 4 + "," + y / 4;            //names the tiles according to some index system

                    BlankTileClone.GetComponent<TileData>().SetPos(x / 4, y / 4);                                     //saves its data into our tile data class
                    BlankTileClone.GetComponent<TileData>().tileType = TileData.Type.BLANK;                   //sets its type to blank
                    TilesSpawned.Add(BlankTileClone);                                //keeps a list of tiles that spawned

                }
            }
            hasSpawned = true;
        }
    }

    void GenerateRooms() //creates rooms in the grid.
    {
        while (RoomsSpawned.Count < roomNum)      //Loop does not stop until number of tries exceed OR until it reaches desired number of rooms 
        {
            int randomIndex = Random.Range(0, Rooms.Length);            //keeps track of the index in current update frame
            int randomTile = Random.Range(0, TilesSpawned.Count);           //keeps track of current tile in current update frame
            GameObject _RoomClone = Instantiate(Rooms[randomIndex]) as GameObject;      //creates room based on random index
            Collider2D _RoomCollider = _RoomClone.GetComponent<Collider2D>();           //grabs the collider component for easier use later
            _RoomClone.transform.localPosition = TilesSpawned[randomTile].transform.position;      //sets its position to a random tile
           // RoomsSpawned.Add(_RoomClone);                                   //adds the room to list of rooms spawned
            RoomDimensions dimensions = new RoomDimensions(_RoomCollider);
            _RoomClone.GetComponent<RoomData>().posX = Mathf.RoundToInt(dimensions.roomCenter.x/4);
            _RoomClone.GetComponent<RoomData>().posY = Mathf.RoundToInt(dimensions.roomCenter.y/4);

            switch (randomIndex)
            {
                case 0:
                    _RoomClone.GetComponent<RoomData>().roomType = RoomData.RoomTypes.Room1;
                    break;
                case 1:
                    _RoomClone.GetComponent<RoomData>().roomType = RoomData.RoomTypes.Room2;
                    break;
                case 2:
                    _RoomClone.GetComponent<RoomData>().roomType = RoomData.RoomTypes.Room3;
                    break;
            }
           
            //Checks if object is within boundaries. If it is, add it to list, else destroy it
            if (dimensions.leftEdge > 0 && dimensions.rightEdge < MapSize.x && dimensions.topEdge < MapSize.y && dimensions.bottomEdge > 0)       //checks if it exceeds bounds of map
            {
                RoomsSpawned.Add(_RoomClone);               //if its within boundaries,add it to the list
                TilesSpawned[randomTile].GetComponent<TileData>().tileType = TileData.Type.ROOM; //then set tile type to room
               

            }
            else
            {
                RoomsSpawned.Remove(_RoomClone);
                Destroy(_RoomClone);
                TilesSpawned[randomTile].GetComponent<TileData>().tileType = TileData.Type.BLANK; //reverts tile type back
            }
            //Check for overlap
            for (short j = 0; j < RoomsSpawned.Count; j++)                                                          //iterates through the list of rooms spawned
            {
                RoomsSpawned[j].GetComponent<Collider2D>().enabled = true;                                          //enables the collider for checking of bounds
                Vector2 _roomCenter = RoomsSpawned[j].GetComponent<Collider2D>().bounds.center;                     //finds Center of the box
                                                                                                                    /*
                ================================================================================================================================================
                Extents are the half length of the box. So taking the middle -the half length should give you the minX.Adding Center +Extents of Y gives MinY. 
                Using this logic, we find the top left and bottom right of the boxes
                ================================================================================================================================================
                                                                                                                    */
                              
                Vector2 _roomTopLeft = new Vector2(_roomCenter.x - RoomsSpawned[j].GetComponent<Collider2D>().bounds.extents.x, _roomCenter.y + RoomsSpawned[j].GetComponent<Collider2D>().bounds.extents.y);
                Vector2 _roomBottomRight = new Vector2(_roomCenter.x + RoomsSpawned[j].GetComponent<Collider2D>().bounds.extents.x, _roomCenter.y - RoomsSpawned[j].GetComponent<Collider2D>().bounds.extents.y);

                RoomsSpawned[j].GetComponent<Collider2D>().enabled = false;                                         //disables collider of self to prevent self detection lol
                Collider2D Result = Physics2D.OverlapArea(_roomTopLeft, _roomBottomRight);                          //checks if there's another collider at where current room is
                if (Result != null)                                                                                 //if there's a room
                {
                    Result.gameObject.SetActive(false);                                                             //hide the room
                    RoomsSpawned.Remove(Result.gameObject);                                                         //remove the room from the list
                    TilesSpawned[randomTile].GetComponent<TileData>().tileType = TileData.Type.BLANK; //sets tile type to blank
                   
                    //I might have wanted to reposition it instead lol.
                    //Result.transform.position = areaCenter + OnUnitRect(rectSize.x, rectSize.y) * rectMagnitude;
                }
                if (RoomsSpawned[j] != null)                                                                        //catches null exception error where the very room itself that spawned is hidden.
                {
                    RoomsSpawned[j].GetComponent<Collider2D>().enabled = true;
                }
            }
            #region Other Method Of Checking Bounds Colliding Rooms

            //if(dimensions.leftEdge <= 0 || dimensions.rightEdge >= MapSize.x || dimensions.topEdge >= MapSize.y || dimensions.bottomEdge <=0)       //checks if it exceeds bounds of map
            //{
            //    RoomsSpawned.Remove(_RoomClone); //remove the offending room from list
            //    _RoomClone.SetActive(false);     //deactivates and hides offending room
            //    Destroy(_RoomClone);             //removes them just to make it less annoying lol
            //    numOfTries++;                    //adds a try counter
            //}
            #endregion  //Checks if room is placed wrong, THEN removes it

            numOfTries++;
            if (numOfTries > 1000)               //catches infinite loops
            {
                break;
            }

            //Column + Row*MapSize/4 + Row = Index of thing
            //e.g int tileCenterIndex = centerTile.posX + centerTile.posY * (Mathf.RoundToInt(rectMagnitude / 4)) + centerTile.posY;
            if (TilesSpawned[randomTile].GetComponent<TileData>().tileType == TileData.Type.ROOM) //helper to find center of rooms
            {
                TilesSpawned[randomTile].SetActive(false);                              //hides the middle tile
                TileData currentTile = TilesSpawned[randomTile].GetComponent<TileData>();   //gets component of said tile
                floorSize = Mathf.CeilToInt(rectMagnitude / 4)-1;                       //finds the roomsize (ceil so the formula works)
            
                switch (randomIndex) //randomIndex is the room type. Sadly, rooms are hard coded in :c
                {
                    case 0:    
                       //room1
                       // int roomSize = 9;
                       //Grabbing the FIRST index of room
                        int rowNum = currentTile.posX -1;        //grabs the row number of the FIRST tile in room
                        int columnNum = currentTile.posY+1;      //grabs the column number of the FIRST tile in room
                  
                        for (int i =0; i < 9;i++) //forloop to cycle through all the tiles in the room +1 
                        {
                            //formula to grab the index of the room
                            int indexToHide = (rowNum) + (columnNum * floorSize) + (columnNum);
                            rowNum++;  //adds the row num so that when it loops back, it picks the correct row num                     
                            if(i == 2 || i == 5 || i ==8 ) //checks in the forloop to see when column moves down. Maybe use modulo..
                            {
                                columnNum--;                //moves column down
                                rowNum -= 3;                //minuses 2 rows (offset by the above rowNum++)
                            }
                            TilesSpawned[indexToHide].SetActive(false);     //hides said tile but in future for maze, maybe don't hide it
                            TilesSpawned[indexToHide].GetComponent<TileData>().tileType = TileData.Type.ROOM;   //sets the tile type to room
                        }
                        break;
                    case 1:
                        //room 2
                        //int roomsize = 27;
                        //Grabbing the FIRST index of room
                        int rowNum2 = currentTile.posX - 4;        //grabs the row number of the FIRST tile in room
                        int columnNum2 = currentTile.posY + 1;      //grabs the column number of the FIRST tile in room

                        for (int i = 0; i < 27; i++) //forloop to cycle through all the tiles in the room +1 
                        {
                            //formula to grab the index of the room
                            int indexToHide = (rowNum2) + (columnNum2 * floorSize) + (columnNum2);
                            rowNum2++;  //adds the row num so that when it loops back, it picks the correct row num                     
                            if (i == 8 || i == 17 || i == 26) //checks in the forloop to see when column moves down. Maybe use modulo..
                            {
                                columnNum2--;                //moves column down
                                rowNum2 -= 9;                //minuses 2 rows (offset by the above rowNum++)
                            }
                            TilesSpawned[indexToHide].SetActive(false);     //hides said tile but in future for maze, maybe don't hide it
                            TilesSpawned[indexToHide].GetComponent<TileData>().tileType = TileData.Type.ROOM;   //sets the tile type to room
                        }
                        break;
                    case 2:
                        //room 3
                        //int roomsize = 81
                        //Grabbing the FIRST index of room
                        int rowNum3 = currentTile.posX - 4;        //grabs the row number of the FIRST tile in room
                        int columnNum3 = currentTile.posY + 4;      //grabs the column number of the FIRST tile in room

                        for (int i = 0; i < 81; i++) //forloop to cycle through all the tiles in the room +1 
                        {
                            //formula to grab the index of the room
                            int indexToHide = (rowNum3) + (columnNum3 * floorSize) + (columnNum3);
                            rowNum3++;  //adds the row num so that when it loops back, it picks the correct row num                     
                            if (i == 8 || i == 17 || i == 26 || i == 35 || i == 44 || i == 53 || i == 62 || i == 71 || i ==80) //checks in the forloop to see when column moves down. Maybe use modulo..
                            {
                                columnNum3--;                //moves column down
                                rowNum3 -= 9;                //minuses 2 rows (offset by the above rowNum++)
                            }
                            TilesSpawned[indexToHide].SetActive(false);     //hides said tile but in future for maze, maybe don't hide it
                            TilesSpawned[indexToHide].GetComponent<TileData>().tileType = TileData.Type.ROOM;   //sets the tile type to room
                        }
                        break;
                }
            }
        }
    }


    void GeneratePassages()
    {
        //Pick 2 random rooms. Like pick one room, then pick another that is not itself
        //check if said room is connected. Yes? then break. No? Connect it somewhere.
        //Maybe let all rooms be connected or something
        //to make passage, maybe grab from center of room + side, then start building a passage from there.
        //needs to check if rooms are already connected to rooms....
        int firstRoomIndex = Random.Range(0, RoomsSpawned.Count - 1);
        GameObject firstRoom = RoomsSpawned[firstRoomIndex];
       // GameObject secondRoom = RoomsSpawned[]
       for(short i=0; i < RoomsSpawned.Count; i++)
        {
            RoomData _roomData = RoomsSpawned[i].GetComponent<RoomData>();
            if(!_roomData.isConnected)
            {
                int j = Random.Range(1, RoomsSpawned.Count); //because 0 is already picked first
                CorridorBuilder(RoomsSpawned[i], RoomsSpawned[(i + j) % RoomsSpawned.Count]); //ensures it never picks itself
            }
        }
    }

    void CorridorBuilder(GameObject r1, GameObject r2)      //builds corridors by taking 2 random rooms
    {
        RoomData room1 = r1.GetComponent<RoomData>();       //grabs the data from room 1
        RoomData room2 = r2.GetComponent<RoomData>();       //grabs data from room 2
        int x = room1.posX;                                 //finds center.x of room 1
        int y = room1.posY;                                 //find center.y of room 1
        int tileIndex;                                      //init an int for the tile index
        int tries = 0;
        while(x!= room2.posX )      //loops to do all X path movements
        {
            tries++;
            //move X wise
            tileIndex = x + (y * floorSize) + y;            //formula to find tile index
            TilesSpawned[tileIndex].SetActive(false);
            x += x < room2.posX ? 1 : -1;       //if room is to the right, add. else if left, minus  (tile index X)
            
          if(tries > 1000)          //catches any infinite loops
            {
                Debug.Log("exited");
                break;
                
            }
        }
        while(y != room2.posY)
        {
            tries++;               
            //move Y wise
            tileIndex = x + (y * floorSize) + y;            //formula to find tile index
            TilesSpawned[tileIndex].SetActive(false);
            y += y < room2.posY ? 1 : -1;        //if room is to the bottom, add. else if top, minus  (tile index Y)
            if (tries > 1000)          //catches any infinite loops
            {
                Debug.Log("exited");
                break;

            }
        }
    }





    
    
    
    void OnDrawGizmos()                             //Draws the cube and stuff just to show you the bounding boxes of its roaming
    {
        Gizmos.color = Color.black;                 //color of the square
        float offsetX = rectSize.x / 2;             //offsets the X to make it centered
        float offsetY = rectSize.y / 2;             //offsets the Y to make it centered
        Vector3 cubePos = new Vector3(offsetX, offsetY, 0);
        Gizmos.DrawWireCube(areaCenter + (cubePos * rectMagnitude), rectSize * rectMagnitude);
        Gizmos.color = Color.red;
    }

    Vector3 OnUnitRect(float x, float y)            //Vector3 function to help get random point in the Rect
    {
        float newX = Random.Range(0, x);            //Gets a new x, random from 0 to max x point
        float newY = Random.Range(0, y);            //Gets a new y, random from 0 to max y point
        return new Vector3(newX, newY,0 );          //returns new values
    }

    #region deprecatedCode

    //void GenerateMaze()
    //{
    //    //Method 3 spawn all first, then check if there's collision. If there is, reposition and check again.
    //    while (RoomsSpawned.Count < roomNum)                                                                        //Keeps the loop running until it reaches the number of rooms you want
    //    {

    //        GameObject RoomClone = Instantiate(Rooms[Random.Range(0, Rooms.Length)]) as GameObject;                 //spawns room
    //        //to fix rooms spawning out of the map, spawn them in MAPSIZE - ROOMSIZE!!!
    //        RoomClone.transform.localPosition = areaCenter + OnUnitRect(rectSize.x, rectSize.y) * rectMagnitude;    //chooses random position in the area.
    //        RoomsSpawned.Add(RoomClone);                                                                            //adds that room into the spawned room list
    //        for (short j = 0; j < RoomsSpawned.Count; j++)                                                          //iterates through the list of rooms spawned
    //        {
    //            RoomsSpawned[j].GetComponent<Collider2D>().enabled = true;                                          //enables the collider for checking of bounds
    //            Vector2 _roomCenter = RoomsSpawned[j].GetComponent<Collider2D>().bounds.center;                     //finds Center of the box
    //                                                                                                                /*                
    //                                                                                                                                ================================================================================================================================================
    //                                                                                                                                Extents are the half length of the box. So taking the middle -the half length should give you the minX.Adding Center +Extents of Y gives MinY. 
    //                                                                                                                                Using this logic, we find the top left and bottom right of the boxes
    //                                                                                                                                ================================================================================================================================================
    //                                                                                                                */
    //            Vector2 _roomTopLeft = new Vector2(_roomCenter.x - RoomsSpawned[j].GetComponent<Collider2D>().bounds.extents.x, _roomCenter.y + RoomsSpawned[j].GetComponent<Collider2D>().bounds.extents.y);
    //            Vector2 _roomBottomRight = new Vector2(_roomCenter.x + RoomsSpawned[j].GetComponent<Collider2D>().bounds.extents.x, _roomCenter.y - RoomsSpawned[j].GetComponent<Collider2D>().bounds.extents.y);

    //            RoomsSpawned[j].GetComponent<Collider2D>().enabled = false;                                         //disables collider of self to prevent self detection lol
    //            Collider2D Result = Physics2D.OverlapArea(_roomTopLeft, _roomBottomRight);                          //checks if there's another collider at where current room is
    //            if (Result != null)                                                                                 //if there's a room
    //            {
    //                Result.gameObject.SetActive(false);                                                             //hide the room
    //                RoomsSpawned.Remove(Result.gameObject);                                                         //remove the room from the list
    //                //I might have wanted to reposition it instead lol.
    //                //Result.transform.position = areaCenter + OnUnitRect(rectSize.x, rectSize.y) * rectMagnitude;
    //            }
    //            if (RoomsSpawned[j] != null)                                                                        //catches null exception error where the very room itself that spawned is hidden.
    //            {
    //                RoomsSpawned[j].GetComponent<Collider2D>().enabled = true;
    //            }
    //        }
    //        numOfTries++;
    //        if (numOfTries > 100) //catches infinite loops and breaks you out of them
    //        {
    //            break;
    //        }
    //    }
    //}


    //foreach(GameObject _room in RoomsSpawned)
    // {
    //     Vector2 _roomCenter = _room.GetComponent<Collider2D>().bounds.center; //finds Center of the box
    //     //Extents are the half length of the box. So taking the middle - the healf length should give you the minX. Adding Center + Extents of Y gives MinY. 
    //     //Using this logic, we find the top left and bottom right of the boxes.
    //     Vector2 _roomTopLeft = new Vector2(_roomCenter.x - _room.GetComponent<Collider2D>().bounds.extents.x,_roomCenter.y + _room.GetComponent<Collider2D>().bounds.extents.y);
    //     Vector2 _roomBottomRight = new Vector2(_roomCenter.x + _room.GetComponent<Collider2D>().bounds.extents.x,_roomCenter.y -_room.GetComponent<Collider2D>().bounds.extents.y);
    //     //Now to check if they collide with something
    //     if(Physics2D.OverlapArea(_roomTopLeft, _roomBottomRight) == true)
    //     {

    //     }
    // }



    //Method 2 with while loop
    //while (RoomsSpawned.Count < roomNum) //while loop to constantly check
    //{
    //    GameObject RoomClone = Instantiate(Rooms[Random.Range(0, Rooms.Length - 1)]) as GameObject; //spawns room
    //    RoomClone.transform.position = areaCenter + OnUnitRect(rectSize.x, rectSize.y) * rectMagnitude; //chooses random position in the area.
    //    //instantiate the things then make sure they don't over lap. 
    //    if (RoomsSpawned.Count > 0) //makes ure at least 1 room has spawned before repositioning
    //    {
    //        for (short i = 0; i < RoomsSpawned.Count - 1; i++)
    //        {
    //            Debug.Log(Vector3.Distance(RoomClone.transform.position, RoomsSpawned[i].transform.position));
    //            while (Vector3.Distance(RoomClone.transform.position, RoomsSpawned[i].transform.position) < 90)
    //            {
    //                Debug.Log("repos");
    //                RoomClone.transform.position = areaCenter + OnUnitRect(rectSize.x, rectSize.y) * rectMagnitude; //new pos
    //                numOfTries++;
    //                if (numOfTries > 1000)
    //                {
    //                    Destroy(RoomClone);
    //                    break;
    //                }

    //                //maybe add a counter for number of retries.
    //            }
    //        }
    //    }
    //    RoomsSpawned.Add(RoomClone);
    //}


    //Method 1 with for loops
    //for(short i = 0; i < roomNum; i++) //spawns the number of rooms stated
    //{
    //    GameObject RoomClone = Instantiate(Rooms[Random.Range(0, Rooms.Length - 1)]) as GameObject; //spawns room
    //    RoomClone.transform.position = areaCenter + OnUnitRect(rectSize.x,rectSize.y) * rectMagnitude; //chooses random position in the area.
    //    //Maybe rotate it in the future by 90 degree angles

    //    //Note: What if there's a check to make sure it always spawns 5 rooms (since it deletes those that overlap it)
    //    if (RoomsSpawned.Count > 0) //if there's at least 1 room spawned
    //    {
    //        for (short j = 0; j < RoomsSpawned.Count; j++) //iterates through the rooms spawned
    //        {
    //            if(Vector3.Distance(RoomClone.transform.position,RoomsSpawned[j].transform.position) < 5)
    //            {
    //                RoomClone.transform.position = areaCenter + OnUnitRect(rectSize.x, rectSize.y) * rectMagnitude; //chooses random position in the area.
    //                Debug.Log("repositioned");
    //            }
    //        }
    //    }
    //    RoomsSpawned.Add(RoomClone);
    //}
    #endregion
}
