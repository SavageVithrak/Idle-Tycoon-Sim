using System;
using UnityEditor;
using UnityEngine;

namespace Editor.IndiePixel.Renamer
{
internal class ObjectRenameWindow : EditorWindow
{ //https://www.youtube.com/watch?v=XH2ut7WYymk&list=PL5V9qxkY_RnIOesA0OXr05miU43xOURii&index=8
    //!! https://youtu.be/XH2ut7WYymk&t=996 hotkeys встроенные изменить
    GameObject[] _selected = Array.Empty<GameObject>();
    private string _prefix, _name, _suffix;
    private bool _shouldAddNumber;


    //todo можно в поле писать с пробелами, а скрипт их вырежет и переведет в PascalCase
    [ MenuItem( "rename/" + nameof( RenameSelectedObject ) + " " ) ]
    public static void RenameSelectedObject()
    {
        ObjectRenameWindow window = GetWindow<ObjectRenameWindow>();
        GUIContent content = new GUIContent( "Rename Objects" );
        window.titleContent = content;
        window.Show(); //но вроде и так вызвается само
    }

    private void OnGUI()
    {
        _selected = Selection.gameObjects;

        EditorGUILayout.LabelField( "Selected: " + _selected.Length );

        DrawUi();

        Repaint(); //не моё, а  UnityEditor.EditorWindow = нужно цветом выделять такое, что от родительск класса а не из этого моего
    }

    private void DrawUi()
    {
        //_name = EditorGUILayout.TextField( "Name: ", _name );
        const string controlName = "name";
        GUI.SetNextControlName( controlName );
        _name = EditorGUILayout.TextField( "Name: ", _name );

        //EditorGUI.FocusTextInControl( controlName ); //тоже работает
        GUI.FocusControl( controlName );

        bool buttonPressed = GUILayout.Button(
            "Rename Selected"
          , GUILayout.Height( 45 )
          , GUILayout.ExpandWidth( true )
        );
        
        var eventKeyPressed = Event.current.type == EventType.KeyDown;
        var eventCode = Event.current.keyCode;

        //const KeyCode enterKeyCode = KeyCode.Return;
        //bool pressedEsc = Input.GetKeyDown( KeyCode.Escape );
        
        bool pressedEnter = eventCode == KeyCode.Return || eventCode == KeyCode.KeypadEnter;
        bool pressedEsc = eventCode == KeyCode.Escape;
        
        //!! надо нажать 2 раза хоть Enter=Return хоть Esc - на 1ый клик только фокус убирается с текстового поля

        if ( eventKeyPressed == false )
            return;

        bool submit = buttonPressed || pressedEnter;
        if ( submit )
        {
            SortSelected();
            RenameOne();
            GetWindow<ObjectRenameWindow>().Close();
        } else if ( pressedEsc )
        {
            GetWindow<ObjectRenameWindow>().Close();
        }

    }

    private void RenameOne()
    {
        string finalName = string.Empty;

        if ( _name?.Length > 0 )
        {
            finalName += _name;
        }

        _selected[ 0 ].name = finalName;

        if ( _selected.Length == 1 )
        {
            return;
        }

        for ( int i = 1; i < _selected.Length; i++ )
        {
            _selected[ i ].name = finalName;
            _selected[ i ].name += "_" + i.ToString();
        }
    }

    private void DrawUiWithFixes()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space( 10 );
        EditorGUILayout.BeginVertical();
        GUILayout.Space( 10 );

        _prefix = EditorGUILayout.TextField( "Prefix: ", _prefix );
        _name = EditorGUILayout.TextField( "Name: ", _name ); //или TextArea
        _suffix = EditorGUILayout.TextField( "Suffix: ", _suffix );

        bool buttonPressed = GUILayout.Button(
            "Rename Selected"
          , GUILayout.Height( 45 )
          , GUILayout.ExpandWidth( true )
        );
        if ( buttonPressed )
        {
            SortSelected();
            RenameWithFixes();

        }


        GUILayout.Space( 10 );
        EditorGUILayout.EndVertical();
        GUILayout.Space( 10 );
        EditorGUILayout.EndHorizontal();


    }

    private void RenameWithFixes()
    {
        for ( int i = 0; i < _selected.Length; i++ )
        {
            string finalName = string.Empty;
            if ( _prefix?.Length > 0 )
            {
                finalName += _prefix;

            }

            if ( _name?.Length > 0 )
            {
                finalName += "_" + _name;

            }

            if ( _suffix?.Length > 0 )
            {
                finalName += "_" + _suffix;
            }

            if ( _shouldAddNumber )
            {
                finalName += "_" + i.ToString( "000" );
            }

            _selected[ i ].name = finalName;
        }

    }

    private void SortSelected()
    { //сам он использует delegate https://youtu.be/DQsTy40PAzA?list=PL5V9qxkY_RnIOesA0OXr05miU43xOURii&t=361
        Array.Sort( _selected
          , ( a, b ) =>
                String.Compare( a.name, b.name, StringComparison.Ordinal ) );

        /*
        Array.Sort( _selected, delegate( GameObject a, GameObject b )
        {
            return a.name.CompareTo( b.name );
        } );
         */
    }


}
}