using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{
public class ParentToLastSelected
{
    [ MenuItem( "Game/" + nameof( SetParentToLastSelected )) ]
    public static void SetParentToLastSelected()
    {
        Stack<HistoryInfo> previousSelections = MultiSelectionHistory.PreviousSelections; //либо можно взять в TreeViewState, кот использовал в рефлексии treeView.lastClickedID

        Object lastSelected = previousSelections.Peek().Selected[ 0 ];

        if ( lastSelected is GameObject lastSelectedGo )
        {
            SetAllSelectedToParent( lastSelectedGo );
        }
        
        //HierarchySelectByReflection.ExpandGoAndChildrenById( Selection.activeInstanceID );
        //todo add to Undo.x

        Debug.Log( $"<color=cyan> {lastSelected} </color>" );
    }

    private static void SetAllSelectedToParent( GameObject parent )
    {
        foreach ( Object obj in Selection.objects )
        {
            if ( obj is GameObject go )
            {
                go.transform.SetParent( parent.transform );
            }
        }
    }
}
}