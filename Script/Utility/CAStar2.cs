using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CAStar2 : CBaseAStar
{
    public override async Task<ArrayList> AsyncFindPaths(int StartPos, int EndPos, int InID)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return FindPaths(StartPos, EndPos, InID);
    }

    public override ArrayList FindPaths(int StartPos, int EndPos, int InID)
    {
        ArrayList Paths = new ArrayList();

        if (!IsValidPosIndex(StartPos) || !IsValidPosIndex(EndPos))
            return Paths;

        CTimeCheck.Start(InID);

        Tile StartTile = (Tile)Tiles[StartPos];
        Tile EndTile = (Tile)Tiles[EndPos];

        if (StartTile.bBlock || EndTile.bBlock)
            return Paths;

        bool bFindedBlock = StartAndEndPathBlockCheck(StartPos, EndPos);
        if(false == bFindedBlock)
        {
            Paths.Add(EndPos);
            CTimeCheck.End(ELogType.AStar, "Success FindPaths");
            return Paths;
        }

        Vector2 StartVec = new Vector2(StartTile.PosX, StartTile.PosY);
        Vector2 EndVec = new Vector2(EndTile.PosX, EndTile.PosY);

        int CloseWayPointIdxStart = -1;
        int CloseWayPointIdxEnd = -1;

        float StartLenth = 99999.0f;
        float StartHeuristicLenth = 99999.0f;
        float EndLenth = 99999.0f;
        float EndHeuristicLenth = 99999.0f;
        
        for (int i=0; i< WayPoints.Count; ++i)
        {
            Tile WayPointTile = (Tile)Tiles[(int)WayPoints[i]];
            Vector2 WayPointPos = new Vector2(WayPointTile.PosX, WayPointTile.PosY);
            float CurStartLenth = Vector2.Distance(StartVec, WayPointPos);
            float CurEndLenth = Vector2.Distance(EndVec, WayPointPos);
            float CurHeristicLenth = CurStartLenth + CurEndLenth;
            if (CurStartLenth < StartLenth || (CurStartLenth == StartLenth && CurHeristicLenth < StartHeuristicLenth)) 
            {
                CloseWayPointIdxStart = WayPointTile.Index;
                StartLenth = CurStartLenth;
                StartHeuristicLenth = CurHeristicLenth;
            }
            if(CurEndLenth < EndLenth || (CurEndLenth == EndLenth && CurHeristicLenth < EndHeuristicLenth))
            {
                CloseWayPointIdxEnd = WayPointTile.Index;
                EndLenth = CurEndLenth;
                EndHeuristicLenth = CurHeristicLenth;
            }
        }
        if(CloseWayPointIdxStart == -1)
        {
            Paths.Add(EndPos);
        }
        else if(CloseWayPointIdxStart == CloseWayPointIdxEnd)
        {
            Paths.Add(CloseWayPointIdxStart);
            Paths.Add(EndPos);
        }
        else
        {
            int Key = CloseWayPointIdxStart < CloseWayPointIdxEnd ? CloseWayPointIdxStart * 10000 + CloseWayPointIdxEnd : CloseWayPointIdxEnd * 10000 + CloseWayPointIdxStart;
            ArrayList FindPaths;
            if (WayPointPaths.TryGetValue(Key, out FindPaths))
            {
                Paths = (ArrayList)FindPaths.Clone();
                if ((int)Paths[0] == CloseWayPointIdxStart)
                {
                    Paths.Insert(Paths.Count, EndPos);
                }
                else
                {
                    Paths.Reverse();
                    Paths.Insert(Paths.Count, EndPos);
                }
            }
            else
            {
                return Paths;
            }
        }
       
        CTimeCheck.End(ELogType.AStar, "Success FindPaths");

        return Paths;
    }

    bool StartAndEndPathBlockCheck(int StartPosIdx, int EndPosIdx)
    {
        Tile StartTile = (Tile)Tiles[StartPosIdx];
        Tile EndTile = (Tile)Tiles[EndPosIdx];

        float Up = EndTile.PosY - StartTile.PosY;
        float Right = EndTile.PosX - StartTile.PosX;

        int MoveX = 0 != Right ? (0 < Right ? 1 : -1) : (0);
        int MoveY = 0 != Up ? (0 < Up ? 1 : -1) : (0);

        int xCount = (int)Mathf.Abs(Right);
        int yCount = (int)Mathf.Abs(Up);

        for(int y=0; y<=yCount; ++y)
        {
            for(int x=0; x<=xCount; ++x)
            {
                Tile CurTile = (Tile)Tiles[StartPosIdx + TileSize * y * MoveY + x*MoveX];
                if(CurTile.bBlock)
                {
                    float Dist = Vector2.Distance(new Vector2(StartTile.PosX, StartTile.PosY), new Vector2(EndTile.PosX, EndTile.PosY));
                    float ADist = Vector2.Distance(new Vector2(StartTile.PosX, StartTile.PosY), new Vector2(CurTile.PosX, CurTile.PosY));
                    float BDist = Vector2.Distance(new Vector2(EndTile.PosX, EndTile.PosY), new Vector2(CurTile.PosX, CurTile.PosY));
                    if(Dist*Dist + 0.25f >  ADist * ADist + BDist*BDist) // 충돌
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    
    public ArrayList WayPoints = new ArrayList();
    public Dictionary<int, ArrayList> WayPointPaths = new Dictionary<int, ArrayList>();

}
