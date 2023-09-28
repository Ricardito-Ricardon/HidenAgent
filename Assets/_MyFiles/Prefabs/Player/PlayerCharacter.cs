using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    [SerializeField] private joystick moveStick;
    [SerializeField] private joystick aimStick;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 30f;
    [SerializeField] float turnAnimationSmoothLerpFactor = 10f;
    [SerializeField] CameraRig cameraRig;
    CharacterController characterController;
    InventoryComponent inventoryComponent;
    Vector2 moveInput;
    Vector2 aimInput;

    Vector3 moveDir;
    Vector3 aimDir;

    Camera viewCamera;

    Animator animator;

    float animationTurnSpeed = 0f;

    public void SwitchWeapon()
    {
        inventoryComponent.NextWeapon();
    }

    private void Awake()
    {
        moveStick.onInputValueChanged += MoveInputUpdated;
        aimStick.onInputValueChanged += AimInputUpdated;
        aimStick.onStickTapped += AimStickTapped;
        // Initializing values
        characterController = GetComponent<CharacterController>();
        viewCamera = Camera.main;
        animator = GetComponent<Animator>();
        inventoryComponent = GetComponent<InventoryComponent>();
    }

    private void AimStickTapped()
    {
        animator.SetTrigger("switchWeapon");
    }

    private void AimInputUpdated(Vector2 inputVal)
    {
        aimInput = inputVal;
        aimDir = ConvertInputToWorldDirection(aimInput);
    }

    private void MoveInputUpdated(Vector2 inputVal)
    {
        moveInput = inputVal;
        moveDir = ConvertInputToWorldDirection(moveInput);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMoveInput();
        ProcessAimInput();
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        float rightSpeed = Vector3.Dot(moveDir, transform.right);
        float forwardSpeed = Vector3.Dot(moveDir, transform.forward);

        animator.SetFloat("leftSpeed", -rightSpeed);
        animator.SetFloat("forSpeed", forwardSpeed);

        animator.SetBool("firing", aimInput.magnitude > 0);
    }

    private void ProcessAimInput()
    {
        Vector3 lookDir = aimDir.magnitude != 0 ? aimDir : moveDir;
        float goalAnimTurnSpeed = 0f;
        if (lookDir.magnitude != 0)
        {
            Quaternion prevRot = transform.rotation; // after rotate
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir, Vector3.up), Time.deltaTime * turnSpeed);
            Quaternion newRot = transform.rotation; // before rotate

            float rotationDelta = Quaternion.Angle(prevRot, newRot); // How much we have rotated in the frame
            float rotateDir = Vector3.Dot(lookDir, transform.right) > 0 ? 1 : -1;

            goalAnimTurnSpeed = rotationDelta / Time.deltaTime;
        }

        animationTurnSpeed = Mathf.Lerp(animationTurnSpeed, goalAnimTurnSpeed, Time.deltaTime * turnAnimationSmoothLerpFactor);
        if(animationTurnSpeed < 0.1f) 
        {
            animationTurnSpeed = 0f;
        }
        animator.SetFloat("turnSpeed", animationTurnSpeed);

    }
    private void ProcessMoveInput()
    {

        characterController.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        if(aimDir.magnitude == 0)
        {
        cameraRig.AddYawInput(moveInput.x);
        }
    }

    Vector3 ConvertInputToWorldDirection(Vector2 inputVal)
    {
        Vector3 rightDir = viewCamera.transform.right;
        Vector3 upDir = Vector3.Cross(rightDir, Vector3.up);

        return (rightDir * inputVal.x + upDir * inputVal.y).normalized;
    }

    public void DamagePoint()
    {
        inventoryComponent.DamagePoint();
    }

}
