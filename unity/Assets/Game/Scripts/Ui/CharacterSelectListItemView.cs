using System;
using Frictionless;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectListItemView : MonoBehaviour
{
    public Button SelectButton;
    public TextMeshProUGUI SelectButtonText;
    public GameObject SelectionBorder;
    
    public CharacterId CharacterId;

    private Action<CharacterSelectListItemView> onClick;

    private void Awake()
    {
        ServiceFactory.RegisterSingleton(this);
        SelectButton.onClick.AddListener(OnSelectButtonClicked);
    }

    public void Init(Action<CharacterSelectListItemView> onCLick)
    {
        this.onClick = onCLick;
    }
    
    public void Select(bool isActive)
    {
        SelectionBorder.gameObject.SetActive(isActive);
    }

    private void OnSelectButtonClicked()
    {
        onClick?.Invoke(this);
    }
}
