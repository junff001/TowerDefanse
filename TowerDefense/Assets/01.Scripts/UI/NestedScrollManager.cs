using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class NestedScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;

    int size;
    float[] pos;
    float distance, curPos ,targetPos;
    bool isDrag;
    int targetIndex =0;


    public GameObject selectTowerPanel;
    public Transform parent;
    public GameObject towerCard;
    public GameObject emteCard;
    public List<GameObject> towers = new List<GameObject>();
    public Button exitBtn;

    void Start()
    {

        exitBtn.onClick.AddListener(() =>
        {
            //타워 패널 띄움
            selectTowerPanel.GetComponent<RectTransform>().DOMoveY(Screen.height * 2, 0.5f);

        });
        List<Dictionary<string, object>> data = CSVReader.Read("TowerList.csv");
        size = data.Count + 1;
        pos = new float[size];

        //거리에 따라 0~1인 pos대입
        distance = 1f / (size-1);
        Instantiate(emteCard, parent);

        for (int i = 0; i < size; i++)
        {
           GameObject tower   = Instantiate(towerCard, parent);
            towers.Add(tower);
            pos[i] = distance * i;
        }
        Instantiate(emteCard, parent);

        towers[targetIndex].transform.DOScale(1.2f, 0.5f);
    }

    float SetPos()
    {
        //절반거리를 기준으로 가까운 위치를 반환
        for (int i = 0; i < size; i++)
        {
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return  pos[i];
            }
        }
        return 0;
    }


    //드래그 시작
    public void OnBeginDrag(PointerEventData eventData) =>  curPos = SetPos();

    //드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    //드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;

        targetPos = SetPos();
        for (int i = 0; i < size; i++)
        {
            towers[i].transform.DOScale(1, 0.5f);
            if (i == targetIndex)
            {
                towers[targetIndex].transform.DOScale(1.2f, 0.5f);
            }
        }

        //절반거리를 넘지 않아도 마우스를 빠르게 이동하면
        if (curPos == targetPos)
        {
            print(eventData.delta.x);
            //스크롤이 왼쪽으로 빠르게 이동시 목표가 하나 감소
            if (eventData.delta.x > 18 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;

            }
            //스크롤이 오른쪽으로 빠르게 이동시 목표가 하나 증가
            else if (eventData.delta.x < 18 && curPos + distance <= 1.0f)
            {
                ++targetIndex;
                targetPos = curPos + distance;
            }

        }

    }

    void Update()
    {
        if (!isDrag)
        {
            //나중에 여기서 크기 크고 작아지는 거 넣어도 될 듯
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);

        }
    }
}
