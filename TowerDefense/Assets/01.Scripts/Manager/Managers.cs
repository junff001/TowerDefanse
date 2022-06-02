using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    UIManager _ui = new UIManager();
    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    PoolManager _pool = new PoolManager();
    RecordManager _record = new RecordManager();
    LoadingSceneManager _loadScene = new LoadingSceneManager();

    public static UIManager UI { get { return Instance._ui; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static RecordManager Record { get { return Instance._record; } }
    public static LoadingSceneManager LoadScene { get { return Instance._loadScene; } }

    public static bool IsInitCompleted = false;

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

    void Start()
    {
         Init();
    }

    void Update()
    {
        // 각 매니저들마다 업데이트

        s_instance._record.Update();
    }

    static void Init()
    {
        if (s_instance == null && IsInitCompleted == false)
        {
            IsInitCompleted = true;
            GameObject obj = GameObject.Find("@Management");
            if (obj == null)
            {
                Debug.LogError("\"@Management\" 이름을 가진 오브젝트가 Scene에 있어야합니다.");
                return;
            }

            Transform myObj = obj.transform.Find("@Manager");

            if(myObj == null)
            {
                Debug.LogError("\"@Management\" 오브젝트 안에 \"@Manager\" 이름을 가진, Managers 컴포넌트가 있는 오브젝트가 Scene에 있어야합니다.");
                return;
            }

            s_instance = myObj.GetComponent<Managers>();

            //초기화들
            s_instance._ui.Init();
            s_instance._sound.Init();
            Transform poolObjectBox = obj.transform.Find("@Pool");
            s_instance._pool.Init(poolObjectBox);
            s_instance._loadScene.Init();

            s_instance._gold = obj.transform.Find("@Gold").GetComponent<GoldManager>();
            s_instance._game = obj.transform.Find("@Game").GetComponent<GameManager>();
            s_instance._wave = obj.transform.Find("@Wave").GetComponent<WaveManager>();
            s_instance._build = obj.transform.Find("@Build").GetComponent<BuildManager>();
            s_instance._invade = obj.transform.Find("@Invade").GetComponent<InvadeManager>();
        }
    }

    public static void Clear()
    {
        // ...
        Sound.Clear();
        Pool.Clear();
    }
}