using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{
public class HistoryInfo
{
    public Object[] Selected;
    public EditorWindow OpenedWindow;
    //public WindowType OpenedWindowType;
    //+ можно хранить открыт ли инспектор, ...
    //+ можно открывать окна парами: если открыт Animation - то всегда открывать и Animator

    public HistoryInfo( Object[] selected, EditorWindow openedWindow )
    {
        Selected = selected;
        OpenedWindow = openedWindow;
    }
}
}