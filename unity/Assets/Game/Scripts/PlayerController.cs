using System;
using Cubeshift;
using Frictionless;
using Game.Scripts;
using ToolBox.Pools;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int CurrentLevel;
    public float CurrentExp;
    public int CurrentCoins;
    public double CollectRange;
    public int CurrentHealth = 50;
    public int MaxHealth = 50;
    public HealthBar HealthBar;
    public AudioSource XpCollectSource;
    public AudioClip XpCollectClip;
    public AudioClip CoinCollectClip;
    
    public GameObject ConfettiPrefab;
    public GameObject LevelUpPrefab;
    public Transform CachedTransform;
    public delegate void onXpChanged(float newXp);
    public onXpChanged OnXpChanged;

    // Define the constants for your leveling system
    private const int MaxLevel = 100;
    private const int ExpRequiredForLevel1 = 100;
    private const float ExpMultiplier = 1.1f;
    private const string CoinsPlayerPrefsKey = "coins";
    private float lastSoundPlayed;

    void Awake()
    {
        ServiceFactory.RegisterSingleton(this);
        CurrentLevel = 1;
        CachedTransform = transform;
        CurrentCoins = PlayerPrefs.GetInt(CoinsPlayerPrefsKey);
    }

    public void Hit(int damage)
    {
        CurrentHealth -= damage;
        var isHealing = damage < 0;
        Color color = isHealing ? Color.green : Color.red;
        string damageString = isHealing ? "+" + Math.Abs(damage) : "-" + damage;
        ServiceFactory.Resolve<BlimpManager>().SpawnBlimp(transform.position, damageString, color);
        if (!isHealing)
        {
            ServiceFactory.Resolve<CameraShake>().Shake(0.1f, 0.5f);   
        }
        if (CurrentHealth <= 0)
        {
            Die();
        }
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        HealthBar.SetValue(CurrentHealth / (float) MaxHealth);
    }

    private void Die()
    {
        ServiceFactory.Resolve<GameController>().GameOver(false);
        Debug.Log("player died");
    }
    
    public void GainExp(float expGained)
    {
        if (Time.time > lastSoundPlayed + 0.05)
        {
            XpCollectSource.PlayOneShot(XpCollectClip);
            lastSoundPlayed = Time.time;
        }
        CurrentExp += expGained;
        ServiceFactory.Resolve<BlimpManager>().SpawnBlimp(transform.position, $"+{expGained}Xp", Color.green);
        OnXpChanged.Invoke(expGained);
        // Check if the player has leveled up
        while (CurrentLevel < MaxLevel && CurrentExp >= GetExpRequiredForNextLevel())
        {
            OnLevelUp();
        }
    }

    public void GainCoin(int coinsGained)
    {
        XpCollectSource.PlayOneShot(CoinCollectClip);
        CurrentCoins += coinsGained;
        ServiceFactory.Resolve<BlimpManager>().SpawnBlimp( transform.position ,$"+{coinsGained} coins!", Color.yellow);
        PlayerPrefs.SetInt(CoinsPlayerPrefsKey, CurrentCoins);
    }

    private void OnLevelUp()
    {
        CurrentExp -= GetExpRequiredForNextLevel();
        CurrentLevel++;
        var confettig = ConfettiPrefab.Reuse();
        confettig.transform.position = transform.position + Vector3.up * 2;
        var levelUpPrefab = LevelUpPrefab.Reuse();
        levelUpPrefab.transform.position = transform.position + Vector3.up * 2;        
        ServiceFactory.Resolve<SkillController>().SpawnCollectableSkill();
        ServiceFactory.Resolve<BlimpManager>().SpawnBlimp(transform.position, "Level Up!", Color.white);
    }

    public float GetLevelProgress()
    {
        float expRequiredForCurrentLevel = GetExpRequiredForLevel(CurrentLevel);
        float expRequiredForNextLevel = GetExpRequiredForNextLevel();

        return CurrentExp / expRequiredForNextLevel;
    }

    private float GetExpRequiredForLevel(int level)
    {
        return ExpRequiredForLevel1 * (float)Math.Pow(ExpMultiplier, level - 1);
    }

    private float GetExpRequiredForNextLevel()
    {
        return GetExpRequiredForLevel(CurrentLevel + 1);
    }
}
