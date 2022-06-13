using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CheatCode : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public bool isShow { get; set; } = false;
    public InputField cheatText;
    public InputField cheatText_temp;

    private GameObject showui_inGameCanvas;
    private bool showui_enabled = true;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(bool show)
    {
        ResetWindow();

        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.blocksRaycasts = show;
        canvasGroup.interactable = show;
        isShow = show;

        if (show)
        {
            StartCoroutine(TextSelect());
        }
    }

    private IEnumerator TextSelect()
    {
        cheatText_temp.Select();
        yield return null;
        cheatText.Select();
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

        Show(false);
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
                        PopupText text = new PopupText($"gold 값을 {result}로 설정했습니다");
                        text.maxSize = 40;
                        Managers.UI.SummonText(new Vector2(960, 300), text);
                    }
                    break;
                case "health":
                    if (int.TryParse(command[2], out int result2))
                    {
                        Managers.Game.SetHP(result2);
                        PopupText text = new PopupText($"health 값을 {result2}로 설정했습니다");
                        text.maxSize = 40;
                        Managers.UI.SummonText(new Vector2(960, 300), text);
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
                    PopupText text = new PopupText($"timeScale 값을 {result}로 설정했습니다");
                    text.maxSize = 40;
                    Managers.UI.SummonText(new Vector2(960, 300), text);
                }
                else
                {
                    PopupText text = new PopupText($"잘못된 값입니다 : (0 ~ 16)");
                    text.maxSize = 40;
                    Managers.UI.SummonText(new Vector2(960, 300), text);
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

            PopupText text = new PopupText($"UI 보이기 여부를 {value}로 설정했습니다");
            text.maxSize = 40;
            Managers.UI.SummonText(new Vector2(960, 300), text);
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
