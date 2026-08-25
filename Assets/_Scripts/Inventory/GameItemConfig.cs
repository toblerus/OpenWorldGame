using System;
using System.Collections.Generic;
using _Scripts.Crafting;
using UnityEngine;

namespace _Scripts.Inventory
{
    [Serializable]
    [CreateAssetMenu(menuName = "Game/GameItem")]
    public class GameItemConfig : ScriptableObject
    {
        public GameItemType Name;
        public CraftingCategoryType Category;
        public string Description;
        public Sprite Icon;
        public int MaxStack;
        public GameObject Prefab;
        public bool ShowsInHand;
        public bool IsPlaceable;
        public Vector3 InHandOffset;
        public Vector3 InHandRotation;
        public List<CraftingIngredient> Ingredients;
    }

    [Serializable]
    public class CraftingIngredient
    {
        public GameItemType Type;
        public int Amount;
    }
}