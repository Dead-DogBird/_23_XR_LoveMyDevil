using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class DroppedPlatform : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Tween _tween;
    [SerializeField] private float dropdelay = 0.5f;
    private float gravity = 0.03f;
    
    private bool isDrop = false;
    
    private Vector3 oriPos;

    private Color oriColor;

    [Header("리스폰 딜레이(초 단위)")]
    [SerializeField] private float respawnDelay = 3;

    private float proDelay;
    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        oriPos = transform.position;
        oriColor = _sprite.color;
        proDelay = respawnDelay;
    }
    // Update is called once per frame
    void Update()
    {
        if(isDrop)
            Falling();
    }

    private bool isActive=true;
    private void OnDestroy()
    {
        isActive = false;
        _tween.Complete();
    }

    void Falling()
    {
        gravity += (gravity*Time.deltaTime)/2;
        transform.position -= new Vector3(0, gravity);
    }
    public async UniTaskVoid Dropped()
    {
        float time = dropdelay;
        while (time > 0)
        {
            time -= 0.1f;
            if(!gameObject)
                return;
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
        _tween =  _sprite.DOColor(new Color(40/255f,36/255f,90/255f), 0.1f);

        for (int i = 0; i < 12; i++)
        {
            if (!isActive) return;
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
        isDrop = true;
        while(respawnDelay>0f)
        {
            if (!isActive) return;
            respawnDelay -= 0.1f;
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

        }
        isDrop = false;
        _tween.Complete();
        _sprite.color = oriColor;
        transform.position = oriPos;
        respawnDelay = proDelay;
        gravity = 0.03f;
    }
}
