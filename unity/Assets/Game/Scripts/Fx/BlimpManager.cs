using System;
using Frictionless;
using ToolBox.Pools;
using UnityEngine;

public class BlimpManager : MonoBehaviour
{
    public TextBlimp TextBlimpPrefab;

    private void Awake()
    {
        ServiceFactory.RegisterSingleton(this);
    }

    public void SpawnBlimp(Vector3 position, string text, Color color)
    {
        var textBlimp = TextBlimpPrefab.gameObject.Reuse<TextBlimp>();
        textBlimp.Init(text, color);
        textBlimp.transform.position = position;
    }
}
