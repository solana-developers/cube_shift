using Cubeshift;
using DG.Tweening;
using Frictionless;
using ToolBox.Pools;
using UnityEngine;

public class XpBlob : MonoBehaviour, IPoolable, IWorldObject
{
    public int Xp;
    public bool IsCollected;

    public void OnReuse()
    {
        IsCollected = false;
    }

    public void OnRelease()
    {
        
    }
    
    public void Init(int xp, Vector3 position)
    {
        Xp = xp;
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
            transform.position = Vector3.Slerp(transform.position, transformPosition, Time.deltaTime * 5);
            if ((transform.position - transformPosition).magnitude < 0.3f)
            {
                ServiceFactory.Resolve<PlayerController>().GainExp(Xp);
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

public interface IWorldObject
{
    bool OnUpdate();
    GameObject GetGameObject();
}
