using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [SerializeField] private float scrollDegree;
    [SerializeField] private float maxSize;

    void Update()
    {
        cinemachine.m_Lens.OrthographicSize = Mathf.Clamp(Input.mouseScrollDelta.y * scrollDegree, 5, maxSize);
    }
}
