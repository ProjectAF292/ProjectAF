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
    public float attackRange = 1f;
    
    [Tooltip("적의 이동 속도")]
    public float moveSpeed = 2f;

    // 컴포넌트 캐싱
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Enemy _enemy;
    private Transform _player;

    // 방향 관련
    private Vector2 _lastDirection;
    private readonly float _directionThreshold = 0.3f;

    private void Awake()
    {
        // 컴포넌트 캐싱
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemy = GetComponent<Enemy>();
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
        // 방향 변화가 임계값보다 작으면 무시
        if (Vector2.Distance(directionToPlayer, _lastDirection) < _directionThreshold)
        {
            return;
        }

        // 좌우 이동이 더 큰 경우
        if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
        {
            HandleHorizontalMovement(directionToPlayer.x);
        }
        else
        {
            HandleVerticalMovement(directionToPlayer.y);
        }
        
        _lastDirection = directionToPlayer;
    }

    /// <summary>
    /// 좌우 이동 처리
    /// </summary>
    private void HandleHorizontalMovement(float xDirection)
    {
        _spriteRenderer.flipX = xDirection < 0;
        transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// 상하 이동 처리
    /// </summary>
    private void HandleVerticalMovement(float yDirection)
    {
        float targetRotation = yDirection > 0 ? 90f : -90f;
        transform.rotation = Quaternion.Euler(0, 0, targetRotation);
        _spriteRenderer.flipX = false;
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
                // 공격 범위 내에 있으면 공격
                HandleAttack(directionToPlayer);
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
        _rb.velocity = Vector2.zero;
        UpdateDirection(directionToPlayer);
        AttackPlayer();
    }

    /// <summary>
    /// 추적 처리
    /// </summary>
    private void HandleChase(Vector2 directionToPlayer)
    {
        _rb.velocity = directionToPlayer * moveSpeed;
        UpdateDirection(directionToPlayer);
        Debug.DrawLine(transform.position, _player.position, Color.red);
    }

    /// <summary>
    /// 대기 상태 처리
    /// </summary>
    private void HandleIdle()
    {
        _rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        _lastDirection = Vector2.right;
    }

    /// <summary>
    /// 플레이어 공격
    /// </summary>
    private void AttackPlayer()
    {
        Debug.Log("플레이어 공격!");
        // 여기에 실제 공격 로직을 구현하세요
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
