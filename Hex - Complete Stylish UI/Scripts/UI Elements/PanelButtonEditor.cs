#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PanelButton))]
    public class PanelButtonEditor : Editor
    {
        private PanelButton buttonTarget;
        private GUISkin customSkin;
        private int currentTab;

        private void OnEnable()
        {
            buttonTarget = (PanelButton)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            HexUIEditorHandler.DrawComponentHeader(customSkin, "TopHeader_Button");

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            currentTab = HexUIEditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab_Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab_Resources")))
                currentTab = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab_Settings")))
                currentTab = 2;

            GUILayout.EndHorizontal();

            var buttonIcon = serializedObject.FindProperty("buttonIcon");
            var selectedIcon = serializedObject.FindProperty("selectedIcon");
            var buttonText = serializedObject.FindProperty("buttonText");

            var disabledCG = serializedObject.FindProperty("disabledCG");
            var normalCG = serializedObject.FindProperty("normalCG");
            var highlightCG = serializedObject.FindProperty("highlightCG");
            var selectCG = serializedObject.FindProperty("selectCG");
            var disabledTextObj = serializedObject.FindProperty("disabledTextObj");
            var normalTextObj = serializedObject.FindProperty("normalTextObj");
            var highlightTextObj = serializedObject.FindProperty("highlightTextObj");
            var selectTextObj = serializedObject.FindProperty("selectTextObj");
            var disabledImageObj = serializedObject.FindProperty("disabledImageObj");
            var normalImageObj = serializedObject.FindProperty("normalImageObj");
            var highlightImageObj = serializedObject.FindProperty("highlightImageObj");
            var selectedImageObj = serializedObject.FindProperty("selectedImageObj");
            var seperator = serializedObject.FindProperty("seperator");

            var isInteractable = serializedObject.FindProperty("isInteractable");
            var isSelected = serializedObject.FindProperty("isSelected");
            var useLocalization = serializedObject.FindProperty("useLocalization");
            var useCustomText = serializedObject.FindProperty("useCustomText");
            var useSeperator = serializedObject.FindProperty("useSeperator");
            var useSounds = serializedObject.FindProperty("useSounds");
            var useUINavigation = serializedObject.FindProperty("useUINavigation");
            var fadingMultiplier = serializedObject.FindProperty("fadingMultiplier");

            var onClick = serializedObject.FindProperty("onClick");
            var onHover = serializedObject.FindProperty("onHover");
            var onLeave = serializedObject.FindProperty("onLeave");
            var onSelect = serializedObject.FindProperty("onSelect");

            switch (currentTab)
            {
                case 0:
                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Content", 6);
                    HexUIEditorHandler.DrawPropertyCW(buttonIcon, customSkin, "Button Icon", 86);
                    if (buttonTarget.buttonIcon != null) { HexUIEditorHandler.DrawPropertyCW(selectedIcon, customSkin, "Selected Icon", 86); }
                    if (useCustomText.boolValue == false) { HexUIEditorHandler.DrawPropertyCW(buttonText, customSkin, "Button Text", 86); }
                    if (buttonTarget.buttonIcon != null || useCustomText.boolValue == false) { buttonTarget.UpdateUI(); }

                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Events", 10);
                    EditorGUILayout.PropertyField(onClick, new GUIContent("On Click"), true);
                    EditorGUILayout.PropertyField(onHover, new GUIContent("On Hover"), true);
                    EditorGUILayout.PropertyField(onLeave, new GUIContent("On Leave"), true);
                    EditorGUILayout.PropertyField(onSelect, new GUIContent("On Select"), true);
                    break;

                case 1:
                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
                    HexUIEditorHandler.DrawProperty(disabledCG, customSkin, "Disabled CG");
                    HexUIEditorHandler.DrawProperty(normalCG, customSkin, "Normal CG");
                    HexUIEditorHandler.DrawProperty(highlightCG, customSkin, "Highlight CG");
                    HexUIEditorHandler.DrawProperty(selectCG, customSkin, "Select CG");
                    HexUIEditorHandler.DrawProperty(disabledTextObj, customSkin, "Disabled Text");
                    HexUIEditorHandler.DrawProperty(normalTextObj, customSkin, "Normal Text");
                    HexUIEditorHandler.DrawProperty(highlightTextObj, customSkin, "Highlight Text");
                    HexUIEditorHandler.DrawProperty(selectTextObj, customSkin, "Select Text");
                    HexUIEditorHandler.DrawProperty(disabledImageObj, customSkin, "Disabled Image");
                    HexUIEditorHandler.DrawProperty(normalImageObj, customSkin, "Normal Image");
                    HexUIEditorHandler.DrawProperty(highlightImageObj, customSkin, "Highlight Image");
                    HexUIEditorHandler.DrawProperty(selectedImageObj, customSkin, "Select Image");
                    HexUIEditorHandler.DrawProperty(seperator, customSkin, "Seperator");
                    break;

                case 2:
                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 6);
                    isInteractable.boolValue = HexUIEditorHandler.DrawToggle(isInteractable.boolValue, customSkin, "Is Interactable");
                    isSelected.boolValue = HexUIEditorHandler.DrawToggle(isSelected.boolValue, customSkin, "Is Selected");
                    useLocalization.boolValue = HexUIEditorHandler.DrawToggle(useLocalization.boolValue, customSkin, "Use Localization", "Bypasses localization functions when disabled.");
                    useCustomText.boolValue = HexUIEditorHandler.DrawToggle(useCustomText.boolValue, customSkin, "Use Custom Text", "Bypasses inspector values and allows manual editing.");
                    useSeperator.boolValue = HexUIEditorHandler.DrawToggle(useSeperator.boolValue, customSkin, "Use Seperator");
                    useUINavigation.boolValue = HexUIEditorHandler.DrawToggle(useUINavigation.boolValue, customSkin, "Use UI Navigation", "Enables controller navigation.");
                    useSounds.boolValue = HexUIEditorHandler.DrawToggle(useSounds.boolValue, customSkin, "Use Button Sounds");
                    HexUIEditorHandler.DrawProperty(fadingMultiplier, customSkin, "Fading Multiplier", "Set the animation fade multiplier.");
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif