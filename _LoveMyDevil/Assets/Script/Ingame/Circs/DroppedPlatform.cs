using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DroppedPlatform : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Tween _tween;
    [SerializeField] private float dropdelay = 0.5f;
    private float gravity = 0.03f;
    
    private bool isDrop = false;
    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if(isDrop)
            Falling();
    }

    private void OnDestroy()
    {
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
        await UniTask.Delay(TimeSpan.FromSeconds(1.2f));
        isDrop = true;
        Destroy(gameObject,2f);
    }
    
}
