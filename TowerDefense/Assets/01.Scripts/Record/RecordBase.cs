using System;

public enum eRecordType
{
    TOWER_PLACE,
    TOWER_UPGRADE,
    // 녹화할 행동을 추가.
}

[Serializable]
public class RecordBase
{
    public float recordedTime = default;
    public eRecordType recordType;
}
