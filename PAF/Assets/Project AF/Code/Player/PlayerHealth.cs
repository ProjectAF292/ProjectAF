using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어의 체력을 관리하는 클래스
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("최대 체력")]
    public float maxHealth = 100f;

    [Tooltip("현재 체력")]
    [SerializeField]
    private float currentHealth;

    // 플레이어의 사망 상태
    private bool isDead = false;
    private bool isInvincible = false; // 무적 상태 추가

    // 컴포넌트 캐싱
    private Animator animator;
    private Rigidbody2D rb;
    WaitForFixedUpdate wait;

    private void Awake()
    {
        // 컴포넌트 캐싱
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        wait = new WaitForFixedUpdate();
    }

    private void Start()
    {
        // 초기 체력 설정
        currentHealth = maxHealth;
    }

   
    public void TakeDamage(float damage)
    {
       
        if (isDead || isInvincible) return; // 무적 상태면 피해를 받지 않음

        currentHealth -= damage;
        //Debug.Log($"플레이어가 {damage} 데미지를 받았습니다. 현재 체력: {currentHealth}");

        // 체력이 0 이하가 되면 사망
        if (currentHealth <= 0)
        {
            Dead();
        }

        else
        {
            animator.SetTrigger("Hit");
        }

        
    }

    /// <summary>
    /// 플레이어 사망 처리
    /// </summary>
    private void Dead()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;

        Debug.Log("플레이어가 사망했습니다!");

        animator.SetTrigger("Dead");
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.isKinematic = true;

        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    // 무적 상태 설정
    public void SetInvincible(bool state)
    {
        isInvincible = state;
    }

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public bool IsDead() => isDead;

} 