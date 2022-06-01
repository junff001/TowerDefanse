using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [SerializeField] private float zoomSpeed;
    [Range(5,20)] [SerializeField] private float maxSize;
    private const float minSize = 5f;
    private float addSize = 0f;

    void Update()
    {
        if (Input.touchCount == 2)
        {
            PinchZoom();
        }
    }

    void MouseZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            addSize = Time.deltaTime * zoomSpeed;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            addSize = -(Time.deltaTime * zoomSpeed);
        }

        Zoom(addSize);
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
}
