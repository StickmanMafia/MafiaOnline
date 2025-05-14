using UnityEngine;

namespace Michsky.UI.Hex
{
    public class LaunchURL : MonoBehaviour
    {
        public void GoToURL(string URL)
        {
            Application.OpenURL(URL);
        }
    }
}