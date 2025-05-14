using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.Hex
{
    public class HeaderTransition : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private Animator animator;
        [SerializeField] private RectTransform fillerParent;
        [SerializeField] private Slider filler;
        [SerializeField] private RectTransform content;
        [SerializeField] private RectTransform contentHelper;
        [SerializeField] private TextMeshProUGUI helperText;
        [SerializeField] private TextMeshProUGUI contentText;

        [Header("Settings")]
        [SerializeField] private UpdateMode updateMode = UpdateMode.UnscaledTime;

        // Helpers
        float cachedStateLength = 0.5f;

        public enum UpdateMode { DeltaTime, UnscaledTime }

        void Awake()
        {
            cachedStateLength = HexUIInternalTools.GetAnimatorClipLength(animator, "PanelHeader_Go") + 0.01f;
        }

        void OnEnable()
        {
            animator.enabled = false;
            fillerParent.gameObject.SetActive(false);
            contentHelper.gameObject.SetActive(false);
        }

        public void DoTransition(string newText)
        {
            if (filler.value < 0.25f)
            {
                helperText.text = contentText.text;
                contentText.text = newText;

                animator.enabled = true;
                animator.Play("Init");

                StopCoroutine("SetFillerWidth");
                StartCoroutine("SetFillerWidth");
            }

            else if (helperText.text.Length < newText.Length)
            {
                contentText.text = newText;
              
                StopCoroutine("SetFillerWidtWhilePlaying");
                StartCoroutine("SetFillerWidtWhilePlaying");
            }

            else
            {
                contentText.text = newText;
            }
        }

        IEnumerator SetFillerWidth()
        {
            StopCoroutine("DisableAnimator");
            StopCoroutine("SetFillerWidtWhilePlaying");

            if (updateMode == UpdateMode.UnscaledTime) { yield return new WaitForSecondsRealtime(0.01f); }
            else { yield return new WaitForSeconds(0.01f); }

            if (content.sizeDelta.x > contentHelper.sizeDelta.x) { fillerParent.sizeDelta = new Vector2(content.sizeDelta.x, fillerParent.sizeDelta.y); }
            else { fillerParent.sizeDelta = new Vector2(contentHelper.sizeDelta.x, fillerParent.sizeDelta.y); }

            animator.Play("Go");
            StartCoroutine("DisableAnimator");
        }

        IEnumerator SetFillerWidtWhilePlaying()
        {
            StopCoroutine("DisableAnimator");
            StopCoroutine("SetFillerWidth");

            if (updateMode == UpdateMode.UnscaledTime) { yield return new WaitForSecondsRealtime(0.01f); }
            else { yield return new WaitForSeconds(0.01f); }

            fillerParent.sizeDelta = new Vector2(content.sizeDelta.x, fillerParent.sizeDelta.y);
        }

        IEnumerator DisableAnimator()
        {
            if (updateMode == UpdateMode.UnscaledTime) { yield return new WaitForSecondsRealtime(cachedStateLength); }
            else { yield return new WaitForSeconds(cachedStateLength); }

            animator.enabled = false;
        }
    }
}