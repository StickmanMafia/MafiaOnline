#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GraphicsManager))]
    public class GraphicsManagerEditor : Editor
    {
        private GraphicsManager gmTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            gmTarget = (GraphicsManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var resolutionDropdown = serializedObject.FindProperty("resolutionDropdown");

            var initializeResolutions = serializedObject.FindProperty("initializeResolutions");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
            HexUIEditorHandler.DrawPropertyCW(resolutionDropdown, customSkin, "Resolution Dropdown", 132);

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 10);
            initializeResolutions.boolValue = HexUIEditorHandler.DrawToggle(initializeResolutions.boolValue, customSkin, "Initialize Resolutions");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif