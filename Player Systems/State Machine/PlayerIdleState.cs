
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateController player)
    {
        Debug.Log("Entered Idle State");
       

        player._chosingIdleState();

    }
    public override void OnFixedUpdate(PlayerStateController player)
    {

    } 
    public override void OnUpdate(PlayerStateController player)
    {
        player.Turning();
        // if Changes to PlayerRunningState
       

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            
            player.SwitchState(player.RunningState);

        }
       // Jumping initiation
        if (Input.GetButtonDown("Jump") && player.isGrounded()) 
        {
            
            player.SwitchState(player.JumpingState);
        }
    }

    public override void ExitState(PlayerStateController player)
    {

    }
    public override void OnCollision(PlayerStateController player)
    {

    }
    
}
