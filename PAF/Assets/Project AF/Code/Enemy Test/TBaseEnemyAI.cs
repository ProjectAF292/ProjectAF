using UnityEngine;
using System.Collections;

public class TBaseEnemyAI : MonoBehaviour
{
    protected TBaseEnemy enemy;
    protected Transform player;
    protected bool isAttacking;
    protected float lastAttackTime;
    protected float lastSkillTime;
    protected float skillCooldown = 5f;
    protected Animator animator;
    protected EnemyData enemyData;
    protected Rigidbody2D rb;
    protected Vector2 moveDirection;
    protected Vector2 lookDirection;
    protected bool isMoving;
    private Vector3 originalScale;  // 초기 스케일 저장용 변수
    protected Vector2 targetDirection;  // 목표 방향을 저장하는 변수

    protected virtual void Start()
    {
        enemy = GetComponent<TBaseEnemy>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
        Debug.Log($"[{gameObject.name}] Player found at position: {player.position}");

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyData = enemy.enemyData;
        
        if (enemyData == null)
        {
            Debug.LogError($"[{gameObject.name}] EnemyData is null!");
            return;
        }
        Debug.Log($"[{gameObject.name}] EnemyData loaded - Detection: {enemyData.detectionRange}, Attack: {enemyData.attackRange}, Speed: {enemyData.moveSpeed}");

        lastAttackTime = -enemyData.attackSpeed;
        lastSkillTime = -skillCooldown;

        // 초기 스케일 저장
        originalScale = transform.localScale;
        targetDirection = Vector2.zero;  // 초기 목표 방향 설정
        lookDirection = Vector2.zero;    // 초기 현재 방향 설정
    }

    protected virtual void Update()
    {
        if (enemy.IsDead()) 
        {
            animator.SetBool("isMoving", false);
            rb.velocity = Vector2.zero;
            return;
        }

        DetectPlayer();
        UpdateAnimation();
    }

    protected virtual void FixedUpdate()
    {
        if (enemy.IsDead() || player == null) return;
        
        // 이동 처리는 DetectPlayer에서 처리되므로 여기서는 제거
    }

    protected virtual void DetectPlayer()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 directionToPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
        
        // 플레이어가 감지 범위 내에 있는지 확인
        if (distanceToPlayer <= enemyData.detectionRange)
        {
            // 플레이어 방향 업데이트 (항상)
            UpdateDirection(directionToPlayer);
            
            // 공격 범위 내에 있으면 공격
            if (distanceToPlayer <= enemyData.attackRange)
            {
                isMoving = false;
                rb.velocity = Vector2.zero;
                if (!isAttacking && Time.time >= lastAttackTime + enemyData.attackSpeed)
                {
                    Attack();
                }
            }
            // 공격 범위 밖에 있으면 플레이어를 추격
            else
            {
                isMoving = true;
                moveDirection = directionToPlayer;
                rb.velocity = directionToPlayer * enemyData.moveSpeed;
                UpdateAnimation(directionToPlayer);
            }
        }
        else
        {
            // 플레이어가 감지 범위를 벗어나면 정지
            isMoving = false;
            moveDirection = Vector2.zero;
            rb.velocity = Vector2.zero;
            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    protected virtual void Attack()
    {
        if (enemy.IsDead() || player == null) return;

        isAttacking = true;
        lastAttackTime = Time.time;
        
        // 공격 애니메이션 설정
        if (animator != null)
        {
            animator.SetBool("isAttacking", true);
        }

        // 공격 컴포넌트를 통해 공격 실행
        MeleeAttack meleeAttack = GetComponent<MeleeAttack>();
        if (meleeAttack != null)
        {
            meleeAttack.Attack(player);
        }
        
        Debug.Log($"[{gameObject.name}] Attacking player! - Distance: {Vector2.Distance(transform.position, player.position):F2}");

        // 공격 시 이동 중지
        rb.velocity = Vector2.zero;

        // 0.5초 후에 자동으로 공격 상태 해제
        StartCoroutine(AttackCompleteRoutine());
    }

    private IEnumerator AttackCompleteRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        OnAttackComplete();
    }

    protected virtual void UpdateAnimation(Vector2 direction = default)
    {
        if (player == null || animator == null) return;

        // 이동 상태 업데이트
        animator.SetBool("isMoving", isMoving && !isAttacking);

        // 현재 방향에서 목표 방향으로 부드럽게 전환
        lookDirection = Vector2.Lerp(lookDirection, targetDirection, Time.deltaTime * 10f);

        // 방향 정규화
        if (lookDirection.magnitude > 0)
        {
            lookDirection.Normalize();
        }

        // 주 방향에 따라 애니메이션 설정
        float absX = Mathf.Abs(lookDirection.x);
        float absY = Mathf.Abs(lookDirection.y);

        // X나 Y 중 더 큰 값을 기준으로 주 방향 결정
        if (absX > absY)
        {
            // 좌우 이동
            animator.SetFloat("moveX", absX);
            animator.SetFloat("moveY", 0);
            
            // 스프라이트 방향 설정
            transform.localScale = new Vector3(
                lookDirection.x > 0 ? Mathf.Abs(originalScale.x) : -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
        else
        {
            // 상하 이동
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", lookDirection.y);
            
            // 스프라이트 방향 유지
            transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x) * (lookDirection.x >= 0 ? 1 : -1),
                originalScale.y,
                originalScale.z
            );
        }
    }

    public virtual void OnAttackComplete()
    {
        isAttacking = false;
        if (animator != null)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    protected virtual void OnSkillComplete()
    {
        lastSkillTime = Time.time;
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError($"[{gameObject.name}] Player not found!");
            return;
        }
        Debug.Log($"[{gameObject.name}] Player found at position: {player.position}");
    }

    private void UpdateDirection(Vector2 direction)
    {
        // 각도 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // 각도를 0~360 범위로 변환
        if (angle < 0) angle += 360f;

        // 4방향 결정 (주 방향만 선택)
        if (angle <= 45f || angle > 315f) // 오른쪽
        {
            targetDirection = new Vector2(1, 0);
        }
        else if (angle <= 135f) // 위
        {
            targetDirection = new Vector2(0, 1);
        }
        else if (angle <= 225f) // 왼쪽
        {
            targetDirection = new Vector2(-1, 0);
        }
        else // 아래
        {
            targetDirection = new Vector2(0, -1);
        }
    }

    private void OnDrawGizmos()
    {
        if (enemyData == null) return;

        // 탐지 범위 (파란색)
        Gizmos.color = new Color(0f, 0f, 1f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);

        // 공격 범위 (빨간색)
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyData == null) return;

        // 선택했을 때 더 진하게 표시
        // 탐지 범위 (파란색)
        Gizmos.color = new Color(0f, 0f, 1f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);

        // 공격 범위 (빨간색)
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
    }
} 

