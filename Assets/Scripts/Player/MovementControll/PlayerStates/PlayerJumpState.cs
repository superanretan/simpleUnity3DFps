using System.Collections;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(PlayerMovement player)
    {
        player.rb.velocity += Vector3.up * player.jumpForce;
      //  player.rb.AddForce(Vector3.up * player.jumpForce);
    }

    public override void ExitState(PlayerMovement player)
    {
        
    }
    
    public override void UpdateState(PlayerMovement player)
    {
        if (player.rb.velocity.y <0)
        {
            player.SwitchState(player.PlayerFallingFromJumpState);
        }
    }
}