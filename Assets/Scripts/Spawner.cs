using Unity.Entities;
using Unity.Mathematics;

public struct Spawner : IComponentData
{
    public Entity Prefab;
    public float2 spawnPos;
    public float nextSpawnTime;
    public float spawnRate;
    public int enemiesPerWave;
}
