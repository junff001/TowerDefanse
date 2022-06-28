using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageManager
{
    public List<StageData> stageDatas;
    public StageData selectedStage = null;

    public void Init()
    {
        stageDatas = new List<StageData>();

        StageData[] datas = Resources.LoadAll<StageData>("StageDatas");
        for (int i = 0; i < datas.Length; i++)
        {
            stageDatas.Add(datas[i]);
        }
    }

    public void OnSceneLoaded()
    {
        Debug.Log("OnSceneLoaded");
        SetStageDatas(selectedStage);
    }

    public void SetTargetStage(MapInfoSO mapInfo)
    {
        selectedStage = stageDatas.Find(x => x.mapInfoSO == mapInfo);
    }

    public void SetStageDatas(StageData stageData)
    {
        StageData obj = MonoBehaviour.Instantiate(stageData);
        Vector3 makeMapPos = new Vector3(-(obj.map.tilemap.size.x / 2f), -(obj.map.tilemap.size.y / 2f), 0);
        obj.transform.position = makeMapPos;

        Managers.Game.waypointsParent = obj.waypointsParent;
        Managers.Game.pointLists = obj.pointLists;
        Managers.Wave.mapInfoSO = obj.mapInfoSO;
        Managers.Build.map = obj.map;
        Managers.Build.Init();
    }
}
