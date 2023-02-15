using System.Collections.Generic;
using Editor.IndiePixel.Renamer;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{

public class EmptyParent
{

    /*
     * все сделать норм имена
     * 
     * https://docs.unity3d.com/ScriptReference/Undo.html все действия добавлять в историю. чтобы работало ctrl+Z
     * CreateEmptyParent for selected:
     *     F2
     *     разворачивать созданный parent до ребенка
     *
     * удалить выбранный GO, но детей не удалять а перекинуть на Go выше (типа лишний пустой объект посередине хочу дуалить)
     */

    private const string _shiftAltD = " #&d";

    [ MenuItem( "for hotkeys/" + _shiftAltD ) ]
    private static void CreateGoAndRename()
    {
        //бред. ctrl+N есть же
        GameObject newObject = new GameObject
        {
            name = "new"
        };
        // SelectInHierarchy( newObject );
    }


    private const string _altD = " &d";

    [ MenuItem( "for hotkeys/" + _altD ) ]
    private static void SelectInHierarchyCtrlAltD()
    {
        var a = HotkeyChooser.Instance.Sample;
        Debug.Log( $"<color=cyan> {a} </color>" );
        //var toSelect = HotkeyChooser.CtrlAltD;
        //SelectInHierarchy( toSelect );
    }

    private const string _selectCamera = nameof( SelectCamera );

    [ MenuItem( "uGUI/" + _selectCamera ) ]
    private static void SelectCamera()
    {
        //todo сделать для: canvas, 1ый элемент в объекте Enviroment и подобное. мб отдельный GO, в котором [SerializeField] для штук 5 свободных клавиш = именно под выбор GO в иерархии. Под менеджеры. и к этому MonoBeh образаться синглтоном
        Camera camera = Camera.main;
        SelectInHierarchy( camera );
    }

    



    [ MenuItem( "for hotkeys/" + nameof( CreateEmptyParents ) ) ]
    private static void CreateEmptyParents()
    {
        Debug.Log( $"<color=cyan> {nameof( CreateEmptyParents )} </color>" );

        GameObject[] selectedChildrens = Selection.gameObjects;

        List<Transform> createdParents = new List<Transform>();

        foreach ( var selected in selectedChildrens )
        {

            var oldParent = selected.transform.parent;
            Transform newParent = new GameObject { name = "parent" }.transform;
            createdParents.Add( newParent );

            bool isAtTopInHierarchy = oldParent == null;
            if ( isAtTopInHierarchy == false )
            {
                var siblingIndex = oldParent.GetSiblingIndex();
                Debug.Log( "SiblingIndex = " + siblingIndex );
                newParent.parent = oldParent;

                newParent.SetSiblingIndex( siblingIndex );
            }

            selected.transform.SetParent( newParent );

        }

        var randomSelected = createdParents[ 0 ];

        Selection.SetActiveObjectWithContext( randomSelected, randomSelected );
        //EditorWindow.focusedWindow.SendEvent( Event.KeyboardEvent( "&f2" ) );
        //вроде срабатывает, но сразу и отпускается. и даже не из-за того, что гк перезаписывается отпусканием моих клавиш
    }

    
    private const string _ctrlShiftD = " %#d";
    private const string _ctrlShiftAltP = " %#&p";
    [ MenuItem( "GameObject/" + nameof( CreateOneEmptyParent ) + _ctrlShiftAltP , priority = 0 ) ]
    private static void CreateOneEmptyParent( MenuCommand command )
    {
        CreateParentForOneGo();
    }
    
    [ MenuItem( "GameObject/" + nameof( CreateOneEmptyParentAndRename ) , priority = 0 ) ]
    private static void CreateOneEmptyParentAndRename( MenuCommand command )
    {
        CreateParentForOneGo();
        ObjectRenameWindow.RenameSelectedObject();
    }

    
    
    private static void CreateParentForOneGo()
    {
        GameObject[] oldSelected = Selection.gameObjects;
        GameObject randomOldSelected = oldSelected[ 0 ];

        Transform oldParent = randomOldSelected.transform.parent;
        Transform newParent = new GameObject().transform;
        newParent.name = "id " + newParent.GetInstanceID();

        PutParentInHierarchy( oldParent, newParent );

        randomOldSelected.transform.SetParent( newParent );

        SelectInHierarchy( newParent );
        //выделяет, но скрывает дочерний GO. и само без этой строки оставляет старое выделение
        // Selection.SetActiveObjectWithContext( newParent, newParent ); //выделяет как нужно
        //!! если создавать parent для объекта у кот нет детей = то сворачивает всю сцену. значит неправильно использую ExpandById( randomOldSelected.GetInstanceID()

        HierarchySelectByReflection.ExpandGoAndChildrenById( randomOldSelected.GetInstanceID() );
    }

    private static void PutParentInHierarchy( Transform oldParent, Transform newParent )
    {
        bool isAtTopInHierarchy = oldParent == null;
        if ( isAtTopInHierarchy )
            return;
        
        int siblingIndex = oldParent.GetSiblingIndex();
        newParent.parent = oldParent;

        newParent.SetSiblingIndex( siblingIndex );
    }
    
    
    private static void SelectInHierarchy( Object toSelect )
    {
        Selection.SetActiveObjectWithContext( toSelect, toSelect );
        
    }
    
    
    
}
}