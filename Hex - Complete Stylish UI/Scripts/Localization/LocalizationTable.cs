using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Hex
{
    [CreateAssetMenu(fileName = "New Localization Table", menuName = "Hex UI/Localization/Localization Table")]
    public class LocalizationTable : ScriptableObject
    {
        public string tableID;
        public LocalizationSettings localizationSettings;
        public List<TableContent> tableContent = new List<TableContent>();

        [System.Serializable]
        public class TableContent
        {
            public string key;
        }
    }
}