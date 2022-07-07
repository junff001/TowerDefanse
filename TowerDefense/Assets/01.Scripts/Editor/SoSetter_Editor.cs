using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(SoSetter))]
[CanEditMultipleObjects] // only if you handle it properly

public class SoSetter_Editor : Editor
{
    SoSetter soSetter = null;

    void OnEnable()
    {
        //Character 컴포넌트를 얻어오기
        soSetter = (SoSetter)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("SetSpawnCost"))
        {
            soSetter.SetEnemySOsSpawnCost();
        }

    }
}