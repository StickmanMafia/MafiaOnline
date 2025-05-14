#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SidebarReveal))]
    public class SidebarRevealEditor : Editor
    {
        private SidebarReveal srTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            srTarget = (SidebarReveal)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = HexUIEditorHandler.GetDarkEditor(); }
            else { customSkin = HexUIEditorHandler.GetLightEditor(); }
        }

        public override void OnInspectorGUI()
        {
            var animator = serializedObject.FindProperty("animator");
            var canvasGroup = serializedObject.FindProperty("canvasGroup");

            var updateMode = serializedObject.FindProperty("updateMode");
            var barDirection = serializedObject.FindProperty("barDirection");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
            HexUIEditorHandler.DrawProperty(animator, customSkin, "Animator");
            HexUIEditorHandler.DrawProperty(canvasGroup, customSkin, "Canvas Group");

            HexUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 10);
            HexUIEditorHandler.DrawProperty(updateMode, customSkin, "Update Mode");
            HexUIEditorHandler.DrawProperty(barDirection, customSkin, "Bar Direction");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif