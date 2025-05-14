using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.UI.Hex
{
    public class PanelManager : MonoBehaviour
    {
        // Content
        public List<PanelItem> panels = new List<PanelItem>();

        // Resources
        [SerializeField] private RectTransform indicator;
        [SerializeField] private HeaderTransition header;

        // Settings
        public int currentPanelIndex = 0;
        private int currentButtonIndex = 0;
        private int newPanelIndex;
        public bool cullPanels = true;
        [SerializeField] private bool initializeButtons = true;
        [SerializeField] private bool checkForPanelOrder = true;
        [SerializeField] private bool useCooldownForHotkeys = false;
        [SerializeField] private bool bypassAnimationOnEnable = false;
        [SerializeField] private UpdateMode updateMode = UpdateMode.UnscaledTime;
        [SerializeField] private PanelMode panelMode = PanelMode.Horizontal;
        [Range(0.75f, 2)] public float animationSpeed = 1;
        [SerializeField] private AnimationCurve indicatorCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
        [SerializeField] [Range(0.1f, 5)] private float indicatorDuration = 0.5f;
        [SerializeField] private IndicatorMode indicatorMode = IndicatorMode.Horizontal;

        // Events
        [System.Serializable] public class PanelChangeCallback : UnityEvent<int> { }
        public PanelChangeCallback onPanelChanged;

        // Helpers
        Animator currentPanel;
        Animator nextPanel;

        PanelButton currentButton;
        PanelButton nextButton;

        string animSpeedKey = "AnimSpeed";
        string panelFadeInScale = "In Scale";
        string panelFadeOutScale = "Out Scale";
        string panelFadeInBottom = "In Bottom";
        string panelFadeOutBottom = "Out Bottom";
        string panelFadeInTop = "In Top";
        string panelFadeOutTop = "Out Top";
        string verPanelFadeInLeft = "In Left";
        string verPanelFadeOutLeft = "Out Left";
        string verPanelFadeInRight = "In Right";
        string verPanelFadeOutRight = "Out Right";

        bool isInitialized = false;
        [HideInInspector] public float cachedStateLength = 1;
        [HideInInspector] public int managerIndex;

        public enum PanelMode { Horizontal, Vertical, Scale }
        public enum IndicatorMode { Horizontal, Vertical }
        public enum UpdateMode { DeltaTime, UnscaledTime }

        [System.Serializable]
        public class PanelItem
        {
            [Tooltip("[Required] This is the variable that you use to call specific panels.")]
            public string panelName = "My Panel";
            [Tooltip("[Required] Main panel object.")]
            public Animator panelObject;
            [Tooltip("[Optional] If you want the panel manager to have tabbing capability, you can assign a panel button here.")]
            public PanelButton panelButton;
            [Tooltip("[Optional] Alternate panel button variable that supports standard buttons instead of panel buttons.")]
            public ButtonManager altPanelButton;
            [Tooltip("[Optional] This is the object that will be selected as the current UI object on panel activation. Useful for gamepad navigation.")]
            public GameObject firstSelected;
            [Tooltip("[Optional] Enables or disables child hotkeys depending on the panel state to avoid conflict between hotkeys.")]
            public Transform hotkeyParent;
            [Tooltip("Enable or disable panel navigation when using the 'Previous' or 'Next' methods.")]
            public bool disableNavigation = false;
            [HideInInspector] public GameObject latestSelected;
            [HideInInspector] public HotkeyEvent[] hotkeys;
        }

        void Awake()
        {
            if (panels.Count == 0)
                return;

            cachedStateLength = HexUIInternalTools.GetAnimatorClipLength(panels[currentPanelIndex].panelObject, "MainPanel_InLeft");
        }

        void Start()
        {
            if (ControllerManager.instance != null)
            {
                managerIndex = ControllerManager.instance.panels.Count;
                ControllerManager.instance.panels.Add(this);
            }
        }

        void OnEnable()
        {
            if (!isInitialized) { InitializePanels(); }
            if (ControllerManager.instance != null) { ControllerManager.instance.currentManagerIndex = managerIndex; }

            if (bypassAnimationOnEnable)
            {
                for (int i = 0; i < panels.Count; i++)
                {
                    if (panels[i].panelObject == null)
                        continue;

                    if (currentPanelIndex == i) 
                    {
                        panels[i].panelObject.gameObject.SetActive(true);
                        panels[i].panelObject.enabled = true;
                        panels[i].panelObject.Play("Instant In"); 
                    }

                    else 
                    {
                        panels[i].panelObject.gameObject.SetActive(false);
                    }
                }
            }

            else if (isInitialized && !bypassAnimationOnEnable && nextPanel == null)
            {
                currentPanel.enabled = true;
                currentPanel.SetFloat(animSpeedKey, animationSpeed);

                if (panelMode == PanelMode.Horizontal) { currentPanel.Play(verPanelFadeInRight); }
                else if (panelMode == PanelMode.Vertical) { currentPanel.Play(panelFadeInBottom); }
                else if (panelMode == PanelMode.Scale) { currentPanel.Play(panelFadeInScale); }

                if (currentButton != null) { currentButton.SetSelected(true); }
            }

            else if (isInitialized && !bypassAnimationOnEnable && nextPanel != null)
            {
                nextPanel.enabled = true;
                nextPanel.SetFloat(animSpeedKey, animationSpeed);

                if (panelMode == PanelMode.Horizontal) { nextPanel.Play(verPanelFadeInRight); }
                else if (panelMode == PanelMode.Vertical) { nextPanel.Play(panelFadeInBottom); }
                else if (panelMode == PanelMode.Scale) { nextPanel.Play(panelFadeInScale); }

                if (nextButton != null) { nextButton.SetSelected(true); }
            }

            StopCoroutine("DisablePreviousPanel");
            StopCoroutine("DisableAnimators");
            StartCoroutine("DisableAnimators");
            StartCoroutine("InitIndicator");
        }

        public void InitializePanels()
        {
            if (panels[currentPanelIndex].panelButton != null)
            {
                currentButton = panels[currentPanelIndex].panelButton;
                currentButton.SetSelected(true);
            }

            currentPanel = panels[currentPanelIndex].panelObject;
            currentPanel.enabled = true;
            currentPanel.gameObject.SetActive(true);
            currentPanel.SetFloat(animSpeedKey, animationSpeed);

            if (panelMode == PanelMode.Horizontal) { currentPanel.Play(verPanelFadeInRight); }
            else if (panelMode == PanelMode.Vertical) { currentPanel.Play(panelFadeInBottom); }
            else if (panelMode == PanelMode.Scale) { currentPanel.Play(panelFadeInScale); }

            onPanelChanged.Invoke(currentPanelIndex);

            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].panelObject == null) { continue; }
                if (i != currentPanelIndex && cullPanels) { panels[i].panelObject.gameObject.SetActive(false); }
                if (initializeButtons)
                {
                    string tempName = panels[i].panelName;
                    if (panels[i].panelButton != null) { panels[i].panelButton.onClick.AddListener(() => OpenPanel(tempName)); }
                    if (panels[i].altPanelButton != null) { panels[i].altPanelButton.onClick.AddListener(() => OpenPanel(tempName)); }
                }
                if (panels[i].hotkeyParent != null)
                {
                    panels[i].hotkeys = panels[i].hotkeyParent.GetComponentsInChildren<HotkeyEvent>();
                    if (useCooldownForHotkeys) { foreach (HotkeyEvent he in panels[i].hotkeys) { he.useCooldown = true; } }
                }
            }

            if (indicator != null && panels[currentPanelIndex].panelButton == null) { indicator.sizeDelta = new Vector2(indicator.rect.width, 0); }
            else if (indicator != null)
            {
                indicator.transform.SetParent(panels[currentPanelIndex].panelButton.transform, true);
                indicator.anchoredPosition = new Vector2(0, 0);
                indicator.SetAsFirstSibling();

                if (indicatorMode == IndicatorMode.Horizontal)
                {
                    indicator.pivot = new Vector2(0.5f, indicator.pivot.y);
                    indicator.sizeDelta = new Vector2(panels[currentPanelIndex].panelButton.GetComponent<RectTransform>().sizeDelta.x, indicator.sizeDelta.y);
                    indicator.anchorMin = new Vector2(0.5f, 0);
                    indicator.anchorMax = new Vector2(0.5f, 0);
                    indicator.anchoredPosition = new Vector2(0, indicator.anchoredPosition.y);
                }

                else if (indicatorMode == IndicatorMode.Vertical)
                {
                    indicator.sizeDelta = new Vector2(indicator.sizeDelta.x, panels[currentPanelIndex].panelButton.GetComponent<RectTransform>().sizeDelta.y);
                    indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, 0);
                }
            }

            if (header != null)
            {
                if (panels[currentPanelIndex].panelButton == null) { header.DoTransition(panels[currentPanelIndex].panelName); }
                else { header.DoTransition(panels[currentPanelIndex].panelButton.buttonText); }
            }

            StopCoroutine("DisableAnimators");
            StartCoroutine("DisableAnimators");

            isInitialized = true;
        }

        public void OpenFirstPanel()
        {
            OpenPanelByIndex(0); 
        }

        public void OpenPanel(string newPanel)
        {
            bool catchedPanel = false;

            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].panelName == newPanel)
                {
                    newPanelIndex = i;
                    catchedPanel = true;
                    break;
                }
            }

            if (!catchedPanel)
            {
                Debug.LogWarning("There is no panel named '" + newPanel + "' in the panel list.", this);
                return;
            }

            if (newPanelIndex != currentPanelIndex)
            {
                int cachedCurrentPanelIndex = currentPanelIndex;

                if (cullPanels) { StopCoroutine("DisablePreviousPanel"); }
                if (ControllerManager.instance != null) { ControllerManager.instance.currentManagerIndex = managerIndex; }

                currentPanel = panels[currentPanelIndex].panelObject;

                if (panels[currentPanelIndex].hotkeyParent != null) { foreach (HotkeyEvent he in panels[currentPanelIndex].hotkeys) { he.enabled = false; } }
                if (panels[currentPanelIndex].panelButton != null) { currentButton = panels[currentPanelIndex].panelButton; }
                if (ControllerManager.instance != null && EventSystem.current.currentSelectedGameObject != null) { panels[currentPanelIndex].latestSelected = EventSystem.current.currentSelectedGameObject; }

                currentPanelIndex = newPanelIndex;
                nextPanel = panels[currentPanelIndex].panelObject;
                nextPanel.gameObject.SetActive(true);
           
                currentPanel.enabled = true;
                nextPanel.enabled = true;

                currentPanel.SetFloat(animSpeedKey, animationSpeed);
                nextPanel.SetFloat(animSpeedKey, animationSpeed);

                if (panelMode == PanelMode.Horizontal) 
                {
                    if (checkForPanelOrder && newPanelIndex > cachedCurrentPanelIndex)
                    {
                        currentPanel.Play(verPanelFadeOutLeft);
                        nextPanel.Play(verPanelFadeInLeft);
                    }

                    else
                    {
                        currentPanel.Play(verPanelFadeOutRight);
                        nextPanel.Play(verPanelFadeInRight);
                    }
                }

                else if (panelMode == PanelMode.Vertical)
                {
                    if (checkForPanelOrder && newPanelIndex > cachedCurrentPanelIndex)
                    {
                        currentPanel.Play(panelFadeOutTop);
                        nextPanel.Play(panelFadeInTop);
                    }

                    else
                    {
                        currentPanel.Play(panelFadeOutBottom);
                        nextPanel.Play(panelFadeInBottom);
                    }
                }

                else if (panelMode == PanelMode.Scale)
                {
                    currentPanel.Play(panelFadeOutScale);
                    nextPanel.Play(panelFadeInScale);
                }

                if (cullPanels) { StartCoroutine("DisablePreviousPanel"); }
                if (panels[currentPanelIndex].hotkeyParent != null) { foreach (HotkeyEvent he in panels[currentPanelIndex].hotkeys) { he.enabled = true; } }

                currentButtonIndex = newPanelIndex;

                if (ControllerManager.instance != null && panels[currentPanelIndex].latestSelected != null) { ControllerManager.instance.SelectUIObject(panels[currentPanelIndex].latestSelected); }
                else if (ControllerManager.instance != null && panels[currentPanelIndex].latestSelected == null) { ControllerManager.instance.SelectUIObject(panels[currentPanelIndex].firstSelected); }

                if (currentButton != null) { currentButton.SetSelected(false); }
                if (panels[currentButtonIndex].panelButton != null)
                {
                    nextButton = panels[currentButtonIndex].panelButton;
                    nextButton.SetSelected(true);
                }

                // Check for indicator and start the coroutine
                StopCoroutine("MoveIndicatorToParent");

                if (indicator != null && panels[currentButtonIndex].panelButton != null) { StartCoroutine("MoveIndicatorToParent", panels[currentButtonIndex].panelButton.transform); }
                else if (indicator != null && panels[currentButtonIndex].panelButton == null && indicatorMode == IndicatorMode.Horizontal) { StartCoroutine("SetIndicatorWidth", 0); }
                else if (indicator != null && panels[currentButtonIndex].panelButton == null && indicatorMode == IndicatorMode.Vertical) { StartCoroutine("SetIndicatorHeight", 0); }

                // Check for header transition
                if (header != null) 
                {
                    if (panels[currentPanelIndex].panelButton == null) { header.DoTransition(panels[currentPanelIndex].panelName); }
                    else { header.DoTransition(panels[currentPanelIndex].panelButton.buttonText); }
                }

                // Invoke assigned events
                onPanelChanged.Invoke(currentPanelIndex);

                StopCoroutine("DisableAnimators");
                StartCoroutine("DisableAnimators");
            }
        }

        public void OpenPanelByIndex(int panelIndex)
        {
            if (panelIndex > panels.Count || panelIndex < 0)
            {
                Debug.LogWarning("Index '" + panelIndex.ToString() + "' not found.", this);
                return;
            }

            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].panelName == panels[panelIndex].panelName)
                {
                    OpenPanel(panels[panelIndex].panelName);
                    break;
                }
            }
        }

        public void NextPanel()
        {
            if (currentPanelIndex <= panels.Count - 2 && !panels[currentPanelIndex + 1].disableNavigation)
            {
                OpenPanelByIndex(currentPanelIndex + 1);
            }
        }

        public void PreviousPanel()
        {
            if (currentPanelIndex >= 1 && !panels[currentPanelIndex - 1].disableNavigation)
            {
                OpenPanelByIndex(currentPanelIndex - 1);
            }
        }

        public void ShowCurrentPanel()
        {
            if (nextPanel == null) 
            {
                StopCoroutine("DisableAnimators");
                StartCoroutine("DisableAnimators");

                currentPanel.enabled = true;
                currentPanel.SetFloat(animSpeedKey, animationSpeed);

                if (panelMode == PanelMode.Horizontal) { currentPanel.Play(verPanelFadeInRight); }
                else if (panelMode == PanelMode.Vertical) { currentPanel.Play(panelFadeInBottom); }
                else if (panelMode == PanelMode.Scale) { currentPanel.Play(panelFadeInScale); }
            }
          
            else 
            {
                StopCoroutine("DisableAnimators");
                StartCoroutine("DisableAnimators");

                nextPanel.enabled = true;
                nextPanel.SetFloat(animSpeedKey, animationSpeed);

                if (panelMode == PanelMode.Horizontal) {  nextPanel.Play(verPanelFadeInRight); }
                else if (panelMode == PanelMode.Vertical) { nextPanel.Play(panelFadeInBottom); }
                else if (panelMode == PanelMode.Scale) { nextPanel.Play(panelFadeInScale); }
            }
        }

        public void HideCurrentPanel()
        {
            if (nextPanel == null) 
            {
                StopCoroutine("DisableAnimators");
                StartCoroutine("DisableAnimators");

                currentPanel.enabled = true;
                currentPanel.SetFloat(animSpeedKey, animationSpeed);

                if (panelMode == PanelMode.Horizontal) { currentPanel.Play(verPanelFadeOutRight); }
                else if (panelMode == PanelMode.Vertical) { currentPanel.Play(panelFadeOutBottom); }
                else if (panelMode == PanelMode.Scale) { currentPanel.Play(panelFadeOutScale); }
            }

            else 
            {
                StopCoroutine("DisableAnimators");
                StartCoroutine("DisableAnimators");

                nextPanel.enabled = true;
                nextPanel.SetFloat(animSpeedKey, animationSpeed);

                if (panelMode == PanelMode.Horizontal) { nextPanel.Play(verPanelFadeOutRight); }
                else if (panelMode == PanelMode.Vertical) { nextPanel.Play(panelFadeOutBottom); }
                else if (panelMode == PanelMode.Scale) { nextPanel.Play(panelFadeOutScale); }
            }
        }

        public void ShowCurrentButton()
        {
            if (nextButton == null) { currentButton.SetSelected(true); }
            else { nextButton.SetSelected(true); }
        }

        public void HideCurrentButton()
        {
            if (nextButton == null) { currentButton.SetSelected(false); }
            else { nextButton.SetSelected(false); }
        }

        public void AddNewItem()
        {
            PanelItem panel = new PanelItem();

            if (panels.Count != 0 && panels[panels.Count - 1].panelObject != null)
            {
                int tempIndex = panels.Count - 1;

                GameObject tempPanel = panels[tempIndex].panelObject.transform.parent.GetChild(tempIndex).gameObject;
                GameObject newPanel = Instantiate(tempPanel, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

                newPanel.transform.SetParent(panels[tempIndex].panelObject.transform.parent, false);
                newPanel.gameObject.name = "New Panel " + tempIndex.ToString();

                panel.panelName = "New Panel " + tempIndex.ToString();
                panel.panelObject = newPanel.GetComponent<Animator>();

                if (panels[tempIndex].panelButton != null)
                {
                    GameObject tempButton = panels[tempIndex].panelButton.transform.parent.GetChild(tempIndex).gameObject;
                    GameObject newButton = Instantiate(tempButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

                    newButton.transform.SetParent(panels[tempIndex].panelButton.transform.parent, false);
                    newButton.gameObject.name = "New Panel " + tempIndex.ToString();

                    panel.panelButton = newButton.GetComponent<PanelButton>();
                }

                else if (panels[tempIndex].altPanelButton != null)
                {
                    GameObject tempButton = panels[tempIndex].altPanelButton.transform.parent.GetChild(tempIndex).gameObject;
                    GameObject newButton = Instantiate(tempButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

                    newButton.transform.SetParent(panels[tempIndex].panelButton.transform.parent, false);
                    newButton.gameObject.name = "New Panel " + tempIndex.ToString();

                    panel.altPanelButton = newButton.GetComponent<ButtonManager>();
                }
            }

            panels.Add(panel);
        }

        IEnumerator DisablePreviousPanel()
        {
            if (updateMode == UpdateMode.UnscaledTime) { yield return new WaitForSecondsRealtime(cachedStateLength * animationSpeed); }
            else { yield return new WaitForSeconds(cachedStateLength * animationSpeed); }

            for (int i = 0; i < panels.Count; i++)
            {
                if (i == currentPanelIndex)
                    continue;

                panels[i].panelObject.gameObject.SetActive(false);
            }
        }

        IEnumerator DisableAnimators()
        {
            if (updateMode == UpdateMode.UnscaledTime) { yield return new WaitForSecondsRealtime(cachedStateLength * animationSpeed); }
            else { yield return new WaitForSeconds(cachedStateLength * animationSpeed); }

            if (currentPanel != null) { currentPanel.enabled = false; }
            if (nextPanel != null) { nextPanel.enabled = false; }
        }

        IEnumerator MoveIndicatorToParent(Transform parent)
        {
            float elapsedTime = 0;

            if (indicatorMode == IndicatorMode.Horizontal)
            {
                StopCoroutine("SetIndicatorWidth");

                if (parent != null)
                {
                    indicator.transform.SetParent(parent, true);
                    indicator.SetAsFirstSibling();
                    StartCoroutine("SetIndicatorWidth", parent.GetComponent<RectTransform>().sizeDelta.x);
                }

                else
                {
                    StartCoroutine("SetIndicatorWidth", 0);
                    StopCoroutine("MoveIndicatorToParent");
                }

                while (elapsedTime < indicatorDuration)
                {
                    if (updateMode == UpdateMode.UnscaledTime) { elapsedTime += Time.unscaledDeltaTime / indicatorDuration; }
                    else { elapsedTime += Time.deltaTime / indicatorDuration; }

                    indicator.anchoredPosition = new Vector2(Mathf.Lerp(indicator.anchoredPosition.x, 0, indicatorCurve.Evaluate(elapsedTime)), indicator.anchoredPosition.y);
                
                    yield return null;
                }

                indicator.anchoredPosition = new Vector2(0, indicator.anchoredPosition.y);
            }

            else if (indicatorMode == IndicatorMode.Vertical)
            {
                StopCoroutine("SetIndicatorHeight");

                if (parent != null)
                {
                    indicator.transform.SetParent(parent, true);
                    indicator.SetAsFirstSibling();
                    StartCoroutine("SetIndicatorHeight", parent.GetComponent<RectTransform>().sizeDelta.y);
                }

                else
                {
                    StartCoroutine("SetIndicatorHeight", 0);
                    StopCoroutine("MoveIndicatorToParent");
                }

                while (elapsedTime < indicatorDuration)
                {
                    if (updateMode == UpdateMode.UnscaledTime) { elapsedTime += Time.unscaledDeltaTime / indicatorDuration; }
                    else { elapsedTime += Time.deltaTime / indicatorDuration; }

                    indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, Mathf.Lerp(indicator.anchoredPosition.y, 0, indicatorCurve.Evaluate(elapsedTime)));
                  
                    yield return null;
                }

                indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, 0);
            }
        }

        IEnumerator SetIndicatorHeight(float targetHeight = 0)
        {
            float elapsedTime = 0;

            Vector2 startPos = new Vector2(indicator.sizeDelta.x, indicator.sizeDelta.y);
            Vector2 endPos = new Vector2(indicator.sizeDelta.x, targetHeight);

            while (elapsedTime < indicatorDuration)
            {
                if (updateMode == UpdateMode.UnscaledTime) { elapsedTime += Time.unscaledDeltaTime / indicatorDuration; }
                else { elapsedTime += Time.deltaTime / indicatorDuration; }

                indicator.sizeDelta = Vector2.Lerp(startPos, endPos, indicatorCurve.Evaluate(elapsedTime * 4));
               
                yield return null;
            }

            indicator.sizeDelta = endPos;
        }

        IEnumerator SetIndicatorWidth(float targetWidth = 0)
        {
            float elapsedTime = 0;

            Vector2 startPos = new Vector2(indicator.sizeDelta.x, indicator.sizeDelta.y);
            Vector2 endPos = new Vector2(targetWidth, indicator.sizeDelta.y);

            while (elapsedTime < indicatorDuration)
            {
                if (updateMode == UpdateMode.UnscaledTime) { elapsedTime += Time.unscaledDeltaTime / indicatorDuration; }
                else { elapsedTime += Time.deltaTime / indicatorDuration; }

                indicator.sizeDelta = Vector2.Lerp(startPos, endPos, indicatorCurve.Evaluate(elapsedTime * 4));
             
                yield return null;
            }

            indicator.sizeDelta = endPos;
        }

        IEnumerator InitIndicator()
        {
            yield return new WaitForSecondsRealtime(0.05f);

            if (indicator != null && panels[currentPanelIndex].panelButton == null) { indicator.sizeDelta = new Vector2(indicator.rect.width, 0); }
            else if (indicator != null)
            {
                indicator.transform.SetParent(panels[currentPanelIndex].panelButton.transform, true);
                indicator.anchoredPosition = new Vector2(0, 0);
                indicator.SetAsFirstSibling();

                if (indicatorMode == IndicatorMode.Horizontal)
                {
                    indicator.pivot = new Vector2(0.5f, indicator.pivot.y);
                    indicator.sizeDelta = new Vector2(panels[currentPanelIndex].panelButton.GetComponent<RectTransform>().sizeDelta.x, indicator.sizeDelta.y);
                    indicator.anchorMin = new Vector2(0.5f, 0);
                    indicator.anchorMax = new Vector2(0.5f, 0);
                    indicator.anchoredPosition = new Vector2(0, indicator.anchoredPosition.y);
                }

                else if (indicatorMode == IndicatorMode.Vertical)
                {
                    indicator.sizeDelta = new Vector2(indicator.sizeDelta.x, panels[currentPanelIndex].panelButton.GetComponent<RectTransform>().sizeDelta.y);
                    indicator.anchoredPosition = new Vector2(indicator.anchoredPosition.x, 0);
                }
            }
        }
    }
}