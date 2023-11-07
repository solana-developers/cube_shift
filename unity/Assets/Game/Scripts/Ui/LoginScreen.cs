using System;
using DG.Tweening;
using Frictionless;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : BaseScreen
{
    public TMP_InputField EmailInput;
    public Button LoginButton;
    public TextMeshProUGUI Error;
    
    private void Awake()
    {
        ServiceFactory.RegisterSingleton(this);
        LoginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void Start()
    {
        ServiceFactory.Resolve<GameshiftService>().OnLogin += OnLogin;
    }

    private void OnLogin()
    {
        Close();
    }
    
    private void OnLoginButtonClicked()
    {
        if (IsEmailValid(EmailInput.text))
        {
            Error.gameObject.SetActive(false);
            ServiceFactory.Resolve<GameshiftService>().LoginOrRegister(EmailInput.text);
        }
        else
        {
            Error.gameObject.SetActive(true);
            Error.gameObject.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
            Error.text = "Invalid Email";
        }
    }
    
    private bool IsEmailValid(string email)
    {
        // Use a regular expression for email validation.
        // This example pattern checks for basic email format but may not catch all edge cases.
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
        return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
    }
}
