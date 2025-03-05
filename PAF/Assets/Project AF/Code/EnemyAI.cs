using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 5f; // 탐지 범위
    public float attackRange = 1f; // 공격 범위
    public float moveSpeed = 2f; // 이동 속도
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastDirection; // 이전 방향 저장

    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Rigidbody2D 설정
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // 회전 방지
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // 연속 충돌 감지
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // 부드러운 움직임
        
        FindPlayer();
        lastDirection = Vector2.right; // 초기 방향 설정
    }

    void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log("플레이어를 찾았습니다!");
        }
        else
        {
            Debug.LogWarning("플레이어를 찾을 수 없습니다!");
        }
    }

    void UpdateDirection(Vector2 directionToPlayer)
    {
        // 방향 전환 임계값 설정
        float directionThreshold = 0.3f; // 임계값 증가
        
        // 현재 방향과 이전 방향의 차이가 임계값보다 작으면 방향 유지
        if (Vector2.Distance(directionToPlayer, lastDirection) < directionThreshold)
        {
            return;
        }

        // 좌우 방향이 더 큰 경우
        if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
        {
            if (directionToPlayer.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
            transform.rotation = Quaternion.identity;
        }
        else
        {
            // 위아래 방향이 더 큰 경우
            // 위로 갈 때는 90도, 아래로 갈 때는 -90도
            float targetRotation = directionToPlayer.y > 0 ? 90f : -90f;
            transform.rotation = Quaternion.Euler(0, 0, targetRotation);
            spriteRenderer.flipX = false; // 위아래 방향일 때는 좌우 반전 없음
        }
        
        lastDirection = directionToPlayer;
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // 플레이어가 탐지 범위 내에 있는지 확인
        if (distanceToPlayer <= detectionRange)
        {
            // 공격 범위 안에 들어왔는지 확인
            if (distanceToPlayer <= attackRange)
            {
                rb.velocity = Vector2.zero;
                UpdateDirection(directionToPlayer);
                AttackPlayer();
            }
            else
            {
                // 공격 범위 밖에 있으면 추적
                rb.velocity = directionToPlayer * moveSpeed;
                UpdateDirection(directionToPlayer);
                Debug.DrawLine(transform.position, player.position, Color.red);
            }
        }
        else
        {
            // 탐지 범위를 벗어나면 정지
            rb.velocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
            lastDirection = Vector2.right; // 초기 방향으로 리셋
        }
    }

    void AttackPlayer()
    {
        Debug.Log("플레이어 공격!");
        // 플레이어 공격 로직 추가
    }

    void OnDrawGizmos()
    {
        // 탐지 범위
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // 공격 범위
        Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 플레이어가 있다면 플레이어 방향 표시
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, player.position);
            
            // 공격 가능한 각도 범위 표시
            Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
            Vector3 right = transform.right;
            Vector3 left = -transform.right;
            Gizmos.DrawLine(transform.position, transform.position + right * attackRange);
            Gizmos.DrawLine(transform.position, transform.position + left * attackRange);
        }
    }
}
