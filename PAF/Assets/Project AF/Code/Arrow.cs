using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 화살의 동작을 제어하는 클래스
/// </summary>
public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    [Tooltip("화살 데미지")]
    public float damage = 10f;
    
    [Tooltip("화살 속도")]
    public float speed = 10f;
    
    [Tooltip("화살이 자동으로 사라지는 시간")]
    public float lifeTime = 3f;

    private void Start()
    {
        // lifeTime 후에 화살 제거
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌했을 때
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            
            // 화살 제거
            Destroy(gameObject);
        }
        // 벽과 충돌했을 때
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
} 