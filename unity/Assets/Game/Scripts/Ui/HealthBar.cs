using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Slider;

    private void Awake()
    {
        Slider.gameObject.SetActive(false);
    }

    public void SetValue(float newValue)
    {
        Slider.value = newValue;
        Slider.gameObject.SetActive(newValue < 1);
    }
}
