using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    public GameObject Root;

    public void Open()
    {
        Root.gameObject.SetActive(true);
        OnOpen();
    }

    protected virtual void OnOpen()
    {
        
    } 
    
    public void Close()
    {
        Root.gameObject.SetActive(false);
    }
}
