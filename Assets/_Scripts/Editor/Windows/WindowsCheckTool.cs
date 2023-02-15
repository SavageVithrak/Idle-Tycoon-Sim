using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class WindowsCheckTool
{

    [ MenuItem( "Window/Save to file" ) ]
    public static void Check()
    {
        StringBuilder sb = new StringBuilder();
        var game = EditorWindowHelper.GetWindow( WindowType.Game );
        sb.AppendLine( game.ToString() );
        var scene = EditorWindowHelper.GetWindow( WindowType.Scene );
        sb.AppendLine( scene.ToString() );
        var hierarchy = EditorWindowHelper.GetWindow( WindowType.Hierarchy );
        sb.AppendLine( hierarchy.ToString() );
        var console = EditorWindowHelper.GetWindow( WindowType.Console );
        sb.AppendLine( console.ToString() );
        var inspector = EditorWindowHelper.GetWindow( WindowType.Inspector );
        sb.AppendLine( inspector.ToString() );
        Write( sb.ToString() );
    }

    private static void Write( string output )
    {
        StreamWriter writer = new StreamWriter( "Assets/all windows names.txt", false );
        writer.Write( output );
        writer.Close();

        Debug.Log( output );
    }
}