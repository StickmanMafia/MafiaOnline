using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.Hex
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class SpotButtonManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        // Content
        public Sprite buttonBackground;
        public string buttonTitle = "Button";
        public string titleLocalizationKey;
        [TextArea(0, 4)] public string buttonDescription = "Description";
        public string descriptionLocalizationKey;
        public string actionText = "Action";
        public string actionLocalizationKey;

        // Resources
        [SerializeField] private Animator animator;
        public Image backgroundObj;
        public TextMeshProUGUI titleObj;
        public TextMeshProUGUI descriptionObj;
        public TextMeshProUGUI actionObj;

        // Settings
        public bool isInteractable = true;
        public bool useCustomContent = false;
        [SerializeField] private bool checkForDoubleClick = true;
        [SerializeField] private bool useLocalization = true;
        [SerializeField] private bool bypassUpdateOnEnable = false;
        public bool useUINavigation = false;
        public Navigation.Mode navigationMode = Navigation.Mode.Automatic;
        public GameObject selectOnUp;
        public GameObject selectOnDown;
        public GameObject selectOnLeft;
        public GameObject selectOnRight;
        public bool wrapAround = false;
        public bool useSounds = true;
        [SerializeField] [Range(0.1f, 1)] private float doubleClickPeriod = 0.25f;

        // Events
        public UnityEvent onClick = new UnityEvent();
        public UnityEvent onDoubleClick = new UnityEvent();
        public UnityEvent onHover = new UnityEvent();
        public UnityEvent onLeave = new UnityEvent();
        public UnityEvent onSelect = new UnityEvent();
        public UnityEvent onDeselect = new UnityEvent();

        // Helpers
        bool isInitialized = false;
        float cachedStateLength = 0.5f;
        bool waitingForDoubleClickInput;
        Button targetButton;
#if UNITY_EDITOR
        public int latestTabIndex = 0;
#endif

        void Awake()
        {
            cachedStateLength = HexUIInternalTools.GetAnimatorClipLength(animator, "SpotButton_Highlighted") + 0.1f;
        }

        void OnEnable()
        {
            if (!isInitialized) { Initialize(); }
            if (!bypassUpdateOnEnable) { UpdateUI(); }
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject) { TriggerAnimation("Highlighted"); }
            if (Application.isPlaying && useUINavigation) { AddUINavigation(); }
            else if (Application.isPlaying && !useUINavigation && targetButton == null)
            {
                if (gameObject.GetComponent<Button>() == null) { targetButton = gameObject.AddComponent<Button>(); }
                else { targetButton = GetComponent<Button>(); }

                if (ControllerManager.instance != null && !ControllerManager.instance.gamepadEnabled)
                {
                    Navigation customNav = new Navigation();
                    customNav.mode = Navigation.Mode.None;
                    targetButton.navigation = customNav;
                }

                targetButton.transition = Selectable.Transition.None;
            }
        }

        void Initialize()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            if (ControllerManager.instance != null) { ControllerManager.instance.spotButtons.Add(this); }
            if (UIManagerAudio.instance == null) { useSounds = false; }
            if (animator == null) { animator = GetComponent<Animator>(); }
            if (GetComponent<Image>() == null)
            {
                Image raycastImg = gameObject.AddComponent<Image>();
                raycastImg.color = new Color(0, 0, 0, 0);
                raycastImg.raycastTarget = true;
            }

            TriggerAnimation("Start");

            if (useLocalization && !useCustomContent)
            {
                LocalizedObject mainLoc = GetComponent<LocalizedObject>();

                if (mainLoc == null || !mainLoc.CheckLocalizationStatus()) { useLocalization = false; }
                else
                {
                    if (titleObj != null && !string.IsNullOrEmpty(titleLocalizationKey))
                    {
                        LocalizedObject titleLoc = titleObj.gameObject.GetComponent<LocalizedObject>();
                        if (titleLoc != null)
                        {
                            titleLoc.tableIndex = mainLoc.tableIndex;
                            titleLoc.localizationKey = titleLocalizationKey;
                            titleLoc.UpdateItem();
                        }
                    }

                    if (descriptionObj != null && !string.IsNullOrEmpty(descriptionLocalizationKey))
                    {
                        LocalizedObject descLoc = descriptionObj.gameObject.GetComponent<LocalizedObject>();
                        if (descLoc != null)
                        {
                            descLoc.tableIndex = mainLoc.tableIndex;
                            descLoc.localizationKey = descriptionLocalizationKey;
                            descLoc.UpdateItem();
                        }
                    }

                    if (actionObj != null && !string.IsNullOrEmpty(actionLocalizationKey))
                    {
                        LocalizedObject descLoc = actionObj.gameObject.GetComponent<LocalizedObject>();
                        if (descLoc != null)
                        {
                            descLoc.tableIndex = mainLoc.tableIndex;
                            descLoc.localizationKey = actionLocalizationKey;
                            descLoc.UpdateItem();
                        }
                    }
                }
            }

            isInitialized = true;
        }

        public void UpdateUI()
        {
            if (titleObj != null) { titleObj.gameObject.SetActive(true); titleObj.text = buttonTitle; }
            else if (titleObj != null) { titleObj.gameObject.SetActive(false); }

            if (descriptionObj != null) { descriptionObj.gameObject.SetActive(true); descriptionObj.text = buttonDescription; }
            else if (descriptionObj != null) { descriptionObj.gameObject.SetActive(false); }

            if (actionObj != null) { actionObj.gameObject.SetActive(true); actionObj.text = actionText; }
            else if (actionObj != null) { actionObj.gameObject.SetActive(false); }

            if (backgroundObj != null) { backgroundObj.sprite = buttonBackground; }
            if (!Application.isPlaying || !gameObject.activeInHierarchy) { return; }

            TriggerAnimation("Start");
        }

        public void SetTitle(string text) { buttonTitle = text; UpdateUI(); }
        public void SetDescription(string text) { buttonDescription = text; UpdateUI(); }
        public void SetActionText(string text) { actionText = text; UpdateUI(); }
        public void SetBackground(Sprite bg) { buttonBackground = bg; UpdateUI(); }
        public void Interactable(bool value) { isInteractable = value; }

        public void AddUINavigation()
        {
            if (targetButton == null)
            {
                if (gameObject.GetComponent<Button>() == null) { targetButton = gameObject.AddComponent<Button>(); }
                else { targetButton = GetComponent<Button>(); }

                targetButton.transition = Selectable.Transition.None;
            }

            if (targetButton.navigation.mode == navigationMode)
                return;

            Navigation customNav = new Navigation();
            customNav.mode = navigationMode;

            if (navigationMode == Navigation.Mode.Vertical || navigationMode == Navigation.Mode.Horizontal) { customNav.wrapAround = wrapAround; }
            else if (navigationMode == Navigation.Mode.Explicit) { StartCoroutine("InitUINavigation", customNav); return; }

            targetButton.navigation = customNav;
        }

        public void DisableUINavigation()
        {
            if (targetButton != null)
            {
                Navigation customNav = new Navigation();
                Navigation.Mode navMode = Navigation.Mode.None;
                customNav.mode = navMode;
                targetButton.navigation = customNav;
            }
        }

        public void InvokeOnClick()
        {
            onClick.Invoke();
        }

        void TriggerAnimation(string triggername)
        {
            animator.enabled = true;

            animator.ResetTrigger("Start");
            animator.ResetTrigger("Normal");
            animator.ResetTrigger("Highlighted");

            animator.SetTrigger(triggername);

            StopCoroutine("DisableAnimator");
            StartCoroutine("DisableAnimator");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isInteractable || eventData.button != PointerEventData.InputButton.Left) { return; }
            if (useSounds) { UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.clickSound); }

            // Invoke click actions
            onClick.Invoke();

            // Check for double click
            if (!checkForDoubleClick) { return; }
            if (waitingForDoubleClickInput)
            {
                onDoubleClick.Invoke();
                waitingForDoubleClickInput = false;
                return;
            }

            waitingForDoubleClickInput = true;

            if (gameObject.activeInHierarchy)
            {
                StopCoroutine("CheckForDoubleClick");
                StartCoroutine("CheckForDoubleClick");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isInteractable) { return; }
            if (useSounds) { UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.hoverSound); }

            TriggerAnimation("Highlighted");
            onHover.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isInteractable)
                return;

            TriggerAnimation("Normal");
            onLeave.Invoke();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (!isInteractable) { return; }
            if (useSounds) { UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.hoverSound); }

            TriggerAnimation("Highlighted");
            onSelect.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (!isInteractable)
                return;

            TriggerAnimation("Normal");
            onDeselect.Invoke();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (!isInteractable) { return; }
            if (useSounds) { UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.clickSound); }
            if (EventSystem.current.currentSelectedGameObject != gameObject) { TriggerAnimation("Normal"); }

            onClick.Invoke();
        }

        IEnumerator CheckForDoubleClick()
        {
            yield return new WaitForSecondsRealtime(doubleClickPeriod);
            waitingForDoubleClickInput = false;
        }

        IEnumerator InitUINavigation(Navigation nav)
        {
            yield return new WaitForSecondsRealtime(0.1f);
           
            if (selectOnUp != null) { nav.selectOnUp = selectOnUp.GetComponent<Selectable>(); }
            if (selectOnDown != null) { nav.selectOnDown = selectOnDown.GetComponent<Selectable>(); }
            if (selectOnLeft != null) { nav.selectOnLeft = selectOnLeft.GetComponent<Selectable>(); }
            if (selectOnRight != null) { nav.selectOnRight = selectOnRight.GetComponent<Selectable>(); }
           
            targetButton.navigation = nav;
        }

        IEnumerator DisableAnimator()
        {
            yield return new WaitForSecondsRealtime(cachedStateLength);
            animator.enabled = false;
        }
    }
}