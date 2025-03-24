using UnityEngine;

/// <summary>
/// 원거리 적의 공격을 처리하는 클래스
/// </summary>
public class RangedEnemyAttack : BaseEnemyAttack
{
    [Header("Ranged Attack Settings")]
    [Tooltip("화살 프리팹")]
    public GameObject arrowPrefab;

    [Tooltip("화살 발사 위치")]
    public Transform firePoint;

    [Tooltip("화살 속도")]
    public float arrowSpeed = 10f;

    [Tooltip("화살 수명")]
    public float arrowLifetime = 3f;

    private bool _isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        
        // 기본값 설정
        if (attackRange == 2f) // 기본값이면 원거리에 맞게 조정
        {
            attackRange = 5f;
        }
        if (attackCooldown == 1f) // 기본값이면 원거리에 맞게 조정
        {
            attackCooldown = 2f;
        }
    }

    protected override void ExecuteAttack()
    {
        if (target == null || arrowPrefab == null || firePoint == null) return;
        if (_isAttacking) return;

        _isAttacking = true;

        // 애니메이션 처리
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
            UpdateDirection(direction);
            animator.SetTrigger("isAttacking");
        }
    }

    /// <summary>
    /// 실제 화살 발사 - 애니메이션 이벤트에서 호출
    /// </summary>
    public void FireArrow()
    {
        if (target == null || arrowPrefab == null || firePoint == null) return;

        // 화살 생성
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        
        // 화살 설정
        Arrow arrowComponent = arrow.GetComponent<Arrow>();
        if (arrowComponent != null)
        {
            arrowComponent.damage = damage;
            arrowComponent.speed = arrowSpeed;
            arrowComponent.lifeTime = arrowLifetime;

            // 화살 방향과 속도 설정
            Vector2 direction = ((Vector2)target.position - (Vector2)firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
            if (arrowRb != null)
            {
                arrowRb.velocity = direction * arrowSpeed;
            }
        }
    }

    /// <summary>
    /// 방향 업데이트
    /// </summary>
    private void UpdateDirection(Vector2 direction)
    {
        if (animator == null) return;

        float angle = Vector2.SignedAngle(Vector2.up, direction);
        if (angle < 0) angle += 360f;
        
        if (angle >= 337.5f || angle < 22.5f) // 위
        {
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", 1);
        }
        else if (angle >= 22.5f && angle < 67.5f) // 오른쪽 위
        {
            animator.SetFloat("moveX", 1);
            animator.SetFloat("moveY", 1);
        }
        else if (angle >= 67.5f && angle < 112.5f) // 오른쪽
        {
            animator.SetFloat("moveX", 1);
            animator.SetFloat("moveY", 0);
        }
        else if (angle >= 112.5f && angle < 157.5f) // 오른쪽 아래
        {
            animator.SetFloat("moveX", 1);
            animator.SetFloat("moveY", -1);
        }
        else if (angle >= 157.5f && angle < 202.5f) // 아래
        {
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", -1);
        }
        else if (angle >= 202.5f && angle < 247.5f) // 왼쪽 아래
        {
            animator.SetFloat("moveX", -1);
            animator.SetFloat("moveY", -1);
        }
        else if (angle >= 247.5f && angle < 292.5f) // 왼쪽
        {
            animator.SetFloat("moveX", -1);
            animator.SetFloat("moveY", 0);
        }
        else // 왼쪽 위
        {
            animator.SetFloat("moveX", -1);
            animator.SetFloat("moveY", 1);
        }
    }

    /// <summary>
    /// 공격 상태 리셋 - 애니메이션 이벤트에서 호출
    /// </summary>
    public void ResetAttack()
    {
        _isAttacking = false;
        
        if (animator != null && target != null)
        {
            Vector2 currentDirection = ((Vector2)target.position - (Vector2)transform.position).normalized;
            animator.SetBool("isMoving", false);
            UpdateDirection(currentDirection);
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        // 발사 위치 표시
        if (firePoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(firePoint.position, 0.2f);
        }
    }
} 