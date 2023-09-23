using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 컴포넌트들
    public Rigidbody2D _playerRigidbody { get; private set; }
    private BoxCollider2D _boxCollider2D;
    private PlayerContrl _playerControll;

    [Header("플레이어 스탯")]
    [SerializeField] private float speed = 5f;
    [Tooltip("플레이어 점프력 (보통 1000 해놓음)")]
    [SerializeField] private float jumpForce = 1000;

    [Header("플레이어 최대 점프 횟수")]
    [SerializeField] private int MAXJUMP = 2;

    [Header("플레이어 점멸 관련 변수들")]
    [Tooltip("점멸 시간")]
    [SerializeField] float blinkDuration = 0.5f;
    [Tooltip("점멸 딜레이")]
    [SerializeField] private float BlinkDelay = 1;
    
    //기타 트리거들
    private bool _isjumping;
    private int jumpCount = 0;

    private float _playerOriSpeed;
    private float oriColliderxsize;
    private float oriGravity;
    private bool isBlink;

    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerControll = GetComponent<PlayerContrl>();

    }

    void Update()
    {
        Jump();
        if (_playerControll.Userinput.SkillKey && !isBlink&&_playerControll.Userinput.AxisState!=0)
        {
            Blink().Forget();
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(speed * _playerControll.Userinput.AxisState, 0) * Time.deltaTime);
    }

    void Jump()
    {
        if (jumpCount >= MAXJUMP) return;
        if (!_playerControll.Userinput.SpaceState) return;
        _isjumping = true;
        jumpCount++;

        if (jumpCount == 2)
        {
            _playerRigidbody.velocity = Vector2.zero;
            _playerRigidbody.AddForce(new Vector2(0, jumpForce));
            return;
        }
        _playerRigidbody.AddForce(new Vector2(0, jumpForce));
        _playerRigidbody.velocity = Vector2.zero;


    }

    private bool isCollisionWall;
    async UniTaskVoid Blink()
    {
        isBlink = true;
        _playerOriSpeed = speed;
        oriGravity = _playerRigidbody.gravityScale;
        oriColliderxsize = _boxCollider2D.size.x;
        _playerRigidbody.velocity = Vector2.zero;
        _playerRigidbody.gravityScale = 0;
        
        speed = 0;
        
        float blinktimer = blinkDuration;
        float blinkDelay = BlinkDelay;
        float toPos = Mathf.Clamp(_playerControll.Userinput.AxisState,-1f,1f);
        while (blinktimer>0)
        {
            blinktimer -= 0.1f;
            if (isCollisionWall)
            {
                isCollisionWall = false;
                break;
            }
            transform.Translate(new Vector3(_playerOriSpeed*3 * toPos, 0) * Time.deltaTime);
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }
        
        speed = _playerOriSpeed;
        _playerRigidbody.gravityScale = oriGravity;
        while (blinkDelay > 0)
        {
            blinkDelay -= 0.1f;
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
        isBlink = false;
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Ground") ||
             other.gameObject.CompareTag("ColoredPlatform") ||
             other.gameObject.CompareTag("DropPlatform") ||
             other.gameObject.CompareTag("Platform")) && other.contacts[1].normal.y > 0.7f &&
            _playerRigidbody.velocity.y >= 0)
        {
            _isjumping = false;
            jumpCount = 0;
        }
        if (other.transform.CompareTag("DropPlatform") && other.transform.position.y < transform.position.y)
        {
            other.transform.GetComponent<DroppedPlatform>().Dropped().Forget();
        }
        if(other.gameObject.CompareTag("Wall"))
        {
            isCollisionWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Ground")||
            other.gameObject.CompareTag("ColoredPlatform") ||
            other.gameObject.CompareTag("DropPlatform")) && !_isjumping)
        {
            _isjumping = true;
            jumpCount = 1;
        }
 
    }




}
