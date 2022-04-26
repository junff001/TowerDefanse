using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PausePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private float saved_timeScale = 1;
    public static bool isPaused = false;

    public Button pauseButton;
    public Button resumeButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        pauseButton.onClick.AddListener(() =>
        {
            Pause();
        });

        resumeButton.onClick.AddListener(() =>
        {
            Pause();
        });
    }

    private void Pause()
    {
        isPaused = !isPaused;

        if(isPaused)
        {
            saved_timeScale = Time.timeScale;
            Time.timeScale = 0;

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            Time.timeScale = saved_timeScale;

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
