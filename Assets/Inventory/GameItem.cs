using System;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    [CreateAssetMenu(menuName = "Game/GameItem")]
    public class GameItem : ScriptableObject
    {
        public GameItemType Name;
        public string Description;
        public Sprite Icon;
        public int MaxStack;
    }
}