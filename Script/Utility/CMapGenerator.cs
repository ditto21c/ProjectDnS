using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class CMapGenerator : UnityEngine.Object
{
    // Use this for initialization
    void Start()
    {
        TileFullSize = Size * Size;
        CreateTile();
        ShowTile();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnUpdateTile()
    {
        UpdateTile();
        ShowTile();
    }

    public void OnReCreateTile()
    {
        RemoveAllMapObject();
        TileFullSize = Size * Size;
        CreateTile();
        ShowTile();
    }

    public void OnChangeRandomSeed()
    {
        UnityEngine.Random.InitState(Environment.TickCount);
    }

    private void UpdateTilesFromAddImage()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("BlockImage");

        for (int i = 0; i < Tiles.Length; ++i)
            Tiles[i] = false;

        foreach (GameObject block in blocks)
        {
            int x = Mathf.RoundToInt(block.transform.position.x);
            int y = Mathf.RoundToInt(block.transform.position.y);
            Tiles[Size * y + x] = true;
        }
    }

    public void OnSave()
    {
        CTimeCheck.Start();

        UpdateTilesFromAddImage();

        int Count = Tiles.Length / 64;
        int Remainder = Tiles.Length % 64;
        if (0 < Remainder)
            ++Count;

        UInt64[] SaveData = new UInt64[Count];
        int SaveDataIndex = 0;
        for (int i = 0; i < Tiles.Length; ++i)
        {
            SaveDataIndex = i / 64;
            if (Tiles[i])
                SaveData[SaveDataIndex] |= 0x01;

            if ((i + 1) % 64 != 0 && i != Tiles.Length - 1)
                SaveData[SaveDataIndex] = SaveData[SaveDataIndex] << 1;

        }
        if (0 < Remainder)
            SaveData[SaveDataIndex] = SaveData[SaveDataIndex] << (64 - Remainder);

        string Path = GetPath(ref FileName);
        FileStream fs = new FileStream(Path, FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);
        bw.Write((UInt16)Count);        // size 넘어 갈수 있으니 조심
        bw.Write((UInt16)Remainder);
        for (int i = 0; i < Count; ++i)
            bw.Write(SaveData[i]);

        CTimeCheck.End(ELogType.MapGenerator, "map generator save");

        SaveMapObject(ref bw);

        bw.Write(BlockImage);
        bw.Write(TileImage);

        bw.Close();
        fs.Close();
    }

    public void OnSaveWayPointPaths()
    {
        CTimeCheck.Start();

        string FileFullName = FileName + "_WPP";
        string Path = GetPath(ref FileName);
        FileStream fs = new FileStream(Path, FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);

        bw.Write(WayPointPaths.Count);
        foreach(var kv in WayPointPaths)
        {
            bw.Write(kv.Key);
            bw.Write(kv.Value.Count);

            for(int i=0; i< kv.Value.Count; ++i)
            {
                bw.Write((int)kv.Value[i]);
            }
        }

        bw.Close();
        fs.Close();
       
        CTimeCheck.End(ELogType.MapGenerator, "OnSaveWayPointPaths");
    }
    public void OnLoadWayPointPaths()
    {
        CTimeCheck.Start();

        FileStream fileStream = null;
        BinaryReader binaryReader = null;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            CTextAssetMgr TextAssetMgr = GameObject.FindGameObjectWithTag("TextAssetMgr").GetComponent<CTextAssetMgr>();
            binaryReader = TextAssetMgr.LoadTextAsset(CTextAssetMgr.EFileType.MapWPP, FileName);
            if (binaryReader == null)
                return;
        }
        else
        {
            string FileFullPath = FileName + "_WPP";
            string Path = GetPath(ref FileFullPath);
            if (false == File.Exists(Path))
                return;
            fileStream = new FileStream(Path, FileMode.Open);
            binaryReader = new BinaryReader(fileStream);
        }
        WayPointPaths.Clear();
        int Count = binaryReader.ReadInt32();
        for (int i = 0; i < Count; ++i)
        {
            int Key = binaryReader.ReadInt32();
            int ValueCount = binaryReader.ReadInt32();
            ArrayList paths = new ArrayList();
            for (int j = 0; j < ValueCount; ++j)
            {
                paths.Add(binaryReader.ReadInt32());
            }
            WayPointPaths.Add(Key, paths);
        }
        binaryReader.Close();
        if (fileStream != null)
            fileStream.Close();
        CTimeCheck.End(ELogType.MapGenerator, "OnLoadWayPointPaths");
    }

    public void OnSaveWayPoints()
    {
        CTimeCheck.Start();

        string FileFullName = FileName + "_WP";
        string Path = GetPath(ref FileFullName); 
        FileStream fs = new FileStream(Path, FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);

        bw.Write(WayPoints.Count);
        for (int i = 0; i < WayPoints.Count; ++i)
        {
            bw.Write((int)WayPoints[i]);
        }

        bw.Close();
        fs.Close();

        CTimeCheck.End(ELogType.MapGenerator, "OnSaveWayPoints");
    }
    public void OnLoadWayPoints()
    {
        CTimeCheck.Start();
        FileStream fileStream = null;
        BinaryReader binaryReader = null;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            CTextAssetMgr TextAssetMgr = GameObject.FindGameObjectWithTag("TextAssetMgr").GetComponent<CTextAssetMgr>();
            binaryReader = TextAssetMgr.LoadTextAsset(CTextAssetMgr.EFileType.MapWP, FileName);
            if (binaryReader == null)
                return;
        }
        else
        {
            string FileFullPath = FileName + "_WP";
            string Path = GetPath(ref FileFullPath);
            if (false == File.Exists(Path))
                return;
            fileStream = new FileStream(Path, FileMode.Open);
            binaryReader = new BinaryReader(fileStream);
        }
        WayPoints.Clear();
        int Count = binaryReader.ReadInt32();
        for (int i = 0; i < Count; ++i)
        {
            WayPoints.Add(binaryReader.ReadInt32());
        }
        binaryReader.Close();
        if (fileStream != null)
            fileStream.Close();

        CTimeCheck.End(ELogType.MapGenerator, "OnLoadWayPoints");
    }

    public void OnAllLoad()
    {
        OnLoad();
        OnLoadWayPoints();
        OnLoadWayPointPaths();
    }

    void SaveMapObject(ref BinaryWriter binaryWriter)
    {
        CTimeCheck.Start();
        
        Dictionary<string, CMapObject[]> SortMapObjects = new Dictionary<string, CMapObject[]>();
        CMapObject[] MapObjects = FindObjectsOfType<CMapObject>();
        foreach(var MapObject in MapObjects)
        {
            CMapObject[] FindMapObjects;
            string FixName = MapObject.name;
            int FindIndex = FixName.IndexOf('(');
            if(-1 != FindIndex)
                FixName = FixName.Remove(FindIndex);
            FixName = FixName.Trim();
            if (SortMapObjects.TryGetValue(FixName, out FindMapObjects))
            {
                for(int i=0; i< FindMapObjects.Length; ++i)
                {
                    if(FindMapObjects[i] == null)
                    {
                        FindMapObjects[i] = MapObject;
                        break;
                    }
                }
            }
            else
            {
                CMapObject[] AddMapObjects = new CMapObject[100];
                AddMapObjects[0] = MapObject;
                SortMapObjects.Add(FixName, AddMapObjects);
            }
        }

        binaryWriter.Write(SortMapObjects.Keys.Count);
        byte[] ObjectCountBuffer = new byte[SortMapObjects.Keys.Count];
        int Index = 0;
        foreach (var kv in SortMapObjects)
        {
            CMapObject[] Values = kv.Value;
            for(int i=0; i<Values.Length; ++i)
            {
                if(Values[i] == null)
                {
                    ObjectCountBuffer[Index] = (byte)i;
                    break;
                }
            }
            ++Index;
        }
        binaryWriter.Write(ObjectCountBuffer);
        foreach (var kv in SortMapObjects)
        {
            binaryWriter.Write(kv.Key);
            CMapObject[] Values = kv.Value;
            for (int i = 0; i < Values.Length; ++i)
            {
                if (Values[i] != null)
                {
                    Values[i].Write(ref binaryWriter);
                }
                else
                { break; }
            }
        }
        CTimeCheck.End(ELogType.MapGenerator, "map generator save map object");
    }

    void LoadMapObjects(ref BinaryReader binaryReader)
    {
        int ObjectCount = binaryReader.ReadInt32();
        byte[] ObjectCountBuffer = binaryReader.ReadBytes(ObjectCount);
        for (int j = 0; j < ObjectCount; ++j)
        {
            string ObjectName = binaryReader.ReadString();
            for (int i = 0; i < ObjectCountBuffer[j]; ++i)
            {
                GameObject instance = Instantiate<GameObject>(CResourceMgr.LoadMapObject(ObjectName));
                CMapObject component = instance.GetComponent<CMapObject>();
                component.Read(ref binaryReader);
                component.UpdatePos();
            }
        }
    }

   
    string GetPath(ref string FileName)
    {
        //if(Application.platform == RuntimePlatform.Android)
        //{
        //    string path = Application.persistentDataPath;
        //    path = path.Substring(0, path.LastIndexOf('/'));
        //    return Path.Combine(path, FileName + ".bytes");
        //    //return Application.persistentDataPath + "/" + FileName + ".bytes";
        //}
            
        return Application.dataPath + "/Resources/TextAsset/" + FileName + ".bytes";
    }

    public void OnLoad()
    {
        CTimeCheck.Start();

        BinaryReader binaryReader = null;
        FileStream fileStream = null;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            CTextAssetMgr TextAssetMgr = GameObject.FindGameObjectWithTag("TextAssetMgr").GetComponent<CTextAssetMgr>();
            binaryReader = TextAssetMgr.LoadTextAsset(CTextAssetMgr.EFileType.Map, FileName);
            if (binaryReader == null)
                return;
        }
        else
        { 
            string Path = GetPath(ref FileName);
            fileStream = new FileStream(Path, FileMode.Open);
            binaryReader = new BinaryReader(fileStream);
            
        }

        int Count = (int)binaryReader.ReadUInt16();
        int Remainder = (int)binaryReader.ReadUInt16();

        UInt64[] SaveData = new UInt64[Count];
        for (int i = 0; i < Count; ++i)
            SaveData[i] = binaryReader.ReadUInt64();

        TileFullSize = 0 < Remainder ? (Count - 1) * 64 + Remainder : Count * 64;
        Size = (int)Math.Sqrt(TileFullSize);
        Tiles = new bool[TileFullSize];

        for (int i = 0; i < Count; ++i)
        {
            for (int j = 0; j < 64; ++j)
            {
                if (0 < Remainder && i == Count - 1 && Remainder == j)
                    break;

                if ((UInt64)0 < (SaveData[i] & 0x8000000000000000))
                    Tiles[i * 64 + j] = true;

                SaveData[i] = SaveData[i] << 1;
            }
        }

        LoadMapObjects(ref binaryReader);

        BlockImage = binaryReader.ReadString();
        TileImage = binaryReader.ReadString();

        binaryReader.Close();
        if (fileStream != null)
            fileStream.Close();

        if (bShowAllTile)
            ShowTile();
        else
            CreateTileObject();

        CTimeCheck.End(ELogType.MapGenerator, "OnLoad");
    }

    void CreateTile()
    {
        Tiles = new bool[TileFullSize];

        List<int> TempBlocks = new List<int>();
        int BlockCount = (int)((float)TileFullSize * BlockRate);
        int RanBlock = 0;
        bool bAdd;
        for (int i = 0; i < BlockCount; ++i)
        {
            bAdd = false;
            do
            {
                RanBlock = UnityEngine.Random.Range(0, TileFullSize);
                if (!TempBlocks.Exists(x => x == RanBlock))
                {
                    TempBlocks.Add(RanBlock);
                    bAdd = true;
                }
            } while (!bAdd);
        }
        TempBlocks.ForEach(delegate (int Block)
        {
            Tiles[Block] = true;
        });
    }

    /*
    (i - Size - 1);
    (i - Size);
    (i - Size + 1);

    (i - 1);
    (i);
    (i + 1);

    (i + Size - 1);
    (i + Size);
    (i + Size + 1);
    */
    void UpdateTile()
    {
        int BlockCount;
        int Temp;
        for (int i = 0; i < TileFullSize; ++i)
        {
            BlockCount = 0;
            for (int j = -1; j < 2; ++j)
            {
                for (int k = -1; k < 2; ++k)
                {
                    Temp = i + Size * j + k;
                    if (0 <= Temp && Temp < TileFullSize)
                    {
                        if (Tiles[Temp])
                            ++BlockCount;
                    }
                    else
                    {
                        ++BlockCount;
                    }
                }
            }

            if (4 < BlockCount)
                Tiles[i] = true;
            else
                Tiles[i] = false;
        }
    }

    void ShowTile()
    {
        RemoveAllTile();

        CTimeCheck.Start();

        GameObject block_image = CResourceMgr.LoadMapImage(BlockImage);
        GameObject tile_image = CResourceMgr.LoadMapImage(TileImage);

        GameObject images_parent = Instantiate< GameObject >(CResourceMgr.LoadMapImage("ImagesParent"), new Vector3(0.0f, 0.0f, ImageDefaultPosZ), new Quaternion());
        

        for (int y = 0; y < Size; ++y)
        {
            for (int x = 0; x < Size; ++x)
            {
                if (Tiles[y * Size + x])
                {
                    GameObject NewGameObject = Instantiate<GameObject>(block_image, new Vector3(x, y, ImageDefaultPosZ), new Quaternion()/*, new_empty_resource.transform*/);
                    NewGameObject.isStatic = true;
                    NewGameObject.tag = "BlockImage";
                    NewGameObject.transform.SetParent(images_parent.transform);
                    
                }
                else
                {
                    GameObject NewGameObject = Instantiate<GameObject>(tile_image, new Vector3(x, y, ImageDefaultPosZ), new Quaternion()/*, new_empty_resource.transform*/);
                    NewGameObject.isStatic = true;
                    NewGameObject.tag = "TileImage";
                    NewGameObject.transform.SetParent(images_parent.transform);
                }
            }
        }

        CTimeCheck.End(ELogType.MapGenerator, "show tiles");
    }

    void RemoveAllTile()
    {
        CTimeCheck.Start();
        GameObject[] BlockImagePrefabs = GameObject.FindGameObjectsWithTag("BlockImage");
        foreach(GameObject Block in BlockImagePrefabs)
        {
            Destroy(Block);
        }

        GameObject[] TileImagePrefabs = GameObject.FindGameObjectsWithTag("TileImage");
        foreach (GameObject Tile in TileImagePrefabs)
        {
            Destroy(Tile);
        }
        CTimeCheck.End(ELogType.MapGenerator, "RemoveAllTile");
    }
    void RemoveAllMapObject()
    {
        CMapObject[] MapObjects = FindObjectsOfType<CMapObject>();
        foreach (var obj in MapObjects)
        {
            Destroy(obj.gameObject);
        }
    }

    public bool[] GetTiles() { return Tiles; }
    public ArrayList GetWayPoints() { return WayPoints; }

    void CreateTileObject()
    {
        CTimeCheck.Start();

        GameObject block_image = CResourceMgr.LoadMapImage(BlockImage);
        GameObject tile_image = CResourceMgr.LoadMapImage(TileImage);

        GameObject images_parent = Instantiate<GameObject>(CResourceMgr.LoadMapImage("ImagesParent"), new Vector3(0.0f, 0.0f, ImageDefaultPosZ), new Quaternion());

        for (int i=0; i<336; ++i)
        {
            GameObject new_object = Instantiate<GameObject>(block_image, new Vector3(-1.0f, -1.0f, ImageDefaultPosZ), new Quaternion());
            new_object.isStatic = true;
            new_object.tag = "BlockImage";
            new_object.transform.SetParent(images_parent.transform);
            block_objects.Add(new_object);
        }

        for(int i=0; i< 336; ++i)
        {
            GameObject new_object = Instantiate<GameObject>(tile_image, new Vector3(-1.0f, -1.0f, ImageDefaultPosZ), new Quaternion());
            new_object.isStatic = true;
            new_object.tag = "TileImage";
            new_object.transform.SetParent(images_parent.transform);
            tile_objects.Add(new_object);
        }

        CTimeCheck.End(ELogType.MapGenerator, "create tile object");
    }

    public struct UsedRenderTileInfo
    {
        public bool bRender;
        public GameObject tile;
    }

    
    public void RenderTileObject(Vector3 center_pos)
    {
        if (bShowAllTile) return;

       // CTimeCheck.Start();

        int[] keys = new int[used_tile_objects.Keys.Count];
        used_tile_objects.Keys.CopyTo(keys, 0);
        for(int i=0; i< used_tile_objects.Keys.Count; ++i)
        {
            UsedRenderTileInfo info = used_tile_objects[keys[i]];
            info.bRender = false;
            used_tile_objects[keys[i]] = info;
        }

        // CreateTileObject 14 * 24 == 264
        for (int y=(int)center_pos.y - 6; y<center_pos.y+6; ++y)
        {
            for(int x=(int)center_pos.x - 11; x<center_pos.x + 11; ++x)
            {
                if(0 <= x && x < TileFullSize && 0 <= y && y < TileFullSize)
                {
                    int idx = y * Size + x;
                    bool bBlock = Tiles[idx];

                    UsedRenderTileInfo info;
                    if (used_tile_objects.TryGetValue(idx, out info))
                    {
                        info.bRender = true;

                        if(bBlock && info.tile.tag == "TileImage")
                        {
                            tile_objects.Add(info.tile);
                            info.tile = (GameObject)block_objects[0];
                            info.tile.transform.position = new Vector3(x, y, ImageDefaultPosZ);
                            block_objects.RemoveAt(0);
                        }
                        else if(!bBlock && info.tile.tag == "BlockImage")
                        {
                            block_objects.Add(info.tile);
                            info.tile = (GameObject)tile_objects[0];
                            info.tile.transform.position = new Vector3(x, y, ImageDefaultPosZ);
                            tile_objects.RemoveAt(0);
                        }

                        used_tile_objects[idx] = info;
                    }
                    else
                    {
                        UsedRenderTileInfo new_info = new UsedRenderTileInfo();
                        new_info.bRender = true;
                        if(bBlock)
                        {
                            new_info.tile = (GameObject)block_objects[0];
                            new_info.tile.transform.position = new Vector3(x, y, ImageDefaultPosZ);
                            block_objects.RemoveAt(0);
                            used_tile_objects.Add(idx, new_info);
                        }
                        else
                        {
                            new_info.tile = (GameObject)tile_objects[0];
                            new_info.tile.transform.position = new Vector3(x, y, ImageDefaultPosZ);
                            tile_objects.RemoveAt(0);
                            used_tile_objects.Add(idx, new_info);
                        }

                    }
                }
            }
        }

        int[] remove_keys = new int[used_tile_objects.Keys.Count];
        used_tile_objects.Keys.CopyTo(remove_keys, 0);
        for (int i = 0; i < used_tile_objects.Keys.Count; ++i)
        {
            int remove_key = remove_keys[i];
            if (used_tile_objects[remove_key].bRender == false)
            {
                used_tile_objects[remove_key].tile.transform.position = new Vector3(-1.0f, -1.0f, ImageDefaultPosZ);
                if (used_tile_objects[remove_key].tile.tag == "TileImage")
                    tile_objects.Add(used_tile_objects[remove_key].tile);
                else
                    block_objects.Add(used_tile_objects[remove_key].tile);

                used_tile_objects.Remove(remove_key);
            }
        }

       // CTimeCheck.End(ELogType.MapGenerator, "render tile object");
    }

    public void DestroyTileImage(object value)
    {
        CDebugLog.Log(ELogType.MapGenerator, "DestroyTileImage in MapGenerator");

        //Vector3 pos = (Vector3)value;
        //Tiles[Size * (int)pos.y + (int)pos.x] = false;
    }

    public void DestroyBlockImage(Vector3 pos)
    {
        CDebugLog.Log(ELogType.MapGenerator, "DestroyBlockImage in MapGenerator");

        //GameObject game_object = (GameObject)value;
        //Vector3 pos = game_object.transform.position;
        Tiles[Size * (int)pos.y + (int)pos.x] = false;
    }

    public void OnRefreshMapObject()
    {
        RemoveAllMapObject();


    }

    public void OnMakeWayPoints()
    {
        WayPoints.Clear();
        for (int i = 0; i < TileFullSize; ++i)
        {
            if (Tiles[i])
            {
                int a = i + Size - 1;
                if (0 <= a)
                {
                    if (a < Tiles.Length && Tiles[a] == false &&
                        a + 1 < Tiles.Length && Tiles[a + 1] == false &&
                        a - Size < Tiles.Length && Tiles[a - Size] == false)
                    {
                        if (WayPoints.Contains(a))
                        {
                            string str = "WayPoint Same Index : {0}";
                            object[] arg = {a};
                            CDebugLog.LogFormat(ELogType.MapGenerator, str, arg);
                        }
                        else
                            WayPoints.Add(a);
                    }
                }

                int b = i + Size + 1;
                if (0 <= b)
                {
                    if (b < Tiles.Length && Tiles[b] == false &&
                        b - 1 < Tiles.Length && Tiles[b - 1] == false &&
                        b - Size < Tiles.Length && Tiles[b - Size] == false)
                    {
                        if (WayPoints.Contains(b))
                        {
                            string str = "WayPoint Same Index : {0}";
                            object[] arg = { b };
                            CDebugLog.LogFormat(ELogType.MapGenerator, str, arg);
                        }
                        else
                            WayPoints.Add(b);
                    }
                }

                int c = i - Size - 1;
                if (0 <= c)
                {
                    if (c < Tiles.Length && Tiles[c] == false &&
                        c + 1 < Tiles.Length && Tiles[c + 1] == false &&
                        c + Size < Tiles.Length && Tiles[c + Size] == false)
                    {
                        if (WayPoints.Contains(c))
                        {
                            string str = "WayPoint Same Index : {0}";
                            object[] arg = { c };
                            CDebugLog.LogFormat(ELogType.MapGenerator, str, arg);
                        }
                        else
                            WayPoints.Add(c);
                    }
                }

                int d = i - Size + 1;
                if (0 <= d)
                {
                    if (d < Tiles.Length && Tiles[d] == false && 0 < d - 1 &&
                        d - 1 < Tiles.Length && Tiles[d - 1] == false &&
                        d + Size < Tiles.Length && Tiles[d + Size] == false)
                    {
                        if (WayPoints.Contains(d))
                        {
                            string str = "WayPoint Same Index : {0}";
                            object[] arg = { d };
                            CDebugLog.LogFormat(ELogType.MapGenerator, str, arg);
                        }
                        else
                            WayPoints.Add(d);
                    }
                }
            }
        }
        ShowWayPoints();

    }

    void ShowWayPoints()
    {
        GameObject path_image = CResourceMgr.LoadMapImage("PathImage");
        GameObject images_parent = Instantiate<GameObject>(CResourceMgr.LoadMapImage("ImagesParent"), new Vector3(0.0f, 0.0f, ImageDefaultPosZ), new Quaternion());

        for (int y = 0; y < Size; ++y)
        {
            for (int x = 0; x < Size; ++x)
            {
                if (WayPoints.Contains(y * Size + x))
                {
                    GameObject new_object = Instantiate<GameObject>(path_image, new Vector3(x, y, ImageDefaultPosZ), new Quaternion());
                    new_object.isStatic = true;
                    new_object.tag = "PathImage";
                    new_object.transform.SetParent(images_parent.transform);
                }
            }
        }
    }

    
    public ArrayList OnFindPathFromWayPointPaths()
    {
        GameObject startPos = GameObject.Find("StartPos");
        GameObject endPos = GameObject.Find("EndPos");

        Vector3 startPosVec = CUtility.RoundVector3(startPos.transform.position);
        Vector3 endPosVec = CUtility.RoundVector3(endPos.transform.position);

        CAStar2 AStar = new CAStar2();
        AStar.InitTiles(ref Tiles);
        AStar.WayPoints = WayPoints;
        AStar.WayPointPaths = WayPointPaths;

        int StartIdx = (int)(startPosVec.y * Size + startPosVec.x);
        int EndIdx = (int)(endPosVec.y * Size + endPosVec.x);

        ArrayList InitTiles = AStar.GetTiles();
        PathIdxs = AStar.FindPaths(StartIdx, EndIdx, 0);
        ArrayList PathTiles = new ArrayList();
        for(int i=0; i< PathIdxs.Count; ++i)
        {
            Tile CurTile = InitTiles[(int)PathIdxs[i]] as Tile;
            PathTiles.Add(CurTile);
        }
        return PathTiles;
    }
    

    public ArrayList OnFindPathFromNoWayPointPaths()
    {

        GameObject startPos = GameObject.Find("StartPos");
        GameObject endPos = GameObject.Find("EndPos");

        Vector3 startPosVec = CUtility.RoundVector3(startPos.transform.position);
        Vector3 endPosVec = CUtility.RoundVector3(endPos.transform.position);

        CAStar AStar = new CAStar();
        AStar.InitTiles(ref Tiles);
        int StartIdx = (int)(startPosVec.y * Size + startPosVec.x);
        int EndIdx = (int)(endPosVec.y * Size + endPosVec.x);

        ArrayList InitTiles = AStar.GetTiles();
        PathIdxs = AStar.FindPaths(StartIdx, EndIdx, 0);
        ArrayList PathTiles = new ArrayList();
        for (int i = 0; i < PathIdxs.Count; ++i)
        {
            Tile CurTile = InitTiles[(int)PathIdxs[i]] as Tile;
            PathTiles.Add(CurTile);
        }
        return PathTiles;
    }

    public void OnBuildWayPointPaths()
    {
        CTimeCheck.Start(0);

        OnMakeWayPoints();

        CAStar AStar = new CAStar();
        AStar.InitTiles(ref Tiles);

        for (int i = WayPoints.Count/2; i < WayPoints.Count; ++i)
        {
            for (int j = WayPoints.Count / 2 - 1; 0 <= j; --j)
            {
                int StartPosIndex = (int)WayPoints[i];
                int EndPosIndex = (int)WayPoints[j];

                if (StartPosIndex == EndPosIndex)
                    continue;

                int Key = StartPosIndex < EndPosIndex ? StartPosIndex * 10000 + EndPosIndex : EndPosIndex * 10000 + StartPosIndex;
                if (WayPointPaths.ContainsKey(Key))
                    continue;


                ArrayList FindedPaths = AStar.FindPaths(StartPosIndex, EndPosIndex, 0);

                ArrayList RemoveIndexs = new ArrayList();
                for (int k = 0; k < FindedPaths.Count; ++k)
                {
                    if (!WayPoints.Contains(FindedPaths[k]))
                        RemoveIndexs.Add(FindedPaths[k]);
                }
                for (int a = 0; a < RemoveIndexs.Count; ++a)
                {
                    FindedPaths.Remove(RemoveIndexs[a]);
                }

                if (3 < FindedPaths.Count)
                {
                    int PathCount = FindedPaths.Count / 2;
                    for (int count = 1; count < PathCount; ++count)
                    {
                        int NewStartIndex = (int)FindedPaths[count];
                        int NewEndIndex = (int)FindedPaths[FindedPaths.Count - 1 - count];
                        int NewKey = NewStartIndex < NewEndIndex ? NewStartIndex * 10000 + NewEndIndex : NewEndIndex * 10000 + NewStartIndex;

                        if (WayPointPaths.ContainsKey(NewKey))
                            continue;

                        ArrayList clone = (ArrayList)FindedPaths.Clone();
                        clone.RemoveAt(FindedPaths.Count - count);
                        clone.RemoveAt(count - 1);
                        //clone = clone.GetRange(count, FindedPaths.Count - 1 - count);
                        WayPointPaths.Add(NewKey, clone);
                    }
                }
                WayPointPaths.Add(Key, FindedPaths);
            }
        }

        for (int i=0; i< WayPoints.Count; ++i)
        {
            for(int j=i+1; j< WayPoints.Count; ++j)
            {
                int StartPosIndex = (int)WayPoints[i];
                int EndPosIndex = (int)WayPoints[j];

                if (StartPosIndex == EndPosIndex)
                    continue;

                int Key = StartPosIndex < EndPosIndex ? StartPosIndex * 10000 + EndPosIndex : EndPosIndex * 10000 + StartPosIndex;
                if (WayPointPaths.ContainsKey(Key))
                    continue;

                ArrayList FindedPaths = AStar.FindPaths(StartPosIndex, EndPosIndex, 0);

                ArrayList RemoveIndexs = new ArrayList();
                for (int k=0; k< FindedPaths.Count; ++k)
                {
                    if (!WayPoints.Contains(FindedPaths[k]))
                        RemoveIndexs.Add(FindedPaths[k]);
                }
                for(int a=0; a<RemoveIndexs.Count; ++a)
                {
                    FindedPaths.Remove(RemoveIndexs[a]);
                }

                if (3 < FindedPaths.Count)
                {
                    int PathCount = FindedPaths.Count / 2;
                    for (int count = 1; count < PathCount; ++count)
                    {
                        int NewStartIndex = (int)FindedPaths[count];
                        int NewEndIndex = (int)FindedPaths[FindedPaths.Count - 1 - count];
                        int NewKey = NewStartIndex < NewEndIndex ? NewStartIndex * 10000 + NewEndIndex : NewEndIndex * 10000 + NewStartIndex;

                        if (WayPointPaths.ContainsKey(NewKey))
                            continue;

                        ArrayList clone = (ArrayList)FindedPaths.Clone();
                        clone.RemoveAt(FindedPaths.Count - count);
                        clone.RemoveAt(count - 1);
                        //clone = clone.GetRange(count, FindedPaths.Count - 1 - count);
                        WayPointPaths.Add(NewKey, clone);
                    }
                }
                WayPointPaths.Add(Key, FindedPaths);
            }
        }

        CTimeCheck.End(ELogType.MapGenerator, "BuildFastPaths");
    }

    public int Size = 100;
    public float BlockRate = 0.5f;
    public string FileName = "TileMaps";
    public string BlockImage = "BlockImage00";
    public string TileImage = "TileImage00";
    int TileFullSize;
    bool[] Tiles;
    public bool bShowAllTile = true;
    static public bool bTool = false;

    ArrayList tile_objects = new ArrayList();
    ArrayList block_objects = new ArrayList();

    Dictionary<int, UsedRenderTileInfo> used_tile_objects = new Dictionary<int, UsedRenderTileInfo>();
    public ArrayList WayPoints = new ArrayList();
    public Dictionary<int, ArrayList> WayPointPaths = new Dictionary<int, ArrayList>();

    const float ImageDefaultPosZ = 2.0f;
    ArrayList PathIdxs = new ArrayList();
}