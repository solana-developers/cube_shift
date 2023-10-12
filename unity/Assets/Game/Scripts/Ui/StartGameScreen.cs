using Frictionless;
using Game.Scripts;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class StartGameScreen : BaseScreen
{
    public CharacterSelectListItemView Fairy;
    public CharacterSelectListItemView FireMage;
    public CharacterSelectListItemView Monkey;

    public Button StartGameButton;

    public CharacterId SelectedCharacter = CharacterId.Default;

    private void Awake()
    {
        ServiceFactory.RegisterSingleton(this);
        Fairy.Select(true);
        FireMage.Select(false);
        Monkey.Select(false);

        Fairy.Init((listItem) =>
        {
            Fairy.Select(true);
            FireMage.Select(false);
            Monkey.Select(false);
            SelectedCharacter = listItem.CharacterId;
        });
        FireMage.Init((listItem) =>
        {
            if (OwnsCharacter(CharacterId.FireMage))
            {
                Fairy.Select(false);
                FireMage.Select(true);
                Monkey.Select(false);
                SelectedCharacter = listItem.CharacterId;
            }
            else
            {
                StartCoroutine(ServiceFactory.Resolve<GameshiftService>().CreateAsset(CharacterId.FireMage));
            }
        });
        Monkey.Init((listItem) =>
        {
            if (OwnsCharacter(CharacterId.Monkey))
            {
                Fairy.Select(false);
                FireMage.Select(false);
                Monkey.Select(true);
                SelectedCharacter = listItem.CharacterId;
            }
            else
            {
                StartCoroutine(ServiceFactory.Resolve<GameshiftService>().CreateAsset(CharacterId.Monkey));
            }
        });

        StartGameButton.onClick.AddListener(OnStartButtonClicked);
    }

    protected override void OnOpen()
    {
        UpdateContent();

        // TODO: Spawn all the different characters from config. For now just three hard coded ones. 
        base.OnOpen();
    }

    private void UpdateContent()
    {
        if (OwnsCharacter(CharacterId.FireMage))
        {
            FireMage.SelectButton.interactable = true;
            FireMage.SelectButtonText.text = "Select";
        }
        else
        {
            FireMage.SelectButton.interactable = true;
            FireMage.SelectButtonText.text = "Buy 0.99$";
        }

        if (OwnsCharacter(CharacterId.Monkey))
        {
            Monkey.SelectButton.interactable = true;
            Monkey.SelectButtonText.text = "Select";
        }
        else
        {
            Monkey.SelectButton.interactable = true;
            Monkey.SelectButtonText.text = "Buy 5.99$";
        }
    }

    private static bool OwnsCharacter(CharacterId characterId)
    {
        var allAssets = ServiceFactory.Resolve<GameshiftService>().AllAssets;
        if (allAssets != null)
        {
            foreach (var asset in allAssets.data)
            {
                if (asset.name == "Fire Mage" && characterId == CharacterId.FireMage)
                {
                    return true;
                }
                if (asset.name == "Monkey" && characterId == CharacterId.Monkey)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnStartButtonClicked()
    {
        ServiceFactory.Resolve<GameController>().StartGame(SelectedCharacter);
        Close();
    }

    public void UpdateAssets(AssetsResult allAssets)
    {
        UpdateContent();
    }
}