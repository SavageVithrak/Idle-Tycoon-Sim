using UnityEditor;

static class ParentMenuItem
{//https://forum.unity.com/threads/last-object-selected-in-editor.239900/
    private const string _hotkey = " %#&o";
    
    
    /*то же, что и Ctrl+Alt+E | Make Parent
    [ MenuItem( "Tools/Make Parent with Hotkey") ]
    private static void ParentToPreviouslyActive()
    {
        EditorApplication.ExecuteMenuItem( "GameObject/Make Parent" );
        //Undo.SetTransformParent(go.transform, activeTransform, "Parent to Previously Active");
        
         old version?    Perform re-parenting.
        foreach (var go in Selection.gameObjects)
            if (go != PreviousLastActiveGameObject)
                Undo.SetTransformParent(go.transform, activeTransform, "Parent to Previously Active");
        
 
        // Allow multiple individual objects to be parented.
        // Selection.activeGameObject = LastActiveGameObject = PreviousLastActiveGameObject;
    }
*/

    [ MenuItem( "Tools/Make Parent with Hotkey" + _hotkey, true ) ]
    private static bool ValidateParentToPreviouslyActive()
    {
        return Selection.gameObjects.Length > 1;
    }
    
    /*
    [DrawGizmo(GizmoType.NotSelected)]
    private static void DrawGizmoForPreviouslyActiveGameObject(Transform transform, GizmoType type) {
        if (PreviousLastActiveGameObject != transform.gameObject)
            return;
 
        Color restoreColor = Gizmos.color;
 
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
 
        Gizmos.color = restoreColor;
    }
    */

}