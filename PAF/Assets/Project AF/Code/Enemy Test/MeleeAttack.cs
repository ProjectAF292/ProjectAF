using UnityEngine;

public class MeleeAttack : MonoBehaviour, IEnemyAttack
{
    private EnemyData enemyData;
    private TBaseEnemyAI enemyAI;
    private GameObject attackEffect;

    [Header("Effect Settings")]
    [SerializeField] private Vector2 effectScale = new Vector2(1.5f, 1.5f);  // 이펙트 크기 조정

    public void Initialize(EnemyData data)
    {
        enemyData = data;
        enemyAI = GetComponent<TBaseEnemyAI>();
        
        // 공격 이펙트 프리팹이 있다면 생성
        if (enemyData.attackEffectPrefab != null)
        {
            attackEffect = Instantiate(enemyData.attackEffectPrefab);
            attackEffect.transform.localScale = enemyData.attackEffectScale;  // EnemyData에서 설정한 스케일 사용
            attackEffect.SetActive(false);
        }
    }

    public void Attack(Transform target)
    {
        if (target == null) return;

        // 공격 이펙트 표시
        if (attackEffect != null)
        {
            // 적과 플레이어 사이의 중간 지점 계산
            Vector2 direction = (target.position - transform.position).normalized;
            Vector2 effectPosition = (Vector2)transform.position + direction * (enemyData.attackRange * 0.5f);
            attackEffect.transform.position = effectPosition;
            
            // 이펙트의 방향을 타겟을 향하게 설정
            attackEffect.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            attackEffect.SetActive(true);
        }

        // 타겟이 플레이어인 경우 데미지 처리
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(enemyData.attackDamage);
        }

        // 공격 완료 후 이펙트 숨기기
        if (attackEffect != null)
        {
            Invoke("HideAttackEffect", 0.5f);
        }
    }

    private void HideAttackEffect()
    {
        if (attackEffect != null)
        {
            attackEffect.SetActive(false);
        }
    }

    public bool CanAttack(Transform target)
    {
        if (target == null) return false;
        float distance = Vector2.Distance(transform.position, target.position);
        return distance <= enemyData.attackRange;
    }

    public void OnAttackComplete()
    {
        if (enemyAI != null)
        {
            enemyAI.OnAttackComplete();
        }
    }
} 