using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyTemplate _config;
    public AnimationList _animationList;
    [HideInInspector] public EnemyBaseAI _currentState;
    [HideInInspector] public float _hp;
    public List<EnemyBaseAI> _aiModules;
    public EnemyIdleAI _idleModule;
    public EnemyFoundPlayerAI _playerFoundModule;
    public EnemyMeleeAI _enemyIsMelee;
    public EnemyRangedAI _enemyIsRanged;
    
    

    [HideInInspector] public  Rigidbody2D _rigidbody;
    [HideInInspector] public bool _groundFound;
    [HideInInspector] public bool _readyToIdle;
    [HideInInspector] public bool _AFKisDone;
    [HideInInspector] public Vector3 _dir;
    [HideInInspector] public Transform _holeChecker;
    [HideInInspector] public Collider2D _playerChecker;
    [HideInInspector] public bool _playerFound;
    [HideInInspector] public Collider2D _playerReferenceCollider;
    [HideInInspector] public Animator _enemyAnim;
    [HideInInspector] public string _currentAnimationState;
    [HideInInspector] public bool _readyToAttack;
    [HideInInspector] public Collider2D _meleeHitbox;
    [HideInInspector] public GameObject _meleeHitboxGameObject;
    [HideInInspector] public bool _isWalking;
    [HideInInspector] public bool _isTurnedRight;
    [HideInInspector] public bool _weaponIsDrawn;
    [HideInInspector] public bool _shootingAnimationRunning;

    Transform _eyeCastPoint;

    // List of animations

    [HideInInspector] public string _idleAnimation = "Test Idle";
    [HideInInspector] public string _followingPlayerAnimation = "Test FollowingPlayer";
    [HideInInspector] public string _meleeAttackAnimation = "Test MeleeAttack";
    [HideInInspector] public string _rangedAttackAnimation = "Test RangedAttack";



    private void Awake()
    {
        _meleeHitbox = transform.Find("MeleeHitBox").GetComponent<BoxCollider2D>();
        _meleeHitboxGameObject = transform.Find("MeleeHitBox").gameObject;
        _meleeHitboxGameObject.SetActive(false);
        _enemyAnim = gameObject.GetComponent<Animator>();
        _playerFound = false;
        _eyeCastPoint = transform.Find("EyeCastPoint");
        _playerReferenceCollider = GameObject.Find("Player").GetComponent<Collider2D>();
        _AFKisDone = false;
        StartCoroutine("_startingAFKtimer");
        _holeChecker = transform.Find("GroundHoleCheck");
        _playerChecker = transform.Find("PlayerChecker").GetComponent<CapsuleCollider2D>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _weaponIsDrawn = false;
        _shootingAnimationRunning = false;

        
        
    }
    private void Start()
    {
    _hp = _config._maxHp; // Health 
        if (_AFKisDone)
        {

        }
    }
    private void FixedUpdate()
    {
        _currentState.OnFixedUpdate(this);
        if (_AFKisDone)
        {
            
        }
            
    }
    private void Update()
    {
        
        _currentState.OnUpdate(this);
        _walkingAndTurningCheck();

    }
    public void _walkingAndTurningCheck()
    {
        if (_rigidbody.velocity.x == 0) _isWalking = false;
        else _isWalking = true;
        if (transform.localScale.x > 0) _isTurnedRight = true;
        else _isTurnedRight = false;
    }
    public void _damageHandling()
    {
        if (_hp <= 0)
        {
            Debug.Log("Enemy dead");
            Destroy(gameObject);
        }
    }
    public IEnumerator _startingAFKtimer()
    {
        yield return new WaitForSeconds(1);
        _switchState(_idleModule);
        _AFKisDone = true;
        _readyToIdle = true;
        _currentState.OnEnter(this);
    }
    public IEnumerator _chosingPosition()
    {
        _readyToIdle = false;
        Vector3 _randomPosition = new Vector3(Random.Range(transform.position.x - 100, transform.position.x + 100), transform.position.y, transform.position.z);
       // Vector3 _randomPosition = new Vector3(Random.Range(transform.position.x - 100, transform.position.x + 100), transform.position.y, transform.position.z);
        Debug.Log("New direction chosen");
        _dir = new Vector3(_randomPosition.x - transform.position.x, transform.position.y).normalized;
        yield return new WaitForSeconds(_config._idleDuration);
        _dir = new Vector3(0, 0);
            yield return new WaitForSeconds(Random.Range(1, 6));
                _readyToIdle = true;
        StopCoroutine("_chosingPosition");

    }
    public void _idleWalking()
    {
        _groundHoleCheck();
        
        if (_readyToIdle)
            StartCoroutine("_chosingPosition");

        _rigidbody.velocity = new Vector2(_dir.x * _config._idleMovespeed * Time.deltaTime, _rigidbody.velocity.y);
        if ((_dir.x < 0 && transform.localScale.x > 0) || (_dir.x > 0 && transform.localScale.x < 0))
            transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);

        if (_groundFound == false)
        {
            transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
            _dir.x *= -1;
        }
    }
    public void _groundHoleCheck()
    {
        RaycastHit2D _holeCheck = Physics2D.Raycast(_holeChecker.position, Vector2.down, 0.2f, _config._groundLayer);
        if (_holeCheck.collider != null)
            _groundFound = true;
        else
            _groundFound = false;
    }

    public void _checkingForPlayer()
    {
        if (_playerChecker.IsTouching(_playerReferenceCollider) )
        {
            RaycastHit2D _playerHit = Physics2D.Raycast(_eyeCastPoint.position, _playerReferenceCollider.transform.position - _eyeCastPoint.position , Mathf.Infinity); // ÂÛÄÂÈÍÓÒÜ ÇÀ ÏÐÅÄÅË ÊÂÀÄÐÀÒÀ
            Debug.DrawRay(_eyeCastPoint.transform.position, (_playerReferenceCollider.transform.position - _eyeCastPoint.position)* Mathf.Infinity, Color.green);
            // Debug.Log("Player in range");
            if (_playerHit.collider != null)
            {
                if (_playerHit.collider.tag == "Player")
                {
                    //Debug.Log("Touching " + _playerHit.collider + ";" + _playerFound);
                    _playerFound = true;
                    if (_playerFound)
                    {
                        _switchState(_playerFoundModule);

                    }

                }
            }
            
                
            
            
                
            
        }
    }

    public void _startFollowTimer()
    {
        StartCoroutine("_followTimer");
    }
    public IEnumerator _followTimer()
    {
        yield return new WaitForSeconds(_config._followTime);
        _playerFound = false;
        StopCoroutine("_followTimer");
    }
    public void _switchState(EnemyBaseAI state)
    {
        _currentState = state;
        _currentState.OnEnter(this);
    }
    public void _switchAnimation(string _newAnimationState)
    {
        if (_currentAnimationState == _newAnimationState) return;
        _enemyAnim.Play(_newAnimationState);
        _currentAnimationState = _newAnimationState;
    }

    public IEnumerator _attackingInMelee()
    {
        _readyToAttack = false;
        if (transform.localScale.x > 0)
       // _switchAnimation(_animationList._enemyMeleeAttackAnimation_Right);
        //else _switchAnimation(_animationList._enemyMeleeAttackAnimation_Left);
        yield return new WaitForSeconds(_config._attackDuration);
        _meleeHitboxGameObject.SetActive(true);
        yield return new WaitForFixedUpdate();
        if (_meleeHitbox.IsTouching(_playerReferenceCollider))
        {
            _playerReferenceCollider.GetComponent<PlayerStateController>()._hp._realValue.value -= _config._enemyDamage;
            Debug.Log("Damage been dealt");
        }
        _meleeHitboxGameObject.SetActive(false);
        _switchAnimation(_followingPlayerAnimation);
        yield return new WaitForSeconds(_config._attackDelay);
        _readyToAttack = true;
        StopCoroutine("_attackingInMelee");
    }
    public void _startMeleeAttack()
    {
        StartCoroutine("_attackingInMelee");
    }

    public IEnumerator _rangedAttacking()
    {
        _readyToAttack = false;
        _shootingAnimationRunning = true;
        if (_isTurnedRight)
       _switchAnimation(_animationList._enemyShooting_Right);
        else _switchAnimation(_animationList._enemyShooting_Left);
        
        GameObject _btr = Instantiate(_config._ammoType, transform.Find("ShootingPoint").position, Quaternion.identity);
        _btr.GetComponent<EnemyAmmoBullet>()._damage = _config._enemyDamage;
        _btr.GetComponent<EnemyAmmoBullet>()._direction = (transform.localScale.x / Mathf.Abs(transform.localScale.x));
        if (_btr.GetComponent<EnemyAmmoBullet>()._direction < 0)
            _btr.transform.localScale = new Vector2(-1 * _btr.transform.localScale.x, _btr.transform.localScale.y);
        _btr.GetComponent<EnemyAmmoBullet>()._movespeed = _config._ammoMoveSpeed;
        yield return new WaitForSeconds(_config._attackDuration);
        _weaponDrawnState();
        _shootingAnimationRunning = false;
        // Debug.Log("Enemy damage is " + _btr.GetComponent<EnemyAmmoBullet>()._damage);
        yield return new WaitForSeconds(_config._attackDelay);
       _readyToAttack = true;
        StopCoroutine("_rangedAttacking");

    }
    public void _startRangedAttack()
    {
        StartCoroutine("_rangedAttacking");
    }

    public IEnumerator _drawingWeapon()
    {
         if (_isWalking)
        {
            if (_isTurnedRight)
                _switchAnimation(_animationList._enemyDrawingWeaponStanding_Right);
            else _switchAnimation(_animationList._enemyDrawingWeaponStanding_Left);
        }
         else
        {
            if (_isTurnedRight)
                _switchAnimation(_animationList._enemyDrawingWeaponWalking_Right);
            else _switchAnimation(_animationList._enemyDrawingWeaponWalking_Left);
        }
        yield return new WaitForSeconds(_config._weaponDrawingTime);
        if (_isWalking)
        {
            if (_isTurnedRight)
                _switchAnimation(_animationList._enemyWalkingWithGun_Right);
            else _switchAnimation(_animationList._enemyWalkingWithGun_Left);
        }
        else
        {
            if (_isTurnedRight)
                _switchAnimation(_animationList._enemyStandingWithGun_Right);
            else _switchAnimation(_animationList._enemyStandingWithGun_Left);
        }
    }
    public void _weaponDrawnState()
    {
        if (_isWalking)
        {
            if (_isTurnedRight)
                _switchAnimation(_animationList._enemyWalkingWithGun_Right);
            else _switchAnimation(_animationList._enemyWalkingWithGun_Left);
        }
        else
        {
            if (_isTurnedRight)
                _switchAnimation(_animationList._enemyStandingWithGun_Right);
            else _switchAnimation(_animationList._enemyStandingWithGun_Left);
        }
    }
    public void _startDrawingWeapon()
    {
        // StartCoroutine("_drawingWeapon");
        if (!_weaponIsDrawn)
        {
            if (_isWalking)
            {
                if (_isTurnedRight)
                    _switchAnimation(_animationList._enemyDrawingWeaponStanding_Right);
                else _switchAnimation(_animationList._enemyDrawingWeaponStanding_Left);
            }
            else
            {
                if (_isTurnedRight)
                    _switchAnimation(_animationList._enemyDrawingWeaponWalking_Right);
                else _switchAnimation(_animationList._enemyDrawingWeaponWalking_Left);
            }
            Invoke("_weaponDrawnState", _config._weaponDrawingTime);
        }
        else return;

    }
}
