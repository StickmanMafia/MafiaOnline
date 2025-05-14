#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(HUDManager))]
    public class HUDManagerEditor : Editor
    {
        private HUDManager hmTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            hmTarget = (HUDManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var HUDPanel = serializedObject.FindProperty("HUDPanel");

            var fadeSpeed = serializedObject.FindProperty("fadeSpeed");
            var defaultBehaviour = serializedObject.FindProperty("defaultBehaviour");

            var onSetVisible = serializedObject.FindProperty("onSetVisible");
            var onSetInvisible = serializedObject.FindProperty("onSetInvisible");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
            HexUIEditorHandler.DrawProperty(HUDPanel, customSkin, "HUD Panel");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 10);
            HexUIEditorHandler.DrawProperty(fadeSpeed, customSkin, "Fade Speed", "Sets the fade animation speed.");
            HexUIEditorHandler.DrawProperty(defaultBehaviour, customSkin, "Default Behaviour");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Events", 10);
            EditorGUILayout.PropertyField(onSetVisible, new GUIContent("On Set Visible"), true);
            EditorGUILayout.PropertyField(onSetInvisible, new GUIContent("On Set Invisible"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif