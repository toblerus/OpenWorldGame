using System;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    [CreateAssetMenu(menuName = "Game/GameItem")]
    public class GameItemConfig : ScriptableObject
    {
        public GameItemType Name;
        public string Description;
        public Sprite Icon;
        public int MaxStack;
        public GameObject InHandPrefab;
        public Vector3 InHandOffset;
        public Vector3 InHandRotation;
    }
}