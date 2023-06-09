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

    Camera mainCam;

    Vector3 moveVector;

    bool isJump;
    bool isRun;
    bool isBorder;

    float interval = 0.25f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;


    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        isJump = false;

        anim = GetComponentInChildren<Animator>();

        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //이동 구현
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        KeyBoard();
        Move();

        //점프 구현
        if (!isJump && Input.GetKeyDown(KeyCode.Space))
            Jump();


        //쉬프트 클릭시 이속 2배
        isRun = Input.GetKey(KeyCode.LeftShift) && moveVector != Vector3.zero;

        if (Input.GetMouseButton(0))
        {
            Turn();
        }
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, moveVector * 5, Color.red);
        isBorder = Physics.Raycast(transform.position, moveVector, 5, LayerMask.GetMask("Wall"));
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
        StopToWall();
    }


    void Turn()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, 100))
        {
            Debug.DrawRay(transform.position, rayHit.point, Color.red);
            Vector3 nextVector = rayHit.point - transform.position;
            nextVector.y = 0;
            transform.LookAt(transform.position + nextVector);
        }
    }

    void KeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            if ((Time.time - doubleClickedTime) < interval)
            {
                isDoubleClicked = true;
                doubleClickedTime = -1.0f;
            }
            else
            {
                isDoubleClicked = false;
                doubleClickedTime = Time.time;
            }
        }
    }

    void Move()
    {
        moveVector = new Vector3(hAxis, 0, vAxis).normalized; // 벡터 정규화 꼭 해주어야함. 안그러면 대각선 이동 이상해짐
        //벨로시티는 속도를 더해주는 방식이여서인지 제대로 이동이 안됨.
        //rb.velocity = moveVector;

        if (isDoubleClicked)
        {
            anim.SetTrigger("doDodge");
            if(!isBorder)
                transform.position += moveVector * 2 * (isRun ? 1.0f : 0.5f) * Time.deltaTime * speed;
            StartCoroutine(doubleStop());
        } else
        {
            anim.SetBool("isWalk", moveVector != Vector3.zero);
            anim.SetBool("isRun", isRun);
            if (!isBorder)
                transform.position += moveVector * (isRun ? 1.0f : 0.5f) * Time.deltaTime * speed;
        }

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

    IEnumerator doubleStop()
    {
        yield return new WaitForSeconds(0.25f);

        isDoubleClicked = false;

    }

    
}
