using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using static Define;


public struct PropertyData
{
    public ParticleSystem AuraEffect;
    public ParticleSystem BuffEffect;
}

public class BuildManager : MonoBehaviour
{
    #region 벡터들
    private Vector3Int tilePos = Vector3Int.zero;                  // 타일 위치

    private Vector3Int[] checkedPos = null;

    private Vector3Int upLeft = Vector3Int.zero;
    private Vector3Int up = Vector3Int.zero;
    private Vector3Int upRight = Vector3Int.zero;
    private Vector3Int left = Vector3Int.zero;
    private Vector3Int curPos = Vector3Int.zero;
    private Vector3Int right = Vector3Int.zero;
    private Vector3Int downLeft = Vector3Int.zero;
    private Vector3Int down = Vector3Int.zero;
    private Vector3Int downRight = Vector3Int.zero;
    #endregion

    private Camera mainCam = null;
    public Tile placeTile;
    public Tile roadTile;
    public Tile waterTile;

    private Vector2 dir = Vector2.zero; // 내가 tilePos를 기준으로 어느쪽에 있는가.
    Vector3 plusPos = Vector2.zero;

    public Map map;
    [SerializeField] private Tower towerBase;

    [SerializeField] PropertySO propertySO;

    public GameObject rangeObj;
    public GameObject movingImg = null;

    [Header("코어 관리")]
    Dictionary<eCoreName, CoreBase> coreDic = new Dictionary<eCoreName, CoreBase>();
    public List<CoreBase> coreList = new List<CoreBase>();

    [Header("버프 관리")]
    public Dictionary<PropertyType, IBuff> buffDictionary = new Dictionary<PropertyType, IBuff>();
    public List<PropertyType> propertyList = new List<PropertyType>();

    public List<Tower> spawnedTowers { get; set; } = new List<Tower>();
   
    PropertyData propertyData = new PropertyData();

    void Start()
    {
        //foreach (var item in propertyList)
        //{
        //    switch (item)
        //    {
        //        case PropertyType.NONE: buffDictionary.Add(item, null); break;
        //        case PropertyType.FIRE: Dot dot = null; buffDictionary.Add(item, dot); break;
        //        case PropertyType.WATER: Slow slow = null; buffDictionary.Add(item, slow); break;
        //        case PropertyType.LIGHTNING: Chain chain = null; buffDictionary.Add(item, chain); break;   
        //        case PropertyType.WIND: KnockBack knockBack = null; buffDictionary.Add(item, knockBack); break;
        //        case PropertyType.SOIL: Splash splash = null; buffDictionary.Add(item, splash); break;
        //        case PropertyType.GRASS: Restriction restriction = null; buffDictionary.Add(item, restriction); break;          
        //    }
        //}

        foreach (var item in coreList)
        {
            coreDic.Add(item.coreType, item);
        }

        //propertyData.AuraEffect = propertySO.AuraEffect;
        //propertyData.BuffEffect = propertySO.BuffEffect;

        mainCam = Camera.main;
        plusPos = new Vector3(map.tilemap.cellSize.x, map.tilemap.cellSize.y, 0) / 2;
    }

    void Update()
    {
        SetTilePos();
        SetAroundTiles();
    }

    public void SetTilePos()
    {
        Vector3 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        tilePos = map.tilemap.WorldToCell(pos);    // 마우스 위치에 위치한 타일 받아오기
        dir = pos - (map.tilemap.CellToWorld(tilePos) + plusPos);
    }

    public void SetAroundTiles()
    {
        upLeft = new Vector3Int(tilePos.x - 1, tilePos.y + 1, tilePos.z);
        up = new Vector3Int(tilePos.x, tilePos.y + 1, tilePos.z);
        upRight = new Vector3Int(tilePos.x + 1, tilePos.y + 1, tilePos.z);
        left = new Vector3Int(tilePos.x - 1, tilePos.y, tilePos.z);
        curPos = new Vector3Int(tilePos.x, tilePos.y, tilePos.z);
        right = new Vector3Int(tilePos.x + 1, tilePos.y, tilePos.z);
        downLeft = new Vector3Int(tilePos.x - 1, tilePos.y - 1, tilePos.z);
        down = new Vector3Int(tilePos.x, tilePos.y - 1, tilePos.z);
        downRight = new Vector3Int(tilePos.x + 1, tilePos.y - 1, tilePos.z);
    }

    public PlaceTileType placingTileType = PlaceTileType.Place; // 어차피 알아서 초기화 해주지 않을까요?

    public void ResetCheckedTiles(bool clearTileColor = false) // 전에 색을 바꿔주었던 친구들은 다시 리셋
    {
        if (checkedPos == null) return; // 처음에 널이라 오류

        if (clearTileColor) // 컬러가 없으면
        {
            foreach(var item in map.gridTilemap.cellBounds.allPositionsWithin) map.gridTilemap.SetColor(item, new Color(1, 1, 1, 0));
            return;
        }

        foreach (var pos in checkedPos)
        {
            if(IsPlaceableTile(pos, placingTileType))
                map.gridTilemap.SetColor(pos, Color.white); // 파란색으로 바꿔주기
        }
    }

    public void SetTilesColor(PlaceTileType placeTileType)
    {
        ResetCheckedTiles(); // 얘는 그냥 자기 색깔 유지하게 해줘야 함

        Vector3Int[] checkPos = Get2By2Tiles();

        bool canPlace = CanPlace(placeTileType);
        foreach (var pos in checkPos)
        {
            if (false == IsPlaceableTile(pos, placeTileType)) continue;

            if (canPlace)
                map.gridTilemap.SetColor(pos, Color.blue);
            else
                map.gridTilemap.SetColor(pos, Color.red);
        }

        checkedPos = checkPos; // 내가 체크할 포지션들을 나중에 지워주야
    }

    public bool CanPlace(PlaceTileType placeTileType) // 2x2 타일 검사
    {
        bool canPlace = true;

        foreach (var pos in Get2By2Tiles())
        {
            if (IsPlaceableTile(pos, placeTileType) == false)
            {
                canPlace = false;
                break;
            }
        }
        return canPlace;
    }

    public Vector3Int GetCurTilePos() => tilePos;

    public Vector3Int[] Get2By2Tiles()
    {
        if (dir.x > 0 && dir.y > 0) // 1사분면
            return new Vector3Int[4] { curPos, right, upRight, up };
        else if (dir.x < 0 && dir.y > 0)// 2사분면
            return new Vector3Int[4] { curPos, left, upLeft, up };
        else if (dir.x < 0 && dir.y < 0)// 3사분면
            return new Vector3Int[4] { curPos, left, downLeft, down };
        else  // 4사분면
            return new Vector3Int[4] { curPos, right, downRight, down };
    }

    public bool IsPlaceableTile(Vector3Int pos, PlaceTileType placeTileType)
    {
        if (pos.x < 0 || pos.y < 0) return false;
        if (pos.x >= map.width || pos.y >= map.height) return false;

        switch(placeTileType)
        {
            case PlaceTileType.Place:
                if (map.mapTileTypeArray[pos.x, pos.y] != TileType.Place) return false;
                break;
            case PlaceTileType.Road:
                if (map.mapTileTypeArray[pos.x, pos.y] != TileType.Road) return false;
                break;
        }

        //여기까지 오면 OK! 타워가 깔릴 수 있는 타입과, 내가 깔려고 하는 타일의 타일타입이 같은거니까 설치 가능.
        return true;
    }   
    
    public Vector3 Get2By2TilesCenter(Vector3Int[] targetTiles)
    {
        float reviseY = targetTiles[0].y > targetTiles[2].y ? -0.5f : 0.5f;

        float x = (float)(targetTiles[0].x + targetTiles[1].x) / 2;
        float y = targetTiles[0].y + reviseY;

        // 그리드를 이동시켰기 때문에, 이제 이동시킨 값만큼 보내줘야 해

        Vector3 center = new Vector3(x - (float)(map.width - 1) / 2, y - (float)(map.height) / 2, targetTiles[0].z);

        return center;
    }

    public void MakeNewCore(TowerSO towerSO, Tower newTower)
    {
        CoreBase newCore = Instantiate(coreDic[towerSO.coreType]);
        newCore.transform.SetParent(newTower.transform);
        newCore.transform.position = newTower.coreTrm.position;
        newCore.TowerData = newTower.TowerData;
        //newCore.Buff = buffDictionary[newCore.TowerData.Property];
    }
    
    public void MakeNoTowerCore(TowerSO towerSO, Tower newTower)
    {
        foreach(var item in newTower.GetComponentsInChildren<SpriteRenderer>())
        {
            item.enabled = false;
        }

        CoreBase newCore = Instantiate(coreDic[towerSO.coreType]);
        newCore.transform.SetParent(newTower.transform);
        newCore.transform.position = newTower.transform.position;
        newCore.TowerData = newTower.TowerData;
        //newCore.Buff = buffDictionary[newCore.TowerData.Property];
    }

    // 타워를 스폰하는 함수
    public void SpawnTower(TowerSO towerSO, Vector3 placePos)
    {
        Tower newTower = Instantiate(towerBase, placePos, Quaternion.identity);
        newTower.InitTowerData(towerSO);

        if(towerSO.hasTower) // 코어가 타워를 가져야 하는 친구인가?
        {
            MakeNewCore(towerSO, newTower);
        }
        else
        {
            MakeNoTowerCore(towerSO, newTower);
        }

        //원래는 타일 값에 따라 받아와야 하지만, 어차피 타일 값 == 포지션 값이니까..

        if (towerSO.coreType != eCoreName.Spike)
        {
            foreach (var item in newTower.GetComponentsInChildren<SpriteRenderer>())
            {
                item.sortingOrder = map.height - (int)newTower.transform.position.y;
            }
        }

        SetTowerGrid(newTower, checkedPos, true);
        newTower.myCheckedPos = checkedPos; // 저장

        spawnedTowers.Add(newTower);
        // 레코드
        RecordTowerPlace recordSegment = new RecordTowerPlace(placePos, towerSO);
        Managers.Record.AddRecord(recordSegment);

        // 설치 이펙트
        Effect_StoneFrag effectStone = Managers.Pool.GetItem<Effect_StoneFrag>();
        effectStone.transform.position = placePos;
    }

    public void SetTowerGrid(Tower tower, Vector3Int[] checkPos, bool value)
    {
        foreach (var pos in checkPos) // 2x2타일은 타워 설치한 칸으로 설정해주고.
        {
            TileType placeTileType = TileType.None;

            switch (tower.TowerData.PlaceTileType)
            {
                case PlaceTileType.Place:
                    placeTileType = value ? TileType.Place_Tower : TileType.Place;
                    break;
                case PlaceTileType.Road:
                    placeTileType = value ? TileType.Road_Tower : TileType.Road;
                    break;
            }
            Managers.Build.map.mapTileTypeArray[pos.x, pos.y] = placeTileType;
        }
    }
}
