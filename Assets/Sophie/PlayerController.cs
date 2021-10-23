using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    Vector2 moveDirection;
    public float moveSpeed = 2;
    public float maxForwardSpeed = 8;
    public float turnSpeed = 100;
    float desiredSpeed;
    float forwardSpeed;

    const float groundAccel = 5;
    const float groundDecel = 25;


    Animator anim;

    bool IsMoveInput
    {
        get { return !Mathf.Approximately(moveDirection.sqrMagnitude, 0f); }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        // Debug.Log(moveDirection); // D+1 , W+1 , S -1 , A -1
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
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(moveDirection);
    }
}
