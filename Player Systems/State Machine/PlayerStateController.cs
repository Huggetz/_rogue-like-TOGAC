using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStateController : MonoBehaviour
{

    // Setting up other states
    [HideInInspector] public PlayerBaseState currentstate;
    [HideInInspector] public PlayerIdleState IdleState = new PlayerIdleState();
    [HideInInspector] public PlayerJumpingState JumpingState = new PlayerJumpingState();
    [HideInInspector] public PlayerRunningState RunningState = new PlayerRunningState();
    [HideInInspector] public PlayerRollingState RollingState = new PlayerRollingState();
    [HideInInspector] public PlayerDeadState DeadState = new PlayerDeadState();

    // Variables and components
    public WeaponTemplate _weapon;
    public LayerMask layerMask;
    public Rigidbody2D rb1;
    public float speed;
    public float jumpforce;
    [HideInInspector] public Animator an1;
    [HideInInspector] public Transform tr1;
    [HideInInspector] public Animator an_muzzleflash;
    [HideInInspector] public SpriteRenderer ArmVisibility;
    // public float attackDelay;
    [HideInInspector] public string _currentAnimState;
    [HideInInspector] public Transform _gunTransform;
    [HideInInspector] public Transform _aimTransform;
    public float _RollingSpeed;
    public float _RollingTime;
    public float _RollingDelay;
    public float _weaponDrawingTime;
    public float _weaponSheathingDelay, _weaponSheathingDuration, _weaponSheathingTimer, _weaponSheathingDurationTimer;
    public FloatReference _hp;
    public FloatReference _maxHp;
    [HideInInspector] public Vector2 _aimTransformPositionRight;

    // Events
    public UnityEvent _turnedRightEvent;
    public UnityEvent _weaponDrawnEvent;
    

    // bools
    [HideInInspector] public bool _turnedRight, _turnedLeft;
    public bool _weaponDrawn;
    [HideInInspector] public bool _animationLocked;
    [HideInInspector] public bool _isShooting, _readyToShoot;
    [HideInInspector] public bool _isRolling;

    // Animations list

    [HideInInspector] public string _meleeKickLeft = "MeleeKickLeft";
    [HideInInspector] public string _meleeKickRight = "MeleeKickRight";
    [HideInInspector] public string _runningAndReloadingLeft = "RunningAndReloading Left";
    [HideInInspector] public string _runningAndReloadingRight = "RunningAndReloading Right";
    [HideInInspector] public string _reloadingRight = "ReloadingRight";
    [HideInInspector] public string _reloadingLeft = "ReloadingLeft";
    [HideInInspector] public string _runningAndShootingLeft = "RunningAndShooting Left";
    [HideInInspector] public string _runningAndShootingRight = "RunningAndShooting Right";
    [HideInInspector] public string _runningWithGunLeft = "RunningWithGun Left";
    [HideInInspector] public string _runningWithGunRight = "RunningWithGun Right";
    [HideInInspector] public string _shootingLeft = "Shooting Left";
    [HideInInspector] public string _shootingRight = "Shooting Right";
    [HideInInspector] public string _shootingUpLeft = "Shooting Up Left";
    [HideInInspector] public string _shootingUpRight = "Shooting Up Right";
    [HideInInspector] public string _weaponSheathingLeft = "WeaponSheathing Left";
    [HideInInspector] public string _weaponSheathingRight = "WeaponSheathing Right";
    [HideInInspector] public string _weaponDrawingLeft = "WeaponDrawing Left";
    [HideInInspector] public string _weaponDrawingRight = "WeaponDrawing Right";
    [HideInInspector] public string _idleLeft = "Idle Left";
    [HideInInspector] public string _idleRight = "Idle Right";
    [HideInInspector] public string _idleWithGunRight = "IdleWithGun Right";
    [HideInInspector] public string _idleWithGunLeft = "IdleWithGun Left";
    [HideInInspector] public string _rollingLeft = "Rolling Left";
    [HideInInspector] public string _rollingRight = "Rolling RIght";
    [HideInInspector] public string _runningLeft = "Running Left";
    [HideInInspector] public string _runningRight = "Running Right";


    public void _death()
    {
        if ((_hp._value <= 0) && (currentstate != DeadState))
        {
            _hp._realValue.value = 0;
            SwitchState(DeadState);
        }
            
    }

    private void Awake()
    {

        if (_hp._value <= 0)
        { _hp._realValue.value = _maxHp._value; }
        ArmVisibility = GameObject.Find("Aim").GetComponent<SpriteRenderer>();
        tr1 = GetComponent<Transform>();
        an1 = GetComponent<Animator>();
        rb1 = GetComponent<Rigidbody2D>();
        // _gunTransform = GameObject.Find("Arm with gun").GetComponent<Transform>();
        _aimTransform = GameObject.Find("Aim").GetComponent<Transform>();
        _isRolling = false;
        _aimTransformPositionRight = _aimTransform.localPosition;
        _turnedLeft = false;
        _turnedRight = false;
        _weaponDrawn = false;
        _readyToShoot = true;
        
        // an_muzzleflash = GameObject.Find("MuzzleFlash").GetComponent<Animator>();

    }

    void Start()
    {
        
        currentstate = IdleState;
        currentstate.EnterState(this);
        
    }
    private void FixedUpdate()
    {
        currentstate.OnFixedUpdate(this);
    }
    void Update()
    {
        _death();
        currentstate.OnUpdate(this);
        _shoot();
        _weaponSheathingLogic();





    }
   
    public void ChangeAnimationState(string newState)
    {
        if (_currentAnimState == newState) return;

        an1.Play(newState);

        _currentAnimState = newState;
    }



    // Switching between states
    public void SwitchState(PlayerBaseState state)
    {
        currentstate = state;
        currentstate.EnterState(this);
    }
    // Checks for a ground collision. Used in state scripts
    [HideInInspector] public bool isGrounded()
    {
        BoxCollider2D groundCheck = GameObject.Find("GroundCheck").GetComponent<BoxCollider2D>();
        RaycastHit2D raycasthit2d = Physics2D.BoxCast(groundCheck.bounds.center, groundCheck.bounds.extents, 0, Vector2.down);
        return (raycasthit2d.collider.IsTouchingLayers(layerMask));
    }
    // Changing the direction character is facing
    public void Turning()
    {
        if (transform.localScale.x < 0)
        {
            _turnedLeft = true;
            _turnedRight = false;
        }
        else
        {
            _turnedRight = true;
            _turnedLeft = false;
        }
        if (!_turnedRight && Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

        }
        else if (!_turnedLeft && Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }
    public IEnumerator _weaponDrawing()
    {
        
        if (_turnedRight) ChangeAnimationState(_weaponDrawingRight);
        else ChangeAnimationState(_weaponDrawingLeft);
        yield return new WaitForSeconds(_weapon._drawDuration);
        yield return new WaitForEndOfFrame();
       
        
        StopCoroutine(_weaponDrawing());
    }
    public void _chosingIdleState()
    {
       if (!_weaponDrawn)
        {
            if (_turnedRight) ChangeAnimationState(_idleRight);
            else ChangeAnimationState(_idleLeft);
        }
       else
        {
            if (_turnedRight) ChangeAnimationState(_idleWithGunRight);
            else ChangeAnimationState(_idleWithGunLeft);
        }
    }
    public void _chosingRunningState()
    {
        if (_weaponDrawn)
        {
            if (_turnedRight) ChangeAnimationState(_runningWithGunRight);
            else ChangeAnimationState(_runningWithGunLeft);
        }
        else
        {
            if (_turnedRight) ChangeAnimationState(_runningRight);
            else ChangeAnimationState(_runningLeft);
        }
    }



    public IEnumerator _rollingBackToIdle(PlayerRollingState _roll)
    {
        _isRolling = true;
        rb1.velocity = _roll._vel;
        gameObject.layer = LayerMask.NameToLayer("Player Rolling");
        yield return new WaitForSeconds(_RollingTime);
        rb1.velocity = new Vector2(0, rb1.velocity.y);
        gameObject.layer = LayerMask.NameToLayer("Player");
        gameObject.tag = "Player";
        transform.Find("Aim").gameObject.SetActive(true);
        yield return new WaitForSeconds(_RollingDelay);
        _isRolling = false;
        if (Input.GetAxisRaw("Horizontal") != 0) SwitchState(RunningState);
        else SwitchState(IdleState);


        StopCoroutine(_rollingBackToIdle(_roll));
        
    }
    

    
   public IEnumerator _shootingInIdle()
    {
        _readyToShoot = false;
        _isShooting = true;
        if (!_weaponDrawn)
        {
            StartCoroutine(_weaponDrawing());
        }
        _weaponDrawnEvent.Invoke();
        if (_turnedRight) ChangeAnimationState(_shootingRight);
        else ChangeAnimationState(_shootingLeft);
        GameObject _btr = Instantiate(_weapon._ammoType, _aimTransform.position, Quaternion.identity);
        yield return new WaitForSeconds(_weapon._shootDuration);
        yield return new WaitForEndOfFrame();
        if (currentstate == IdleState) _chosingIdleState();
        else if (currentstate == RunningState) _chosingRunningState();
        _isShooting = false;
        yield return new WaitForSeconds(_weapon._shootDelay);
        _readyToShoot = true;
        StopCoroutine(_shootingInIdle());
    }

    public IEnumerator _shootingInRunning()
    {
        _readyToShoot = false;
        _isShooting = true;
        if (!_weaponDrawn) StartCoroutine(_weaponDrawing());
        _weaponDrawnEvent.Invoke();
        if (_turnedRight) ChangeAnimationState(_runningAndShootingRight);
        else ChangeAnimationState(_runningAndShootingLeft);
        yield return new WaitForSeconds(_weapon._shootDuration);
        if (currentstate == IdleState) _chosingIdleState();
        else if (currentstate == RunningState) _chosingRunningState();
        _isShooting = false;
        yield return new WaitForSeconds(_weapon._shootDelay);
        _readyToShoot = true;
        StopCoroutine(_shootingInRunning());
    }
    public void _shoot()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.W))
        {
            if (_readyToShoot)
            {
                if (currentstate == IdleState) StartCoroutine(_shootingInIdle());
                else if (currentstate == RunningState) StartCoroutine(_shootingInRunning());
            }
           
        }
    }

    public void _renewSheathingTimer()
    {
        _weaponSheathingTimer = _weaponSheathingDelay;
        _weaponSheathingDurationTimer = _weaponSheathingDuration;
        Debug.Log("Timer Renewed");
        if (!_weaponDrawn) _weaponDrawn = true;
    }
    public void _weaponSheathingLogic()
    {
        if (_weaponDrawn)
        {

            //Debug.Log($"Event Started");



            if (_weaponSheathingTimer > 0)
            {
                _weaponSheathingTimer -= Time.deltaTime;
            }
            if (_weaponSheathingTimer <= 0 && _weaponSheathingDurationTimer > 0)
            {
                if (_turnedRight) ChangeAnimationState(_weaponSheathingRight);
                else ChangeAnimationState(_weaponSheathingLeft);
                _weaponSheathingDurationTimer -= Time.deltaTime;
            }
            if (_weaponSheathingDurationTimer <= 0)
            {
                _weaponDrawn = false;
                if (currentstate == RunningState) _chosingRunningState();
                else _chosingIdleState();
            }
        }
       
    }
    
  
}
