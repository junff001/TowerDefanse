using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 0f;                             // 이동 속도
    [SerializeField] protected ParticleSystem hitEffect = null;              // 타격 이펙트

    public Transform Target { get; set; }                                  // 목표물
    public int BulletDamage { get; set; } = 0;                               // 데미지
    public bool IsShoot { get; set; } = false;
    public Define.PropertyType PropertyType = Define.PropertyType.NONE;

    protected Vector2 startPos = Vector2.zero;
    protected Vector2 targetPos = Vector2.zero;

    [SerializeField] protected float curTime;
    [SerializeField] protected float maxTime; // 얘는 거리에 따라 바뀌는 투사체 날아가는 시간
    protected float _maxTime = 0f; // 얘는 최대시간

    private void Awake()
    {
        _maxTime = maxTime;
    }

    public void OnEnable()
    {
        transform.SetParent(Managers.Pool.poolInitPos);
    }

    public void OnDisable()
    {
        IsShoot = false;
    }

    public virtual void Init(TowerData towerData, Transform enemyTrm)
    {
        Target = enemyTrm;
        startPos = transform.position;
        targetPos = enemyTrm.position;
        PropertyType = towerData.Property;
        BulletDamage = towerData.OffensePower;
        curTime = 0;
    }

    public virtual void Update()
    {
        if (IsShoot)
        {
            if (Target != null)
            {
                if (IsCollision())
                {
                    CollisionEvent();
                }
                Shoot();
            }
            else
            {
                gameObject.SetActive(false);
            }

            curTime += Time.deltaTime;
        }
    }

    // 기본 유도탄
    public virtual void Shoot()
    {
        transform.position = StraightShoot(startPos, targetPos, curTime / maxTime);
    }

    // 거리 충돌 체크
    public virtual bool IsCollision()
    {
        return (Vector2.Distance(transform.position, targetPos) <= 0.1f ||
            Vector2.Distance(transform.position, Target.transform.position) <= 0.2f) && IsShoot;
    }

    // 충돌 시 발생 로직
    public virtual void CollisionEvent()
    {
        if (Target != null)
        {
            Target.gameObject.GetComponent<HealthSystem>().TakeDamage(BulletDamage, PropertyType);

            var ps = Instantiate(hitEffect);
            ps.transform.position = Target.position;
            ps.Play();
        }
        IsShoot = false;
        gameObject.SetActive(false);
    }

    protected Vector2 GetExpectPos(EnemyBase enemy, float maxTime)
    {
        //현재 목표 웨이포인트
        Vector2 destPoint = Managers.Game.wayPoints[enemy.CurrentWayPointIndex].position;
        //이동하고 있는 방향
        Vector2 movingDir = (destPoint - (Vector2)enemy.transform.position).normalized;
        // maxTime이 지난 후, 적의 위치
        Vector2 enemyPosLaterMaxTime = (Vector2)enemy.transform.position + (movingDir * enemy.enemyData.MoveSpeed * maxTime);
        // 예상 위치
        
        // 목표지점을 넘어선 경우인데, 다음 웨이포인트가 존재한다면.
        if (CheckPass(destPoint, movingDir, enemyPosLaterMaxTime) && Managers.Game.wayPoints.Count > (enemy.CurrentWayPointIndex + 1))
        {
            return GetLerpPoint(enemy, enemyPosLaterMaxTime);
        }
        else
        {
            return enemyPosLaterMaxTime;
        }
    }

    public Vector2 GetLerpPoint(EnemyBase enemy, Vector2 expectPos) // 원래 목적지를 넘어갔을때, 내가 지나간 만큼을 빼주는.. 그런걸 해보자
    {
        //지점을 지나면서 인덱스가 1 추가 됐을거임.
        Vector2 dest = Managers.Game.wayPoints[enemy.CurrentWayPointIndex- 1].position;
        Vector2 next = Managers.Game.wayPoints[enemy.CurrentWayPointIndex].position;
        Vector2 moveDir = (next - dest).normalized;

        float overDist = Vector2.Distance(dest, expectPos);

        if (moveDir.x != 0)
        {
            if (moveDir.x > 0) dest.x += overDist;
            else dest.x -= overDist;
        }
        else
        {
            if (moveDir.y > 0) dest.y += overDist;
            else dest.y -= overDist;
        }

        return expectPos;
    }



    protected bool CheckPass(Vector2 destPoint, Vector2 movingDir, Vector2 enemyPosLaterMaxTime)
    {
        if (movingDir.x != 0) // 옆으로 이동중
            return CheckPassedPoint(movingDir.x, enemyPosLaterMaxTime.x, destPoint.x);
        else
            return CheckPassedPoint(movingDir.y, enemyPosLaterMaxTime.y, destPoint.y);


        //이동중인 축의 값, 예상 위치 축 값, 목적지 축 값
        bool CheckPassedPoint(float movingAxisValue, float expect, float dest)
        {
            if (movingAxisValue > 0 && false == expect >= dest) return true;
            else if (movingAxisValue < 0 && false == expect <= dest) return true;
            return false;
        }
    }

    protected Vector2 BezierCurves(Vector2 startPos, Vector2 curve, Vector2 endPos, float t)
    {
        Vector2 lerp1 = Vector2.Lerp(startPos, curve, t);
        Vector2 lerp2 = Vector2.Lerp(curve, endPos, t);

        return Vector2.Lerp(lerp1, lerp2, t);
    }

    protected Vector2 StraightShoot(Vector2 startPos, Vector2 targetPos, float t)
    {
        return Vector2.Lerp(startPos, targetPos, t);
    }
}
