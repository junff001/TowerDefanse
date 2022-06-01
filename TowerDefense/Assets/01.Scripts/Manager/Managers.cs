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

    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    PoolManager _pool = new PoolManager();
    RecordManager _record = new RecordManager();

    GoldManager _gold;
    GameManager _game;

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static RecordManager Record { get { return Instance._record; } }

    public static GoldManager Gold { get { return Instance._gold; } }
    public static GameManager Game { get { return Instance._game; } }

    void Start()
    {
        Init();
    }

    void Update()
    {
        // �� �Ŵ����鸶�� ������Ʈ

        s_instance._record.Update();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject obj = GameObject.Find("@Management");
            if (obj == null)
            {
                Debug.LogError("\"@Management\" �̸��� ���� ������Ʈ�� Scene�� �־���մϴ�.");
                return;
            }

            Transform myObj = obj.transform.Find("@Manager");

            if(myObj == null)
            {
                Debug.LogError("\"@Management\" ������Ʈ �ȿ� \"@Manager\" �̸��� ����, Managers ������Ʈ�� �ִ� ������Ʈ�� Scene�� �־���մϴ�.");
                return;
            }

            s_instance = myObj.GetComponent<Managers>();

            //�ʱ�ȭ��
            s_instance._sound.Init();
            Transform poolObjectBox = obj.transform.Find("@Pool");
            s_instance._pool.Init(poolObjectBox);

            s_instance._gold = obj.transform.Find("@Gold").GetComponent<GoldManager>();
            s_instance._game = obj.transform.Find("@Game").GetComponent<GameManager>();
        }
    }

    public static void Clear()
    {
        // ...
        Pool.Clear();
    }
}