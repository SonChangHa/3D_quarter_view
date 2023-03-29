using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float hAxis;
    float vAxis;

    public float speed = 15.0f;
    public float jumpSpeed = 5.0f;

    Rigidbody rb;
    Animator anim;

    Vector3 moveVector;

    bool isJump;
    bool isRun;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isJump = false;

        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //이동 구현
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        Move();

        //점프 구현
        if (!isJump && Input.GetKeyDown(KeyCode.Space))
            Jump();


        //쉬프트 클릭시 이속 2배
        isRun = Input.GetKey(KeyCode.LeftShift) && moveVector != Vector3.zero;



    }

    void Move()
    {
        moveVector = new Vector3(hAxis, 0, vAxis).normalized; // 벡터 정규화 꼭 해주어야함. 안그러면 대각선 이동 이상해짐
        transform.position += moveVector * (isRun ? 1.0f : 0.5f) * Time.deltaTime * speed;
        //벨로시티는 속도를 더해주는 방식이여서인지 제대로 이동이 안됨.
        //rb.velocity = moveVector;

        anim.SetBool("isWalk", moveVector != Vector3.zero);
        anim.SetBool("isRun", isRun);

        transform.LookAt(transform.position + moveVector); //방향 회전
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        anim.SetTrigger("doJump");
        isJump = true;
        anim.SetBool("isLand", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJump = false;
            anim.SetBool("isLand", true);
        }
    }
}
