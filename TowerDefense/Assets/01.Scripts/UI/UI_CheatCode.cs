using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_CheatCode : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public bool isShow { get; set; } = false;
    public Text cheatText;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(bool show)
    {
        canvasGroup.DOFade(show ? 1 : 0, 0.5f);
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
            }
        }
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

    private bool CheckNullIndex(string[] command, int index)
    {
        return command.Length > index;
    }

    public void ResetWindow()
    {
        cheatText.text = "";
    }
}
