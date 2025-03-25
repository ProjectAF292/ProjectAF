using UnityEngine;

/// <summary>
/// 모든 적의 기본 클래스
/// </summary>
public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("최대 체력")]
    public float maxHealth = 100f;

    [Tooltip("현재 체력")]
    public float currentHealth;

    // 적의 사망 상태
    protected bool isDead = false;

    // 컴포넌트 캐싱
    protected Animator _animator;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        // 컴포넌트 캐싱
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    /// <summary>
    /// 데미지를 받는 메서드
    /// </summary>
    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"{gameObject.name}이(가) {damage}의 데미지를 받았습니다. 남은 체력: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 적 사망 처리
    /// </summary>
    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;

        Debug.Log($"{gameObject.name}이(가) 사망했습니다!");

        // 사망 애니메이션 재생
        if (_animator != null)
        {
            _animator.SetTrigger("Die");
            // 애니메이션 재생 후 오브젝트 파괴
            Destroy(gameObject, 0.5f);
        }
        else
        {
            // 애니메이터가 없으면 바로 파괴
            Destroy(gameObject);
        }

        // Rigidbody 비활성화
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        // 콜라이더 비활성화
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    /// <summary>
    /// 현재 체력을 반환
    /// </summary>
    public virtual float GetCurrentHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// 최대 체력을 반환
    /// </summary>
    public virtual float GetMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// 사망 상태를 반환
    /// </summary>
    public virtual bool IsDead()
    {
        return isDead;
    }

    /// <summary>
    /// 체력 비율을 반환 (0 ~ 1)
    /// </summary>
    public virtual float GetHealthRatio()
    {
        if (maxHealth <= 0) return 0;
        return Mathf.Clamp01(currentHealth / maxHealth);
    }
} 