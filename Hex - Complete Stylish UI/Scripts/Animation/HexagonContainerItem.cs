using System.Collections;
using UnityEngine;

namespace Michsky.UI.Hex
{
    [RequireComponent(typeof(CanvasGroup))]
    public class HexagonContainerItem : MonoBehaviour
    {
        [HideInInspector] public HexagonContainer container;

        // Helpers
        CanvasGroup cg;
        Vector3 defaultScale = new Vector3 (1f, 1f, 1f);

        void Awake()
        {
            defaultScale = transform.localScale;
            cg = GetComponent<CanvasGroup>();
            cg.alpha = 0;
        }

        void OnDisable()
        {
            if (container.playOnce && container.isPlayedOnce)
            {
                cg.alpha = 1;
                transform.localScale = defaultScale;
            }

            else
            {
                cg.alpha = 0;
                transform.localScale = new Vector3(0, 0, 0);
            }
        }

        public void Process(float time)
        {
            if (!gameObject.activeInHierarchy)
                return;

            StartCoroutine(ProcessHexagonScale(time));
        }

        IEnumerator ProcessHexagonScale(float time)
        {
            transform.localScale = new Vector3(0, 0, 0);

            if (container.updateMode == HexagonContainer.UpdateMode.DeltaTime) { yield return new WaitForSeconds(time); }
            else { yield return new WaitForSecondsRealtime(time); }

            float elapsedTime = 0;
            float startingPoint = 0;
            bool fadeStarted = false;

            while (elapsedTime < 1)
            {
                float lerpValue = Mathf.Lerp(startingPoint, defaultScale.x, container.animationCurve.Evaluate(elapsedTime));
                transform.localScale = new Vector3(lerpValue, lerpValue, lerpValue);

                if (transform.localScale.x > container.fadeAfterScale && !fadeStarted)
                {
                    fadeStarted = true;
                    StartCoroutine("ProcessHexagonFade");
                }

                if (container.updateMode == HexagonContainer.UpdateMode.DeltaTime) { elapsedTime += Time.deltaTime * container.curveSpeed; }
                else { elapsedTime += Time.unscaledDeltaTime * container.curveSpeed; }

                yield return null;
            }

            transform.localScale = defaultScale;
        }

        IEnumerator ProcessHexagonFade()
        {
            cg.alpha = 0;

            while (cg.alpha < 0.99f)
            {
                if (container.updateMode == HexagonContainer.UpdateMode.DeltaTime) { cg.alpha += Time.deltaTime * container.fadeSpeed; }
                else { cg.alpha += Time.unscaledDeltaTime * container.fadeSpeed; }

                yield return null;
            }

            cg.alpha = 1;
        }
    }
}