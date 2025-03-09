using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Skill Settings")]
    public GameObject[] availableSkills; // ��� ������ ��ų �����յ� (���Ƿ� 10�� ��� ����)
    private Dictionary<KeyCode, GameObject> assignedSkills = new Dictionary<KeyCode, GameObject>();

    public Transform attackPoint;   // ���� ��Ʈ�ڽ� ���� ��ġ
    public float attackDuration = 0.2f; // ��Ʈ�ڽ� ���� �ð�

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public void UseSkillByKey(KeyCode key)
    {
        if (assignedSkills.ContainsKey(key))
        {
            UseSkill(assignedSkills[key]);
        }
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �ʱ� Ű ���� (���� ���� �� �⺻ ��ų ����)
        AssignSkill(KeyCode.Mouse0, availableSkills[0]); // ��Ŭ��
        AssignSkill(KeyCode.Mouse1, availableSkills[1]); // ��Ŭ��
        AssignSkill(KeyCode.Q, availableSkills[2]);      // Q
        AssignSkill(KeyCode.E, availableSkills[3]);      // E
    }

    void Update()
    {
        AimTowardsMouse();

        // Ű �Է��� �����Ͽ� �ش� Ű�� �Ҵ�� ��ų ����
        foreach (var key in assignedSkills.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                UseSkill(assignedSkills[key]);
            }
        }
    }


    void AimTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        if (direction.x != 0)
        {
            spriteRenderer.flipX = direction.x < 0;
        }
    }

    void UseSkill(GameObject skillPrefab)
    {
        if (skillPrefab == null) return;

        //anim.SetTrigger("Attack");

        // ���콺 ��ġ ��������
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // 2D ȯ���̹Ƿ� Z�� 0���� ����

        // ���� ���� ���
        Vector2 attackDirection = (mousePos - transform.position).normalized;

        // ?? ���� ���� �Ÿ� ����
        float spawnDistance = 1.5f; // ���ϴ� �Ÿ� �� ����
        Vector3 spawnPosition = transform.position + (Vector3)(attackDirection * spawnDistance);

        // ���� ������ ����
        GameObject skill = Instantiate(skillPrefab, spawnPosition, Quaternion.identity);

        // ���� �������� ȸ�� ����
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        skill.transform.rotation = Quaternion.Euler(0, 0, angle);

        // ���� �̵� ó�� (�ӵ� ����)
        Attack attackComponent = skill.GetComponent<Attack>();
        if (attackComponent != null)
        {
            attackComponent.Init(20f, attackDirection, 10f, attackDuration); // ? spawnDistance ����
        }
    }
    public void AssignSkill(KeyCode key, GameObject skillPrefab)
    {
        if (!assignedSkills.ContainsKey(key))
        {
            assignedSkills.Add(key, skillPrefab);
        }
        else
        {
            assignedSkills[key] = skillPrefab;
        }
    }



}
