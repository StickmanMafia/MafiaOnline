#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : Editor
    {
        private LocalizationManager lmTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            lmTarget = (LocalizationManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var UIManagerAsset = serializedObject.FindProperty("UIManagerAsset");
            var languageSelector = serializedObject.FindProperty("languageSelector");

            var setLanguageOnAwake = serializedObject.FindProperty("setLanguageOnAwake");
            var updateItemsOnSet = serializedObject.FindProperty("updateItemsOnSet");
            var saveLanguageChanges = serializedObject.FindProperty("saveLanguageChanges");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
            HexUIEditorHandler.DrawProperty(UIManagerAsset, customSkin, "UI Manager");
            HexUIEditorHandler.DrawProperty(languageSelector, customSkin, "Language Selector");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 10);
            setLanguageOnAwake.boolValue = HexUIEditorHandler.DrawToggle(setLanguageOnAwake.boolValue, customSkin, "Set Language On Awake");
            updateItemsOnSet.boolValue = HexUIEditorHandler.DrawToggle(updateItemsOnSet.boolValue, customSkin, "Update Items On Language Set");
            saveLanguageChanges.boolValue = HexUIEditorHandler.DrawToggle(saveLanguageChanges.boolValue, customSkin, "Save Language Changes");
            LocalizationManager.enableLogs = HexUIEditorHandler.DrawToggle(LocalizationManager.enableLogs, customSkin, "Enable Logs");

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif