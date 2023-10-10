using System;
using System.Collections.Generic;
using Frictionless;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cubeshift
{
    [Serializable]
    public class SkillTrio
    {
        public List<SkillBlob> Skills = new List<SkillBlob>();
    } 
    
    public class SkillController : MonoBehaviour
    {
        public List<SkillConfig> SkillConfigs;
        public List<BaseSkill> ActiveSkills;
        // This is so we can delete the other two skills when one of the three is collected
        public List<SkillTrio> SkillTrios = new List<SkillTrio>();
        public SkillBlob SkillBlob;
        
        private void Awake()
        {
            ServiceFactory.RegisterSingleton(this);
            //var skill = Instantiate(SkillConfigs[0].SkillPrefab, transform);
            //ActiveSkills.Add(skill.GetComponent<BaseSkill>());
        }

        private SkillConfig GetSkillConfigByType(SkillType skillType)
        {
            foreach (var skillConfig in SkillConfigs)
            {
                if (skillConfig.SkillType == skillType)
                {
                    return skillConfig;
                }
            }
            Debug.LogError("Tried to add a skill that didnt have a config.");
            return null;
        }
        
        public void AddSkillByType(SkillType skillType)
        {
            var skillConfig = GetSkillConfigByType(skillType);
            
            var baseSkill = ActiveSkills.Find(s => s.SkillType == skillConfig.SkillType);
            if (baseSkill != null)
            {
                if (baseSkill.Level < skillConfig.AttributesConfigs.Count - 1)
                {
                    baseSkill.Level++;   
                    baseSkill.OnLevelUp();
                    ServiceFactory.Resolve<BlimpManager>().SpawnBlimp(transform.position, baseSkill.SkillType + " Level " + baseSkill.Level, Color.green);
                }
                else
                {
                    // TODO: Handle collected skill when max is reached! 
                    Debug.LogWarning("Max skill level reached");
                }
            }
            else
            {
                var newSkill = Instantiate(skillConfig.SkillPrefab, transform);
                ActiveSkills.Add(newSkill.GetComponent<BaseSkill>());

                ServiceFactory.Resolve<BlimpManager>().SpawnBlimp(transform.position, skillConfig.SkillType + " Level 1", Color.green);
            }
        }

        
        public void AddSkillFromBlob(SkillBlob skillBlob)
        {
            AddSkillByType(skillBlob.SkillConfig.SkillType);
            // Remove the other two skills from that skill trio 
            for (int i = 0; i < SkillTrios.Count; i++)
            {
                var skillTrio = SkillTrios[i];
                bool destroy = false;
                foreach (var skill in skillTrio.Skills)
                {
                    if (skill == skillBlob)
                    {
                        destroy = true;
                    }
                }

                if (destroy)
                {
                    for (int j = 0; j < skillTrio.Skills.Count; j++)
                    {
                        var skill = skillTrio.Skills[j];
                        Destroy(skill.gameObject);
                    }
                    SkillTrios.RemoveAt(i);
                }
            }
        }

        public void SpawnCollectableSkill()
        {
            List<Vector3> positions = new List<Vector3>();
            var transformPosition = transform.position;
            
            positions.Add(transformPosition + Vector3.forward * 5 * (SkillTrios.Count +1));
            positions.Add(transformPosition + Vector3.left * 5 * (SkillTrios.Count +1));
            positions.Add(transformPosition + Vector3.right * 5 * (SkillTrios.Count +1));
            var skillTrio = new SkillTrio();
            
            SkillTrios.Add(skillTrio);
            for (int i = 0; i < 3; i++)
            {
                var randomSkillFromConfig = SkillConfigs[Random.Range(0, SkillConfigs.Count)];
                var baseSkill = ActiveSkills.Find(s => s.SkillType == randomSkillFromConfig.SkillType);
                if (baseSkill != null)
                {
                    if (baseSkill.Level < randomSkillFromConfig.AttributesConfigs.Count - 1)
                    {
                        var skillBlob = Instantiate(SkillBlob);
                        skillBlob.Init(randomSkillFromConfig);
                        skillBlob.transform.position = positions[i];
                        skillTrio.Skills.Add(skillBlob);
                    }
                    else
                    {
                        Debug.LogWarning("Max skill level reached. Get coins? Ask dev");
                        // TODO: Create replacement skill like coins or smth
                    }
                }
                else
                {
                    var skillBlob = Instantiate(SkillBlob);
                    skillBlob.Init(randomSkillFromConfig);
                    skillBlob.transform.position = positions[i];
                    skillTrio.Skills.Add(skillBlob);
                }
            }
        }
    }
}