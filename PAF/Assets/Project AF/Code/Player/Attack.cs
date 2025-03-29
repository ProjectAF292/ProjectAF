using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 0.3f;  // 공격 지속 시간
    public float attackRadius = 1.5f;  // 검이 휘두르는 반지름 (플레이어와의 거리)
    private Transform attackCenter;  // 회전 중심 (공격이 생성된 위치)

    private float startAngle;
    private float elapsedTime = 0f;

    void Start()
    {
        // 공격 판정이 생성된 위치를 기준으로 회전
        attackCenter = transform;  // 지금 이 스크립트가 적용된 오브젝트가 중심이 됨

        // 플레이어가 바라보는 방향을 기준으로 회전 시작 각도 설정
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (attackCenter.position - player.transform.position).normalized;
            startAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // 시작 각도 설정
            StartCoroutine(SwingAttack());
        }
        else
        {
            Debug.LogError("플레이어를 찾을 수 없습니다.");
            Destroy(gameObject);
        }
    }

    IEnumerator SwingAttack()
    {
        float targetAngle = startAngle + 180f;  // 180도 회전 목표
        float duration = lifetime;  // 회전 시간
        Vector3 originalPosition = transform.position;  // 초기 위치 저장

        while (elapsedTime < duration)
        {
            float angle = Mathf.Lerp(startAngle, targetAngle, elapsedTime / duration);  // 부드러운 회전
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);  // Z축 기준 회전
            transform.position = originalPosition + rotation * (Vector3.right * attackRadius);  // 회전 적용
            transform.rotation = rotation;  // 검의 방향도 같이 회전

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
