using Frictionless;
using UnityEngine;

public class CircleEnemySpawner : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject RotationRoot;
    public float Radius;
    public int AmountEnemies = 30;

    private void Start()
    {
        SpawnEnemyCircle(Prefab, AmountEnemies, Radius);
    }

    public void SpawnEnemyCircle(GameObject prefab, int amount, float radius)
    {
        for (int i = 0; i < amount; i++)
        {
            float angle = i * (360f / amount);
            Vector3 positionOffset = Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
            var position = RotationRoot.transform.position;
            Vector3 newPosition = position + positionOffset;

            var enemy = ServiceFactory.Resolve<EnemySpawner>().SpawnNewEnemy(prefab, newPosition);
            enemy.transform.SetParent(RotationRoot.transform, true);
            enemy.transform.position = newPosition;
            enemy.transform.LookAt(position);
        }
    }
}

