#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CustomEditor(typeof(UIManagerText))]
    public class UIManagerTextEditor : Editor
    {
        private UIManagerText uimtTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            uimtTarget = (UIManagerText)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var UIManagerAsset = serializedObject.FindProperty("UIManagerAsset");
            var fontType = serializedObject.FindProperty("fontType");
            var colorType = serializedObject.FindProperty("colorType");
            var useCustomColor = serializedObject.FindProperty("useCustomColor");
            var useCustomAlpha = serializedObject.FindProperty("useCustomAlpha");
            var useCustomFont = serializedObject.FindProperty("useCustomFont");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
            HexUIEditorHandler.DrawProperty(UIManagerAsset, customSkin, "UI Manager");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 10);

            if (uimtTarget.UIManagerAsset != null)
            {
                if (useCustomFont.boolValue == true) { GUI.enabled = false; }
                HexUIEditorHandler.DrawProperty(fontType, customSkin, "Font Type");
                GUI.enabled = true;
                HexUIEditorHandler.DrawProperty(colorType, customSkin, "Color Type");
                useCustomColor.boolValue = HexUIEditorHandler.DrawToggle(useCustomColor.boolValue, customSkin, "Use Custom Color");
                if (useCustomColor.boolValue == true) { GUI.enabled = false; }
                useCustomAlpha.boolValue = HexUIEditorHandler.DrawToggle(useCustomAlpha.boolValue, customSkin, "Use Custom Alpha");
                GUI.enabled = true;
                useCustomFont.boolValue = HexUIEditorHandler.DrawToggle(useCustomFont.boolValue, customSkin, "Use Custom Font");
            }

            else { EditorGUILayout.HelpBox("UI Manager should be assigned.", MessageType.Error); }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif