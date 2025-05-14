using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Hex
{
    public class HexagonContainer : MonoBehaviour
    {
        [Header("Animation")]
        public AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));
        [Range(0.5f, 10)] public float curveSpeed = 1;
        [Tooltip("Add a delay on enable for the animation process.")]
        [Range(0, 5)] public float animationDelay = 0;

        [Header("Fading")]
        [Tooltip("Start fade-in transition after the specified transform scale (between 0-1).")]
        [Range(0, 0.99f)] public float fadeAfterScale = 0.6f;
        [Range(0.1f, 10)] public float fadeSpeed = 5f;
     
        [Header("Settings")]
        public UpdateMode updateMode = UpdateMode.DeltaTime;
        [Tooltip("Add a cooldown before skipping to the next hexagon item.")]
        [Range(0, 1)] public float itemCooldown = 0.1f;
        [Tooltip("Play the animation only once.")]
        public bool playOnce = false;

        // Helpers
        List<HexagonContainerItem> cachedItems = new List<HexagonContainerItem>();
        [HideInInspector] public bool isPlayedOnce = false;

        public enum UpdateMode { DeltaTime, UnscaledTime }

        void Awake()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
                /*HexagonContainerItem temp = child.gameObject.AddComponent<HexagonContainerItem>();
                temp.container = this;
                cachedItems.Add(temp);*/
            }
        }

        void OnEnable()
        {
            /// Animate();
        }

        public void Animate()
        {
            if (playOnce && isPlayedOnce)
                return;

            StopAllCoroutines();
            StartCoroutine(AnimateHelper());
        }

        IEnumerator AnimateHelper()
        {
            if (updateMode == UpdateMode.DeltaTime) { yield return new WaitForSeconds(animationDelay); }
            else { yield return new WaitForSecondsRealtime(animationDelay); }

            float tempTime = 0;

            if (cachedItems.Count > 0)
            {
                foreach (HexagonContainerItem item in cachedItems)
                {
                    item.Process(tempTime);
                    tempTime += itemCooldown;
                }
            }

            isPlayedOnce = true;
        }
    }
}