using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runningMoveSpeed;
    public float walkingMoveSpeed;
    public float crawlingMoveSpeed;
    private float moveSpeed = 0;

    public float walkingRotationSpeed;
    public float crawlingRotationSpeed;
    public float runningRotationSpeed;
    private float rotationSpeed = 0;
    
    [SerializeField]
    private Transform CameraReferenceTransform;

    private Rigidbody rbComponent;
    private Animator animatorComponent;

    public RagdollController playerRagdollController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
        rbComponent = GetComponent<Rigidbody>();
        if (rbComponent == null)
        {
            Debug.LogWarning("No rb detected on" + gameObject.name);
        }

        animatorComponent = GetComponent<Animator>();
        if (animatorComponent == null)
        {
            Debug.LogWarning("No animator detected on" + gameObject.name);
        }
    }

    private enum MoveState
    {
        Idle,
        Walking,
        Crawling,
        Running,
        Punching,
        Stun,
        GettingUp
    }
    [SerializeField]
    private MoveState currentMoveState = MoveState.Idle;

    // Update is called once per frame
    void Update()
    {
        if (currentMoveState != MoveState.Stun && currentMoveState != MoveState.GettingUp)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 camForward = CameraReferenceTransform.forward;
            Vector3 deplacement = verticalInput * camForward + horizontalInput * CameraReferenceTransform.right;
            deplacement.y = 0;

            MoveState previousMoveState = currentMoveState;

            if (currentMoveState != MoveState.Punching)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    currentMoveState = MoveState.Crawling;
                }
                else if ((horizontalInput != 0 || verticalInput != 0) && Input.GetKey(KeyCode.LeftShift))
                {
                    currentMoveState = MoveState.Running;
                }
                else if (horizontalInput != 0 || verticalInput != 0)
                {
                    currentMoveState = MoveState.Walking;
                }
                else
                {
                    currentMoveState = MoveState.Idle;
                }
            }


            if (Input.GetKeyDown(KeyCode.F))
            {
                currentMoveState = MoveState.Punching;
            }


            if (currentMoveState != previousMoveState)
            {
                switch (currentMoveState)
                {
                    case MoveState.Crawling:
                        moveSpeed = crawlingMoveSpeed;
                        rotationSpeed = crawlingRotationSpeed;
                        animatorComponent.SetBool("isCrawling", true);
                        ResetAllAnimationsExcept("isCrawling");
                        break;
                    case MoveState.Walking:
                        moveSpeed = walkingMoveSpeed;
                        rotationSpeed = walkingRotationSpeed;
                        animatorComponent.SetBool("isWalking", true);
                        ResetAllAnimationsExcept("isWalking");
                        break;
                    case MoveState.Running:
                        moveSpeed = runningMoveSpeed;
                        rotationSpeed = runningRotationSpeed;
                        animatorComponent.SetBool("isRunning", true);
                        ResetAllAnimationsExcept("isRunning");
                        StartCoroutine(DrinkedRun());
                        break;
                    case MoveState.Idle:
                        moveSpeed = 0;
                        ResetAllAnimationsExcept("");
                        break;
                    case MoveState.Punching:
                        moveSpeed = 0;
                        rotationSpeed = 0;
                        animatorComponent.SetTrigger("Punch");
                        ResetAllAnimationsExcept("");
                        StartCoroutine(Punch());
                        break;
                }
            }

            Vector3 deplacementFinal = Vector3.ClampMagnitude(deplacement, 1) * moveSpeed;
            rbComponent.velocity = new Vector3(deplacementFinal.x, rbComponent.velocity.y, deplacementFinal.z);

            if (deplacement != Vector3.zero)
            {
                rbComponent.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(deplacement),
                    Time.deltaTime * rotationSpeed);
            }

        }

    }
    
    private void ResetAllAnimationsExcept(string animName)
    {
        string[] animationNames = { "isCrawling", "isWalking", "isRunning" };

        foreach (string anim in animationNames)
        {
            if (anim != animName)
            {
                animatorComponent.SetBool(anim, false);
            }
        }
    }

    public AnimationClip punch;
    private IEnumerator Punch()
    {
        yield return new WaitForSeconds(punch.length);
        currentMoveState = MoveState.Idle;
    }

    
    private IEnumerator DrinkedRun()
    {
        yield return new WaitForSeconds(5f);
        float random = Random.Range(0f, 1f);
        if (random > 0.5f)
        {
            currentMoveState = MoveState.Stun;
            playerRagdollController.EnableRagdoll(true);
            yield return new WaitForSeconds(3f);
            playerRagdollController.EnableRagdoll(false);
        }
        else if (currentMoveState == MoveState.Running)
        {
            StartCoroutine(DrinkedRun());
        }
    }

    public void GetUp()
    {
        currentMoveState = MoveState.GettingUp;
        animatorComponent.SetTrigger("GetUp");
        ResetAllAnimationsExcept("");
    }

}
