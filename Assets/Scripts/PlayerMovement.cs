using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    private InputAction moveAction;
    private InputAction jumpAction;

    public float moveSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public float rotationSpeed = 0.1f;
    public bool isGrounded;

    private CharacterController player;
    private PlayerInput input;
    private Vector3 position;
    private Transform moveCamera;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        moveCamera = Camera.main.transform;

        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        isGrounded = player.isGrounded;
        if (isGrounded && position.y < 0)
        {
            position.y = -1.0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        move = move.x * moveCamera.right.normalized +
              move.z * moveCamera.forward.normalized;
        move.y = 0f;

        player.Move(move * Time.deltaTime * moveSpeed);

        // Changes the height position of the player..
   
        if (jumpAction.triggered && isGrounded)
        {
            position.y += 6.0f;     //Change to acceptable jump
        }

        position.y += gravityValue * Time.deltaTime;
        player.Move(position * Time.deltaTime);

        // Rotate towards camera direction.
        if (input.x != 0 || input.y != 0)
        {
            float targetAngle = moveCamera.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }

}