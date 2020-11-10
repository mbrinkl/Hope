using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Animator anim;
    public CharacterController controller;
    public Transform cam;

    public float speed = 3;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    Vector3 velocity;
    bool isGrounded;
    bool isEquipped = false;
    bool isDodging = false;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isBlocking = false;
    bool isAttacking = false;
    bool isRunning = false;
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    // Update is called once per frame
    void Update()
    {
        //jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded && isBlocking == false && isAttacking == false)
        {
            anim.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if (Input.GetButtonDown("Fire3") && isGrounded && isBlocking == false)
        {
            isRunning = true;
            speed = 8;
        }

        if(Input.GetButtonUp("Fire3") && isGrounded && isBlocking == false)
        {
            isRunning = false;
            speed = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && isEquipped == true)
        {
            isEquipped = false;
            anim.SetTrigger("Unequip");
            anim.SetBool("Equipped", isEquipped);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && isEquipped == false)
        {
            isEquipped = true;
            anim.SetTrigger("Equip");
            anim.SetBool("Equipped", isEquipped);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isAttacking == false && isEquipped == true)
        {
            isAttacking = true;
            speed = 0;
            anim.SetTrigger("Attack");
            StartCoroutine(WaitForAttack());
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isBlocking = true;
            anim.SetBool("isBlocking", true);
            anim.SetTrigger("Block");
            speed = 0;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            speed = 3;
            isBlocking = false;
            anim.SetBool("isBlocking", false);
        }

        if (Input.GetKeyDown(KeyCode.C) && isEquipped == true && isDodging == false)
        {
            anim.SetTrigger("Dodge");
            isDodging = true;
            speed = 12;
            StartCoroutine(WaitForDodge());
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && isBlocking == false)
        {
            //speed = 4;
        }

        if (isGrounded == true)
        {
            anim.SetBool("isGrounded",true);
        }

        if (isGrounded == false)
        {
            anim.SetBool("isGrounded", false);
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //walk
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        

        IEnumerator WaitForAttack()
        {
            yield return new WaitForSecondsRealtime(1);
            isAttacking = false;
            if (isAttacking == true || isBlocking == true)
            {
                speed = 0;
            }
            else if (isRunning == true)
            {
                speed = 8;
            }
            else
            {
                speed = 3;
            }
        }

        IEnumerator WaitForDodge()
        {
            yield return new WaitForSecondsRealtime(1);
            isDodging = false;
            if (isRunning == true)
                speed = 8;
            else
                speed = 3;
        }
    }
}
