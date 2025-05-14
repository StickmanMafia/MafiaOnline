using System.Collections;
using UnityEngine;

namespace Michsky.UI.Hex
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class SidebarReveal : MonoBehaviour
    {
        // Resources
        [SerializeField] private Animator animator;
        [SerializeField] private CanvasGroup canvasGroup;

        // Settings
        [SerializeField] private UpdateMode updateMode = UpdateMode.DeltaTime;
        [SerializeField] private BarDirection barDirection = BarDirection.Top;

        // Helpers
        float cachedStateLength = 0.4f;

        public enum UpdateMode { DeltaTime, UnscaledTime }
        public enum BarDirection { Top, Bottom, Left, Right }

        void Awake()
        {
            if (animator == null) { GetComponent<Animator>(); }
            if (canvasGroup == null) { GetComponent<CanvasGroup>(); }

            cachedStateLength = HexUIInternalTools.GetAnimatorClipLength(animator, "Sidebar_TopShow") + 0.02f;
        }

        void OnEnable()
        {
            Show();
        }

        public void Show()
        {
            animator.enabled = true;

            StopCoroutine("DisableAnimator");
            StartCoroutine("DisableAnimator");

            if (barDirection == BarDirection.Top) { animator.Play("Top Show"); }
            else if (barDirection == BarDirection.Bottom) { animator.Play("Bottom Show"); }
            else if (barDirection == BarDirection.Left) { animator.Play("Left Show"); }
            else if (barDirection == BarDirection.Right) { animator.Play("Right Show"); }
        }

        public void Hide()
        {
            animator.enabled = true;

            StopCoroutine("DisableAnimator");
            StartCoroutine("DisableAnimator");

            if (barDirection == BarDirection.Top) { animator.Play("Top Hide"); }
            else if (barDirection == BarDirection.Bottom) { animator.Play("Bottom Hide"); }
            else if (barDirection == BarDirection.Left) { animator.Play("Left Hide"); }
            else if (barDirection == BarDirection.Right) { animator.Play("Right Hide"); }
        }

        IEnumerator DisableAnimator()
        {
            if (updateMode == UpdateMode.DeltaTime) { yield return new WaitForSeconds(cachedStateLength); }
            else { yield return new WaitForSecondsRealtime(cachedStateLength); }

            animator.enabled = false;
        }
    }
}