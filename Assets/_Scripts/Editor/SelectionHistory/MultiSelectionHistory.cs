using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Editor
{
/// <summary>
/// Adds Back and Forward items to the Edit > Selection menu to navigate between Hierarchy and Project pane selections.
/// </summary>
[ InitializeOnLoad ]
internal static class MultiSelectionHistory
{ //https://github.com/mminer/selection-history-navigator/blob/master/SelectionHistoryNavigator.cs

    //гк Ctrl+tab вроде и должны работать - но хер там
    //todo в идеале вообще окно истории всех выбранных. которые щелкать по гк: например g5+q, пауза, и ждет что кликну как в aceJump 1234qwrasdzxc. если, например, выбранные уже свернул, или не помню как давно они выбирались = как глубоко в списке  
    //todo скачай ассеты Selection History 25 января 2023 зцб +сс
    private static HistoryInfo _activeSelection;


    private static bool _ignoreNextSelectionChangedEvent;
    private static EditorWindow _lastWindow;

    public static Stack<HistoryInfo> PreviousSelections => _previousSelections;
    //отрубил инспекцию кот предлагала Prop => _field в более многословную. поаккуратнее
    
    private static readonly Stack<HistoryInfo> _nextSelections = new Stack<HistoryInfo>();
    private static readonly Stack<HistoryInfo> _previousSelections = new Stack<HistoryInfo>();

    static MultiSelectionHistory()
    {
        Selection.selectionChanged += OnSelectionChange;
    }

    private static void OnSelectionChange()
    {
        if ( _ignoreNextSelectionChangedEvent )
        {
            //while click Back or Forward
            _ignoreNextSelectionChangedEvent = false;
            return;
        }

        if ( _activeSelection != null )
        {
            _previousSelections.Push( _activeSelection );
        }

        _activeSelection = new HistoryInfo( Selection.objects, EditorWindow.focusedWindow );
        _nextSelections.Clear();
    }



    private const string _backMenuLabelHotkey = " %[";
    private const string _forwardMenuLabelHotkey = " %]";

    private const string _backMenuLabel = "Edit/Selection/Multi Back" + _backMenuLabelHotkey;
    private const string _forwardMenuLabel = "Edit/Selection/Multi Forward" + _forwardMenuLabelHotkey;

    [ MenuItem( _backMenuLabel ) ]
    private static void Back()
    {
        if ( _activeSelection != null )
        {
            HistoryInfo next = new HistoryInfo( Selection.objects, EditorWindow.focusedWindow );
            _nextSelections.Push( next );
        }

        HistoryInfo prev = _previousSelections.Pop();
        Selection.objects = prev.Selected;
        OpenWindow( prev.OpenedWindow ); 

        _ignoreNextSelectionChangedEvent = true;
    }


    [ MenuItem( _forwardMenuLabel ) ]
    private static void Forward()
    {
        if ( _activeSelection != null )
        {
            HistoryInfo prev = new HistoryInfo( Selection.objects, EditorWindow.focusedWindow );
            _previousSelections.Push( prev );
        }

        HistoryInfo next = _nextSelections.Pop();
        Selection.objects = next.Selected;
        OpenWindow( next.OpenedWindow ); 

        _ignoreNextSelectionChangedEvent = true;
    }

    private static void OpenWindow( EditorWindow nextOpenedWindow )
    {
        EditorWindowHelper.GetWindow( nextOpenedWindow );
    }

    [ MenuItem( _backMenuLabel, true ) ]
    private static bool ValidateBack() => _previousSelections.Count > 0;

    [ MenuItem( _forwardMenuLabel, true ) ]
    private static bool ValidateForward() => _nextSelections.Count > 0;


}
}