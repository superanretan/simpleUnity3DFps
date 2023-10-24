public class PlayerWalkState : PlayerBaseState
{
    public override void EnterState(PlayerMovement player)
    {
     
    }

    public override void ExitState(PlayerMovement player)
    {
    }

    public override void UpdateState(PlayerMovement player)
    {
        if (player.MoveVector.magnitude == 0)
            player.SwitchState(player.PlayerIdleState);
        else
        {
            if (!player.isSprinting)
                player.HandleMovement();
            else
                player.SwitchState(player.PlayerRunState);
        }
    }
}
