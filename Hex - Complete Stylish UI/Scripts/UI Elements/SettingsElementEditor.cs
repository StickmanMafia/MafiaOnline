#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SettingsElement))]
    public class SettingsElementEditor : Editor
    {
        private SettingsElement seTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            seTarget = (SettingsElement)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var highlightCG = serializedObject.FindProperty("highlightCG");

            var isInteractable = serializedObject.FindProperty("isInteractable");
            var useSounds = serializedObject.FindProperty("useSounds");
            var useUINavigation = serializedObject.FindProperty("useUINavigation");
            var navigationMode = serializedObject.FindProperty("navigationMode");
            var selectOnUp = serializedObject.FindProperty("selectOnUp");
            var selectOnDown = serializedObject.FindProperty("selectOnDown");
            var selectOnLeft = serializedObject.FindProperty("selectOnLeft");
            var selectOnRight = serializedObject.FindProperty("selectOnRight");
            var wrapAround = serializedObject.FindProperty("wrapAround");
            var fadingMultiplier = serializedObject.FindProperty("fadingMultiplier");

            var onClick = serializedObject.FindProperty("onClick");
            var onHover = serializedObject.FindProperty("onHover");
            var onLeave = serializedObject.FindProperty("onLeave");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
            HexUIEditorHandler.DrawProperty(highlightCG, customSkin, "Highlight CG");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 10);
            isInteractable.boolValue = HexUIEditorHandler.DrawToggle(isInteractable.boolValue, customSkin, "Is Interactable");
            useSounds.boolValue = HexUIEditorHandler.DrawToggle(useSounds.boolValue, customSkin, "Use Sounds");

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(-3);

            useUINavigation.boolValue = HexUIEditorHandler.DrawTogglePlain(useUINavigation.boolValue, customSkin, "Use UI Navigation", "Enables controller navigation.");

            GUILayout.Space(4);

            if (useUINavigation.boolValue == true)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                HexUIEditorHandler.DrawPropertyPlain(navigationMode, customSkin, "Navigation Mode");

                if (seTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Horizontal)
                {
                    EditorGUI.indentLevel = 1;
                    wrapAround.boolValue = HexUIEditorHandler.DrawToggle(wrapAround.boolValue, customSkin, "Wrap Around");
                    EditorGUI.indentLevel = 0;
                }

                else if (seTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Vertical)
                {
                    wrapAround.boolValue = HexUIEditorHandler.DrawTogglePlain(wrapAround.boolValue, customSkin, "Wrap Around");
                }

                else if (seTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Explicit)
                {
                    EditorGUI.indentLevel = 1;
                    HexUIEditorHandler.DrawPropertyPlain(selectOnUp, customSkin, "Select On Up");
                    HexUIEditorHandler.DrawPropertyPlain(selectOnDown, customSkin, "Select On Down");
                    HexUIEditorHandler.DrawPropertyPlain(selectOnLeft, customSkin, "Select On Left");
                    HexUIEditorHandler.DrawPropertyPlain(selectOnRight, customSkin, "Select On Right");
                    EditorGUI.indentLevel = 0;
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndVertical();
            HexUIEditorHandler.DrawProperty(fadingMultiplier, customSkin, "Fading Multiplier", "Set the animation fade multiplier.");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Events", 10);
            EditorGUILayout.PropertyField(onClick, new GUIContent("On Click"), true);
            EditorGUILayout.PropertyField(onHover, new GUIContent("On Hover"), true);
            EditorGUILayout.PropertyField(onLeave, new GUIContent("On Leave"), true);

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif