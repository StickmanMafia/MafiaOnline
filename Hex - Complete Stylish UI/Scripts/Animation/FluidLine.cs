using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.Hex
{
    public class FluidLine : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private Image line;
        [SerializeField] private ButtonManager targetButton;
        [SerializeField] private SpotButtonManager targetSpotButton;
        [SerializeField] private HotkeyEvent targetHotkey;

        [Header("Settings")]
        [SerializeField] private StartAnimation startAnimation = StartAnimation.Loop;
        [SerializeField] private AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
        [SerializeField] [Range(1, 10)] private float curveSpeed = 4;
        [Tooltip("Cancels the 'In' animation when the line fill amount doesn't reach to this threshold.")]
        [SerializeField] [Range(0.1f, 1)] private float abortTolerance = 0.4f;
        [Tooltip("Wait for specific amount of time before starting another loop.")]
        [SerializeField] [Range(0, 1)] private float loopRest = 0.15f;

        public enum StartAnimation { In, Out, Loop, ComponentDriven }

        void OnEnable()
        {
            if (line == null)
                return;

            UpdateLine();
        }

        void Start()
        {
            if (line == null)
                return;

            CheckForButtons();
        }

        public void UpdateLine()
        {
            line.fillAmount = 0;

            if (startAnimation == StartAnimation.In) { ProcessIn(); }
            else if (startAnimation == StartAnimation.Out) { ProcessOut(); }
            else if (startAnimation == StartAnimation.Loop) { ProcessLoop(); }
        }

        public void CheckForButtons()
        {
            if (targetButton != null)
            {
                targetButton.onHover.AddListener(ProcessIn);
                targetButton.onLeave.AddListener(ProcessOut);
                targetButton.onClick.AddListener(ProcessOut);
            }

            if (targetSpotButton != null)
            {
                targetSpotButton.onHover.AddListener(ProcessIn);
                targetSpotButton.onLeave.AddListener(ProcessOut);
                targetSpotButton.onClick.AddListener(ProcessOut);
            }

            if (targetHotkey != null)
            {
                targetHotkey.onHover.AddListener(ProcessIn);
                targetHotkey.onLeave.AddListener(ProcessOut);
                targetHotkey.onHotkeyPress.AddListener(ProcessOut);
            }
        }

        void ProcessIn()
        {
            StopCoroutine("LineIn");
            StopCoroutine("LineOut");
            StopCoroutine("LineAbort");
            StopCoroutine("LineLoop");

            StartCoroutine("LineIn");
        }

        void ProcessOut()
        {
            StopCoroutine("LineIn");
            StopCoroutine("LineOut");
            StopCoroutine("LineAbort");
            StopCoroutine("LineLoop");

            if (line.fillAmount > abortTolerance) { StartCoroutine("LineOut"); }
            else { StartCoroutine("LineAbort"); }
        }

        void ProcessLoop()
        {
            StopCoroutine("LineIn");
            StopCoroutine("LineOut");
            StopCoroutine("LineAbort");
            StopCoroutine("LineLoop");

            StartCoroutine("LineLoop");
        }

        IEnumerator LineIn()
        {
            float elapsedTime = 0;
            float startingPoint = line.fillAmount;

            line.fillOrigin = 0;

            while (line.fillAmount < 0.999f)
            {
                elapsedTime += Time.unscaledDeltaTime;
                line.fillAmount = Mathf.Lerp(startingPoint, 1, animationCurve.Evaluate(elapsedTime * curveSpeed));
                yield return null;
            }

            line.fillAmount = 1;
        }

        IEnumerator LineOut()
        {
            float elapsedTime = 0;
            float startingPoint = line.fillAmount;

            line.fillOrigin = 1;

            while (line.fillAmount > 0.001f)
            {
                elapsedTime += Time.unscaledDeltaTime;
                line.fillAmount = Mathf.Lerp(startingPoint, 0, animationCurve.Evaluate(elapsedTime * curveSpeed));
                yield return null;
            }

            line.fillAmount = 0;
        }

        IEnumerator LineAbort()
        {
            float elapsedTime = 0;
            float startingPoint = line.fillAmount;

            line.fillOrigin = 0;

            while (line.fillAmount > 0.001f)
            {
                elapsedTime += Time.unscaledDeltaTime;
                line.fillAmount = Mathf.Lerp(startingPoint, 0, animationCurve.Evaluate(elapsedTime * curveSpeed));
                yield return null;
            }

            line.fillAmount = 0;
        }

        IEnumerator LineLoop()
        {
            // Phase 1
            float elapsedTime = 0;
            float startingPoint = line.fillAmount;

            line.fillOrigin = 0;

            while (line.fillAmount < 0.999f)
            {
                elapsedTime += Time.unscaledDeltaTime;
                line.fillAmount = Mathf.Lerp(startingPoint, 1, animationCurve.Evaluate(elapsedTime * curveSpeed));
                yield return null;
            }

            line.fillAmount = 1;

            // Phase 2
            elapsedTime = 0;
            startingPoint = line.fillAmount;
            line.fillOrigin = 1;

            while (line.fillAmount > 0.001f)
            {
                elapsedTime += Time.unscaledDeltaTime;
                line.fillAmount = Mathf.Lerp(startingPoint, 0, animationCurve.Evaluate(elapsedTime * curveSpeed));
                yield return null;
            }

            line.fillAmount = 0;

            // Check for rest timer
            yield return new WaitForSecondsRealtime(loopRest);

            // Phase 3
            elapsedTime = 0;
            startingPoint = line.fillAmount;

            line.fillOrigin = 1;

            while (line.fillAmount < 0.999f)
            {
                elapsedTime += Time.unscaledDeltaTime;
                line.fillAmount = Mathf.Lerp(startingPoint, 1, animationCurve.Evaluate(elapsedTime * curveSpeed));
                yield return null;
            }

            line.fillAmount = 1;

            // Phase 4
            elapsedTime = 0;
            startingPoint = line.fillAmount;

            line.fillOrigin = 0;

            while (line.fillAmount > 0.001f)
            {
                elapsedTime += Time.unscaledDeltaTime;
                line.fillAmount = Mathf.Lerp(startingPoint, 0, animationCurve.Evaluate(elapsedTime * curveSpeed));
                yield return null;
            }

            line.fillAmount = 0;

            // Check for rest timer
            yield return new WaitForSecondsRealtime(loopRest);

            // Restart coroutine
            StartCoroutine("LineLoop");
        }
    }
}