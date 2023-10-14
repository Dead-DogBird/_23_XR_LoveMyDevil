using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestBoss : MonoBehaviour
{
    
    //ToDo:
    [SerializeField] protected float _hp = 20000;
    [SerializeField] protected float _speed = 3.5f;
    [SerializeField] protected Color BulletColor;
    [SerializeField] public Color priColor,secColor;
    
    [SerializeField] public float ConnectDamage;
    [SerializeField] protected GameObject Gate;
    public float SkillDamage;
    public bool isDie { get; protected set; } = false;
    
    public float BossHp => _hp;
    
    protected delegate UniTaskVoid BossPattern();
    protected BossPattern[] BossPatterns;
    protected Rigidbody2D _rigid;
    
    protected bool isBossPattern = true;
    // Start is called before the first frame update
    protected void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        EnterDelay().Forget();
        BossPatterns = new BossPattern[]{BossPattern1,BossPattern2,BossPattern3,BossPattern4,BossPattern5};
        isDie = false;
    }

    private int random;
    // Update is called once per frame
    protected void Update()
    {
        if(_hp<=0&&!isDie)
            DieTask().Forget();
        if (!isBossPattern)
        {
            random = Random.Range(0, BossPatterns.Length);
            BossPatterns[random]().Forget();
            Debug.Log(random);
        }
    }

    protected void OnDestroy()
    {
       
    }

    protected async UniTaskVoid EnterDelay()
    {
        isBossPattern = true;
        float orispeed = _speed;
        _speed = 0;
        await UniTask.Delay(TimeSpan.FromSeconds(3), false);
        _speed = orispeed;
        isBossPattern = false;
    }
    protected virtual async UniTaskVoid DieTask()
    {
        isDie = true;
        Time.timeScale = 0.3f;
        await UniTask.Delay(TimeSpan.FromSeconds(0.4f),true);
        for (int i = 0; i < 100; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Time.unscaledDeltaTime), true);
        }
        transform.localScale = Vector3.zero;
        Time.timeScale = 1f;
        for (int i = 0; i < 10; i++)
        {
       
        }
        
    }
     //1.시작패턴 발판 및 사탄 올라감
    async UniTaskVoid BossPattern1()
    {
        isBossPattern = true;
        isBossPattern = false;
    }
    //2. 사탄 빔
    async UniTaskVoid BossPattern2()
    {
        isBossPattern = true;
        isBossPattern = false;
    }
    //3. 총알 원형 생성 발사
    async UniTaskVoid BossPattern3()
    {
        isBossPattern = true;
        isBossPattern = false;
    }
    //4. 상단에서 총알 낙하
    async UniTaskVoid BossPattern4()
    {
        isBossPattern = true;
        isBossPattern = false;
    }
    //5. 총알 난사
    async UniTaskVoid BossPattern5()
    {
        isBossPattern = true;
        isBossPattern = false;
    }
    protected void OnTriggerEnter2D(Collider2D other)
    {

    }
}
