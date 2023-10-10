using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ColoredPlatform : MonoBehaviour
{

    public float maxHeight = 1.0f; // 최대 높이
    private float initialYOffset = -0.5f; // 초기 Offset Y 값
    private float initialSizeY = 0.0f; // 초기 Y 축 크기
    private float ratio; //비율
    
    private BoxCollider2D _collider;
    private Vector3 _scale;
    private float max = 8;
    private float curfillingAmount = 0;
    [SerializeField] private Transform _mask;

    [Header("사라지는 속도(기본값 : 0.1)")]
    [SerializeField]private float disappearFigure = 0.1f;
        
    [Header("사라질 때 까지 걸리는 시간(초 단위)")]
    [SerializeField]private float disappearDelay = 2.5f;

    private bool isDie = false;
    
    private bool isDone = false;
    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = false;
        _scale = transform.localScale;
        ratio = (0 - initialYOffset) / (1 - initialSizeY);
    }
    // Update is called once per frame
    private void Update()
    {
        OnSprayHit();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void OnSprayHit()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position,_scale,0);
        foreach(Collider2D i in hit)
        {
            if (i.CompareTag("Spray"))
            {
                i.GetComponent<CircleCollider2D>().enabled = false;
                PaintedPlatform();
                if(!isDisapper)
                    disappearFillAmount().Forget();
                if(!isDelay)
                    TaskDelay().Forget();
            }
        }
    }

    bool PaintedPlatform(bool addAmount = true,float amount = 0.1f)
    {
        if (!_collider.enabled)
            _collider.enabled = true;
        curfillingAmount += addAmount ? amount: -amount;
        if (Mathf.Abs(curfillingAmount - max) <= 0.008f&&addAmount)
        {
            curfillingAmount = max;
            _collider.size = new Vector2(_collider.size.x, 1);
            _collider.offset = new Vector2(_collider.offset.x, 0);
            _mask.localPosition = new Vector3(_mask.localPosition.x, 0);
            return false;
        }
        if (curfillingAmount < 0)
        {
            _collider.size = new Vector2(_collider.size.x, 0);
            _collider.offset = new Vector2(_collider.offset.x, 0.5f);
            _mask.localScale = new Vector2(_mask.localScale.x, 0);
            _mask.localPosition = new Vector2(_mask.localPosition.x, -0.5f);
            return false;
        }
        // 크기 조절
        float newSize = Mathf.Lerp(0, 1.0f, curfillingAmount / max);

        _collider.size = new Vector2(_collider.size.x, newSize);
        _mask.localScale = new Vector2(_mask.localScale.x, newSize);
        
        // Offset 계산
        float newOffset = initialYOffset + (ratio * (newSize - initialSizeY));
        newOffset = Mathf.Clamp(newOffset, initialYOffset, maxHeight);

        // Offset 값 설정
        _collider.offset = new Vector2(_collider.offset.x, newOffset);
        _mask.localPosition = new Vector2(_mask.localPosition.x, newOffset);

        return true;
    }

    private bool isDisapper = false;
    private bool isDelay;
    async UniTaskVoid disappearFillAmount()
    {
        isDisapper = true;
        while (true)
        {
            if (isDie) return;
            if(!isDelay)
            {if (!PaintedPlatform(false,disappearFigure))break;}
            await UniTask.Delay(TimeSpan.FromSeconds(0.02f));
        }
        isDisapper = false;
    }
    
    async UniTaskVoid TaskDelay()
    {
        isDelay = true;
        float timer = disappearDelay;
        while(timer>0)
        {
            if (isDie) return;
            timer -= 0.1f;
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
        isDelay = false;
    }
    
}
