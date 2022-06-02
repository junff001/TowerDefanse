using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundSide : MonoBehaviour
{
    [SerializeField]
    float roundness = 1.0f;

    Material runtimeMaterial;


    void Awake()
    {
        this.runtimeMaterial = Instantiate(GetComponent<Image>().material);
        GetComponent<Image>().material = this.runtimeMaterial;
        Rect rect = GetComponent<Image>().rectTransform.rect;
        float halfWidth = rect.width * 0.5f;
        float radius = Mathf.Abs(this.roundness * halfWidth);

        this.runtimeMaterial.SetFloat("_Width", rect.width);
        this.runtimeMaterial.SetFloat("_Height", rect.height);
        this.runtimeMaterial.SetFloat("_Radius", radius);
    }
}
