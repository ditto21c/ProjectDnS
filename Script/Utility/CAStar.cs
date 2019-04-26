using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CAStar : CBaseAStar
{
    public override ArrayList FindPaths(int StartPos, int InEndPos, int InID)
    {
        ArrayList Paths = new ArrayList();

        if (!IsValidPosIndex(StartPos) || !IsValidPosIndex(InEndPos))
            return Paths;

        CTimeCheck.Start(InID);
        EndPos = InEndPos;

        ClosedList.Clear();
        OpenList.Clear();
        foreach (Tile tile in Tiles)
        {
            if (tile.bBlock)
                ClosedList.Add(tile.Index);

            tile.Reset();
        }

        ClosedList.Add(StartPos);
        int FindNextPath = StartPos;
        int path_make_start_time = Environment.TickCount;
        while (true)
        {
            //if (100 <= Environment.TickCount - path_make_start_time)
            //    return Paths;

            ArrayList MakedOpenList = MakeOpenList(FindNextPath);

            if (OpenList.Count == 0)
                CDebugLog.Log(ELogType.AStar, "OpenList.Count == 0");

            //FindNextPath = FindNextPathIndex(ref MakedOpenList);
            ////FindNextPath = FindNextPathIndexFromOpenList();

            //if (-1 == FindNextPath)
            //{
            FindNextPath = FindNextPathIndexFromOpenList();
            //}

            if (-1 == FindNextPath)
            {
                CTimeCheck.End(ELogType.AStar, "Failed FindPaths");
                return Paths;
            }

            if (FindNextPath == EndPos)
                break;

            OpenList.Remove(FindNextPath);
            ClosedList.Add(FindNextPath);
        }

        int path_index = EndPos;
        Paths.Add(EndPos);
        while (true)
        {
            if (path_index == StartPos)
            {
                break;
            }

            Paths.Add(((Tile)Tiles[path_index]).ParentTile);
            path_index = ((Tile)Tiles[path_index]).ParentTile;
        }
        Paths.Reverse();


        CTimeCheck.End(ELogType.AStar, "Success FindPaths");
        return Paths;
    }
    public override async Task<ArrayList> AsyncFindPaths(int StartPos, int InEndPos, int InID)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return FindPaths(StartPos, InEndPos, InID); 
    }

    float CalcuGoal(int StandardPos, int OpenPos)
    {
        Tile standard_tile = (Tile)Tiles[StandardPos];
        Tile open_tile = (Tile)Tiles[OpenPos];
        float X = Mathf.Abs(standard_tile.PosX - open_tile.PosX);
        float Y = Mathf.Abs(standard_tile.PosY - open_tile.PosY);

        float Dist = Mathf.Sqrt(X * X + Y * Y);
        return standard_tile.Fitness + Dist;
    }

    float CalcuHeuristic(int OpenPos, int EndPos)
    {
        Tile end_tile = (Tile)Tiles[EndPos];
        Tile open_tile = (Tile)Tiles[OpenPos];

        return Mathf.Abs(end_tile.PosX - open_tile.PosX) + Mathf.Abs(end_tile.PosY - open_tile.PosY);
    }

    bool PushOpenList(int StandardPos, int OpenPos)
    {
        if (!ClosedList.Contains(OpenPos))
        {
            if (!OpenList.Contains(OpenPos))
            {
                OpenList.Add(OpenPos);

                ((Tile)Tiles[OpenPos]).Goal = CalcuGoal(StandardPos, OpenPos);
                ((Tile)Tiles[OpenPos]).Heuristic = CalcuHeuristic(OpenPos, EndPos);
                ((Tile)Tiles[OpenPos]).Fitness = ((Tile)Tiles[OpenPos]).Goal + ((Tile)Tiles[OpenPos]).Heuristic;
                ((Tile)Tiles[OpenPos]).ParentTile = StandardPos;
                return true;
            }
        }
        return false;
    }

    ArrayList FindNearOpenList(int StandardPos)
    {
        ArrayList FindedNearOpenList = new ArrayList();

        if (IsValidPosIndex(StandardPos))
        {
            // bottom left ~
            for (int i = 0; i < 3; ++i)
            {
                int OpenPos = StandardPos - (TileSize + 1) + i;
                if (IsValidPosIndex(OpenPos))
                {
                    float XGap = Mathf.Abs(((Tile)Tiles[StandardPos]).PosX - ((Tile)Tiles[OpenPos]).PosX);
                    float YGap = Mathf.Abs(((Tile)Tiles[StandardPos]).PosY - ((Tile)Tiles[OpenPos]).PosY);
                    if (XGap < 2.0f && YGap < 2.0f)
                    {
                        if (0 == i)
                        {
                            if (((Tile)Tiles[OpenPos + 1]).bBlock || ((Tile)Tiles[OpenPos + TileSize]).bBlock )
                                continue;
                        }
                        else if (2 == i)
                        {
                            if (((Tile)Tiles[OpenPos - 1]).bBlock || ((Tile)Tiles[OpenPos + TileSize]).bBlock )
                                continue;
                        }
                        FindedNearOpenList.Add(OpenPos);
                    }
                }
            }

            if (0 <= StandardPos - 1 && StandardPos - 1 < TileSize * TileSize)
            {
                int OpenPos = StandardPos - 1;
                float XGap = Mathf.Abs(((Tile)Tiles[StandardPos]).PosX - ((Tile)Tiles[OpenPos]).PosX);
                float YGap = Mathf.Abs(((Tile)Tiles[StandardPos]).PosY - ((Tile)Tiles[OpenPos]).PosY);
                if (XGap < 2.0f && YGap < 2.0f)
                {
                    FindedNearOpenList.Add(OpenPos);
                }
            }

            if (0 <= StandardPos + 1 && StandardPos + 1 < TileSize * TileSize)
            {
                int OpenPos = StandardPos + 1;
                float XGap = Mathf.Abs(((Tile)Tiles[StandardPos]).PosX - ((Tile)Tiles[OpenPos]).PosX);
                float YGap = Mathf.Abs(((Tile)Tiles[StandardPos]).PosY - ((Tile)Tiles[OpenPos]).PosY);
                if (XGap < 2.0f && YGap < 2.0f)
                {
                    FindedNearOpenList.Add(OpenPos);
                }
            }

            for (int i = 0; i < 3; ++i)
            {
                int OpenPos = StandardPos + (TileSize - 1) + i;
                if (IsValidPosIndex(OpenPos))
                {
                    float XGap = Mathf.Abs(((Tile)Tiles[StandardPos]).PosX - ((Tile)Tiles[OpenPos]).PosX);
                    float YGap = Mathf.Abs(((Tile)Tiles[StandardPos]).PosY - ((Tile)Tiles[OpenPos]).PosY);
                    if (XGap < 2.0f && YGap < 2.0f)
                    {
                        if (0 == i)
                        {
                            if (((Tile)Tiles[OpenPos + 1]).bBlock || ((Tile)Tiles[OpenPos - TileSize]).bBlock )
                                continue;
                        }
                        else if (2 == i)
                        {
                            if (((Tile)Tiles[OpenPos - 1]).bBlock || ((Tile)Tiles[OpenPos - TileSize]).bBlock )
                                continue;
                        }

                        FindedNearOpenList.Add(OpenPos);
                    }
                }
            }
        }
        return FindedNearOpenList;

    }

    ArrayList MakeOpenList(int StandardPos)
    {
        ArrayList MakedOpenList = new ArrayList();

        ArrayList FindedNearOpenList = FindNearOpenList(StandardPos);
        foreach (int OpenPos in FindedNearOpenList)
        {
            if (PushOpenList(StandardPos, OpenPos))
                MakedOpenList.Add(OpenPos);
        }

        return MakedOpenList;
    }

    int FindNextPathIndexFromOpenList()
    {
        int FindPath = -1;
        float FindFitness = 999999999.0f;
        foreach (int CurIndex in OpenList)
        {
            if (false == ClosedList.Contains(CurIndex))
            {
                if (((Tile)Tiles[CurIndex]).Fitness < FindFitness)
                {
                    FindPath = CurIndex;
                    FindFitness = ((Tile)Tiles[CurIndex]).Fitness;
                }
            }
        }
        return FindPath;
    }

    int FindNextPathIndex(ref ArrayList CheckOpenList)
    {
        int FindPath = -1;
        float FindFitness = 999999999.0f;
        foreach (int CurIndex in CheckOpenList)
        {
            if (false == ClosedList.Contains(CurIndex))
            {
                if (((Tile)Tiles[CurIndex]).Fitness < FindFitness)
                {
                    FindPath = CurIndex;
                    FindFitness = ((Tile)Tiles[CurIndex]).Fitness;
                }
            }
        }

        return FindPath;
    }
    
}
