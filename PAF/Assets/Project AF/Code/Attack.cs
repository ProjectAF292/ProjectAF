using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage;
    public float speed;  // 공격 속도
    public float lifetime = 2f;

    private Vector2 moveDirection;  // 이동 방향
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Init(damage, moveDirection, speed, lifetime);
    }

    public void Init(float damage, Vector2 direction, float speed, float lifetime)
    {
        this.damage = damage;
        this.speed = speed;
        this.moveDirection = direction;
        
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 가져오기


        gameObject.SetActive(true); // 일정 시간 후 자동 삭제

        if (rb != null)
        {
            rb.velocity = moveDirection * speed; // 지정된 방향으로 이동
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // 적과 충돌 감지
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // 적에게 데미지 적용
            }

            gameObject.SetActive(false); // 충돌 후 삭제 (필요시 삭제 X)
        }
    }

}
