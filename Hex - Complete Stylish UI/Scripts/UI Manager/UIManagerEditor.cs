#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;

namespace Michsky.UI.Hex
{
    [CustomEditor(typeof(UIManager))]
    [System.Serializable]
    public class UIManagerEditor : Editor
    {
        GUISkin customSkin;
        private UIManager uimTarget;

        protected static float foldoutItemSpace = 2;
        protected static float foldoutTopSpace = 5;
        protected static float foldoutBottomSpace = 2;

        protected static bool showAudio = false;
        protected static bool showColors = false;
        protected static bool showFonts = false;
        protected static bool showLocalization = false;
        protected static bool showLogo = false;
        protected static bool showSplashScreen = false;

        private void OnEnable()
        {
            uimTarget = (UIManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            if (customSkin == null)
            {
                EditorGUILayout.HelpBox("Editor resources are missing. You can manually fix this by deleting " +
                    "Hex UI > Editor folder and then re-import the package. \n\nIf you're still seeing this " +
                    "dialog even after the re-import, contact me with this ID: " + UIManager.buildID, MessageType.Error);

                if (GUILayout.Button("Contact")) { Email(); }
                return;
            }

            // Foldout style
            GUIStyle foldoutStyle = customSkin.FindStyle("Foldout");

            // Header_UIM
            HexUIEditorHandler.DrawHeader(customSkin, "Header_UIM", 8);

            #region Audio
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showAudio = EditorGUILayout.Foldout(showAudio, "Audio", true, foldoutStyle);
            showAudio = GUILayout.Toggle(showAudio, new GUIContent(""), customSkin.FindStyle("ToggleHelper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showAudio)
            {
                var hoverSound = serializedObject.FindProperty("hoverSound");
                var clickSound = serializedObject.FindProperty("clickSound");
                var notificationSound = serializedObject.FindProperty("notificationSound");

                HexUIEditorHandler.DrawProperty(hoverSound, customSkin, "Hover Sound");
                HexUIEditorHandler.DrawProperty(clickSound, customSkin, "Click Sound");
                HexUIEditorHandler.DrawProperty(notificationSound, customSkin, "Notification Sound");
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            #endregion

            #region Colors
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showColors = EditorGUILayout.Foldout(showColors, "Colors", true, foldoutStyle);
            showColors = GUILayout.Toggle(showColors, new GUIContent(""), customSkin.FindStyle("ToggleHelper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showColors)
            {
                var accentColor = serializedObject.FindProperty("accentColor");
                var accentColorInvert = serializedObject.FindProperty("accentColorInvert");
                var primaryColor = serializedObject.FindProperty("primaryColor");
                var secondaryColor = serializedObject.FindProperty("secondaryColor");
                var negativeColor = serializedObject.FindProperty("negativeColor");
                var backgroundColor = serializedObject.FindProperty("backgroundColor");
                var altBackgroundColor = serializedObject.FindProperty("altBackgroundColor");

                HexUIEditorHandler.DrawProperty(accentColor, customSkin, "Accent Color");
                HexUIEditorHandler.DrawProperty(accentColorInvert, customSkin, "Accent Match");
                HexUIEditorHandler.DrawProperty(primaryColor, customSkin, "Primary Color");
                HexUIEditorHandler.DrawProperty(secondaryColor, customSkin, "Secondary Color");
                HexUIEditorHandler.DrawProperty(negativeColor, customSkin, "Negative Color");
                HexUIEditorHandler.DrawProperty(backgroundColor, customSkin, "Background Color");
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            #endregion

            #region Fonts
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showFonts = EditorGUILayout.Foldout(showFonts, "Fonts", true, foldoutStyle);
            showFonts = GUILayout.Toggle(showFonts, new GUIContent(""), customSkin.FindStyle("ToggleHelper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showFonts)
            {
                var fontLight = serializedObject.FindProperty("fontLight");
                var fontRegular = serializedObject.FindProperty("fontRegular");
                var fontMedium = serializedObject.FindProperty("fontMedium");
                var fontSemiBold = serializedObject.FindProperty("fontSemiBold");
                var fontBold = serializedObject.FindProperty("fontBold");
                var customFont = serializedObject.FindProperty("customFont");

                HexUIEditorHandler.DrawProperty(fontLight, customSkin, "Light Font");
                HexUIEditorHandler.DrawProperty(fontRegular, customSkin, "Regular Font");
                HexUIEditorHandler.DrawProperty(fontMedium, customSkin, "Medium Font");
                HexUIEditorHandler.DrawProperty(fontSemiBold, customSkin, "Semibold Font");
                HexUIEditorHandler.DrawProperty(fontBold, customSkin, "Bold Font");
                HexUIEditorHandler.DrawProperty(customFont, customSkin, "Custom Font");
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            #endregion

            #region Localization
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showLocalization = EditorGUILayout.Foldout(showLocalization, "Localization", true, foldoutStyle);
            showLocalization = GUILayout.Toggle(showLocalization, new GUIContent(""), customSkin.FindStyle("ToggleHelper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showLocalization)
            {
                var enableLocalization = serializedObject.FindProperty("enableLocalization");
                var localizationSettings = serializedObject.FindProperty("localizationSettings");

                enableLocalization.boolValue = HexUIEditorHandler.DrawToggle(enableLocalization.boolValue, customSkin, "Enable Localization (Beta)");

                if (enableLocalization.boolValue == true)
                {
                    HexUIEditorHandler.DrawPropertyCW(localizationSettings, customSkin, "Localization Settings", 130);
                    
                    if (uimTarget.localizationSettings != null)
                    {
                        if (GUILayout.Button("Open Localization Settings", customSkin.button)) { Selection.activeObject = uimTarget.localizationSettings; }
                        EditorGUILayout.HelpBox("Localization is enabled. You can use the localization settings asset to manage localization.", MessageType.Info);
                    }
                    else { EditorGUILayout.HelpBox("Localization is enabled, but 'Localization Settings' is missing.", MessageType.Warning); }
                }
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            #endregion

            #region Logo
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showLogo = EditorGUILayout.Foldout(showLogo, "Logo", true, foldoutStyle);
            showLogo = GUILayout.Toggle(showLogo, new GUIContent(""), customSkin.FindStyle("ToggleHelper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showLogo)
            {
                var gameLogo = serializedObject.FindProperty("gameLogo");
                var gameIcon = serializedObject.FindProperty("gameIcon");
                var brandLogo = serializedObject.FindProperty("brandLogo");

                HexUIEditorHandler.DrawProperty(gameIcon, customSkin, "Game Icon");
                HexUIEditorHandler.DrawProperty(gameLogo, customSkin, "Game Logo");
                HexUIEditorHandler.DrawProperty(brandLogo, customSkin, "Brand Logo");
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            #endregion

            #region Splash Screen
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showSplashScreen = EditorGUILayout.Foldout(showSplashScreen, "Splash Screen", true, foldoutStyle);
            showSplashScreen = GUILayout.Toggle(showSplashScreen, new GUIContent(""), customSkin.FindStyle("ToggleHelper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showSplashScreen)
            {
                var enableSplashScreen = serializedObject.FindProperty("enableSplashScreen");
                var showSplashScreenOnce = serializedObject.FindProperty("showSplashScreenOnce");
                var pakType = serializedObject.FindProperty("pakType");
                var pakText = serializedObject.FindProperty("pakText");
                var pakLocalizationText = serializedObject.FindProperty("pakLocalizationText");

                enableSplashScreen.boolValue = HexUIEditorHandler.DrawToggle(enableSplashScreen.boolValue, customSkin, "Enable Splash Screen");
                if (enableSplashScreen.boolValue == false) { GUI.enabled = false; }
                showSplashScreenOnce.boolValue = HexUIEditorHandler.DrawToggle(showSplashScreenOnce.boolValue, customSkin, "Show Only Once", "Only appears in the current session when enabled.");

                HexUIEditorHandler.DrawProperty(pakType, customSkin, "Press Any Key Type");

                if (pakType.enumValueIndex == 0)
                {
                    if (uimTarget.enableLocalization == true) 
                    {
                        HexUIEditorHandler.DrawProperty(pakLocalizationText, customSkin, "Press Any Key Text");
                        EditorGUILayout.HelpBox("Localization formatting: {StringKey}" + "\nDefault: PAK_Part1 {PAK_Key} PAK_Part3"
                            + "\nDefault output: Press [Any Key] To Start", MessageType.Info); 
                    }

                    else
                    {
                        HexUIEditorHandler.DrawProperty(pakText, customSkin, "Press Any Key Text");
                        EditorGUILayout.HelpBox("Formatting: {Key Text}" + "\nSample: Press {Any Key} To Start", MessageType.Info);
                    }
                }
            }

            GUI.enabled = true;
            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            #endregion

            #region Settings
            HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 14);

            var enableDynamicUpdate = serializedObject.FindProperty("enableDynamicUpdate");
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(-2);
            GUILayout.BeginHorizontal();
            enableDynamicUpdate.boolValue = GUILayout.Toggle(enableDynamicUpdate.boolValue, new GUIContent("Enable Dynamic Update"), customSkin.FindStyle("Toggle"));
            enableDynamicUpdate.boolValue = GUILayout.Toggle(enableDynamicUpdate.boolValue, new GUIContent(""), customSkin.FindStyle("ToggleHelper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(4);

            if (enableDynamicUpdate.boolValue == true)
            {
                EditorGUILayout.HelpBox("When this option is enabled, all objects connected to this manager will be dynamically updated synchronously. " +
                    "Basically; consumes more resources, but allows dynamic changes at runtime/editor.", MessageType.Info);
            }

            else
            {
                EditorGUILayout.HelpBox("When this option is disabled, all objects connected to this manager will be updated only once on awake. " +
                    "Basically; has better performance, but it's static.", MessageType.Info);
            }

            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset to defaults", customSkin.button)) { ResetToDefaults(); }
            GUILayout.EndHorizontal();
            #endregion

            #region Integrations
            HexUIEditorHandler.DrawHeader(customSkin, "Header_Integrations", 16);
            if (GUILayout.Button("Assembly Definition Patch", customSkin.button)) { Application.OpenURL("https://docs.michsky.com/docs/hex-ui/others/"); }
            #endregion

            #region Support
            HexUIEditorHandler.DrawHeader(customSkin, "Header_Support", 16);
            GUILayout.BeginVertical();
         
            GUILayout.BeginHorizontal();
            GUILayout.Label("Need help? Contact me via:", customSkin.FindStyle("Text"));
            GUILayout.EndHorizontal();
          
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Discord", customSkin.button)) { Discord(); }
            if (GUILayout.Button("Twitter", customSkin.button)) { Twitter(); }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Documentation", customSkin.button)) { Docs(); }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("E-mail", customSkin.button)) { Email(); }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("ID: " + UIManager.buildID);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
            #endregion

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }

        void Discord() { Application.OpenURL("https://discord.gg/VXpHyUt"); }
        void Docs() { Application.OpenURL("https://docs.michsky.com/docs/hex-ui/"); }
        void Email() { Application.OpenURL("https://www.michsky.com/contact/"); }
        void Twitter() { Application.OpenURL("https://www.twitter.com/michskyHQ"); }

        void ResetToDefaults()
        {
            if (EditorUtility.DisplayDialog("Reset to defaults", "Are you sure you want to reset UI Manager values to default?", "Yes", "Cancel"))
            {
                try
                {
                    Preset defaultPreset = Resources.Load<Preset>("Hex UIM Presets/Default");
                    defaultPreset.ApplyTo(Resources.Load("Hex UI Manager"));
                   
                    Selection.activeObject = null;
                    Selection.activeObject = Resources.Load("Hex UI Manager");
                   
                    Debug.Log("<b>[UI Manager]</b> Resetting successful.");
                }

                catch { Debug.LogWarning("<b>[UI Manager]</b> Resetting failed. Default preset seems to be missing."); }
            }
        }
    }
}
#endif