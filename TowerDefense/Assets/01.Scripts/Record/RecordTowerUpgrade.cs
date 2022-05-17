using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordTowerUpgrade : RecordBase
{
    // ���⿡ Ÿ���� ���õ� ���� �ۼ�
    public TowerData towerData; // ?? �̰� �ӽ÷� �ϴ�

    public RecordTowerUpgrade(TowerData _towerData)
    {
        recordedTime = RecordManager.Instance.currentTime;
        recordType = eRecordType.TOWER_UPGRADE;

        towerData = _towerData;
    }
}
