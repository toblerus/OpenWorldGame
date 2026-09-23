using System.Collections.Generic;
using _Scripts.Injection;
using _Scripts.Inventory;
using UnityEngine;

namespace _Scripts.InWorld
{
    public class InWorldPopulationController
    {
        public void Setup(InWorldPopulationView view, InWorldPopulationModel settings)
        {
            var persistence = ServiceLocator.Resolve<InWorldObjectPersistenceModel>();
            var model = persistence.LoadPopulation(settings);
            var terrain = view.Terrain;
            var data = terrain.terrainData;

            if (model.Origin != terrain.transform.position || model.Size != data.size)
            {
                Debug.LogError("The terrain bounds changed. Start a new world before generating its population.", view);
                return;
            }

            var itemTypes = new HashSet<GameItemType>();
            foreach (var entry in model.Entries)
            {
                if (!itemTypes.Add(entry.ItemType))
                {
                    Debug.LogError($"Duplicate population entry for {entry.ItemType}.", view);
                    continue;
                }
                if (!ServiceLocator.IsBound<ViewFactory<InWorldResourceView>>(entry.ItemType))
                {
                    Debug.LogWarning($"No population factory configured for {entry.ItemType}.", view);
                    continue;
                }

                var factory = ServiceLocator.Resolve<ViewFactory<InWorldResourceView>>(entry.ItemType);
                var count = Mathf.FloorToInt(model.Size.x * model.Size.z * Mathf.Max(0, entry.Density) / 1000f);
                for (var index = 0; index < count; index++)
                {
                    var id = model.GetObjectId(entry.ItemType, index);
                    if (persistence.IsHarvested(id)) continue;

                    var sample = model.GetSample(entry.ItemType, index);
                    var holeX = Mathf.Min((int)(sample.x * data.holesResolution), data.holesResolution - 1);
                    var holeZ = Mathf.Min((int)(sample.z * data.holesResolution), data.holesResolution - 1);
                    if (data.IsHole(holeX, holeZ) || data.GetSteepness(sample.x, sample.z) > entry.MaxSlope)
                        continue;

                    var position = model.Origin + new Vector3(sample.x * model.Size.x,
                        data.GetInterpolatedHeight(sample.x, sample.z), sample.z * model.Size.z);
                    var rotation = Quaternion.Euler(0, sample.y * 360, 0);
                    var resource = factory.Create(position, rotation, view.transform);
                    resource.Setup(id);
                }
            }
        }
    }
}
