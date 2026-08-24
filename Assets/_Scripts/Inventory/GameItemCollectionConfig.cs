using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Inventory
{
    [Serializable]
    [CreateAssetMenu(menuName = "Game/GameItemCollectionConfig")]
    public class GameItemCollectionConfig : ScriptableObject
    {
        [SerializeField] private List<GameItemConfig> _gameItems;

        public GameItemConfig GetItemOfType(GameItemType type)
        {
            return _gameItems.FirstOrDefault(item => item.Name == type);
        }

        public List<GameItemConfig> GetAllItems()
        {
            return _gameItems;
        }
    }
}