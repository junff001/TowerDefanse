using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rigid { get; set; }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); 
    }
}
