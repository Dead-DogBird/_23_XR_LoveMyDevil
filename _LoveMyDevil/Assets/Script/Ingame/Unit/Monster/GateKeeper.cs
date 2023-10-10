using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class GateKeeper : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    
    
    private bool isPlayer = false;
    private bool isClimb = false;
    
    private int focus = -1;

    //중력
    [Header("점프 시 적용되는 중력 값(기본값 : 5)")]
    [SerializeField] private float _gravity = 5;

    //뛰는 각도
    [Header("점프 각도(기본값 : 45)")]
    [SerializeField] float Degree = 45;
    //뛰는 힘
    [Header("뛰는 힘(기본값 : 3000)")]
    [SerializeField] float JumpForce = 3000;
    
    
    public AnimationClip[] animationClips; // 애니메이션 클립 배열
    private Animator animator;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Invoke("Jump",2);
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
       ColliderCheckCallback();
    }

    void Jump()
    {
        _rigidbody.gravityScale = _gravity;
        PlayAnimation((focus==1?2:0));
        _rigidbody.AddForce(D9Extension.DegreeToVector2(Degree + (focus==-1?90:0))*JumpForce);
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            ClimbWall().Forget();
        }
    }

    async UniTaskVoid ClimbWall()
    {
        PlayAnimation((focus==1?3:1));
        focus *= -1;
        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = new Vector2(0, 0);
        await UniTask.Delay(TimeSpan.FromSeconds(1.2f));
        Jump();
    }
    void ColliderCheckCallback()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, Vector2.one, 0);
        foreach(Collider2D i in hit) 
        {
            if (i.CompareTag("Platform"))
            {
                Destroy(i.gameObject);
            }                   
        }
    }
    //0. LeftJump, 1. LeftLanding, 2. RightJump, 3. RightLanding
    void PlayAnimation(int index)
    {
        if (index >= 0 && index < animationClips.Length)
        {
            animator.Play(animationClips[index].name);
        }
        else
        {
            Debug.LogWarning("Invalid animation index.");
        }
    }
}
