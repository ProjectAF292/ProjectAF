using UnityEngine;

/// <summary>
/// 마법 투사체를 처리하는 클래스
/// </summary>
public class MagicProjectile : MonoBehaviour
{
    private float damage;
    private bool hasHit = false;

    /// <summary>
    /// 투사체 초기화
    /// </summary>
    public void Initialize(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        // 플레이어와 충돌 시 데미지 처리
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"플레이어에게 데미지 적용: {damage}");
                hasHit = true;
                HandleHit();
            }
        }
        // 벽이나 다른 장애물과 충돌 시
        else if (!other.CompareTag("Enemy") && !other.CompareTag("EnemyAttack"))
        {
            hasHit = true;
            HandleHit();
        }
    }

    private void HandleHit()
    {
        // 이동 중지
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        // 콜라이더 비활성화
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        // 오브젝트 즉시 파괴
        Destroy(gameObject);
    }
} 