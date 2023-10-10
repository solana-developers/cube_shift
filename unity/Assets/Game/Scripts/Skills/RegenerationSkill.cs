using System.Collections;
using Frictionless;
using ToolBox.Pools;
using UnityEngine;

namespace Cubeshift
{
    public class RegenerationSkill : BaseSkill
    {
        public SkillConfig SkillConfig;

        private float lastTimeTriggered;
        
        public void Update()
        {
            var skillConfig = SkillConfig.AttributesConfigs[Level];
            if (lastTimeTriggered + skillConfig.Cooldown < Time.time)
            {
                lastTimeTriggered = Time.time;
                ServiceFactory.Resolve<PlayerController>().Hit(skillConfig.Damage);
            }
        }
    }
}