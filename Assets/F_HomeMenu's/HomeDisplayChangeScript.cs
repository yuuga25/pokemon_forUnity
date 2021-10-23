using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Threading.Tasks;

public class HomeDisplayChangeScript : MonoBehaviour
{
    public HomeController homeController;

    public GameObject loading_Image;
    public GameObject error_Image;

    [SerializeField] private UIGradient backgroundColor;
    public GameObject pictorialPattern;
    private Image[] ChildObject;
    
    [Header("ホーム")]
    public GameObject home;
    public Sprite icon_Home;
    public Color topColor_Home;
    public Color bottomColor_Home;
    public Animator anim_Home;

    [Header("バトル")]
    public GameObject battle;
    public Sprite icon_Battle;
    public Color topColor_Battle;
    public Color bottomColor_Battle;
    public Menu_BattleController battleController;

    [Header("バッグ")]
    public GameObject bag;
    public Sprite icon_Bag;
    public Color topColor_Bag;
    public Color bottomColor_Bag;
    public Text buttonText_Bag;
    public Menu_BagController bagController;

    [Header("ポケモン")]
    public GameObject pokémon;
    public Sprite icon_Pokémon;
    public Color topColor_Pokémon;
    public Color bottomColor_Pokémon;
    public Text buttonText_Pokémon;
    public Menu_PokéController pokéController;

    [Header("ショップ")]
    public GameObject shop;
    public Sprite icon_Shop;
    public Color topColor_Shop;
    public Color bottomColor_Shop;
    public Text buttonText_Shop;
    public Menu_ShopController shopController;

    [Header("図鑑")]
    public GameObject pic;
    public Sprite icon_Pic;
    public Color topColor_Pic;
    public Color bottomColor_Pic;
    public Menu_PicController picController;

    [Header("トレーナーカード")]
    public GameObject trainerCard;
    public Sprite icon_TrainerCard;
    public Color topColor_TrainerCard;
    public Color bottomColor_TrainerCard;

    [Header("設定")]
    public GameObject settings;
    public Sprite icon_Settings;
    public Color topColor_Settings;
    public Color bottomColor_Settings;
    public Text buttonText_Settings;
    public Menu_SettingsController settingsController;

    [Header("ステータスメニュー")]
    public Color topColor_Status;
    public Color bottomColor_Status;

    private void Awake()
    {
        loading_Image.SetActive(false);
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            print("ログインしていません。");
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }

    private void Start()
    {
        anim_Home.enabled = false;
        ChildObject = new Image[pictorialPattern.transform.childCount];
        for(var i = 0; i < pictorialPattern.transform.childCount; i++)
        {
            ChildObject[i] = pictorialPattern.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        HomeButton();
    }
    public void HomeButton()
    {
        homeController.readCP();
        displayObject(home, icon_Home, topColor_Home, bottomColor_Home);
    }
    public void BattleButton()
    {
        if (!anim_Home.enabled) { anim_Home.enabled = true; }
        if (battle.activeInHierarchy) 
        {
            HomeButton();
        }
        else
        {
            displayObject(battle, icon_Battle, topColor_Battle, bottomColor_Battle);
            battleController.SetModeSelect();
        }
    }
    public void BagButton()
    {
        if (!anim_Home.enabled) { anim_Home.enabled = true; }
        if (bag.activeInHierarchy)
        {
            HomeButton();
        }
        else
        {
            displayObject(bag, icon_Bag, topColor_Bag, bottomColor_Bag);
            bagController.GetItemList();
            buttonText_Bag.text = "閉じる";
        }
    }
    public void PokémonButton()
    {
        if (!anim_Home.enabled) { anim_Home.enabled = true; }
        if (pokémon.activeInHierarchy)
        {
            HomeButton();
        }
        else
        {
            displayObject(pokémon, icon_Pokémon, topColor_Pokémon, bottomColor_Pokémon);
            pokéController.Button_Organization();
            buttonText_Pokémon.text = "閉じる";
        }
    }
    public void ShopButton()
    {
        if (!anim_Home.enabled) { anim_Home.enabled = true; }
        if (shop.activeInHierarchy)
        {
            HomeButton();
        }
        else
        {
            displayObject(shop, icon_Shop, topColor_Shop, bottomColor_Shop);
            shopController.GetCatalogData();
            shopController.scrollValue_y = 1;
            buttonText_Shop.text = "閉じる";
        }
    }
    public void PicButton()
    {
        if (!anim_Home.enabled) { anim_Home.enabled = true; }
        if (pic.activeInHierarchy)
        {
            HomeButton();
        }
        else
        {
            picController.SetPic();
            displayObject(pic, icon_Pic, topColor_Pic, bottomColor_Pic);
        }
    }
    public void TrainerCardButton()
    {
        if (!anim_Home.enabled) { anim_Home.enabled = true; }
        if (trainerCard.activeInHierarchy)
        {
            HomeButton();
        }
        else
        {
            displayObject(trainerCard, icon_TrainerCard, topColor_TrainerCard, bottomColor_TrainerCard);
        }
    }
    public void SettingsButton()
    {
        if (!anim_Home.enabled) { anim_Home.enabled = true; }
        if (settings.activeInHierarchy)
        {
            HomeButton();
        }
        else
        {
            displayObject(settings, icon_Settings, topColor_Settings, bottomColor_Settings);
            settingsController.SetSettings();
            buttonText_Settings.text = "閉じる";
        }
    }
    private void displayObject(GameObject onenabled, Sprite image, Color changeTopColor, Color changeBottomColor)
    {
        Menu_PokéController.levelupItemValue = -1;
        if (FlagManager.isDataChange)
        {
            loading_Image.SetActive(true);

            PlayFabClientAPI.UpdateUserData(
                new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>()
                    {
                        {"PlayerData", PlayFabSimpleJson.SerializeObject(DataLists.playerData) },
                    }
                },
                result =>
                {
                    buttonText_Bag.text = "持ち物";
                    buttonText_Pokémon.text = "ポケモン";
                    buttonText_Shop.text = "ショップ";
                    buttonText_Settings.text = "設定";

                    backgroundColor.m_color1 = changeTopColor;
                    backgroundColor.m_color2 = changeBottomColor;
                    backgroundColor.enabled = false;
                    backgroundColor.enabled = true;

                    home.SetActive(false);
                    battle.SetActive(false);
                    bag.SetActive(false);
                    pokémon.SetActive(false);
                    shop.SetActive(false);
                    pic.SetActive(false);
                    trainerCard.SetActive(false);
                    settings.SetActive(false);

                    onenabled.SetActive(true);

                    for (var i = 0; i < ChildObject.Length; i++)
                    {
                        ChildObject[i].sprite = image;
                    }
                    pokéController.copyData_Organization = null;
                    pokéController.copyData_List = null;

                    FlagManager.isDataChange = false;
                    loading_Image.SetActive(false);
                },
                error => { error_Image.SetActive(true); error.GenerateErrorReport(); });
        }
        else
        {
            buttonText_Bag.text = "持ち物";
            buttonText_Pokémon.text = "ポケモン";
            buttonText_Shop.text = "ショップ";
            buttonText_Settings.text = "設定";

            backgroundColor.m_color1 = changeTopColor;
            backgroundColor.m_color2 = changeBottomColor;
            backgroundColor.enabled = false;
            backgroundColor.enabled = true;

            home.SetActive(false);
            battle.SetActive(false);
            bag.SetActive(false);
            pokémon.SetActive(false);
            shop.SetActive(false);
            pic.SetActive(false);
            trainerCard.SetActive(false);
            settings.SetActive(false);

            onenabled.SetActive(true);

            for (var i = 0; i < ChildObject.Length; i++)
            {
                ChildObject[i].sprite = image;
            }
        }
    }
}
