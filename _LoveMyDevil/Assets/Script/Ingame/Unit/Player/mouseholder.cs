using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseholder : MonoBehaviour
{
    private Vector3 pos;
    [SerializeField, Range(0.0f, 10.0f)]
    private float leverRange;  
    [SerializeField] private GameObject player;
    
    private void Start()
    {
        
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var distanceToMouse = Vector3.Distance(mousePosition, player.transform.position);
        var limitedDistance = Mathf.Min(distanceToMouse, leverRange);
        Vector3 finalPosition = player.transform.position + (mousePosition - player.transform.position).normalized * limitedDistance;

        
        transform.position = finalPosition;
    }


}
