#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UIManagerAudio))]
    public class UIManagerAudioEditor : Editor
    {
        private UIManagerAudio uimaTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            uimaTarget = (UIManagerAudio)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var UIManagerAsset = serializedObject.FindProperty("UIManagerAsset");
            var audioMixer = serializedObject.FindProperty("audioMixer");
            var audioSource = serializedObject.FindProperty("audioSource");
            var masterSlider = serializedObject.FindProperty("masterSlider");
            var musicSlider = serializedObject.FindProperty("musicSlider");
            var SFXSlider = serializedObject.FindProperty("SFXSlider");
            var UISlider = serializedObject.FindProperty("UISlider");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
            HexUIEditorHandler.DrawProperty(UIManagerAsset, customSkin, "UI Manager");
            HexUIEditorHandler.DrawProperty(audioMixer, customSkin, "Audio Mixer");
            HexUIEditorHandler.DrawProperty(audioSource, customSkin, "Audio Source");
            HexUIEditorHandler.DrawProperty(masterSlider, customSkin, "Master Slider");
            HexUIEditorHandler.DrawProperty(musicSlider, customSkin, "Music Slider");
            HexUIEditorHandler.DrawProperty(SFXSlider, customSkin, "SFX Slider");
            HexUIEditorHandler.DrawProperty(UISlider, customSkin, "UI Slider");

            if (Application.isPlaying == true)
                return;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif