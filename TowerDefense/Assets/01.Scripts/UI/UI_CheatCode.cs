using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CheatCode : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public bool isShow { get; set; } = false;
    public Text cheatText;

    private GameObject showui_inGameCanvas;
    private bool showui_enabled = true;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(bool show)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.blocksRaycasts = show;
        canvasGroup.interactable = show;
        isShow = show;
    }

    public void CheckCommand()
    {
        string command = cheatText.text;

        string[] split_command = command.Split(' ');

        if (CheckNullIndex(split_command, 0))
        {
            switch (split_command[0])
            {
                case "/setvalue":
                    SetValueCommand(split_command);
                    break;
                case "/timescale":
                    TimeScaleCommand(split_command);
                    break;
                case "/showui":
                    ShowUICommand(split_command);
                    break;
            }
        }

        ResetWindow();
    }

    private void SetValueCommand(string[] command)
    {
        if (CheckNullIndex(command, 2))
        {
            switch (command[1])
            {
                case "gold":
                    if(int.TryParse(command[2], out int result))
                    {
                        Managers.Gold.GoldSet(result);
                        Managers.UI.SummonText(new Vector2(960, 300), $"gold 값을 {result}로 설정했습니다", 40);
                    }
                    break;
                case "health":
                    if (int.TryParse(command[2], out int result2))
                    {
                        Managers.Game.SetHP(result2);
                        Managers.UI.SummonText(new Vector2(960, 300), $"health 값을 {result2}로 설정했습니다", 40);
                    }
                    break;
            }
        }
    }

    private void TimeScaleCommand(string[] command)
    {
        if (CheckNullIndex(command, 1))
        {
            if (int.TryParse(command[1], out int result))
            {
                if (result >= 0 && result <= 16)
                {
                    Time.timeScale = result;
                    Managers.UI.SummonText(new Vector2(960, 300), $"timeScale 값을 {result}로 설정했습니다", 40);
                }
                else
                {
                    Managers.UI.SummonText(new Vector2(960, 300), $"잘못된 값입니다 : (0 ~ 16)", 40);
                }
            }
        }
    }

    private void ShowUICommand(string[] command)
    {
        if (CheckNullIndex(command, 1))
        {
            if (bool.TryParse(command[1], out bool result))
            {
                ChangeUIShow(result);
            }
        }
        else
        {
            showui_enabled = !showui_enabled;
            ChangeUIShow(showui_enabled);
        }

        void ChangeUIShow(bool value)
        {
            if (showui_inGameCanvas == null)
            {
                showui_inGameCanvas = GameObject.Find("InGame Canvas");
            }

            showui_inGameCanvas.SetActive(value);
            showui_enabled = value;

            Managers.UI.SummonText(new Vector2(960, 300), $"UI 보이기 여부를 {value}로 설정했습니다", 40);
        }
    }

    private bool CheckNullIndex(string[] command, int index)
    {
        return command.Length > index;
    }

    public void ResetWindow()
    {
        cheatText.text = "";
    }
}
