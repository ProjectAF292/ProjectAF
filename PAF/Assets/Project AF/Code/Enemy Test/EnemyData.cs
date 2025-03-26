using UnityEngine;

public enum AttackType
{
    Melee,
    Ranged,
    Magic
}

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    public EnemyType enemyType;
    public ElementType elementType;
    public AttackType attackType;
    public float maxHealth = 100f;

    [Header("AI Settings")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 2f;
    public float attackSpeed = 1f;
    public float attackDamage = 10f;

    [Header("Effect Settings")]
    public GameObject attackEffectPrefab;
    public Vector2 attackEffectScale = new Vector2(1.5f, 1.5f);
    public GameObject skillEffectPrefab;

    [Header("Animation")]
    public RuntimeAnimatorController animatorController;

    [Header("Prefab")]
    public GameObject enemyPrefab;
}

public enum EnemyType
{
    Normal,
    Elite,
    Boss
}

public enum ElementType
{
    Fire,
    Water,
    Wood,
    Metal,
    Earth
} 