using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    public static Managers Instance
    {
        get
        {
            Init();
            return s_instance;
        }
    }

    static GameObject managementObj;
    static GameObject myObj;

    #region Managers

    UIManager _ui = new UIManager();
    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    PoolManager _pool = new PoolManager();
    RecordManager _record = new RecordManager();
    LoadingSceneManager _loadScene = new LoadingSceneManager();
    StageManager _stage = new StageManager();

    public static UIManager UI { get { return Instance._ui; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static RecordManager Record { get { return Instance._record; } }
    public static LoadingSceneManager LoadScene { get { return Instance._loadScene; } }
    public static StageManager Stage { get { return Instance._stage; } }

    GoldManager _gold;
    GameManager _game;
    WaveManager _wave;
    BuildManager _build;
    InvadeManager _invade;

    public static GoldManager Gold { get { return Instance._gold; } }
    public static GameManager Game { get { return Instance._game; } }
    public static WaveManager Wave { get { return Instance._wave; } }
    public static BuildManager Build { get { return Instance._build; } }
    public static InvadeManager Invade { get { return Instance._invade; } }

    #endregion

    void Start()
    {
         Init();
    }

    void Update()
    {
        // 각 매니저들마다 업데이트

        s_instance._record.Update();
        s_instance._ui.Update();
    }

    static void Init()
    {
        managementObj = GameObject.Find("@Management");
        myObj = GameObject.Find("@Manager");

        if (s_instance == null)
        {
            if (managementObj == null)
            {
                managementObj = new GameObject("@Management");
            }

            if (myObj == null)
            {
                myObj = new GameObject("@Manager");
                myObj.AddComponent<Managers>();
            }

            s_instance = myObj.GetComponent<Managers>();
            DontDestroyOnLoad(myObj);

            //초기화들
            s_instance._ui.Init();
            s_instance._sound.Init();
            s_instance._loadScene.Init();
            s_instance._stage.Init();
            Transform poolObjectBox = myObj.transform.Find("@Pool");

            if (poolObjectBox == null)
            {
                poolObjectBox = new GameObject("@Pool").transform;
                poolObjectBox.SetParent(myObj.transform);
            }

            s_instance._pool.Init(poolObjectBox);

            SceneManager.sceneLoaded += OnSceneChanged;

            s_instance._gold = managementObj.transform.Find("@Gold")?.GetComponent<GoldManager>();
            s_instance._game = managementObj.transform.Find("@Game")?.GetComponent<GameManager>();
            s_instance._wave = managementObj.transform.Find("@Wave")?.GetComponent<WaveManager>();
            s_instance._build = managementObj.transform.Find("@Build")?.GetComponent<BuildManager>();
            s_instance._invade = managementObj.transform.Find("@Invade")?.GetComponent<InvadeManager>();
        }
    }

    public static void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Managers.OnSceneChanged");
        managementObj = GameObject.Find("@Management");

        if (managementObj != null)
        {
            s_instance._gold = managementObj.transform.Find("@Gold")?.GetComponent<GoldManager>();
            s_instance._game = managementObj.transform.Find("@Game")?.GetComponent<GameManager>();
            s_instance._wave = managementObj.transform.Find("@Wave")?.GetComponent<WaveManager>();
            s_instance._build = managementObj.transform.Find("@Build")?.GetComponent<BuildManager>();
            s_instance._invade = managementObj.transform.Find("@Invade")?.GetComponent<InvadeManager>();
            Record.recordBox.Clear();
        }

        Stage.OnSceneLoaded();
    }

    public static void Clear()
    {
        // ...
        Sound.Clear();
        Pool.Clear();
    }
}