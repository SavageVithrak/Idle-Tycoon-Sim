using UnityEngine;


[ System.Serializable ]
public class HotkeyCommand
{
    private enum ModificatorType { Ctrl, Shift, Alt, } //Win

    public static string ResultString;
    public Transform GameObjectToSelect;
    [ SerializeField ] private KeyCode _keyCode;

    [ SerializeField ] private ModificatorType[] ModificatorTypes = { ModificatorType.Alt };
    
    //непонятно как всунуть это значение внутрь аттрибута? чтобы стало const
    //и вот этих HotkeyCommand засунуть в Monobeh, чтобы из инспектора создавать сколько хочешь

    private void OnValidate()
    {
        ResultString = GetResultCodeInString();
    }

    private string GetResultCodeInString()
    {
        string modificatorStringCode = CalculateModificatorStringCode();

        return " " + _keyCode + modificatorStringCode;
    }

    private string CalculateModificatorStringCode()
    {
        string modificatorStringCode = string.Empty;

        foreach ( ModificatorType modificator in ModificatorTypes )
        {
            if ( modificator == ModificatorType.Shift )
                modificatorStringCode += "#";
            if ( modificator == ModificatorType.Alt )
                modificatorStringCode += "&";
            if ( modificator == ModificatorType.Ctrl )
                modificatorStringCode += "%";
        }

        return modificatorStringCode;
    }

}
