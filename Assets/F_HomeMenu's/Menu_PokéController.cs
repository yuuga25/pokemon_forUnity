using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class Menu_PokéController : MonoBehaviour
{
    public VoiceData_Pokémon voiceData;
    public AudioSource audioManager_SE;
    public AudioSource audioManager_Voice;
    public ReorganizationScript reorganization;

    public GameObject text_Pokémon;     //上部タブイメージ

    public GameObject loading_Image;    //ロード画面
    public GameObject error_Image;      //エラー画面

    #region//編成
    [Header("編成画面")]
    public GameObject Organization;
    public TeamData_Pokémon copyData_Organization;
    public ImageData_Pokémon imageData;
    public GameObject[] pokéUnit = new GameObject[6];
    public Sprite[] genderImageSprite = new Sprite[2];

    public GameObject belongingsObj;
    public ImageData_Belongings imageData_Belongings;
    public GameObject[] belongings_Obj;

    [Header("編成入れ替え")]
    public GameObject Reorganization;

    #endregion

    #region//一覧
    [Header("一覧")]
    public GameObject List;
    public GameObject content_List;
    public GameObject pokémon_Unit;
    public List<UserData_Pokémon> copyData_List = new List<UserData_Pokémon>();

    public Color[] typeColor = new Color[19];

    [Header("フレーム/強化レベル1")]
    public Color Color1_lv1;
    public Color Color2_lv1;

    [Header("フレーム/強化レベル2")]
    public Color Color1_lv2;
    public Color Color2_lv2;

    [Header("フレーム/強化レベル3")]
    public Color Color1_lv3;
    public Color Color2_lv3;

    [Header("フレーム/強化レベル4")]
    public Color Color1_lv4;
    public Color Color2_lv4;

    [Header("フレーム/強化レベル5")]
    public Color Color1_lv5;
    public Color Color2_lv5;

    [Header("ボール")]
    public Sprite[] ballSprite = new Sprite[4];

    [Header("所持数表示")]
    public Text displayText;
    #endregion

    #region//ステータス
    [Header("ステータスメニュー")]
    private string temporarySaveUniqueID;

    public GameObject Status;
    public Image image_Pokemon;
    public Text pokeName;
    public Text pokeType;
    public Text pokePersonality;
    public Text pokeExperiencePoint;
    public Text pokeToTheNextLevel;
    public Slider pokeToTheNextLevel_Slider;
    public Text pokeCharacteristic;

    public Text pokeNickName;
    public Text pokeLevel;

    public GameObject genderImage_Status;
    public GameObject kiraImage;

    public Color defaultColor;
    public Color upStatusColor;
    public Color downStatusColor;
    public Text realATK;
    public Text realDEF;
    public Text realSATK;
    public Text realSDEF;
    public Text realSPE;
    public Text realValueHP;
    public Text realValueATK;
    public Text realValueDEF;
    public Text realValueSATK;
    public Text realValueSDEF;
    public Text realValueSPE;
    public Text Elevel;

    public Sprite[] Classification = new Sprite[3];
    public Image Image_Classification;
    public Text Text_Power;
    public Text Text_Hit;
    public Text Text_Explanation;
    public GameObject Technique1;
    public GameObject Technique2;
    public GameObject Technique3;
    public GameObject Technique4;

    public GameObject characteristictext;

    [Header("ステータスUI")]
    public GameObject Image_AlwaysDisplayed;

    public Image buttonImage_BasicStatus;
    public Image buttonImage_BattleStatus;
    public Image buttonImage_Technique;

    public GameObject List_BasicStatus;
    public GameObject List_BattleStatus;
    public GameObject List_Technique;
    public GameObject List_EffortValueController;
    public GameObject List_IndividualValueController;

    public GameObject button_Individual;

    [Header("技変更")]
    public GameObject TechniqueChangeDisplay;
    public GameObject[] techniqueButtons = new GameObject[10];

    [Header("ニックネーム")]
    public InputField nicknameText;
    public GameObject errorObject;

    [Header("持ち物")]
    public Status_ListBagScript bagList;
    #endregion

    #region//レベルアップ・進化
    [Header("レベルアップ/進化")]
    public AudioClip clickSE;

    public GameObject levelUpSystemObject;
    public GameObject button_Evolution;
    public GameObject text_Max;
    public GameObject text_notItem;

    public GameObject content_levelUpItem;
    public GameObject levelUpItem_Unit;

    public Sprite[] levelUpItemImage = new Sprite[6];

    private Text Text_levelUpItem_XS;
    private Text Text_levelUpItem_S;
    private Text Text_levelUpItem_M;
    private Text Text_levelUpItem_L;
    private Text Text_levelUpItem_XL;
    private Text Text_levelUpItem_Mysterious;

    //使用できるアイテム数
    private int possession_XS;
    private int possession_S;
    private int possession_M;
    private int possession_L;
    private int possession_XL;
    private int possession_Mysterious;

    //使用したアイテム数
    [HideInInspector] public int c_possession_XS = 0;
    [HideInInspector] public int c_possession_S = 0;
    [HideInInspector] public int c_possession_M = 0;
    [HideInInspector] public int c_possession_L = 0;
    [HideInInspector] public int c_possession_XL = 0;
    [HideInInspector] public int c_possession_Mysterious = 0;
    [HideInInspector] public string c_itemInstanceId_XS;
    [HideInInspector] public string c_itemInstanceId_S;
    [HideInInspector] public string c_itemInstanceId_M;
    [HideInInspector] public string c_itemInstanceId_L;
    [HideInInspector] public string c_itemInstanceId_XL;
    [HideInInspector] public string c_itemInstanceId_Mysterious;

    public GameObject Evolution_Confirmation;
    #endregion

    #region//強化レベル
    [Header("強化レベル")]
    public Text Text_money;
    public Text Text_Cost;
    public GameObject ELevelSystem;
    public GameObject Text_ELevel_Max;
    public GameObject ButtonCover;

    const int Cost_ELevel1 = 10000;
    const int Cost_ELevel2 = 80000;
    const int Cost_ELevel3 = 180000;
    const int Cost_ELevel4 = 320000;
    #endregion

    #region//努力値・個体値
    [Header("努力値")]
    public InputField inputField_HP;
    public InputField inputField_ATK;
    public InputField inputField_DEF;
    public InputField inputField_SATK;
    public InputField inputField_SDEF;
    public InputField inputField_SPE;

    public Text textValue_HP;
    public Text textValue_ATK;
    public Text textValue_DEF;
    public Text textValue_SATK;
    public Text textValue_SDEF;
    public Text textValue_SPE;

    public Text text_Total_Value;

    [Header("個体値")]
    public InputField inputField_Indi_HP;
    public InputField inputField_Indi_ATK;
    public InputField inputField_Indi_DEF;
    public InputField inputField_Indi_SATK;
    public InputField inputField_Indi_SDEF;
    public InputField inputField_Indi_SPE;

    public Text textValue_Indi_HP;
    public Text textValue_Indi_ATK;
    public Text textValue_Indi_DEF;
    public Text textValue_Indi_SATK;
    public Text textValue_Indi_SDEF;
    public Text textValue_Indi_SPE;
    #endregion

    #region//ガチャ
    [Header("ガチャ")]
    public GameObject Gacha;
    public GachaScript gachaScript;
    #endregion

    private void Start()
    {
        copyData_Organization = null;
        copyData_List = null;
        Button_Organization();
    }
    private void Update()
    {
        if(copyData_Organization != DataLists.playerData.teamDatas)
        {
            copyData_Organization = DataLists.playerData.teamDatas;
            SetOrganization();
        }
        if(copyData_List != DataLists.playerData.pokémonsList)
        {
            copyData_List = DataLists.playerData.pokémonsList;
            foreach(Transform childUnit in content_List.transform)
            {
                Destroy(childUnit.gameObject);
            }
            SetList();
        }
    }
    public void Button_Organization()
    {
        Organization.SetActive(true);
        List.SetActive(false);
        Gacha.SetActive(false);
        Status.SetActive(false);
        Reorganization.SetActive(false);
        text_Pokémon.SetActive(true);
        belongingsObj.SetActive(false);
        LGConfObj.SetActive(false);
    }    //編成画面表示
    public void Button_List()
    {
        Organization.SetActive(false);
        List.SetActive(true);
        Gacha.SetActive(false);
    }            //一覧画面表示
    public void Button_Gacha()
    {
        Organization.SetActive(false);
        List.SetActive(false);
        Gacha.SetActive(true);
        gachaScript.SetGacha();
    }           //ガチャ画面表示
    public void SetOrganization()
    {
        for(int u = 0; u < pokéUnit.Length; u++)
        {
            pokéUnit[u].SetActive(false);
        }

        for(int i = 0; i < DataLists.playerData.teamDatas.pokémons.Length; i++)
        {
            if(DataLists.playerData.teamDatas.pokémons[i] == null)
            {
                break;
            }

            pokéUnit[i].SetActive(true);
            var unitData = DataLists.playerData.teamDatas.pokémons[i];

            //手持ちポケモン画像
            var images = imageData.sheet.Where(x => x.p_Id == unitData.userP_Id);
            var UnitImage = pokéUnit[i].transform.Find("Image_Unit").gameObject.GetComponent<Image>();
            foreach(var sprite in images)
            {
                if (unitData.isDifferentColors)
                {
                    UnitImage.sprite = sprite.p_ImageHand_C;
                }
                else if (!unitData.isDifferentColors)
                {
                    UnitImage.sprite = sprite.p_ImageHand;
                }
            }

            //ポケモンのニックネーム表示
            var UnitName = pokéUnit[i].transform.Find("Text_Name").gameObject.GetComponent<Text>();
            UnitName.text = $"{unitData.userP_NickName}";

            //ポケモンの性別表示
            var genderImage = pokéUnit[i].transform.Find("Image_Gender").gameObject;
            if (unitData.userP_gender == 2)
            {
                genderImage.SetActive(false);
            }
            else
            {
                genderImage.SetActive(true);
                var image = genderImage.GetComponent<Image>();
                
                if(unitData.userP_gender == 0)
                {
                    image.sprite = genderImageSprite[0];
                }
                else if(unitData.userP_gender == 1)
                {
                    image.sprite = genderImageSprite[1];
                }
            }

            //ポケモンのレベル表示
            var UnitLevel = pokéUnit[i].transform.Find("Text_Level").gameObject.GetComponent<Text>();
            UnitLevel.text = $"Lv.{unitData.userP_Level}";
        }

        for(int b = 0; b < 6; b++)
        {
            belongings_Obj[b].transform.Find("Image").gameObject.SetActive(true);
            belongings_Obj[b].transform.Find("Text").gameObject.SetActive(true);
            string linq = null;
            switch (b+1)
            {
                case 1:
                    linq = DataLists.playerData.teamDatas.b_item1;
                    break;
                case 2:
                    linq = DataLists.playerData.teamDatas.b_item2;
                    break;
                case 3:
                    linq = DataLists.playerData.teamDatas.b_item3;
                    break;
                case 4:
                    linq = DataLists.playerData.teamDatas.b_item4;
                    break;
                case 5:
                    linq = DataLists.playerData.teamDatas.b_item5;
                    break;
                case 6:
                    linq = DataLists.playerData.teamDatas.b_item6;
                    break;
            }
            if (linq == null)
            {
                belongings_Obj[b].transform.Find("Image").gameObject.SetActive(false);
            }
            else
            {
                belongings_Obj[b].transform.Find("Text").gameObject.SetActive(false);

                var image = imageData_Belongings.sheet.Find(x => x.imageId == linq);
                belongings_Obj[b].transform.Find("Image").GetComponent<Image>().sprite = image.item_Image;
            }
        }
    }       //編成キャラ表示
    private void SetList()
    {
        var datas = DataLists.playerData.pokémonsList.OrderBy(x => x.userP_Type1);

        displayText.text = $"{DataLists.playerData.pokémonsList.Count}/150";

        foreach(var i in datas) 
        {
            GameObject unit = Instantiate(pokémon_Unit, content_List.transform);

            //背景色・タイプカラー
            int colorNum = 0;
            switch (i.userP_Type1)
            {
                case Pokémon_Type.Type.None:
                    colorNum = 18;
                    break;
                case Pokémon_Type.Type.Normal:
                    colorNum = 0;
                    break;
                case Pokémon_Type.Type.Fire:
                    colorNum = 1;
                    break;
                case Pokémon_Type.Type.Water:
                    colorNum = 2;
                    break;
                case Pokémon_Type.Type.Electric:
                    colorNum = 4;
                    break;
                case Pokémon_Type.Type.Grass:
                    colorNum = 3;
                    break;
                case Pokémon_Type.Type.Ice:
                    colorNum = 5;
                    break;
                case Pokémon_Type.Type.Fighting:
                    colorNum = 6;
                    break;
                case Pokémon_Type.Type.Poison:
                    colorNum = 7;
                    break;
                case Pokémon_Type.Type.Ground:
                    colorNum = 8;
                    break;
                case Pokémon_Type.Type.Flying:
                    colorNum = 9;
                    break;
                case Pokémon_Type.Type.Psychic:
                    colorNum = 10;
                    break;
                case Pokémon_Type.Type.Bug:
                    colorNum = 11;
                    break;
                case Pokémon_Type.Type.Rock:
                    colorNum = 12;
                    break;
                case Pokémon_Type.Type.Ghost:
                    colorNum = 13;
                    break;
                case Pokémon_Type.Type.Dragon:
                    colorNum = 14;
                    break;
                case Pokémon_Type.Type.Dark:
                    colorNum = 15;
                    break;
                case Pokémon_Type.Type.Steel:
                    colorNum = 16;
                    break;
                case Pokémon_Type.Type.Fairy:
                    colorNum = 17;
                    break;
            }
            unit.transform.Find("Image_FirstColor").gameObject.GetComponent<Image>().color = typeColor[colorNum];
            switch (i.userP_Type2)
            {
                case Pokémon_Type.Type.None:
                    colorNum = 18;
                    break;
                case Pokémon_Type.Type.Normal:
                    colorNum = 0;
                    break;
                case Pokémon_Type.Type.Fire:
                    colorNum = 1;
                    break;
                case Pokémon_Type.Type.Water:
                    colorNum = 2;
                    break;
                case Pokémon_Type.Type.Electric:
                    colorNum = 4;
                    break;
                case Pokémon_Type.Type.Grass:
                    colorNum = 3;
                    break;
                case Pokémon_Type.Type.Ice:
                    colorNum = 5;
                    break;
                case Pokémon_Type.Type.Fighting:
                    colorNum = 6;
                    break;
                case Pokémon_Type.Type.Poison:
                    colorNum = 7;
                    break;
                case Pokémon_Type.Type.Ground:
                    colorNum = 8;
                    break;
                case Pokémon_Type.Type.Flying:
                    colorNum = 9;
                    break;
                case Pokémon_Type.Type.Psychic:
                    colorNum = 10;
                    break;
                case Pokémon_Type.Type.Bug:
                    colorNum = 11;
                    break;
                case Pokémon_Type.Type.Rock:
                    colorNum = 12;
                    break;
                case Pokémon_Type.Type.Ghost:
                    colorNum = 13;
                    break;
                case Pokémon_Type.Type.Dragon:
                    colorNum = 14;
                    break;
                case Pokémon_Type.Type.Dark:
                    colorNum = 15;
                    break;
                case Pokémon_Type.Type.Steel:
                    colorNum = 16;
                    break;
                case Pokémon_Type.Type.Fairy:
                    colorNum = 17;
                    break;
            }
            unit.transform.Find("Image_SecondColor").gameObject.GetComponent<Image>().color = typeColor[colorNum];

            //手持ちポケモン画像
            var imageData_Hand = imageData.sheet.Where(x => x.p_Id == i.userP_Id);
            Sprite image = null;
            foreach(var ii in imageData_Hand)
            {
                if (i.isDifferentColors)
                {
                    image = ii.p_ImageHand_C;
                }
                else if (!i.isDifferentColors)
                {
                    image = ii.p_ImageHand;
                }
                unit.transform.Find("Image_Poké").gameObject.GetComponent<Image>().sprite = image;
            }

            //フレーム
            Color color1 = new Color();
            Color color2 = new Color();
            switch (i.userP_ELevel)
            {
                case 1:
                    color1 = Color1_lv1;
                    color2 = Color2_lv1;
                    break;
                case 2:
                    color1 = Color1_lv2;
                    color2 = Color2_lv2;
                    break;
                case 3:
                    color1 = Color1_lv3;
                    color2 = Color2_lv3;
                    break;
                case 4:
                    color1 = Color1_lv4;
                    color2 = Color2_lv4;
                    break;
                case 5:
                    color1 = Color1_lv5;
                    color2 = Color2_lv5;
                    break;
            }
            var Frame = unit.transform.Find("Frame").gameObject.GetComponent<UICornersGradient>();
            Frame.m_topLeftColor = color1;
            Frame.m_topRightColor = color2;
            Frame.m_bottomRightColor = color1;
            Frame.m_bottomLeftColor = color2;

            //レベル
            unit.transform.Find("Text_Level").gameObject.GetComponent<Text>().text = $"Lv.{i.userP_Level}";

            //ボール
            int ballId = 0;
            switch (i.userP_Ball)
            {
                case Pokémon_Ball.Ball.Poké:
                    ballId = 0;
                    break;
                case Pokémon_Ball.Ball.Great:
                    ballId = 1;
                    break;
                case Pokémon_Ball.Ball.Ultra:
                    ballId = 2;
                    break;
                case Pokémon_Ball.Ball.Master:
                    ballId = 3;
                    break;
            }
            unit.transform.Find("Image_Ball").gameObject.GetComponent<Image>().sprite = ballSprite[ballId];
            string tmp = i.unique_Id;
            unit.GetComponent<Button>().onClick.AddListener(() => { SetStatus(tmp); });
        }
    }               //キャラ一覧表示

    #region ステータス関連
    public void SetStatus(string id)
    {
        List.SetActive(false);
        Status.SetActive(true);
        text_Pokémon.SetActive(false);
        bagList.List_Bag.SetActive(false);
        LGConfObj.SetActive(false);

        var data = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == id);
        temporarySaveUniqueID = id;
        foreach(var i in data)
        {
            var standingImage = imageData.sheet.Where(x => x.p_Id == i.userP_Id);
            foreach(var si in standingImage)
            {
                if (i.isDifferentColors)
                {
                    image_Pokemon.sprite = si.p_ImageFront_C;
                }
                else
                {
                    image_Pokemon.sprite = si.p_ImageFront;
                }
            }
            pokeName.text = $"{i.userP_Name}";
            string typeName1 = "";
            switch (i.userP_Type1)
            {
                case Pokémon_Type.Type.Normal:
                    typeName1 = "ノーマル";
                    break;
                case Pokémon_Type.Type.Fire:
                    typeName1 = "ほのお";
                    break;
                case Pokémon_Type.Type.Water:
                    typeName1 = "みず";
                    break;
                case Pokémon_Type.Type.Grass:
                    typeName1 = "くさ";
                    break;
                case Pokémon_Type.Type.Electric:
                    typeName1 = "でんき";
                    break;
                case Pokémon_Type.Type.Ice:
                    typeName1 = "こおり";
                    break;
                case Pokémon_Type.Type.Fighting:
                    typeName1 = "かくとう";
                    break;
                case Pokémon_Type.Type.Poison:
                    typeName1 = "どく";
                    break;
                case Pokémon_Type.Type.Ground:
                    typeName1 = "じめん";
                    break;
                case Pokémon_Type.Type.Flying:
                    typeName1 = "ひこう";
                    break;
                case Pokémon_Type.Type.Psychic:
                    typeName1 = "エスパー";
                    break;
                case Pokémon_Type.Type.Bug:
                    typeName1 = "むし";
                    break;
                case Pokémon_Type.Type.Rock:
                    typeName1 = "いわ";
                    break;
                case Pokémon_Type.Type.Ghost:
                    typeName1 = "ゴースト";
                    break;
                case Pokémon_Type.Type.Dragon:
                    typeName1 = "ドラゴン";
                    break;
                case Pokémon_Type.Type.Dark:
                    typeName1 = "あく";
                    break;
                case Pokémon_Type.Type.Steel:
                    typeName1 = "はがね";
                    break;
                case Pokémon_Type.Type.Fairy:
                    typeName1 = "フェアリー";
                    break;
            }
            string typeName2 = "";
            switch (i.userP_Type2)
            {
                case Pokémon_Type.Type.Normal:
                    typeName2 = "ノーマル";
                    break;
                case Pokémon_Type.Type.Fire:
                    typeName2 = "ほのお";
                    break;
                case Pokémon_Type.Type.Water:
                    typeName2 = "みず";
                    break;
                case Pokémon_Type.Type.Grass:
                    typeName2 = "くさ";
                    break;
                case Pokémon_Type.Type.Electric:
                    typeName2 = "でんき";
                    break;
                case Pokémon_Type.Type.Ice:
                    typeName2 = "こおり";
                    break;
                case Pokémon_Type.Type.Fighting:
                    typeName2 = "かくとう";
                    break;
                case Pokémon_Type.Type.Poison:
                    typeName2 = "どく";
                    break;
                case Pokémon_Type.Type.Ground:
                    typeName2 = "じめん";
                    break;
                case Pokémon_Type.Type.Flying:
                    typeName2 = "ひこう";
                    break;
                case Pokémon_Type.Type.Psychic:
                    typeName2 = "エスパー";
                    break;
                case Pokémon_Type.Type.Bug:
                    typeName2 = "むし";
                    break;
                case Pokémon_Type.Type.Rock:
                    typeName2 = "いわ";
                    break;
                case Pokémon_Type.Type.Ghost:
                    typeName2 = "ゴースト";
                    break;
                case Pokémon_Type.Type.Dragon:
                    typeName2 = "ドラゴン";
                    break;
                case Pokémon_Type.Type.Dark:
                    typeName2 = "あく";
                    break;
                case Pokémon_Type.Type.Steel:
                    typeName2 = "はがね";
                    break;
                case Pokémon_Type.Type.Fairy:
                    typeName2 = "フェアリー";
                    break;
            }
            if(i.userP_Type2 != Pokémon_Type.Type.None)
            {
                pokeType.text = $"{typeName1}・{typeName2}";
            }
            else
            {
                pokeType.text = $"{typeName1}";
            }
            string personality = "";
            switch (i.userP_Personality)
            {
                case 1:
                    personality = "さみしがり";
                    break;
                case 2:
                    personality = "いじっぱり";
                    break;
                case 3:
                    personality = "やんちゃ";
                    break;
                case 4:
                    personality = "ゆうかん";
                    break;
                case 5:
                    personality = "ずぶとい";
                    break;
                case 6:
                    personality = "わんぱく";
                    break;
                case 7:
                    personality = "のうてんき";
                    break;
                case 8:
                    personality = "のんき";
                    break;
                case 9:
                    personality = "ひかえめ";
                    break;
                case 10:
                    personality = "おっとり";
                    break;
                case 11:
                    personality = "うっかりや";
                    break;
                case 12:
                    personality = "れいせい";
                    break;
                case 13:
                    personality = "おだやか";
                    break;
                case 14:
                    personality = "おとなしい";
                    break;
                case 15:
                    personality = "しんちょう";
                    break;
                case 16:
                    personality = "なまいき";
                    break;
                case 17:
                    personality = "おくびょう";
                    break;
                case 18:
                    personality = "せっかち";
                    break;
                case 19:
                    personality = "ようき";
                    break;
                case 20:
                    personality = "むじゃき";
                    break;
                case 21:
                    personality = "てれや";
                    break;
                case 22:
                    personality = "がんばりや";
                    break;
                case 23:
                    personality = "すなお";
                    break;
                case 24:
                    personality = "きまぐれ";
                    break;
                case 25:
                    personality = "まじめ";
                    break;
                default:
                    personality = "";
                    break;
            }
            pokePersonality.text = $"{personality}";
            var minValue = Experience_MinValue(i.userP_Id, i.userP_Level);
            if (i.userP_Level == 1) minValue = 0;
            pokeToTheNextLevel_Slider.minValue = minValue;
            pokeToTheNextLevel_Slider.maxValue = i.userP_UntilLevelUp;
            pokeToTheNextLevel_Slider.value = i.userP_ExperiencePoint;
            pokeExperiencePoint.text = $"{i.userP_ExperiencePoint}";
            pokeToTheNextLevel.text = $"{i.userP_UntilLevelUp - i.userP_ExperiencePoint}";
            if(i.userP_Level == 50)
            {
                pokeToTheNextLevel_Slider.value = pokeToTheNextLevel_Slider.minValue;
                pokeExperiencePoint.text = $"{pokeToTheNextLevel_Slider.maxValue}";
                pokeToTheNextLevel.text = $" - ";
            }
            pokeCharacteristic.text = $"{i.userP_Characteristic}";

            pokeNickName.text = $"{i.userP_NickName}";
            pokeLevel.text = $"Lv. {i.userP_Level}";

            if(i.userP_gender == 2)
            {
                genderImage_Status.SetActive(false);
            }
            else
            {
                genderImage_Status.SetActive(true);
                var image = genderImage_Status.GetComponent<Image>();

                if (i.userP_gender == 0)
                {
                    image.sprite = genderImageSprite[0];
                }
                else if (i.userP_gender == 1)
                {
                    image.sprite = genderImageSprite[1];
                }
            }
            if (i.isDifferentColors)
            {
                kiraImage.SetActive(true);
            }
            else
            {
                kiraImage.SetActive(false);
            }

            #region//上昇数値と下降数値に色を付けて数値を表示する
            realATK.color = defaultColor;
            realDEF.color = defaultColor;
            realSATK.color = defaultColor;
            realSDEF.color = defaultColor;
            realSPE.color = defaultColor;
            if (i.userP_Personality == 1 || i.userP_Personality == 2 || i.userP_Personality == 3 || i.userP_Personality == 4)
            {
                realATK.color = upStatusColor;
            }
            else if (i.userP_Personality == 5 || i.userP_Personality == 9 || i.userP_Personality == 13 || i.userP_Personality == 17)
            {
                realATK.color = downStatusColor;
            }
            if (i.userP_Personality == 5 || i.userP_Personality == 6 || i.userP_Personality == 7 || i.userP_Personality == 8)
            {
                realDEF.color = upStatusColor;
            }
            else if (i.userP_Personality == 1 || i.userP_Personality == 10 || i.userP_Personality == 14 || i.userP_Personality == 18)
            {
                realDEF.color = downStatusColor;
            }
            if (i.userP_Personality == 9 || i.userP_Personality == 10 || i.userP_Personality == 11 || i.userP_Personality == 12)
            {
                realSATK.color = upStatusColor;
            }
            else if (i.userP_Personality == 2 || i.userP_Personality == 6 || i.userP_Personality == 15 || i.userP_Personality == 19)
            {
                realSATK.color = downStatusColor;
            }
            if (i.userP_Personality == 13 || i.userP_Personality == 14 || i.userP_Personality == 15 || i.userP_Personality == 16)
            {
                realSDEF.color = upStatusColor;
            }
            else if (i.userP_Personality == 3 || i.userP_Personality == 7 || i.userP_Personality == 11 || i.userP_Personality == 20)
            {
                realSDEF.color = downStatusColor;
            }
            if (i.userP_Personality == 17 || i.userP_Personality == 18 || i.userP_Personality == 19 || i.userP_Personality == 20)
            {
                realSPE.color = upStatusColor;
            }
            else if (i.userP_Personality == 4 || i.userP_Personality == 8 || i.userP_Personality == 12 || i.userP_Personality == 16)
            {
                realSPE.color = downStatusColor;
            }
            realValueHP.text = $"{i.userP_Real_Hp}";
            realValueATK.text = $"{i.userP_Real_A}";
            realValueDEF.text = $"{i.userP_Real_B}";
            realValueSATK.text = $"{i.userP_Real_C}";
            realValueSDEF.text = $"{i.userP_Real_D}";
            realValueSPE.text = $"{i.userP_Real_S}";
            Elevel.text = $"{i.userP_ELevel}";
            #endregion

            Image_Classification.sprite = null;
            
            Text_Power.text = $" - ";
            Text_Hit.text = $" - ";
            Text_Explanation.text = $"";

            var c = DataLists.titleData_CharacteristicDatas.Find(x => x.c_Name == i.userP_Characteristic);
            characteristictext.transform.Find("Text").GetComponent<Text>().text = c.c_Explanation;

            #region//技を４つセットする
            var techniqueData = DataLists.titleData_Technique.Where(x => x.t_Name == i.set_Technique1);
            foreach(var ii in techniqueData)
            {
                Technique1.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.t_Name;
                Technique1.transform.Find("Text_Type").gameObject.GetComponent<Text>().text = ii.t_Type;
                var typeColor_Technique = Technique1.transform.Find("Image_Type").gameObject.GetComponent<Image>();
                switch (ii.t_Type)
                {
                    case "ノーマル":
                        typeColor_Technique.color = typeColor[0];
                        break;
                    case "ほのお":
                        typeColor_Technique.color = typeColor[1];
                        break;
                    case "みず":
                        typeColor_Technique.color = typeColor[2];
                        break;
                    case "くさ":
                        typeColor_Technique.color = typeColor[3];
                        break;
                    case "でんき":
                        typeColor_Technique.color = typeColor[4];
                        break;
                    case "こおり":
                        typeColor_Technique.color = typeColor[5];
                        break;
                    case "かくとう":
                        typeColor_Technique.color = typeColor[6];
                        break;
                    case "どく":
                        typeColor_Technique.color = typeColor[7];
                        break;
                    case "じめん":
                        typeColor_Technique.color = typeColor[8];
                        break;
                    case "ひこう":
                        typeColor_Technique.color = typeColor[9];
                        break;
                    case "エスパー":
                        typeColor_Technique.color = typeColor[10];
                        break;
                    case "むし":
                        typeColor_Technique.color = typeColor[11];
                        break;
                    case "いわ":
                        typeColor_Technique.color = typeColor[12];
                        break;
                    case "ゴースト":
                        typeColor_Technique.color = typeColor[13];
                        break;
                    case "ドラゴン":
                        typeColor_Technique.color = typeColor[14];
                        break;
                    case "あく":
                        typeColor_Technique.color = typeColor[15];
                        break;
                    case "はがね":
                        typeColor_Technique.color = typeColor[16];
                        break;
                    case "フェアリー":
                        typeColor_Technique.color = typeColor[17];
                        break;

                }
                Technique1.transform.Find("Text_PP").gameObject.GetComponent<Text>().text = $"{ii.t_PP}/{ii.t_PP}";   
            }
            techniqueData = DataLists.titleData_Technique.Where(x => x.t_Name == i.set_Technique2);
            foreach (var ii in techniqueData)
            {
                Technique2.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.t_Name;
                Technique2.transform.Find("Text_Type").gameObject.GetComponent<Text>().text = ii.t_Type;
                var typeColor_Technique = Technique2.transform.Find("Image_Type").gameObject.GetComponent<Image>();
                switch (ii.t_Type)
                {
                    case "ノーマル":
                        typeColor_Technique.color = typeColor[0];
                        break;
                    case "ほのお":
                        typeColor_Technique.color = typeColor[1];
                        break;
                    case "みず":
                        typeColor_Technique.color = typeColor[2];
                        break;
                    case "くさ":
                        typeColor_Technique.color = typeColor[3];
                        break;
                    case "でんき":
                        typeColor_Technique.color = typeColor[4];
                        break;
                    case "こおり":
                        typeColor_Technique.color = typeColor[5];
                        break;
                    case "かくとう":
                        typeColor_Technique.color = typeColor[6];
                        break;
                    case "どく":
                        typeColor_Technique.color = typeColor[7];
                        break;
                    case "じめん":
                        typeColor_Technique.color = typeColor[8];
                        break;
                    case "ひこう":
                        typeColor_Technique.color = typeColor[9];
                        break;
                    case "エスパー":
                        typeColor_Technique.color = typeColor[10];
                        break;
                    case "むし":
                        typeColor_Technique.color = typeColor[11];
                        break;
                    case "いわ":
                        typeColor_Technique.color = typeColor[12];
                        break;
                    case "ゴースト":
                        typeColor_Technique.color = typeColor[13];
                        break;
                    case "ドラゴン":
                        typeColor_Technique.color = typeColor[14];
                        break;
                    case "あく":
                        typeColor_Technique.color = typeColor[15];
                        break;
                    case "はがね":
                        typeColor_Technique.color = typeColor[16];
                        break;
                    case "フェアリー":
                        typeColor_Technique.color = typeColor[17];
                        break;

                }
                Technique2.transform.Find("Text_PP").gameObject.GetComponent<Text>().text = $"{ii.t_PP}/{ii.t_PP}";
            }
            techniqueData = DataLists.titleData_Technique.Where(x => x.t_Name == i.set_Technique3);
            foreach (var ii in techniqueData)
            {
                Technique3.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.t_Name;
                Technique3.transform.Find("Text_Type").gameObject.GetComponent<Text>().text = ii.t_Type;
                var typeColor_Technique = Technique3.transform.Find("Image_Type").gameObject.GetComponent<Image>();
                switch (ii.t_Type)
                {
                    case "ノーマル":
                        typeColor_Technique.color = typeColor[0];
                        break;
                    case "ほのお":
                        typeColor_Technique.color = typeColor[1];
                        break;
                    case "みず":
                        typeColor_Technique.color = typeColor[2];
                        break;
                    case "くさ":
                        typeColor_Technique.color = typeColor[3];
                        break;
                    case "でんき":
                        typeColor_Technique.color = typeColor[4];
                        break;
                    case "こおり":
                        typeColor_Technique.color = typeColor[5];
                        break;
                    case "かくとう":
                        typeColor_Technique.color = typeColor[6];
                        break;
                    case "どく":
                        typeColor_Technique.color = typeColor[7];
                        break;
                    case "じめん":
                        typeColor_Technique.color = typeColor[8];
                        break;
                    case "ひこう":
                        typeColor_Technique.color = typeColor[9];
                        break;
                    case "エスパー":
                        typeColor_Technique.color = typeColor[10];
                        break;
                    case "むし":
                        typeColor_Technique.color = typeColor[11];
                        break;
                    case "いわ":
                        typeColor_Technique.color = typeColor[12];
                        break;
                    case "ゴースト":
                        typeColor_Technique.color = typeColor[13];
                        break;
                    case "ドラゴン":
                        typeColor_Technique.color = typeColor[14];
                        break;
                    case "あく":
                        typeColor_Technique.color = typeColor[15];
                        break;
                    case "はがね":
                        typeColor_Technique.color = typeColor[16];
                        break;
                    case "フェアリー":
                        typeColor_Technique.color = typeColor[17];
                        break;

                }
                Technique3.transform.Find("Text_PP").gameObject.GetComponent<Text>().text = $"{ii.t_PP}/{ii.t_PP}";
            }
            techniqueData = DataLists.titleData_Technique.Where(x => x.t_Name == i.set_Technique4);
            foreach (var ii in techniqueData)
            {
                Technique4.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.t_Name;
                Technique4.transform.Find("Text_Type").gameObject.GetComponent<Text>().text = ii.t_Type;
                var typeColor_Technique = Technique4.transform.Find("Image_Type").gameObject.GetComponent<Image>();
                switch (ii.t_Type)
                {
                    case "ノーマル":
                        typeColor_Technique.color = typeColor[0];
                        break;
                    case "ほのお":
                        typeColor_Technique.color = typeColor[1];
                        break;
                    case "みず":
                        typeColor_Technique.color = typeColor[2];
                        break;
                    case "くさ":
                        typeColor_Technique.color = typeColor[3];
                        break;
                    case "でんき":
                        typeColor_Technique.color = typeColor[4];
                        break;
                    case "こおり":
                        typeColor_Technique.color = typeColor[5];
                        break;
                    case "かくとう":
                        typeColor_Technique.color = typeColor[6];
                        break;
                    case "どく":
                        typeColor_Technique.color = typeColor[7];
                        break;
                    case "じめん":
                        typeColor_Technique.color = typeColor[8];
                        break;
                    case "ひこう":
                        typeColor_Technique.color = typeColor[9];
                        break;
                    case "エスパー":
                        typeColor_Technique.color = typeColor[10];
                        break;
                    case "むし":
                        typeColor_Technique.color = typeColor[11];
                        break;
                    case "いわ":
                        typeColor_Technique.color = typeColor[12];
                        break;
                    case "ゴースト":
                        typeColor_Technique.color = typeColor[13];
                        break;
                    case "ドラゴン":
                        typeColor_Technique.color = typeColor[14];
                        break;
                    case "あく":
                        typeColor_Technique.color = typeColor[15];
                        break;
                    case "はがね":
                        typeColor_Technique.color = typeColor[16];
                        break;
                    case "フェアリー":
                        typeColor_Technique.color = typeColor[17];
                        break;

                }
                Technique4.transform.Find("Text_PP").gameObject.GetComponent<Text>().text = $"{ii.t_PP}/{ii.t_PP}";
            }
            #endregion

            Change_Unique_ID = i.unique_Id;

            var voice = voiceData.sheet.Where(x => x.p_Id == i.userP_Id);
            foreach(var ii in voice)
            {
                audioManager_Voice.PlayOneShot(ii.voiceData);
            }
            Techniqu_Status_Set(Change_Technique_ID);
            
            if(content_levelUpItem.transform.childCount != levelupItemValue || FlagManager.isBuy)
            {
                FlagManager.isBuy = false;
                EvolutionCheck(i.userP_Level, i.userP_Id);
            }

            #region//努力値表示
            textValue_HP.text = $"{i.userP_Effort_Hp}";
            textValue_ATK.text = $"{i.userP_Effort_A}";
            textValue_DEF.text = $"{i.userP_Effort_B}";
            textValue_SATK.text = $"{i.userP_Effort_C}";
            textValue_SDEF.text = $"{i.userP_Effort_D}";
            textValue_SPE.text = $"{i.userP_Effort_S}";
            #endregion

            #region//個体値表示
            textValue_Indi_HP.text = $"{i.userP_Individual_Hp}";
            textValue_Indi_ATK.text = $"{i.userP_Individual_A}";
            textValue_Indi_DEF.text = $"{i.userP_Individual_B}";
            textValue_Indi_SATK.text = $"{i.userP_Individual_C}";
            textValue_Indi_SDEF.text = $"{i.userP_Individual_D}";
            textValue_Indi_SPE.text = $"{i.userP_Individual_S}";
            #endregion

            ELevelSystem.SetActive(true);
            Text_ELevel_Max.SetActive(false);
            int cost = 0;
            switch (i.userP_ELevel)
            {
                case 1:
                    Text_Cost.text = $"消費 {Cost_ELevel1}円";
                    cost = Cost_ELevel1;
                    break;
                case 2:
                    Text_Cost.text = $"消費 {Cost_ELevel2}円";
                    cost = Cost_ELevel2;
                    break;
                case 3:
                    Text_Cost.text = $"消費 {Cost_ELevel3}円";
                    cost = Cost_ELevel3;
                    break;
                case 4:
                    Text_Cost.text = $"消費 {Cost_ELevel4}円";
                    cost = Cost_ELevel4;
                    break;
                case 5:
                    ELevelSystem.SetActive(false);
                    Text_ELevel_Max.SetActive(true);
                    break;
            }

            ButtonCover.SetActive(false);
            if (DataLists.player_Money < cost)
            {
                ButtonCover.SetActive(true);
            }

            Button_BasicStatus();
        }
    }     //ステータスセット
    private int Experience_MinValue(int id, int level)
    {
        var pokeData = DataLists.titleData_Pokémon.Where(x => x.p_Id == id);
        int minValue = 0;
        foreach (var pokémon in pokeData)
        {
            switch (pokémon.p_ExperienceType)
            {
                case 60:
                    if (2 < level && level <= 50)
                    {
                        minValue = Mathf.FloorToInt(Mathf.Pow(level, 3) * (100 - level) / 50);
                    }
                    else if (50 <= level && level <= 68)
                    {
                        minValue = Mathf.FloorToInt(Mathf.Pow(level, 3) * (150 - level) / 100);
                    }
                    else if (68 <= level && level <= 98)
                    {
                        minValue = Mathf.FloorToInt(Mathf.Pow(level, 3) * Mathf.FloorToInt(637 - 10 * level / 3) / 500);
                    }
                    else if (98 <= level && level <= 100)
                    {
                        minValue = Mathf.FloorToInt(Mathf.Pow(level, 3) * (160 - level) / 100);
                    }
                    break;
                case 80:
                    minValue = Mathf.FloorToInt(Mathf.Pow(level, 3) * 0.8f);
                    break;
                case 100:
                    minValue = Mathf.FloorToInt(Mathf.Pow(level, 3));
                    break;
                case 105:
                    minValue = Mathf.FloorToInt(1.2f * Mathf.Pow(level, 3) - (15 * Mathf.Pow(level, 2)) + (100 * level - 140));
                    break;
                case 125:
                    minValue = Mathf.FloorToInt(Mathf.Pow(level, 3) * 1.25f);
                    break;
                case 164:
                    if (2 <= level && level <= 15)
                    {
                        minValue = Mathf.FloorToInt(Mathf.Pow(level, 3) * (24 + Mathf.FloorToInt((level + 1) / 3)) / 50);
                    }
                    else if (15 <= level && level <= 36)
                    {
                        minValue = Mathf.FloorToInt(Mathf.Pow(level, 3) * (14 + level) / 50);
                    }
                    else if (36 <= level && level <= 100)
                    {
                        minValue = Mathf.FloorToInt(Mathf.Pow(level, 3) * (32 + Mathf.FloorToInt(level / 2)) / 50);
                    }
                    break;
            }
        }
        return minValue;
    } //現在のレベルの最小経験値計算
    public void CloseStatus()
    {
        levelupItemValue = -1;
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
                    Debug.Log("プレイヤーデータの登録成功");
                    loading_Image.SetActive(false);
                    Status.SetActive(false);
                    List.SetActive(true);
                    text_Pokémon.SetActive(true);
                    FlagManager.isDataChange = false;
                    levelupItemValue = -1;

                    copyData_List = DataLists.playerData.pokémonsList;
                    foreach (Transform childUnit in content_List.transform)
                    {
                        Destroy(childUnit.gameObject);
                    }
                    SetList();
                    SetOrganization();
                },
                error => { Debug.Log(error.GenerateErrorReport()); error_Image.SetActive(true); });
        }
        else
        {
            Status.SetActive(false);
            List.SetActive(true);
            text_Pokémon.SetActive(true);
        }
    }            //ステータス非表示化
    public void Button_BasicStatus()
    {
        StatusButtonController(buttonImage_BasicStatus, List_BasicStatus);
    }     //基礎ステータス表示
    public void Button_BattleStatus()
    {
        StatusButtonController(buttonImage_BattleStatus, List_BattleStatus);
    }    //バトルステータス表示
    public void Button_Technique()
    {
        StatusButtonController(buttonImage_Technique, List_Technique);
    }       //技表示
    public void Button_InputEffortValue()
    {
        List_BasicStatus.SetActive(false);
        List_BattleStatus.SetActive(false);
        List_Technique.SetActive(false);
        List_EffortValueController.SetActive(true);
        List_IndividualValueController.SetActive(false);
        Image_AlwaysDisplayed.SetActive(false);

        var data = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == temporarySaveUniqueID);
        foreach(var i in data)
        {
            if(i.userP_ELevel == 5)
            {
                button_Individual.SetActive(true);
            }
            else
            {
                button_Individual.SetActive(false);
            }

            textValue_HP.text = $"{i.userP_Effort_Hp}";
            textValue_ATK.text = $"{i.userP_Effort_A}";
            textValue_DEF.text = $"{i.userP_Effort_B}";
            textValue_SATK.text = $"{i.userP_Effort_C}";
            textValue_SDEF.text = $"{i.userP_Effort_D}";
            textValue_SPE.text = $"{i.userP_Effort_S}";
        }

        EffortTotal();
    }//努力値表示
    public void Button_InputIndividualValue()
    {
        List_EffortValueController.SetActive(false);
        List_IndividualValueController.SetActive(true);
    }//個体値表示
    private void StatusButtonController(Image image ,GameObject TrueActiveObject)
    {
        buttonImage_BasicStatus.color = Color1_lv2;
        buttonImage_BattleStatus.color = Color1_lv2;
        buttonImage_Technique.color = Color1_lv2;

        image.color = defaultColor;

        List_BasicStatus.SetActive(false);
        List_BattleStatus.SetActive(false);
        List_Technique.SetActive(false);
        List_EffortValueController.SetActive(false);
        List_IndividualValueController.SetActive(false);
        TechniqueChangeDisplay.SetActive(false);

        Image_AlwaysDisplayed.SetActive(true);

        TrueActiveObject.SetActive(true);

        errorObject.SetActive(false);
    }//ステータス表示リセット

    public string Change_Unique_ID;        //表示済みポケモンのユニークID
    private int Change_Technique_ID = 1;    //表示済みポケモンの変更先技ID
    public static int levelupItemValue = -1;           //レベルアップ用アイテムの数

    const string VC_GD = "GD";
    const string VC_BP = "BP";

    public void Techniqu_Status_Set(int techniqueNum)
    {
        //public Image[] Classification = new Image[3];
        //public Image Image_Classification;
        //public Text Text_Power;
        //public Text Text_Hit;
        //public Text Text_Explanation;

        Technique1.GetComponent<Image>().color = Color1_lv2;
        Technique2.GetComponent<Image>().color = Color1_lv2;
        Technique3.GetComponent<Image>().color = Color1_lv2;
        Technique4.GetComponent<Image>().color = Color1_lv2;
        Technique1.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().color = defaultColor;
        Technique2.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().color = defaultColor;
        Technique3.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().color = defaultColor;
        Technique4.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().color = defaultColor;

        string technique_l = "";
        switch (techniqueNum)
        {
            case 1:
                technique_l = Technique1.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text;
                Technique1.GetComponent<Image>().color = Color.black;
                Technique1.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().color = Color1_lv2;
                break;
            case 2:
                technique_l = Technique2.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text;
                Technique2.GetComponent<Image>().color = Color.black;
                Technique2.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().color = Color1_lv2;
                break;
            case 3:
                technique_l = Technique3.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text;
                Technique3.GetComponent<Image>().color = Color.black;
                Technique3.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().color = Color1_lv2;
                break;
            case 4:
                technique_l = Technique4.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text;
                Technique4.GetComponent<Image>().color = Color.black;
                Technique4.transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().color = Color1_lv2;
                break;
        }
        var data = DataLists.titleData_Technique.Where(x => x.t_Name == technique_l);
        foreach (var i in data)
        {
            if(i.t_Classification == "Physics")
            {
                Image_Classification.sprite = Classification[0];
            }
            else if(i.t_Classification == "Special")
            {
                Image_Classification.sprite = Classification[1];
            }
            else if(i.t_Classification == "Change")
            {
                Image_Classification.sprite = Classification[2];
            }
            string power = "";
            string hit = "";
            if (i.t_Power == 255)
            {
                power = " - ";
            }
            else { power = i.t_Power.ToString(); }
            if (i.t_Hit == 255)
            {
                hit = " - ";
            }
            else { hit = i.t_Hit.ToString(); }
            Text_Power.text = $"{power}";
            Text_Hit.text = $"{hit}";
            Text_Explanation.text = $"{i.t_Explanation}";
        }

        Change_Technique_ID = techniqueNum;
    }//技セット

    public void TechniqueChange_Set()
    {
        Image_AlwaysDisplayed.SetActive(false);
        List_Technique.SetActive(false);

        string pokeName_l = pokeName.text;
        var data = DataLists.titleData_Remember.Where(x => x.p_T_Name == pokeName_l);
        foreach (var ii in data)
        {
            techniqueButtons[0].transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.Technique1;
            techniqueButtons[1].transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.Technique2;
            techniqueButtons[2].transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.Technique3;
            techniqueButtons[3].transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.Technique4;
            techniqueButtons[4].transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.Technique5;
            techniqueButtons[5].transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.Technique6;
            techniqueButtons[6].transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.Technique7;
            techniqueButtons[7].transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.Technique8;
            techniqueButtons[8].transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.Technique9;
            techniqueButtons[9].transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text = ii.Technique10;
        }
        for (int i = 0; i < techniqueButtons.Length; i++)
        {
            var backGround = techniqueButtons[i].GetComponent<Image>();
            var techniqueData = DataLists.titleData_Technique.Where(x => x.t_Name == techniqueButtons[i].transform.Find("Text_TechniqueName").gameObject.GetComponent<Text>().text);
            foreach(var ii in techniqueData)
            {
                switch (ii.t_Type)
                {
                    case "ノーマル":
                        backGround.color = typeColor[0];
                        break;
                    case "ほのお":
                        backGround.color = typeColor[1];
                        break;
                    case "みず":
                        backGround.color = typeColor[2];
                        break;
                    case "くさ":
                        backGround.color = typeColor[3];
                        break;
                    case "でんき":
                        backGround.color = typeColor[4];
                        break;
                    case "こおり":
                        backGround.color = typeColor[5];
                        break;
                    case "かくとう":
                        backGround.color = typeColor[6];
                        break;
                    case "どく":
                        backGround.color = typeColor[7];
                        break;
                    case "じめん":
                        backGround.color = typeColor[8];
                        break;
                    case "ひこう":
                        backGround.color = typeColor[9];
                        break;
                    case "エスパー":
                        backGround.color = typeColor[10];
                        break;
                    case "むし":
                        backGround.color = typeColor[11];
                        break;
                    case "いわ":
                        backGround.color = typeColor[12];
                        break;
                    case "ゴースト":
                        backGround.color = typeColor[13];
                        break;
                    case "ドラゴン":
                        backGround.color = typeColor[14];
                        break;
                    case "あく":
                        backGround.color = typeColor[15];
                        break;
                    case "はがね":
                        backGround.color = typeColor[16];
                        break;
                    case "フェアリー":
                        backGround.color = typeColor[17];
                        break;

                }
                string power = "";
                string hit = "";
                if(ii.t_Power == 255)
                {
                    power = " - ";
                }
                else { power = ii.t_Power.ToString(); }
                if(ii.t_Hit == 255)
                {
                    hit = " - ";
                }
                else { hit = ii.t_Hit.ToString(); }
                techniqueButtons[i].transform.Find("Text_TechniqueDetail").gameObject.GetComponent<Text>().text = $"威力 {power}/命中 {hit}";
            }
        }

        TechniqueChangeDisplay.SetActive(true);
    }    //技変更一覧表示
    public void TechniqueChange(int technique_ID)
    {
        string pokeName_l = pokeName.text;
        
        var data = DataLists.titleData_Remember.Where(x => x.p_T_Name == pokeName_l);
        foreach (var ii in data)
        {
            string t = "";
            int t_Id = 0;
            switch (technique_ID)
            {
                case 1:
                    t = ii.Technique1;
                    t_Id = ii.TechniqueID1;
                    break;
                case 2:
                    t = ii.Technique2;
                    t_Id = ii.TechniqueID2;
                    break;
                case 3:
                    t = ii.Technique3;
                    t_Id = ii.TechniqueID3;
                    break;
                case 4:
                    t = ii.Technique4;
                    t_Id = ii.TechniqueID4;
                    break;
                case 5:
                    t = ii.Technique5;
                    t_Id = ii.TechniqueID5;
                    break;
                case 6:
                    t = ii.Technique6;
                    t_Id = ii.TechniqueID6;
                    break;
                case 7:
                    t = ii.Technique7;
                    t_Id = ii.TechniqueID7;
                    break;
                case 8:
                    t = ii.Technique8;
                    t_Id = ii.TechniqueID8;
                    break;
                case 9:
                    t = ii.Technique9;
                    t_Id = ii.TechniqueID9;
                    break;
                case 10:
                    t = ii.Technique10;
                    t_Id = ii.TechniqueID10;
                    break;
            }
            var pokeData = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
            foreach(var i in pokeData)
            {
                if(Change_Technique_ID == 1)
                {
                    //2
                    if(i.set_TechniqueID2 == technique_ID)
                    {
                        (i.set_Technique1, i.set_Technique2) = (i.set_Technique2, i.set_Technique1);
                        (i.set_TechniqueID1, i.set_TechniqueID2) = (i.set_TechniqueID2, i.set_TechniqueID1);
                    }
                    //3
                    else if (i.set_TechniqueID3 == technique_ID)
                    {
                        (i.set_Technique1, i.set_Technique3) = (i.set_Technique3, i.set_Technique1);
                        (i.set_TechniqueID1, i.set_TechniqueID3) = (i.set_TechniqueID3, i.set_TechniqueID1);
                    }
                    //4
                    else if (i.set_TechniqueID4 == technique_ID)
                    {
                        (i.set_Technique1, i.set_Technique4) = (i.set_Technique4, i.set_Technique1);
                        (i.set_TechniqueID1, i.set_TechniqueID4) = (i.set_TechniqueID4, i.set_TechniqueID1);
                    }
                    else
                    {
                        i.set_Technique1 = t;
                        i.set_TechniqueID1 = t_Id;
                    }
                }
                else if (Change_Technique_ID == 2)
                {
                    //1
                    if (i.set_TechniqueID1 == technique_ID)
                    {
                        (i.set_Technique2, i.set_Technique1) = (i.set_Technique1, i.set_Technique2);
                        (i.set_TechniqueID2, i.set_TechniqueID1) = (i.set_TechniqueID1, i.set_TechniqueID2);
                    }
                    //3
                    else if (i.set_TechniqueID3 == technique_ID)
                    {
                        (i.set_Technique2, i.set_Technique3) = (i.set_Technique3, i.set_Technique2);
                        (i.set_TechniqueID2, i.set_TechniqueID3) = (i.set_TechniqueID3, i.set_TechniqueID2);
                    }
                    //4
                    else if (i.set_TechniqueID4 == technique_ID)
                    {
                        (i.set_Technique2, i.set_Technique4) = (i.set_Technique4, i.set_Technique2);
                        (i.set_TechniqueID2, i.set_TechniqueID4) = (i.set_TechniqueID4, i.set_TechniqueID2);
                    }
                    else
                    {
                        i.set_Technique2 = t;
                        i.set_TechniqueID2 = t_Id;
                    }
                }
                else if (Change_Technique_ID == 3)
                {
                    //1
                    if (i.set_TechniqueID1 == technique_ID)
                    {
                        (i.set_Technique3, i.set_Technique1) = (i.set_Technique1, i.set_Technique3);
                        (i.set_TechniqueID3, i.set_TechniqueID1) = (i.set_TechniqueID1, i.set_TechniqueID3);
                    }
                    //2
                    else if (i.set_TechniqueID2 == technique_ID)
                    {
                        (i.set_Technique3, i.set_Technique2) = (i.set_Technique2, i.set_Technique3);
                        (i.set_TechniqueID3, i.set_TechniqueID2) = (i.set_TechniqueID2, i.set_TechniqueID3);
                    }
                    //4
                    else if (i.set_TechniqueID4 == technique_ID)
                    {
                        (i.set_Technique3, i.set_Technique4) = (i.set_Technique4, i.set_Technique3);
                        (i.set_TechniqueID3, i.set_TechniqueID4) = (i.set_TechniqueID4, i.set_TechniqueID3);
                    }
                    else
                    {
                        i.set_Technique3 = t;
                        i.set_TechniqueID3 = t_Id;
                    }
                }
                else if (Change_Technique_ID == 4)
                {
                    //1
                    if (i.set_TechniqueID1 == technique_ID)
                    {
                        (i.set_Technique4, i.set_Technique1) = (i.set_Technique1, i.set_Technique4);
                        (i.set_TechniqueID4, i.set_TechniqueID1) = (i.set_TechniqueID1, i.set_TechniqueID4);
                    }
                    //2
                    else if (i.set_TechniqueID2 == technique_ID)
                    {
                        (i.set_Technique4, i.set_Technique2) = (i.set_Technique2, i.set_Technique4);
                        (i.set_TechniqueID4, i.set_TechniqueID2) = (i.set_TechniqueID2, i.set_TechniqueID4);
                    }
                    //3
                    else if (i.set_TechniqueID4 == technique_ID)
                    {
                        (i.set_Technique4, i.set_Technique3) = (i.set_Technique3, i.set_Technique4);
                        (i.set_TechniqueID4, i.set_TechniqueID3) = (i.set_TechniqueID3, i.set_TechniqueID4);
                    }
                    else
                    {
                        i.set_Technique4 = t;
                        i.set_TechniqueID4 = t_Id;
                    }
                }

                var teamUnit = DataLists.playerData.teamDatas.pokémons;
                for (int unitID = 0; unitID < teamUnit.Length; unitID++)
                {
                    if (teamUnit[unitID] == null)
                    {
                        break;
                    }

                    if (i.unique_Id == teamUnit[unitID].unique_Id)
                    {

                        teamUnit[unitID].set_Technique1 = i.set_Technique1;
                        teamUnit[unitID].set_Technique2 = i.set_Technique2;
                        teamUnit[unitID].set_Technique3 = i.set_Technique3;
                        teamUnit[unitID].set_Technique4 = i.set_Technique4;

                        teamUnit[unitID].set_TechniqueID1 = i.set_TechniqueID1;
                        teamUnit[unitID].set_TechniqueID2 = i.set_TechniqueID2;
                        teamUnit[unitID].set_TechniqueID3 = i.set_TechniqueID3;
                        teamUnit[unitID].set_TechniqueID4 = i.set_TechniqueID4;
                    }
                }
            }   
        }

        SetStatus(Change_Unique_ID);
        Button_Technique();
        Techniqu_Status_Set(Change_Technique_ID);
        TechniqueChangeDisplay.SetActive(false);
        FlagManager.isDataChange = true;
    }//技変更処理

    #region//進化・強化
    public void EvolutionCheck(int level, int id)//進化するかどうかのチェック、またレベルアップさせる表示
    {
        Evolution_Confirmation.SetActive(false);

        var data_Poké = DataLists.titleData_Pokémon.Where(x => x.p_Id == id);
        foreach(var i in data_Poké)
        {
            if (level >= 50 && i.p_EvolutionDN != 0)
            {
                levelUpSystemObject.SetActive(false);
                text_Max.SetActive(false);
                button_Evolution.SetActive(true);
            }
            else if(level >= 50 && i.p_EvolutionDN == 0)
            {
                levelUpSystemObject.SetActive(false);
                text_Max.SetActive(true);
                button_Evolution.SetActive(false);
            }
            else if(level < 50)
            {
                levelUpSystemObject.SetActive(true);
                text_Max.SetActive(false);
                button_Evolution.SetActive(false);
                text_notItem.SetActive(true);

                foreach (Transform childUnit in content_levelUpItem.transform)
                {
                    Destroy(childUnit.gameObject);
                }

                PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest
                { 
                }, result => 
                {
                    Debug.Log("インベントリの取得成功！ ");
                    DataLists.playerData_Inventry = result.Inventory;
                    var levelUpItem_XS = DataLists.playerData_Inventry.Where(x => x.DisplayName == "けいけんアメXS");
                    foreach (var item in levelUpItem_XS)
                    {
                        if(item.RemainingUses > 0)
                        {
                            GameObject unit = Instantiate(levelUpItem_Unit, content_levelUpItem.transform);
                            unit.transform.Find("Item_Icon").gameObject.GetComponent<Image>().sprite = levelUpItemImage[0];
                            unit.transform.Find("Text_ItemName").gameObject.GetComponent<Text>().text = "けいけんアメXS";
                            unit.transform.Find("Text_ItemExplanation").gameObject.GetComponent<Text>().text = "ポケモンに与えると、100経験値が獲得できる。";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { UseLevelUpItem("けいけんアメXS"); });
                            Text_levelUpItem_XS = unit.transform.Find("Text_ItemValue").gameObject.GetComponent<Text>();
                            possession_XS = item.RemainingUses.Value;
                            Text_levelUpItem_XS.text = $"× {possession_XS}";
                            text_notItem.SetActive(false);
                            unit.name = "Button_levelUpItem_XS";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { audioManager_SE.PlayOneShot(clickSE); });
                            c_itemInstanceId_XS = item.ItemInstanceId;
                        }
                    }
                    var levelUpItem_S = DataLists.playerData_Inventry.Where(x => x.DisplayName == "けいけんアメS");
                    foreach (var item in levelUpItem_S)
                    {
                        if (item.RemainingUses > 0)
                        {
                            GameObject unit = Instantiate(levelUpItem_Unit, content_levelUpItem.transform);
                            unit.transform.Find("Item_Icon").gameObject.GetComponent<Image>().sprite = levelUpItemImage[1];
                            unit.transform.Find("Text_ItemName").gameObject.GetComponent<Text>().text = "けいけんアメS";
                            unit.transform.Find("Text_ItemExplanation").gameObject.GetComponent<Text>().text = "ポケモンに与えると、800経験値が獲得できる。";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { UseLevelUpItem("けいけんアメS"); });
                            Text_levelUpItem_S = unit.transform.Find("Text_ItemValue").gameObject.GetComponent<Text>();
                            possession_S = item.RemainingUses.Value;
                            Text_levelUpItem_S.text = $"× {possession_S}";
                            text_notItem.SetActive(false);
                            unit.name = "Button_levelUpItem_S";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { audioManager_SE.PlayOneShot(clickSE); });
                            c_itemInstanceId_S = item.ItemInstanceId;
                        }
                    }
                    var levelUpItem_M = DataLists.playerData_Inventry.Where(x => x.DisplayName == "けいけんアメM");
                    foreach (var item in levelUpItem_M)
                    {
                        if (item.RemainingUses > 0)
                        {
                            GameObject unit = Instantiate(levelUpItem_Unit, content_levelUpItem.transform);
                            unit.transform.Find("Item_Icon").gameObject.GetComponent<Image>().sprite = levelUpItemImage[2];
                            unit.transform.Find("Text_ItemName").gameObject.GetComponent<Text>().text = "けいけんアメM";
                            unit.transform.Find("Text_ItemExplanation").gameObject.GetComponent<Text>().text = "ポケモンに与えると、3000経験値が獲得できる。";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { UseLevelUpItem("けいけんアメM"); });
                            Text_levelUpItem_M = unit.transform.Find("Text_ItemValue").gameObject.GetComponent<Text>();
                            possession_M = item.RemainingUses.Value;
                            Text_levelUpItem_M.text = $"× {possession_M}";
                            text_notItem.SetActive(false);
                            unit.name = "Button_levelUpItem_M";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { audioManager_SE.PlayOneShot(clickSE); });
                            c_itemInstanceId_M = item.ItemInstanceId;
                        }
                    }
                    var levelUpItem_L = DataLists.playerData_Inventry.Where(x => x.DisplayName == "けいけんアメL");
                    foreach (var item in levelUpItem_L)
                    {
                        if (item.RemainingUses > 0)
                        {
                            GameObject unit = Instantiate(levelUpItem_Unit, content_levelUpItem.transform);
                            unit.transform.Find("Item_Icon").gameObject.GetComponent<Image>().sprite = levelUpItemImage[3];
                            unit.transform.Find("Text_ItemName").gameObject.GetComponent<Text>().text = "けいけんアメL";
                            unit.transform.Find("Text_ItemExplanation").gameObject.GetComponent<Text>().text = "ポケモンに与えると、10000経験値が獲得できる。";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { UseLevelUpItem("けいけんアメL"); });
                            Text_levelUpItem_L = unit.transform.Find("Text_ItemValue").gameObject.GetComponent<Text>();
                            possession_L = item.RemainingUses.Value;
                            Text_levelUpItem_L.text = $"× {possession_L}";
                            text_notItem.SetActive(false);
                            unit.name = "Button_levelUpItem_L";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { audioManager_SE.PlayOneShot(clickSE); });
                            c_itemInstanceId_L = item.ItemInstanceId;
                        }
                    }
                    var levelUpItem_XL = DataLists.playerData_Inventry.Where(x => x.DisplayName == "けいけんアメXL");
                    foreach (var item in levelUpItem_XL)
                    {
                        if (item.RemainingUses > 0)
                        {
                            GameObject unit = Instantiate(levelUpItem_Unit, content_levelUpItem.transform);
                            unit.transform.Find("Item_Icon").gameObject.GetComponent<Image>().sprite = levelUpItemImage[4];
                            unit.transform.Find("Text_ItemName").gameObject.GetComponent<Text>().text = "けいけんアメXL";
                            unit.transform.Find("Text_ItemExplanation").gameObject.GetComponent<Text>().text = "ポケモンに与えると、30000経験値が獲得できる。";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { UseLevelUpItem("けいけんアメXL"); });
                            Text_levelUpItem_XL = unit.transform.Find("Text_ItemValue").gameObject.GetComponent<Text>();
                            possession_XL = item.RemainingUses.Value;
                            Text_levelUpItem_XL.text = $"× {possession_XL}";
                            text_notItem.SetActive(false);
                            unit.name = "Button_levelUpItem_XL";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { audioManager_SE.PlayOneShot(clickSE); });
                            c_itemInstanceId_XL = item.ItemInstanceId;
                        }
                    }
                    var levelUpItem_Mysterious = DataLists.playerData_Inventry.Where(x => x.DisplayName == "ふしぎなアメ");
                    foreach (var item in levelUpItem_Mysterious)
                    {
                        if (item.RemainingUses > 0)
                        {
                            GameObject unit = Instantiate(levelUpItem_Unit, content_levelUpItem.transform);
                            unit.transform.Find("Item_Icon").gameObject.GetComponent<Image>().sprite = levelUpItemImage[5];
                            unit.transform.Find("Text_ItemName").gameObject.GetComponent<Text>().text = "ふしぎなアメ";
                            unit.transform.Find("Text_ItemExplanation").gameObject.GetComponent<Text>().text = "ポケモンのレベルが1上がる。";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { UseLevelUpItem("ふしぎなアメ"); });
                            Text_levelUpItem_Mysterious = unit.transform.Find("Text_ItemValue").gameObject.GetComponent<Text>();
                            possession_Mysterious = item.RemainingUses.Value;
                            Text_levelUpItem_Mysterious.text = $"× {possession_Mysterious}";
                            text_notItem.SetActive(false);
                            unit.name = "Button_levelUpItem_Mysterious";
                            unit.transform.Find("Button_LevelUpItem").gameObject.GetComponent<Button>().onClick.AddListener(() => { audioManager_SE.PlayOneShot(clickSE); });
                            c_itemInstanceId_Mysterious = item.ItemInstanceId;
                        }
                    }

                    levelupItemValue = content_levelUpItem.transform.childCount;

                    result.VirtualCurrency.TryGetValue(VC_GD, out DataLists.player_Money);
                    result.VirtualCurrency.TryGetValue(VC_BP, out DataLists.player_BattlePoint);

                    Text_money.text = $"おこづかい {DataLists.player_Money}円";
                    var UnitData = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
                    foreach(var ii in UnitData)
                    {
                        int cost = 0;
                        switch (ii.userP_ELevel)
                        {
                            case 1:
                                cost = Cost_ELevel1;
                                break;
                            case 2:
                                cost = Cost_ELevel2;
                                break;
                            case 3:
                                cost = Cost_ELevel3;
                                break;
                            case 4:
                                cost = Cost_ELevel4;
                                break;
                            case 5:
                                cost = 0;
                                break;
                        }
                        ButtonCover.SetActive(false);
                        if(DataLists.player_Money < cost)
                        {
                            ButtonCover.SetActive(true);
                        }
                    }
                }, 
                error => { Debug.Log(error.GenerateErrorReport()); error_Image.SetActive(true);});
            }
        }
    }
    public void UseLevelUpItem(string itemName)//アイテムを使用したかどうか
    {
        loading_Image.SetActive(true);

        var levelUpPokémon = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
        foreach(var i in levelUpPokémon)
        {
            var minValue = Experience_MinValue(i.userP_Id, i.userP_Level);
            if (itemName == "けいけんアメXS" && possession_XS > 0)
            {
                i.userP_ExperiencePoint += 100;
                possession_XS--;
                Text_levelUpItem_XS.text = $"× {possession_XS}";
                c_possession_XS++;
                if (possession_XS <= 0)
                {
                    Destroy(content_levelUpItem.transform.Find("Button_levelUpItem_XS").gameObject);
                }

                PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
                {
                    ItemInstanceId = c_itemInstanceId_XS,
                    ConsumeCount = c_possession_XS
                }, result =>
                {
                    Debug.Log("アイテム消費成功");

                    print("けいけんアメXSの消費完了");
                    c_possession_XS = 0;

                    RealValueCalculation();
                    loading_Image.SetActive(false);
                }, error => { error_Image.SetActive(true); error.GenerateErrorReport(); });
            }
            else if (itemName == "けいけんアメS")
            {
                i.userP_ExperiencePoint += 800;
                possession_S--;
                Text_levelUpItem_S.text = $"× {possession_S}";
                c_possession_S++;
                if(possession_S <= 0)
                {
                    Destroy(content_levelUpItem.transform.Find("Button_levelUpItem_S").gameObject);
                }

                PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
                {
                    ItemInstanceId = c_itemInstanceId_S,
                    ConsumeCount = c_possession_S
                }, result =>
                {
                    print("けいけんアメSの消費完了");
                    c_possession_S = 0;

                    RealValueCalculation();
                    loading_Image.SetActive(false);
                }, error => { error_Image.SetActive(true); error.GenerateErrorReport(); });
            }
            else if (itemName == "けいけんアメM")
            {
                i.userP_ExperiencePoint += 3000;
                possession_M--;
                Text_levelUpItem_M.text = $"× {possession_M}";
                c_possession_M++;
                if (possession_M <= 0)
                {
                    Destroy(content_levelUpItem.transform.Find("Button_levelUpItem_M").gameObject);
                }

                PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
                {
                    ItemInstanceId = c_itemInstanceId_M,
                    ConsumeCount = c_possession_M
                }, result =>
                {
                    print("けいけんアメMの消費完了");
                    c_possession_M = 0;

                    RealValueCalculation();
                    loading_Image.SetActive(false);
                }, error => { error_Image.SetActive(true); error.GenerateErrorReport(); });
            }
            else if (itemName == "けいけんアメL")
            {
                i.userP_ExperiencePoint += 10000;
                possession_L--;
                Text_levelUpItem_L.text = $"× {possession_L}";
                c_possession_L++;
                if (possession_L <= 0)
                {
                    Destroy(content_levelUpItem.transform.Find("Button_levelUpItem_L").gameObject);
                }

                PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
                {
                    ItemInstanceId = c_itemInstanceId_L,
                    ConsumeCount = c_possession_L
                }, result =>
                {
                    print("けいけんアメLの消費完了");
                    c_possession_L = 0;

                    RealValueCalculation();
                    loading_Image.SetActive(false);
                }, error => { error_Image.SetActive(true); error.GenerateErrorReport(); });
            }
            else if (itemName == "けいけんアメXL")
            {
                i.userP_ExperiencePoint += 30000;
                possession_XL--;
                Text_levelUpItem_XL.text = $"× {possession_XL}";
                c_possession_XL++;
                if (possession_XL <= 0)
                {
                    Destroy(content_levelUpItem.transform.Find("Button_levelUpItem_XL").gameObject);
                }

                PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
                {
                    ItemInstanceId = c_itemInstanceId_XL,
                    ConsumeCount = c_possession_XL
                }, result =>
                {
                    print("けいけんアメXLの消費完了");
                    c_possession_XL = 0;

                    RealValueCalculation();
                    loading_Image.SetActive(false);
                }, error => { error_Image.SetActive(true); error.GenerateErrorReport(); });
            }
            else if (itemName == "ふしぎなアメ")
            {
                i.userP_Level++;
                i.userP_ExperiencePoint = minValue;
                possession_Mysterious--;
                Text_levelUpItem_Mysterious.text = $"× {possession_Mysterious}";
                c_possession_Mysterious++;
                if (possession_Mysterious <= 0)
                {
                    Destroy(content_levelUpItem.transform.Find("Button_levelUpItem_Mysterious").gameObject);
                }

                var titleData = DataLists.titleData_Pokémon.Where(x => x.p_Id == i.userP_Id);
                foreach (var ii in titleData)
                {
                    switch (ii.p_ExperienceType)
                    {
                        case 60:
                            if (2 <= i.userP_Level + 1 && i.userP_Level + 1 <= 50)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (100 - (i.userP_Level + 1)) / 50);
                            }
                            else if (50 <= i.userP_Level + 1 && i.userP_Level + 1 <= 68)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (150 - (i.userP_Level + 1)) / 100);
                            }
                            else if (68 <= i.userP_Level + 1 && i.userP_Level + 1 <= 98)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * Mathf.FloorToInt(637 - 10 * (i.userP_Level + 1) / 3) / 500);
                            }
                            else if (98 <= i.userP_Level + 1 && i.userP_Level + 1 <= 100)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (160 - (i.userP_Level + 1)) / 100);
                            }
                            break;
                        case 80:
                            i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * 0.8f);
                            break;
                        case 100:
                            i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3));
                            break;
                        case 105:
                            i.userP_UntilLevelUp = Mathf.FloorToInt(1.2f * Mathf.Pow(i.userP_Level + 1, 3) - (15 * Mathf.Pow(i.userP_Level + 1, 2)) + (100 * (i.userP_Level + 1) - 140));
                            break;
                        case 125:
                            i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * 1.25f);

                            break;
                        case 164:
                            if (2 <= i.userP_Level + 1 && i.userP_Level + 1 <= 15)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (24 + Mathf.FloorToInt(((i.userP_Level + 1) + 1) / 3)) / 50);
                            }
                            else if (15 <= i.userP_Level + 1 && i.userP_Level + 1 <= 36)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (14 + (i.userP_Level + 1)) / 50);
                            }
                            else if (36 <= i.userP_Level + 1 && i.userP_Level + 1 <= 100)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (32 + Mathf.FloorToInt((i.userP_Level + 1) / 2)) / 50);
                            }
                            break;
                    }
                }

                PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
                {
                    ItemInstanceId = c_itemInstanceId_Mysterious,
                    ConsumeCount = c_possession_Mysterious
                }, result =>
                {
                    print("ふしぎなアメの消費完了");
                    c_possession_Mysterious = 0;

                    RealValueCalculation();
                    loading_Image.SetActive(false);
                }, error => { error_Image.SetActive(true); error.GenerateErrorReport(); });
            }
            if (i.userP_UntilLevelUp < i.userP_ExperiencePoint)
            {
                while (true)
                {
                    i.userP_Level++;
                    var titleData = DataLists.titleData_Pokémon.Where(x => x.p_Id == i.userP_Id);
                    foreach (var ii in titleData)
                    {
                        switch (ii.p_ExperienceType)
                        {
                            case 60:
                                if (2 <= i.userP_Level + 1 && i.userP_Level + 1 <= 50)
                                {
                                    i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (100 - (i.userP_Level + 1)) / 50);
                                }
                                else if (50 <= i.userP_Level + 1 && i.userP_Level + 1 <= 68)
                                {
                                    i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (150 - (i.userP_Level + 1)) / 100);
                                }
                                else if (68 <= i.userP_Level + 1 && i.userP_Level + 1 <= 98)
                                {
                                    i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * Mathf.FloorToInt(637 - 10 * (i.userP_Level + 1) / 3) / 500);
                                }
                                else if (98 <= i.userP_Level + 1 && i.userP_Level + 1 <= 100)
                                {
                                    i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (160 - (i.userP_Level + 1)) / 100);
                                }
                                break;
                            case 80:
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * 0.8f);
                                break;
                            case 100:
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3));
                                break;
                            case 105:
                                i.userP_UntilLevelUp = Mathf.FloorToInt(1.2f * Mathf.Pow(i.userP_Level + 1, 3) - (15 * Mathf.Pow(i.userP_Level + 1, 2)) + (100 * (i.userP_Level + 1) - 140));
                                break;
                            case 125:
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * 1.25f);

                                break;
                            case 164:
                                if (2 <= i.userP_Level + 1 && i.userP_Level + 1 <= 15)
                                {
                                    i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (24 + Mathf.FloorToInt(((i.userP_Level + 1) + 1) / 3)) / 50);
                                }
                                else if (15 <= i.userP_Level + 1 && i.userP_Level + 1 <= 36)
                                {
                                    i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (14 + (i.userP_Level + 1)) / 50);
                                }
                                else if (36 <= i.userP_Level + 1 && i.userP_Level + 1 <= 100)
                                {
                                    i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (32 + Mathf.FloorToInt((i.userP_Level + 1) / 2)) / 50);
                                }
                                break;
                        }
                    }
                    if(i.userP_UntilLevelUp >= i.userP_ExperiencePoint)
                    {
                        break;
                    }
                }
            }

            if(i.userP_Level >= 50)
            {
                i.userP_Level = 50;

                EvolutionCheck(i.userP_Level, i.userP_Id);
            }

            pokeToTheNextLevel_Slider.minValue = minValue;
            pokeToTheNextLevel_Slider.maxValue = i.userP_UntilLevelUp;
            pokeToTheNextLevel_Slider.value = i.userP_ExperiencePoint;
            pokeExperiencePoint.text = $"{i.userP_ExperiencePoint}";
            pokeToTheNextLevel.text = $"{i.userP_UntilLevelUp - i.userP_ExperiencePoint}";
            pokeLevel.text = $"Lv. {i.userP_Level}";

            var teamUnit = DataLists.playerData.teamDatas.pokémons;
            for (int unitID = 0; unitID < teamUnit.Length; unitID++)
            {
                if(teamUnit[unitID] == null)
                {
                    break;
                }

                if (i.unique_Id == teamUnit[unitID].unique_Id)
                {
                    teamUnit[unitID].userP_ExperiencePoint = i.userP_ExperiencePoint;
                    teamUnit[unitID].userP_UntilLevelUp = i.userP_UntilLevelUp;
                    teamUnit[unitID].userP_Level = i.userP_Level;

                    teamUnit[unitID].userP_Real_Hp = i.userP_Real_Hp;
                    teamUnit[unitID].userP_Real_A = i.userP_Real_A;
                    teamUnit[unitID].userP_Real_B = i.userP_Real_B;
                    teamUnit[unitID].userP_Real_C = i.userP_Real_C;
                    teamUnit[unitID].userP_Real_D = i.userP_Real_D;
                    teamUnit[unitID].userP_Real_S = i.userP_Real_S;
                }
            }

            FlagManager.isDataChange = true;
        }
    }
    public void RealValueCalculation()//実数値計算
    {
        var Pokémon = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
        foreach(var pokémon in Pokémon)
        {
            var titleData = DataLists.titleData_Pokémon.Where(x => x.p_Id == pokémon.userP_Id);
            foreach(var i in titleData)
            {
                //HP
                pokémon.userP_Real_Hp = Mathf.FloorToInt((i.race_H * 2 + pokémon.userP_Individual_Hp + pokémon.userP_Effort_Hp / 4) * pokémon.userP_Level / 100 + pokémon.userP_Level + 10);
                pokémon.userP_CurrentHp = pokémon.userP_Real_Hp;
                //攻撃
                {
                    var poke_a = Mathf.FloorToInt((i.race_A * 2 + pokémon.userP_Individual_A + pokémon.userP_Effort_A / 4) * pokémon.userP_Level / 100 + 5);
                    if (pokémon.userP_Personality == 1 || pokémon.userP_Personality == 2 || pokémon.userP_Personality == 3 || pokémon.userP_Personality == 4)
                    {
                        poke_a = Mathf.FloorToInt(poke_a * 1.1f);
                    }
                    else if (pokémon.userP_Personality == 5 || pokémon.userP_Personality == 9 || pokémon.userP_Personality == 13 || pokémon.userP_Personality == 17)
                    {
                        poke_a = Mathf.FloorToInt(poke_a * 0.9f);
                    }
                    pokémon.userP_Real_A = Mathf.FloorToInt(poke_a);
                }
                //防御
                {
                    var poke_b = Mathf.FloorToInt((i.race_B * 2 + pokémon.userP_Individual_B + pokémon.userP_Effort_B / 4) * pokémon.userP_Level / 100 + 5);
                    if (pokémon.userP_Personality == 5 || pokémon.userP_Personality == 6 || pokémon.userP_Personality == 7 || pokémon.userP_Personality == 8)
                    {
                        poke_b = Mathf.FloorToInt(poke_b * 1.1f);
                    }
                    else if (pokémon.userP_Personality == 1 || pokémon.userP_Personality == 10 || pokémon.userP_Personality == 14 || pokémon.userP_Personality == 18)
                    {
                        poke_b = Mathf.FloorToInt(poke_b * 0.9f);
                    }
                    pokémon.userP_Real_B = Mathf.FloorToInt(poke_b);
                }
                //特攻
                {
                    var poke_c = Mathf.FloorToInt((i.race_C * 2 + pokémon.userP_Individual_C + pokémon.userP_Effort_C / 4) * pokémon.userP_Level / 100 + 5);
                    if (pokémon.userP_Personality == 9 || pokémon.userP_Personality == 10 || pokémon.userP_Personality == 11 || pokémon.userP_Personality == 12)
                    {
                        poke_c = Mathf.FloorToInt(poke_c * 1.1f);
                    }
                    else if (pokémon.userP_Personality == 2 || pokémon.userP_Personality == 6 || pokémon.userP_Personality == 15 || pokémon.userP_Personality == 19)
                    {
                        poke_c = Mathf.FloorToInt(poke_c * 0.9f);
                    }
                    pokémon.userP_Real_C = Mathf.FloorToInt(poke_c);
                }
                //特防
                {
                    var poke_d = Mathf.FloorToInt((i.race_D * 2 + pokémon.userP_Individual_D + pokémon.userP_Effort_D / 4) * pokémon.userP_Level / 100 + 5);
                    if (pokémon.userP_Personality == 13 || pokémon.userP_Personality == 14 || pokémon.userP_Personality == 15 || pokémon.userP_Personality == 16)
                    {
                        poke_d = Mathf.FloorToInt(poke_d * 1.1f);
                    }
                    else if (pokémon.userP_Personality == 3 || pokémon.userP_Personality == 7 || pokémon.userP_Personality == 11 || pokémon.userP_Personality == 20)
                    {
                        poke_d = Mathf.FloorToInt(poke_d * 0.9f);
                    }
                    pokémon.userP_Real_D = Mathf.FloorToInt(poke_d);
                }
                //素早さ
                {
                    var poke_s = Mathf.FloorToInt((i.race_S * 2 + pokémon.userP_Individual_S + pokémon.userP_Effort_S / 4) * pokémon.userP_Level / 100 + 5);
                    if (pokémon.userP_Personality == 17 || pokémon.userP_Personality == 18 || pokémon.userP_Personality == 19 || pokémon.userP_Personality == 20)
                    {
                        poke_s = Mathf.FloorToInt(poke_s * 1.1f);
                    }
                    else if (pokémon.userP_Personality == 4 || pokémon.userP_Personality == 8 || pokémon.userP_Personality == 12 || pokémon.userP_Personality == 16)
                    {
                        poke_s = Mathf.FloorToInt(poke_s * 0.9f);
                    }
                    pokémon.userP_Real_S = Mathf.FloorToInt(poke_s);
                }
                var teamData = DataLists.playerData.teamDatas.pokémons;
                for (var u = 0; u < teamData.Length; u++)
                {
                    if (teamData[u] == null) break;

                    if (teamData[u].unique_Id == pokémon.unique_Id)
                    {
                        teamData[u] = pokémon;
                    }
                }
            }
        }

        SetStatus(Change_Unique_ID);
    }
    public void EvolutionButton()      //進化確認ボタン
    {
        Evolution_Confirmation.SetActive(true);
    }
    public void EvolutionConfirmation()
    {
        var data = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
        foreach(var i in data)
        {
            var pokeData = DataLists.titleData_Pokémon.Where(x => x.p_Id == i.userP_Id);
            foreach(var ii in pokeData)
            {
                var evoData = DataLists.titleData_Pokémon.Where(x => x.p_Id == ii.p_EvolutionDN);
                foreach (var iii in evoData)
                {

                    i.userP_Id = iii.p_Id;
                    i.userP_Name = iii.p_Name;
                    i.userP_NickName = iii.p_Name;
                    i.userP_Level = 1;

                    switch (iii.p_Type1)//タイプ1
                    {
                        #region
                        case "ノーマル":
                            i.userP_Type1 = Pokémon_Type.Type.Normal;
                            break;
                        case "ほのお":
                            i.userP_Type1 = Pokémon_Type.Type.Fire;
                            break;
                        case "みず":
                            i.userP_Type1 = Pokémon_Type.Type.Water;
                            break;
                        case "でんき":
                            i.userP_Type1 = Pokémon_Type.Type.Electric;
                            break;
                        case "くさ":
                            i.userP_Type1 = Pokémon_Type.Type.Grass;
                            break;
                        case "こおり":
                            i.userP_Type1 = Pokémon_Type.Type.Ice;
                            break;
                        case "かくとう":
                            i.userP_Type1 = Pokémon_Type.Type.Fighting;
                            break;
                        case "どく":
                            i.userP_Type1 = Pokémon_Type.Type.Poison;
                            break;
                        case "じめん":
                            i.userP_Type1 = Pokémon_Type.Type.Ground;
                            break;
                        case "ひこう":
                            i.userP_Type1 = Pokémon_Type.Type.Flying;
                            break;
                        case "エスパー":
                            i.userP_Type1 = Pokémon_Type.Type.Psychic;
                            break;
                        case "むし":
                            i.userP_Type1 = Pokémon_Type.Type.Bug;
                            break;
                        case "いわ":
                            i.userP_Type1 = Pokémon_Type.Type.Rock;
                            break;
                        case "ゴースト":
                            i.userP_Type1 = Pokémon_Type.Type.Ghost;
                            break;
                        case "ドラゴン":
                            i.userP_Type1 = Pokémon_Type.Type.Dragon;
                            break;
                        case "あく":
                            i.userP_Type1 = Pokémon_Type.Type.Dark;
                            break;
                        case "はがね":
                            i.userP_Type1 = Pokémon_Type.Type.Steel;
                            break;
                        case "フェアリー":
                            i.userP_Type1 = Pokémon_Type.Type.Fairy;
                            break;
                        case "":
                        case null:
                            Debug.LogError("タイプが設定されていません");
                            break;
                        default:
                            i.userP_Type1 = Pokémon_Type.Type.Normal;
                            break;
                            #endregion
                    }
                    switch (iii.p_Type2)//タイプ2
                    {
                        #region
                        case "ノーマル":
                            i.userP_Type2 = Pokémon_Type.Type.Normal;
                            break;
                        case "ほのお":
                            i.userP_Type2 = Pokémon_Type.Type.Fire;
                            break;
                        case "みず":
                            i.userP_Type2 = Pokémon_Type.Type.Water;
                            break;
                        case "でんき":
                            i.userP_Type2 = Pokémon_Type.Type.Electric;
                            break;
                        case "くさ":
                            i.userP_Type2 = Pokémon_Type.Type.Grass;
                            break;
                        case "こおり":
                            i.userP_Type2 = Pokémon_Type.Type.Ice;
                            break;
                        case "かくとう":
                            i.userP_Type2 = Pokémon_Type.Type.Fighting;
                            break;
                        case "どく":
                            i.userP_Type2 = Pokémon_Type.Type.Poison;
                            break;
                        case "じめん":
                            i.userP_Type2 = Pokémon_Type.Type.Ground;
                            break;
                        case "ひこう":
                            i.userP_Type2 = Pokémon_Type.Type.Flying;
                            break;
                        case "エスパー":
                            i.userP_Type2 = Pokémon_Type.Type.Psychic;
                            break;
                        case "むし":
                            i.userP_Type2 = Pokémon_Type.Type.Bug;
                            break;
                        case "いわ":
                            i.userP_Type2 = Pokémon_Type.Type.Rock;
                            break;
                        case "ゴースト":
                            i.userP_Type2 = Pokémon_Type.Type.Ghost;
                            break;
                        case "ドラゴン":
                            i.userP_Type2 = Pokémon_Type.Type.Dragon;
                            break;
                        case "あく":
                            i.userP_Type2 = Pokémon_Type.Type.Dark;
                            break;
                        case "はがね":
                            i.userP_Type2 = Pokémon_Type.Type.Steel;
                            break;
                        case "フェアリー":
                            i.userP_Type2 = Pokémon_Type.Type.Fairy;
                            break;
                        case "":
                            i.userP_Type2 = Pokémon_Type.Type.None;
                            break;
                        case null:
                            i.userP_Type2 = Pokémon_Type.Type.None;
                            break;
                        default:
                            i.userP_Type1 = Pokémon_Type.Type.Normal;
                            break;
                            #endregion
                    }

                    //所持経験値
                    switch (iii.p_ExperienceType)
                    {
                        case 60:
                            if (2 < i.userP_Level && i.userP_Level <= 50)
                            {
                                i.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(i.userP_Level, 3) * (100 - i.userP_Level) / 50);
                            }
                            else if (50 <= i.userP_Level && i.userP_Level <= 68)
                            {
                                i.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(i.userP_Level, 3) * (150 - i.userP_Level) / 100);
                            }
                            else if (68 <= i.userP_Level && i.userP_Level <= 98)
                            {
                                i.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(i.userP_Level, 3) * Mathf.FloorToInt(637 - 10 * i.userP_Level / 3) / 500);
                            }
                            else if (98 <= i.userP_Level && i.userP_Level <= 100)
                            {
                                i.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(i.userP_Level, 3) * (160 - i.userP_Level) / 100);
                            }
                            break;
                        case 80:
                            i.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(i.userP_Level, 3) * 0.8f);
                            break;
                        case 100:
                            i.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(i.userP_Level, 3));
                            break;
                        case 105:
                            i.userP_ExperiencePoint = Mathf.FloorToInt(1.2f * Mathf.Pow(i.userP_Level, 3) - (15 * Mathf.Pow(i.userP_Level, 2)) + (100 * i.userP_Level - 140));
                            break;
                        case 125:
                            i.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(i.userP_Level, 3) * 1.25f);
                            break;
                        case 164:
                            if (2 <= i.userP_Level && i.userP_Level <= 15)
                            {
                                i.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(i.userP_Level, 3) * (24 + Mathf.FloorToInt((i.userP_Level + 1) / 3)) / 50);
                            }
                            else if (15 <= i.userP_Level && i.userP_Level <= 36)
                            {
                                i.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(i.userP_Level, 3) * (14 + i.userP_Level) / 50);
                            }
                            else if (36 <= i.userP_Level && i.userP_Level <= 100)
                            {
                                i.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(i.userP_Level, 3) * (32 + Mathf.FloorToInt(i.userP_Level / 2)) / 50);
                            }
                            break;
                    }
                    i.userP_ExperiencePoint = 0;
                    //次のレベルまで
                    switch (iii.p_ExperienceType)
                    {
                        case 60:
                            if (2 <= i.userP_Level + 1 && i.userP_Level + 1 <= 50)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (100 - (i.userP_Level + 1)) / 50);
                            }
                            else if (50 <= i.userP_Level + 1 && i.userP_Level + 1 <= 68)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (150 - (i.userP_Level + 1)) / 100);
                            }
                            else if (68 <= i.userP_Level + 1 && i.userP_Level + 1 <= 98)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * Mathf.FloorToInt(637 - 10 * (i.userP_Level + 1) / 3) / 500);
                            }
                            else if (98 <= i.userP_Level + 1 && i.userP_Level + 1 <= 100)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (160 - (i.userP_Level + 1)) / 100);
                            }
                            break;
                        case 80:
                            i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * 0.8f);
                            break;
                        case 100:
                            i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3));
                            break;
                        case 105:
                            i.userP_UntilLevelUp = Mathf.FloorToInt(1.2f * Mathf.Pow(i.userP_Level + 1, 3) - (15 * Mathf.Pow(i.userP_Level + 1, 2)) + (100 * (i.userP_Level + 1) - 140));
                            print(i.userP_UntilLevelUp);
                            break;
                        case 125:
                            i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * 1.25f);

                            break;
                        case 164:
                            if (2 <= i.userP_Level + 1 && i.userP_Level + 1 <= 15)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (24 + Mathf.FloorToInt(((i.userP_Level + 1) + 1) / 3)) / 50);
                            }
                            else if (15 <= i.userP_Level + 1 && i.userP_Level + 1 <= 36)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (14 + (i.userP_Level + 1)) / 50);
                            }
                            else if (36 <= i.userP_Level + 1 && i.userP_Level + 1 <= 100)
                            {
                                i.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(i.userP_Level + 1, 3) * (32 + Mathf.FloorToInt((i.userP_Level + 1) / 2)) / 50);
                            }
                            break;
                    }

                    if (i.isDreamCharacteristic)
                    {
                        i.userP_Characteristic = iii.p_Characteristic_Dream;
                    }
                    else
                    {
                        i.userP_Characteristic = iii.p_Characteristic;
                    }

                    var techData = DataLists.titleData_Remember.Where(x => x.p_T_Id == iii.p_Id);
                    foreach ( var iiii in techData)
                    {
                        switch (i.set_TechniqueID1)
                        {
                            case 1:
                                i.set_Technique1 = iiii.Technique1;
                                break;
                            case 2:
                                i.set_Technique1 = iiii.Technique2;
                                break;
                            case 3:
                                i.set_Technique1 = iiii.Technique3;
                                break;
                            case 4:
                                i.set_Technique1 = iiii.Technique4;
                                break;
                            case 5:
                                i.set_Technique1 = iiii.Technique5;
                                break;
                            case 6:
                                i.set_Technique1 = iiii.Technique6;
                                break;
                            case 7:
                                i.set_Technique1 = iiii.Technique7;
                                break;
                            case 8:
                                i.set_Technique1 = iiii.Technique8;
                                break;
                            case 9:
                                i.set_Technique1 = iiii.Technique9;
                                break;
                            case 10:
                                i.set_Technique1 = iiii.Technique10;
                                break;
                        }
                        switch (i.set_TechniqueID2)
                        {
                            case 1:
                                i.set_Technique2 = iiii.Technique1;
                                break;
                            case 2:
                                i.set_Technique2 = iiii.Technique2;
                                break;
                            case 3:
                                i.set_Technique2 = iiii.Technique3;
                                break;
                            case 4:
                                i.set_Technique2 = iiii.Technique4;
                                break;
                            case 5:
                                i.set_Technique2 = iiii.Technique5;
                                break;
                            case 6:
                                i.set_Technique2 = iiii.Technique6;
                                break;
                            case 7:
                                i.set_Technique2 = iiii.Technique7;
                                break;
                            case 8:
                                i.set_Technique2 = iiii.Technique8;
                                break;
                            case 9:
                                i.set_Technique2 = iiii.Technique9;
                                break;
                            case 10:
                                i.set_Technique2 = iiii.Technique10;
                                break;
                        }
                        switch (i.set_TechniqueID3)
                        {
                            case 1:
                                i.set_Technique3 = iiii.Technique1;
                                break;
                            case 2:
                                i.set_Technique3 = iiii.Technique2;
                                break;
                            case 3:
                                i.set_Technique3 = iiii.Technique3;
                                break;
                            case 4:
                                i.set_Technique3 = iiii.Technique4;
                                break;
                            case 5:
                                i.set_Technique3 = iiii.Technique5;
                                break;
                            case 6:
                                i.set_Technique3 = iiii.Technique6;
                                break;
                            case 7:
                                i.set_Technique3 = iiii.Technique7;
                                break;
                            case 8:
                                i.set_Technique3 = iiii.Technique8;
                                break;
                            case 9:
                                i.set_Technique3 = iiii.Technique9;
                                break;
                            case 10:
                                i.set_Technique3 = iiii.Technique10;
                                break;
                        }
                        switch (i.set_TechniqueID4)
                        {
                            case 1:
                                i.set_Technique4 = iiii.Technique1;
                                break;
                            case 2:
                                i.set_Technique4 = iiii.Technique2;
                                break;
                            case 3:
                                i.set_Technique4 = iiii.Technique3;
                                break;
                            case 4:
                                i.set_Technique4 = iiii.Technique4;
                                break;
                            case 5:
                                i.set_Technique4 = iiii.Technique5;
                                break;
                            case 6:
                                i.set_Technique4 = iiii.Technique6;
                                break;
                            case 7:
                                i.set_Technique4 = iiii.Technique7;
                                break;
                            case 8:
                                i.set_Technique4 = iiii.Technique8;
                                break;
                            case 9:
                                i.set_Technique4 = iiii.Technique9;
                                break;
                            case 10:
                                i.set_Technique4 = iiii.Technique10;
                                break;
                        }
                    }

                    RealValueCalculation();

                    var teamData = DataLists.playerData.teamDatas.pokémons;
                    for (var u = 0; u < teamData.Length; u++)
                    {
                        if (teamData[u] == null) break;

                        if(teamData[u].unique_Id == i.unique_Id)
                        {
                            teamData[u].userP_Id = i.userP_Id;
                            teamData[u].userP_Name = i.userP_Name;
                            teamData[u].userP_NickName = i.userP_NickName;
                            teamData[u].userP_Level = i.userP_Level;

                            teamData[u].userP_Type1 = i.userP_Type1;
                            teamData[u].userP_Type2 = i.userP_Type2;

                            teamData[u].userP_ExperiencePoint = i.userP_ExperiencePoint;
                            teamData[u].userP_UntilLevelUp = i.userP_UntilLevelUp;

                            teamData[u].userP_Characteristic = i.userP_Characteristic;

                            teamData[u].set_Technique1 = i.set_Technique1;
                            teamData[u].set_Technique2 = i.set_Technique2;
                            teamData[u].set_Technique3 = i.set_Technique3;
                            teamData[u].set_Technique4 = i.set_Technique4;
                        }
                    }

                    EvolutionCancel();
                    FlagManager.isDataChange = true;

                    DataLists.playerData.picList.Add(i.userP_Id);

                    button_Evolution.SetActive(false);
                    levelUpSystemObject.SetActive(true);

                    break;
                }
                break;
            }
            break;
        }
    } //進化決定ボタン
    public void EvolutionCancel()      //進化キャンセルボタン
    {
        Evolution_Confirmation.SetActive(false);
    }
    public void ELevelUpButton()
    {
        loading_Image.SetActive(true);
        var data = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
        int cost = 0;
        foreach(var i in data)
        {
            switch (i.userP_ELevel)
            {
                case 1:
                    cost = Cost_ELevel1;
                    break;
                case 2:
                    cost = Cost_ELevel2;
                    break;
                case 3:
                    cost = Cost_ELevel3;
                    break;
                case 4:
                    cost = Cost_ELevel4;
                    break;
            }
            PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest
            {
                VirtualCurrency = VC_GD,
                Amount = cost
            }, result =>
             {
                 Debug.Log("仮想通貨を減らしました");
                 loading_Image.SetActive(false);

                 DataLists.player_Money = DataLists.player_Money - cost;
                 Text_money.text = $"おこづかい {DataLists.player_Money}円";

                 i.userP_ELevel++;
                 switch (i.userP_ELevel)
                 {
                     case 1:
                         i.userP_Individual_Hp = 5;
                         i.userP_Individual_A = 5;
                         i.userP_Individual_B = 5;
                         i.userP_Individual_C = 5;
                         i.userP_Individual_D = 5;
                         i.userP_Individual_S = 5;
                         break;
                     case 2:
                         i.userP_Individual_Hp = 12;
                         i.userP_Individual_A = 12;
                         i.userP_Individual_B = 12;
                         i.userP_Individual_C = 12;
                         i.userP_Individual_D = 12;
                         i.userP_Individual_S = 12;
                         break;
                     case 3:
                         i.userP_Individual_Hp = 18;
                         i.userP_Individual_A = 18;
                         i.userP_Individual_B = 18;
                         i.userP_Individual_C = 18;
                         i.userP_Individual_D = 18;
                         i.userP_Individual_S = 18;
                         break;
                     case 4:
                         i.userP_Individual_Hp = 25;
                         i.userP_Individual_A = 25;
                         i.userP_Individual_B = 25;
                         i.userP_Individual_C = 5;
                         i.userP_Individual_D = 25;
                         i.userP_Individual_S = 25;
                         break;
                     case 5:
                         i.userP_Individual_Hp = 31;
                         i.userP_Individual_A = 31;
                         i.userP_Individual_B = 31;
                         i.userP_Individual_C = 31;
                         i.userP_Individual_D = 31;
                         i.userP_Individual_S = 31;
                         break;
                 }

                 RealValueCalculation();
                 Button_BattleStatus();

                 FlagManager.isDataChange = true;

             }, error => { Debug.Log(error.GenerateErrorReport()); error_Image.SetActive(true); });
        }
    }//強化レベルアップ
    #endregion

    #region//努力値・個体値
    public void EffortValueSetting(int switchValue)
    {
        var data = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
        int value = 0;

        int value_H = int.Parse(Regex.Replace(textValue_HP.text, @"[^0-9]", ""));
        int value_A = int.Parse(Regex.Replace(textValue_ATK.text, @"[^0-9]", ""));
        int value_B = int.Parse(Regex.Replace(textValue_DEF.text, @"[^0-9]", ""));
        int value_C = int.Parse(Regex.Replace(textValue_SATK.text, @"[^0-9]", ""));
        int value_D = int.Parse(Regex.Replace(textValue_SDEF.text, @"[^0-9]", ""));
        int value_S = int.Parse(Regex.Replace(textValue_SPE.text, @"[^0-9]", ""));

        foreach(var i in data)
        {
            int effort_Value = value_H + value_A + value_B + value_C + value_D + value_S;
            
            switch (switchValue)
            {
                case 0:
                    if(inputField_HP.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_HP.text, @"[^0-9]", ""));
                        inputField_HP.text = "";
                        if (value > 252 || value < 0) value = 252;
                        if ((effort_Value - value_H) + value >= 510) value = 510 - effort_Value;
                        textValue_HP.text = $"{value}";
                    }
                    break;
                case 1:
                    if(inputField_ATK.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_ATK.text, @"[^0-9]", ""));
                        inputField_ATK.text = "";
                        if (value > 252 || value < 0) value = 252;
                        if ((effort_Value - value_A) + value >= 510) value = 510 - effort_Value;
                        textValue_ATK.text = $"{value}";
                    }
                    break;
                case 2:
                    if(inputField_DEF.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_DEF.text, @"[^0-9]", ""));
                        inputField_DEF.text = "";
                        if (value > 252 || value < 0) value = 252;
                        if ((effort_Value - value_B) + value >= 510) value = 510 - effort_Value;
                        textValue_DEF.text = $"{value}";
                    }
                    break;
                case 3:
                    if(inputField_SATK.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_SATK.text, @"[^0-9]", ""));
                        inputField_SATK.text = "";
                        if (value > 252 || value < 0) value = 252;
                        if ((effort_Value - value_C) + value >= 510) value = 510 - effort_Value;
                        textValue_SATK.text = $"{value}";
                    }
                    break;
                case 4:
                    if(inputField_SDEF.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_SDEF.text, @"[^0-9]", ""));
                        inputField_SDEF.text = "";
                        if (value > 252 || value < 0) value = 252;
                        if ((effort_Value - value_D) + value >= 510) value = 510 - effort_Value;
                        textValue_SDEF.text = $"{value}";
                    }
                    break;
                case 5:
                    if(inputField_SPE.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_SPE.text, @"[^0-9]", ""));
                        inputField_SPE.text = "";
                        if (value > 252 || value < 0) value = 252;
                        if ((effort_Value - value_S) + value >= 510) value = 510 - effort_Value;
                        textValue_SPE.text = $"{value}";
                    }
                    break;
            }
        }
        print(value);

        EffortTotal();
    }//inputFieldで努力値を設定する
    public void EffortValueRandom()
    {
        var data = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
        int value = 0;

        textValue_HP.text = $"{value}";
        textValue_ATK.text = $"{value}";
        textValue_DEF.text = $"{value}";
        textValue_SATK.text = $"{value}";
        textValue_SDEF.text = $"{value}";
        textValue_SPE.text = $"{value}";

        foreach (var i in data)
        {
            var titleData = DataLists.titleData_Pokémon.Where(x => x.p_Id == i.userP_Id);
            foreach(var poké in titleData)
            {
                int h = poké.race_H;
                int a = poké.race_A;
                int b = poké.race_B;
                int c = poké.race_C;
                int d = poké.race_D;
                int s = poké.race_S;

                //A
                if (a >= h && a >= b && a >= c && a >= d && a >= s)
                {
                    textValue_ATK.text = $"{252}";

                    if(s > h)
                    {
                        textValue_SPE.text = $"{252}";

                        if(h > b && h > d)
                        {
                            textValue_HP.text = $"{4}";
                        }
                        else if(b > d)
                        {
                            textValue_DEF.text = $"{4}";
                        }
                        else
                        {
                            textValue_SDEF.text = $"{4}";
                        }
                    }
                    else
                    {
                        textValue_HP.text = $"{252}";

                        if (b > d)
                        {
                            textValue_DEF.text = $"{4}";
                        }
                        else
                        {
                            textValue_SDEF.text = $"{4}";
                        }
                    }
                }
                //C
                else if (c >= h && c >= a && c >= b && c >= d && c >= s)
                {
                    textValue_SATK.text = $"{252}";

                    if(s > h)
                    {
                        textValue_SPE.text = $"{252}";

                        if (h > b && h > d) textValue_HP.text = $"{4}";
                        else if (b > d) textValue_DEF.text = $"{4}";
                        else textValue_SDEF.text = $"{4}";
                    }
                    else
                    {
                        textValue_HP.text = $"{252}";

                        if (b > d) textValue_DEF.text = $"{4}";
                        else textValue_SDEF.text = $"{4}";
                    }
                }
                //S
                else if (s >= h && s >= a && s >= b && s >= c && s >= d)
                {
                    textValue_SPE.text = $"{252}";

                    if(h > c)
                    {
                        textValue_HP.text = $"{252}";

                        if (c > b && c > d) textValue_SATK.text = $"{4}";
                        else if (b > d) textValue_DEF.text = $"{4}";
                        else textValue_SDEF.text = $"{4}";
                    }
                    else
                    {
                        textValue_SATK.text = $"{252}";
                        if (h > b && h > d) textValue_HP.text = $"{4}";
                        else if (b > d) textValue_DEF.text = $"{4}";
                        else textValue_SDEF.text = $"{4}";
                    }
                }
                //HP
                else if (h >= a && h >= b && h >= c && h >= d && h >= s)
                {
                    textValue_HP.text = $"{252}";

                    if(a > s && a > b && a > d)
                    {
                        textValue_ATK.text = $"{252}";
                        textValue_DEF.text = $"{4}";
                    }
                    else if(b > d)
                    {
                        textValue_DEF.text = $"{252}";
                        textValue_SDEF.text = $"{4}";
                    }
                    else
                    {
                        textValue_SDEF.text = $"{252}";
                        textValue_DEF.text = $"{4}";
                    }
                }
                //B
                else if (b >= h && b >= a && b >= c && b >= d && b >= s)
                {
                    textValue_DEF.text = $"{252}";
                    textValue_HP.text = $"{252}";
                    textValue_SDEF.text = $"{4}";
                }
                //D
                else if (d >= h && d >= a && d >= b && d >= c && d >= s)
                {
                    textValue_SDEF.text = $"{252}";
                    textValue_HP.text = $"{252}";
                    textValue_DEF.text = $"{4}";
                }
            }
        }

        EffortTotal();
    }   //努力値ランダムセット
    public void EffortValueReset()
    {
        int value = 0;

        textValue_HP.text = $"{value}";
        textValue_ATK.text = $"{value}";
        textValue_DEF.text = $"{value}";
        textValue_SATK.text = $"{value}";
        textValue_SDEF.text = $"{value}";
        textValue_SPE.text = $"{value}";

        EffortTotal();
    }    //努力値リセット
    public void EffortDecision()
    {
        loading_Image.SetActive(true);
        var data = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
        foreach(var i in data)
        {
            i.userP_Effort_Hp = int.Parse(Regex.Replace(textValue_HP.text, @"[^0-9]", ""));
            i.userP_Effort_A = int.Parse(Regex.Replace(textValue_ATK.text, @"[^0-9]", ""));
            i.userP_Effort_B = int.Parse(Regex.Replace(textValue_DEF.text, @"[^0-9]", ""));
            i.userP_Effort_C = int.Parse(Regex.Replace(textValue_SATK.text, @"[^0-9]", ""));
            i.userP_Effort_D = int.Parse(Regex.Replace(textValue_SDEF.text, @"[^0-9]", ""));
            i.userP_Effort_S = int.Parse(Regex.Replace(textValue_SPE.text, @"[^0-9]", ""));
        }

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
                loading_Image.SetActive(false);
                RealValueCalculation();
                Button_InputEffortValue();
            },
            error => { error_Image.SetActive(true); error.GenerateErrorReport(); });
    }      //努力値決定
    public void EffortTotal()
    {
        int h = int.Parse(Regex.Replace(textValue_HP.text, @"[^0-9]", ""));
        int a = int.Parse(Regex.Replace(textValue_ATK.text, @"[^0-9]", ""));
        int b = int.Parse(Regex.Replace(textValue_DEF.text, @"[^0-9]", ""));
        int c = int.Parse(Regex.Replace(textValue_SATK.text, @"[^0-9]", ""));
        int d = int.Parse(Regex.Replace(textValue_SDEF.text, @"[^0-9]", ""));
        int s = int.Parse(Regex.Replace(textValue_SPE.text, @"[^0-9]", ""));

        text_Total_Value.text = $"合計 {h + a + b + c + d + s}";
    }         //努力値合計表示

    public void IndividualSetting(int switchValue)
    {
        var data = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
        int value = 0;

        foreach (var i in data)
        {
            switch (switchValue)
            {
                case 0:
                    if (inputField_Indi_HP.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_Indi_HP.text, @"[^0-9]", ""));
                        inputField_Indi_HP.text = "";
                        if (value > 31 || value < 0) value = 31;
                        textValue_Indi_HP.text = $"{value}";
                    }
                    break;
                case 1:
                    if (inputField_Indi_ATK.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_Indi_ATK.text, @"[^0-9]", ""));
                        inputField_Indi_ATK.text = "";
                        if (value > 31 || value < 0) value = 31;
                        textValue_Indi_ATK.text = $"{value}";
                    }
                    break;
                case 2:
                    if (inputField_Indi_DEF.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_Indi_DEF.text, @"[^0-9]", ""));
                        inputField_Indi_DEF.text = "";
                        if (value > 31 || value < 0) value = 31;
                        textValue_Indi_DEF.text = $"{value}";
                    }
                    break;
                case 3:
                    if (inputField_Indi_SATK.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_Indi_SATK.text, @"[^0-9]", ""));
                        inputField_Indi_SATK.text = "";
                        if (value > 31 || value < 0) value = 31;
                        textValue_Indi_SATK.text = $"{value}";
                    }
                    break;
                case 4:
                    if (inputField_Indi_SDEF.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_Indi_SDEF.text, @"[^0-9]", ""));
                        inputField_Indi_SDEF.text = "";
                        if (value > 31 || value < 0) value = 31;
                        textValue_Indi_SDEF.text = $"{value}";
                    }
                    break;
                case 5:
                    if (inputField_Indi_SPE.text != "")
                    {
                        value = int.Parse(Regex.Replace(inputField_Indi_SPE.text, @"[^0-9]", ""));
                        inputField_Indi_SPE.text = "";
                        if (value > 31 || value < 0) value = 31;
                        textValue_Indi_SPE.text = $"{value}";
                    }
                    break;
            }
        }
    } //inputFieldで個体値を設定する
    public void IndividualDecision()
    {
        loading_Image.SetActive(true);
        var data = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
        foreach(var i in data)
        {
            i.userP_Individual_Hp = int.Parse(Regex.Replace(textValue_Indi_HP.text, @"[^0-9]", ""));
            i.userP_Individual_A = int.Parse(Regex.Replace(textValue_Indi_ATK.text, @"[^0-9]", ""));
            i.userP_Individual_B = int.Parse(Regex.Replace(textValue_Indi_DEF.text, @"[^0-9]", ""));
            i.userP_Individual_C = int.Parse(Regex.Replace(textValue_Indi_SATK.text, @"[^0-9]", ""));
            i.userP_Individual_D = int.Parse(Regex.Replace(textValue_Indi_SDEF.text, @"[^0-9]", ""));
            i.userP_Individual_S = int.Parse(Regex.Replace(textValue_Indi_SPE.text, @"[^0-9]", ""));
        }

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
                loading_Image.SetActive(false);
                RealValueCalculation();
                Button_InputEffortValue();
                Button_InputIndividualValue();
            },
            error => { error_Image.SetActive(true); });
    }  //個体値決定
    #endregion

    public void NicknameChange()
    {
        var data = DataLists.playerData.pokémonsList.Where(x => x.unique_Id == Change_Unique_ID);
        foreach(var i in data)
        {
            string name = nicknameText.text;
            bool isMatch = false;
            foreach(var ii in Controller_AC.errorName)
            {
                if(Regex.IsMatch(name, ii, RegexOptions.IgnoreCase))
                {
                    isMatch = true;
                }
            }
            if (isMatch)
            {
                errorObject.SetActive(true);
            }
            else
            {
                if(name.Length > 2)
                {
                    i.userP_NickName = name;
                    RealValueCalculation();
                    FlagManager.isDataChange = true;
                }
                else
                {
                    errorObject.SetActive(true);
                }
            }
        }
    }

    public GameObject LGConfObj;

    public void LetGoConf()
    {
        LGConfObj.SetActive(true);
        var poke = DataLists.playerData.pokémonsList.Find(x => x.unique_Id == Change_Unique_ID);
        var image = imageData.sheet.Find(x => x.p_Id == poke.userP_Id);
        var pokeImage= LGConfObj.transform.Find("Image_Poke").GetComponent<Image>();

        if (poke.isDifferentColors)
        {
            pokeImage.sprite = image.p_ImageFront_C;
        }
        else
        {
            pokeImage.sprite = image.p_ImageFront;
        }

        LGConfObj.transform.Find("Button0").gameObject.SetActive(false);
        LGConfObj.transform.Find("Button1").gameObject.SetActive(false);
        LGConfObj.transform.Find("Text").gameObject.SetActive(false);
        LGConfObj.transform.Find("Error").gameObject.SetActive(false);

        bool isA = false;

        foreach (var i in DataLists.playerData.teamDatas.pokémons)
        {
            if(i == null)
            {
                break;
            }

            if(i.unique_Id == Change_Unique_ID)
            {
                LGConfObj.transform.Find("Button0").gameObject.SetActive(false);
                LGConfObj.transform.Find("Button1").gameObject.SetActive(false);
                LGConfObj.transform.Find("Text").gameObject.SetActive(false);
                LGConfObj.transform.Find("Error").gameObject.SetActive(true);
                isA = true;
            }
        }

        if (isA == false)
        {
            LGConfObj.transform.Find("Button0").gameObject.SetActive(true);
            LGConfObj.transform.Find("Button1").gameObject.SetActive(true);
            LGConfObj.transform.Find("Text").gameObject.SetActive(true);
            LGConfObj.transform.Find("Error").gameObject.SetActive(false);
        }
    }

    public void LetGo()
    {
        var poke = DataLists.playerData.pokémonsList.Find(x => x.unique_Id == Change_Unique_ID);
        DataLists.playerData.pokémonsList.Remove(poke);
        FlagManager.isDataChange = true;
        CloseStatus();
        LGConfObj.SetActive(false);
    }
    #endregion
}
