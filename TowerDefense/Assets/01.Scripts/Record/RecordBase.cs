using System;

public enum eRecordType
{
    TOWER_PLACE,
    TOWER_UPGRADE,
    // ��ȭ�� �ൿ�� �߰�.
}

[Serializable]
public class RecordBase
{
    public float recordedTime = default;
    public eRecordType recordType;
}
