using UnityEngine;

/// <summary>
/// 모든 적 AI의 기본 클래스
/// </summary>
public abstract class BaseEnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("적이 플레이어를 감지할 수 있는 범위")]
    protected float baseDetectionRange = 5f;
    
    [Tooltip("적이 플레이어를 공격할 수 있는 범위")]
    protected float baseAttackRange = 4f;
    
    [Tooltip("적의 이동 속도")]
    protected float baseMoveSpeed = 2f;

    // 컴포넌트 캐싱
    protected Rigidbody2D _rb;
    protected SpriteRenderer _spriteRenderer;
    protected BaseEnemy _enemy;
    protected Transform _player;
    protected Animator _animator;

    // 방향 관련
    protected Vector2 _lastDirection;

    protected virtual void Awake()
    {
        // 컴포넌트 캐싱
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemy = GetComponent<BaseEnemy>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        // 초기 설정
        InitializeAI();
    }

    /// <summary>
    /// AI 초기 설정
    /// </summary>
    protected virtual void InitializeAI()
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
    protected virtual void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
        }
    }

    protected virtual void FixedUpdate()
    {
        // 사망했거나 플레이어가 없으면 처리하지 않음
        if (_enemy != null && _enemy.IsDead()) return;
        if (_player == null)
        {
            FindPlayer();
            return;
        }

        HandleAIBehavior();
    }

    /// <summary>
    /// AI 행동 처리 (자식 클래스에서 구현)
    /// </summary>
    protected abstract void HandleAIBehavior();

    /// <summary>
    /// 적의 방향 업데이트
    /// </summary>
    protected virtual void UpdateDirection(Vector2 directionToPlayer)
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
    }
} 