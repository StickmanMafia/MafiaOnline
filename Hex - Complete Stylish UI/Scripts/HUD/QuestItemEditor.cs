#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(QuestItem))]
    public class QuestItemEditor : Editor
    {
        private QuestItem qiTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            qiTarget = (QuestItem)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var questText = serializedObject.FindProperty("questText");
            var localizationKey = serializedObject.FindProperty("localizationKey");

            var questAnimator = serializedObject.FindProperty("questAnimator");
            var questTextObj = serializedObject.FindProperty("questTextObj");

            var useLocalization = serializedObject.FindProperty("useLocalization");
            var updateOnAnimate = serializedObject.FindProperty("updateOnAnimate");
            var minimizeAfter = serializedObject.FindProperty("minimizeAfter");
            var defaultState = serializedObject.FindProperty("defaultState");
            var afterMinimize = serializedObject.FindProperty("afterMinimize");

            var onDestroy = serializedObject.FindProperty("onDestroy");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Content", 6);
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.LabelField(new GUIContent("Quest Text"), customSkin.FindStyle("Text"), GUILayout.Width(-3));
            EditorGUILayout.PropertyField(questText, new GUIContent(""), GUILayout.Height(70));
            GUILayout.EndHorizontal();
            HexUIEditorHandler.DrawProperty(localizationKey, customSkin, "Localization Key");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 10);
            HexUIEditorHandler.DrawProperty(questAnimator, customSkin, "Quest Animator");
            HexUIEditorHandler.DrawProperty(questTextObj, customSkin, "Quest Text Object");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 10);
            useLocalization.boolValue = HexUIEditorHandler.DrawToggle(useLocalization.boolValue, customSkin, "Use Localization", "Bypasses localization functions when disabled.");
            updateOnAnimate.boolValue = HexUIEditorHandler.DrawToggle(updateOnAnimate.boolValue, customSkin, "Update On Animate");
            HexUIEditorHandler.DrawProperty(minimizeAfter, customSkin, "Minimize After");
            HexUIEditorHandler.DrawProperty(defaultState, customSkin, "Default State");
            HexUIEditorHandler.DrawProperty(afterMinimize, customSkin, "After Minimize");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Events", 10);
            EditorGUILayout.PropertyField(onDestroy, new GUIContent("On Destroy"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif