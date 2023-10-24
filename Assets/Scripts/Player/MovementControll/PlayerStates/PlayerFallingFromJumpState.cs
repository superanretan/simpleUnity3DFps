using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingFromJumpState : PlayerBaseState
{
    public override void EnterState(PlayerMovement player)
    {
        
    }

    public override void ExitState(PlayerMovement player)
    {
        
    }

    public override void UpdateState(PlayerMovement player)
    {
       if(player.isGrounded)
           player.SwitchState(player.PlayerIdleState);
    }
}
