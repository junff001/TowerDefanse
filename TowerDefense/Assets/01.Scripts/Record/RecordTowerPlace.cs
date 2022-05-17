using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordTowerPlace : RecordBase
{
    public Vector2 towerPos;

    // ���⿡ Ÿ���� ���õ� ���� �ۼ�
    public TowerData towerData; // �̰� �ӽ÷� �ϴ�

    public RecordTowerPlace(Vector3 _towerPos, TowerData _towerData)
    {
        recordedTime = RecordManager.Instance.currentTime;
        recordType = eRecordType.TOWER_PLACE;

        towerPos = _towerPos;
        towerData = _towerData;
    }
}
