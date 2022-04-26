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

    [Header("Ä¿¼­")]
    public Sprite disableCursor;
    public Sprite ableCursor;

    public Image cursorObj;

    private void Awake()
    {
        Cursor.visible = false;
        cursorObj.gameObject.SetActive(true);
    }

    public void UpdateHPText()
    {
        hpText.text = GameManager.Instance.Hp.ToString();
    }

    public void UpdateGoldText()
    {
        moneyText.text = GameManager.Instance.Gold.ToString();
    }

    public void MousePosition(InputAction.CallbackContext context)
    {
        cursorObj.transform.position = context.ReadValue<Vector2>();
    }
}
