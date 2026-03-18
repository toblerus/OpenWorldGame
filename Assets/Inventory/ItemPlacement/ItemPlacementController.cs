using System.Collections.Generic;
using System.Linq;
using Injection;
using Inventory.Hotbar;
using InWorld;
using ReactiveCore.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory.ItemPlacement
{
    public class ItemPlacementController {
        private ItemPlacementView _itemPlacementView;
        private readonly ReactiveTimer _timer = new(0.001f);
        private ItemPlacementView _view;
        private GameItemConfig _item;
        private int _layerMask;
        private const float MaxDistance = 3;
        private static Color HighlightColor = Color.white;


        private Dictionary<GameItemType, GameObject> _placeableItems = new();
        private GameItemConfig _currentGameItem;
        private GameObject _activeItem;
        private HotBarController _hotBarController;
        private Vector3 _targetPosition;
        private Vector3 _velocity;
        private float _smoothTime = 0.1f;



        public void Setup(ItemPlacementView itemPlacementView)
        {
            _view = itemPlacementView;
            _layerMask = LayerMask.GetMask("Ground");
            ColorUtility.TryParseHtmlString("00FF62", out HighlightColor);
            _hotBarController = ServiceLocator.Resolve<HotBarController>();

            _hotBarController.ActiveItemConfig
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

                _view.PlacementItemPreviewParent.gameObject.SetActive(true);
                
                _targetPosition = hit.point;

                _view.PlacementItemPreviewParent.transform.position =
                    Vector3.SmoothDamp(
                        _view.PlacementItemPreviewParent.transform.position,
                        _targetPosition,
                        ref _velocity,
                        _smoothTime
                    );
            }
            else
            {
                _view.PlacementItemPreviewParent.gameObject.SetActive(false);
            }
        }
        
        private void PreviewPlaceableItem(GameItemConfig config)
        {
            foreach (var item in _placeableItems)
            {
                item.Value.SetActive(false);
                _activeItem = null;
            }
            
            if (config == null || !config.IsPlaceable)
            {
                _activeItem = null;
                return;
            }

            if (_placeableItems.TryGetValue(config.Name, out var gameItem))
            {
                gameItem.SetActive(true);
            }
            else
            {
                gameItem = Object.Instantiate(config.Prefab, _view.PlacementItemPreviewParent);
                if (config.Prefab != null)
                    _placeableItems.Add(config.Name, gameItem);
            }
            _activeItem = gameItem;
        }

        public void PlaceItem()
        {
            if(_activeItem == null) return;
            _activeItem.transform.SetParent(_view.PlacementParent);
            _placeableItems.Clear();
            var item = _activeItem;
            _activeItem = null;
            _hotBarController.RemoveActiveItem();
            item.SetActive(true);
            item.AddComponent<InWorldObjectPersistenceView>();
        }
    }
}