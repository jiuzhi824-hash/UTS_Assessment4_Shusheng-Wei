using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildActivationManager : MonoBehaviour
{
    public Transform parentObject;
  
    public GameObject objectToActivate;
   
    public string wallTag = "Wall";

    public int activeChildCount;
    public bool allChildrenDeactivated = false;

    void Start()
    {
     
        parentObject = transform;
      
        activeChildCount = parentObject.childCount;
      
        if (objectToActivate != null)
            objectToActivate.SetActive(false);
    }

    public void DeactivateChild()
    {
        if (allChildrenDeactivated) return;
    
        for (int i = 0; i < parentObject.childCount; i++)
        {
            Transform child = parentObject.GetChild(i);
            if (child.gameObject.activeSelf)
            {              
                child.gameObject.SetActive(false);
                activeChildCount--;
                break;
            }
        }
       
        if (activeChildCount <= 0)
        {
            allChildrenDeactivated = true;

           
            if (objectToActivate != null)
                objectToActivate.SetActive(true);
        }
    }
   
    public void ResetChildren()
    {
        for (int i = 0; i < parentObject.childCount; i++)
        {
            parentObject.GetChild(i).gameObject.SetActive(true);
        }

        activeChildCount = parentObject.childCount;
        allChildrenDeactivated = false;
     
        if (objectToActivate != null)
            objectToActivate.SetActive(false);
    }
}