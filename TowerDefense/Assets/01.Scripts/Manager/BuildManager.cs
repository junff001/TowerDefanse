using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildManager : Singleton<BuildManager>
{
    public GameObject towerPrefab = null;               // 타워 프리팹

    private Ray ray = default;
    private RaycastHit hit = default;
    private bool isBuild = false;                            // 좌클릭 bool 값
    private Camera mainCam = null;                       
    private SpriteRenderer currnetTileSprite = null;    // 컬러를 활성화할 현재 타일의 스프라이트
   
    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        SpawnTileRaycast();
    }

    // 타워를 스폰하는 함수
    public void SpawnTower(Transform tileTransform)
    {
        // Tower 가 SpawnTile 자식으로 들어감
        // 중복 생성 방지
        if (tileTransform.childCount < 1)
        {
            Instantiate(towerPrefab, tileTransform);
        }
    }

    // 좌클릭 bool 값 담는 함수
    public void SpawnTowerAcitve(InputAction.CallbackContext context)
    {
        isBuild = context.ReadValueAsButton();
    }

    // 스폰 타일인지 Raycast 로 체크하는 함수
    void SpawnTileRaycast()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // UI 가 있나없나 체크
        {
            ray = mainCam.ScreenPointToRay(GameManager.Instance.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("SpawnTile"))
                {
                    SpawnTileColorActive(hit.transform.GetComponent<SpriteRenderer>());

                    if (isBuild)
                    {
                        SpawnTower(hit.transform);
                    }
                }
            }
        }
    } 

    // 스폰 타일 컬러 활성화하는 함수
    void SpawnTileColorActive(SpriteRenderer tileSprite)
    {
        if (currnetTileSprite != null)
        {
            currnetTileSprite.color = Color.black;
        }

        currnetTileSprite = tileSprite;
        currnetTileSprite.color = Color.white;
    }
}
