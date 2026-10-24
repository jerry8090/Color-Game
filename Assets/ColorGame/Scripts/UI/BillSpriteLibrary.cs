using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColorGame.UI
{
    /// <summary>Maps a bet amount to its bill artwork. Filled by Design step 1, or by hand if you draw your own sprites.</summary>
    [CreateAssetMenu(fileName = "BillSpriteLibrary", menuName = "Color Game/Bill Sprite Library")]
    public class BillSpriteLibrary : ScriptableObject
    {
        [Serializable]
        public class Entry
        {
            public int denomination;
            public Sprite sprite;
        }

        [SerializeField] private List<Entry> entries = new List<Entry>();

        public Sprite Get(int denomination)
        {
            foreach (Entry entry in entries)
            {
                if (entry != null && entry.denomination == denomination) return entry.sprite;
            }
            return null;
        }
    }
}