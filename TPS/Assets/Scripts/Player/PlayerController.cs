using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkingMoveSpeed;
    [SerializeField] private float crawlingMoveSpeed;
    [SerializeField] private float silentMoveSpeed;
    private float moveSpeed = 0;

    [SerializeField] private float walkingRotationSpeed;
    [SerializeField] private float crawlingRotationSpeed;
    [SerializeField] private float silentRotationSpeed;
    private float rotationSpeed = 0;

    private Rigidbody rbComponent;
    private Animator animatorComponent;

    [SerializeField] private RagdollController playerRagdollController;
    [SerializeField] private AnimationClip punch;
    [SerializeField] private GameObject punchTrigger;
    [SerializeField] private AnimationClip gettingUp;
    
    private BoxCollider boxCollider;
    private Vector3 originalSize;
    private Vector3 crouchedSize;
    private Vector3 originalCenter;
    private Vector3 crouchedCenter;
    
    public event Action OnDying;

    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        LoadCheckPointPosition();
        
        rbComponent = GetComponent<Rigidbody>();
        animatorComponent = GetComponent<Animator>();
        
        InitCrouchedCollider();

        OnDying += GameManager.Instance.EndGame;
    }

    private void LoadCheckPointPosition()
    {
        transform.position = GameManager.Instance.currentCheckPointPosition;
    }

    private void InitCrouchedCollider()
    {
        boxCollider = GetComponent<BoxCollider>();
        originalSize = boxCollider.size;
        originalCenter = boxCollider.center;
        
        crouchedSize = new Vector3(originalSize.x, originalSize.y * 0.3f, originalSize.z);
        crouchedCenter = new Vector3(originalCenter.x, originalCenter.y * 0.3f, originalCenter.z);
    }

    public enum MoveState
    {
        Idle,
        Walking,
        Crawling,
        Punching,
        Stun,
        GettingUp,
        WalkingSilently
    }

    public MoveState currentMoveState { get; private set; }= MoveState.Idle;

    private void Update()
    {
        if (currentMoveState == MoveState.Stun || currentMoveState == MoveState.GettingUp) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 deplacement = verticalInput * Camera.main.gameObject.transform.forward + horizontalInput * Camera.main.gameObject.transform.right;
        deplacement.y = 0;

        MoveState previousMoveState = currentMoveState;

        if (Input.GetKeyDown(KeyCode.F) && currentMoveState != MoveState.Crawling)
        {
            currentMoveState = MoveState.Punching;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && currentMoveState != MoveState.Crawling && currentMoveState != MoveState.Idle)
        {
            currentMoveState = MoveState.Crawling;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && currentMoveState == MoveState.Crawling)
        {
            StartCoroutine(GetUp());
        }
        else if (currentMoveState != MoveState.Crawling && currentMoveState != MoveState.Punching)
        {
            if (horizontalInput != 0 || verticalInput != 0)
            {
                if (Input.GetKey(KeyCode.P))
                {
                    currentMoveState = MoveState.WalkingSilently;
                }
                else
                {
                    currentMoveState = MoveState.Walking;
                }
            }
            else
            {
                currentMoveState = MoveState.Idle;
            }
        }

        if (currentMoveState != previousMoveState)
        {
            HandleStateTransition();
        }

        Vector3 deplacementFinal = Vector3.ClampMagnitude(deplacement, 1) * moveSpeed;
        
        rbComponent.velocity = new Vector3(deplacementFinal.x, rbComponent.velocity.y, deplacementFinal.z);

        if (deplacement != Vector3.zero)
        {
            rbComponent.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(deplacement), Time.deltaTime * rotationSpeed);
        }
    }


    private void HandleStateTransition()
    {
        switch (currentMoveState)
        {
            case MoveState.Crawling:
                moveSpeed = crawlingMoveSpeed;
                rotationSpeed = crawlingRotationSpeed;
                animatorComponent.SetBool("isCrawling", true);
                ResetAllAnimationsExcept("isCrawling");
                ToggleCrouch();
                break;

            case MoveState.Walking:
                moveSpeed = walkingMoveSpeed;
                rotationSpeed = walkingRotationSpeed;
                animatorComponent.SetBool("isWalking", true);
                ResetAllAnimationsExcept("isWalking");
                break;
            
            case MoveState.WalkingSilently:
                moveSpeed = silentMoveSpeed;
                rotationSpeed = silentRotationSpeed;
                animatorComponent.SetBool("isWalkingSilently", true);
                ResetAllAnimationsExcept("isWalkingSilently");
                break;

            case MoveState.Idle:
                moveSpeed = 0;
                ResetAllAnimationsExcept("");
                break;

            case MoveState.Punching:
                moveSpeed = 0;
                rotationSpeed = 0;
                StartCoroutine(Punch());
                break;
        }
    }

    private void ResetAllAnimationsExcept(string animName)
    {
        string[] animationNames = { "isCrawling", "isWalking", "isWalkingSilently" };

        foreach (string anim in animationNames)
        {
            if (anim != animName)
            {
                animatorComponent.SetBool(anim, false);
            }
        }
    }

    private IEnumerator Punch()
    {
        animatorComponent.SetTrigger("Punch");
        ResetAllAnimationsExcept("");
        punchTrigger.SetActive(true);
        yield return new WaitForSeconds(punch.length);
        punchTrigger.SetActive(false);
        currentMoveState = MoveState.Idle;
    }

    private IEnumerator GetUp()
    {
        if (IsEnoughSpaceToStand())
        {
            currentMoveState = MoveState.GettingUp;
            animatorComponent.SetTrigger("GetUp");
            ResetAllAnimationsExcept("");
            yield return new WaitForSeconds(gettingUp.length);
            currentMoveState = MoveState.Idle;
            ToggleCrouch();
        }
    }

    private void ToggleCrouch()
    {
        if (currentMoveState == MoveState.Crawling)
        {
            boxCollider.size = crouchedSize;
            boxCollider.center = crouchedCenter;
        }
        else
        {
            boxCollider.size = originalSize;
            boxCollider.center = originalCenter;
        }
    }
    
    public void Dying()
    {
        OnDying?.Invoke();
        
        playerRagdollController.EnableRagdoll(true);
        this.enabled = false;
    }

    private bool IsEnoughSpaceToStand()
    {
        Vector3 rayStart = transform.position + Vector3.up * (crouchedSize.y / 2);
        float requiredHeight = originalSize.y - crouchedSize.y;
        return !Physics.Raycast(rayStart, Vector3.up, requiredHeight);
    }
}
