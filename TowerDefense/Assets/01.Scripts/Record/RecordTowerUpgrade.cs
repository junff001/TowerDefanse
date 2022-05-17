using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordTowerUpgrade : RecordBase
{
    // 여기에 타워에 관련된 변수 작성
    public TowerData towerData; // ?? 이건 임시로 일단

    public RecordTowerUpgrade(TowerData _towerData)
    {
        recordedTime = RecordManager.Instance.currentTime;
        recordType = eRecordType.TOWER_UPGRADE;

        towerData = _towerData;
    }
}
