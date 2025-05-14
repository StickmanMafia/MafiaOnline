#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Michsky.UI.Hex
{
    [CustomEditor(typeof(ModalWindowManager))]
    public class ModalWindowManagerEditor : Editor
    {
        private GUISkin customSkin;
        private ModalWindowManager mwTarget;
        private int currentTab;

        private void OnEnable()
        {
            mwTarget = (ModalWindowManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            HexUIEditorHandler.DrawComponentHeader(customSkin, "TopHeader_ModalWIndow");

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

            var windowIcon = serializedObject.FindProperty("windowIcon");
            var windowTitle = serializedObject.FindProperty("windowTitle");
            var windowDescription = serializedObject.FindProperty("windowDescription");

            var titleKey = serializedObject.FindProperty("titleKey");
            var descriptionKey = serializedObject.FindProperty("descriptionKey");

            var onConfirm = serializedObject.FindProperty("onConfirm");
            var onCancel = serializedObject.FindProperty("onCancel");
            var onOpen = serializedObject.FindProperty("onOpen");
            var onClose = serializedObject.FindProperty("onClose");

            var icon = serializedObject.FindProperty("icon");
            var titleText = serializedObject.FindProperty("titleText");
            var descriptionText = serializedObject.FindProperty("descriptionText");
            var confirmButton = serializedObject.FindProperty("confirmButton");
            var cancelButton = serializedObject.FindProperty("cancelButton");
            var mwAnimator = serializedObject.FindProperty("mwAnimator");

            var animationSpeed = serializedObject.FindProperty("animationSpeed");
            var closeBehaviour = serializedObject.FindProperty("closeBehaviour");
            var startBehaviour = serializedObject.FindProperty("startBehaviour");
            var useCustomContent = serializedObject.FindProperty("useCustomContent");
            var closeOnCancel = serializedObject.FindProperty("closeOnCancel");
            var closeOnConfirm = serializedObject.FindProperty("closeOnConfirm");
            var showCancelButton = serializedObject.FindProperty("showCancelButton");
            var showConfirmButton = serializedObject.FindProperty("showConfirmButton");
            var useLocalization = serializedObject.FindProperty("useLocalization");
            var liftNavigationRestrictions = serializedObject.FindProperty("liftNavigationRestrictions");

            switch (currentTab)
            {
                case 0:
                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Content", 6);

                    if (useCustomContent.boolValue == false)
                    {
                        if (mwTarget.windowIcon != null) 
                        {
                            HexUIEditorHandler.DrawProperty(icon, customSkin, "Icon");
                            if (Application.isPlaying == false) { mwTarget.windowIcon.sprite = mwTarget.icon; }
                        }

                        if (mwTarget.windowTitle != null) 
                        {
                            HexUIEditorHandler.DrawProperty(titleText, customSkin, "Title");
                            if (Application.isPlaying == false) { mwTarget.windowTitle.text = titleText.stringValue; }
                        }

                        if (mwTarget.windowDescription != null) 
                        {
                            GUILayout.BeginHorizontal(EditorStyles.helpBox);
                            EditorGUILayout.LabelField(new GUIContent("Description"), customSkin.FindStyle("Text"), GUILayout.Width(-3));
                            EditorGUILayout.PropertyField(descriptionText, new GUIContent(""));
                            GUILayout.EndHorizontal();
                            if (Application.isPlaying == false) { mwTarget.windowDescription.text = descriptionText.stringValue; }
                        }
                    }

                    else { EditorGUILayout.HelpBox("'Use Custom Content' is enabled.", MessageType.Info); }

                    GUILayout.BeginHorizontal();
                    if (mwTarget.showConfirmButton == true && mwTarget.confirmButton != null && GUILayout.Button("Edit Confirm Button", customSkin.button)) { Selection.activeObject = mwTarget.confirmButton; }
                    if (mwTarget.showCancelButton == true && mwTarget.cancelButton != null && GUILayout.Button("Edit Cancel Button", customSkin.button)) { Selection.activeObject = mwTarget.cancelButton; }
                    GUILayout.EndHorizontal();

                    if (Application.isPlaying == false)
                    {
                        if (mwTarget.GetComponent<CanvasGroup>().alpha == 0 && GUILayout.Button("Set Visible", customSkin.button))
                        {
                            mwTarget.GetComponent<CanvasGroup>().alpha = 1;
                            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        }

                        else if (mwTarget.GetComponent<CanvasGroup>().alpha == 1 && GUILayout.Button("Set Invisible", customSkin.button))
                        {
                            mwTarget.GetComponent<CanvasGroup>().alpha = 0;
                            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        }
                    }

                    if (mwTarget.useCustomContent == false && mwTarget.useLocalization == true)
                    {
                        HexUIEditorHandler.DrawHeader(customSkin, "Header_Languages", 10);
                        HexUIEditorHandler.DrawProperty(titleKey, customSkin, "Title Key", "Used for localization.");
                        HexUIEditorHandler.DrawProperty(descriptionKey, customSkin, "Description Key", "Used for localization.");
                    }

                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Events", 10);
                    EditorGUILayout.PropertyField(onConfirm, new GUIContent("On Confirm"), true);
                    EditorGUILayout.PropertyField(onCancel, new GUIContent("On Cancel"), true);
                    EditorGUILayout.PropertyField(onOpen, new GUIContent("On Open"), true);
                    EditorGUILayout.PropertyField(onClose, new GUIContent("On Close"), true);
                    break;

                case 1:
                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
                    HexUIEditorHandler.DrawProperty(windowIcon, customSkin, "Icon Object");
                    HexUIEditorHandler.DrawProperty(windowTitle, customSkin, "Title Object");
                    HexUIEditorHandler.DrawProperty(windowDescription, customSkin, "Description Object");
                    HexUIEditorHandler.DrawProperty(confirmButton, customSkin, "Confirm Button");
                    HexUIEditorHandler.DrawProperty(cancelButton, customSkin, "Cancel Button");
                    HexUIEditorHandler.DrawProperty(mwAnimator, customSkin, "Animator");
                    break;

                case 2:
                    HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 6);
                    HexUIEditorHandler.DrawProperty(animationSpeed, customSkin, "Animation Speed");
                    HexUIEditorHandler.DrawProperty(startBehaviour, customSkin, "Start Behaviour");
                    HexUIEditorHandler.DrawProperty(closeBehaviour, customSkin, "Close Behaviour");
                    useCustomContent.boolValue = HexUIEditorHandler.DrawToggle(useCustomContent.boolValue, customSkin, "Use Custom Content", "Bypasses inspector values and allows manual editing.");
                    closeOnCancel.boolValue = HexUIEditorHandler.DrawToggle(closeOnCancel.boolValue, customSkin, "Close Window On Cancel");
                    closeOnConfirm.boolValue = HexUIEditorHandler.DrawToggle(closeOnConfirm.boolValue, customSkin, "Close Window On Confirm");
                    showCancelButton.boolValue = HexUIEditorHandler.DrawToggle(showCancelButton.boolValue, customSkin, "Show Cancel Button");
                    showConfirmButton.boolValue = HexUIEditorHandler.DrawToggle(showConfirmButton.boolValue, customSkin, "Show Confirm Button");
                    useLocalization.boolValue = HexUIEditorHandler.DrawToggle(useLocalization.boolValue, customSkin, "Use Localization", "Bypasses localization functions when disabled.");
                    liftNavigationRestrictions.boolValue = HexUIEditorHandler.DrawToggle(liftNavigationRestrictions.boolValue, customSkin, "Lift Navigation Restrictions", "Limits UGUI navigation to cancel and confirm buttons for a smooth experience when disabled.");
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif