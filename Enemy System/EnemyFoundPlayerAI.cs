using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy AI/Player Found")]
public class EnemyFoundPlayerAI : EnemyBaseAI
{
    [HideInInspector] public Vector2 _directionToPlayer;
    [HideInInspector] public EnemyBaseAI _thisTypeOfAttack;
    

    public override void OnEnter(EnemyScript _enemyScript)
    {
        _enemyScript._readyToAttack = true;
        _attackType(_enemyScript);
        _enemyScript._rigidbody.velocity = new Vector2(0, _enemyScript._rigidbody.velocity.y);
        _enemyScript._startDrawingWeapon();
    }
    public override void OnUpdate(EnemyScript _enemyScript)
    {
        _outOfRange(_enemyScript);
        _turning(_enemyScript);
        _thisTypeOfAttack.OnUpdate(_enemyScript);
        


    }
    public override void OnFixedUpdate(EnemyScript _enemyScript)
    {
        
        _thisTypeOfAttack.OnFixedUpdate(_enemyScript);

    }
    public override void OnCollision(EnemyScript _enemyScript)
    {
        _thisTypeOfAttack.OnCollision(_enemyScript);
    }
    public override void OnExit(EnemyScript _enemyScript)
    {
        _thisTypeOfAttack.OnExit(_enemyScript);
    }

    public void _movingToPlayer(EnemyScript _enemyScript)
    {
        _enemyScript._groundHoleCheck();
        if (_enemyScript._groundFound)
        {
            _enemyScript._rigidbody.velocity = new Vector2(_directionToPlayer.x * _enemyScript._config._moveSpeed * Time.deltaTime, _enemyScript._rigidbody.velocity.y);
        }
        else _enemyScript._rigidbody.velocity = new Vector2(0, _enemyScript._rigidbody.velocity.y);


    }
    public void _stopping(EnemyScript _enemyScript)
    {
        _enemyScript._rigidbody.velocity = new Vector2(0, _enemyScript._rigidbody.velocity.y);
    }
    public void _turning(EnemyScript _enemyScript)
    {
        _directionToPlayer = (_enemyScript._playerReferenceCollider.transform.position - _enemyScript.transform.position).normalized;
        if ((_directionToPlayer.x < 0 && _enemyScript.transform.localScale.x > 0) || (_directionToPlayer.x > 0 && _enemyScript.transform.localScale.x < 0))
            _enemyScript.transform.localScale = new Vector2(-1 * _enemyScript.transform.localScale.x, _enemyScript.transform.localScale.y);
    }
    public void _outOfRange(EnemyScript _enemyScript)
    {
        // Debug.Log(Mathf.Abs(_enemyScript._playerReferenceCollider.transform.position.x - _enemyScript.transform.position.x));
        if ((_enemyScript._config._followRange < Mathf.Abs(_enemyScript._playerReferenceCollider.transform.position.x - _enemyScript.transform.position.x)) || _enemyScript._playerReferenceCollider.transform.position.y > (_enemyScript.transform.position.y + _enemyScript.transform.localScale.y))
        {

            Debug.Log("Player out of range");
            _enemyScript._startFollowTimer();
            if (!_enemyScript._playerFound)
            {
               _enemyScript._switchState(_enemyScript._idleModule);
            }
        }
    }
    public void _attackType(EnemyScript _enemyScript)
    {
        if (_enemyScript._config._isMelee)
        {
            _thisTypeOfAttack = _enemyScript._enemyIsMelee;
            
        }
        if (_enemyScript._config._isRanged || (_enemyScript._config._isRanged && _enemyScript._config._isMelee))
        {
            _thisTypeOfAttack = _enemyScript._enemyIsRanged;
            
        }  
        //Debug.Log(_thisTypeOfAttack);

    }


}
