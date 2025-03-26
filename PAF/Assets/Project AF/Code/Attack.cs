using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 0.3f;  // 공격 지속 시간
    public float attackRadius = 1.5f;  // 검이 휘두르는 반지름 (플레이어와의 거리)
    private Transform attackCenter;  // 회전 중심 (자동으로 플레이어를 찾음)

    private float startAngle;
    private float elapsedTime = 0f;

    void Start()
    {
        // 공격 중심을 플레이어의 위치로 설정
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            attackCenter = player.transform;  // 플레이어를 중심으로 설정
            startAngle = player.transform.eulerAngles.z - 90f; // 플레이어 방향을 기준으로 휘두름
            StartCoroutine(SwingAttack());
        }
        else
        {
            Debug.LogError("플레이어를 찾을 수 없습니다. Attack 스크립트가 정상 동작하지 않습니다.");
            Destroy(gameObject);
        }
    }

    IEnumerator SwingAttack()
    {
        float targetAngle = startAngle + 180f;  // 180도 회전 목표
        float duration = lifetime;  // 회전하는 데 걸리는 시간

        while (elapsedTime < duration)
        {
            float angle = Mathf.Lerp(startAngle, targetAngle, elapsedTime / duration);  // 부드럽게 회전
            float radian = angle * Mathf.Deg2Rad;

            // 검 위치를 회전 중심 기준으로 이동
            transform.position = attackCenter.position + new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)) * attackRadius;
            transform.rotation = Quaternion.Euler(0, 0, angle);  // 검이 항상 앞을 향하도록 회전

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);  // 회전이 끝나면 삭제
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TBaseEnemy tbaseEnemy = collision.GetComponent<TBaseEnemy>();
            if (tbaseEnemy != null)
            {
                tbaseEnemy.TakeDamage(damage);
                return;
            }

            BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

}
