using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BossStagePlatformController : MonoSingleton<BossStagePlatformController>
{
    [SerializeField] private GameObject[] _platforms;
    [SerializeField] private GameObject[] _toYposes;
    [Header("발판 떨림 폭")] [SerializeField][Range(0.001f,0.30f)] private float platformVibration = 0.1f;
    [Header("발판 떨림 속도")] [SerializeField][Range(0.00f,2.00f)] private float platformVibrationSpeed = 0.1f;
    // Start is called before the first frame update

    private bool[] isMovePlatform = new bool[5];
    void Start()
    {
        MovePlatformTask(0, 1,1,3).Forget();
        MovePlatformTask(2, 0,1,3).Forget();
        MovePlatformTask(4, 1,1,3).Forget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlatform(int _platform, int topos,float moveSpeed = 1f,float delay=0)
    {
        MovePlatformTask(_platform,topos,moveSpeed,delay).Forget();
    }

    async UniTaskVoid MovePlatformTask(int _platform, int topos,float moveSpeed = 1f,float delay = 0)
    {
        if (isMovePlatform[_platform])
        {
            await UniTask.WaitUntil(()=>!isMovePlatform[_platform]);
        }
        isMovePlatform[_platform] = true;
        var platform = _platforms[_platform].transform;
        var toYpos = _toYposes[topos].transform.position.y;
        float _delay =0;
        float tick = 0;
        while (_delay <= delay)
        {
            _delay += Time.deltaTime;
            tick += 0.1f*platformVibrationSpeed;
            platform.position += new Vector3(0, MathF.Sin(tick)*platformVibration);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        
        while (MathF.Abs(platform.position.y - toYpos) >= 0.08f)
        {
            platform.position += (new Vector3(platform.position.x, toYpos) - platform.position) * (moveSpeed*Time.deltaTime);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        isMovePlatform[_platform] = false;
    }
}
