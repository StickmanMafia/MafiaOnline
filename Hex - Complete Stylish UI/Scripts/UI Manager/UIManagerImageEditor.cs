#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CustomEditor(typeof(UIManagerImage))]
    public class UIManagerImageEditor : Editor
    {
        private UIManagerImage uimiTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            uimiTarget = (UIManagerImage)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var UIManagerAsset = serializedObject.FindProperty("UIManagerAsset");
            var colorType = serializedObject.FindProperty("colorType");
            var useCustomColor = serializedObject.FindProperty("useCustomColor");
            var useCustomAlpha = serializedObject.FindProperty("useCustomAlpha");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
            HexUIEditorHandler.DrawProperty(UIManagerAsset, customSkin, "UI Manager");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 10);

            if (uimiTarget.UIManagerAsset != null)
            {
                HexUIEditorHandler.DrawProperty(colorType, customSkin, "Color Type");
                useCustomColor.boolValue = HexUIEditorHandler.DrawToggle(useCustomColor.boolValue, customSkin, "Use Custom Color");
                if (useCustomColor.boolValue == true) { GUI.enabled = false; }
                useCustomAlpha.boolValue = HexUIEditorHandler.DrawToggle(useCustomAlpha.boolValue, customSkin, "Use Custom Alpha");
            }

            else { EditorGUILayout.HelpBox("UI Manager should be assigned.", MessageType.Error); }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif