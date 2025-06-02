using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public float walkingMoveSpeed;
    public float crawlingMoveSpeed;
    public float silentMoveSpeed;
    private float moveSpeed = 0;

    public float walkingRotationSpeed;
    public float crawlingRotationSpeed;
    public float silentRotationSpeed;
    private float rotationSpeed = 0;

    private Rigidbody rbComponent;
    private Animator animatorComponent;

    public RagdollController playerRagdollController;
    
    private BoxCollider boxCollider;
    private Vector3 originalSize;
    private Vector3 crouchedSize;
    private Vector3 originalCenter;
    private Vector3 crouchedCenter;

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

    void Start()
    {
        transform.position = GameManager.Instance.currentCheckPointPosition;
        
        boxCollider = GetComponent<BoxCollider>();
        originalSize = boxCollider.size;
        originalCenter = boxCollider.center;

        crouchedSize = new Vector3(originalSize.x, originalSize.y * 0.3f, originalSize.z);
        crouchedCenter = new Vector3(originalCenter.x, originalCenter.y * 0.3f, originalCenter.z);

        rbComponent = GetComponent<Rigidbody>();
        if (rbComponent == null)
        {
            Debug.LogWarning("No Rigidbody detected on " + gameObject.name);
        }

        animatorComponent = GetComponent<Animator>();
        if (animatorComponent == null)
        {
            Debug.LogWarning("No Animator detected on " + gameObject.name);
        }

        OnDying += GameManager.Instance.EndGame;
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

    public MoveState currentMoveState = MoveState.Idle;

    void Update()
    {
        if (currentMoveState == MoveState.Stun || currentMoveState == MoveState.GettingUp)
        {
            return;
        }

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


    void HandleStateTransition()
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
        string[] animationNames = { "isCrawling", "isWalking", "isRunning", "isWalkingSilently" };

        foreach (string anim in animationNames)
        {
            if (anim != animName)
            {
                animatorComponent.SetBool(anim, false);
            }
        }
    }

    public AnimationClip punch;
    public GameObject punchTrigger;

    private IEnumerator Punch()
    {
        animatorComponent.SetTrigger("Punch");
        ResetAllAnimationsExcept("");
        punchTrigger.SetActive(true);
        yield return new WaitForSeconds(punch.length);
        punchTrigger.SetActive(false);
        currentMoveState = MoveState.Idle;
    }

    public AnimationClip gettingUp;

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

    void ToggleCrouch()
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
    
    public event Action OnDying;
    
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
