using UnityEngine;

/// <summary>
/// 마법 적의 공격을 처리하는 클래스
/// </summary>
public class MagicEnemyAttack : BaseEnemyAttack
{
    [Header("Magic Attack Settings")]
    [Tooltip("마법 투사체 프리팹")]
    public GameObject magicProjectilePrefab;

    [Tooltip("투사체 발사 위치")]
    public Transform firePoint;

    [Tooltip("투사체 이동 속도")]
    public float projectileSpeed = 8f;

    [Tooltip("투사체 수명")]
    public float projectileLifetime = 3f;

    [Header("Magic Effect")]
    [Tooltip("마법 이펙트 애니메이션")]
    public RuntimeAnimatorController magicEffectAnimator;
    
    [Tooltip("이펙트 크기")]
    public Vector2 effectSize = Vector2.one;

    private bool _isAttacking = false;
    protected SpriteRenderer _spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 기본값 설정
        if (damage == 0) damage = 15f;
        if (attackCooldown == 0) attackCooldown = 2f;
        if (attackRange == 0) attackRange = 6f;
    }

    protected override void ExecuteAttack()
    {
        if (target == null || magicProjectilePrefab == null || firePoint == null) return;
        if (_isAttacking) return;

        _isAttacking = true;

        // 애니메이션 처리
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
            UpdateDirection(direction);            
        }

        // 투사체 생성 및 발사
        FireMagicProjectile();
    }

    private void FireMagicProjectile()
    {
        if (target == null || magicProjectilePrefab == null || firePoint == null) return;

        // 투사체 생성
        GameObject projectile = Instantiate(magicProjectilePrefab, firePoint.position, Quaternion.identity);
        
        // 투사체 설정
        Vector2 direction = ((Vector2)target.position - (Vector2)firePoint.position).normalized;
        
        // 투사체 컴포넌트 설정
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.velocity = direction * projectileSpeed;
        }

        // 이펙트 설정
        if (magicEffectAnimator != null)
        {
            Animator projectileAnimator = projectile.GetComponent<Animator>();
            if (projectileAnimator != null)
            {
                projectileAnimator.runtimeAnimatorController = magicEffectAnimator;
            }
        }

        // 크기 설정
        projectile.transform.localScale = effectSize;

        // 데미지 설정
        MagicProjectile projectileScript = projectile.GetComponent<MagicProjectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(damage);
        }

        // 수명 설정
        Destroy(projectile, projectileLifetime);

        // 공격 상태 초기화
        _isAttacking = false;
    }

    private void UpdateDirection(Vector2 direction)
    {
        if (animator == null) return;

        float angle = Vector2.SignedAngle(Vector2.right, direction);
        if (angle < 0) angle += 360f;
        
        if (angle >= 315f || angle < 45f) // 오른쪽
        {
            animator.SetFloat("moveX", 1);
            animator.SetFloat("moveY", 0);
            if (_spriteRenderer != null) _spriteRenderer.flipX = false;
        }
        else if (angle >= 45f && angle < 135f) // 위
        {
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", 1);
            if (_spriteRenderer != null) _spriteRenderer.flipX = false;
        }
        else if (angle >= 135f && angle < 225f) // 왼쪽
        {
            animator.SetFloat("moveX", -1);
            animator.SetFloat("moveY", 0);
            if (_spriteRenderer != null) _spriteRenderer.flipX = true;
        }
        else // 아래
        {
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", -1);
            if (_spriteRenderer != null) _spriteRenderer.flipX = false;
        }
    }
} 