using Frictionless;
using UnityEngine;

public class Boss_01 : MonoBehaviour
{
    public GameObject Boss;

    public void Start()
    {
        ServiceFactory.Resolve<EnemySpawner>().SpawnNewEnemy(Boss, transform.position);
    }
}
