using UnityEditor;
using UnityEngine;

public class WavyTerrainGenerator : EditorWindow
{
    private Terrain terrain;
    private float noiseScale = 0.01f;
    private float heightMultiplier = 10f;

    [MenuItem("Tools/Wavy Terrain Generator")]
    public static void ShowWindow()
    {
        GetWindow<WavyTerrainGenerator>("Wavy Terrain Generator");
    }

    private void OnGUI()
    {
        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);
        noiseScale = EditorGUILayout.Slider("Noise Scale", noiseScale, 0.001f, 0.1f);
        heightMultiplier = EditorGUILayout.Slider("Height Multiplier", heightMultiplier, 1f, 50f);

        if (GUILayout.Button("Generate Terrain"))
        {
            if (terrain != null)
            {
                GenerateTerrain();
            }
        }
    }

    private void GenerateTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;

        float[,] heights = new float[heightmapWidth, heightmapHeight];

        for (int x = 0; x < heightmapWidth; x++)
        {
            for (int y = 0; y < heightmapHeight; y++)
            {
                float xCoord = x * noiseScale;
                float yCoord = y * noiseScale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                heights[x, y] = sample * heightMultiplier / terrainData.size.y;
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
}