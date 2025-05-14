using UnityEngine;
using TMPro;

namespace Michsky.UI.Hex
{
    [CreateAssetMenu(fileName = "New UI Manager", menuName = "Hex UI/UI Manager")]
    public class UIManager : ScriptableObject
    {
        public static string buildID = "TO24-0501";

        // Settings
        public bool enableDynamicUpdate = true;

        // Audio
        public AudioClip hoverSound;
        public AudioClip clickSound;
        public AudioClip notificationSound;

        // Colors
        public Color accentColor = new Color32(255, 255, 255, 255);
        public Color accentColorInvert = new Color32(255, 255, 255, 255);
        public Color primaryColor = new Color32(255, 255, 255, 255);
        public Color secondaryColor = new Color32(255, 255, 255, 255);
        public Color negativeColor = new Color32(255, 255, 255, 255);
        public Color backgroundColor = new Color32(255, 255, 255, 255);

        // Fonts
        public TMP_FontAsset fontLight;
        public TMP_FontAsset fontRegular;
        public TMP_FontAsset fontMedium;
        public TMP_FontAsset fontSemiBold;
        public TMP_FontAsset fontBold;
        public TMP_FontAsset customFont;

        // Localization
        public bool enableLocalization;
        public LocalizationSettings localizationSettings;
        public static string localizationSaveKey = "GameLanguage_";
        public LocalizationLanguage currentLanguage;
        public static bool isLocalizationEnabled = false;

        // Logo & Icon
        public Sprite gameIcon;
        public Sprite gameLogo;
        public Sprite brandLogo;

        // Splash Screen
        public bool enableSplashScreen = true;
        public bool showSplashScreenOnce = true;
        public PressAnyKeyTextType pakType;
        public string pakText = "Press {Any Key} To Start";
        public string pakLocalizationText = "PAK_Part1 {PAK_Key} PAK_Part2";

        public enum PressAnyKeyTextType { Default, Custom }
    }
}