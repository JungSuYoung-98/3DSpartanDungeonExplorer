using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private float curSpeed;
    private Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraBox;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    private Vector3 startTransfrom;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rigidbody;
    public Action inventory;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        startTransfrom = transform.position;
    }

    private void FixedUpdate()
    {
        Move();
        Dash();
        Fall();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded() && CharacterManager.Instance.Player.condition.CurStamina(10))
        {
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            CharacterManager.Instance.Player.condition.UseStamina(10);

        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed && CharacterManager.Instance.Player.condition.CurStamina(5))
        {
            moveSpeed *= 2;
            curSpeed = moveSpeed;
        }
        else if (context.phase == InputActionPhase.Canceled && moveSpeed == curSpeed)
        {
            moveSpeed /= 2;
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    // 대쉬
    private void Dash()
    {
        if(curSpeed == moveSpeed)
            CharacterManager.Instance.Player.condition.UseStamina(1f);

        if (!CharacterManager.Instance.Player.condition.CurStamina(5) && curSpeed != 0)
        {
            curSpeed = 0;
            moveSpeed /= 2;
        }
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraBox.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 1.6f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    // 스테이지 이탈
    public void Fall()
    {
        if(this.gameObject.transform.position.y < 0)
        {
            this.gameObject.transform.position = startTransfrom;
            CharacterManager.Instance.Player.condition.TakePhysicalDamage(10);
        }
    }

    // 점프대
    public void JumpingPlayform(int power)
    {
        rigidbody.AddForce(Vector2.up * power, ForceMode.Impulse);
    }

    // 인벤토리 버튼
    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

}