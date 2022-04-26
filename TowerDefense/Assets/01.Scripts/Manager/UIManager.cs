using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    public Text hpText;
    public Text moneyText;

    public Button speedButton;

    [Header("Ŀ��")]
    public Sprite disableCursor;
    public Sprite ableCursor;

    public Image cursorObj;

    private void Awake()
    {
        Cursor.visible = false;
        cursorObj.gameObject.SetActive(true);
    }

    private void Update()
    {
        cursorObj.transform.position = GameManager.Instance.mousePosition;
    }

    public void UpdateHPText()
    {
        hpText.text = GameManager.Instance.Hp.ToString();
    }

    public void UpdateGoldText()
    {
        moneyText.text = GameManager.Instance.Gold.ToString();
    }
}
