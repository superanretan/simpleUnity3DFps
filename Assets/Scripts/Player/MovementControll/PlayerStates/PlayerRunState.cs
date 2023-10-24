public class PlayerRunState : PlayerBaseState
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
                player.SwitchState(player.PlayerWalkState);
            else
                player.HandleMovement();
            
        }
    }
}