﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    public class HexUIEditorHandler : Editor
    {
        public static GUISkin GetDarkEditor()
        {
            GUISkin tempSkin = (GUISkin)Resources.Load("HexUIEditor-Dark");
            return tempSkin;
        }

        public static GUISkin GetLightEditor()
        {
            GUISkin tempSkin = (GUISkin)Resources.Load("HexUIEditor-Light");
            return tempSkin;
        }

        public static void DrawProperty(SerializedProperty property, GUISkin skin, string content)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent(content), skin.FindStyle("Text"), GUILayout.Width(120));
            EditorGUILayout.PropertyField(property, new GUIContent(""));

            GUILayout.EndHorizontal();
        }

        public static void DrawProperty(SerializedProperty property, GUISkin skin, string content, string tooltip)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent(content, tooltip), skin.FindStyle("Text"), GUILayout.Width(120));
            EditorGUILayout.PropertyField(property, new GUIContent("", tooltip));

            GUILayout.EndHorizontal();
        }

        public static void DrawPropertyPlain(SerializedProperty property, GUISkin skin, string content)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent(content), skin.FindStyle("Text"), GUILayout.Width(120));
            EditorGUILayout.PropertyField(property, new GUIContent(""));

            GUILayout.EndHorizontal();
        }

        public static void DrawPropertyPlain(SerializedProperty property, GUISkin skin, string content, string tooltip)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent(content, tooltip), skin.FindStyle("Text"), GUILayout.Width(120));
            EditorGUILayout.PropertyField(property, new GUIContent("", tooltip));

            GUILayout.EndHorizontal();
        }

        public static void DrawPropertyCW(SerializedProperty property, GUISkin skin, string content, float width)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent(content), skin.FindStyle("Text"), GUILayout.Width(width));
            EditorGUILayout.PropertyField(property, new GUIContent(""));

            GUILayout.EndHorizontal();
        }

        public static void DrawPropertyCW(SerializedProperty property, GUISkin skin, string content, string tooltip, float width)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent(content, tooltip), skin.FindStyle("Text"), GUILayout.Width(width));
            EditorGUILayout.PropertyField(property, new GUIContent("", tooltip));

            GUILayout.EndHorizontal();
        }

        public static void DrawPropertyPlainCW(SerializedProperty property, GUISkin skin, string content, float width)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent(content), skin.FindStyle("Text"), GUILayout.Width(width));
            EditorGUILayout.PropertyField(property, new GUIContent(""));

            GUILayout.EndHorizontal();
        }

        public static int DrawTabs(int tabIndex, GUIContent[] tabs, GUISkin skin)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            tabIndex = GUILayout.Toolbar(tabIndex, tabs, skin.FindStyle("Tab_Indicator"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-40);
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            return tabIndex;
        }

        public static void DrawComponentHeader(GUISkin skin, string content)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box(new GUIContent(""), skin.FindStyle(content));
            GUILayout.EndHorizontal();
            GUILayout.Space(-42);
        }

        public static void DrawHeader(GUISkin skin, string content, int space)
        {
            GUILayout.Space(space);
            GUILayout.Box(new GUIContent(""), skin.FindStyle(content));
        }

        public static bool DrawToggle(bool value, GUISkin skin, string content)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            value = GUILayout.Toggle(value, new GUIContent(content, "Current state: " + value.ToString()), skin.FindStyle("Toggle"));
            value = GUILayout.Toggle(value, new GUIContent("", "Current state: " + value.ToString()), skin.FindStyle("ToggleHelper"));

            GUILayout.EndHorizontal();
            return value;
        }

        public static bool DrawToggle(bool value, GUISkin skin, string content, string tooltip)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            value = GUILayout.Toggle(value, new GUIContent(content, tooltip), skin.FindStyle("Toggle"));
            value = GUILayout.Toggle(value, new GUIContent("", tooltip), skin.FindStyle("ToggleHelper"));

            GUILayout.EndHorizontal();
            return value;
        }

        public static bool DrawTogglePlain(bool value, GUISkin skin, string content)
        {
            GUILayout.BeginHorizontal();

            value = GUILayout.Toggle(value, new GUIContent(content, "Current state: " + value.ToString()), skin.FindStyle("Toggle"));
            value = GUILayout.Toggle(value, new GUIContent("", "Current state: " + value.ToString()), skin.FindStyle("ToggleHelper"));

            GUILayout.EndHorizontal();
            return value;
        }

        public static bool DrawTogglePlain(bool value, GUISkin skin, string content, string tooltip)
        {
            GUILayout.BeginHorizontal();

            value = GUILayout.Toggle(value, new GUIContent(content, tooltip), skin.FindStyle("Toggle"));
            value = GUILayout.Toggle(value, new GUIContent("", tooltip), skin.FindStyle("ToggleHelper"));

            GUILayout.EndHorizontal();
            return value;
        }

        public static Texture2D TextureFromSprite(Sprite sprite)
        {
            if (sprite == null) { return null; }

            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                             (int)sprite.textureRect.y,
                                                             (int)sprite.textureRect.width,
                                                             (int)sprite.textureRect.height);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }

            else { return sprite.texture; }
        }
    }
}
#endif