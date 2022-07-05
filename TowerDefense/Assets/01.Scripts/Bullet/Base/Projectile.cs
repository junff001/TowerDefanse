using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected ParticleSystem hitEffect;             
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;  

    [HideInInspector] public Transform Target;                               
    [HideInInspector] public bool IsShoot = false;
    [HideInInspector] public BuffBase Buff;

    protected Vector2 startPos;
    protected Vector2 firePos;
    protected Vector2 targetPos;
    public Vector2 TargetPos => targetPos;

    protected LayerMask opponentLayer;

    [SerializeField] protected float curTime;
    [SerializeField] protected float maxTime; // 얘는 거리에 따라 바뀌는 투사체 날아가는 시간
    protected float _maxTime = 0f; // 얘는 최대시간

    private void Awake()
    {
        _maxTime = maxTime;
    }

    public virtual void InitProjectileData(Transform target, BuffBase buff, LayerMask opponent)
    {
        Target = target;
        startPos = transform.position;
        targetPos = target.position;
        Buff = buff;
        curTime = 0;
        opponentLayer = opponent;
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
        return curTime >= maxTime;
        // 유도탄은 
        // return (Vector2.Distance(transform.position, targetPos) <= 0.1f ||
        //    Vector2.Distance(transform.position, Target.transform.position) <= 0.2f) && IsShoot;
    }

    // 충돌 시 발생 로직
    public virtual void CollisionEvent()
    {
        if (Target != null)
        {
            Debug.Log("버프");
            Target.GetComponent<Enemy>().AddBuff(Buff);
            Target.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);

            var ps = Instantiate(hitEffect);
            ps.transform.position = Target.position;
            ps.Play();
        }
        IsShoot = false;
        transform.SetParent(Managers.Pool.poolInitPos);
        gameObject.SetActive(false);
    }

    protected Vector2 GetExpectPos(Enemy enemy, float maxTime)
    {
        Vector2 curPos = enemy.transform.position;
        float moveDistWhileMaxTime = enemy.enemyData.MoveSpeed * maxTime;

        return GetExpectPos(curPos, moveDistWhileMaxTime, enemy.CurrentWayPointIndex);
    }

    public Vector2 GetExpectPos(Vector2 startPos, float moveDistWhileMaxTime, int index)
    {
        Vector2 destPos = Managers.Game.wayPoints[index].position; // 현재 이동중인 목적지 인덱스

        float distToWaypoint = Vector2.Distance(startPos, destPos); // enemy 위치를 이 위치로 옮기고.
        if(moveDistWhileMaxTime > distToWaypoint) // n초간 목적지보다 더 멀리가면 
        {
            moveDistWhileMaxTime -= distToWaypoint; // 이동한 거리 빼주고 다음 인덱스도 넘어가는 체크하기.

            if(index + 1 < Managers.Game.wayPoints.Count)
            {
                return GetExpectPos(destPos, moveDistWhileMaxTime, ++index); // 한번 더 굴려!
            }
        }
        Vector2 moveDir = (destPos - startPos).normalized; // 안 넘어가니까 그냥 벡터 구하고
        return startPos += moveDistWhileMaxTime * moveDir; // 현재 위치에 방향 * 거리 곱해서 더해주고 리턴.
    }

    protected Vector2 BezierCurves(Vector2 startPos, Vector2 curvePos, Vector2 endPos, float t)
    {
        Vector2 lerp1 = Vector2.Lerp(startPos, curvePos, t);
        Vector2 lerp2 = Vector2.Lerp(curvePos, endPos, t);

        return Vector2.Lerp(lerp1, lerp2, t);
    }

    protected Vector2 StraightShoot(Vector2 startPos, Vector2 targetPos, float t)
    {
        return Vector2.Lerp(startPos, targetPos, t);
    }
}
