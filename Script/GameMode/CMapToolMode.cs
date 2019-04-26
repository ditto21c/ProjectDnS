using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class CMapToolMode : CBaseGameMode
{
    private void Awake()
    {
        CDebugLog.Log(ELogType.GameMode, "MapToolMode Awake");
        Map.FileName = FileName;
        Map.Size = Size;
        Map.BlockRate = BlockRate;
        
    }
    // Start is called before the first frame update
    private void Start()
    {
        CDebugLog.Log(ELogType.GameMode, "MapToolMode Start");

        CMapGenerator.bTool = true;
        Map.bShowAllTile = true;

        Button FindButton = GameObject.Find("ReCreateTileBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnReCreateTile());

        FindButton = GameObject.Find("UpdateTileBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnUpdateTile());

        FindButton = GameObject.Find("ChangeRandomSeedBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnChangeRandomSeed());

        FindButton = GameObject.Find("SaveBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnSave());

        FindButton = GameObject.Find("LoadBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnLoad());

        FindButton = GameObject.Find("RefreshMapObjectBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnRefreshMapObject());

        FindButton = GameObject.Find("MakeWayPointsBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnMakeWayPoints());

        FindButton = GameObject.Find("SaveWayPointsBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnSaveWayPoints());

        FindButton = GameObject.Find("LoadWayPointsBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnLoadWayPoints());

        FindButton = GameObject.Find("BuildWayPointPathsBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnBuildWayPointPaths());

        FindButton = GameObject.Find("SaveWayPointPathsBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnSaveWayPointPaths());

        FindButton = GameObject.Find("LoadWayPointPathsBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnLoadWayPointPaths());

        FindButton = GameObject.Find("FindPathFromWayPointPathsBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnFindPathFromWayPointPaths());

        FindButton = GameObject.Find("FindPathFromNoWayPointPathsBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnFindPathFromNoWayPointPaths());

        FindButton = GameObject.Find("AllLoadBtn").GetComponent<Button>();
        FindButton.onClick.AddListener(() => OnAllLoad());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnReCreateTile()
    {
        Map.FileName = FileName;
        Map.Size = Size;
        Map.BlockRate = BlockRate;
        Map.BlockImage = BlockImage;
        Map.TileImage = TileImage;
        Map.OnRefreshMapObject();
        Map.OnReCreateTile();
    }
    public void OnUpdateTile()
    {
        Map.BlockRate = BlockRate;
        Map.OnUpdateTile();
    }
    public void OnChangeRandomSeed()
    {
        Map.OnChangeRandomSeed();
    }
    public void OnSave()
    {
        Map.FileName = FileName;
        Map.OnSave();
    }
    public void OnLoad()
    {
        Map.FileName = FileName;
        Map.OnRefreshMapObject();
        Map.OnLoad();
    }
    public void OnRefreshMapObject()
    {
        Map.OnRefreshMapObject();
    }
    public void OnMakeWayPoints()
    {
        Map.OnMakeWayPoints();
    }
    public void OnSaveWayPoints()
    {
        Map.FileName = FileName;
        Map.OnSaveWayPoints();
    }
    public void OnLoadWayPoints()
    {
        Map.FileName = FileName;
        Map.OnLoadWayPoints();
    }
    public void OnBuildWayPointPaths()
    {
        Map.FileName = FileName;
        Map.OnBuildWayPointPaths();
    }
    public void OnSaveWayPointPaths()
    {
        Map.FileName = FileName;
        Map.OnSaveWayPointPaths();
    }
    public void OnLoadWayPointPaths()
    {
        Map.FileName = FileName;
        Map.OnLoadWayPointPaths();
    }
    public void OnFindPathFromWayPointPaths()
    {
        GameObject StartPosObj = GameObject.Find("StartPos");
        Vector3 StartObjPos = StartPosObj.transform.position;
        StartObjPos[2] = 0.0f;
        ArrayList Paths = Map.OnFindPathFromWayPointPaths();
        Tile FirstPathTile = Paths[0] as Tile;
        Vector3 FirstPathTilePos = new Vector3(FirstPathTile.PosX, FirstPathTile.PosY, 0.0f);

        Debug.DrawLine(StartObjPos, FirstPathTilePos, Color.white, 5.0f);

        for (int i=0; i< Paths.Count; ++i)
        {
            if(i + 1 < Paths.Count)
            {
                Tile StartTile = Paths[i] as Tile;
                Tile EndTile = Paths[i + 1] as Tile;
                Vector3 StartPos = new Vector3(StartTile.PosX, StartTile.PosY, 0.0f);
                Vector3 EndPos = new Vector3(EndTile.PosX, EndTile.PosY, 0.0f);

                Debug.DrawLine(StartPos, EndPos, Color.white, 5.0f);
            }
        }
    }
    public void OnFindPathFromNoWayPointPaths()
    {
        GameObject StartPosObj = GameObject.Find("StartPos");
        Vector3 StartObjPos = StartPosObj.transform.position;
        StartObjPos[2] = 0.0f;
        ArrayList Paths = Map.OnFindPathFromNoWayPointPaths();
        Tile FirstPathTile = Paths[0] as Tile;
        Vector3 FirstPathTilePos = new Vector3(FirstPathTile.PosX, FirstPathTile.PosY, 0.0f);

        Debug.DrawLine(StartObjPos, FirstPathTilePos, Color.white, 5.0f);

        for (int i = 0; i < Paths.Count; ++i)
        {
            if (i + 1 < Paths.Count)
            {
                Tile StartTile = Paths[i] as Tile;
                Tile EndTile = Paths[i + 1] as Tile;
                Vector3 StartPos = new Vector3(StartTile.PosX, StartTile.PosY, 0.0f);
                Vector3 EndPos = new Vector3(EndTile.PosX, EndTile.PosY, 0.0f);

                Debug.DrawLine(StartPos, EndPos, Color.white, 5.0f);
            }
        }
    }



    public void OnAllLoad()
    {
        Map.FileName = FileName;
        Map.Size = Size;
        Map.BlockRate = BlockRate;
        Map.BlockImage = BlockImage;
        Map.TileImage = TileImage;
        Map.OnAllLoad();
    }

    public int Size;
    public float BlockRate;
    public string BlockImage = "BlockImage00";
    public string TileImage = "TileImage00";

}
