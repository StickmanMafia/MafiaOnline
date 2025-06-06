﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CustomEditor(typeof(UIGradient))]
    public class UIGradientEditor : Editor
    {
        private GUISkin customSkin;
        private int currentTab;

        private void OnEnable()
        {
            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var _effectGradient = serializedObject.FindProperty("_effectGradient");
            var _gradientType = serializedObject.FindProperty("_gradientType");
            var _offset = serializedObject.FindProperty("_offset");
            var _zoom = serializedObject.FindProperty("_zoom");
            var _modifyVertices = serializedObject.FindProperty("_modifyVertices");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 6);
            HexUIEditorHandler.DrawPropertyCW(_effectGradient, customSkin, "Gradient", 100);
            HexUIEditorHandler.DrawPropertyCW(_gradientType, customSkin, "Type", 100);
            HexUIEditorHandler.DrawPropertyCW(_offset, customSkin, "Offset", 100);
            HexUIEditorHandler.DrawPropertyCW(_zoom, customSkin, "Zoom", 100);
            _modifyVertices.boolValue = HexUIEditorHandler.DrawToggle(_modifyVertices.boolValue, customSkin, "Complex Gradient");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif