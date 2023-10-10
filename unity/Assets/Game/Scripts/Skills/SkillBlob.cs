using Cubeshift;
using DG.Tweening;
using Frictionless;
using UnityEngine;

public class SkillBlob : MonoBehaviour
{
    public SkillConfig SkillConfig;
    public bool IsCollected;
    public SpriteRenderer Sprite;
    
    public void Init(SkillConfig skillConfig)
    {
        SkillConfig = skillConfig;
        Sprite.sprite = skillConfig.Icon;
    }
    
    public void TriggerCollect()
    {
        IsCollected = true;
        transform.DOLocalJump(ServiceFactory.Resolve<PlayerController>().transform.position, 1, 1, 0.5f);
    }

    public void Update()
    {
        if (IsCollected)
        {
            var transformPosition = ServiceFactory.Resolve<PlayerController>().transform.position;
            transform.position = Vector3.Slerp(transform.position, transformPosition, Time.deltaTime * 15);
            if ((transform.position - transformPosition).magnitude < 1f)
            {
                ServiceFactory.Resolve<SkillController>().AddSkillFromBlob(this);
            }
            return;
        }

        var playerController = ServiceFactory.Resolve<PlayerController>();
        if ((transform.position - playerController.CachedTransform.position).magnitude < 2)
        {
            TriggerCollect();
        }
    }
}
