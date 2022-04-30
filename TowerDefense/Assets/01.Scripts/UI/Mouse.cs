using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Mouse : MonoBehaviour
{
    private static Ray ray = default;
    private static RaycastHit hit = default;
    private static Camera mainCam = null;

    public static Vector2 position { get; set; } = Vector2.zero;       // 마우스 위치
    public static bool isPressLeftClick { get; set; } = false;         // 마우스 좌클릭

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        RaycastHit();
    }

    // 마우스 커서 포지션 잡는 함수
    public void MousePosition(InputAction.CallbackContext context)
    {
        position = context.ReadValue<Vector2>();
    }

    // 마우스 좌클릭 값을 저장하는 함수
    public void MouseLeftClick(InputAction.CallbackContext context) // Only Press
    {
        isPressLeftClick = context.ReadValueAsButton() && context.started;
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
