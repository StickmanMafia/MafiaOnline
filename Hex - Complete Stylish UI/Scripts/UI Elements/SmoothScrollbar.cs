using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.Hex
{
    [RequireComponent(typeof(Scrollbar))]
    public class SmoothScrollbar : MonoBehaviour, IPointerEnterHandler
    {
        [Header("Settings")]
        [SerializeField][Range(0.3f, 5)] private float curveSpeed = 1.5f;
        [SerializeField] private AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));

        [Header("Auto-Fading")]
        [SerializeField] private bool enableAutoFading = true;
        [SerializeField] [Range(0.1f, 2)] private float waitDuration = 1;
        [SerializeField][Range(0, 0.9f)] private float targetOpacity = 0;
        [SerializeField] [Range(1, 8)] private float fadeSpeed = 4;
        [SerializeField] private CanvasGroup canvasGroup;

        // Helpers
        Scrollbar scrollbar;

        void Awake()
        {
            if (scrollbar == null) { scrollbar = GetComponent<Scrollbar>(); }
            if (enableAutoFading && canvasGroup != null) 
            {
                canvasGroup.alpha = targetOpacity;
                scrollbar.onValueChanged.AddListener(delegate { TriggerScrollbar(); }); 
            }
        }

        void OnEnable()
        {
            if (enableAutoFading && canvasGroup != null)
            {
                canvasGroup.alpha = targetOpacity;
            }
        }

        public void GoToTop()
        {
            StopCoroutine("GoBottom");
            StopCoroutine("GoTop");
            StartCoroutine("GoTop");
        }

        public void GoToBottom()
        {
            StopCoroutine("GoBottom");
            StopCoroutine("GoTop");
            StartCoroutine("GoBottom");
        }

        public void TriggerScrollbar()
        {
            if (!gameObject.activeInHierarchy)
                return;

            StopCoroutine("WaitForDuration");
            StopCoroutine("ShowScrollbar");
            StartCoroutine("ShowScrollbar");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableAutoFading && canvasGroup != null)
            {
                TriggerScrollbar();
            }
        }

        IEnumerator GoTop()
        {
            float startingPoint = scrollbar.value;
            float elapsedTime = 0;

            while (scrollbar.value < 0.999f)
            {
                elapsedTime += Time.unscaledDeltaTime;
                scrollbar.value = Mathf.Lerp(startingPoint, 1, animationCurve.Evaluate(elapsedTime * curveSpeed));
                yield return null;
            }

            scrollbar.value = 1;
        }

        IEnumerator GoBottom()
        {
            float startingPoint = scrollbar.value;
            float elapsedTime = 0;

            while (scrollbar.value > 0.001f)
            {
                elapsedTime += Time.unscaledDeltaTime;
                scrollbar.value = Mathf.Lerp(startingPoint, targetOpacity, animationCurve.Evaluate(elapsedTime * curveSpeed));
                yield return null;
            }

            scrollbar.value = 0;
        }

        IEnumerator ShowScrollbar()
        {
            StopCoroutine("HideScrollbar");

            while (canvasGroup.alpha < 0.99f)
            {
                canvasGroup.alpha += Time.unscaledDeltaTime * fadeSpeed;
                yield return null;
            }

            canvasGroup.alpha = 1;

            StartCoroutine("WaitForDuration");
        }

        IEnumerator HideScrollbar()
        {
            StopCoroutine("ShowScrollbar");

            while (canvasGroup.alpha > (targetOpacity + 0.01f))
            {
                canvasGroup.alpha -= Time.unscaledDeltaTime * fadeSpeed;
                yield return null;
            }

            canvasGroup.alpha = targetOpacity;
        }

        IEnumerator WaitForDuration()
        {
            yield return new WaitForSecondsRealtime(waitDuration);
            StartCoroutine("HideScrollbar");
        }
    }
}