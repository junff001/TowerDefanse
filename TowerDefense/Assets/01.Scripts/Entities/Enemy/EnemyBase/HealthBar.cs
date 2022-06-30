using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private int _healthAmountPerSeparator = 2; //체력 2당 바 한개
    [SerializeField]
    private float _barSize = 1.88f;
    [SerializeField]
    private Vector3 _sepSize = new Vector3(0.02f, 0.5f);

    private HealthSystem healthSystem;

    [SerializeField] private Transform healthBarTrm;
    [SerializeField] private Transform shieldBarTrm;

    private Transform _separator;

    private MeshFilter _sepMeshFilter;
    private Mesh _sepMesh;
    private MeshRenderer _sepMeshRenderer;

    private void Awake()
    {
        if (!healthSystem)
        {
            healthSystem = transform.parent.GetComponent<HealthSystem>();
        }
        _separator = transform.Find("SeparatorContainer/Separator");
        _sepMeshFilter = _separator.GetComponent<MeshFilter>();
        _sepMeshRenderer = _separator.GetComponent<MeshRenderer>();

        healthSystem.OnMaxHealed += (x) => CalcSeparator((int)x);
        healthSystem.OnDamaged += CallHealthSystemOnDamaged;
    }

    private void Start()
    {

        _sepMeshRenderer.sortingLayerName = "Default";
        _sepMeshRenderer.sortingOrder = 20;

        UpdateBar();    
        UpdateHealthBarVisible();
    }

    private void Update()
    {
        float xScale = healthSystem.transform.localScale.x;

        if (xScale > 0)
        {
            xScale /= xScale;
            xScale *= -1;
        }
        else
        {
            xScale /= xScale;
        }

        transform.localScale = new Vector3(-xScale, 1, 1);
    }

    private void CallHealthSystemOnDamaged()
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateBar()
    {
        healthBarTrm.localScale = new Vector3(healthSystem.GetAmountNormalized(eHealthType.HEALTH), 1, 1);
        shieldBarTrm.localScale = new Vector3(healthSystem.GetAmountNormalized(eHealthType.SHIELD), 1, 1);
    }

    private void UpdateHealthBarVisible()
    {
        if (healthSystem.IsFullValue(eHealthType.SHIELD) && healthSystem.IsFullValue(eHealthType.HEALTH))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void CalcSeparator(int value)
    {
        _sepMesh = new Mesh();

        int gridCnt = Mathf.FloorToInt(value / _healthAmountPerSeparator);

        Vector3[] vertices = new Vector3[(gridCnt - 1) * 4];
        Vector2[] uv = new Vector2[(gridCnt - 1) * 4];
        Color[] colors = new Color[vertices.Length];
        int[] triangles = new int[(gridCnt - 1) * 6];

        float barOneSize = _barSize / gridCnt;

        for (int i = 0; i < gridCnt - 1; i++)
        {
            Vector3 pos = new Vector3(barOneSize * (i + 1), 0, 0);

            int vIndex = i * 4;
            vertices[vIndex + 0] = pos + new Vector3(-_sepSize.x, -_sepSize.y);
            vertices[vIndex + 1] = pos + new Vector3(-_sepSize.x, +_sepSize.y);
            vertices[vIndex + 2] = pos + new Vector3(+_sepSize.x, +_sepSize.y);
            vertices[vIndex + 3] = pos + new Vector3(+_sepSize.x, -_sepSize.y);

            uv[vIndex + 0] = Vector2.zero;
            uv[vIndex + 1] = Vector2.up;
            uv[vIndex + 2] = Vector2.one;
            uv[vIndex + 3] = Vector2.right;

            int tIndex = i * 6;
            triangles[tIndex + 0] = vIndex + 0;
            triangles[tIndex + 1] = vIndex + 1;
            triangles[tIndex + 2] = vIndex + 2;
            triangles[tIndex + 3] = vIndex + 0;
            triangles[tIndex + 4] = vIndex + 2;
            triangles[tIndex + 5] = vIndex + 3;

        }
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.black;
        }

        _sepMesh.vertices = vertices;
        _sepMesh.uv = uv;
        _sepMesh.triangles = triangles;
        _sepMesh.colors = colors;

        _sepMeshFilter.mesh = _sepMesh;
    }
}
