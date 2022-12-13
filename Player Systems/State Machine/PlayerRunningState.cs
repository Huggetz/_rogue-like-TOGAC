
using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    public override void EnterState(PlayerStateController player)
    {
        Debug.Log("Entered Running State");
        player._chosingRunningState();
    }
    public override void OnUpdate(PlayerStateController player)
    {
       
       player.Turning();
        // Switch to jump
        if (Input.GetButtonDown("Jump") && player.isGrounded())
        {
            Debug.Log("You Pressed Jump");
            player.SwitchState(player.JumpingState);
        };
        // Return to Idle
        if (player.rb1.velocity.x == 0)
            player.SwitchState(player.IdleState);
        // Switch to rolling
        if (Input.GetKeyDown(KeyCode.LeftShift) && player._isRolling == false) 
          player.SwitchState(player.RollingState);
    }
    public override void ExitState(PlayerStateController player)
    {

    }
    public override void OnFixedUpdate(PlayerStateController player)
    {
        Movement(player);
    }
    public override void OnCollision(PlayerStateController player)
    {

    }
   public void Movement(PlayerStateController player)
    {
        player.rb1.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * player.speed, player.rb1.velocity.y);
    }
    public void PlayerMovementAnimation(PlayerStateController player)
    {
        if (player.tr1.localScale.x > 0)
        {
            player.ChangeAnimationState(player._runningRight);
        }
        else if (player.tr1.localScale.x < 0)
        {
            player.ChangeAnimationState(player._runningLeft);
        } 
        
     
    }
    
}
