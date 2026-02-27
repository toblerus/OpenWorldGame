using System.Collections.Generic;
using Injection;
using Inventory.Hotbar;
using UnityEngine;

namespace Inventory.HandItem
{
    public class HandItemController
    {
        private Dictionary<GameItemType, GameObject> _handItems = new();
        private HandItemView _view;

        public void Setup(HandItemView handItemView)
        {
            _view = handItemView;
            
            var hotBarController = ServiceLocator.Resolve<HotBarController>();

            hotBarController.ActiveItemConfig
                .Subscribe(EnableType);
        }

        private void EnableType(GameItemConfig config)
        {
            foreach (var item in _handItems)
            {
                item.Value.SetActive(false);
            }

            if(config == null) return;
            if (_handItems.TryGetValue(config.Name, out var gameItem))
            {
                gameItem.SetActive(true);
            }
            else
            {
                _handItems.Add(config.Name, Object.Instantiate(config.InHandPrefab, _view.ItemParent));
            }
        }
    }
}