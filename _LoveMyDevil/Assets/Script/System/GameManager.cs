using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public float progress;
    internal PoolingManager _poolingManager;

    private float allPoints;
    private float nowPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPoint()
    {
        allPoints++;
        UImanager.Instance.SetStageProgress(0);
    }

    public void GetPoint()
    {
        nowPoints++;
        UImanager.Instance.SetStageProgress(nowPoints/allPoints);
    }
}
