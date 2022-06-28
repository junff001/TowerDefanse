using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorldMap : MonoBehaviour
{
    private static UI_MapInfoPanel infoPanel;

    private Button[] stageBtns;
    public MapInfoSO[] stageMapInfoSO;

    void Start()
    {
        if (!infoPanel)
        {
            infoPanel = FindObjectOfType<UI_MapInfoPanel>();
        }

        stageBtns = GetComponentsInChildren<Button>();

        for (int i = 0; i < stageBtns.Length; i++)
        {
            TextMeshProUGUI stageText = stageBtns[i].GetComponentInChildren<TextMeshProUGUI>();
            stageText.text = stageMapInfoSO[i].stageName;

            int index = i;
            stageBtns[i].onClick.AddListener(() =>
            {
                Debug.Log(index);
                ChooseStage(stageMapInfoSO[index]);
                infoPanel.ShowMapInfo(stageMapInfoSO[index]);
            });
        }
    }

    public void ChooseStage(MapInfoSO so)
    {
        Debug.Log($"Target Stage : {so.stageName}");
        Managers.Stage.SetTargetStage(so);
    }
}
