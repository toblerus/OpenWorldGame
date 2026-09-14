using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.Injection
{
    public class ViewFactory<TView> where TView : MonoBehaviour
    {
        private readonly TView _prefab;

        public ViewFactory(TView prefab)
        {
            if (prefab == null)
                throw new ArgumentNullException(nameof(prefab));

            _prefab = prefab;
        }

        public TView Create(Transform parent = null)
        {
            return Object.Instantiate(_prefab, parent, false);
        }

        public TView Create(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return Object.Instantiate(_prefab, position, rotation, parent);
        }
    }
}
