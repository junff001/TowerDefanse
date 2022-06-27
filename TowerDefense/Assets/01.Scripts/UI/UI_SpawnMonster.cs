using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SpawnMonster : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Text stackText = null; // 스택 텍스트
    [SerializeField] Text typeText = null; // 타입 텍스트
    public Image monsterImg;
    public Image cooltimeImg; // 스택이 0일 때, 충전률 보여주기

    public EnemySO so = null;
    public int stackCount;
    public int maxStackCount = 10;

    float timer = 0f;
    public float coolTime = 1f;

    private void Update()
    {
        if (stackCount >= maxStackCount) return; // 더 쌓아두면 안돼!

        ShowCoolTime();
    }

    private void ShowCoolTime()
    {
        float fillAmount = timer / coolTime;
        cooltimeImg.fillAmount = Mathf.Clamp(fillAmount, 0, 1);
        timer += Time.deltaTime;

        if (timer >= coolTime)
        {
            timer = 0f;
            stackCount++;
            stackText.text = stackCount.ToString();

            CheckStack();
        }
    }

    private void CheckStack()
    {
        // 스택이 0일때만 쿨타임 보여주기. 그게 아니면 텍스트로 초만 띄워주죠?
        if(stackCount == 0)
        {
            Color targetColor = new Color(cooltimeImg.color.r, cooltimeImg.color.g, cooltimeImg.color.b, 0.8f);
            cooltimeImg.color = targetColor;
        }
    }

    public void Init(EnemySO so)
    {
        this.so = so;

        monsterImg.sprite = so.Sprite;
        coolTime = so.ChargeTime;
        typeText.text = so.MyType;
        stackText.text = stackCount.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(stackCount > 0)
        {
            Managers.Invade.SpawnEnemy(so.SpeciesType, so.MonsterType);
            stackCount--;
            CheckStack();
        }
        else
        {
            Managers.UI.SummonPosText(transform.position, new PopupText($"{(int)(coolTime - timer)}초 뒤 충전됩니다."));
        }
    }
}
