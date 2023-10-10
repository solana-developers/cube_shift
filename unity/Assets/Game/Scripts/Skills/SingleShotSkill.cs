using System.Collections;
using Frictionless;
using ToolBox.Pools;
using UnityEngine;

namespace Cubeshift
{
    public class SingleShotSkill : BaseSkill
    {
        [SerializeField] private Bullet Bullet = null;

        public SkillConfig SkillConfig;

        private float lastShotFired;
        private GameObject owner;
        private Transform cachedTransform;
        
        private void Awake()
        {
            Bullet.gameObject.Populate(count: 20);
            owner = gameObject.transform.parent.gameObject;
            cachedTransform = transform;
        }
        
        private void OnDestroy()
        {
            Bullet.gameObject.Clear(true);
        }

        public void Update()
        {
            var skillConfig = SkillConfig.AttributesConfigs[Level];
            if (lastShotFired + skillConfig.Cooldown < Time.time)
            {
                BaseEnemy closestEnemy = ServiceFactory.Resolve<EnemyManager>().GetClosestEnemy(transform.position);
                if (closestEnemy != null)
                {
                    StartCoroutine(Shoot(owner, closestEnemy.gameObject));
                    lastShotFired = Time.time;
                }
            }
        }

        public IEnumerator Shoot(GameObject owner, GameObject target)
        {
            var skillConfig = SkillConfig.AttributesConfigs[Level];

            for (int i = 0; i < 1 + skillConfig.Bullets; i++)
            {
                var bullet = Bullet.gameObject.Reuse<Bullet>(cachedTransform.position, cachedTransform.rotation);
                bullet.gameObject.SetActive(false);
                bullet.Init(owner, target, skillConfig.Damage, skillConfig.Speed);
                bullet.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}