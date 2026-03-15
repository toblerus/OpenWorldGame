using System.Collections.Generic;
using Injection;
using Inventory.Hotbar;
using ReactiveCore.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory.ItemPlacement
{
    public class ItemPlacementController {
        private ItemPlacementView _itemPlacementView;
        private readonly ReactiveTimer _timer = new(0.01f);
        private ItemPlacementView _view;
        private GameItemConfig _item;
        private int _layerMask;
        private const float MaxDistance = 3;
        
        private Dictionary<GameItemType, GameObject> _placeableItems = new();
        private GameItemConfig _currentGameItem;


        public void Setup(ItemPlacementView itemPlacementView)
        {
            _view = itemPlacementView;
            _layerMask = LayerMask.GetMask("Ground");
            var hotBarController = ServiceLocator.Resolve<HotBarController>();

            hotBarController.ActiveItemConfig
                .Subscribe(value =>
                {
                    _currentGameItem = value;
                    PreviewPlaceableItem(value);
                });
            
            _itemPlacementView = itemPlacementView;
            _timer.Elapsed.Subscribe(() => VisualizePlacement(Camera.main));
            _timer.Start();
        }

        public void VisualizePlacement(Camera camera)
        {
            var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
            if (Physics.Raycast(ray, out var hit, MaxDistance, _layerMask)) {
                var objectHit = hit.transform;

                _view.PlacementItemParent.gameObject.SetActive(true);
                _view.PlacementItemParent.transform.position = hit.point;
            }
            else
            {
                _view.PlacementItemParent.gameObject.SetActive(false);
            }
        }
        
        private void PreviewPlaceableItem(GameItemConfig config)
        {
            foreach (var item in _placeableItems)
            {
                item.Value.SetActive(false);
            }
            
            if (config == null || !config.IsPlaceable)
            {
                return;
            }

            if (_placeableItems.TryGetValue(config.Name, out var gameItem))
            {
                gameItem.SetActive(true);
            }
            else
            {
                if (config.Prefab != null)
                    _placeableItems.Add(config.Name, Object.Instantiate(config.Prefab, _view.PlacementItemParent));
            }
        }
    }
}