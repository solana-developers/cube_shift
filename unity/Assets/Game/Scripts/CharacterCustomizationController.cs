using System;
using System.Collections.Generic;
using Cubeshift;
using Frictionless;
using UnityEngine;

public enum CharacterId
{
    Default = 0,
    Monkey = 1,
    FireMage = 2
}

[Serializable]
public class CharacterCustomizationConfig
{
    public CharacterId CharacterId;
    public GameObject GraphicsPrefab;
    public List<ExtraSkillConfig> ExtraSkills;
    public AttributesConfig ExtraAttributes;
}

[Serializable]
public class ExtraSkillConfig
{
    public SkillType SkillType;
    public int Amount;
}

public class CharacterCustomizationController : MonoBehaviour
{
    public List<CharacterCustomizationConfig> AllCharacters;
    public GameObject GraphicsRoot;

    private void Awake()
    {
        ServiceFactory.RegisterSingleton(this);
    }

    public void SpawnCharacter(CharacterId characterId)
    {
        foreach (var config in AllCharacters)
        {
            if (config.CharacterId == characterId)
            {
                Instantiate(config.GraphicsPrefab, GraphicsRoot.transform);
                foreach (var skillType in config.ExtraSkills)
                {
                    for (int i = 0; i < skillType.Amount; i++)
                    {
                        ServiceFactory.Resolve<SkillController>().AddSkillByType(skillType.SkillType);   
                    }
                }
                return;
            }
        }
        
        Debug.LogError("Could not spawn character of id: " + characterId);
    }
}
