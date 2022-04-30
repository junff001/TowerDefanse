using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float attackRange = 0f;          // 공격 범위
    [SerializeField] private float offensePower = 0f;         // 공격력
    [SerializeField] private float attackSpeed = 1f;          // 공격 속도
    [SerializeField] private Sprite towerLook1 = null;        // 타워 외형 1
    [SerializeField] private Sprite towerLook2 = null;        // 타워 외형 2
    [SerializeField] private float enableTermTime = 0f;       // 생성되고 기다리는 간격

    public GameObject attackRangeObj { get; set; } = null;    // 공격 범위 오브젝트
    public bool canInteraction { get; set; } = false;         // 상호작용 가능값

    private int cardLevel = 0;                                // 카드 레벨
    private int attackTargetCount = 1;
    private int blockTargetCount = 1;
    private bool isEnableTerm = false;
    private LayerMask enemyMask = default;                    // 적을 분별하는 마스크
    private Collider2D[] enemies = null;                      // 공격 범위이 안에 있는 적들
    private SpriteRenderer spriteRenderer = null;

    void OnEnable()
    {
        StartCoroutine(EnableTerm(enableTermTime));
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    { 
        StartCoroutine(Rader());
        StartCoroutine(OnAttack());
    }

    // 생성되고 일정시간 텀이 발생하게끔 하는 함수
    // 이 텀이 없으면 타워 생성과 업그레이드가 같이 실행됨
    IEnumerator EnableTerm(float time)
    {
        isEnableTerm = true;
        yield return new WaitForSeconds(time);
        isEnableTerm = false;
    }

    // 0.1초 텀을 두고 공격 범위 체크 처리
    IEnumerator Rader()
    {
        enemyMask = LayerMask.GetMask("Enemy");
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            enemies = EnemyRader(enemyMask);
        }
    }

    // 공격 범위 처리 함수
    Collider2D[] EnemyRader(LayerMask targetMask)
    {
        return Physics2D.OverlapCircleAll(transform.position, attackRange, targetMask);
    }  

    // 공격 실행 함수
    IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies != null && enemies.Length > 0);

            List<EnemyBase> enemyList = new List<EnemyBase>();
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                    enemyList.Add(enemies[i].GetComponent<EnemyBase>());
            }

            enemyList.Sort((x, y) => x.movedDistance.CompareTo(y.movedDistance));

            for (int i = 0; i < enemies?.Length; i++) // 공격 
            {
                if (i >= attackTargetCount)
                    break; // 공격 가능 대상 수만큼 때렸으면 그만 때리기
                if (enemies[i] != null)
                {
                    Attack(offensePower, enemies[i].GetComponent<HealthSystem>());
                }
            }

            yield return new WaitForSeconds(1f / attackSpeed); // 공속만큼 기다리고,
        }
    }

    // 공격 로직 함수 (임시 원거리)    
    public virtual void Attack(float power, HealthSystem enemy)
    {
        Bullet bullet = PoolManager.GetItem<Bullet>();

        bullet.transform.position = transform.position;
        bullet.target = enemy.transform;
        bullet.bulletDamage = power;
    }

    // 타워를 업그레이드하는 함수
    public void TowerUpgrade()
    {
        if (!isEnableTerm)
        {
            spriteRenderer.sprite = towerLook1;
        }
    }

    // 공격 범위를 표시하는 함수
    public void AttackRangeActive()
    {

    }

    // 공격 범위 기즈모 표시
#if UNITY_EDITOR 
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.white;
        }
    }
#endif
}
