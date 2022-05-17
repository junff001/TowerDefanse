using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecordWaveBox
{
    public List<RecordBase> recordLog = new List<RecordBase>();
}

public class RecordManager : Singleton<RecordManager>
{
    public static bool isRecord { get; private set; } = false;
    public float currentTime { get; private set; } = 0f;

    public List<RecordWaveBox> recordBox = new List<RecordWaveBox>();

    [ContextMenu("Test")]
    public void Test()
    {
        RecordTowerUpgrade recordSegment = new RecordTowerUpgrade(new TowerData());

        AddRecord(recordSegment);
    }

    private void Start()
    {
        StartRecord();
    }

    private void Update()
    {
        if(isRecord)
        {
            currentTime += Time.deltaTime;
        }
    }

    public void StartRecord()
    {
        isRecord = true;
    }

    public void EndRecord()
    {
        isRecord = false;
        currentTime = 0;
    }

    public void AddRecord(RecordBase recordSegment)
    {
        if(recordBox.Count < GameManager.Instance.Wave)
        {
            recordBox.Add(new RecordWaveBox());
        }

        recordBox[GameManager.Instance.Wave - 1].recordLog.Add(recordSegment);
    }
}
