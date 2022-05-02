using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

public class Mouse : MonoBehaviour
{
    private static Ray ray = default;
    private static RaycastHit hit = default;
    private static Camera mainCam = null;
    
    public static Vector2 position { get; set; } = Vector2.zero;       // 마우스 위치
    public static bool isPressLeftClick { get; set; } = false;         // 마우스 좌클릭
    public static bool isInteractionTerm { get; set; } = false;        // 마우스 상호작용 텀

    private static float interactionTerm = 0f;
   
    [SerializeField] private static float interactionTermTime = 0.1f;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        RaycastHit();

        if (isPressLeftClick)
        {
            isInteractionTerm = true;
        }
        if (isInteractionTerm)
        {
            MouseInteractionTerm(interactionTermTime);
        }

        Debug.Log(interactionTerm);
    }

    // 마우스 커서 포지션 잡는 함수
    public void MousePosition(InputAction.CallbackContext context)
    {
        position = context.ReadValue<Vector2>();
    }

    // 마우스 좌클릭 값을 저장하는 함수
    public void MouseLeftClick(InputAction.CallbackContext context) // Only Press
    {
        if (!isInteractionTerm) // 상호작용 텀이 아니라면
        {
            isPressLeftClick = context.ReadValueAsButton() && context.started;
        }
    }

    // 마우스 클릭 상호작용 시 약간에 텀을 주어 로직 실행 순서를 맞추는 함수
    static void MouseInteractionTerm(float termTime)
    {
        interactionTerm -= Time.deltaTime;

        if (interactionTerm <= 0)
        {
            interactionTerm = 0f;
            isInteractionTerm = false;
        }

        interactionTerm = termTime;
    }

    // 마우스 클릭을 통한 이벤트 실행
    public static void ClickToAction(Action action)
    {
        action.Invoke();
        MouseInteractionTerm(interactionTermTime);
    }

    // 마우스 커서의 레이캐스트를 반환하는 함수
    public static RaycastHit RaycastHit()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // UI 가 있나없나 체크
        {
            ray = mainCam.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                return hit;
            }
            else
            {
                return default;
            } 
        }
        else
        {
            return default;
        }
    }
}
