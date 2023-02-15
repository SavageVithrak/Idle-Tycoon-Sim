using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Editor
{
public class LastSelected : MonoBehaviour
{
    //https://answers.unity.com/questions/1248389/how-to-refernece-the-button-previously-selected-ga.html
    
    [SerializeField] private EventSystem _eventSystem; //select event system in editor
    private GameObject _previousSelectedGameObject;
    private GameObject _currentSelectedGameObjectRecent;
    
    
    private void GetLastGameObjectSelected()
    {
        GameObject selectedGameObject = _eventSystem.currentSelectedGameObject;
        
        if ( selectedGameObject == _currentSelectedGameObjectRecent )
            return;
        
        _previousSelectedGameObject = _currentSelectedGameObjectRecent;
        _currentSelectedGameObjectRecent = selectedGameObject;
    }
    
    
}
}