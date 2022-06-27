using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Define;

public class UI_ChooseDiffculty : MonoBehaviour
{
    public Button exitBtn;
    public GameObject difficultyPanel;

    public Button easyBtn;
    public Button normalBtn;
    public Button hardBtn;

    void Start()
    {
        exitBtn.onClick.AddListener(() =>
        {
            difficultyPanel.transform.DOScale(0, 0.3f);
        });

        easyBtn.onClick.AddListener(() =>
        {
            GoInGame(GameLevel.Easy);
        });
        normalBtn.onClick.AddListener(() =>
        {
            GoInGame(GameLevel.Normal);
        });
        hardBtn.onClick.AddListener(() =>
        {
            GoInGame(GameLevel.Hard);
        });
    }

    public void GoInGame(GameLevel level)
    {
        GameManager.StageLevel = level;
        Managers.LoadScene.LoadScene("SampleScene");
    }
}
