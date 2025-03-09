using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

     

    
    public Vector2 inputVec;
    public float speed;

    [Header("회피 세팅")]
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float invincibleTime = 0.3f;
    private bool isDashing = false;
    private bool isInvincible = false;

    [Header("공격 관련")]
    public Transform attackPoint;   // 공격을 생성할 위치
    public SkillManager skillManager; // SkillManager 참조


    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    WaitForFixedUpdate wait;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
         wait = new WaitForFixedUpdate();
    }

        private void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }



    void Update()
    {
        AimTowardsMouse();

        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            Attack(KeyCode.Mouse0);
        }
        if (Input.GetMouseButtonDown(1)) // 우클릭
        {
            Attack(KeyCode.Mouse1);
        }
        if (Input.GetKeyDown(KeyCode.Q)) // Q
        {
            Attack(KeyCode.Q);
        }
        if (Input.GetKeyDown(KeyCode.E)) // E
        {
            Attack(KeyCode.E);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }
    IEnumerator Dash()
    {
        isDashing = true;
        isInvincible = true;

        float originalSpeed = speed;
        speed = dashSpeed;

        anim.SetTrigger("Dash");

        yield return new WaitForSeconds(dashDuration);

        speed = originalSpeed;
        isDashing = false;

        yield return new WaitForSeconds(invincibleTime - dashDuration);
        isInvincible = false;
    }

    void AimTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;

        if (direction.x != 0)
        {
            spriter.flipX = direction.x < 0;
        }
    }

    void Attack(KeyCode key)
    {
        //anim.SetTrigger("Attack");

        if (skillManager != null)
        {
            skillManager.UseSkillByKey(key);
        }
    }


    // 무적 상태 확인 함수 추가
    public bool IsInvincible()
    {
        return isInvincible;
    }

    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0) // �������� ���� ĳ���� �̹��� x ������ ����
        {
            spriter.flipX = inputVec.x < 0;   
        }
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
       
             
         
}
