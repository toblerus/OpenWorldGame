using System;
using _Scripts.Inventory;
using UnityEngine;

namespace _Scripts.InWorld
{
    [Serializable]
    public class InWorldPopulationModel
    {
        public string TerrainId;
        public int Seed;
        public Vector3 Origin;
        public Vector3 Size;
        public InWorldPopulationEntry[] Entries;

        public string GetObjectId(GameItemType itemType, int index)
        {
            return FormattableString.Invariant($"{TerrainId}/{(int)itemType}/{index}");
        }

        // Each candidate has its own random sequence, independent of spawn order and harvesting.
        public Vector3 GetSample(GameItemType itemType, int index)
        {
            var state = unchecked(2166136261u ^ (uint)Seed);
            foreach (var character in GetObjectId(itemType, index))
                state = unchecked((state ^ character) * 16777619u);

            if (state == 0) state = 1;
            return new Vector3(Next(ref state), Next(ref state), Next(ref state));
        }

        private static float Next(ref uint state)
        {
            state ^= state << 13;
            state ^= state >> 17;
            state ^= state << 5;
            return (state >> 8) / 16777216f;
        }
    }

    [Serializable]
    public struct InWorldPopulationEntry
    {
        public GameItemType ItemType;
        [Min(0)] [Tooltip("Spawn candidates per 1,000 square metres.")]
        public float Density;
        [Range(0, 90)] public float MaxSlope;
    }
}
