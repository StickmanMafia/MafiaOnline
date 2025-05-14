#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MenuManager))]
    public class MenuManagerEditor : Editor
    {
        private MenuManager mmTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            mmTarget = (MenuManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var UIManagerAsset = serializedObject.FindProperty("UIManagerAsset");
            var splashScreen = serializedObject.FindProperty("splashScreen");
            var mainContent = serializedObject.FindProperty("mainContent");
            var initPanel = serializedObject.FindProperty("initPanel");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
            HexUIEditorHandler.DrawProperty(UIManagerAsset, customSkin, "UI Manager");
            HexUIEditorHandler.DrawProperty(splashScreen, customSkin, "Splash Screen");
            HexUIEditorHandler.DrawProperty(mainContent, customSkin, "Main Content");
            HexUIEditorHandler.DrawProperty(initPanel, customSkin, "Init Screen");

            if (mmTarget.UIManagerAsset != null)
            {
                HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 10);
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                mmTarget.UIManagerAsset.enableSplashScreen = GUILayout.Toggle(mmTarget.UIManagerAsset.enableSplashScreen, "Enable Splash Screen", customSkin.FindStyle("Toggle"));
                mmTarget.UIManagerAsset.enableSplashScreen = GUILayout.Toggle(mmTarget.UIManagerAsset.enableSplashScreen, new GUIContent(""), customSkin.FindStyle("ToggleHelper"));
                GUILayout.EndHorizontal();

                if (mmTarget.splashScreen != null)
                {
                    GUILayout.BeginHorizontal();

                    if (Application.isPlaying == false)
                    {
                        if (mmTarget.splashScreen.gameObject.activeSelf == false && GUILayout.Button("Show Splash Screen", customSkin.button))
                        {
                            mmTarget.splashScreen.gameObject.SetActive(true);
                            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        }

                        else if (mmTarget.splashScreen.gameObject.activeSelf == true && GUILayout.Button("Hide Splash Screen", customSkin.button))
                        {
                            mmTarget.splashScreen.gameObject.SetActive(false);
                            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        }
                    }

                    if (GUILayout.Button("Select Splash Screen", customSkin.button)) { Selection.activeObject = mmTarget.splashScreen; }
                    GUILayout.EndHorizontal();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif