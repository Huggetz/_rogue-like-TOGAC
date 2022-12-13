using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy AI/Ranged Attack Module")]
public class EnemyRangedAI : EnemyBaseAI
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
    public void _movingIntoDistance(EnemyScript _enemyScript)
    {
        if (Mathf.Abs(_enemyScript._playerReferenceCollider.transform.position.x - _enemyScript.transform.position.x) > _enemyScript._config._rangedRange)
        {
            _enemyScript._playerFoundModule._movingToPlayer(_enemyScript);
            if (!_enemyScript._shootingAnimationRunning) _enemyScript._weaponDrawnState();
        }
        else if (Mathf.Abs(_enemyScript._playerReferenceCollider.transform.position.x - _enemyScript.transform.position.x) <= 0.75 * _enemyScript._config._rangedRange)
        {
            _enemyScript._playerFoundModule._stopping(_enemyScript);
            if (!_enemyScript._shootingAnimationRunning) _enemyScript._weaponDrawnState();
            if (_enemyScript._readyToAttack) _enemyScript._startRangedAttack();
         
        }
        else if (((_enemyScript._playerReferenceCollider.transform.position.x - _enemyScript.transform.position.x) <= _enemyScript._config._rangedRange) && _enemyScript._rigidbody.velocity.x == 0)
        {
            if (_enemyScript._readyToAttack) _enemyScript._startRangedAttack();
        }
            
        

    }
}
