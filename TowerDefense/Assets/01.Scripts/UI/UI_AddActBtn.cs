using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AddActBtn : MonoBehaviour, IEndDragHandler,IDragHandler, IPointerUpHandler, IBeginDragHandler
{
    public ActData actData = null;
    public RectTransform moveImg; // 버튼 대신에 움직여줄 이미지 
    public float movedDist = 0f;
    bool bDraged = false;
    WaitForSeconds ws = new WaitForSeconds(0.1f);

    private GraphicRaycaster gr;

    private Mask mask;

    private void Start()
    {
        mask = transform.GetComponentInChildren<Mask>();
        gr = transform.root.GetComponent<GraphicRaycaster>();

        moveImg.GetComponent<Image>().sprite = GameManager.Instance.GetActBtnSprite(actData.monsterType);

        StartCoroutine(CheckDrag());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!bDraged)
        {
            if (InvadeManager.Instance.CanSetWave(actData.actType))
            {
                InvadeManager.Instance.AddAct(actData);
            }
            else
            {
                UIManager.SummonText(transform.position, "웨이브를 추가할 수 없습니다.", 30);
            }
        }
        bDraged = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        InvadeManager.Instance.ShowInsertPlace(Input.mousePosition, actData);
        moveImg.transform.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InvadeManager.Instance.draggingBtn = this;
        mask.enabled = false;
        InvadeManager.Instance.invisibleObj.gameObject.SetActive(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragEnd();
        InvadeManager.Instance.draggingBtn = null;
        if (InvadeManager.Instance.CanSetWave(actData.actType))
        {
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(eventData, results);

            foreach (var item in results)
            {
                if (item.gameObject.CompareTag("ActContent"))
                {
                    
                    InvadeManager.Instance.InsertAct(Input.mousePosition, actData);
                }
            }
        }
        else
        {
            UIManager.SummonText(transform.position, "웨이브를 추가할 수 없습니다.", 30);
        }

    }

    public void OnDragEnd()
    {
        mask.enabled = true;
        moveImg.anchoredPosition = Vector3.zero;
        InvadeManager.Instance.ReduceDummyObj();
        InvadeManager.Instance.ResetButtons();
    }

    IEnumerator CheckDrag()
    {
        while (true)
        {
            yield return ws;
            movedDist = Vector3.Distance(moveImg.anchoredPosition, Vector3.zero);
            if (movedDist > 10) // 10 진짜 엄청 조금 움직인거임 화면상 0.1cm 미만
            {
                bDraged = true;
            }
        }
    }

}
