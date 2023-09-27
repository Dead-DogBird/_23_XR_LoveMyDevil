using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColorObject : MonoBehaviour
{
    [SerializeField] private GameObject colordPart;
    [SerializeField] private ColorCallBackController colorCallBackController;

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
            Destroy(colorCallBackController.gameObject);
       }
    }

    public void setSprayControl(Collider2D spray)
    {
        if (spray.CompareTag("Spray") && !isActive)
        {
            spray.transform.GetComponent<Spray>().CancelDestroyCallback(colorCallBackController.transform);
        }
    }
    
}
