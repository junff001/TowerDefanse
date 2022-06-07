using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UI_CancelActBtn : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler
{
    public ActData actData = null;
    public Button cancelActBtn;
    public int actStackCount = 0;
    public int idx = 0;
    public int cost;


    public Image monsterImg;
    public Text countText;

    public bool bDragging = false;

    private void Start()
    {
        gr = transform.root.GetComponent<GraphicRaycaster>();
    }

    public void Stack() // 쌓기
    {
        actStackCount++;
        countText.text = actStackCount.ToString();
    }

    public void Cancel()
    {
        Managers.Invade.OnCancelAct(actData, cost);
        actStackCount--;
        countText.text = actStackCount.ToString();
        DestroyCheck();
        Managers.Invade.RefreshRemoveIdxes();
    }

    public void Init(ActData actData, Transform parentTrm)
    {
        this.actData = actData;
        EnemySO enemySO = Managers.Game.GetActBtnSprite(actData.monsterType);
        this.monsterImg.sprite = enemySO.Sprite;
        cost = enemySO.Cost;
        this.name = actData.monsterType.ToString();
        this.parent = parentTrm;
    }
        
    public void DestroyCheck() // todo 풀매니저
    {
        if (actStackCount == 0)
        {
            InvadeManager im = Managers.Invade;
            im.waitingActs.Remove(this);

            if (im.waitingActs.Count > 0)
            {
                im.addedAct = im.waitingActs[im.waitingActs.Count - 1].actData;
                im.addedBtn = im.waitingActs[im.waitingActs.Count - 1];
                Managers.Invade.OnBtnRemoved(idx);
            }
            else
            {
                im.addedBtn = null;
                im.addedAct = null;
            }

            Destroy(this.gameObject);
        }
    }

    Transform parent = null;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Managers.Game.isAnyActing = true;
        bDragging = true;
        transform.parent = parent.parent; // 잠깐 밖으로 뺐다가 들어오게 해서 정렬 다시하려구

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    private GraphicRaycaster gr;

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(eventData, results);

        bool bThrowAway = true;
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.CompareTag("ActContent"))
            {
                bThrowAway = false;
                break;
            }
        }

        if (bThrowAway == true)
        {
            Managers.UI.SummonText(Camera.main.WorldToScreenPoint(transform.position),
                $"{actData.actType} {actStackCount}개 삭제", 30);
            while (actStackCount > 0) Cancel();
        }
        else
        {
            transform.parent = parent; // 다시 제 부모 찾아가게 해주고
            transform.SetSiblingIndex(idx + 1);
        }

        bDragging = false;
        Managers.Game.isAnyActing = false;
    }
}
