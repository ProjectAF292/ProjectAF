using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangedAttack : MonoBehaviour
{
    public float damage = 10f;  // 데미지 값
    public float speed = 10f;   // 이동 속도
    public float lifetime = 2f; // 존재 시간
    public Transform attackPos;
    private float startAngle;

    private Rigidbody2D rb;
    private Vector2 moveDirection; // 이동 방향
    private Transform player;  // 플레이어 위치 저장

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attackPos = GameObject.Find("Attack Pos").transform;

        // 플레이어 찾기 (태그 기반, 플레이어 오브젝트가 "Player" 태그를 가지고 있어야 함)
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // 플레이어 위치가 있다면 반대 방향으로 설정
        if (player != null)
        {
            moveDirection = ((Vector2)transform.position - (Vector2)player.position).normalized;
            startAngle = Mathf.Atan2(transform.position.y, transform.position.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = attackPos.rotation;
            //transform.rotation = Quaternion.Euler(0, 0, startAngle);
            //Debug.Log(transform.rotation);

        }
        

        MoveStraight();
        Destroy(gameObject, lifetime); // 일정 시간이 지나면 삭제
    }

    

    private void MoveStraight()
    {
        rb.velocity = moveDirection * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // 적과 충돌했을 때
        {
            BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject); // 적과 충돌하면 삭제
        }
    }

}
