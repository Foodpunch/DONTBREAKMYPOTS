using UnityEngine;
using System.Collections.Generic;

public class DTileMap {

    protected class DRoom
    {
        public int left;
        public int top;
        public int width;
        public int height;

        public bool isConnected = false;

        public int center_x
        {
            get { return left + width / 2; }
        }
        public int center_y
        {
            get { return top + height / 2; }
        }

        public int right
        {
            get { return left + width - 1; } 
        }
        public int bottom
        {
           get { return top + height - 1; }
        }

        public bool CollidesWith(DRoom other)
        {
            if (left > other.right)
            {
                return false;
            }
            if (top > other.bottom)
            {
                return false;
            }
            if(right < other.left)
            {
                return false;
            }
            if(bottom < other.top)
            {
                return false;
            }
            return true;
        }

    }

    int size_x;
    int size_y;

    int[,] map_data; //maybe can use enums for this
    List<DRoom> rooms;

    //0 is floor
    //1 is wood
    //2 is black
    //3 is wall
    //4 is bottom left corner
    //5 is top right corner



    //public DTileMap()
    //{
    //    DTileMap(20, 20);
    //}


    public DTileMap(int sizeX, int sizeY)
    {
        DRoom r;
        this.size_x = sizeX;
        this.size_y = sizeY;

        map_data = new int[size_x,size_y];
        rooms = new List<DRoom>();

        int maxFails = 100;

      
        while(rooms.Count < 10)
        {
            int rsx = Random.Range(4, 8);
            int rsy = Random.Range(4, 8);
            r = new DRoom();
            r.left = Random.Range(0, size_x - rsx);
            r.top = Random.Range(0, size_y - rsy);
            r.width = rsx;
            r.height = rsy;

            if (!RoomCollides(r))
            {
                rooms.Add(r);
            }
            else
            {
                maxFails--;
                if (maxFails <= 0)
                {
                    break;
                }
            }
        }
        foreach (DRoom r2 in rooms)
        {
            CreateRoom(r2);
        }
        for (int i = 0; i < rooms.Count; i++)
        {
            if (!rooms[i].isConnected)
            {
                int j = Random.Range(1, rooms.Count);
                CreateCorridor(rooms[i], rooms[(i + j) % rooms.Count]);
            }
        }


      //  CreateWalls();

    }

    public int GetTileAt(int x, int y)
    {
        return map_data[x, y];
    }

    bool RoomCollides(DRoom r)
    {
        foreach(DRoom r2 in rooms)
        {
            if(r.CollidesWith(r2))
            {
                return true;
            }
        }
        return false;
    }

    void CreateRoom(DRoom r)
    {
        for(int x=0; x<r.width; x++)
        {
            for(int y=0;y<r.height;y++)
            {
                if(x== 0 || x == r.width -1 || y == 0 || y == r.height -1)
                {
                    map_data[r.left + x, r.top + y] = 3;
                }
                else
                {
                    map_data[r.left + x, r.top + y] = 1;
                }
                if(x==0 && y == 0)
                {
                    map_data[r.left + x, r.top + y] = 4;
                }
                if(x == r.width-1 && y == r.height-1)
                {
                    map_data[r.left + x, r.top + y] = 5;
                }

                
            }
        }
        
    }

    void CreateCorridor(DRoom r1,DRoom r2)
    {
        int x = r1.center_x;
        int y = r1.center_y;

        while(x !=r2.center_x)
        {
            map_data[x, y] = 1;
            //can do a check here to make adjacent tiles to the corridor become wall tiles
            //but a little complicated because you need to check for quite a bit
            x += x < r2.center_x ? 1 : -1;
        }
        while(y != r2.center_y)
        {
            map_data[x, y] = 1;
            y += y < r2.center_y ? 1 : -1;
        }
        r1.isConnected = true;
        r2.isConnected = true;
    }

    void CreateWalls()
    {
        for(int x=0; x< size_x;x++)
        {
            for(int y=0;y<size_y;y++)
            {
                if(map_data[x,y] == 0 && hasAdjacentFloor(x, y))
                {
                    map_data[x, y] = 3;
                }
            }
        }
    }

    bool hasAdjacentFloor(int x, int y)
    {
        if (x > 0 && map_data[x-1, y] == 1)
            return true;
        if (x < size_x - 1 && map_data[x + 1, y] == 1)
            return true;
        if (y > 0 && map_data[x, y-1] == 1)
            return true;
        if (y < size_y - 1 && map_data[x, y+1] == 1)
            return true;
        if (x > 0 && y > 0 && map_data[x - 1, y - 1] == 1)
            return true;
        if (x < size_x-1 && y > 0 && map_data[x + 1, y - 1] == 1)
            return true;
        if (x > 0 && y <size_y-1 && map_data[x - 1, y + 1] == 1)
            return true;
        if (x < size_x - 1 && y < size_y-1 && map_data[x + 1, y + 1] == 1)
            return true;

        return false;
    }
}
