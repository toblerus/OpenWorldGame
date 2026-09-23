using _Scripts.Injection;
using UnityEngine;
using System;

namespace _Scripts.Inventory
{
    public class ItemDropSpawnerModel
    {
        private readonly ViewFactory<ItemDropView> _viewFactory;
        private readonly Transform _dropOrigin;

        public ItemDropSpawnerModel(Transform dropOrigin)
        {
            if (dropOrigin == null) throw new ArgumentNullException(nameof(dropOrigin));
            _dropOrigin = dropOrigin;
            _viewFactory = ServiceLocator.Resolve<ViewFactory<ItemDropView>>();
        }

        public void Spawn(GameItemConfig itemConfig, int amount)
        {
            Spawn(itemConfig, amount, _dropOrigin.position + _dropOrigin.forward);
        }
        
        public void Spawn(GameItemConfig itemConfig, int amount, Vector3 position)
        {
            if (itemConfig == null || amount <= 0) return;

            var view = _viewFactory.Create(position, Quaternion.identity);
            view.Setup(new ItemDropModel(itemConfig, amount));
        }
    }
}
