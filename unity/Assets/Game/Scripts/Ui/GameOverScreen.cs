using Frictionless;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : BaseScreen
{
    public Button RestartButton;
    public TextMeshProUGUI WonText;
    public GameObject WonRoot;
    
    private void Awake()
    {
        ServiceFactory.RegisterSingleton(this);
        RestartButton.onClick.AddListener(OnRestartClicked);
    }

    private void OnRestartClicked()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void Init(bool won)
    {
        WonRoot.SetActive(won);
        WonText.text = won ? "You won!" : "You lost";
        Open();
    }
}
