using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy AI/Melee Attack Module")]
public class EnemyMeleeAI : EnemyBaseAI
{
    public override void OnEnter(EnemyScript _enemyScript)
    {
        
    }
    
    public override void OnUpdate(EnemyScript _enemyScript)
    {
        _movingIntoDistance(_enemyScript);
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
    public void _movingIntoDistance (EnemyScript _enemyScript)
    {
        if (Mathf.Abs(_enemyScript._playerReferenceCollider.transform.position.x - _enemyScript.transform.position.x) > _enemyScript._config._meleeRange)
        {
            _enemyScript._playerFoundModule._movingToPlayer(_enemyScript);
            Debug.Log("MeleeRange is " + _enemyScript._config._meleeRange);
            Debug.Log("Distance is " + (_enemyScript._playerReferenceCollider.transform.position.x - _enemyScript.transform.position.x));
        }
        else if (Mathf.Abs(_enemyScript._playerReferenceCollider.transform.position.x - _enemyScript.transform.position.x) <= _enemyScript._config._meleeRange)
        {
            _enemyScript._playerFoundModule._stopping(_enemyScript);
            if (_enemyScript._readyToAttack) _enemyScript._startMeleeAttack();
            // Debug.Log("Stopping in motion " + _enemyScript._rigidbody.velocity.x);
        }
    }
}
