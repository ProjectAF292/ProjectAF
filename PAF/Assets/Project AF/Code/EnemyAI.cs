using UnityEngine;

/// <summary>
/// 적의 AI 동작을 제어하는 클래스
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("적이 플레이어를 감지할 수 있는 범위")]
    public float detectionRange = 5f;
    
    [Tooltip("적이 플레이어를 공격할 수 있는 범위")]
    public float attackRange = 4f;
    
    [Tooltip("적의 이동 속도")]
    public float moveSpeed = 2f;
    
    [Tooltip("공격 범위 안에서 플레이어가 움직여도 공격을 유지할 최소 거리")]
    public float minAttackDistance = 2f;

    [Header("Attack Settings")]
    [Tooltip("화살 프리팹")]
    public GameObject arrowPrefab;
    
    [Tooltip("화살 발사 위치")]
    public Transform firePoint;
    
    [Tooltip("공격 쿨타임 (초)")]
    public float attackCooldown = 2f;

    // 컴포넌트 캐싱
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Enemy _enemy;
    private Transform _player;
    private Animator _animator;

    // 방향 관련
    private Vector2 _lastDirection;

    // 공격 관련
    private float _lastAttackTime;
    private bool _canAttack = true;
    private bool _isAttacking = false;

    private void Awake()
    {
        // 컴포넌트 캐싱
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemy = GetComponent<Enemy>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // 초기 설정
        InitializeAI();
    }

    /// <summary>
    /// AI 초기 설정
    /// </summary>
    private void InitializeAI()
    {
        // Rigidbody2D 설정
        if (_rb != null)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
        
        _lastDirection = Vector2.right;
        FindPlayer();
    }

    /// <summary>
    /// 플레이어 찾기
    /// </summary>
    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
            Debug.Log("플레이어를 찾았습니다!");
        }
        else
        {
            Debug.LogWarning("플레이어를 찾을 수 없습니다!");
        }
    }

    /// <summary>
    /// 적의 방향 업데이트
    /// </summary>
    private void UpdateDirection(Vector2 directionToPlayer)
    {
        float angle = Vector2.SignedAngle(Vector2.up, directionToPlayer);
        
        // 각도를 0-360도 범위로 변환
        if (angle < 0) angle += 360f;
        
        // 8방향 기준으로 방향 결정
        if (angle >= 337.5f || angle < 22.5f) // 위
        {
            _animator.SetFloat("moveX", 0);
            _animator.SetFloat("moveY", 1);
            _spriteRenderer.flipX = false;
        }
        else if (angle >= 22.5f && angle < 67.5f) // 오른쪽 위
        {
            _animator.SetFloat("moveX", 1);
            _animator.SetFloat("moveY", 1);
            _spriteRenderer.flipX = true;
        }
        else if (angle >= 67.5f && angle < 112.5f) // 오른쪽
        {
            _animator.SetFloat("moveX", 1);
            _animator.SetFloat("moveY", 0);
            _spriteRenderer.flipX = true;
        }
        else if (angle >= 112.5f && angle < 157.5f) // 오른쪽 아래
        {
            _animator.SetFloat("moveX", 1);
            _animator.SetFloat("moveY", -1);
            _spriteRenderer.flipX = true;
        }
        else if (angle >= 157.5f && angle < 202.5f) // 아래
        {
            _animator.SetFloat("moveX", 0);
            _animator.SetFloat("moveY", -1);
            _spriteRenderer.flipX = false;
        }
        else if (angle >= 202.5f && angle < 247.5f) // 왼쪽 아래
        {
            _animator.SetFloat("moveX", -1);
            _animator.SetFloat("moveY", -1);
            _spriteRenderer.flipX = false;
        }
        else if (angle >= 247.5f && angle < 292.5f) // 왼쪽
        {
            _animator.SetFloat("moveX", -1);
            _animator.SetFloat("moveY", 0);
            _spriteRenderer.flipX = false;
        }
        else // 왼쪽 위
        {
            _animator.SetFloat("moveX", -1);
            _animator.SetFloat("moveY", 1);
            _spriteRenderer.flipX = false;
        }
        
        _lastDirection = new Vector2(_animator.GetFloat("moveX"), _animator.GetFloat("moveY"));
        Debug.Log($"Direction Updated - Angle: {angle:F2}, Direction: {_lastDirection}");
    }

    /// <summary>
    /// 애니메이션 업데이트
    /// </summary>
    private void UpdateAnimation(Vector2 directionToPlayer)
    {
        if (_enemy != null && _enemy.IsDead) return;
        if (_isAttacking) return;

        bool isMoving = _rb.velocity.magnitude > 0.01f || directionToPlayer.magnitude > 0.1f;
        _animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            UpdateDirection(directionToPlayer);
        }
        else
        {
            _animator.SetFloat("moveX", _lastDirection.x);
            _animator.SetFloat("moveY", _lastDirection.y);
        }
    }

    private void FixedUpdate()
    {
        // 사망했거나 플레이어가 없으면 처리하지 않음
        if (_enemy != null && _enemy.IsDead) return;
        if (_player == null)
        {
            FindPlayer();
            return;
        }

        // 플레이어가 사망했는지 확인
        PlayerHealth playerHealth = _player.GetComponent<PlayerHealth>();
        if (playerHealth != null && playerHealth.IsDead())
        {
            // 플레이어가 사망했으면 모든 행동 중지
            _rb.velocity = Vector2.zero;
            _isAttacking = false;
            _animator.SetBool("isMoving", false);
            return;
        }

        HandleAIBehavior();
    }

    /// <summary>
    /// AI 행동 처리
    /// </summary>
    private void HandleAIBehavior()
    {
        Vector2 directionToPlayer = (_player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        // 플레이어가 탐지 범위 내에 있는 경우
        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= attackRange)
            {
                // 공격 범위 내에 있고 공격 가능한 상태일 때 즉시 공격
                if (!_isAttacking && _canAttack && Time.time >= _lastAttackTime + attackCooldown)
                {
                    if (distanceToPlayer >= minAttackDistance)
                    {
                        _rb.velocity = Vector2.zero;
                        HandleAttack(directionToPlayer);
                    }
                    else
                    {
                        // 너무 가까우면 약간 뒤로 이동
                        Vector2 backwardDirection = -directionToPlayer;
                        _rb.velocity = backwardDirection * (moveSpeed * 0.5f);
                        UpdateAnimation(directionToPlayer);
                    }
                }
                else if (_isAttacking)
                {
                    _rb.velocity = Vector2.zero;
                }
            }
            else
            {
                // 공격 범위 밖에 있으면 추적
                HandleChase(directionToPlayer);
            }
        }
        else
        {
            // 탐지 범위를 벗어나면 대기
            HandleIdle();
        }
    }

    /// <summary>
    /// 공격 처리
    /// </summary>
    private void HandleAttack(Vector2 directionToPlayer)
    {
        if (_player == null || arrowPrefab == null || firePoint == null) return;

        _isAttacking = true;
        _canAttack = false;
        _lastAttackTime = Time.time;

        // 공격 애니메이션 재생
        if (_animator != null)
        {
            _animator.SetBool("isMoving", false);
            UpdateDirection(directionToPlayer);
            _animator.SetTrigger("isAttacking");
        }

        // 화살 생성 및 발사
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrowComponent = arrow.GetComponent<Arrow>();
        
        if (arrowComponent != null)
        {
            // 화살 방향 설정
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            
            // 화살에 속도 적용
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
            if (arrowRb != null)
            {
                arrowRb.velocity = directionToPlayer * arrowComponent.speed;
            }
        }
    }

    /// <summary>
    /// 공격 가능 상태로 리셋 - 애니메이션 이벤트에서 호출
    /// </summary>
    public void ResetAttack()
    {
        _canAttack = true;
        _isAttacking = false;
        
        if (_animator != null && _player != null)
        {
            Vector2 currentDirection = (_player.position - transform.position).normalized;
            _animator.SetBool("isMoving", false);
            UpdateDirection(currentDirection);
        }
    }

    /// <summary>
    /// 추적 처리
    /// </summary>
    private void HandleChase(Vector2 directionToPlayer)
    {
        _rb.velocity = directionToPlayer * moveSpeed;
        UpdateDirection(directionToPlayer);
        UpdateAnimation(directionToPlayer);
        Debug.Log($"Chasing player - Direction: {directionToPlayer}");
    }

    /// <summary>
    /// 대기 상태 처리
    /// </summary>
    private void HandleIdle()
    {
        _rb.velocity = Vector2.zero;
        
        // 기본 방향을 아래쪽(Front)으로 설정
        _lastDirection = Vector2.down;
        
        if (_animator != null)
        {
            _animator.SetBool("isMoving", false);
            _animator.SetFloat("moveX", 0);
            _animator.SetFloat("moveY", -1);  // Front를 위해 -1 설정
            _spriteRenderer.flipX = false;
        }
        
        UpdateAnimation(Vector2.zero);
    }

    private void OnDrawGizmos()
    {
        DrawDetectionRange();
        DrawAttackRange();
        DrawPlayerDirection();
    }

    /// <summary>
    /// 탐지 범위 시각화
    /// </summary>
    private void DrawDetectionRange()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    /// <summary>
    /// 공격 범위 시각화
    /// </summary>
    private void DrawAttackRange()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    /// <summary>
    /// 플레이어 방향 시각화
    /// </summary>
    private void DrawPlayerDirection()
    {
        if (_player == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, _player.position);
        
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Vector3 right = transform.right;
        Vector3 left = -transform.right;
        Gizmos.DrawLine(transform.position, transform.position + right * attackRange);
        Gizmos.DrawLine(transform.position, transform.position + left * attackRange);
    }
}
