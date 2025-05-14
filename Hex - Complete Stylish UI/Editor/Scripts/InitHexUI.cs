#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Hex
{
    public class InitHexUI
    {
        [InitializeOnLoad]
        public class InitOnLoad
        {
            static InitOnLoad()
            {
                if (!EditorPrefs.HasKey("HexUI.HasCustomEditorData"))
                {
                    string darkPath = AssetDatabase.GetAssetPath(Resources.Load("HexUIEditor-Dark"));
                    string lightPath = AssetDatabase.GetAssetPath(Resources.Load("HexUIEditor-Light"));

                    EditorPrefs.SetString("HexUI.CustomEditorDark", darkPath);
                    EditorPrefs.SetString("HexUI.CustomEditorLight", lightPath);
                    EditorPrefs.SetInt("HexUI.HasCustomEditorData", 1);
                }
            }
        }
    }
}
#endif