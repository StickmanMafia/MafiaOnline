using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Hex
{
    [CreateAssetMenu(fileName = "New Controller Preset Manager", menuName = "Hex UI/Controller/Controller Preset Manager")]
    public class ControllerPresetManager : ScriptableObject
    {
        public ControllerPreset keyboardPreset;
        public ControllerPreset xboxPreset;
        public ControllerPreset dualsensePreset;
        public List<ControllerPreset> customPresets = new List<ControllerPreset>();
    }
}