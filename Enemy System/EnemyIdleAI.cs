using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy AI/Idle")]
public class EnemyIdleAI : EnemyBaseAI
{
    
    
    public override void OnEnter(EnemyScript _enemyScript)
    {
        if (_enemyScript.transform.localScale.x > 0)
        _enemyScript._switchAnimation(_enemyScript._animationList._enemyIdleWalking_Right);
        else 
            _enemyScript._switchAnimation(_enemyScript._animationList._enemyIdleWalking_Left);
        Debug.Log("Enemy is Idle");
        _enemyScript._rigidbody.velocity = new Vector2(0, _enemyScript._rigidbody.velocity.y);
        _enemyScript._weaponIsDrawn = false;
    }
    public override void OnUpdate(EnemyScript _enemyScript)
    {
        
        _enemyScript._checkingForPlayer();
        _enemyScript._idleWalking();

        if (_enemyScript._rigidbody.velocity.x == 0)
        {
            if (_enemyScript.transform.localScale.x > 0)
                _enemyScript._switchAnimation(_enemyScript._animationList._enemyIdleStanding_Right);
            else _enemyScript._switchAnimation(_enemyScript._animationList._enemyIdleStanding_Left);

        }
            
        else if ((_enemyScript._rigidbody.velocity.x != 0) && (_enemyScript.transform.localScale.x > 0)) 
            _enemyScript._switchAnimation(_enemyScript._animationList._enemyIdleWalking_Right);
        else _enemyScript._switchAnimation(_enemyScript._animationList._enemyIdleWalking_Left);
    }
    public override void OnFixedUpdate(EnemyScript _enemyScript)
    {
        
    }
    public override void OnCollision(EnemyScript _enemyScript)
    {

    }
    public override void OnExit(EnemyScript _enemyScript)
    {

    }

    

}
