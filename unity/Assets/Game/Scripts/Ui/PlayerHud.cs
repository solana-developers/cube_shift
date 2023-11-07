using System;
using System.Collections;
using DG.Tweening;
using Frictionless;
using Game.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    public Slider Slider;
    public HealthBar HealthBar;
    public TextMeshProUGUI CurrentLevel;
    public TextMeshProUGUI Timer;
    public GameObject CoinsHud;
    public TextMeshProUGUI CoinsAmount;
    public float lerpSpeed = 2.0f; // Adjust this value to control the speed of the interpolation.

    private float startTime;
    private float xpTargetValue;
    private Coroutine xpUpdateCoroutine;

    public void Start()
    {
        ServiceFactory.Resolve<GameController>().OnGameStart += OnGameStart;
        ServiceFactory.Resolve<PlayerController>().OnXpChanged += OnXpChanged;
    }

    public void OnDestroy()
    {
        ServiceFactory.Resolve<GameController>().OnGameStart -= OnGameStart;
        ServiceFactory.Resolve<PlayerController>().OnXpChanged -= OnXpChanged;
    }

    private void OnGameStart()
    {
        startTime = Time.time;
    }
    
    public string FormatTime(float timeInSeconds)
    {
        // Convert the time in seconds to minutes and seconds
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        // Create a formatted string in "0:00" format
        string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);

        return formattedTime;
    }
    
    public void OnXpChanged(float newXpValue)
    {
        xpTargetValue = newXpValue;

        if (xpUpdateCoroutine == null)
        {
            // Start a new coroutine to update the slider smoothly.
            xpUpdateCoroutine = StartCoroutine(UpdateXPSlider());
            Slider.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
        }
    }
    
    private IEnumerator UpdateXPSlider()
    {
        float startValue = Slider.value;
        float elapsedTime = 0f;

        while (!Mathf.Approximately(Slider.value, xpTargetValue))
        {
            // Calculate the current slider value based on the lerp progress.
            float currentValue = Mathf.Lerp(startValue, xpTargetValue, elapsedTime / lerpSpeed);

            Slider.value = currentValue;

            // Update elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Set the slider value to the exact target value and reset the scale.
        Slider.value = xpTargetValue;

        // Reset the coroutine reference.
        xpUpdateCoroutine = null;
    }
    
    void Update()
    {
        var playerController = ServiceFactory.Resolve<PlayerController>();
        xpTargetValue = playerController.GetLevelProgress();

        CurrentLevel.text = playerController.CurrentLevel.ToString();
        Timer.text = FormatTime(Time.time - startTime);
        var gameController = ServiceFactory.Resolve<GameController>();
        Timer.gameObject.SetActive(gameController.IsGameRunning);
        Slider.gameObject.SetActive(gameController.IsGameRunning);
        CurrentLevel.gameObject.SetActive(gameController.IsGameRunning);
        CoinsHud.gameObject.SetActive(gameController.IsGameRunning);

        CoinsAmount.text = playerController.CurrentCoins.ToString();
        
        var boss = ServiceFactory.Resolve<EnemySpawner>().ActiveBoss;
        if (boss != null && boss.isActiveAndEnabled)
        {
            HealthBar.SetValue(boss.CurrentHealth / (float) boss.StartHealth);
            HealthBar.Slider.gameObject.SetActive(true);
        }
        else
        {
            HealthBar.SetValue(1);
        }
    }
}
