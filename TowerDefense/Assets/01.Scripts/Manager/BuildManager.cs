using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildManager : Singleton<BuildManager>
{
    public GameObject towerPrefab = null;               // 타워 프리팹

    private RaycastHit hit = default;
    private bool isPressLeftClick = false;                 
    private SpriteRenderer currnetTileSprite = null;    // 컬러를 활성화할 현재 타일의 스프라이트
    private Tower currentTower = null;                  // 공격 범위릃 활성화할 현재 타워

    void Update()
    {
        InteractionByTile();
    }

    // 타워를 스폰하는 함수
    void SpawnTower(Transform tileTransform)
    {
        // Tower 가 SpawnTile 자식으로 들어감
        // 중복 생성 방지
        if (tileTransform.childCount < 1)
        {
            Instantiate(towerPrefab, tileTransform);
        }
    }

    // Mouse.RaycastHit() 으로 타일에 따라 상호작용하는 함수
    void InteractionByTile()
    {
        hit = Mouse.RaycastHit();
        isPressLeftClick = Mouse.isPressLeftClick;

        if (hit.transform != null)
        {
            switch (hit.transform.tag)
            {
                case "SpawnTile":
                {
                    SpawnTileColorActive(hit.transform.GetComponent<SpriteRenderer>());

                    if (isPressLeftClick)
                    {
                        SpawnTower(hit.transform);
                    }
                }
                break;
                case "RoadTile":
                {
                    if (currnetTileSprite != null)
                    {
                        currnetTileSprite.color = Color.white;
                    }
                }
                break;
                case "Tower":
                {
                    if (isPressLeftClick)
                    {
                        hit.transform.GetComponent<Tower>().TowerUpgrade();
                    }
                    // 업그레이드 함수
                    // 공격 범위 표시 함수
                }
                break;
                default:
                    break;
            }
        }
    } 

    // 스폰 타일 컬러 활성화하는 함수
    void SpawnTileColorActive(SpriteRenderer tileSprite)
    {
        if (currnetTileSprite != null)
        {
            currnetTileSprite.color = Color.white;
        }

        currnetTileSprite = tileSprite;
        currnetTileSprite.color = Color.red;
    }
}
