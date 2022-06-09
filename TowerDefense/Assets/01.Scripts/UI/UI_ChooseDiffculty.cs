using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
            GoInGame(1);
        });
        normalBtn.onClick.AddListener(() =>
        {
            GoInGame(2);
        });
        hardBtn.onClick.AddListener(() =>
        {
            GoInGame(3);
        });
    }

    public void GoInGame(int Level)
    {
        GameManager.stageLevel = 3;
        Managers.LoadScene.LoadScene("JuhyeongScene");
    }
}
