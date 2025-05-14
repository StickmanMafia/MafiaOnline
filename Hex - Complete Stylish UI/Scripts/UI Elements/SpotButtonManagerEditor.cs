#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SpotButtonManager))]
    public class SpotButtonManagerEditor : Editor
    {
        private SpotButtonManager buttonTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            buttonTarget = (SpotButtonManager)target;

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

            buttonTarget.latestTabIndex = HexUIEditorHandler.DrawTabs(buttonTarget.latestTabIndex, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab_Content")))
                buttonTarget.latestTabIndex = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab_Resources")))
                buttonTarget.latestTabIndex = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab_Settings")))
                buttonTarget.latestTabIndex = 2;

            GUILayout.EndHorizontal();

            var animator = serializedObject.FindProperty("animator");
            var backgroundObj = serializedObject.FindProperty("backgroundObj");
            var titleObj = serializedObject.FindProperty("titleObj");
            var descriptionObj = serializedObject.FindProperty("descriptionObj");
            var actionObj = serializedObject.FindProperty("actionObj");

            var buttonBackground = serializedObject.FindProperty("buttonBackground");
            var buttonTitle = serializedObject.FindProperty("buttonTitle");
            var titleLocalizationKey = serializedObject.FindProperty("titleLocalizationKey");
            var buttonDescription = serializedObject.FindProperty("buttonDescription");
            var descriptionLocalizationKey = serializedObject.FindProperty("descriptionLocalizationKey");
            var actionText = serializedObject.FindProperty("actionText");
            var actionLocalizationKey = serializedObject.FindProperty("actionLocalizationKey");

            var isInteractable = serializedObject.FindProperty("isInteractable");
            var useUINavigation = serializedObject.FindProperty("useUINavigation");
            var navigationMode = serializedObject.FindProperty("navigationMode");
            var wrapAround = serializedObject.FindProperty("wrapAround");
            var selectOnUp = serializedObject.FindProperty("selectOnUp");
            var selectOnDown = serializedObject.FindProperty("selectOnDown");
            var selectOnLeft = serializedObject.FindProperty("selectOnLeft");
            var selectOnRight = serializedObject.FindProperty("selectOnRight");
            var checkForDoubleClick = serializedObject.FindProperty("checkForDoubleClick");
            var useLocalization = serializedObject.FindProperty("useLocalization");
            var useSounds = serializedObject.FindProperty("useSounds");
            var doubleClickPeriod = serializedObject.FindProperty("doubleClickPeriod");
            var useCustomContent = serializedObject.FindProperty("useCustomContent");

            var onClick = serializedObject.FindProperty("onClick");
            var onDoubleClick = serializedObject.FindProperty("onDoubleClick");
            var onHover = serializedObject.FindProperty("onHover");
            var onLeave = serializedObject.FindProperty("onLeave");

            switch (buttonTarget.latestTabIndex)
            {
                case 0:
                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Content", 6);

                    if (useCustomContent.boolValue == false)
                    {
                        if (buttonTarget.backgroundObj != null)
                        {
                            HexUIEditorHandler.DrawPropertyCW(buttonBackground, customSkin, "Background", 110);
                        }

                        if (buttonTarget.titleObj != null)
                        {
                            GUILayout.Space(10);
                            HexUIEditorHandler.DrawPropertyCW(buttonTitle, customSkin, "Button Title", 110);
                            if (useLocalization.boolValue == true) { HexUIEditorHandler.DrawPropertyCW(titleLocalizationKey, customSkin, "Localization Key", 110); }
                        }

                        if (buttonTarget.descriptionObj != null)
                        {
                            GUILayout.Space(10);
                            GUILayout.BeginHorizontal(EditorStyles.helpBox);
                            EditorGUILayout.LabelField(new GUIContent("Button Description"), customSkin.FindStyle("Text"), GUILayout.Width(-3));
                            EditorGUILayout.PropertyField(buttonDescription, new GUIContent(""));
                            GUILayout.EndHorizontal();
                            if (useLocalization.boolValue == true) { HexUIEditorHandler.DrawPropertyCW(descriptionLocalizationKey, customSkin, "Localization Key", 110); }
                        }

                        if (buttonTarget.actionObj != null)
                        {
                            GUILayout.Space(10);
                            HexUIEditorHandler.DrawPropertyCW(actionText, customSkin, "Action Text", 110);
                            if (useLocalization.boolValue == true) { HexUIEditorHandler.DrawPropertyCW(actionLocalizationKey, customSkin, "Localization Key", 110); }
                        }

                        if (Application.isPlaying == false) { buttonTarget.UpdateUI(); }
                    }

                    else { EditorGUILayout.HelpBox("'Use Custom Content' is enabled. Content is now managed manually.", MessageType.Info); }


                    if (Application.isPlaying == true && GUILayout.Button("Update UI", customSkin.button)) 
                    { 
                        buttonTarget.UpdateUI(); 
                    }

                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Events", 10);
                    EditorGUILayout.PropertyField(onClick, new GUIContent("On Click"), true);
                    EditorGUILayout.PropertyField(onDoubleClick, new GUIContent("On Double Click"), true);
                    EditorGUILayout.PropertyField(onHover, new GUIContent("On Hover"), true);
                    EditorGUILayout.PropertyField(onLeave, new GUIContent("On Leave"), true);
                    break;

                case 1:
                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
                    HexUIEditorHandler.DrawProperty(backgroundObj, customSkin, "Background Object");
                    HexUIEditorHandler.DrawProperty(titleObj, customSkin, "Title Object");
                    HexUIEditorHandler.DrawProperty(descriptionObj, customSkin, "Description Object");
                    HexUIEditorHandler.DrawProperty(actionObj, customSkin, "Action Object");
                    HexUIEditorHandler.DrawProperty(animator, customSkin, "Animator");
                    break;

                case 2:
                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 6);
                    HexUIEditorHandler.DrawProperty(doubleClickPeriod, customSkin, "Double Click Period");
                    isInteractable.boolValue = HexUIEditorHandler.DrawToggle(isInteractable.boolValue, customSkin, "Is Interactable");
                    useCustomContent.boolValue = HexUIEditorHandler.DrawToggle(useCustomContent.boolValue, customSkin, "Use Custom Content", "Bypasses inspector values and allows manual editing.");
                    useLocalization.boolValue = HexUIEditorHandler.DrawToggle(useLocalization.boolValue, customSkin, "Use Localization", "Bypasses localization functions when disabled.");
                    GUI.enabled = true;
                    checkForDoubleClick.boolValue = HexUIEditorHandler.DrawToggle(checkForDoubleClick.boolValue, customSkin, "Check For Double Click");
                    useSounds.boolValue = HexUIEditorHandler.DrawToggle(useSounds.boolValue, customSkin, "Use Button Sounds");
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);

                    useUINavigation.boolValue = HexUIEditorHandler.DrawTogglePlain(useUINavigation.boolValue, customSkin, "Use UI Navigation", "Enables controller navigation.");

                    GUILayout.Space(4);

                    if (useUINavigation.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        HexUIEditorHandler.DrawPropertyPlain(navigationMode, customSkin, "Navigation Mode");

                        if (buttonTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Horizontal)
                        {
                            EditorGUI.indentLevel = 1;
                            wrapAround.boolValue = HexUIEditorHandler.DrawToggle(wrapAround.boolValue, customSkin, "Wrap Around");
                            EditorGUI.indentLevel = 0;
                        }

                        else if (buttonTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Vertical)
                        {
                            wrapAround.boolValue = HexUIEditorHandler.DrawTogglePlain(wrapAround.boolValue, customSkin, "Wrap Around");
                        }

                        else if (buttonTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Explicit)
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
                    buttonTarget.UpdateUI();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif