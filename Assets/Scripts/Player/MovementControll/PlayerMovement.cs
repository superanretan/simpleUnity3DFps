using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMovement : MonoBehaviour
{
    private void Awake()
    {
        playerMain = GetComponent<PlayerMain>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetupInputTriggers();
        SetupStartState();
    }

    private void SetupStartState()      //setup start state because player always must be in any state
    {
        CurrentState = PlayerIdleState;
        CurrentState.EnterState(this);
    }

    private void Update()
    {
        CurrentState.UpdateState(this);
        CheckIsGrounded();
        HandleMovementInput();
        HandleRotation();
        UpdatePlayerMovingEvent();
    }

    private void UpdatePlayerMovingEvent()
    {
        OnPlayerIsMoving?.Invoke(GetPlayerSpeed());
    }
    
    private void SetupInputTriggers() //setup triggers for movement from players input system control scheme
    {
        playerMain.GetPlayerControls().PlayerMovement.Jump.performed += i => Jump(); //jump
        playerMain.GetPlayerControls().PlayerMovement.Sprint.performed += i => { isSprinting = true; }; //shift pressed to sprint
        playerMain.GetPlayerControls().PlayerMovement.Sprint.canceled += i => { isSprinting = false; }; //shift released 
    }

    public void SwitchState(PlayerBaseState state) //switch for player  states simple state machine
    {
        NewState = state;
        CurrentState.ExitState(this);
        CurrentState = NewState;
        CurrentState.EnterState(this);
    }

    public void HandleMovementInput()       //handle movement input and values for movement and events
    {
        Vector2 movementInput = playerMain.GetPlayerMovementVector();
        moveAmount = Mathf.Clamp01(Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.y));
        MoveVector.x = movementInput.x;
        MoveVector.z = movementInput.y;
    }
    
    private float GetPlayerSpeed()      //return player speed based on current player state
    {
        float speed = 0;
        if (CurrentState == PlayerWalkState)
            speed = walkSpeed;

        if (CurrentState == PlayerRunState)
            speed = runSpeed;

        if (CurrentState == PlayerIdleState)
            speed = 0;
        
        return speed;
    }
    
}