using TMPro;
using UnityEngine;

namespace _Scripts.Core
{
public class MoneyUi : MonoBehaviour
{
    [ SerializeField ] private TextMeshProUGUI _ui;
    [ SerializeField ] private string _ending = " $";

    public void SetUi( int amount )
    {
        _ui.text = amount.ToString() + _ending;
        
    }
}


}