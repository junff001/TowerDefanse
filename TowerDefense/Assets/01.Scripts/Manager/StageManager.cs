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
        BuildManager.Instance.SetTilemap(stageData.tileMap);
        GameManager.Instance.SetWaypoints(stageData.waypointsParent);
        WaveManager.Instance.waveSO = stageData.waveSO;
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StageDataSO stageData = selectedStage;
        SetStageDatas(selectedStage);
        Debug.Log("ㅎㅇ");
    }
}
