using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    Vector2 moveDirection;
    float jumpDirection;
    public float moveSpeed = 2;
    public float maxForwardSpeed = 8;
    public float turnSpeed = 100;
    float desiredSpeed;
    float forwardSpeed;
    [SerializeField] float jumpSpeed = 3000;

    const float groundAccel = 5;
    const float groundDecel = 25;
    bool readyJump = false;
    bool onGround = true;

    Animator anim;
    Rigidbody rb;

    

    bool IsMoveInput
    {
        get { return !Mathf.Approximately(moveDirection.sqrMagnitude, 0f); }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        // Debug.Log(moveDirection); // D+1 , W+1 , S -1 , A -1
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpDirection = context.ReadValue<float>();
    }

    void Move(Vector2 direction)
    {
        float turnAmount = direction.x;
        float fDirection = direction.y;
        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        desiredSpeed = direction.magnitude * maxForwardSpeed * Mathf.Sign(fDirection); // Sign 함수 값이 양수이면 1 음수이면 -1
        float acceleration = IsMoveInput ? groundAccel : groundDecel;

        forwardSpeed = Mathf.MoveTowards(forwardSpeed, desiredSpeed, acceleration * Time.deltaTime);
        anim.SetFloat("ForwardSpeed", forwardSpeed);


        transform.Rotate(0, turnAmount *turnSpeed * Time.deltaTime, 0);
        //direction = direction * moveSpeed * Time.deltaTime;
        //transform.Translate(direction.x, 0, direction.y);
    }


    
    void Jump(float direction)
    {
        Debug.Log(direction);
        if (direction > 0 && onGround)
        {
            anim.SetBool("ReadyJump", true);
            readyJump = true;
            
        }
        else if (readyJump)
        {
            anim.SetBool("Launch", true);
            readyJump = false;
            anim.SetBool("ReadyJump", false);

        }

    }

    public void Launch()
    {
        rb.AddForce(0, jumpSpeed, 0);
        anim.SetBool("Launch", false);
        anim.applyRootMotion = false;
    }

    public void Land()
    {
        anim.SetBool("Land", false);
        anim.applyRootMotion = true;
        anim.SetBool("Launch", false);
    }


    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
    }

    [SerializeField] float groundRayDist = 2.7f;

    // Update is called once per frame
    void Update()
    {
        Move(moveDirection);
        Jump(jumpDirection);

        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * groundRayDist * 0.5f, -Vector3.up);
        Debug.DrawRay(transform.position + Vector3.up * groundRayDist * 0.5f, -Vector3.up * groundRayDist, Color.red);

        if (Physics.Raycast(ray, out hit, groundRayDist))
        {
            if (!onGround)
            {
                onGround = true;
                anim.SetBool("Land", true);
            }
        }
        else
        {
            onGround = false;
        }
    }


}
