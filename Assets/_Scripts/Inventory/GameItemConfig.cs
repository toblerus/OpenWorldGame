using System;
using UnityEngine;

namespace _Scripts.Inventory
{
    [Serializable]
    [CreateAssetMenu(menuName = "Game/GameItem")]
    public class GameItemConfig : ScriptableObject
    {
        public GameItemType Name;
        public string Description;
        public Sprite Icon;
        public int MaxStack;
        public GameObject Prefab;
        public bool ShowsInHand;
        public bool IsPlaceable;
        public Vector3 InHandOffset;
        public Vector3 InHandRotation;
    }
}