using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileData : MonoBehaviour {

    public int posX;
    public int posY;

    // public int[,] tile_positions;
   

    public enum Type
    {
        BLANK,
        PASSAGE,
        ROOM,
    };
    public Type tileType;

    //Constructor??
    //public TileData(int posX,int posY)
    //{
    //    this.posX = posX;
    //    this.posY = posY;

    //    tile_positions = new int[posX, posY];


    //}

    //public Vector2 GetTileAt(int _index) //gets whichever tile is at said index.
    //{
    //    return tilePositions[_index];
    //}
    public void SetPos(int x, int y) //receives position data
    {
        posX = x;
        posY = y;
       // tilePositions.Add(tempVect);
    }
    public Vector2 GetPos() //returns it when I need it
    {
        Vector2 tilePos = new Vector2(posX, posY);
        return tilePos;
    }
	
}


