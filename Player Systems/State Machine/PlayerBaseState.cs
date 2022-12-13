
using UnityEngine;

public abstract class PlayerBaseState
{


    public abstract void EnterState(PlayerStateController player);
    public abstract void OnUpdate(PlayerStateController player);
    public abstract void ExitState(PlayerStateController player);
    public abstract void OnCollision(PlayerStateController player);
    public abstract void OnFixedUpdate(PlayerStateController player);



}
