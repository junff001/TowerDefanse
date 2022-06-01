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
        if (Input.mouseScrollDelta.y > 0) 
        {
            addSize = Time.deltaTime * zoomSpeed;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            addSize = -(Time.deltaTime * zoomSpeed);
        }

        cinemachine.m_Lens.OrthographicSize = Mathf.Clamp(cinemachine.m_Lens.OrthographicSize + addSize, minSize, maxSize);
    }
}
