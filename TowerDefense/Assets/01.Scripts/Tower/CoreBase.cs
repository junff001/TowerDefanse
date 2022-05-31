using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CoreBase : MonoBehaviour
{
    [SerializeField] private float gizmoHeight = 0f;

    public LayerMask enemyMask = default;                      // ?곸쓣 遺꾨퀎?섎뒗 留덉뒪??
    protected Collider2D[] enemies = null;                         // 怨듦꺽 踰붿쐞???덉뿉 ?덈뒗 ?곷뱾
    protected Collider2D currentTarget { get; set; } = null;       // ?꾩옱 ?寃?

    public TowerData towerData { get; set; } = default;
    public eCoreName coreType;

    public virtual void Start()
    {
        StartCoroutine(Rader());
        StartCoroutine(OnAttack());
    }

    // 0.1珥?????먭퀬 怨듦꺽 踰붿쐞 泥댄겕 泥섎━
    protected IEnumerator Rader()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            enemies = EnemyRader(enemyMask); 
        }
    }

    // 怨듦꺽 踰붿쐞 泥섎━ ?⑥닔
    Collider2D[] EnemyRader(LayerMask targetMask)
    {
        return Physics2D.OverlapCircleAll(transform.position + new Vector3(0, gizmoHeight, 0), towerData.AttackRange, targetMask);
    }

    // 怨듦꺽 踰붿쐞 湲곗쫰紐??쒖떆
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, gizmoHeight, 0), towerData.AttackRange);
            Gizmos.color = Color.white;
        }
    }
#endif

    // 怨듦꺽 ?ㅽ뻾 ?⑥닔
    public virtual IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies?.Length > 0);
            currentTarget = enemies[0];

            Collider2D guardian = enemies.ToList().Find((x => x.gameObject.layer == 11)); // Guardian

            if(guardian == null)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (i >= towerData.attackTargetCount)
                        break;

                    if (enemies[i] != null)
                    {
                        Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                    }
                }
            }
            else
            {
                Attack(towerData.OffensePower * enemies.Length, guardian.GetComponent<HealthSystem>());
            }
            
        
            yield return new WaitForSeconds(1f / towerData.AttackSpeed); // 怨듭냽留뚰겮 湲곕떎由ш퀬,
        }
    }

    // 怨듦꺽 濡쒖쭅 ?⑥닔
    public abstract void Attack(int power, HealthSystem enemy);
}
