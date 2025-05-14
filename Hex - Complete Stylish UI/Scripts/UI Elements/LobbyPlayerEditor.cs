#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LobbyPlayer))]
    public class LobbyPlayerEditor : Editor
    {
        private LobbyPlayer lpTarget;
        private GUISkin customSkin;
        public bool showResources;

        private void OnEnable()
        {
            lpTarget = (LobbyPlayer)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var playerPicture = serializedObject.FindProperty("playerPicture");
            var playerName = serializedObject.FindProperty("playerName");
            var additionalText = serializedObject.FindProperty("additionalText");
            var currentState = serializedObject.FindProperty("currentState");

            var emptyParent = serializedObject.FindProperty("emptyParent");
            var readyParent = serializedObject.FindProperty("readyParent");
            var notReadyParent = serializedObject.FindProperty("notReadyParent");
            var playerIndicatorReady = serializedObject.FindProperty("playerIndicatorReady");
            var playerIndicatorNotReady = serializedObject.FindProperty("playerIndicatorNotReady");
            var pictureReadyImg = serializedObject.FindProperty("pictureReadyImg");
            var pictureNotReadyImg = serializedObject.FindProperty("pictureNotReadyImg");
            var nameReadyTMP = serializedObject.FindProperty("nameReadyTMP");
            var nameNotReadyTMP = serializedObject.FindProperty("nameNotReadyTMP");
            var adtReadyTMP = serializedObject.FindProperty("adtReadyTMP");
            var adtNotReadyTMP = serializedObject.FindProperty("adtNotReadyTMP");

            var onEmpty = serializedObject.FindProperty("onEmpty");
            var onReady = serializedObject.FindProperty("onReady");
            var onUnready = serializedObject.FindProperty("onUnready");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 6);
            HexUIEditorHandler.DrawProperty(playerPicture, customSkin, "Player Picture");
            HexUIEditorHandler.DrawProperty(playerName, customSkin, "Player Name");
            HexUIEditorHandler.DrawProperty(additionalText, customSkin, "Additional Text");
            HexUIEditorHandler.DrawProperty(currentState, customSkin, "Current State");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 10);
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            showResources = GUILayout.Toggle(showResources, new GUIContent("Show Resources", "Current state: " + showResources.ToString()), customSkin.FindStyle("Toggle"));
            showResources = GUILayout.Toggle(showResources, new GUIContent("", "Current state: " + showResources.ToString()), customSkin.FindStyle("ToggleHelper"));
            GUILayout.EndHorizontal();

            if (showResources == true)
            {
                HexUIEditorHandler.DrawProperty(emptyParent, customSkin, "Empty Parent");
                HexUIEditorHandler.DrawProperty(readyParent, customSkin, "Ready Parent");
                HexUIEditorHandler.DrawProperty(notReadyParent, customSkin, "Not Ready Parent");
                HexUIEditorHandler.DrawProperty(playerIndicatorReady, customSkin, "Indicator Ready");
                HexUIEditorHandler.DrawProperty(playerIndicatorNotReady, customSkin, "Indicator Not Ready");
                HexUIEditorHandler.DrawProperty(pictureReadyImg, customSkin, "Picture Ready");
                HexUIEditorHandler.DrawProperty(pictureNotReadyImg, customSkin, "Picture Not Ready");
                HexUIEditorHandler.DrawProperty(nameReadyTMP, customSkin, "Name Ready");
                HexUIEditorHandler.DrawProperty(nameNotReadyTMP, customSkin, "Name Not Ready");
                HexUIEditorHandler.DrawProperty(adtReadyTMP, customSkin, "Adt. Ready");
                HexUIEditorHandler.DrawProperty(adtNotReadyTMP, customSkin, "Adt. Not Ready");
            }

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Events", 10);
            EditorGUILayout.PropertyField(onEmpty, new GUIContent("On Empty"), true);
            EditorGUILayout.PropertyField(onReady, new GUIContent("On Ready"), true);
            EditorGUILayout.PropertyField(onUnready, new GUIContent("On Unready"), true);

            if (Application.isPlaying == false) { Repaint(); }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif