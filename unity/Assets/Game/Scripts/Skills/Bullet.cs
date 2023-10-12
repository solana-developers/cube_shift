using Frictionless;
using Game.Scripts;
using ToolBox.Pools;
using UnityEngine;

namespace Cubeshift
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        public int Damage;
        public float Speed;
        public GameObject Target;
        public GameObject Owner;
        public GameObject HitFx;
        public bool DestroyOnImpact;
        
        private Vector3 ShootDirection;
        private Vector3 StartPos;
        private Transform cachedTransform;

        private void Awake()
        {
            cachedTransform = transform;
            StartPos = cachedTransform.position;
        }

        public void OnReuse()
        {
            gameObject.SetActive(true);
            StartPos = cachedTransform.position;
        }

        public void OnRelease()
        {
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            cachedTransform.Translate(ShootDirection * Time.deltaTime * Speed);
            if ((StartPos - cachedTransform.transform.position).magnitude > 20)
            {
                if (DestroyOnImpact)
                {
                    gameObject.Release();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (ServiceFactory.Resolve<GameController>().IsGameOver)
            {
                return;
            }
            var enemy = other.GetComponent<BaseEnemy>();
            if (enemy == null)
            {
                return;
            }
            enemy.Hit(Damage, gameObject);
            
            var hitFx = HitFx.Reuse();
            hitFx.transform.position = other.transform.position;

            if (DestroyOnImpact)
            {
                gameObject.Release();   
            }
        }

        public void Init(GameObject owner, GameObject target, int skillConfigDamage, float skillConfigSpeed)
        {
            if (target != null)
            {
                ShootDirection = (target.transform.position - owner.transform.position).normalized;   
            }
            ShootDirection.y = 0;
            
            Owner = owner;
            Target = target;
            Damage = skillConfigDamage;
            Speed = skillConfigSpeed;
        }
    }
}