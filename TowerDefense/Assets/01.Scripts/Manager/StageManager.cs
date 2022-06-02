using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StageManager
{
    public List<StageData> stageDatas;
    public StageData selectedStage = null;

    public void Init()
    {
        stageDatas = new List<StageData>();
        SceneManager.sceneLoaded += OnSceneLoaded;

        StageData[] datas = Resources.LoadAll<StageData>("StageDatas");
        for (int i = 0; i < datas.Length; i++)
        {
            stageDatas.Add(datas[i]);
        }
    }

    public void SetTargetStage(int stageNum)
    {
        selectedStage = stageDatas[stageNum];
    }

    public void SetStageDatas(StageData stageData)
    {
        StageData obj = MonoBehaviour.Instantiate(stageData);
        Vector3 makeMapPos = new Vector3(-(obj.map.tilemap.size.x / 2f), -(obj.map.tilemap.size.y / 2f), 0);
        obj.transform.position = makeMapPos;

        Managers.Game.waypointsParent = obj.waypointsParent;
        Managers.Wave.waveSO = obj.waveSO;
        Managers.Build.map = obj.map;


        //여기서 스테이지 타워 데이터, 내가 가져온 몹 데이터 기반으로 버튼들 초기화 해주고
        //나머지는 매니저나 씬 자체에서 알아서 관리.
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //StageDataSO stageData = selectedStage;
        SetStageDatas(selectedStage);
    }
}
