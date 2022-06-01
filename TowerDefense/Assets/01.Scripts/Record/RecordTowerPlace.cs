using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordTowerPlace : RecordBase
{
    public Vector2 towerPos;

    // ���⿡ Ÿ���� ���õ� ���� �ۼ�
    public TowerSO towerSO; // �̰� �ӽ÷� �ϴ�

    public RecordTowerPlace(Vector3 _towerPos, TowerSO _towerSO)
    {
        recordedTime = Managers.Record.currentTime;
        recordType = eRecordType.TOWER_PLACE;

        towerPos = _towerPos;
        towerSO = _towerSO;
    }
}
