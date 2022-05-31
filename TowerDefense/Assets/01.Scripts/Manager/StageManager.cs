using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    public List<StageDataSO> stageDatas = new List<StageDataSO>();
    public Button[] stageBtns;

    public StageDataSO selectedStage = null;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        object[] datas = Resources.LoadAll("StageDatas");
        for (int i = 0; i < datas.Length; i++)
        {
            stageDatas.Add(datas[i] as StageDataSO);
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


    public void SetStageDatas(StageDataSO stageData)
    {
        BuildManager.Instance.SetTilemap(stageData.gridObj);
        GameManager.Instance.SetWaypoints(stageData.waypointsParent);
        WaveManager.Instance.waveSO = stageData.waveSO;
        

        //여기서 스테이지 타워 데이터, 내가 가져온 몹 데이터 기반으로 버튼들 초기화 해주고
        //나머지는 매니저나 씬 자체에서 알아서 관리.
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //StageDataSO stageData = selectedStage;
        SetStageDatas(selectedStage);
    }
}
