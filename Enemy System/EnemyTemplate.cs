using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemies/Templates/Humanoid")]
public class EnemyTemplate : ScriptableObject
{
    public float _maxHp;
    public float _moveSpeed;
    public float _jumpforce;
    public float _followRange;
    public float _enemyDamage;
    public float _meleeRange;
    public bool _isMelee;
    public float _rangedRange;
    public bool _isRanged;
    public float _ammoMoveSpeed;

    public float _idleDuration;
    public float _idleMovespeed;
    public float _followTime;
    public float _attackDuration;
    public float _attackDelay;
    public float _weaponDrawingTime;
    public GameObject _weaponPrefab;
    public GameObject _ammoType;
    public Sprite _enemySprite;
    public LayerMask _groundLayer;
    public LayerMask _playerLayer;
    

}
