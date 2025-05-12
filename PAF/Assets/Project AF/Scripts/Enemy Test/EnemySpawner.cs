using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnData
{
    public float spawnInterval = 5f;
    public float spawnRadius = 10f;
    public int maxEnemies = 5;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<SpawnData> spawnDataList = new List<SpawnData>();
    
    [SerializeField]
    private EnemyData enemyData;  // Inspector에서 설정할 EnemyData

    private float lastSpawnTime;
    private int currentEnemies;

    private void Start()
    {
        if (spawnDataList.Count == 0)
        {
            spawnDataList.Add(new SpawnData());
        }
        
        if (enemyData == null)
        {
            Debug.LogError("EnemyData is not assigned!");
            return;
        }

        lastSpawnTime = -spawnDataList[0].spawnInterval;  // 게임 시작 시 바로 스폰되도록
    }

    private void Update()
    {
        if (enemyData == null) return;

        // 현재 적의 수가 최대치보다 적고, 스폰 간격이 지났다면
        if (currentEnemies < spawnDataList[0].maxEnemies && 
            Time.time >= lastSpawnTime + spawnDataList[0].spawnInterval)
        {
            SpawnEnemy(enemyData);
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnEnemy(EnemyData enemyData)
    {
        // 스폰 위치 계산 (원형 범위 내 랜덤)
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float randomDistance = Random.Range(0f, spawnDataList[0].spawnRadius);
        Vector2 offset = new Vector2(
            Mathf.Cos(randomAngle) * randomDistance,
            Mathf.Sin(randomAngle) * randomDistance
        );
        Vector3 spawnPosition = transform.position + new Vector3(offset.x, offset.y, 0);

        // 몬스터 생성
        GameObject enemyObject = Instantiate(enemyData.enemyPrefab, spawnPosition, Quaternion.identity);
        
        // 기본 컴포넌트 설정
        TBaseEnemy enemy = enemyObject.GetComponent<TBaseEnemy>();
        if (enemy == null)
        {
            enemy = enemyObject.AddComponent<TBaseEnemy>();
        }

        // AI 컴포넌트 설정
        TBaseEnemyAI enemyAI = enemyObject.GetComponent<TBaseEnemyAI>();
        if (enemyAI == null)
        {
            enemyAI = enemyObject.AddComponent<TBaseEnemyAI>();
        }

        // Rigidbody2D 설정
        Rigidbody2D rb = enemyObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = enemyObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // EnemyData 설정
        enemy.SetEnemyData(enemyData);
            
        // 공격 타입에 따른 컴포넌트 추가
        switch (enemyData.attackType)
        {
            case AttackType.Melee:
                MeleeAttack meleeAttack = enemyObject.AddComponent<MeleeAttack>();
                enemy.AddAttackComponent(meleeAttack);
                break;
        }
            
        // 속성 컴포넌트 추가
        switch (enemyData.elementType)
        {
            case ElementType.Fire:
                FireAttribute fireAttribute = enemyObject.AddComponent<FireAttribute>();
                enemy.AddAttributeComponent(fireAttribute);
                break;
        }

        currentEnemies++;
        Debug.Log($"Spawned enemy with detection range: {enemyData.detectionRange}, attack range: {enemyData.attackRange}, move speed: {enemyData.moveSpeed}");
    }

    public void OnEnemyDestroyed()
    {
        currentEnemies--;
    }

    private void OnDrawGizmos()
    {
        // 스포너 위치 표시 (빨간색 구체)
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // 반투명 빨간색
        Gizmos.DrawSphere(transform.position, 0.5f);

        // 스폰 범위 표시 (노란색 원)
        if (spawnDataList != null && spawnDataList.Count > 0)
        {
            Gizmos.color = new Color(1f, 1f, 0f, 0.3f); // 반투명 노란색
            foreach (var spawnData in spawnDataList)
            {
                // XZ 평면에 원 그리기
                DrawCircle(transform.position, spawnData.spawnRadius, 32);
            }
        }
    }

    private void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angle = 0f;
        float angleStep = 360f / segments;
        Vector3 previousPoint = center + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * radius, Mathf.Sin(Mathf.Deg2Rad * angle) * radius, 0f);

        for (int i = 0; i <= segments; i++)
        {
            angle += angleStep;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * radius, Mathf.Sin(Mathf.Deg2Rad * angle) * radius, 0f);
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 스포너가 선택되었을 때만 추가 정보 표시
        if (spawnDataList != null && spawnDataList.Count > 0)
        {
            // 스폰 정보 표시
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, 
                $"Spawn Count: {spawnDataList[0].maxEnemies}\n" +
                $"Interval: {spawnDataList[0].spawnInterval}s\n" +
                $"Radius: {spawnDataList[0].spawnRadius}");
        }
    }
} 