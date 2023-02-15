using UnityEditor;
using UnityEngine;



/*ведь я буду скрипты экспортировать, зачем тогда создавать папки - если можно все вместе экспортнуть?!
уже будет: 
Assets/_Project/_Scripts/Editor/$THIS$. тогда нужно: 
        _Scripts/Core
        _Scripts/Extensions - скорее экспортну
    логичнее доделать .unitypackage в котором будут все плюшки
*/
public static class InitProject
{
    
    
    [MenuItem( @"ProjectSettings/InitAllMy" )]
    private static void InitAllMy()
    {
        EnableFastEnterPlayMode();
        
    }
    
    //создать канвас, растянуть его, поставить ScaleWithScreen, поставить разрешение

    private static void EnableFastEnterPlayMode() => 
        EditorSettings.enterPlayModeOptionsEnabled = true;

    
    [MenuItem(@"ProjectSettings/Create Folder and Some Assets")]
    private static void CreateFolders()
    {
        var parent2Folder = NotImplement();
        AssetDatabase.CreateFolder(parent2Folder, "Core");
        
        //string guid = AssetDatabase.CreateFolder("Assets/My Folder", "My Another Folder");
        //string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
        //Debug.Log(newFolderPath);

        AssetDatabase.Refresh();
    }

    private static string NotImplement()
    {
        
        string currentFolder = GetМетодТочноБыл();
        //return assetsScriptsMenuPath.Trim();
        return currentFolder.Replace(@"/Menu", "");
    }

    private static string GetМетодТочноБыл()
    {
        return @"Assets/_Scripts/Menu";
    }
}
