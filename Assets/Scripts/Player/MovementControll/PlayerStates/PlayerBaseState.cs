using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerMovement player);
    public abstract void ExitState(PlayerMovement player);
    public abstract void UpdateState(PlayerMovement player);
}