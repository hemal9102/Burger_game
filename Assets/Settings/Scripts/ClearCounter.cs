using System;
using UnityEditor;
using UnityEngine;

public class ClearCounter:MonoBehaviour
{
    [SerializeField] private KitchenObjectSO KitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private ClearCounter secondClearCounter;
    [SerializeField] private bool testing;
    
    private KitchenObject kitchenObject;

    private void Update()
    {
        if(testing && Input.GetKeyDown(KeyCode.T))
        {
            if (kitchenObject != null) { 
                kitchenObject.SetClearCounter(secondClearCounter);
                Debug.Log(kitchenObject.GetClearCounter());
            }
        }
    }

    public void Interact()
    {
        if (kitchenObject == null)
        {
            Transform KitchenObjectTransform = Instantiate(KitchenObjectSO.prefabs, counterTopPoint);
            KitchenObjectTransform.localPosition = Vector3.zero;

            kitchenObject = KitchenObjectTransform.GetComponent<KitchenObject>();
            kitchenObject.SetClearCounter(this); ;
        }
        else {
            Debug.Log(kitchenObject.GetClearCounter());
        }
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }
} 
