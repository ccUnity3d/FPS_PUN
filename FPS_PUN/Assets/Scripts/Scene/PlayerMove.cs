using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    private float moveSpeed =6f;
    private float rotateSpeed=10f;
    private float jumpVelocity = 5.0f;
    private float minRotate = -45;
    private float maxRotate = 45;
    private Animator animator;
    private Camera playCamera;
    private float mouseRotateX;
    private CapsuleCollider capsuleCollider;
    private Rigidbody rigibody;
    private bool isGround;
    // Use this for initialization
    void Start () {
        playCamera = Camera.main;
        mouseRotateX = playCamera.transform.localEulerAngles.x;
        rigibody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float rv = Input.GetAxisRaw("Mouse Y");
        float rh = Input.GetAxisRaw("Mouse X");

        Move(h,v);
        Rotate(rv, rh);
        CheckJump(isGround);
    }
    void FixedUpdate() {
        isGround = CheckGround();
        if (isGround==false) {
            animator.SetBool("isJump",false);
        };
    }
    void Move(float h,float v) {
        transform.Translate((Vector3.forward*v+Vector3.right*h)*moveSpeed*Time.deltaTime);
        if (h != 0.0f || v != 0.0f)
        {
            animator.SetBool("isMove", true);
        }
        else{
            animator.SetBool("isMove", false);
        }
    }
    void Rotate(float y,float x) {
        transform.Rotate(Vector3.up*x* rotateSpeed);
        mouseRotateX -= y * rotateSpeed;
        mouseRotateX = Mathf.Clamp(mouseRotateX,minRotate,maxRotate);
        playCamera.transform.localEulerAngles = new Vector3(mouseRotateX,0,0);
    }

    bool CheckGround() {
        RaycastHit hitInfo;
        float shellOffset = 0.01f;
        float radius = capsuleCollider.height / 2 - shellOffset;

        float maxDistance = capsuleCollider.height / 2;
        Vector3 currentPos = transform.position;
        currentPos.y += capsuleCollider.height / 2;
        return Physics.SphereCast(currentPos, radius, Vector3.down, out hitInfo, maxDistance, ~0, QueryTriggerInteraction.Ignore);
    }
    bool CheckJump(bool isGround) {

        if (Input.GetButtonDown("Jump")&&isGround)
        {
            rigibody.AddForce(Vector3.up*jumpVelocity,ForceMode.VelocityChange);
            animator.SetBool("isJump", true);
        }
        else
        {
            animator.SetBool("isJump", false);
        }
        return false;
    }
}
