using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Move Set")]
    public Vector2 inputVec;
    public float speed;
    [Header("Dash Set")]
    public float dashSpeedMultiplier = 2f; // 대시 속도 배율
    public float dashDuration = 0.3f; // 대시 지속 시간
    public bool isDashing = false; // 대시 중 여부 확인
    public bool SetInvincible = false; // 대시 중 여부 확인
    
    CapsuleCollider2D coll2d;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    private float defaultSpeed; // 원래 이동 속도 저장

    

       
        void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll2d = GetComponent<CapsuleCollider2D>();
        defaultSpeed = speed; // 기본 이동 속도 저장

    }

    private void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }
                        
    }

    private void FixedUpdate()
    {
        
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // 플레이어 이동 입렵값을 일정하게 유지
        rigid.MovePosition(rigid.position + nextVec); //리지드 위치에 내가 입력한 좌표값 더한곳으로 이동
        rigid.velocity = new Vector2(inputVec.x, inputVec.y); // 이건 왜 작동하는거지
    }
    
        
                  
    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude); // 이동에 따라서 캐릭터가 바라보고 있는 방향 지정

        if (inputVec.x != 0) 
        {
            spriter.flipX = inputVec.x < 0; // 지금 백터x의 값이 0보다 작으면 스프라이트를 x 값으로 플립 해라
        }
    }

    // 대시 기능을 위한 코루틴
    IEnumerator Dash()
    {
        isDashing = true;
        speed *= dashSpeedMultiplier; // 이동 속도 증가

        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.SetInvincible(true); // 무적 상태 활성화
        }

        yield return new WaitForSeconds(dashDuration); // 일정 시간 동안 대시 유지

        speed = defaultSpeed; // 원래 속도로 복귀
        isDashing = false;

        if (health != null)
        {
            health.SetInvincible(false); // 무적 해제
        }
    }



}
