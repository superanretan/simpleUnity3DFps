using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
   public static InputManager Instance { get; private set; }
   
   private PlayerControls playerControls;
   
   private void Awake()
   {
      Instance = this;
      playerControls = new PlayerControls();
   }

   public PlayerControls GetPlayerControls()
   {
      return playerControls;
   }
   
   private void OnEnable()
   {
      playerControls.Enable();
   }

   private void OnDisable()
   {
      playerControls.Disable();
   }
   public Vector2 GetPlayerMovementVector() //for WASD steering and moveVector
   {
      return GetPlayerControls().PlayerMovement.Movement.ReadValue<Vector2>();
   }
    
   public Vector2 GetMouseDelta()  //for mouse look
   {
      return GetPlayerControls().PlayerMovement.Look.ReadValue<Vector2>();
   }
  

  
   
}
