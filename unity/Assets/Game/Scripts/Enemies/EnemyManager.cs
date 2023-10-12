using System;
using System.Collections.Generic;
using Frictionless;
using Game.Scripts;
using ToolBox.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cubeshift
{
    public class EnemyManager : MonoBehaviour
    {
        public List<BaseEnemy> AllEnemies = new List<BaseEnemy>();
        public List<IWorldObject> AllWorldObjectBlobs = new List<IWorldObject>();

        public GameObject XpBlob;
        public GameObject CoinsXpBlob;

        public int StartXpBlobs = 10;
        public float StartXpBlobRadius = 20;
        
        public void Awake()
        {
            ServiceFactory.RegisterSingleton(this);
            XpBlob.Populate(20);
        }

        public void SpawnStartXpBlobs()
        {
            for (int i = 0; i < StartXpBlobs; i++)
            {
                var insideUnitCircle = Random.insideUnitCircle;
                SpawnXpBlob(5, new Vector3(insideUnitCircle.x, 0, insideUnitCircle.y) * StartXpBlobRadius);
            }
        }
        
        public void AddEnemy(BaseEnemy enemy)
        {
            AllEnemies.Add(enemy);
        }

        public void RemoveEnemy(BaseEnemy enemy)
        {
            AllEnemies.Remove(enemy);
        }

        public void AddWorldObjectBlob(IWorldObject xp)
        {
            AllWorldObjectBlobs.Add(xp);
        }

        public void RemoveWorldObject(IWorldObject xp)
        {
            xp.GetGameObject().Release();
            AllWorldObjectBlobs.Remove(xp);
        }

        public void SpawnXpBlobByEnemy(BaseEnemy enemy)
        {
            SpawnXpBlob(enemy.Xp, enemy.transform.position);
        }

        private void SpawnXpBlob(int xp, Vector3 position)
        {
            var newXpBlob = XpBlob.Reuse<XpBlob>();
            newXpBlob.Init(xp, position);
            AddWorldObjectBlob(newXpBlob);
        }

        public void SpawnCoinsBlob(int coins, Vector3 position)
        {
            var newCoinBlob = CoinsXpBlob.Reuse<CoinBlob>();
            newCoinBlob.Init(coins, position);
            AddWorldObjectBlob(newCoinBlob);
        }

        public BaseEnemy GetClosestEnemy(Vector3 transformPosition)
        {
            BaseEnemy closestEnemy = null;
            float closestDistance = Single.MaxValue;
            
            foreach (var enemy in AllEnemies)
            {
                var magnitude = (transformPosition - enemy.transform.position).magnitude;
                if (magnitude < closestDistance)
                {
                    closestDistance = magnitude;
                    closestEnemy = enemy;
                }
            }

            return closestEnemy;
        }
        
        public void FixedUpdate()
        {
            if (ServiceFactory.Resolve<GameController>().IsGameOver)
            {
                return;
            }
            
            for (var index = AllEnemies.Count - 1; index >= 0; index--)
            {
                var enemy = AllEnemies[index];
                enemy.OnUpdate();
            }

            for (int i = AllWorldObjectBlobs.Count - 1; i >= 0; i--)
            {
                var xpBlob = AllWorldObjectBlobs[i];
                // This makes the collecting a bit cooler because it can only collect on per frame
                if (!xpBlob.OnUpdate())
                {
                    break;
                }
            }
        }
    }
}