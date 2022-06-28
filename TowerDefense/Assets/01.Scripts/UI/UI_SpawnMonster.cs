using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SpawnMonster : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public EnemySO so = null;

    [SerializeField] private Image monsterImg;
    [SerializeField] private UI_EnemyInfo infoUI;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Transform iconParentTrm;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
                             
    float pressedTime = 0;   
    private const float checkPressTime = 1f;

    bool isSpawnCooltime = false;
    bool isPressing = false;

    private void Update()
    {
        //CheckPress();
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
        Managers.Invade.SpawnEnemy(so);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       //if (false == isSpawnCooltime && pressedTime < checkPressTime) // 스폰 가능
       //{
       //    Managers.Invade.SpawnEnemy(so);
       //    Debug.Log("스폰");
       //}
       //else // 스폰 불가능
       //{
       //    Debug.Log("불가");
       //}
       //
       //isPressing = false;
       //pressedTime = 0f;
       //
       //ShowInfoUI(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowInfoUI(true) ;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShowInfoUI(false);
    }
}
