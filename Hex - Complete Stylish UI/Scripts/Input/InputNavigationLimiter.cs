using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.UI.Hex
{
    [AddComponentMenu("Hex UI/Input/Input Navigation Limiter")]
    public class InputNavigationLimiter : MonoBehaviour
    {
        [SerializeField] private GameObject sourceButton;

        void Awake()
        {
            if (sourceButton == null)
            {
                sourceButton = gameObject;
            }
        }

        void Update()
        {
            if (ControllerManager.instance != null 
                && EventSystem.current.currentSelectedGameObject != sourceButton
                && (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.transform.parent != transform))
            {
                ControllerManager.instance.SelectUIObject(transform.GetChild(0).gameObject);
            }
        }
    }
}