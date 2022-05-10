using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TowerPanel : MonoBehaviour
{
    [SerializeField] private Image towerImage = null;

    public void TowerSetting()
    {
        BuildManager.Instance.isTowerSetting = true;
        BuildManager.Instance.towerColor = towerImage.color;
    }
}
