using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 7.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform barrelTransform;
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private float bulletHitMissDistance = 25f;
    [SerializeField]
    private float animationSmoothTime = 0.05f;
    [SerializeField]
    private float animationPlayTransition = 0.15f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;

    private Animator animator;
    int jumpAnimationParam;
    int moveXAnimationParam;
    int moveZAnimationParam;

    Vector2 currentAnimationBlend;
    Vector2 animationVelocity;

    private void Awake()
    {   

        // to access the controls from player Input
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        cameraTransform = Camera.main.transform;


        // to get the input of player
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];

        Cursor.lockState = CursorLockMode.Locked;

        // Animatons
        animator = GetComponent<Animator>();
        jumpAnimationParam = Animator.StringToHash("Pistol Jump");
        moveXAnimationParam = Animator.StringToHash("MoveX");
        moveZAnimationParam = Animator.StringToHash("MoveZ");
        
    }

   
    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
    }

    private void ShootGun()
    {
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            
            bulletController.target = hit.point;
            bulletController.hit = true;

        }

        else
        {
            bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
        }
    }

    

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        
        // to read value
        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlend = Vector2.SmoothDamp(currentAnimationBlend, input, ref animationVelocity, animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlend.x, 0, currentAnimationBlend.y);
        // to move in the direction of camera
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Blend Strafe Animations
        animator.SetFloat(moveXAnimationParam, currentAnimationBlend.x);
        animator.SetFloat(moveZAnimationParam, currentAnimationBlend.y);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimationParam, animationPlayTransition);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


         
        // Rotate towards camera direction
        //float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed *Time.deltaTime );

       
    }
}
