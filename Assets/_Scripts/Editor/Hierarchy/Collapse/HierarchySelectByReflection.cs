using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace _Project.Editor
{
public static class HierarchySelectByReflection
{ //Yasirkula

    private const BindingFlags InstanceFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    private const BindingFlags StaticFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

    static HierarchySelectByReflection( )
    {
        CashHierarchyTreeMembers();  
    }
    
    private static void CashHierarchyTreeMembers( )
    {
        //throw new System.NotImplementedException();
    }


    public static void ShrinkToRoot( )
    {
        GameObject[] rootGameObjects = UnityEngine.SceneManagement.SceneManager
           .GetActiveScene().GetRootGameObjects();

        int[] rootIds = ( from rootGameObject in rootGameObjects
                          select rootGameObject.GetInstanceID() ).ToArray();

        ShrinkToIds( rootIds );
    }
    /*
     project скрыть все, кроме папки X. для каждой папки своя гк. в рефлексии получать имя папки = оставить _Scripts.Core, или Prefabs или Sprites
     
     вычислить ось изменений. буду вызывать так:
     + скрыть всё до корня
     + скрыть до корня, кроме ветки выделенного ShrinkAllExceptSelected
        кроме выделеннЫХ
     * нажал ПКМ на треугольник скрытия Go = скрыть все кроме кликнутого, кликнутый GO раскрыть со всеми детьми.
     *     не гуглится  unity drop-down arrow in code Hierarchy
     *     в UI (вне кода) = Alt+LMB click на треугольник
     + раскрыть до выделенного = для createParent
     *
    т.е. обрабатывать Id. мб через enum? не, просто публичн методы оставь
    
        мб под InitializeOnLoad отдельный bootstrap'er? чтобы все автозопускающееся из одного места
    
        кэшировать полученное в релексии = 1 раз в конструкторе
        меню отдельно не нужно, все в 1 файл
     */

    private static void ShrinkToIds( int[] ids )
    {
        EditorWindow hierarchyWindow = GetHierarchyWindow();
        object hierarchyTreeOwner = GetHierarchyTreeOwner( hierarchyWindow );
        object hierarchyTree = GetHierarchyTree( hierarchyTreeOwner );
        object treeViewDataSource = GetTreeViewDataSource( hierarchyTree );
        //SetExpandedIDs( treeViewDataSource );

        RevealItems( treeViewDataSource, ids );
        hierarchyWindow.Repaint();
    }

    public static void ExpandGoAndChildrenById( int newIDToExpand )
    {
        //но скрывает остальные ветки!
        EditorWindow hierarchyWindow = GetHierarchyWindow();
        object hierarchyTreeOwner = GetHierarchyTreeOwner( hierarchyWindow );
        object hierarchyTree = GetHierarchyTree( hierarchyTreeOwner );
        object treeViewDataSource = GetTreeViewDataSource( hierarchyTree );
        SetExpandedIDs( treeViewDataSource ); //реально скрывает до корня. можно без нее.
        //пробуй отдельно SetExpandedIDs +передавай id параметром в рефлексию
        //и отдельно RevealItems

        //мб можно в Undo.selected. нужно? парлельную свою Undo.selectedOnly

        TreeViewState treeView = GetTreeViewState( hierarchyTreeOwner );
        List<int> oldTreeViewExpandedIDs = treeView.expandedIDs;
        oldTreeViewExpandedIDs.Add( newIDToExpand );

        RevealItems( treeViewDataSource, oldTreeViewExpandedIDs );
        hierarchyWindow.Repaint();
    }

    public static void DebugExpandGoAndChildrenById( )
    {
        EditorWindow hierarchyWindow = GetHierarchyWindow();
        object hierarchyTreeOwner = GetHierarchyTreeOwner( hierarchyWindow );
        TreeViewState treeView = GetTreeViewState( hierarchyTreeOwner );


        int count = treeView.expandedIDs.Count; //сцена считается за 1, но можно свернуть все и тогда = 0 //считает если внутри СВЕРНУТОГО родителя  ребенок со своими РАЗВЕРНУТЫМИ детьми. 
        Debug.Log( $"expanded Count: <color=cyan> {count} </color>" );

        //работает treeView.lastClickedID

        hierarchyWindow.Repaint();
    }

    public static void ShrinkAllExceptSelected( )
    {
        EditorWindow hierarchyWindow = GetHierarchyWindow();
        object hierarchyTreeOwner = GetHierarchyTreeOwner( hierarchyWindow );
        object treeViewController = GetHierarchyTree( hierarchyTreeOwner );
        TreeViewState treeView = GetTreeViewState( hierarchyTreeOwner );

        object treeViewDataSource = GetTreeViewDataSource( treeViewController );
        SetExpandedIDs( treeViewDataSource );


        //здесь selected а не expandedIDs
        RevealItems( treeViewDataSource, treeView.selectedIDs );

        hierarchyWindow.Repaint();
    }

    [ MenuItem( "GameObject/" + nameof( PartialSetExpandedIDs ) ) ]
    public static void PartialSetExpandedIDs( )
    { //проверить после того как по гк поставил выделенного ребенком в lastSelected
        //проверка - ниче не должно измениться
        
        EditorWindow hierarchyWindow = GetHierarchyWindow();
        object hierarchyTreeOwner = GetHierarchyTreeOwner( hierarchyWindow );
        object treeViewController = GetHierarchyTree( hierarchyTreeOwner );

        object treeViewDataSource = GetTreeViewDataSource( treeViewController );

        //SetExpandedIDs( treeViewDataSource );
        int[] expandedIDs = GetTreeViewState().expandedIDs.ToArray();
        SetExpandedIDs( treeViewDataSource, expandedIDs );//не работает, даже если в конце перерисовать. но и до корня не скрывает. ничего не делает.

        //hierarchyWindow.Repaint();
    }

    [ MenuItem( "GameObject/" + nameof( PartialRevealItems ) ) ]
    public static void PartialRevealItems( )
    {
        EditorWindow hierarchyWindow = GetHierarchyWindow();
        object hierarchyTreeOwner = GetHierarchyTreeOwner( hierarchyWindow );
        object treeViewController = GetHierarchyTree( hierarchyTreeOwner );
        object treeViewDataSource = GetTreeViewDataSource( treeViewController );

        TreeViewState treeView = GetTreeViewState();

        RevealItems( treeViewDataSource, treeView.selectedIDs );//показывает и скрытые. напр если выделенный в свернутом GO
        hierarchyWindow.Repaint(); //да, обязательно
    }

    [ MenuItem( "GameObject/" + nameof( RepaintHierarchy ) ) ]
    public static void RepaintHierarchy( )
    { //проверить после того как по гк поставил выделенного ребенком в lastSelected
        GetHierarchyWindow().Repaint();
    }

    private static TreeViewState GetTreeViewState( )
    {
        EditorWindow hierarchyWindow = GetHierarchyWindow();
        object hierarchyTreeOwner = GetHierarchyTreeOwner( hierarchyWindow );
        return GetTreeViewState( hierarchyTreeOwner );
    }


    private static EditorWindow GetHierarchyWindow( )
    {

        FieldInfo fieldInfo = typeof( EditorWindow ).Assembly
           .GetType( "UnityEditor.SceneHierarchyWindow" )
           .GetField( "s_LastInteractedHierarchy", StaticFlags );
        return fieldInfo.GetValue( null ) as EditorWindow;
    }

    private static object GetHierarchyTreeOwner( EditorWindow hierarchyWindow )
    {
        return hierarchyWindow.GetType().GetField( "m_SceneHierarchy", InstanceFlags ).GetValue( hierarchyWindow );
    }

    private static object GetHierarchyTree( object hierarchyTreeOwner )
    {
        return hierarchyTreeOwner.GetType()
           .GetField( "m_TreeView", InstanceFlags )
           .GetValue( hierarchyTreeOwner );
    }

    private static object GetTreeViewDataSource( object treeViewController )
    {
        return treeViewController.GetType().GetProperty( "data", InstanceFlags ).GetValue( treeViewController, null );
    }


    private static void SetExpandedIDs( object treeViewDataSource )
    {
        SetExpandedIDs(treeViewDataSource, new int[0] );
    }
    
    private static void SetExpandedIDs( object treeViewDataSource, int[] ids )
    {
        treeViewDataSource.GetType()
           .GetMethod( "SetExpandedIDs", InstanceFlags )
           .Invoke( treeViewDataSource, new object[]
                { ids }
            );
    }



    private static object RevealItems( object treeViewDataSource, List<int> selectedIDs )
    {
        int[] selected = selectedIDs.ToArray();
        return RevealItems( treeViewDataSource, selected );
    }

    private static object RevealItems( object treeViewDataSource, int[] selectedIDs )
    {
        return treeViewDataSource.GetType()
           .GetMethod( "RevealItems", InstanceFlags )
           .Invoke( treeViewDataSource, new object[]
            { selectedIDs } );
    }


    private static TreeViewState GetTreeViewState( object hierarchyTreeOwner )
    {
        FieldInfo fieldInfo = hierarchyTreeOwner.GetType()
           .GetField( "m_TreeViewState", InstanceFlags );
        return ( TreeViewState ) fieldInfo.GetValue( hierarchyTreeOwner );
    }
}
}