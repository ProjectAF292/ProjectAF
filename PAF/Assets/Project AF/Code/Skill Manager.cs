using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Skill Settings")]
    public GameObject[] availableSkills; // 사용 가능한 스킬 프리팹들 (임의로 10개 등록 가능)
    private Dictionary<KeyCode, GameObject> assignedSkills = new Dictionary<KeyCode, GameObject>();

    public Transform attackPoint;   // 공격 히트박스 생성 위치
    public float attackDuration = 0.2f; // 히트박스 유지 시간

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

        // 초기 키 설정 (게임 시작 시 기본 스킬 설정)
        AssignSkill(KeyCode.Mouse0, availableSkills[0]); // 좌클릭
        AssignSkill(KeyCode.Mouse1, availableSkills[1]); // 우클릭
        AssignSkill(KeyCode.Q, availableSkills[2]);      // Q
        AssignSkill(KeyCode.E, availableSkills[3]);      // E
    }

    void Update()
    {
        AimTowardsMouse();

        // 키 입력을 감지하여 해당 키에 할당된 스킬 실행
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

        anim.SetTrigger("Attack");

        // 마우스 위치 가져오기
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // 2D 환경이므로 Z값 0으로 설정

        // 공격 방향 계산
        Vector2 attackDirection = (mousePos - transform.position).normalized;

        // ?? 공격 생성 거리 설정
        float spawnDistance = 1.5f; // 원하는 거리 값 설정
        Vector3 spawnPosition = transform.position + (Vector3)(attackDirection * spawnDistance);

        // 공격 프리팹 생성
        GameObject skill = Instantiate(skillPrefab, spawnPosition, Quaternion.identity);

        // 공격 방향으로 회전 적용
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        skill.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 공격 이동 처리 (속도 전달)
        Attack attackComponent = skill.GetComponent<Attack>();
        if (attackComponent != null)
        {
            attackComponent.Init(20f, attackDirection, 10f, attackDuration); // ? spawnDistance 제거
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
