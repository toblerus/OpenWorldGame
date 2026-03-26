using System.Collections.Generic;
using _Scripts.Saving;
using UnityEngine;

namespace _Scripts.InWorld
{
    public class InWorldObjectPersistenceModel
    {
        private readonly List<GameObject> _inWorldObjects = new();

        public InWorldObjectPersistenceModel()
        {
            Load();
        }
        
        public void Register(InWorldObjectPersistenceView inWorldObjectPersistenceView)
        {
            _inWorldObjects.Add(inWorldObjectPersistenceView.gameObject);
            Save();
        }

        public void Unregister(InWorldObjectPersistenceView inWorldObjectPersistenceView)
        {
            _inWorldObjects.Remove(inWorldObjectPersistenceView.gameObject);
            Save();
        }

        private void Save()
        {
            ES3.Save(SavegameConstants.InWorldObjects, _inWorldObjects);
        }

        public void Load()
        {
            ES3.Load(SavegameConstants.InWorldObjects, _inWorldObjects);
        }
    }
}
