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
        //�̵� ����
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        Move();

        //���� ����
        if (!isJump && Input.GetKeyDown(KeyCode.Space))
            Jump();


        //����Ʈ Ŭ���� �̼� 2��
        isRun = Input.GetKey(KeyCode.LeftShift) && moveVector != Vector3.zero;



    }

    void Move()
    {
        moveVector = new Vector3(hAxis, 0, vAxis).normalized; // ���� ����ȭ �� ���־����. �ȱ׷��� �밢�� �̵� �̻�����
        transform.position += moveVector * (isRun ? 1.0f : 0.5f) * Time.deltaTime * speed;
        //���ν�Ƽ�� �ӵ��� �����ִ� ����̿������� ����� �̵��� �ȵ�.
        //rb.velocity = moveVector;

        anim.SetBool("isWalk", moveVector != Vector3.zero);
        anim.SetBool("isRun", isRun);

        transform.LookAt(transform.position + moveVector); //���� ȸ��
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
