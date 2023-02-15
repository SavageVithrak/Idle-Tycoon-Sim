using UnityEditor;
using UnityEngine;


namespace _Project.Editor
{
public static class HierarchyMenus
{
    [ MenuItem( "GameObject/" + nameof( LogIdSelectedInHierarchy ), priority = 1 ) ]
    private static void LogIdSelectedInHierarchy( MenuCommand command )
    {
        GameObject[] selected = Selection.gameObjects;
        Debug.Log( $"<color=cyan> {selected[ 0 ].GetInstanceID()} </color>" + selected[ 0 ] );


        //CollapseHierarchyYasirkula.LogArrayInString( selected );
        //3rd commit
    }

    [ MenuItem( "GameObject/" + nameof( ShrinkWholeHierarchyExceptSelectedGo ), priority = 0 ) ]
    private static void ShrinkWholeHierarchyExceptSelectedGo( MenuCommand command )
    {
        HierarchySelectByReflection.ShrinkAllExceptSelected();
    }


    [ MenuItem( "GameObject/" + nameof( ShrinkWholeHierarchyToRoot ), priority = 0 ) ]
    private static void ShrinkWholeHierarchyToRoot( MenuCommand command )
    {
        HierarchySelectByReflection.ShrinkToRoot();
    }

    [ MenuItem( "GameObject/" + nameof( ExpandHierarchyToIdAndOneChildren ), priority = 0 ) ]
    private static void ExpandHierarchyToIdAndOneChildren( MenuCommand command )
    {

        /*
         раскрывает выбранный и на 1 уровень детей вниз
         */
        int selectedId = GetSelectedGoId();

        HierarchySelectByReflection.ExpandGoAndChildrenById( selectedId );
    }

    [ MenuItem( "GameObject/" + nameof( LogExpandedCount ), priority = 0 ) ]
    private static void LogExpandedCount( MenuCommand command )
    {
        HierarchySelectByReflection.DebugExpandGoAndChildrenById();
    }

    private static int GetSelectedGoId( )
    {
        GameObject[] selected = Selection.gameObjects;
        GameObject randomOldSelected = selected[ 0 ];

        return randomOldSelected.GetInstanceID();
    }

}
}