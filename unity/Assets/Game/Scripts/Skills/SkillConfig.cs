using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cubeshift
{
    public enum SkillType
    {
        SingleShotSkill = 0,
        RotatingShotSkill = 1,
        RegenerationSkill = 2,
    } 
    
   [Serializable]
   [CreateAssetMenu(menuName = "CubeConfigs/SkillConfig")]
   public class SkillConfig : ScriptableObject
   {
        public SkillType SkillType;
        public GameObject SkillPrefab;
        public Sprite Icon;
        public bool IsPassive;
        public List<AttributesConfig> AttributesConfigs;
   }
   
   [Serializable]
   public class AttributesConfig
   {
       public int Damage;
       public float Cooldown;
       public float Speed;
       public float Range;
       public int HealthPerTick;
       public int Bullets;
       public int ExtraBullets;
       public float HealthPercentage = 1;
       public float DamagePercentage = 1;
       public float MoveSpeedPercentage = 1;
       public float XpPercentage = 1;
   }
}