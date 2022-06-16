using UnityEngine;

public class Single_Core : CoreBase
{
    [SerializeField] private Transform head = null;
    [SerializeField] private float angleSpeed = 0f;

    private void Update()
    {
        RotateCanon(angleSpeed);
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        if (bullet == null)
        {
            bullet = Managers.Pool.GetItem<CanonBall>();
        }

        bullet.transform.position = transform.position;
        bullet.Init(TowerData,enemy.transform);
        OnAttack();
    }

    // 대포 머리 회전
    void RotateCanon(float angleSpeed)
    {
        if (target != null)
        {
            Vector2 direction = target.transform.position - head.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
            head.rotation = Quaternion.Slerp(head.rotation, rotation, angleSpeed * Time.deltaTime);
        }
    }
}
