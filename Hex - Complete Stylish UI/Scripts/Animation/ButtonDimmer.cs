using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Hex
{
    public class ButtonDimmer : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private Transform buttonParent;

        [Header("Settings")]
        public bool allowGamepad = true;

        // Helpers
        List<ButtonManager> buttons = new List<ButtonManager>();

        void Awake()
        {
            if (buttonParent == null) { buttonParent = gameObject.transform; }
            FetchButtons();
        }

        public void FetchButtons()
        {
            buttons.Clear();

            foreach (Transform child in buttonParent)
            {
                if (child.GetComponent<ButtonManager>() != null)
                {
                    ButtonManager btn = child.GetComponent<ButtonManager>();
                    btn.dimmer = this;
                    buttons.Add(btn);
                }
            }
        }

        public void LitButtons(ButtonManager source = null)
        {
            foreach (ButtonManager btn in buttons)
            {
                if (source != null && btn == source)
                    continue;

                btn.Interactable(true);
            }
        }

        public void DimButtons(ButtonManager source)
        {
            foreach (ButtonManager btn in buttons)
            {
                if (btn == source)
                    continue;

                btn.Interactable(false);
            }
        }
    }
}