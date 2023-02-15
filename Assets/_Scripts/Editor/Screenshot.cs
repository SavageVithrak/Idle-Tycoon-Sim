using System.IO;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{
public class Screenshot
{

    [ MenuItem( "Game/" + nameof( TakeScreenshot ) ) ]
    public static void TakeScreenshot()
    {
        const string path = "Screenshots";

        Directory.CreateDirectory( path );

        int i = 0;
        while ( File.Exists( $"{path}/{i}.png" ) )
        {
            i++;
        }

        ScreenCapture.CaptureScreenshot( $"{path}/{i}.png" );
    }

}
}