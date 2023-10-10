using Frictionless;
using UnityEngine;

public class MovementUi : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
        ServiceFactory.RegisterSingleton(this);
    }

}
