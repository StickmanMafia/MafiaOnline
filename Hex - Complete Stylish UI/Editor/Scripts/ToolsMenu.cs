#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Michsky.UI.Hex
{
    public class ToolsMenu : Editor
    {
        static string objectPath;

        static void GetObjectPath()
        {
            objectPath = AssetDatabase.GetAssetPath(Resources.Load("Hex UI Manager"));
            objectPath = objectPath.Replace("Resources/Hex UI Manager.asset", "").Trim();
            objectPath = objectPath + "Prefabs/";
        }

        static void MakeSceneDirty(GameObject source, string sourceName)
        {
            if (Application.isPlaying == false)
            {
                Undo.RegisterCreatedObjectUndo(source, sourceName);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

        static void ShowErrorDialog()
        {
            EditorUtility.DisplayDialog("Hex UI", "Cannot create the object due to missing manager file. " +
                    "Make sure you have 'Hex UI Manager' file in Hex UI > Resources folder.", "Dismiss");
        }

        static void UpdateCustomEditorPath()
        {
            string darkPath = AssetDatabase.GetAssetPath(Resources.Load("HexUIEditor-Dark"));
            string lightPath = AssetDatabase.GetAssetPath(Resources.Load("HexUIEditor-Light"));

            EditorPrefs.SetString("HexUI.CustomEditorDark", darkPath);
            EditorPrefs.SetString("HexUI.CustomEditorLight", lightPath);
        }

        [MenuItem("Tools/Hex UI/Show UI Manager %#M")]
        static void ShowManager()
        {
            Selection.activeObject = Resources.Load("Hex UI Manager");

            if (Selection.activeObject == null)
                Debug.Log("<b>[Hex UI]</b>Can't find an asset called 'Hex UI Manager'. Make sure you have 'Hex UI Manager' in: Hex UI > Editor > Resources");
        }

        static void CreateObject(string resourcePath)
        {
            try
            {
                GetObjectPath();
                UpdateCustomEditorPath();
                GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath(objectPath + resourcePath + ".prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;

                try
                {
                    if (Selection.activeGameObject == null)
                    {

#if UNITY_2023_2_OR_NEWER
                        var canvas = FindObjectsByType<Canvas>(FindObjectsSortMode.None)[0];
#else
                        var canvas = (Canvas)FindObjectsOfType(typeof(Canvas))[0];
#endif
                        clone.transform.SetParent(canvas.transform, false);
                    }

                    else { clone.transform.SetParent(Selection.activeGameObject.transform, false); }

                    clone.name = clone.name.Replace("(Clone)", "").Trim();
                    MakeSceneDirty(clone, clone.name);
                }

                catch
                {
                    CreateCanvas();
#if UNITY_2023_2_OR_NEWER
                    var canvas = FindObjectsByType<Canvas>(FindObjectsSortMode.None)[0];
#else
                    var canvas = (Canvas)FindObjectsOfType(typeof(Canvas))[0];
#endif
                    clone.transform.SetParent(canvas.transform, false);
                    clone.name = clone.name.Replace("(Clone)", "").Trim();
                    MakeSceneDirty(clone, clone.name);
                }

                Selection.activeObject = clone;
            }

            catch { ShowErrorDialog(); }
        }

        [MenuItem("GameObject/Hex UI/Canvas", false, 8)]
        static void CreateCanvas()
        {
            try
            {
                GetObjectPath();
                UpdateCustomEditorPath();
                GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath(objectPath + "UI Elements/Canvas/Canvas" + ".prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                clone.name = clone.name.Replace("(Clone)", "").Trim();
                Selection.activeObject = clone;
                MakeSceneDirty(clone, clone.name);
            }

            catch { ShowErrorDialog(); }
        }

        [MenuItem("GameObject/Hex UI/Button/Button", false, 8)]
        static void CreateButtonMain() { CreateObject("UI Elements/Button/Button"); }

        [MenuItem("GameObject/Hex UI/Button/Button (Alt Frame)", false, 8)]
        static void CreateButtonAlt() { CreateObject("UI Elements/Button/Button (Alt Frame)"); }

        [MenuItem("GameObject/Hex UI/Button/Button (Honeycomb)", false, 8)]
        static void CreateButtonHoneycomb() { CreateObject("UI Elements/Button/Button (Honeycomb)"); }

        [MenuItem("GameObject/Hex UI/Button/Button (Icon Only)", false, 8)]
        static void CreateButtonIconOnly() { CreateObject("UI Elements/Button/Button (Icon Only)"); }

        [MenuItem("GameObject/Hex UI/Button/Button (Icon Sway)", false, 8)]
        static void CreateButtonIconSway() { CreateObject("UI Elements/Button/Button (Icon Sway)"); }

        [MenuItem("GameObject/Hex UI/Button/Button (Panel)", false, 8)]
        static void CreateButtonPanel() { CreateObject("UI Elements/Button/Button (Panel)"); }

        [MenuItem("GameObject/Hex UI/Button/Button (Panel Alt)", false, 8)]
        static void CreateButtonPanelAlt() { CreateObject("UI Elements/Button/Button (Panel Alt)"); }

        [MenuItem("GameObject/Hex UI/Button/Button (Shop)", false, 8)]
        static void CreateButtonShop() { CreateObject("UI Elements/Button/Button (Shop)"); }

        [MenuItem("GameObject/Hex UI/Button/Button (Spot)", false, 8)]
        static void CreateButtonSpot() { CreateObject("UI Elements/Button/Button (Spot)"); }

        [MenuItem("GameObject/Hex UI/Dropdown/Standard", false, 8)]
        static void CreateDropdown() { CreateObject("UI Elements/Dropdown/Dropdown"); }

        [MenuItem("GameObject/Hex UI/HUD/Health Bar", false, 8)]
        static void CreateHudHealthBar() { CreateObject("HUD/Health Bar"); }

        [MenuItem("GameObject/Hex UI/HUD/Minimap", false, 8)]
        static void CreateHudMinimap() { CreateObject("HUD/Minimap"); }

        [MenuItem("GameObject/Hex UI/HUD/Quest Item", false, 8)]
        static void CreateHudQuestItem() { CreateObject("HUD/Quest Item"); }

        [MenuItem("GameObject/Hex UI/Input/Hotkey Indicator", false, 8)]
        static void CreateHotkeyIndicator() { CreateObject("UI Elements/Input/Hotkey Indicator"); }

        [MenuItem("GameObject/Hex UI/Input Field/Standard", false, 8)]
        static void CreateInputField() { CreateObject("UI Elements/Input Field/Input Field"); }

        [MenuItem("GameObject/Hex UI/Modal Window/Standard", false, 8)]
        static void CreateModalWindow() { CreateObject("UI Elements/Modal Window/Modal Window"); }

        [MenuItem("GameObject/Hex UI/Modal Window/Custom Content", false, 8)]
        static void CreateModalWindowCC() { CreateObject("UI Elements/Modal Window/Modal Window (Custom Content)"); }

        [MenuItem("GameObject/Hex UI/Notification/Standard", false, 8)]
        static void CreateNotification() { CreateObject("UI Elements/Notification/Notification"); }

        [MenuItem("GameObject/Hex UI/Panels/Credits", false, 8)]
        static void CreateCredits() { CreateObject("Panels/Credits"); }

        [MenuItem("GameObject/Hex UI/Panels/Panel Manager", false, 8)]
        static void CreatePanelManager() { CreateObject("Panels/Panel Manager"); }

        [MenuItem("GameObject/Hex UI/Progress Bar/Standard", false, 8)]
        static void CreateProgressBar() { CreateObject("UI Elements/Progress Bar/Progress Bar"); }

        [MenuItem("GameObject/Hex UI/Scrollbar/Horizontal", false, 8)]
        static void CreateScrollbarHorizontal() { CreateObject("UI Elements/Scrollbar/Scrollbar Horizontal"); }

        [MenuItem("GameObject/Hex UI/Scrollbar/Vertical", false, 8)]
        static void CreateScrollbarVertical() { CreateObject("UI Elements/Scrollbar/Scrollbar Vertical"); }

        [MenuItem("GameObject/Hex UI/Selectors/Horizontal Selector", false, 8)]
        static void CreateHorizontalSelector() { CreateObject("UI Elements/Selectors/Horizontal Selector"); }

        [MenuItem("GameObject/Hex UI/Settings/Settings Element (Dropdown)", false, 8)]
        static void CreateSettingsDropdownt() { CreateObject("UI Elements/Settings/Settings Element (Dropdown Alt)"); }

        [MenuItem("GameObject/Hex UI/Settings/Settings Element (Horizontal Selector)", false, 8)]
        static void CreateSettingsHS() { CreateObject("UI Elements/Settings/Settings Element (Horizontal Selector)"); }

        [MenuItem("GameObject/Hex UI/Settings/Settings Element (Slider)", false, 8)]
        static void CreateSettingsSlider() { CreateObject("UI Elements/Settings/Settings Element (Slider)"); }

        [MenuItem("GameObject/Hex UI/Settings/Settings Element (Switch)", false, 8)]
        static void CreateSettingsSwitch() { CreateObject("UI Elements/Settings/Settings Element (Switch)"); }

        [MenuItem("GameObject/Hex UI/Settings/Settings Header", false, 8)]
        static void CreateSettingsHeader() { CreateObject("UI Elements/Settings/Settings Header"); }

        [MenuItem("GameObject/Hex UI/Slider/Standard", false, 8)]
        static void CreateSlider() { CreateObject("UI Elements/Slider/Slider"); }

        [MenuItem("GameObject/Hex UI/Spinners/Basic Spinner", false, 8)]
        static void CreateSpinner() { CreateObject("UI Elements/Spinners/Basic Spinner"); }

        [MenuItem("GameObject/Hex UI/Spinners/Fluid Line", false, 8)]
        static void CreateFluidLine() { CreateObject("UI Elements/Spinners/Fluid Line"); }

        [MenuItem("GameObject/Hex UI/Spinners/Hexagon Spinner", false, 8)]
        static void CreateHexSpinner() { CreateObject("UI Elements/Spinners/Hexagon Spinner"); }

        [MenuItem("GameObject/Hex UI/Switch/Standard", false, 8)]
        static void CreateSwitch() { CreateObject("UI Elements/Switch/Switch"); }

        [MenuItem("GameObject/Hex UI/Text/Text (TMP)", false, 8)]
        static void CreateText() { CreateObject("UI Elements/Text/Text (TMP)"); }

        [MenuItem("GameObject/Hex UI/Timer/Timer Bar", false, 8)]
        static void CreateTimerBar() { CreateObject("UI Elements/Timer/Timer Bar"); }

        [MenuItem("GameObject/Hex UI/Widgets/News Slider", false, 8)]
        static void CreateNewsSlider() { CreateObject("UI Elements/Widgets/News Slider/News Slider"); }

        [MenuItem("GameObject/Hex UI/Widgets/Socials", false, 8)]
        static void CreateSocialsWidget() { CreateObject("UI Elements/Widgets/Socials/Socials Widget"); }
    }
}
#endif