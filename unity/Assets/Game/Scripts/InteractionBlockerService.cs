using Frictionless;
using UnityEngine;

public class InteractionBlockerService : MonoBehaviour
{
    public GameObject Root;

    void Update()
    {
        var isRequestRunning = ServiceFactory.Resolve<GameshiftService>().IsRequestRunning();
        Root.gameObject.SetActive(isRequestRunning);
    }
}
