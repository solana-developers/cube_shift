using System.Collections.Generic;
using ToolBox.Pools;
using UnityEngine;

namespace Cubeshift
{
    public class RotationShotSkill : BaseSkill
    {
        [SerializeField] private Bullet Bullet = null;

        public SkillConfig SkillConfig;
        public GameObject RotationRoot;
        public float rotationSpeed = 150;
        public float rotationDistance = 3;

        private float lastShotFired;
        private GameObject owner;
        private List<Bullet> bullets = new List<Bullet>();
        
        private void Awake()
        {
            Bullet.gameObject.Populate(count: 10);
            owner = gameObject.transform.parent.gameObject;

            OnLevelUp();
        }

        private void SpawnBullets()
        {
            var skillConfig = SkillConfig.AttributesConfigs[Level];

            var skillConfigExtraBullets = skillConfig.Bullets;
            for (int i = 0; i < skillConfigExtraBullets; i++)
            {
                float angle = i * (360f / skillConfigExtraBullets);
                Vector3 positionOffset = Quaternion.Euler(0, angle, 0) * Vector3.forward * rotationDistance;
                Vector3 newPosition = RotationRoot.transform.position + positionOffset;

                // Instantiate or move your game objects to the new positions
                var bullet = Bullet.gameObject.Reuse<Bullet>(RotationRoot.transform);

                bullet.gameObject.SetActive(false);
                bullet.Init(owner, null, skillConfig.Damage, skillConfig.Speed);
                bullet.transform.position = newPosition;
                bullet.gameObject.SetActive(true);
                Debug.Log("Banana spawned");
                bullets.Add(bullet);
            }
        }

        public override void OnLevelUp()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                var bullet = bullets[i];
                bullet.gameObject.Release();  
            }
            bullets.Clear();

            SpawnBullets();
        }
        
        private void OnDestroy()
        {
            Bullet.gameObject.Clear(true);
        }

        public void Update()
        {
            var skillConfig = SkillConfig.AttributesConfigs[Level];

            RotationRoot.transform.Rotate(Vector3.up, (rotationSpeed + skillConfig.Speed) * Time.deltaTime);
        }
    }
}