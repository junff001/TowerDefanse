using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SpawnMonster : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public EnemySO so = null;

    [SerializeField] private Image monsterImg;
    [SerializeField] private UI_EnemyInfo infoUI;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Transform iconParentTrm;
    [SerializeField] private Text nameText;
    [SerializeField] private Text costText;

    float pressedTime = 0;
    private const float checkPressTime = 1f;

    bool isSpawnCooltime = false;
    bool isPressing = false;

    private void Update()
    {
        CheckPress();
    }

    private void CheckPress()
    {
        if (isPressing)
        {
            pressedTime += Time.deltaTime;

            if (pressedTime > checkPressTime)
            {
                ShowInfoUI(true);
                isPressing = false;
            }
        }
    }

    private void ShowInfoUI(bool active)
    {
        infoUI.gameObject.SetActive(active);
    }


    public void Init(EnemySO so)
    {
        this.so = so;
        infoUI.Init(this.so);

        monsterImg.sprite = so.Sprite;
        nameText.text = so.MyName;
        costText.text = so.SpawnCost.ToString();

        for(int i = 0;i < so.TypeIcons.Length; i++)
        {
            GameObject go = Instantiate(iconPrefab, iconParentTrm);
            go.GetComponent<Image>().sprite = so.TypeIcons[i];
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"time : {pressedTime}, checkTime : {checkPressTime}");

        if (false == isSpawnCooltime && pressedTime < checkPressTime) // 스폰 가능
        {
            if(so.SpawnCost < Managers.Gold.Gold)
            {
                Managers.Invade.SpawnEnemy(so.SpeciesType, so.MonsterType);
            }
            else
            {
                Managers.UI.SummonRectText(Input.mousePosition, new PopupText("돈이 부족합니다"));
            }
        }
        else // 스폰 불가능
        {
            Managers.UI.SummonPosText(Input.mousePosition, new PopupText("지금은 소환 불가능합니다!"));
        }
        
        isPressing = false;
        pressedTime = 0f;

        ShowInfoUI(false);
    }
}
