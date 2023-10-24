using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerMovement
{
    private void Jump()
    {
        if (isGrounded)
            SwitchState(PlayerJumpState);
    }

    public void HandleMovement()                //move player on XZ axis by changing velocity
    {
        Vector2 movementInput = playerMain.GetPlayerMovementVector();
        Vector3 move = playerMain.GetPlayerCamera().transform.right * movementInput.x +
                       playerMain.GetPlayerCamera().transform.forward * movementInput.y;
        move.Normalize();
        
        if (isGrounded)
            move.y = 0;
        
        rb.velocity = move * GetPlayerSpeed();
    }

    public void HandleRotation()            //rotate player using mouse delta
    {
        var mouseDelta = playerMain.GetMouseDelta();
        if (mouseDelta.x > 300) return;
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        rb.transform.Rotate(Vector3.up * mouseX);
    }
}