using Cubeshift;
using DG.Tweening;
using Frictionless;
using ToolBox.Pools;
using UnityEngine;

public class CoinBlob : MonoBehaviour, IPoolable, IWorldObject
{
    public int Coins = 10;
    public bool IsCollected;
    
    public void OnReuse()
    {
        IsCollected = false;
    }

    public void OnRelease()
    {
        
    }
    
    public void Init(int coins, Vector3 position)
    {
        Coins = coins;
        transform.position = position;
    }

    public void TriggerCollect()
    {
        IsCollected = true;
        transform.DOLocalJump(ServiceFactory.Resolve<PlayerController>().transform.position, 1, 1, 0.5f);
    }

    public bool OnUpdate()
    {
        if (IsCollected)
        {
            var transformPosition = ServiceFactory.Resolve<PlayerController>().transform.position;
            transform.position = Vector3.Slerp(transform.position, transformPosition, Time.deltaTime * 30);
            if ((transform.position - transformPosition).magnitude < 0.3f)
            {
                ServiceFactory.Resolve<PlayerController>().GainCoin(Coins);
                ServiceFactory.Resolve<EnemyManager>().RemoveWorldObject(this);
                return false;
            }
        }

        var playerController = ServiceFactory.Resolve<PlayerController>();
        if ((transform.position - playerController.CachedTransform.position).magnitude <
            playerController.CollectRange && !IsCollected)
        {
            TriggerCollect();
        }

        return true;
    }
    
    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
