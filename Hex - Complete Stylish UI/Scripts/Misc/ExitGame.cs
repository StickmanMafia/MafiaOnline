using UnityEngine;

namespace Michsky.UI.Hex
{
    public class ExitGame : MonoBehaviour
    {
        public void Exit() 
        { 
            Application.Quit();
#if UNITY_EDITOR
            Debug.Log("<b>[Hex UI]</b> Exit function works in builds only.");
#endif
        }
    }
}