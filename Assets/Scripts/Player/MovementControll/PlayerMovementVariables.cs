using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMovement
{
    public delegate void PlayerIsMoving(float speed);
    public static event  PlayerIsMoving OnPlayerIsMoving;
    
    #region Player states for movement stateMachine

    public PlayerBaseState CurrentState;    //actual player state 
    public PlayerBaseState NewState;        

    public PlayerIdleState PlayerIdleState = new PlayerIdleState();     //state when player is in idle
    public PlayerWalkState PlayerWalkState = new PlayerWalkState();        //player walk state
    public PlayerJumpState PlayerJumpState = new PlayerJumpState();     //player jump state
    public PlayerRunState PlayerRunState = new PlayerRunState();        //player run state
    public PlayerFallingFromJumpState PlayerFallingFromJumpState = new PlayerFallingFromJumpState();    //falling state for transition from jump to idle
                                                                                //prevents from glitching when try to jump and check isGrounded
    #endregion

    #region References
    [HideInInspector] public PlayerMain playerMain;
    [HideInInspector] public Rigidbody rb;

    #endregion

    #region Movement variables
    [Header("---Player Movement Variables---")] 
    public float walkSpeed = 10;
    public float runSpeed = 15;
    public float jumpForce = 200f;

    [Tooltip("Distance from player pivot where start to raycast is grounded check")]
    public float isGroundedRaycastHeightOffset = 0.5f;

    [Tooltip("Distance how far to raycast checking is grounded")]
    public float isGroundedRaycastCheckDistance = 0.7f;

    public Vector3 MoveVector;
    public bool isGrounded;
    [HideInInspector] public bool isSprinting = false;
     public float moveAmount;
    
    [Tooltip("Layer to ray for check if player is grounded")]
    public LayerMask groundLayer;

    [Tooltip("Mouse rotation sensitivity")] 
    public float mouseSensitivity = 25f;

    #endregion

}