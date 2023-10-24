public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerMovement player)
    {
       
    }

    public override void ExitState(PlayerMovement player)
    {
       
    }

    public override void UpdateState(PlayerMovement player)
    {
        if(player.MoveVector.magnitude != 0 && player.isGrounded)
             player.SwitchState(player.PlayerWalkState);
     
    }
}
