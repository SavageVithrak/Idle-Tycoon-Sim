using UnityEngine;

namespace _Scripts.Core
{
public class Store : MonoBehaviour
{
    
    [ SerializeField ] private StoreUi _storeUi;
    
    [SerializeField] private int _storeCount;
    
    public int StoreCount { get => _storeCount; set => _storeCount = value; }
    public int BaseStoreCost { get; set; } = 10;

    

    public void BuyStore()
    {
        bool isEnoughMoneyForBuy = IsEnoughMoneyForBuy();
        if ( isEnoughMoneyForBuy == false )
            return ;

        Money.Instance.Currency =- BaseStoreCost;
        
        StoreCount++;
        
        _storeUi.SetUi( StoreCount );
    }

    private bool IsEnoughMoneyForBuy()
    {
        return Money.Instance.Currency >= BaseStoreCost;
    }
}
}