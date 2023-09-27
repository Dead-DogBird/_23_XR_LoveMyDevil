using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAct : MonoBehaviour
{
    [SerializeField] private GameObject spray;
    [SerializeField] private Transform mousePointer;
    [SerializeField] private Color _sprayColor;
    PlayerContrl _playerContrl;
    private float _sprayGauge = 100;
    private float sprayGauge
    {
        get => _sprayGauge;
        set
        {
            _sprayGauge = value;
            UImanager.Instance.SetSprayGauge(_sprayGauge);
        }
    }
    private bool fillGauge;
    private bool isWaitForfillGauge;
    void Start()
    {
        _playerContrl = GetComponent<PlayerContrl>();
        GameManager.Instance._poolingManager.AddPoolingList<Spray>(100,spray);
    }

    void Update()
    {
        if (_playerContrl.Userinput.LeftMouseState)
            Spray();
        else if(!isWaitForfillGauge)
        {
            FillGaugeTask().Forget();
        }
    }
    void Spray()
    {
        if (sprayGauge > 0)
        {
            isWaitForfillGauge = false;
            sprayGauge -= 0.2f;
            GameManager.Instance._poolingManager.Spawn<Spray>().Init(mousePointer.position,_sprayColor);
        }
    }
    async UniTaskVoid FillGaugeTask()
    {
        isWaitForfillGauge = true;
        for(int i =0;i<50;i++)
        {
            if (_playerContrl.Userinput.LeftMouseState)
            {
                isWaitForfillGauge = false;
                return;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
        }
        while (sprayGauge < 100)
        {
            if (_playerContrl.Userinput.LeftMouseState)
            {
                isWaitForfillGauge = false;
                return;
            }
            sprayGauge += 0.5f;
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        sprayGauge = 100;
    }

}
