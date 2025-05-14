#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(NotificationManager))]
    public class NotificationManagerEditor : Editor
    {
        private NotificationManager nmTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            nmTarget = (NotificationManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var icon = serializedObject.FindProperty("icon");
            var notificationText = serializedObject.FindProperty("notificationText");
            var localizationKey = serializedObject.FindProperty("localizationKey");
            var customSFX = serializedObject.FindProperty("customSFX");

            var itemAnimator = serializedObject.FindProperty("itemAnimator");
            var iconObj = serializedObject.FindProperty("iconObj");
            var textObj = serializedObject.FindProperty("textObj");

            var useLocalization = serializedObject.FindProperty("useLocalization");
            var updateOnAnimate = serializedObject.FindProperty("updateOnAnimate");
            var minimizeAfter = serializedObject.FindProperty("minimizeAfter");
            var defaultState = serializedObject.FindProperty("defaultState");
            var afterMinimize = serializedObject.FindProperty("afterMinimize");

            var onDestroy = serializedObject.FindProperty("onDestroy");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Content", 6);
            HexUIEditorHandler.DrawProperty(icon, customSkin, "Icon");
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.LabelField(new GUIContent("Notification Text"), customSkin.FindStyle("Text"), GUILayout.Width(-3));
            EditorGUILayout.PropertyField(notificationText, new GUIContent(""), GUILayout.Height(70));
            GUILayout.EndHorizontal();
            HexUIEditorHandler.DrawProperty(localizationKey, customSkin, "Localization Key");
            HexUIEditorHandler.DrawProperty(customSFX, customSkin, "Custom SFX");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 10);
            HexUIEditorHandler.DrawProperty(itemAnimator, customSkin, "Animator");
            HexUIEditorHandler.DrawProperty(iconObj, customSkin, "Icon Object");
            HexUIEditorHandler.DrawProperty(textObj, customSkin, "Text Object");

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