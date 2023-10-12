using Frictionless;
using Game.Scripts;
using UI_InputSystem.Base;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform, groundChecker;

    [SerializeField]
    private Transform graphics;

    [SerializeField]
    private CharacterController controllerPlayer;

    [SerializeField]
    [Range(2f, 10f)]
    private float playerHorizontalSpeed = 8;

    [SerializeField]
    private bool useGravity = true;

    [SerializeField]
    [Range(-50f, -9.8f)]
    private float gravityValue = -10;

    [SerializeField]
    [Range(0.1f, 1f)]
    private float groundDistance = 0.5f;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private bool allowedJumping = true;

    [SerializeField]
    private float jumpHeight = 2;

    private float JumpForce => Mathf.Sqrt(jumpHeight * -2f * gravityValue);
    private Vector3 gravityVelocity;
    
    public bool Grounded => Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

    private void Awake()
    {
        ServiceFactory.RegisterSingleton(this);
    }

    private void Start()
    {
        UIInputSystem.ME.AddOnTouchEvent(ButtonAction.Jump, ProcessJumping);
    }

    private void OnDestroy()
    {
        UIInputSystem.ME.RemoveOnTouchEvent(ButtonAction.Jump, ProcessJumping);
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
        CalculateGravity();
    }
    
    private void MovePlayer()
    {
        if (!playerTransform) return;
        if (!ServiceFactory.Resolve<GameController>().IsGameRunning) return; 
        
        Vector3 directionVector3 = Vector3.zero;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            directionVector3 += playerHorizontalSpeed * Time.deltaTime * new Vector3(-1,0,0);
        } 
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            directionVector3 += playerHorizontalSpeed * Time.deltaTime *new Vector3(0,0,1);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            directionVector3 += playerHorizontalSpeed * Time.deltaTime *new Vector3(1,0,0);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            directionVector3 += playerHorizontalSpeed * Time.deltaTime *new Vector3(0,0,-1);
        }
        
        directionVector3 += PlayerMovementDirection();
        directionVector3.Normalize();
        directionVector3 *= playerHorizontalSpeed * Time.deltaTime;

        if (directionVector3.magnitude > 0.1f)
        {
            controllerPlayer.Move(directionVector3);
            graphics.transform.rotation = Quaternion.LookRotation(directionVector3);
        }
    }

    private void CalculateGravity() 
    {
        if (!useGravity) return;
        if (!groundChecker) return;

        ResetGravityIfGrounded();
        ApplyGravity();
    }

    private void ProcessJumping()
    {
        if (!allowedJumping) return;

        if (Grounded)      
            gravityVelocity.y = JumpForce;      
    }

    private void ApplyGravity()
    {
        gravityVelocity.y += gravityValue * Time.deltaTime;
        controllerPlayer.Move(gravityVelocity * Time.deltaTime);
        if (controllerPlayer.transform.position.y < 0)
        {
            controllerPlayer.transform.position = new Vector3(controllerPlayer.transform.position.x, 1,
                controllerPlayer.transform.position.z);
        }
    }

    private void ResetGravityIfGrounded()
    {
        if (Grounded && gravityVelocity.y < 0)
            gravityVelocity.y = -1.5f;
    }
    
    private Vector3 PlayerMovementDirection()
    {
        var baseDirection = playerTransform.right * UIInputSystem.ME.GetAxisHorizontal(JoyStickAction.Movement) +
                            playerTransform.forward * UIInputSystem.ME.GetAxisVertical(JoyStickAction.Movement);

        return baseDirection;
    }
}