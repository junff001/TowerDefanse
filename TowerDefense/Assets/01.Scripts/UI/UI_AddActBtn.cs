using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AddActBtn : MonoBehaviour, IEndDragHandler,IDragHandler, IPointerUpHandler, IBeginDragHandler
{
    [SerializeField] Text costText;

    public ActData actData = null;
    public Image monsterImg; // 버튼 대신에 움직여줄 이미지 
    public int cost = 1;

    public float movedDist = 0f;
    bool bDraged = false;
    WaitForSeconds ws = new WaitForSeconds(0.1f);

    private GraphicRaycaster gr;

    private Mask mask;

    public void Init(Define.MonsterType monsterType = Define.MonsterType.None)
    {
        if (monsterType == Define.MonsterType.None)
        {
            actData = new ActData(Define.ActType.Wait , monsterType);
        }
        else
        {
            actData = new ActData(Define.ActType.Enemy, monsterType);
        }
        
        mask = transform.GetComponentInChildren<Mask>();
        gr = transform.root.GetComponent<GraphicRaycaster>();

        StartCoroutine(CheckDrag());

        EnemySO enemySO = Managers.Game.GetActBtnSprite(monsterType);
        monsterImg.sprite = enemySO.Sprite;
        cost = enemySO.Cost;
        costText.text = cost.ToString();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!bDraged)
        {
            if (Managers.Invade.CanSetWave(actData.actType, cost))
            {
                Managers.Invade.AddAct(actData, cost);
            }
            else
            {
                PopupText text = new PopupText("웨이브를 추가할 수 없습니다.");
                Managers.UI.SummonPosText(transform.position, text);
            }
        }
        bDraged = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Managers.Invade.ShowInsertPlace(Input.mousePosition, actData);
        monsterImg.transform.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Managers.Invade.draggingBtn = this;
        mask.enabled = false;
        Managers.Invade.invisibleObj.gameObject.SetActive(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragEnd();
        Managers.Invade.draggingBtn = null;
        if (Managers.Invade.CanSetWave(actData.actType, cost))
        {
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(eventData, results);

            for(int i = 0; i< results.Count; i++)
            {
                if (results[i].gameObject.CompareTag("ActContent"))
                {
                    Managers.Invade.InsertAct(Input.mousePosition, actData, cost);
                }
            }
        }
        else
        {
            PopupText text = new PopupText("웨이브를 추가할 수 없습니다.");
            Managers.UI.SummonPosText(transform.position, text);
        }

    }

    public void OnDragEnd()
    {
        mask.enabled = true;
        monsterImg.rectTransform.anchoredPosition = Vector3.zero;
        Managers.Invade.ReduceDummyObj();
        Managers.Invade.ResetButtons();
    }

    IEnumerator CheckDrag()
    {
        while (true)
        {
            yield return ws;
            movedDist = Vector3.Distance(monsterImg.rectTransform.anchoredPosition, Vector3.zero);
            if (movedDist > 10) // 10 진짜 엄청 조금 움직인거임 화면상 0.1cm 미만
            {
                bDraged = true;
            }
        }
    }

}
