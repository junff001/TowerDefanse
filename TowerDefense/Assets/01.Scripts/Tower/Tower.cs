using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TowerData
{
    public int Level;
    public int HP;
    public float OffensePower;
    public float AttackSpeed;
    public float AttackRange;
}

public class Tower : MonoBehaviour
{
    private TowerData towerData = new TowerData();              // 인스턴스 타워 정보
                                                                
    [SerializeField] private TowerSO towerSO = null;            // 실질적 타워 정보
    [SerializeField] private Sprite towerLook1 = null;          // 타워 외형 1
    [SerializeField] private Sprite towerLook2 = null;          // 타워 외형 2
                                                                
    public GameObject attackRangeObj { get; set; } = null;      // 공격 범위 오브젝트
    public bool canInteraction { get; set; } = false;           // 상호작용 가능값
                                                                
    private int cardLevel = 0;                                  // 카드 레벨
    private int attackTargetCount = 1;                          
    private int blockTargetCount = 1;                           
    private bool isInteractionTerm = false;                     
    private LayerMask enemyMask = default;                      // 적을 분별하는 마스크
    private Collider2D[] enemies = null;                        // 공격 범위이 안에 있는 적들
    public SpriteRenderer spriteRenderer { get; set; } = null;              


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        InitTowerData();

        StartCoroutine(Rader());
        StartCoroutine(OnAttack());
    }

    void InitTowerData()
    {
        towerData.HP = towerSO.HP;
        towerData.Level = towerSO.Level;
        towerData.OffensePower = towerSO.OffensePower;
        towerData.AttackSpeed = towerSO.AttackSpeed;
        towerData.AttackRange = towerSO.AttackRange;
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
        return Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange, targetMask);
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
                    Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                }
            }

            yield return new WaitForSeconds(1f / towerData.AttackSpeed); // 공속만큼 기다리고,
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
        if (towerData.Level >= 3)  // 3렙이면 업그레이드 안함
        {
            return;
        }
        else
        {
            towerData.Level++;
           
            switch (towerData.Level)
            {
                case 2:    // 2레벨
                {
                    spriteRenderer.sprite = towerLook1;
                }
                break;
                case 3:    // 3레벨
                {
                    spriteRenderer.sprite = towerLook2;
                }
                break;
                default:
                    break;
            }
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
            Gizmos.DrawWireSphere(transform.position, towerData.AttackRange);
            Gizmos.color = Color.white;
        }
    }
#endif
}
