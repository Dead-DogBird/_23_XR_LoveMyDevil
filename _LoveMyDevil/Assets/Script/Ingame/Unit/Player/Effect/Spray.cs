using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Spray : PoolableObj
{
    private CircleCollider2D my_collider;
    
    private bool isColiderCheck = false;
    protected SpriteRenderer _sprite;
    private Tween _tween;
    private int id;
    void Start()
    {
    }

    void OnEnable()
    {
        base.OnEnable();
        GetComponent<CircleCollider2D>().enabled = true;
        my_collider = GetComponent<CircleCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
       
    }
    void Update()
    {
    }

    public virtual GameObject Init(Vector3 pos,Color _sprayColor)
    {
        transform.position = pos;
        _sprite.color = _sprayColor;
        isColiderCheck = false;
        _tween = _sprite.DOColor(new Color(_sprite.color.r,_sprite.color.g,_sprite.color.b,0), 0.2f);
        return gameObject;
    }
    public void CancelDestroyCallback()
    {
        isColiderCheck = true;
        Debug.Log("Called cancelCallback!");
        GetComponent<CircleCollider2D>().enabled = false;
    }

    private void OnDisable()
    {
        _tween.Complete();
    }
    private void OnDestroy()
    {
 
    }
    protected override async UniTaskVoid ReleseReserv(float delay = 1.5f)
    {
        var timer = delay;
        while(timer>=0f)
        {
            timer -= 0.1f;
            if (isColiderCheck)
            {
                return;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
        GameManager.Instance._poolingManager.Despawn(this);
    }
    
}
