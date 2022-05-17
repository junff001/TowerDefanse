using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AddActBtn : MonoBehaviour, IEndDragHandler,IDragHandler, IPointerUpHandler
{
    public ActData actData = null;
    public RectTransform moveImg; // 버튼 대신에 움직여줄 이미지 
    public float movedDist = 0f;
    bool bDraged = false;
    WaitForSeconds ws = new WaitForSeconds(0.1f);

    private void Start()
    {
        StartCoroutine(CheckDrag());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        foreach (var item in eventData.hovered)
        {
            if (item.CompareTag("ActContent")) // 따로 캔버스 바로 아래에 체크할 영역용 GameObject 만들어두기
            {
                InvadeManager.Instance.InsertAct(Input.mousePosition, actData.actType, actData.monsterType);
            }
        }
        moveImg.anchoredPosition = Vector3.zero;
        InvadeManager.Instance.ReduceDummyObj();
    }

    public void OnDrag(PointerEventData eventData)
    {
        InvadeManager.Instance.ShowInsertPlace(Input.mousePosition);
        moveImg.transform.position = Input.mousePosition;
    }

    IEnumerator CheckDrag()
    {
        while(true)
        {
            yield return ws;
            movedDist = Vector3.Distance(moveImg.anchoredPosition, Vector3.zero);
            if (movedDist > 10) // 10 진짜 엄청 조금 움직인거임 화면상 0.1cm 미만
            {
                bDraged = true;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!bDraged)
        {
            InvadeManager.Instance.AddAct(actData.actType, actData.monsterType);
        }
        bDraged = false;
    }
}
