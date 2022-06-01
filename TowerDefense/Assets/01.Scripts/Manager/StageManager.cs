using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    public List<StageData> stageDatas = new List<StageData>();
    public Button[] stageBtns;

    public StageData selectedStage = null;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        object[] datas = Resources.LoadAll("StageDatas");
        for (int i = 0; i < datas.Length; i++)
        {
            stageDatas.Add(datas[i] as StageData);
        }

        for (int i = 0; i < stageDatas.Count; i++)
        {
            stageBtns[i].onClick.AddListener(() =>
            {
                int stageIdx = i - 1;
                selectedStage = stageDatas[stageIdx];
                SceneManager.LoadScene("JuhyeongScene");
            });
        }
    }


    public void SetStageDatas(StageData stageData)
    {
        Managers.Build.SetTilemap(stageData.tilemap);
        Managers.Game.SetWaypoints(stageData.waypointsParent);
        Managers.Wave.waveSO = stageData.waveSO;
        

        //여기서 스테이지 타워 데이터, 내가 가져온 몹 데이터 기반으로 버튼들 초기화 해주고
        //나머지는 매니저나 씬 자체에서 알아서 관리.
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //StageDataSO stageData = selectedStage;
        SetStageDatas(selectedStage);
    }
}
