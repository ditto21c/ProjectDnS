using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Tile : UnityEngine.Object
{
    public int Index;
    public float PosX = 0;
    public float PosY = 0;
    public bool bBlock = false;

    public float Fitness = 0;
    public float Goal = 0;
    public float Heuristic = 0;
    public int ParentTile = 0;
    public ArrayList OpenList = new ArrayList();

    public void Reset()
    {
        Fitness = 0;
        Goal = 0;
        Heuristic = 0;
        ParentTile = 0;
        OpenList.Clear();
    }
};

public class CBaseAStar : UnityEngine.Object
{
    public void InitTiles(ref bool[] InTiles)
    {
        bTiles = InTiles;
        TileSize = (int)Mathf.Sqrt((float)InTiles.Length);

        for (int y = 0; y < TileSize; ++y)
        {
            for (int x = 0; x < TileSize; ++x)
            {
                Tile tile = new Tile();
                tile.Index = y * TileSize + x;
                tile.PosX = (float)x;
                tile.PosY = (float)y;
                tile.bBlock = InTiles[tile.Index];
                Tiles.Add(tile);
            }
        }
    }

    public virtual ArrayList FindPaths(int StartPos, int InEndPos, int InID)
    {
        return new ArrayList();
    }
    public virtual async Task<ArrayList> AsyncFindPaths(int StartPos, int InEndPos, int InID)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return new ArrayList();
    }

    public ArrayList GetTiles() { return Tiles; }

    public bool IsValidPosIndex(int CheckPosIndex)
    {
        return 0 <= CheckPosIndex && CheckPosIndex < TileSize * TileSize;
    }

    public bool IsValidVec(ref Vector3 CheckVec)
    {
        return 0 < CheckVec.x && CheckVec.x < TileSize && 0 < CheckVec.y && CheckVec.y < TileSize;
    }

    protected ArrayList Tiles = new ArrayList();
    protected ArrayList OpenList = new ArrayList();
    protected ArrayList ClosedList = new ArrayList();
    protected int TileSize;
    protected int EndPos;
    protected bool[] bTiles;
}