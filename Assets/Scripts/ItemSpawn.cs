﻿using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemSpawn : MonoBehaviour
{
    [Range(0, 1)]
    public float spawnProbability = 0.2f;
    public Item[] items;
    [Range(0,100)]
    public int maxSpawnRate = 1;

    private float[] spawnWeights;
    private float spawnedLastMinute;

    [System.Serializable]
    public struct Item
    {
        public Collectable prefab;
        [Range(0,1)]
        public float probability;
    }

    void Awake()
    {
        spawnWeights = NormalizedSpawnWeight();
    }

    void Update()
    {
        if (spawnedLastMinute > 0)
        {
            spawnedLastMinute -= Time.deltaTime / 60;
        }
    }

    public void Spawn(int x, int y)
    {
        if (spawnedLastMinute > maxSpawnRate) return;

        var item = GetSpawnItem(spawnWeights);
        var itemGameObject = Instantiate(item.prefab, new Vector3(x, y, 0), Quaternion.identity);
        itemGameObject.transform.SetParent(transform);
        spawnedLastMinute++;
    }

    public void Spawn(Tilemap tilemap)
    {
        var gridSize = tilemap.size;
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                var tilePos = new Vector3Int(x, y, 0);
                var tile = tilemap.GetTile(tilePos);
                if (tile == null) continue;
                if (tile.name == "Floor")
                {
                    float random = Random.Range(0, 1f);
                    if (random < spawnProbability)
                    {
                        var pos = new Vector3(x + 0.5f, y - 0.5f, -1);
                        var item = GetSpawnItem(spawnWeights);
                        var enemyGameObject = Instantiate(item.prefab, pos, Quaternion.identity);
                        enemyGameObject.transform.SetParent(transform);
                    }
                }
            }
        }
    }

    private Item GetSpawnItem(float[] weights)
    {
        float rand = Random.Range(0, 1f);
        var item = new Item();
        double cumulative = 0.0;
        for (int i = 0; i < items.Length; i++)
        {
            cumulative += weights[i];
            if (rand < cumulative)
            {
                item = items[i];
            }
        }

        return item;
    }

    private float[] NormalizedSpawnWeight()
    {
        var weights = new float[items.Length];
        var weightSum = 0f;
        foreach (Item item in items)
        {
            weightSum += item.probability;
        }

        for (int i = 0; i < items.Length; i++)
        {
            weights[i] = items[i].probability / weightSum;
        }

        return weights;
    }
}
