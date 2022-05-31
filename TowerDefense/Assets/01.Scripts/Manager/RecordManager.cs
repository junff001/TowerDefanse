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
        if (WaveManager.Instance.GameMode == eGameMode.DEFENSE)
        {
            while(recordBox.Count < WaveManager.Instance.Wave)
            {
                recordBox.Add(new RecordWaveBox());
            }

            recordBox[WaveManager.Instance.Wave - 1].recordLog.Add(recordSegment);
        }
    }
}
