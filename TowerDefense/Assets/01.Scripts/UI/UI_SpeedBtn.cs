using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SpeedBtn : MonoBehaviour
{
    private Button button;

    private bool IsDoubleSpeed = false;

    public Sprite oneSpeedSpr;
    public Sprite doubleSpeedSpr;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        

        button.onClick.AddListener(() =>
        {
            IsDoubleSpeed = !IsDoubleSpeed;
            button.image.sprite = IsDoubleSpeed ? doubleSpeedSpr : oneSpeedSpr;

            Time.timeScale = IsDoubleSpeed ? 2 : 1;

        });
    }
}
