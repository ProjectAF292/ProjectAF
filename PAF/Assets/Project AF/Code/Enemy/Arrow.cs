using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 화살의 동작을 처리하는 클래스
/// </summary>
public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    [Tooltip("화살 데미지")]
    public float damage = 10f;
    
    [Tooltip("화살 속도")]
    public float speed = 10f;
    
    [Tooltip("화살 수명")]
    public float lifeTime = 3f;

    private void Start()
    {
        if (lifeTime > 0)
        {
            Destroy(gameObject, lifeTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌했을 때
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            
            // 화살 제거
            Destroy(gameObject);
        }
        // 벽과 충돌했을 때
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
} 