using System;
using System.Collections.Generic;
using Cubeshift;
using Frictionless;
using ToolBox.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WaveConfigs
{
    public List<WaveConfig> Configs = new List<WaveConfig>();
}

[Serializable]
public class WaveConfig
{
    public List<EnemySpawnConfig> EnemySpawnConfigs = new List<EnemySpawnConfig>();
    public float Cooldown;
    public int Level;
    public float TimeToStart;
    public bool IsBossLevel;
    public GameObject BossPrefab;
}

[Serializable]
public class EnemySpawnConfig
{
    public GameObject Enemy;
    public float Chance;
}

public class EnemySpawner : MonoBehaviour
{
    public WaveConfigs WaveConfigs;
    public WaveConfig CurrentWaveConfig;

    public float LastSpawnedEnemy = Single.MinValue;

    public float Radius = 5;
    public bool Spawning;
    public float StartTime;
    public BaseEnemy ActiveBoss;

    private void Awake()
    {
        ServiceFactory.RegisterSingleton(this);
        CurrentWaveConfig = WaveConfigs.Configs[0];
    }

    public void StartSpawning()
    {
        Spawning = true;
        StartTime = Time.time;
    }
    
    public void StopSpawning()
    {
        Spawning = false;
    }
    
    public void Update()
    {
        if (!Spawning)
        {
            return;
        }

        var lastConfig = CurrentWaveConfig;
        CurrentWaveConfig = GetWaveConfigByTime(Time.time - StartTime);

        if (lastConfig != CurrentWaveConfig)
        {
            Debug.Log("New wave " + CurrentWaveConfig.Level);
            if (CurrentWaveConfig.IsBossLevel)
            {
                ServiceFactory.Resolve<PlayerMovement>().transform.position = Vector3.zero;
                ActiveBoss = ServiceFactory.Resolve<EnemySpawner>().SpawnNewEnemy(CurrentWaveConfig.BossPrefab, new Vector3(0,0, 40));
                Debug.Log("BOSS LEVEL" + CurrentWaveConfig.Level);
            }
        }
        
        if (LastSpawnedEnemy + CurrentWaveConfig.Cooldown < Time.time)
        {
            LastSpawnedEnemy = Time.time;
            SpawnNewEnemyFromWaveConfig();
        }
    }

    public BaseEnemy SpawnNewEnemy(GameObject enemyPrefab, Vector3 position)
    {
        var enemy = enemyPrefab.Reuse<BaseEnemy>(position, transform.rotation);
        // TODO: Add Enemy Config
        enemy.CurrentHealth = enemy.StartHealth;
        enemy.gameObject.SetActive(true);
        enemy.Target = ServiceFactory.Resolve<PlayerController>().gameObject;
        ServiceFactory.Resolve<EnemyManager>().AddEnemy(enemy);
        return enemy;
    }

    private void SpawnNewEnemyFromWaveConfig()
    {
        var insideUnitCircle = Random.insideUnitCircle * new Vector2(Radius, Radius);

        var enemyPrefab = PickRandomEnemyFromWaveConfig(ServiceFactory.Resolve<PlayerController>().CurrentLevel);
            
        var enemy = enemyPrefab.gameObject.Reuse<BaseEnemy>(transform.position + new Vector3(insideUnitCircle.x, 0, insideUnitCircle.y) , transform.rotation);
        // TODO: Add Enemy Config
        enemy.CurrentHealth = enemy.StartHealth;
        enemy.gameObject.SetActive(true);
        enemy.Target = ServiceFactory.Resolve<PlayerController>().gameObject;
        ServiceFactory.Resolve<EnemyManager>().AddEnemy(enemy);
    }
    
    WaveConfig GetWaveConfigByTime(float time)
    {
        for (var index = WaveConfigs.Configs.Count - 1; index >= 0; index--)
        {
            WaveConfig waveConfig = WaveConfigs.Configs[index];
            if (waveConfig.TimeToStart <= time)
            {
                return waveConfig;
            }
        }

        return WaveConfigs.Configs[WaveConfigs.Configs.Count - 1]; // Use the last config for the rest of the game
    }

    
    WaveConfig GetWaveConfigForLevel(int level)
    {
        foreach (WaveConfig waveConfig in WaveConfigs.Configs)
        {
            if (waveConfig.Level >= level)
            {
                return waveConfig;
            }
        }

        return WaveConfigs.Configs[WaveConfigs.Configs.Count - 1]; // Use the last config for the rest of the game
    }
    
    GameObject PickRandomEnemyFromWaveConfig(int waveIndex)
    {
        var waveConfig = GetWaveConfigForLevel(waveIndex);
        if (waveConfig != null)
        {
            float totalChance = CalculateTotalChance(waveConfig.EnemySpawnConfigs);

            if (totalChance <= 0)
            {
                Debug.LogWarning($"Total chance is <= 0 for Wave {waveIndex}");
                return null;
            }

            float randomValue = Random.Range(0f, totalChance);

            foreach (EnemySpawnConfig enemySpawnConfig in waveConfig.EnemySpawnConfigs)
            {
                if (randomValue <= enemySpawnConfig.Chance)
                {
                    return enemySpawnConfig.Enemy;
                }
                randomValue -= enemySpawnConfig.Chance;
            }
        }
        else
        {
            Debug.LogWarning($"Invalid wave index: {waveIndex}");
        }

        return null; // No enemy selected
    }

    float CalculateTotalChance(List<EnemySpawnConfig> spawnConfigs)
    {
        float totalChance = 0f;

        foreach (EnemySpawnConfig enemySpawnConfig in spawnConfigs)
        {
            totalChance += enemySpawnConfig.Chance;
        }

        return totalChance;
    }
}
