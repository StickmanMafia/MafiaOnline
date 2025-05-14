using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Hex
{
    [CreateAssetMenu(fileName = "New Controller Preset", menuName = "Hex UI/Controller/Controller Preset")]
    public class ControllerPreset : ScriptableObject
    {
        [Header("Settings")]
        public string controllerName = "Controller Name";

        [Space(10)]
        public List<ControllerItem> items = new List<ControllerItem>();

        public enum ItemType { Icon, Text }

        [System.Serializable]
        public class ControllerItem
        {
            public string itemID;
            public ItemType itemType;
            public Sprite itemIcon;
            public string itemText;
        }
    }
}