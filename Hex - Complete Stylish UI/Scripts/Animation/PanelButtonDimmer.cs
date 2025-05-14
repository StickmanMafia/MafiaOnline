using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Hex
{
    public class PanelButtonDimmer : MonoBehaviour
    {
        // Resources
        [SerializeField] private Transform buttonParent;

        // Helpers
        List<PanelButton> buttons = new List<PanelButton>();

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
                if (child.GetComponent<PanelButton>() != null)
                {
                    PanelButton btn = child.GetComponent<PanelButton>();
                    btn.dimmer = this;
                    buttons.Add(btn);
                }
            }
        }

        public void LitButtons(PanelButton source = null)
        {
            foreach (PanelButton btn in buttons)
            {
                if (btn.isSelected || (source != null && btn == source))
                    continue;

                btn.IsInteractable(true);
            }
        }

        public void DimButtons(PanelButton source)
        {
            foreach (PanelButton btn in buttons)
            {
                if (btn.isSelected || btn == source)
                    continue;

                btn.IsInteractable(false);
            }
        }
    }
}