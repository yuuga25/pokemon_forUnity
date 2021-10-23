using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HomeController : MonoBehaviour
{
    [Header("データリスト")]
    [SerializeField] private ImageData_Pokémon imageData;
    [SerializeField] private VoiceData_Pokémon voiceData;

    [Header("ポケモンステータスUI")]
    [SerializeField] private Text text_Poké_Name;
    [SerializeField] private Text text_Poké_NickName;
    [SerializeField] private Text text_Poké_Num;
    [SerializeField] private Image image_Poké;
    [SerializeField] private GameObject[] image_Hand = new GameObject[6];
    [SerializeField] private Text text_Poké_Level;
    [SerializeField] private GameObject image_Poké_Type1;
    [SerializeField] private GameObject image_Poké_Type2;
    [SerializeField] private GameObject image_Poké_Gender;
    [SerializeField] private Sprite[] sprite_Gender = new Sprite[2];
    [SerializeField] private GameObject image_Kira;

    [Header("プレイヤーステータスUI")]
    [SerializeField] private Text text_User_Name;
    [SerializeField] private Text text_User_Rank;
    [SerializeField] private Text text_User_Money;
    [SerializeField] private Text text_User_BP;

    [Header("画像参照")]
    [SerializeField] private Sprite[] ballImages = new Sprite[4];

    [Header("カラー参照")]
    [SerializeField] private Color[] handButtonColor = new Color[2];
    [SerializeField] private Color[] typeColor = new Color[18];

    [Header("オーディオマネージャー")]
    [SerializeField] private GameObject audioManeger_SE;
    [SerializeField] private GameObject audioManeger_BGM;
    [SerializeField] private GameObject audioManeger_Voice;


    private int center_Poké_Id = 20;

    public void Start()
    {
        readCP();
    }

    public void Update()
    {
        text_User_Name.text = DataLists.playerData.user_Name;
        text_User_Rank.text = $"{DataLists.playerData.user_Rank}";
        text_User_Money.text = $"{DataLists.player_Money}円";
        text_User_BP.text = $"{DataLists.player_BattlePoint}BP";
    }

    public void readCP()
    {
        center_Poké_Id = 20;
        ChangePokémon(0);

        for (var i = 0; i < DataLists.playerData.teamDatas.pokémons.Length; i++)
        {
            if (DataLists.playerData.teamDatas.pokémons[i] != null)
            {
                image_Hand[i].SetActive(true);

                var ballImage = image_Hand[i].transform.Find("Image_Ball").gameObject.GetComponentInChildren<Image>();
                switch (DataLists.playerData.teamDatas.pokémons[i].userP_Ball)
                {
                    case Pokémon_Ball.Ball.Poké:
                        ballImage.sprite = ballImages[0];
                        break;
                    case Pokémon_Ball.Ball.Great:
                        ballImage.sprite = ballImages[1];
                        break;
                    case Pokémon_Ball.Ball.Ultra:
                        ballImage.sprite = ballImages[2];
                        break;
                    case Pokémon_Ball.Ball.Master:
                        ballImage.sprite = ballImages[3];
                        break;
                }
            }
            else if (DataLists.playerData.teamDatas.pokémons[i] == null)
            {
                image_Hand[i].SetActive(false);
            }
        }
    }

    //ポケモンが変更された時、ホームメニューのセンターイメージ及びステータス表示を変更する
    public void ChangePokémon(int buttonId)
    {
        if(center_Poké_Id != buttonId)
        {
            center_Poké_Id = buttonId;
            var pokeData = DataLists.playerData.teamDatas.pokémons[buttonId];

            text_Poké_Name.text = $"{pokeData.userP_Name}";
            text_Poké_NickName.text = $"{pokeData.userP_NickName}";
            text_Poké_Level.text = $"{pokeData.userP_Level}";
            text_Poké_Num.text = $"No.{pokeData.userP_Id}";

            var image = imageData.sheet.Where(x => x.p_Id == pokeData.userP_Id);
            var genderType = DataLists.titleData_Pokémon.Where(x => x.p_Id == pokeData.userP_Id);
            foreach(var g in genderType)
            {
                if (g.p_GenderType == true)
                {
                    if(pokeData.userP_gender == 0)
                    {
                        image = imageData.sheet.Where(x => x.p_Id == pokeData.userP_Id)
                                               .Where(x => x.genderType == ImageData_Pokémon.GenderType.maleOnly);
                    }
                    else if(pokeData.userP_gender == 1)
                    {
                        image = imageData.sheet.Where(x => x.p_Id == pokeData.userP_Id)
                                               .Where(x => x.genderType == ImageData_Pokémon.GenderType.femeleOnly);
                    }
                }
                else if (g.p_GenderType == false)
                {
                    image = imageData.sheet.Where(x => x.p_Id == pokeData.userP_Id)
                                           .Where(x => x.genderType == ImageData_Pokémon.GenderType.same);
                }
            }
            foreach(var i in image)
            {
                if (pokeData.isDifferentColors)
                {
                    image_Poké.sprite = i.p_ImageFront_C;
                    image_Kira.SetActive(true);
                }
                else
                {
                    image_Poké.sprite = i.p_ImageFront;
                    image_Kira.SetActive(false);
                }
            }

            int colorNum = 0;
            string typeName = "";
            switch (pokeData.userP_Type1)
            {
                case Pokémon_Type.Type.Normal:
                    colorNum = 0;
                    typeName = "ノーマル";
                    break;
                case Pokémon_Type.Type.Fire:
                    colorNum = 1;
                    typeName = "ほのお";
                    break;
                case Pokémon_Type.Type.Water:
                    colorNum = 2;
                    typeName = "みず";
                    break;
                case Pokémon_Type.Type.Grass:
                    colorNum = 3;
                    typeName = "くさ";
                    break;
                case Pokémon_Type.Type.Electric:
                    colorNum = 4;
                    typeName = "でんき";
                    break;
                case Pokémon_Type.Type.Ice:
                    colorNum = 5;
                    typeName = "こおり";
                    break;
                case Pokémon_Type.Type.Fighting:
                    colorNum = 6;
                    typeName = "かくとう";
                    break;
                case Pokémon_Type.Type.Poison:
                    colorNum = 7;
                    typeName = "どく";
                    break;
                case Pokémon_Type.Type.Ground:
                    colorNum = 8;
                    typeName = "じめん";
                    break;
                case Pokémon_Type.Type.Flying:
                    colorNum = 9;
                    typeName = "ひこう";
                    break;
                case Pokémon_Type.Type.Psychic:
                    colorNum = 10;
                    typeName = "エスパー";
                    break;
                case Pokémon_Type.Type.Bug:
                    colorNum = 11;
                    typeName = "むし";
                    break;
                case Pokémon_Type.Type.Rock:
                    colorNum = 12;
                    typeName = "いわ";
                    break;
                case Pokémon_Type.Type.Ghost:
                    colorNum = 13;
                    typeName = "ゴースト";
                    break;
                case Pokémon_Type.Type.Dragon:
                    colorNum = 14;
                    typeName = "ドラゴン";
                    break;
                case Pokémon_Type.Type.Dark:
                    colorNum = 15;
                    typeName = "あく";
                    break;
                case Pokémon_Type.Type.Steel:
                    colorNum = 16;
                    typeName = "はがね";
                    break;
                case Pokémon_Type.Type.Fairy:
                    colorNum = 17;
                    typeName = "フェアリー";
                    break;
            }
            image_Poké_Type1.GetComponent<Image>().color = typeColor[colorNum];
            image_Poké_Type1.transform.GetComponentInChildren<Text>().text = typeName;
            if(pokeData.userP_Type2 == Pokémon_Type.Type.None)
            {
                image_Poké_Type2.SetActive(false);
            }
            else
            {
                image_Poké_Type2.SetActive(true);
            }
            switch (pokeData.userP_Type2)
            {
                case Pokémon_Type.Type.Normal:
                    colorNum = 0;
                    typeName = "ノーマル";
                    break;
                case Pokémon_Type.Type.Fire:
                    colorNum = 1;
                    typeName = "ほのお";
                    break;
                case Pokémon_Type.Type.Water:
                    colorNum = 2;
                    typeName = "みず";
                    break;
                case Pokémon_Type.Type.Grass:
                    colorNum = 3;
                    typeName = "くさ";
                    break;
                case Pokémon_Type.Type.Electric:
                    colorNum = 4;
                    typeName = "でんき";
                    break;
                case Pokémon_Type.Type.Ice:
                    colorNum = 5;
                    typeName = "こおり";
                    break;
                case Pokémon_Type.Type.Fighting:
                    colorNum = 6;
                    typeName = "かくとう";
                    break;
                case Pokémon_Type.Type.Poison:
                    colorNum = 7;
                    typeName = "どく";
                    break;
                case Pokémon_Type.Type.Ground:
                    colorNum = 8;
                    typeName = "じめん";
                    break;
                case Pokémon_Type.Type.Flying:
                    colorNum = 9;
                    typeName = "ひこう";
                    break;
                case Pokémon_Type.Type.Psychic:
                    colorNum = 10;
                    typeName = "エスパー";
                    break;
                case Pokémon_Type.Type.Bug:
                    colorNum = 11;
                    typeName = "むし";
                    break;
                case Pokémon_Type.Type.Rock:
                    colorNum = 12;
                    typeName = "いわ";
                    break;
                case Pokémon_Type.Type.Ghost:
                    colorNum = 13;
                    typeName = "ゴースト";
                    break;
                case Pokémon_Type.Type.Dragon:
                    colorNum = 14;
                    typeName = "ドラゴン";
                    break;
                case Pokémon_Type.Type.Dark:
                    colorNum = 15;
                    typeName = "あく";
                    break;
                case Pokémon_Type.Type.Steel:
                    colorNum = 16;
                    typeName = "はがね";
                    break;
                case Pokémon_Type.Type.Fairy:
                    colorNum = 17;
                    typeName = "フェアリー";
                    break;
            }
            image_Poké_Type2.GetComponent<Image>().color = typeColor[colorNum];
            image_Poké_Type2.transform.GetComponentInChildren<Text>().text = typeName;

            if(pokeData.userP_gender == 0)
            {
                image_Poké_Gender.SetActive(true);
                image_Poké_Gender.GetComponent<Image>().sprite = sprite_Gender[0];
            }
            else if(pokeData.userP_gender == 1)
            {
                image_Poké_Gender.SetActive(true);
                image_Poké_Gender.GetComponent<Image>().sprite = sprite_Gender[1];
            }
            else if(pokeData.userP_gender == 2)
            {
                image_Poké_Gender.SetActive(false);
            }

            foreach(var i in image_Hand)
            {
                i.GetComponent<Image>().color = handButtonColor[1];
            }
            image_Hand[buttonId].GetComponent<Image>().color = handButtonColor[0];

            var voice = audioManeger_Voice.GetComponent<AudioSource>();
            var voiceID = voiceData.sheet.Where(x => x.p_Id == pokeData.userP_Id);
            foreach(var i in voiceID)
            {
                voice.PlayOneShot(i.voiceData);
            }
        }
    }
}
