using System.Collections.Generic;
using _Scripts.Saving;
using UnityEngine;

namespace _Scripts.InWorld
{
    public class InWorldObjectPersistenceModel
    {
        private List<GameObject> _inWorldObjects = new();
        private HashSet<string> _harvestedObjects = new();

        public InWorldObjectPersistenceModel()
        {
            Load();
        }
        
        public void Register(InWorldObjectPersistenceView inWorldObjectPersistenceView)
        {
            if (_inWorldObjects.Contains(inWorldObjectPersistenceView.gameObject)) return;
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
            _inWorldObjects = ES3.Load(SavegameConstants.InWorldObjects, new List<GameObject>());
            _harvestedObjects = new HashSet<string>(ES3.Load(SavegameConstants.HarvestedObjects, new List<string>()));
        }

        public InWorldPopulationModel LoadPopulation(InWorldPopulationModel defaults)
        {
            var key = $"{SavegameConstants.InWorldPopulation}/{defaults.TerrainId}";
            if (ES3.KeyExists(key)) return ES3.Load<InWorldPopulationModel>(key);

            ES3.Save(key, defaults);
            return defaults;
        }

        public bool IsHarvested(string id)
        {
            return _harvestedObjects.Contains(id);
        }

        public void MarkHarvested(string id)
        {
            if (!_harvestedObjects.Add(id)) return;
            ES3.Save(SavegameConstants.HarvestedObjects, new List<string>(_harvestedObjects));
        }
    }
}
