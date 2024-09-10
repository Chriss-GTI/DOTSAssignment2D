using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Burst;
using Random = UnityEngine.Random;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<Spawner> spawner in SystemAPI.Query<RefRW<Spawner>>())
        {
            if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
            {
                Camera mainCamera = Camera.main;
                if (mainCamera == null & !mainCamera.orthographic) return;

                float cameraHeight = mainCamera.orthographicSize;
                float cameraWidth = cameraHeight * mainCamera.aspect;
                float minX = mainCamera.transform.position.x - cameraWidth;
                float maxX = mainCamera.transform.position.x + cameraWidth;
                float minY = mainCamera.transform.position.y - cameraHeight;
                float maxY = mainCamera.transform.position.y + cameraHeight;

                for (int i = 0; i < spawner.ValueRO.enemiesPerWave; i++)
                {
                    float randomX = Random.Range(minX, maxX);
                    float randomY = Random.Range(minY, maxY);
                    float3 finalSpawnPos = new float3(randomX, randomY, 0f);

                    Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);
                    state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(finalSpawnPos));
                }

                spawner.ValueRW.nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            }
        }
    }
}