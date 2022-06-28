using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorldMap : MonoBehaviour
{
    private Button[] stageBtns;
    public MapInfoSO[] stageMapInfoSO;

    void Start()
    {
        stageBtns = GetComponentsInChildren<Button>();

        for (int i = 0; i < stageBtns.Length; i++)
        {
            TextMeshProUGUI stageText = stageBtns[i].GetComponentInChildren<TextMeshProUGUI>();
            stageText.text = stageMapInfoSO[i].stageName;

            stageBtns[i].onClick.AddListener(() =>
            {

            });
        }
    }


}
