using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollingState : PlayerBaseState
{

    public Vector2 _vel;
    public override void EnterState(PlayerStateController player)
    {
        
        Debug.Log("Rolling State");
        _vel = new Vector2((Input.GetAxisRaw("Horizontal") * player._RollingSpeed), player.rb1.velocity.y);
        if (player._turnedRight) player.ChangeAnimationState(player._rollingRight);
        else player.ChangeAnimationState(player._rollingLeft);
        _rolling(player);


    }
    public override void OnUpdate(PlayerStateController player)
    {
        
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

    public void _rolling(PlayerStateController player)
    {
        //player.rb1.velocity = _vel;
        player.StartCoroutine(player._rollingBackToIdle(this));
        

    }
   
    
}
