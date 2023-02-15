using System;
using UnityEngine;

/*
    ^ НЕ РАБОТАЕТ  (ctrl on Windows, Linux, and macOS), 
    % (ctrl on Windows and Linux, cmd on macOS),
    # (shift), 
    & (alt). 
    _ = просто клавиша. To create a menu with hotkey g and no key modifiers pressed use "MyMenu/Do Something _g".
    For example to create a menu with hotkey shift-alt-g use "MyMenu/Do Something #&g". 
*/


//есть встроенные гк для save load selection. на alt+Shift+1..8  мб проще в них ставить sceneManager, SaveLoadManager,...?

[ ExecuteAlways ] // [ ExecuteInEditMode ]
//[IntializeOnLoad] //= Allows you to initialize an Editor class when Unity loads, and when your scripts are recompiled
public class HotkeyChooser : MonoBehaviour
{
    public static HotkeyChooser Instance { get; private set; }

    private void Awake()
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else if ( Instance == this )
        {
            Destroy( gameObject );
        }
    }


    [ SerializeField ] private GameObject _ctrlAltD;

    public GameObject CtrlAltD => _ctrlAltD;
    //public GameObject Ctrl_Alt_D => _ctrlAltD;
    //todo custom Inspector Drawer, в котором CtrlAltD -> не в Ctrl Alt D, а в Ctrl+Alt+D, и еще и разными цветами выделять модификаторы и собственно букву. Win = синий. буква = зеленый. Ctrl = оранж, Alt = , Shift = голубой. 

    [ SerializeField ] private string _sample = "from singlthone";

    public string Sample => _sample;
    //[ SerializeField ] private GameObject _ctrlAltD;

    [ SerializeField ] private string[] _strings;


}