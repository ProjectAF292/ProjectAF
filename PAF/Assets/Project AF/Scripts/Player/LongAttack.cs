using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongAttack : MonoBehaviour
{
    public float damage = 5f; // 한 번에 입히는 피해량
    public float duration = 5f; // 공격 지속 시간
    public float damageInterval = 1f; // 피해 주기

    private List<BaseEnemy> enemiesInRange = new List<BaseEnemy>(); // 범위 내 적들 저장

    void Start()
    {
        // 일정 시간 후 자동 삭제
        Destroy(gameObject, duration);

        // 일정 시간마다 피해를 입히는 코루틴 실행
        StartCoroutine(ApplyDamageOverTime());
    }

    // 적이 범위에 들어왔을 때 리스트에 추가
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
            
        }
    }

    // 적이 범위를 벗어나면 리스트에서 제거
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

    // 일정 시간마다 범위 내 적들에게 지속 피해 적용
    IEnumerator ApplyDamageOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageInterval);

            foreach (BaseEnemy enemy in enemiesInRange)
            {
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
