using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[ InitializeOnLoad ]
public static class TrackLastSelectedAndWindow
{

    public static GameObject LastSelected { get; private set; }

    private static GameObject[] LastSelection;

    static TrackLastSelectedAndWindow()
    {
        //EditorApplication.update += OnUpdate; //every Editor Tick (basically Update() )
        //Selection.selectionChanged += OnSelectionChanged;
    }

    private static void OnSelectionChanged()
    {
        foreach ( Object o in Selection.objects )
        {
            Debug.Log( $"<color=cyan> {o} </color>" );
        }

        Debug.Log( $"-----------------------" );
    }


    private static void OnUpdate()
    {
        //не знаю там И или ИЛИ https://forum.unity.com/threads/last-object-selected-in-editor.239900/ = у него опечатка
        if ( Selection.gameObjects == null || Selection.gameObjects == LastSelection )
            return;


        if ( LastSelection != null )
        {
            HashSet<GameObject> lastSelectionHash = new HashSet<GameObject>( LastSelection );
            foreach ( GameObject go in Selection.gameObjects )
            {
                if ( lastSelectionHash.Contains( go ) == false )
                {
                    LastSelected = go;
                    break;
                }
            }
        }

        LastSelection = Selection.gameObjects;
    }
}