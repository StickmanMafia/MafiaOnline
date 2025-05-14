using UnityEngine;

namespace Michsky.UI.Hex
{
    public class ControllerManagerObject : MonoBehaviour
    {
        [SerializeField] private GameObject targetObject;
        [SerializeField] private ObjectType objectType = ObjectType.Gamepad;

        public enum ObjectType { Keyboard, Gamepad }

        void Start()
        {
            if (ControllerManager.instance == null)
                return;

            if (targetObject == null) { targetObject = gameObject; }
            if (objectType == ObjectType.Gamepad) { ControllerManager.instance.gamepadObjects.Add(targetObject); }
            else if (objectType == ObjectType.Keyboard) { ControllerManager.instance.keyboardObjects.Add(targetObject); }
        }
    }
}