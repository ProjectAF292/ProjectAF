using UnityEngine;

/// <summary>
/// 근접 공격의 히트박스를 처리하는 클래스
/// </summary>
public class MeleeHitBox : MonoBehaviour
{
    private float _damage;  // private으로 변경
    private bool hasDamaged = false;  // 현재 활성화 중에 데미지를 줬는지 체크

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void OnEnable()
    {
        // 히트박스가 활성화될 때마다 데미지 체크 초기화
        hasDamaged = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // 아직 데미지를 주지 않았고, 플레이어와 충돌 중일 때
        if (!hasDamaged && other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(_damage);
                Debug.Log($"플레이어에게 데미지 적용: {_damage}");
                hasDamaged = true;  // 이번 활성화 중에는 더 이상 데미지를 주지 않음
            }
        }
    }
} 