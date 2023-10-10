using UnityEngine;

namespace Cubeshift
{
    public class BaseSkill : MonoBehaviour
    {
        public int Level;
        public SkillType SkillType;

        public virtual void OnLevelUp()
        {
            
        }
    }
}