using UnityEngine;

/// <summary>
/// 모든 적의 공격을 처리하는 기본 클래스
/// </summary>
public abstract class BaseEnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [Tooltip("공격 데미지")]
    public float damage = 10f;

    [Tooltip("공격 쿨타임")]
    public float attackCooldown = 1f;

    [Tooltip("공격 범위")]
    public float attackRange = 2f;

    protected float nextAttackTime;
    protected Transform target;
    protected BaseEnemy enemy;
    protected Animator animator;

    protected virtual void Awake()
    {
        enemy = GetComponent<BaseEnemy>();
        animator = GetComponent<Animator>();
        FindTarget();
    }

    protected virtual void Start()
    {
        nextAttackTime = 0f;
    }

    /// <summary>
    /// 타겟(플레이어) 찾기
    /// </summary>
    protected virtual void FindTarget()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
    }

    /// <summary>
    /// 공격 가능 여부 확인
    /// </summary>
    protected virtual bool CanAttack()
    {
        if (target == null || enemy == null || enemy.IsDead()) return false;
        
        // 쿨타임 체크
        if (Time.time < nextAttackTime) return false;

        // 거리 체크
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        return distanceToTarget <= attackRange;
    }

    /// <summary>
    /// 공격 쿨타임 갱신
    /// </summary>
    protected virtual void UpdateAttackCooldown()
    {
        nextAttackTime = Time.time + attackCooldown;
    }

    /// <summary>
    /// 공격 시도
    /// </summary>
    public virtual void TryAttack()
    {
        if (CanAttack())
        {
            ExecuteAttack();
            UpdateAttackCooldown();
        }
    }

    /// <summary>
    /// 실제 공격 실행 (자식 클래스에서 구현)
    /// </summary>
    protected abstract void ExecuteAttack();

    protected virtual void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
} 