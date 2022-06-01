using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecordWaveBox
{
    public List<RecordBase> recordLog = new List<RecordBase>();
}

public class RecordManager
{
    public static bool isRecord { get; private set; } = false;
    public float currentTime { get; private set; } = 0f;

    public List<RecordWaveBox> recordBox = new List<RecordWaveBox>();

    public void Update()
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
        if (Managers.Wave.GameMode == Define.GameMode.DEFENSE)
        {
            recordBox[Managers.Wave.Wave - 1].recordLog.Add(recordSegment);
        }
    }
}
