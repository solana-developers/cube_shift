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

    private float startTime;
    
    public void Start()
    {
        ServiceFactory.Resolve<GameController>().OnGameStart += OnGameStart;
    }

    public void OnDestroy()
    {
        ServiceFactory.Resolve<GameController>().OnGameStart -= OnGameStart;
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
    
    void Update()
    {
        var playerController = ServiceFactory.Resolve<PlayerController>();
        Slider.value = playerController.GetLevelProgress();
         CurrentLevel.text = playerController.CurrentLevel.ToString();
        Timer.text = FormatTime(Time.time - startTime);
        Timer.gameObject.SetActive(ServiceFactory.Resolve<GameController>().IsGameRunning);
        Slider.gameObject.SetActive(ServiceFactory.Resolve<GameController>().IsGameRunning);
        CurrentLevel.gameObject.SetActive(ServiceFactory.Resolve<GameController>().IsGameRunning);
        
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
