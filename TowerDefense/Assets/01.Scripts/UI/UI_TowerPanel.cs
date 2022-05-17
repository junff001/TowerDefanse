using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TowerPanel : MonoBehaviour
{
    private Button myButton;

    [SerializeField] private Image towerImage = null;
    [SerializeField] private Text towerCostText = null;
    [SerializeField] private GameObject towerPrefab = null;

    private void Awake()
    {
        myButton = GetComponent<Button>();
    }

    private void Start()
    {
        myButton.onClick.AddListener(() =>
        {
            BuildManager.Instance.TowerSetting(towerPrefab);
        });
    }

    public void BtnInit(Sprite towerSprite, int towerCost)
    {
        towerImage.sprite = towerSprite;
        towerCostText.text = towerCost.ToString();
    }
}
