using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordTowerPlace : RecordBase
{
    public Vector2 towerPos;

    // 여기에 타워에 관련된 변수 작성
    public TowerSO towerSO; // 이건 임시로 일단

    public RecordTowerPlace(Vector3 _towerPos, TowerSO _towerSO)
    {
        recordedTime = Managers.Record.currentTime;
        recordType = eRecordType.TOWER_PLACE;

        towerPos = _towerPos;
        towerSO = _towerSO;
    }
}
