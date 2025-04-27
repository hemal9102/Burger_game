using UnityEngine;

public class KitchenObject : MonoBehaviour 
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;
     
    private ClearCounter clearCounter;
    
    
    public KitchenObjectSO GetKitchenObjectSO () { 
        return kitchenObjectSO; 
    }
    public void SetClearCounter (ClearCounter clearCounter)
    {
        this.clearCounter = clearCounter;
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero; 
    }
    public ClearCounter GetClearCounter() { 
        return clearCounter;
    }

}
