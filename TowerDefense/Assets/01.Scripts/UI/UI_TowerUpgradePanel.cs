using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TowerUpgradePanel : MonoBehaviour
{
    [SerializeField] private float plusX = 0f;
    [SerializeField] private float plusY = 0f;

    // 패널의 포지션을 잡는 함수
    public void SetPosition(Tower tower)
    {
        transform.position = Camera.main.ScreenToWorldPoint(tower.transform.position);
        transform.position += new Vector3(transform.position.x + plusX, transform.position.y + plusY, 0);
    }

    // 타워 업그레이드를 실행하는 함수
    public void OnTowerUpgrade()
    {

    }
}
