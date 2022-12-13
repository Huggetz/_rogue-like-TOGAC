using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public override void EnterState(PlayerStateController player)
    {
        player.transform.Find("Aim").gameObject.SetActive(false);
        Debug.Log("You are dead");
        player.rb1.velocity = new Vector2(0, player.rb1.velocity.y);
        //player.ChangeAnimationState(player.Player_Dead);
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
}
