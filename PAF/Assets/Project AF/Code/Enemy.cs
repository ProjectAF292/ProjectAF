using UnityEngine;

/// <summary>
/// 적의 기본 속성과 상태를 관리하는 클래스
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [Tooltip("적의 이름")]
    public string enemyName = "Enemy";
    
    [Tooltip("최대 체력")]
    public float maxHealth = 100f;
    
    [Tooltip("현재 체력")]
    [SerializeField]
    private float _currentHealth;
    public float CurrentHealth => _currentHealth; // 읽기 전용 프로퍼티

    // 사망 상태를 외부에서 읽을 수만 있도록 프로퍼티로 설정
    public bool IsDead { get; private set; }

    // 캐싱
    private Rigidbody2D _rb;
    private Animator _animator;

    private void Awake()
    {
        // 컴포넌트 캐싱
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // 초기 상태 설정
        InitializeEnemy();
    }

    /// <summary>
    /// 적의 초기 상태를 설정
    /// </summary>
    private void InitializeEnemy()
    {
        _currentHealth = maxHealth;
        IsDead = false;
    }

    //적이 물리적으로 공격을 받았는지 체크 및 데미지 부여
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Attack")) // 만약 적에게 충돌한게 어택이 아니라면
            return; // 이 스크립트는 걍 꺼-져라

        // Attack 컴포넌트에서 데미지를 가져와서 적용
        Attack attack = collision.GetComponent<Attack>();
        if (attack != null)
        {
            TakeDamage(attack.damage);
        }
    }

    /// <summary>
    /// 적이 데미지를 받았을 때 호출되는 메서드
    /// </summary>
    /// <param name="damage">받은 데미지량</param>
    public void TakeDamage(float damage)
    {
        // 이미 사망한 상태면 데미지를 받지 않음
        if (IsDead) return;
        
        _currentHealth -= damage;
        Debug.Log($"Enemy hit! Current Health: {_currentHealth}");
        
        // 체력이 0 이하가 되면 사망 처리
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    /// <summary>
    /// 적이 사망했을 때 호출되는 메서드
    /// </summary>
    private void Die()
    {
        IsDead = true;
        
        // Rigidbody가 있다면 움직임을 멈춤
        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
        }

        // 사망 애니메이션 재생
        if (_animator != null)
        {
            _animator.SetTrigger("Die");
        }
        
        // 콜라이더 비활성화 (이미 죽은 적은 충돌하지 않도록)
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        // 1초 후에 오브젝트 제거 (애니메이션 재생 시간 고려)
        Destroy(gameObject, 1f);
    }
} 