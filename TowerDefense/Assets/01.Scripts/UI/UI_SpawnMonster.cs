using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SpawnMonster : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public EnemySO so = null;

    [SerializeField] private Image monsterImg;
    [SerializeField] private UI_EnemyInfo infoUI;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Transform iconParentTrm;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private Sprite markedSprite;
    [SerializeField] private Sprite notMarkedSprite;
    [SerializeField] private Button bookmarkBtn;
    [HideInInspector] public bool isMarked = false;

    float pressedTime = 0;   
    private const float checkPressTime = 0.25f;

    bool isPressing = false;


    private void Start()
    {
        bookmarkBtn.onClick.AddListener(() =>
        {
            isMarked = !isMarked;
            if(isMarked)
            {
                Managers.Invade.bookmarkedMonsters.Add(this);
                bookmarkBtn.image.sprite = markedSprite;
            }
            else
            {
                Managers.Invade.bookmarkedMonsters.Remove(this);
                bookmarkBtn.image.sprite = notMarkedSprite;
            }
        });
    }

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

    Vector2 pointerDownPos = Vector2.zero;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressing = true;
        pointerDownPos = eventData.position;
    }

    private bool CanSpawn(Vector2 start, Vector2 end)
    {
        return Vector2.Distance(start, end) < 10 && pressedTime < checkPressTime;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       if (CanSpawn(pointerDownPos, eventData.position))
       {
           Managers.Invade.SpawnEnemy(so);
       }

       isPressing = false;
       pressedTime = 0f;
       ShowInfoUI(false);
    }
}
