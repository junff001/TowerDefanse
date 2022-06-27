using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SpawnMonster : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image monsterImg;
    public Image cooltimeImg; // 스택이 0일 때, 충전률 보여주기

    [HideInInspector] public EnemySO so = null;
    [SerializeField] private GameObject iconPrefab;

    [SerializeField] Transform iconParentTrm;
    [SerializeField] Text nameText;
    [SerializeField] Text costText;

    [SerializeField] private GameObject infoUI;

    float timer = 0f;
    public float coolTime = 1f;

    bool isSpawnCooltime = false;
    bool isPressing = false;
    float pressedTime = 0;
    float checkPressTime = 0.5f;
    
    private void Update()
    {
        if(isPressing)
        {
            pressedTime += Time.deltaTime;

            if(pressedTime > checkPressTime)
            {
                ShowInfoUI(true);
                isPressing = false;
            }
        }

        ShowCoolTime();
    }

    private void ShowInfoUI(bool active)
    {
        infoUI.SetActive(active);
    }

    private void ShowCoolTime()
    {
        if (isSpawnCooltime)
        {
            timer += Time.deltaTime;
            float fillAmount = timer / coolTime;
            cooltimeImg.fillAmount = Mathf.Clamp(fillAmount, 0, 1);

            if (timer > coolTime)
            {
                timer = 0f;
                isSpawnCooltime = false;
                cooltimeImg.gameObject.SetActive(false);
            }
        }
    }

    public void Init(EnemySO so)
    {
        this.so = so;

        monsterImg.sprite = so.Sprite;
        coolTime = so.ChargeTime;
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
        if (false == isSpawnCooltime && pressedTime < checkPressTime) // 스폰 가능
        {
            Managers.Invade.SpawnEnemy(so.SpeciesType, so.MonsterType);
        }
        else // 스폰 불가능
        {
            Managers.UI.SummonPosText(transform.position, new PopupText($"{(int)(coolTime - timer)}초 뒤 충전됩니다."));
        }
        
        isPressing = false;
        pressedTime = 0f;

        ShowInfoUI(false);
    }
}
