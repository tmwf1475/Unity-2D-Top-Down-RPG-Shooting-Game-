using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 12.0f;

    private Vector2 _moveDirection;
    private Rigidbody2D _rigibody2D;
    private Animator _animator;
    private Camera  _mainCamera;
    private PlayerGun _playerGun;
    

    private void Awake() 
    {
        _rigibody2D = GetComponent<Rigidbody2D>();   
        _animator = GetComponent<Animator>(); 
        _mainCamera = Camera.main;
        _playerGun = GetComponentInChildren<PlayerGun>();
        if (_mainCamera == null)
        {
            Debug.LogError("Can't find the main camera, please check it !!");
        }
    }

    private void Update() 
    {
        // move
        _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _moveDirection *= moveSpeed;

        _animator.SetFloat(AnimatorHash.MoveSpeed, Mathf.Abs(_moveDirection.x) + Mathf.Abs(_moveDirection.y));  

        // round and aim
        Vector3 direction = Input.mousePosition - _mainCamera.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

        //Input
        if (Input.GetMouseButton(0))
        {
            //use trigger
            _playerGun.OnTriggerHold(_mainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButtonUp(0))
        {
            // release trigger
            _playerGun.OnTriggerRelease();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //change bullet
            _playerGun.Reload();
        }
    }

    private void FixedUpdate() 
    {
        _rigibody2D.AddForce(_moveDirection, ForceMode2D.Impulse);    
    }

    public void SetShootingState(bool isShooting)
    {
        _animator.SetBool(AnimatorHash.IsShooting, isShooting);
    }
}
