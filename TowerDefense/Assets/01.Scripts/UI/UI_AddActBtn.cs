using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AddActBtn : MonoBehaviour, IEndDragHandler,IDragHandler, IPointerUpHandler, IBeginDragHandler
{
    public ActData actData = null;
    public Image moveImg; // 버튼 대신에 움직여줄 이미지 
    public float movedDist = 0f;
    public int cost = 1;

    bool bDraged = false;
    WaitForSeconds ws = new WaitForSeconds(0.1f);

    private GraphicRaycaster gr;

    private Mask mask;

    private void Start()
    {
        mask = transform.GetComponentInChildren<Mask>();
        gr = transform.root.GetComponent<GraphicRaycaster>();

        EnemySO enemySO = Managers.Game.GetActBtnSprite(actData.monsterType);
        moveImg.sprite = enemySO.Sprite;
        cost = enemySO.Cost;

        StartCoroutine(CheckDrag());
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
                Managers.UI.SummonText(transform.position, "웨이브를 추가할 수 없습니다.", 30);
            }
        }
        bDraged = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Managers.Invade.ShowInsertPlace(Input.mousePosition, actData);
        moveImg.transform.position = Input.mousePosition;
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
            Managers.UI.SummonText(transform.position, "웨이브를 추가할 수 없습니다.", 30);
        }

    }

    public void OnDragEnd()
    {
        mask.enabled = true;
        moveImg.rectTransform.anchoredPosition = Vector3.zero;
        Managers.Invade.ReduceDummyObj();
        Managers.Invade.ResetButtons();
    }

    IEnumerator CheckDrag()
    {
        while (true)
        {
            yield return ws;
            movedDist = Vector3.Distance(moveImg.rectTransform.anchoredPosition, Vector3.zero);
            if (movedDist > 10) // 10 진짜 엄청 조금 움직인거임 화면상 0.1cm 미만
            {
                bDraged = true;
            }
        }
    }

}
