using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [SerializeField] private float zoomSpeed = 5f;
    [Range(5,8)] 
    [SerializeField] private float maxSize;
    private const float minSize = 5f;
    private float addSize = 0f;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            PinchZoom();
        }
        MouseZoom();
    }

    void MouseZoom()
    {
        if (addSize > 0 && Input.mouseScrollDelta.y < 0) addSize = 0;
        if (addSize < 0 && Input.mouseScrollDelta.y > 0) addSize = 0;

        addSize += -Input.mouseScrollDelta.y * 0.5f;
        SmoothZoom();
    }

    void PinchZoom()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchZero.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;

        Zoom(difference * 0.01f);
    }

    void Zoom(float increment)
    {
        cinemachine.m_Lens.OrthographicSize = Mathf.Clamp(cinemachine.m_Lens.OrthographicSize + increment, minSize, maxSize); 
    }

    void SmoothZoom()
    {
        float camSize = cinemachine.m_Lens.OrthographicSize;
        float value = Mathf.Lerp(camSize, camSize + addSize, Time.deltaTime * zoomSpeed);
        addSize -= (value - camSize);
        cinemachine.m_Lens.OrthographicSize = Mathf.Clamp(value, minSize, maxSize);
    }
}
