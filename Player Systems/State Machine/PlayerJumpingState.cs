
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public override void EnterState(PlayerStateController player)
    {
        Debug.Log("Entered Jumping State");
        Jumping(player);
        

    }
    public override void OnUpdate(PlayerStateController player)
    {
        player.Turning();

    }
    public override void ExitState(PlayerStateController player)
    {

    }
    public override void OnCollision(PlayerStateController player)
    {

    }
    public override void OnFixedUpdate(PlayerStateController player)
    {

    }
    private void Jumping(PlayerStateController player)
    {
        player.rb1.velocity = new Vector2(player.rb1.velocity.x, player.jumpforce);
        if (player.isGrounded())
            player.SwitchState(player.IdleState);        
    }


}
