using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordTowerSale : RecordBase
{
    public int towerIdx;

    public RecordTowerSale(int _towerIdx)
    {
        recordedTime = Managers.Record.currentTime;
        recordType = eRecordType.TOWER_SALE;

        towerIdx = _towerIdx;
    }
}