using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBarrel : EnemyBase
{
    Vector3 direction = Vector3.zero;

    [SerializeField] private float timerMax = 3f; // 날아가는 시간?
    private float timerCurrent = 0f; // 현재 날아가는 중
    public float speed;


    protected override void Awake()
    {
        // 그냥 아무것도 안하게.
    }

    protected override void Start()
    {
        direction = targetPos - transform.position;
        speed = direction.x / timerMax; // 가야 하는 거리를 총 시간으로 빼 봤음..
    }

    protected override void Update()
    {
        timerCurrent += Time.deltaTime * speed;
        Shoot();

        if (timerMax <= timerCurrent) OnArrivedTartgetPoint(); // curTime이 더 커지면 목적지까지 이동한거임.
    }

    public void Shoot()
    {
        Vector3 z = Vector3.forward;
        Vector3 curve = Vector3.Cross(direction, z);
        Vector3 pos = transform.position + direction / 2;
        Vector3 result = pos + curve.normalized * Vector3.Distance(targetPos, transform.position) / 2;
        transform.position = BezierCurves(transform.position, result, targetPos, timerCurrent / timerMax);
    }

    Vector2 BezierCurves(Vector2 startPos, Vector2 curve, Vector2 endPos, float t)
    {
        Vector2 lerp1 = Vector2.Lerp(startPos, curve, t);
        Vector2 lerp2 = Vector2.Lerp(curve, endPos, t);

        return Vector2.Lerp(lerp1, lerp2, t);
    }

    public void OnArrivedTartgetPoint()
    {
        Vector3Int destPos = Vector3Int.FloorToInt(targetPos); // 목표 지점을 int로 바꿔주고,
        destPos = Managers.Build.map.tilemap.WorldToCell(destPos);

        // 고블린 소환
        EnemyBase goblin = Instantiate(Managers.Wave.enemyDic[Define.MonsterType.Goblin], destPos, Quaternion.identity); // 고블린 소환..
        Managers.Wave.aliveEnemies.Add(this);
        goblin.InitEnemyData();
        goblin.SetStartWaypoint();


        // 대충 이펙트 던져주고
        Managers.Wave.aliveEnemies.Remove(this);
        Destroy(this.gameObject);
    }
}
