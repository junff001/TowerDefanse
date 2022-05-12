using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AddActBtn : MonoBehaviour, IEndDragHandler,IDragHandler
{
    public ActData actData = null;

    public LayerMask la;

    public Image moveImg; // 버튼 대신에 움직여줄 이미지 

    float checkStandardDist = 15f;
    public float movedDist;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if(movedDist < checkStandardDist)
            {
                movedDist = Vector3.Distance(moveImg.GetComponent<RectTransform>().anchoredPosition, Vector3.zero);
                InvadeManager.Instance.AddAct(actData.actType, actData.monsterType);
            }
        });
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
        moveImg.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        moveImg.transform.position = Input.mousePosition;
    }

}
