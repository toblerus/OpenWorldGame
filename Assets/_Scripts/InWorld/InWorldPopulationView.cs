using _Scripts.Injection;
using _Scripts.Inventory;
using UnityEngine;

namespace _Scripts.InWorld
{
    public class InWorldPopulationView : MonoBehaviour
    {
        [SerializeField] private Terrain _terrain;
        [SerializeField] private string _terrainId = "MainTerrain";
        [Tooltip("Seed and entries are saved on first generation. Existing worlds use their saved settings.")]
        [SerializeField] private int _seed = 12345;
        [SerializeField] private InWorldPopulationEntry[] _entries =
        {
            new() { ItemType = GameItemType.Cactus, Density = 2, MaxSlope = 35 }
        };

        public Terrain Terrain => _terrain;

        private void Start()
        {
            if (_terrain == null || _terrain.terrainData == null || string.IsNullOrWhiteSpace(_terrainId))
            {
                Debug.LogError("Population needs a terrain and a stable terrain ID.", this);
                return;
            }

            var model = new InWorldPopulationModel
            {
                TerrainId = _terrainId,
                Seed = _seed,
                Origin = _terrain.transform.position,
                Size = _terrain.terrainData.size,
                Entries = (InWorldPopulationEntry[])_entries.Clone()
            };
            ServiceLocator.Resolve<InWorldPopulationController>().Setup(this, model);
        }
    }
}
