using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TestColorObject : MonoBehaviour
{
    [SerializeField] private GameObject colordPart;
    [SerializeField] private ColorCallBackController colorCallBackController;
    private List<Spray> sprayList = new();
    private bool isActive;
    // Start is called before the first frame update
    void Start()
    {
       
        colorCallBackController.onColiderEnter += setSprayControl;
        GameManager.Instance.SetPoint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
       if(other.CompareTag("Spray")&&!isActive)
       {
            colordPart.SetActive(true);
            isActive = true;
            GameManager.Instance.GetPoint();
            foreach (var spray in sprayList)
            {
                GameManager.Instance._poolingManager.Despawn(spray);
            }
            Destroy(colorCallBackController.gameObject);
            Destroy(this);
       }
    }

    public void setSprayControl(Collider2D spray)
    {
        if (spray.CompareTag("Spray") && !isActive)
        {
            sprayList.Add(spray.GetComponent<Spray>());
            spray.transform.GetComponent<Spray>().CancelDestroyCallback();
        }
    }
    
}
