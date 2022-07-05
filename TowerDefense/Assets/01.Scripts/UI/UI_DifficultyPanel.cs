using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DifficultyPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public Button difficultyBtn;
    public Button difficultyExitBtn;
    private TextMeshProUGUI difficultyTxt;
    private Dictionary<Define.GameDifficulty, string> diffToStrDic = new Dictionary<Define.GameDifficulty, string>()
    {
        [Define.GameDifficulty.Easy] = "쉬움",
        [Define.GameDifficulty.Normal] = "보통",
        [Define.GameDifficulty.Hard] = "어려움"
    };

    public Toggle easyToggle;
    public Toggle normalToggle;
    public Toggle hardToggle;

    public Image selectOutline;

    private Toggle[] toggles;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        toggles = GetComponentsInChildren<Toggle>();
        difficultyTxt = difficultyBtn.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        difficultyBtn.onClick.AddListener(() =>
        {
            for (int i = 0; i < toggles.Length; i++)
            {
                if (toggles[i].isOn)
                {
                    selectOutline.transform.position = toggles[i].transform.position;
                }
            }

            Managers.UI.UIFade(canvasGroup, true);
        });

        difficultyExitBtn.onClick.AddListener(() =>
        {
            Managers.UI.UIFade(canvasGroup, false);
        });

        easyToggle.onValueChanged.AddListener(value =>
        {
            Selected(value, easyToggle, Define.GameDifficulty.Easy);
        });

        normalToggle.onValueChanged.AddListener(value =>
        {
            Selected(value, normalToggle, Define.GameDifficulty.Normal);
        });

        hardToggle.onValueChanged.AddListener(value =>
        {
            Selected(value, hardToggle, Define.GameDifficulty.Hard);
        });
    }

    private void Selected(bool value, Toggle toggle, Define.GameDifficulty difficulty)
    {
        if (value)
        {
            selectOutline.transform.position = toggle.transform.position;
            GameManager.SetCoefficient(difficulty);
            difficultyTxt.text = diffToStrDic[difficulty];
        }
    }
}
