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
        public List<XpBlob> AllXpBlobs = new List<XpBlob>();

        public GameObject XpBlob;

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

        public void AddXpBlob(XpBlob xp)
        {
            AllXpBlobs.Add(xp);
        }

        public void RemoveXpBlob(XpBlob xp)
        {
            xp.gameObject.Release();
            AllXpBlobs.Remove(xp);
        }

        public void SpawnXpBlobByEnemy(BaseEnemy enemy)
        {
            SpawnXpBlob(enemy.Xp, enemy.transform.position);
        }

        private void SpawnXpBlob(int xp, Vector3 position)
        {
            var newXpBlob = XpBlob.Reuse<XpBlob>();
            newXpBlob.Init(xp, position);
            AddXpBlob(newXpBlob);
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

            for (int i = AllXpBlobs.Count - 1; i >= 0; i--)
            {
                var xpBlob = AllXpBlobs[i];
                // This makes the collecting a bit cooler because it can only collect on per frame
                if (!xpBlob.OnUpdate())
                {
                    break;
                }
            }
        }
    }
}