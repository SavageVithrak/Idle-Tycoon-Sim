using UnityEditor;
using UnityEngine;

public class TestPopup : EditorWindow
{
    static TestPopup()
    {
        //EditorApplication.update += CloseOnLostFocus;  //todo баги если закрыть
    }

    static void CloseOnLostFocus()
    {
        if ( win != null )
            return;

        if ( focusedWindow && focusedWindow != win )
        {
            win.Close();
            win = null;
        }
    }

    private static TestPopup win;

    //[MenuItem("Test/Popup")] 
    static void Init()
    {
        win = CreateInstance<TestPopup>();
        win.position = new Rect( Screen.width / 2, Screen.height / 2, 300, 200 );
        win.Show();
        win.Focus();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField( "This is an example of EditorWindow.ShowPopup", EditorStyles.wordWrappedLabel );
        GUILayout.Space( 70 );
        if ( GUILayout.Button( "Agree!" ) )
            Close();
        var ev = Event.current;
        if ( ev.type == EventType.KeyDown && ev.keyCode == KeyCode.Escape )
        {
            Close();
        }
    }
}