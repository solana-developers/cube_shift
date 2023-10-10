using TMPro;
using ToolBox.Pools;
using UnityEngine;

public class TextBlimp : MonoBehaviour, IPoolable
{
    public TextMeshPro Text;
    
    public void Init(string text, Color color)
    {
        Text.text = text;
        Text.color = color;
    }
    
    public void OnReuse()
    {
    }

    public void OnRelease()
    {
    }
}
