using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletMiniGame : MonoBehaviour
{
    [SerializeField] private PeePoolingSystem peeGenerator;
    
    [SerializeField] private float minUpwardForce = -1f;
    [SerializeField] private float maxUpwardForce = 0f;
    [SerializeField] private float verticalSpeed = 0.01f;
    [SerializeField] private float smoothSpeed = 5f;
    private float targetUpwardForce;
    [SerializeField] private float horizontalSpeed = 45f;
    
    private void Start()
    {
        
        targetUpwardForce = peeGenerator.upwardForce;
    }

    private void MoveVertically(float direction)
    {
        targetUpwardForce = Mathf.Clamp(targetUpwardForce + direction * verticalSpeed * Time.deltaTime, minUpwardForce, maxUpwardForce);
    }
    
    private void RotateHorizontally(float direction)
    {
        peeGenerator.transform.Rotate(0f, direction * horizontalSpeed * Time.deltaTime, 0f, Space.Self);
    }

    private void Update()
    {
        float verticalInput = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) verticalInput = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) verticalInput = -1f;
        if (verticalInput != 0f)
        {
            MoveVertically(verticalInput);
        }
        peeGenerator.upwardForce = Mathf.Lerp(
            peeGenerator.upwardForce,
            targetUpwardForce, 
            smoothSpeed * Time.deltaTime);
        
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow)) horizontalInput = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow)) horizontalInput = -1f;

        if (horizontalInput != 0f)
        {
            RotateHorizontally(horizontalInput);
        }
    }
}
