using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MapInfoPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public Image mapImage;
    public Button mapStartBtn;
    public Button infoExitBtn;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        mapStartBtn.onClick.AddListener(() =>
        {
            GoInGame();
        });

        infoExitBtn.onClick.AddListener(() =>
        {
            Managers.UI.UIFade(canvasGroup, false);
        });
    }

    public void ShowMapInfo(MapInfoSO so)
    {
        Managers.UI.UIFade(canvasGroup, true);
        mapImage.sprite = so.stageSprite;
    }

    public void GoInGame()
    {
        Managers.LoadScene.LoadScene("SampleScene");
    }
}
