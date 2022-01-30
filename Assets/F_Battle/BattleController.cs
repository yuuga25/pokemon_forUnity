using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PlayFab;

public class BattleController : MonoBehaviour
{
    public TurnScript turnScript;
    public ReplaceScript replaceScript;

    [Header("データ（スクリプタブルオブジェクト）")]
    #region データ（スクリプタブルオブジェクト）
    public ImageData_Pokémon p_ImageDatas;
    public VoiceData_Pokémon p_VoiceDatas;
    public CompatibilityData c_Data;
    public ImageData_Belongings b_ImageDatas;
    #endregion
    [Header("データ")]
    #region データ
    public List<Sprite> Image_Ball;
    public List<Color> Color_Type;
    public List<Color> Color_Gender;
    public List<Color> Color_HpBer;
    public List<Sprite> Image_Classification;
    public Color defaultColor;
    public Color upStatusColor;
    public Color downStatusColor;
    public List<Sprite> Image_Gender;
    public AudioClip click;
    public List<Color> Color_Condition;
    #endregion
    [Header("オーディオ関連")]
    #region オーディオ関連
    public AudioSource Audio_SE;
    public AudioSource Audio_Voice;
    #endregion
    [Header("プレイヤーが操作するオブジェクトたち")]
    #region プレイヤーが操作するオブジェクトたち
    public GameObject PlayerControlObj;     //プレイヤーが操作できるオブジェクト　親
    public GameObject Control_ModeSelect;   //「たたかう」などの選択画面
    public GameObject Control_Situation;    //場に出ているポケモンのステータス確認画面
    public GameObject Control_TechniqueSelect;  //技を選択する画面
    public GameObject Control_ReplaceSelect;    //入れ替えるポケモンを選択する画面
    public GameObject Control_Surrender;    //サレンダーする時の確認画面
    #endregion
    [Header("ModeSelectのオブジェクトたち")]
    #region ModeSelectのオブジェクトたち
    public GameObject On_hand_List;         //手持ちリスト
    #endregion
    [Header("バトル場のオブジェクト")]
    #region バトル場のオブジェクト
    public GameObject Pokemon1;             //自分のポケモンのUI
    public GameObject Pokemon2;             //相手のポケモンのUI
    public Animator player_Anim;            //自分のポケモンのアニメーション
    public Animator enemy_Anim;             //相手のポケモンのアニメーション

    public Text textDisplay;                //メッセージを入力するテキスト
    #endregion
    [Header("hpスライダー")]
    public Slider player_HpSlider;
    public Slider enemy_HpSlider;

    public int player_PokeTeamNum;         //場に出ているポケモンのチーム内での番号　：プレイヤー
    public int enemy_PokeTeamNum;          //場に出ているポケモンのチーム内での番号　：エネミー
    public string player_PokeName;         //場に出ているポケモンの名前　：プレイヤー
    public string enemy_PokeName;          //場に出ているポケモンの名前　：エネミー

    private void Awake()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            print("ログインしていません。");
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }

    private void Start()
    {
        BattleDatas_Default.user_BattleStatus = new BattleStatus();
        BattleDatas_Default.enemy_BattleStatus = new BattleStatus();

        PlayerControlObj.SetActive(true);

        Control_ModeSelect.SetActive(false);
        Control_Situation.SetActive(false);
        Control_TechniqueSelect.SetActive(false);
        Control_ReplaceSelect.SetActive(false);
        Control_Surrender.SetActive(false);

        #region 手持ちのポケモン表示
        for (int i = 0; i < 3; i++)
        {
            var image_UserPoke = On_hand_List.transform.Find($"P1_poke{i + 1}").GetComponent<Image>();
            var data = p_ImageDatas.sheet.Find(x => x.p_Id == BattleDatas_Default.user_PokemonData[i].userP_Id);
            if (BattleDatas_Default.user_PokemonData[i].isDifferentColors)
            {
                image_UserPoke.sprite = data.p_ImageHand_C;
            }
            else
            {
                image_UserPoke.sprite = data.p_ImageHand;
            }
        }
        var image_EnemyPoke = On_hand_List.transform.Find($"P2_poke1").GetComponent<Image>();
        var dataa = p_ImageDatas.sheet.Find(x => x.p_Id == BattleDatas_Default.enemy_PokemonData[enemy_PokeTeamNum].userP_Id);
        if (BattleDatas_Default.enemy_PokemonData[enemy_PokeTeamNum].isDifferentColors)
        {
            image_EnemyPoke.sprite = dataa.p_ImageHand_C;
        }
        else
        {
            image_EnemyPoke.sprite = dataa.p_ImageHand;
        }
        #endregion


        textDisplay.text = "";

        int player_Speed = 0;
        int enemy_Speed = 0;
        string player_Chara = "";
        string enemy_Chara = "";

        #region プレイヤーポケモンの表示・UI
        var player_Poke_Image = Pokemon1.transform.Find("Image_Poke_Player1").GetComponent<Image>();
        var poke = BattleDatas_Default.user_PokemonData[0];
        var pokeData = p_ImageDatas.sheet.Where(x => x.p_Id == poke.userP_Id);
        ImageData_Pokémon.GenderType genderType = new ImageData_Pokémon.GenderType();
        foreach (var p in pokeData)
        {
            if (p.genderType == ImageData_Pokémon.GenderType.same)
            {
                genderType = ImageData_Pokémon.GenderType.same;
                break;
            }
            else
            {
                if (poke.userP_gender == 0)
                {
                    genderType = ImageData_Pokémon.GenderType.maleOnly;
                    break;
                }
                else if (poke.userP_gender == 1)
                {
                    genderType = ImageData_Pokémon.GenderType.femeleOnly;
                    break;
                }
            }
        }
        var image = pokeData.Where(x => x.genderType == genderType);
        foreach (var pp in image)
        {
            if (poke.isDifferentColors)
            {
                player_Poke_Image.sprite = pp.p_ImageBack_C;
            }
            else
            {
                player_Poke_Image.sprite = pp.p_ImageBack;
            }
            break;
        }

        var HpBer_Player = Pokemon1.transform.Find("HPBer_Player1");
        var slider_Player = HpBer_Player.transform.Find("Slider").GetComponent<Slider>();
        slider_Player.minValue = Mathf.Floor(poke.userP_Real_Hp / 36.4f * -1);
        slider_Player.maxValue = poke.userP_Real_Hp;
        slider_Player.value = BattleDatas_Default.user_OthersStatus[0].hp;

        HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
        if (BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].condition != BattleEnum.condition.none)
        {
            var condition = HpBer_Player.transform.Find("condition");
            condition.gameObject.SetActive(true);
            Color conditionColor = new Color();
            string conditionName = "";
            switch (BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].condition)
            {
                case BattleEnum.condition.paralysis:
                    conditionColor = Color_Condition[0];
                    conditionName = "まひ";
                    break;
                case BattleEnum.condition.ice:
                    conditionColor = Color_Condition[1];
                    conditionName = "こおり";
                    break;
                case BattleEnum.condition.burn:
                    conditionColor = Color_Condition[2];
                    conditionName = "やけど";
                    break;
                case BattleEnum.condition.poison:
                    conditionColor = Color_Condition[3];
                    conditionName = "どく";
                    break;
                case BattleEnum.condition.veryPoisonous:
                    conditionColor = Color_Condition[3];
                    conditionName = "どく";
                    break;
                case BattleEnum.condition.sleep:
                    conditionColor = Color_Condition[4];
                    conditionName = "ねむり";
                    break;
                case BattleEnum.condition.dying:
                    conditionColor = Color_Condition[5];
                    conditionName = "ひんし";
                    break;
            }
            condition.gameObject.GetComponent<Image>().color = conditionColor;
            condition.Find("Text").GetComponent<Text>().text = conditionName;
        }

        HpBer_Player.transform.Find("Text_PokeName").GetComponent<Text>().text = poke.userP_NickName;
        HpBer_Player.transform.Find("Text_HPValue").GetComponent<Text>().text = $"{BattleDatas_Default.user_OthersStatus[0].hp}/{poke.userP_Real_Hp}";

        var genderText = HpBer_Player.Find("Text_Gender").GetComponent<Text>();
        genderText.gameObject.SetActive(true); 
        if (poke.userP_gender == 0)
        {
            genderText.text = "♂"; genderText.color = Color_Gender[0];
        }
        else if (poke.userP_gender == 1)
        {
            genderText.text = "♀"; genderText.color = Color_Gender[1];
        }
        else if(poke.userP_gender == 2)
        {
            genderText.gameObject.SetActive(false);
        }
        HpBer_Player.Find("Text_Level").GetComponent<Text>().text = poke.userP_Level.ToString();

        var voice_Player = p_VoiceDatas.sheet.Find(x => x.p_Id == poke.userP_Id).voiceData;
        player_PokeName = poke.userP_NickName;

        player_Speed = poke.userP_Real_S;
        if(BattleDatas_Default.user_OthersStatus[0].b_item == "こだわりスカーフ")
        {
            player_Speed = Mathf.FloorToInt(player_Speed * 1.5f);
        }
        player_Chara = poke.userP_Characteristic;

        #endregion
        #region エネミーポケモンの表示・UI
        var enemy_Poke_Image = Pokemon2.transform.Find("Image_Poke_Player2").GetComponent<Image>();
        poke = BattleDatas_Default.enemy_PokemonData[0];
        pokeData = p_ImageDatas.sheet.Where(x => x.p_Id == poke.userP_Id);
        foreach (var p in pokeData)
        {
            if (p.genderType == ImageData_Pokémon.GenderType.same)
            {
                genderType = ImageData_Pokémon.GenderType.same;
                break;
            }
            else
            {
                if (poke.userP_gender == 0)
                {
                    genderType = ImageData_Pokémon.GenderType.maleOnly;
                    break;
                }
                else if (poke.userP_gender == 1)
                {
                    genderType = ImageData_Pokémon.GenderType.femeleOnly;
                    break;
                }
            }
        }
        image = pokeData.Where(x => x.genderType == genderType);
        foreach (var pp in image)
        {
            if (poke.isDifferentColors)
            {
                enemy_Poke_Image.sprite = pp.p_ImageFront_C;
            }
            else
            {
                enemy_Poke_Image.sprite = pp.p_ImageFront;
            }
            break;
        }

        HpBer_Player = Pokemon2.transform.Find("HPBer_Player2");
        slider_Player = HpBer_Player.transform.Find("Slider").GetComponent<Slider>();
        slider_Player.minValue = Mathf.Floor(poke.userP_Real_Hp / 36.4f * -1);
        slider_Player.maxValue = poke.userP_Real_Hp;
        slider_Player.value = BattleDatas_Default.enemy_OthersStatus[0].hp;

        HpBer_Player.transform.Find("condition").gameObject.SetActive(false);
        if (BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].condition != BattleEnum.condition.none)
        {
            var condition = HpBer_Player.transform.Find("condition");
            condition.gameObject.SetActive(true);
            Color conditionColor = new Color();
            string conditionName = "";
            switch (BattleDatas_Default.enemy_OthersStatus[enemy_PokeTeamNum].condition)
            {
                case BattleEnum.condition.paralysis:
                    conditionColor = Color_Condition[0];
                    conditionName = "まひ";
                    break;
                case BattleEnum.condition.ice:
                    conditionColor = Color_Condition[1];
                    conditionName = "こおり";
                    break;
                case BattleEnum.condition.burn:
                    conditionColor = Color_Condition[2];
                    conditionName = "やけど";
                    break;
                case BattleEnum.condition.poison:
                    conditionColor = Color_Condition[3];
                    conditionName = "どく";
                    break;
                case BattleEnum.condition.veryPoisonous:
                    conditionColor = Color_Condition[3];
                    conditionName = "どく";
                    break;
                case BattleEnum.condition.sleep:
                    conditionColor = Color_Condition[4];
                    conditionName = "ねむり";
                    break;
                case BattleEnum.condition.dying:
                    conditionColor = Color_Condition[5];
                    conditionName = "ひんし";
                    break;
            }
            condition.gameObject.GetComponent<Image>().color = conditionColor;
            condition.Find("Text").GetComponent<Text>().text = conditionName;
        }

        HpBer_Player.transform.Find("Text_PokeName").GetComponent<Text>().text = poke.userP_Name;

        genderText = HpBer_Player.Find("Text_Gender").GetComponent<Text>();
        genderText.gameObject.SetActive(true);
        if (poke.userP_gender == 0)
        {
            genderText.text = "♂"; genderText.color = Color_Gender[0];
        }
        else if (poke.userP_gender == 1)
        {
            genderText.text = "♀"; genderText.color = Color_Gender[1];
        }
        else if (poke.userP_gender == 2)
        {
            genderText.gameObject.SetActive(false);
        }
        HpBer_Player.Find("Text_Level").GetComponent<Text>().text = poke.userP_Level.ToString();

        var voice_Enemy = p_VoiceDatas.sheet.Find(x => x.p_Id == poke.userP_Id).voiceData;
        enemy_PokeName = poke.userP_Name;

        enemy_Speed = poke.userP_Real_S;
        if (BattleDatas_Default.enemy_OthersStatus[0].b_item == "こだわりスカーフ")
        {
            enemy_Speed = Mathf.FloorToInt(enemy_Speed * 1.5f);
        }

        enemy_Chara = poke.userP_Characteristic;

        #endregion
        StartCoroutine(inPokemon());

        IEnumerator inPokemon()
        {
            yield return new WaitForSeconds(3);
            enemy_Anim.SetBool("p_In", true);
            Audio_SE.PlayOneShot(turnScript.inOut_Audio[0]);
            yield return new WaitForSeconds(0.25f);
            textDisplay.text = $"{enemy_PokeName}があらわれた！";
            yield return new WaitForSeconds(0.75f);
            Audio_Voice.PlayOneShot(voice_Enemy);
            yield return new WaitForSeconds(3);
            player_Anim.SetBool("p_In", true);
            Audio_SE.PlayOneShot(turnScript.inOut_Audio[0]);
            textDisplay.text = $"行け！{player_PokeName}！";
            yield return new WaitForSeconds(1);
            Audio_Voice.PlayOneShot(voice_Player);
            yield return new WaitForSeconds(2);

            var p1_Image_Characteristic = Pokemon1.transform.Find("Image_Characteristic");
            var p2_Image_Characteristic = Pokemon2.transform.Find("Image_Characteristic");

            var player1 = BattleDatas_Default.user_PokemonData[player_PokeTeamNum];
            var player2 = BattleDatas_Default.enemy_PokemonData[enemy_PokeTeamNum];

            var anim1 = player_Anim;
            var anim2 = enemy_Anim;

            if(player_Speed < enemy_Speed)
            {
                (player1, player2) = (player2, player1);
                (p1_Image_Characteristic, p2_Image_Characteristic) = (p2_Image_Characteristic, p1_Image_Characteristic);
                (anim1, anim2) = (anim2, anim1);
            }
            else if(player_Speed == enemy_Speed)
            {
                int r = Random.Range(0, 100);
                if (r >= 50)
                {
                    (player1, player2) = (player2, player1);
                    (p1_Image_Characteristic, p2_Image_Characteristic) = (p2_Image_Characteristic, p1_Image_Characteristic);
                    (anim1, anim2) = (anim2, anim1);
                }
            }

            for(var c = 0; c < 2; c++)
            {
                if(c == 1)
                {
                    (player1, player2) = (player2, player1);
                    (p1_Image_Characteristic, p2_Image_Characteristic) = (p2_Image_Characteristic, p1_Image_Characteristic);
                    (anim1, anim2) = (anim2, anim1);
                }

                if (player1.userP_Characteristic == "きけんよち")
                {
                    bool isFear = false;
                    for (var i = 0; i < 4; i++)
                    {
                        string technique = "";
                        switch (i)
                        {
                            case 0:
                                technique = player2.set_Technique1;
                                break;
                            case 1:
                                technique = player2.set_Technique2;
                                break;
                            case 2:
                                technique = player2.set_Technique3;
                                break;
                            case 3:
                                technique = player2.set_Technique4;
                                break;
                        }

                        var techData = DataLists.titleData_Technique.Find(x => x.t_Name == technique);

                        var type = c_Data.sheet.Find(x => x.typeName == techData.t_Type);
                        if(!type.invalid.Contains(player1.userP_Type1) && !type.invalid.Contains(player1.userP_Type2))
                        {
                            if(type.twice.Contains(player1.userP_Type1) || type.twice.Contains(player1.userP_Type2))
                            {
                                isFear = true;
                            }
                        }
                    }

                    p1_Image_Characteristic.Find("Text_Name").GetComponent<Text>().text = player1.userP_Name + "の";
                    p1_Image_Characteristic.Find("Text_Characteristic").GetComponent<Text>().text = player1.userP_Characteristic;
                    if (isFear)
                    {
                        anim1.SetBool("c_In", true);
                        yield return new WaitForSeconds(1.5f);
                        anim1.SetBool("c_In", false);
                        textDisplay.text = $"{player1.userP_NickName}は みぶるいした！";
                        yield return new WaitForSeconds(2);
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.5f);
                    }
                }
                else if (player1.userP_Characteristic == "テラボルテージ")
                {
                    p1_Image_Characteristic.Find("Text_Name").GetComponent<Text>().text = player1.userP_Name + "の";
                    p1_Image_Characteristic.Find("Text_Characteristic").GetComponent<Text>().text = player1.userP_Characteristic;
                    anim1.SetBool("c_In", true);
                    yield return new WaitForSeconds(2f);
                    anim1.SetBool("c_In", false);
                }
            }

            textDisplay.text = $"行動を選択してください";
            Control_ModeSelect.SetActive(true);
            turnScript.SetBattleZone();
        }
    }

    private void Update()
    {
        if(BattleDatas_Default.enemy_OthersStatus[0].hp <= 0 && BattleDatas_Default.enemy_OthersStatus[1].hp <= 0 && BattleDatas_Default.enemy_OthersStatus[2].hp <= 0)
        {
            if (!turnScript.isLose && !turnScript.isWin)
            {
                turnScript.isWin = true;
            }
        }

        if (BattleDatas_Default.user_OthersStatus[0].hp <= 0 && BattleDatas_Default.user_OthersStatus[1].hp <= 0 && BattleDatas_Default.user_OthersStatus[2].hp <= 0)
        {
            if (!turnScript.isWin && !turnScript.isLose)
            {
                turnScript.isLose = true;
            }
        }
    }

    public void TechniqueDisplay()
    {
        var poke = BattleDatas_Default.user_PokemonData[player_PokeTeamNum];

        for(var i = 0; i < 4; i++)
        {
            var techniqueObj = Control_TechniqueSelect.transform.Find($"Technique{i + 1}");
            var button = techniqueObj.Find("Button_Technique");
            var detail = techniqueObj.Find("Technique");

            int techniqueID = 0;
            string techniqueName = "";
            switch (i)
            {
                case 0:
                    techniqueID = poke.set_TechniqueID1;
                    techniqueName = poke.set_Technique1;
                    break;
                case 1:
                    techniqueID = poke.set_TechniqueID2;
                    techniqueName = poke.set_Technique2;
                    break;
                case 2:
                    techniqueID = poke.set_TechniqueID3;
                    techniqueName = poke.set_Technique3;
                    break;
                case 3:
                    techniqueID = poke.set_TechniqueID4;
                    techniqueName = poke.set_Technique4;
                    break;
            }

            var techniqueData = DataLists.titleData_Technique.Find(x => x.t_Name==techniqueName);

            var icon_Classification = detail.Find("Image").GetComponent<Image>();
            var text_Value1 = detail.Find("Text_Value1").GetComponent<Text>();
            var text_Value2 = detail.Find("Text_Value2").GetComponent<Text>();
            var text_Expe = detail.Find("Text_Expe").GetComponent<Text>();
            switch (techniqueData.t_Classification)
            {
                case "Change":
                    icon_Classification.sprite = Image_Classification[0];
                    break;
                case "Physics":
                    icon_Classification.sprite = Image_Classification[1];
                    break;
                case "Special":
                    icon_Classification.sprite = Image_Classification[2];
                    break;
            }
            text_Value1.text = $"{techniqueData.t_Power}";
            if (techniqueData.t_Power == 255) text_Value1.text = $" - ";
            text_Value2.text = $"{techniqueData.t_Hit}";
            if(techniqueData.t_Hit == 255) text_Value2.text = $" - ";
            text_Expe.text = techniqueData.t_Explanation;

            button.Find("Text_TechniqueName").GetComponent<Text>().text = techniqueData.t_Name;
            Color buttonColor = new Color();
            switch (techniqueData.t_Type)
            {
                case "ノーマル":
                    buttonColor = Color_Type[0];
                    break;
                case "ほのお":
                    buttonColor = Color_Type[1];
                    break;
                case "みず":
                    buttonColor = Color_Type[2];
                    break;
                case "でんき":
                    buttonColor = Color_Type[3];
                    break;
                case "くさ":
                    buttonColor = Color_Type[4];
                    break;
                case "こおり":
                    buttonColor = Color_Type[5];
                    break;
                case "かくとう":
                    buttonColor = Color_Type[6];
                    break;
                case "どく":
                    buttonColor = Color_Type[7];
                    break;
                case "じめん":
                    buttonColor = Color_Type[8];
                    break;
                case "ひこう":
                    buttonColor = Color_Type[9];
                    break;
                case "エスパー":
                    buttonColor = Color_Type[10];
                    break;
                case "むし":
                    buttonColor = Color_Type[11];
                    break;
                case "いわ":
                    buttonColor = Color_Type[12];
                    break;
                case "ゴースト":
                    buttonColor = Color_Type[13];
                    break;
                case "ドラゴン":
                    buttonColor = Color_Type[14];
                    break;
                case "あく":
                    buttonColor = Color_Type[15];
                    break;
                case "はがね":
                    buttonColor = Color_Type[16];
                    break;
                case "フェアリー":
                    buttonColor = Color_Type[17];
                    break;
            }
            button.Find("Image_Type").GetComponent<Image>().color = buttonColor;
            button.Find("Text_Type").GetComponent<Text>().text = techniqueData.t_Type;
            int pp = 0; //FF1C1C
            switch (i)
            {
                case 0:
                    button.Find("Text_PP").GetComponent<Text>().text = $"{BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].pp_Technique1}/{techniqueData.t_PP}";
                    pp = BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].pp_Technique1;
                    break;
                case 1:
                    button.Find("Text_PP").GetComponent<Text>().text = $"{BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].pp_Technique2}/{techniqueData.t_PP}";
                    pp = BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].pp_Technique2;
                    break;
                case 2:
                    button.Find("Text_PP").GetComponent<Text>().text = $"{BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].pp_Technique3}/{techniqueData.t_PP}";
                    pp = BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].pp_Technique3;
                    break;
                case 3:
                    button.Find("Text_PP").GetComponent<Text>().text = $"{BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].pp_Technique4}/{techniqueData.t_PP}";
                    pp = BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].pp_Technique4;
                    break;
            }
            if(pp <= 0)
            {
                button.Find("Text_PP").GetComponent<Text>().color = new Color32(255, 28, 28, 255);
            }
            else
            {
                button.Find("Text_PP").GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }

            int magnification = 100;

            var attackType = c_Data.sheet.Find(x => x.typeName == techniqueData.t_Type);
            var enemyPoke_Data = BattleDatas_Default.enemy_PokemonData[enemy_PokeTeamNum];

            if (attackType.twice.Contains(enemyPoke_Data.userP_Type1))
            {
                magnification *= 2;
            }
            else if (attackType.half.Contains(enemyPoke_Data.userP_Type1))
            {
                magnification /= 2;
            }
            else if (attackType.invalid.Contains(enemyPoke_Data.userP_Type1))
            {
                magnification *= 0;
            }
            if (attackType.twice.Contains(enemyPoke_Data.userP_Type2))
            {
                magnification *= 2;
            }
            else if (attackType.half.Contains(enemyPoke_Data.userP_Type2))
            {
                magnification /= 2;
            }
            else if (attackType.invalid.Contains(enemyPoke_Data.userP_Type2))
            {
                magnification *= 0;
            }

            string message = "こうかあり";
            if(magnification >= 200)
            {
                message = "こうかばつぐん";
            }
            else if(magnification <= 50)
            {
                message = "こうかいまひとつ";
            }
            if(magnification <= 0)
            {
                message = "こうかなし";
            }
            if (techniqueData.t_Classification == "Change")
            {
                message = "";
            }
            button.Find("Text").GetComponent<Text>().text = message;
        }

        Control_ModeSelect.SetActive(false);
        Control_TechniqueSelect.SetActive(true);
    }

    public void SituationDiaplay()
    {
        var poke = Control_Situation.transform.Find("Poke").gameObject;

        var poke_Status = poke.transform.Find("poke_Status");
        poke.transform.Find("poke_Status").gameObject.SetActive(true);
        var Button_poke = poke.transform.Find("Button_poke");
        var poke_Situation = poke.transform.Find("poke_Situation");

        Control_Situation.transform.Find("Button_Front").gameObject.SetActive(true);
        Control_Situation.transform.Find("Button_BackGround").gameObject.SetActive(false);

        Button_poke.transform.Find("condition").gameObject.SetActive(false);
        if (BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].condition != BattleEnum.condition.none)
        {
            var condition = Button_poke.transform.Find("condition");
            condition.gameObject.SetActive(true);
            Color conditionColor = new Color();
            string conditionName = "";
            switch (BattleDatas_Default.user_OthersStatus[player_PokeTeamNum].condition)
            {
                case BattleEnum.condition.paralysis:
                    conditionColor = Color_Condition[0];
                    conditionName = "まひ";
                    break;
                case BattleEnum.condition.ice:
                    conditionColor = Color_Condition[1];
                    conditionName = "こおり";
                    break;
                case BattleEnum.condition.burn:
                    conditionColor = Color_Condition[2];
                    conditionName = "やけど";
                    break;
                case BattleEnum.condition.poison:
                    conditionColor = Color_Condition[3];
                    conditionName = "どく";
                    break;
                case BattleEnum.condition.veryPoisonous:
                    conditionColor = Color_Condition[3];
                    conditionName = "どく";
                    break;
                case BattleEnum.condition.sleep:
                    conditionColor = Color_Condition[4];
                    conditionName = "ねむり";
                    break;
                case BattleEnum.condition.dying:
                    conditionColor = Color_Condition[5];
                    conditionName = "ひんし";
                    break;
            }
            condition.gameObject.GetComponent<Image>().color = conditionColor;
            condition.Find("Text").GetComponent<Text>().text = conditionName;
        }

        var thisPoke = BattleDatas_Default.user_PokemonData[player_PokeTeamNum];
        var thisPoke_OtherStatus = BattleDatas_Default.user_OthersStatus[player_PokeTeamNum];
        var thisPoke_BattleStatus = BattleDatas_Default.user_BattleStatus;

        var type1 = poke_Status.Find("Type1");
        Color buttonColor = new Color();
        string typeName = "";
        switch (thisPoke.userP_Type1)
        {
            case Pokémon_Type.Type.Normal:
                buttonColor = Color_Type[0];
                typeName = "ノーマル";
                break;
            case Pokémon_Type.Type.Fire:
                buttonColor = Color_Type[1];
                typeName = "ほのお";
                break;
            case Pokémon_Type.Type.Water:
                buttonColor = Color_Type[2];
                typeName = "みず";
                break;
            case Pokémon_Type.Type.Electric:
                buttonColor = Color_Type[3];
                typeName = "でんき";
                break;
            case Pokémon_Type.Type.Grass:
                buttonColor = Color_Type[4];
                typeName = "くさ";
                break;
            case Pokémon_Type.Type.Ice:
                buttonColor = Color_Type[5];
                typeName = "こおり";
                break;
            case Pokémon_Type.Type.Fighting:
                buttonColor = Color_Type[6];
                typeName = "かくとう";
                break;
            case Pokémon_Type.Type.Poison:
                buttonColor = Color_Type[7];
                typeName = "どく";
                break;
            case Pokémon_Type.Type.Ground:
                buttonColor = Color_Type[8];
                typeName = "じめん";
                break;
            case Pokémon_Type.Type.Flying:
                buttonColor = Color_Type[9];
                typeName = "ひこう";
                break;
            case Pokémon_Type.Type.Psychic:
                buttonColor = Color_Type[10];
                typeName = "エスパー";
                break;
            case Pokémon_Type.Type.Bug:
                buttonColor = Color_Type[11];
                typeName = "むし";
                break;
            case Pokémon_Type.Type.Rock:
                buttonColor = Color_Type[12];
                typeName = "いわ";
                break;
            case Pokémon_Type.Type.Ghost:
                buttonColor = Color_Type[13];
                typeName = "ゴースト";
                break;
            case Pokémon_Type.Type.Dragon:
                buttonColor = Color_Type[14];
                typeName = "ドラゴン";
                break;
            case Pokémon_Type.Type.Dark:
                buttonColor = Color_Type[15];
                typeName = "あく";
                break;
            case Pokémon_Type.Type.Steel:
                buttonColor = Color_Type[16];
                typeName = "はがね";
                break;
            case Pokémon_Type.Type.Fairy:
                buttonColor = Color_Type[17];
                typeName = "フェアリー";
                break;
        }
        type1.GetComponent<Image>().color = buttonColor;
        type1.Find("Text").GetComponent<Text>().text = typeName;
        var type2 = poke_Status.Find("Type2");
        type2.gameObject.SetActive(true);
        switch (thisPoke.userP_Type2)
        {
            case Pokémon_Type.Type.None:
                type2.gameObject.SetActive(false);
                break;
            case Pokémon_Type.Type.Normal:
                buttonColor = Color_Type[0];
                typeName = "ノーマル";
                break;
            case Pokémon_Type.Type.Fire:
                buttonColor = Color_Type[1];
                typeName = "ほのお";
                break;
            case Pokémon_Type.Type.Water:
                buttonColor = Color_Type[2];
                typeName = "みず";
                break;
            case Pokémon_Type.Type.Electric:
                buttonColor = Color_Type[3];
                typeName = "でんき";
                break;
            case Pokémon_Type.Type.Grass:
                buttonColor = Color_Type[4];
                typeName = "くさ";
                break;
            case Pokémon_Type.Type.Ice:
                buttonColor = Color_Type[5];
                typeName = "こおり";
                break;
            case Pokémon_Type.Type.Fighting:
                buttonColor = Color_Type[6];
                typeName = "かくとう";
                break;
            case Pokémon_Type.Type.Poison:
                buttonColor = Color_Type[7];
                typeName = "どく";
                break;
            case Pokémon_Type.Type.Ground:
                buttonColor = Color_Type[8];
                typeName = "じめん";
                break;
            case Pokémon_Type.Type.Flying:
                buttonColor = Color_Type[9];
                typeName = "ひこう";
                break;
            case Pokémon_Type.Type.Psychic:
                buttonColor = Color_Type[10];
                typeName = "エスパー";
                break;
            case Pokémon_Type.Type.Bug:
                buttonColor = Color_Type[11];
                typeName = "むし";
                break;
            case Pokémon_Type.Type.Rock:
                buttonColor = Color_Type[12];
                typeName = "いわ";
                break;
            case Pokémon_Type.Type.Ghost:
                buttonColor = Color_Type[13];
                typeName = "ゴースト";
                break;
            case Pokémon_Type.Type.Dragon:
                buttonColor = Color_Type[14];
                typeName = "ドラゴン";
                break;
            case Pokémon_Type.Type.Dark:
                buttonColor = Color_Type[15];
                typeName = "あく";
                break;
            case Pokémon_Type.Type.Steel:
                buttonColor = Color_Type[16];
                typeName = "はがね";
                break;
            case Pokémon_Type.Type.Fairy:
                buttonColor = Color_Type[17];
                typeName = "フェアリー";
                break;
        }
        type2.GetComponent<Image>().color = buttonColor;
        type2.Find("Text").GetComponent<Text>().text = typeName;

        var realValue = poke_Status.Find("RealValue");
        realValue.Find("ValueText_Hp").GetComponent<Text>().text = thisPoke.userP_Real_Hp.ToString();
        realValue.Find("ValueText_Atk").GetComponent<Text>().text = thisPoke.userP_Real_A.ToString();
        realValue.Find("ValueText_Def").GetComponent<Text>().text = thisPoke.userP_Real_B.ToString();
        realValue.Find("ValueText_Sat").GetComponent<Text>().text = thisPoke.userP_Real_C.ToString();
        realValue.Find("ValueText_Sde").GetComponent<Text>().text = thisPoke.userP_Real_D.ToString();
        realValue.Find("ValueText_Spe").GetComponent<Text>().text = thisPoke.userP_Real_S.ToString();

        poke_Status.Find("Characteristic").Find("Text_Characteristic").GetComponent<Text>().text = thisPoke.userP_Characteristic;
        if(thisPoke_OtherStatus.b_item == "" || thisPoke_OtherStatus.b_item == null)
        {
            poke_Status.Find("Belongings").Find("Text_Belongings").GetComponent<Text>().text = " - ";
        }
        else
        {
            var itemName = b_ImageDatas.sheet.Find(x => x.imageId == thisPoke_OtherStatus.b_item);
            poke_Status.Find("Belongings").Find("Text_Belongings").GetComponent<Text>().text = itemName.displayName;
        }

        var technique = poke_Status.Find("Technique");
        for(var i = 0; i < 4; i++)
        {
            string name = "";
            int pp = 0;
            switch (i)
            {
                case 0:
                    name = thisPoke.set_Technique1;
                    pp = thisPoke_OtherStatus.pp_Technique1;
                    break;
                case 1:
                    name = thisPoke.set_Technique2;
                    pp = thisPoke_OtherStatus.pp_Technique2;
                    break;
                case 2:
                    name = thisPoke.set_Technique3;
                    pp = thisPoke_OtherStatus.pp_Technique3;
                    break;
                case 3:
                    name = thisPoke.set_Technique4;
                    pp = thisPoke_OtherStatus.pp_Technique4;
                    break;
            }
            var techniqueData = DataLists.titleData_Technique.Find(x => x.t_Name == name);

            var techniqueIcon = technique.Find($"Technique{i + 1}");

            buttonColor = new Color();
            switch (techniqueData.t_Type)
            {
                case "ノーマル":
                    buttonColor = Color_Type[0];
                    break;
                case "ほのお":
                    buttonColor = Color_Type[1];
                    break;
                case "みず":
                    buttonColor = Color_Type[2];
                    break;
                case "でんき":
                    buttonColor = Color_Type[3];
                    break;
                case "くさ":
                    buttonColor = Color_Type[4];
                    break;
                case "こおり":
                    buttonColor = Color_Type[5];
                    break;
                case "かくとう":
                    buttonColor = Color_Type[6];
                    break;
                case "どく":
                    buttonColor = Color_Type[7];
                    break;
                case "じめん":
                    buttonColor = Color_Type[8];
                    break;
                case "ひこう":
                    buttonColor = Color_Type[9];
                    break;
                case "エスパー":
                    buttonColor = Color_Type[10];
                    break;
                case "むし":
                    buttonColor = Color_Type[11];
                    break;
                case "いわ":
                    buttonColor = Color_Type[12];
                    break;
                case "ゴースト":
                    buttonColor = Color_Type[13];
                    break;
                case "ドラゴン":
                    buttonColor = Color_Type[14];
                    break;
                case "あく":
                    buttonColor = Color_Type[15];
                    break;
                case "はがね":
                    buttonColor = Color_Type[16];
                    break;
                case "フェアリー":
                    buttonColor = Color_Type[17];
                    break;
            }
            techniqueIcon.GetComponent<Image>().color = buttonColor;
            techniqueIcon.Find("Text_PP").GetComponent<Text>().text = $"{pp}/{techniqueData.t_PP}";
            techniqueIcon.Find("Text_TechniqueName").GetComponent<Text>().text = techniqueData.t_Name;
        }

        #region//上昇数値と下降数値に色を付けて数値を表示する
        var image = realValue.Find("Image");
        var realATK = image.Find("a").GetComponent<Text>();
        var realDEF = image.Find("b").GetComponent<Text>();
        var realSATK = image.Find("c").GetComponent<Text>();
        var realSDEF = image.Find("d").GetComponent<Text>();
        var realSPE = image.Find("s").GetComponent<Text>();

        realATK.color = defaultColor;
        realDEF.color = defaultColor;
        realSATK.color = defaultColor;
        realSDEF.color = defaultColor;
        realSPE.color = defaultColor;
        if (thisPoke.userP_Personality == 1 || thisPoke.userP_Personality == 2 || thisPoke.userP_Personality == 3 || thisPoke.userP_Personality == 4)
        {
            realATK.color = upStatusColor;
        }
        else if (thisPoke.userP_Personality == 5 || thisPoke.userP_Personality == 9 || thisPoke.userP_Personality == 13 || thisPoke.userP_Personality == 17)
        {
            realATK.color = downStatusColor;
        }
        if (thisPoke.userP_Personality == 5 || thisPoke.userP_Personality == 6 || thisPoke.userP_Personality == 7 || thisPoke.userP_Personality == 8)
        {
            realDEF.color = upStatusColor;
        }
        else if (thisPoke.userP_Personality == 1 || thisPoke.userP_Personality == 10 || thisPoke.userP_Personality == 14 || thisPoke.userP_Personality == 18)
        {
            realDEF.color = downStatusColor;
        }
        if (thisPoke.userP_Personality == 9 || thisPoke.userP_Personality == 10 || thisPoke.userP_Personality == 11 || thisPoke.userP_Personality == 12)
        {
            realSATK.color = upStatusColor;
        }
        else if (thisPoke.userP_Personality == 2 || thisPoke.userP_Personality == 6 || thisPoke.userP_Personality == 15 || thisPoke.userP_Personality == 19)
        {
            realSATK.color = downStatusColor;
        }
        if (thisPoke.userP_Personality == 13 || thisPoke.userP_Personality == 14 || thisPoke.userP_Personality == 15 || thisPoke.userP_Personality == 16)
        {
            realSDEF.color = upStatusColor;
        }
        else if (thisPoke.userP_Personality == 3 || thisPoke.userP_Personality == 7 || thisPoke.userP_Personality == 11 || thisPoke.userP_Personality == 20)
        {
            realSDEF.color = downStatusColor;
        }
        if (thisPoke.userP_Personality == 17 || thisPoke.userP_Personality == 18 || thisPoke.userP_Personality == 19 || thisPoke.userP_Personality == 20)
        {
            realSPE.color = upStatusColor;
        }
        else if (thisPoke.userP_Personality == 4 || thisPoke.userP_Personality == 8 || thisPoke.userP_Personality == 12 || thisPoke.userP_Personality == 16)
        {
            realSPE.color = downStatusColor;
        }
        #endregion

        var icon = Button_poke.Find("Image_Icon").GetComponent<Image>();
        if (thisPoke.isDifferentColors)
        {
            icon.sprite = p_ImageDatas.sheet.Find(x => x.p_Id == thisPoke.userP_Id).p_ImageHand_C;
        }
        else
        {
            icon.sprite = p_ImageDatas.sheet.Find(x => x.p_Id == thisPoke.userP_Id).p_ImageHand;
        }
        Button_poke.Find("Text_PokeName").GetComponent<Text>().text = thisPoke.userP_Name;
        Button_poke.Find("Text_Level").GetComponent<Text>().text = $"Lv.{thisPoke.userP_Level}";

        var slider = Button_poke.Find("Slider").GetComponent<Slider>();
        slider.minValue = Mathf.Floor(thisPoke.userP_Real_Hp / 36.4f * -1);
        slider.maxValue = thisPoke.userP_Real_Hp;
        slider.value = thisPoke_OtherStatus.hp;
        if(slider.value <= slider.maxValue / 5)
        {
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color_HpBer[2];
        }
        else if(slider.value <= slider.maxValue / 2)
        {
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color_HpBer[1];
        }
        else
        {
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color_HpBer[0];
        }
        Button_poke.Find("Text_HPValue").GetComponent<Text>().text = $"{thisPoke_OtherStatus.hp}/{thisPoke.userP_Real_Hp}";
        var image_Gender = Button_poke.Find("Image_gender").GetComponent<Image>();
        image_Gender.gameObject.SetActive(true);
        if(thisPoke.userP_gender == 0)
        {
            image_Gender.sprite = Image_Gender[0];
        }
        else if (thisPoke.userP_gender == 1)
        {
            image_Gender.sprite = Image_Gender[1];
        }
        else if (thisPoke.userP_gender == 2)
        {
            image_Gender.gameObject.SetActive(false);
        }

        var situationText = poke_Situation.Find("Text_Value").GetComponent<Text>();
        situationText.text = "";
        for (int i = 1; i < 9; i++)
        {
            int rank = 0;
            switch (i)
            {
                case 1:
                    rank = thisPoke_BattleStatus.AscendingRank_Atk;
                    break;
                case 2:
                    rank = thisPoke_BattleStatus.AscendingRank_Def;
                    break;
                case 3:
                    rank = thisPoke_BattleStatus.AscendingRank_Sat;
                    break;
                case 4:
                    rank = thisPoke_BattleStatus.AscendingRank_Sde;
                    break;
                case 5:
                    rank = thisPoke_BattleStatus.AscendingRank_Spe;
                    break;
                case 6:
                    break;
                case 7:
                    rank = thisPoke_BattleStatus.AscendingRank_Hit;
                    break;
                case 8:
                    rank = thisPoke_BattleStatus.AscendingRank_Avo;
                    break;
            }
            if (i % 6 != 0)
            {
                if(rank >= 0)
                {
                    for (int r = 0; r < rank; r++)
                    {
                        situationText.text += "▲";
                    }
                    for (int t = 0; t < 6 - rank; t++)
                    {
                        situationText.text += "・";
                    }
                }
                else if(rank < 0)
                {
                    for (int r = 0; r < (rank*-1); r++)
                    {
                        situationText.text += "▼";
                    }
                    for (int t = 0; t < 6 + rank; t++)
                    {
                        situationText.text += "・";
                    }
                }
            }
            situationText.text += "\n";
        }

        Control_ModeSelect.SetActive(false);
        Control_Situation.SetActive(true);
    }

    public void EnemySituationDisplay()
    {
        var poke = Control_Situation.transform.Find("Poke").gameObject;

        poke.transform.Find("poke_Status").gameObject.SetActive(false);
        var Button_poke = poke.transform.Find("Button_poke");
        var poke_Situation = poke.transform.Find("poke_Situation");

        Control_Situation.transform.Find("Button_Front").gameObject.SetActive(false);
        Control_Situation.transform.Find("Button_BackGround").gameObject.SetActive(true);

        var thisPoke = BattleDatas_Default.enemy_PokemonData[enemy_PokeTeamNum];
        var thisPoke_OtherStatus = BattleDatas_Default.enemy_OthersStatus[enemy_PokeTeamNum];
        var thisPoke_BattleStatus = BattleDatas_Default.enemy_BattleStatus;

        var icon = Button_poke.Find("Image_Icon").GetComponent<Image>();
        if (thisPoke.isDifferentColors)
        {
            icon.sprite = p_ImageDatas.sheet.Find(x => x.p_Id == thisPoke.userP_Id).p_ImageHand_C;
        }
        else
        {
            icon.sprite = p_ImageDatas.sheet.Find(x => x.p_Id == thisPoke.userP_Id).p_ImageHand;
        }
        Button_poke.Find("Text_PokeName").GetComponent<Text>().text = thisPoke.userP_Name;
        Button_poke.Find("Text_Level").GetComponent<Text>().text = $"Lv.{thisPoke.userP_Level}";

        var slider = Button_poke.Find("Slider").GetComponent<Slider>();
        slider.minValue = Mathf.Floor(thisPoke.userP_Real_Hp / 36.4f * -1);
        slider.maxValue = thisPoke.userP_Real_Hp;
        slider.value = thisPoke_OtherStatus.hp;
        if (slider.value <= slider.maxValue / 5)
        {
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color_HpBer[2];
        }
        else if (slider.value <= slider.maxValue / 2)
        {
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color_HpBer[1];
        }
        else
        {
            slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color_HpBer[0];
        }
        Button_poke.Find("Text_HPValue").GetComponent<Text>().text = $"";
        var image_Gender = Button_poke.Find("Image_gender").GetComponent<Image>();
        image_Gender.gameObject.SetActive(true);
        if (thisPoke.userP_gender == 0)
        {
            image_Gender.sprite = Image_Gender[0];
        }
        else if (thisPoke.userP_gender == 1)
        {
            image_Gender.sprite = Image_Gender[1];
        }
        else if (thisPoke.userP_gender == 2)
        {
            image_Gender.gameObject.SetActive(false);
        }

        var situationText = poke_Situation.Find("Text_Value").GetComponent<Text>();
        situationText.text = "";
        for (int i = 1; i < 9; i++)
        {
            int rank = 0;
            switch (i)
            {
                case 1:
                    rank = thisPoke_BattleStatus.AscendingRank_Atk;
                    break;
                case 2:
                    rank = thisPoke_BattleStatus.AscendingRank_Def;
                    break;
                case 3:
                    rank = thisPoke_BattleStatus.AscendingRank_Sat;
                    break;
                case 4:
                    rank = thisPoke_BattleStatus.AscendingRank_Sde;
                    break;
                case 5:
                    rank = thisPoke_BattleStatus.AscendingRank_Spe;
                    break;
                case 6:
                    break;
                case 7:
                    rank = thisPoke_BattleStatus.AscendingRank_Hit;
                    break;
                case 8:
                    rank = thisPoke_BattleStatus.AscendingRank_Avo;
                    break;
            }
            if (i % 6 != 0)
            {
                if (rank >= 0)
                {
                    for (int r = 0; r < rank; r++)
                    {
                        situationText.text += "▲";
                    }
                    for (int t = 0; t < 6 - rank; t++)
                    {
                        situationText.text += "・";
                    }
                }
                else if (rank < 0)
                {
                    for (int r = 0; r < (rank * -1); r++)
                    {
                        situationText.text += "▼";
                    }
                    for (int t = 0; t < 6 + rank; t++)
                    {
                        situationText.text += "・";
                    }
                }
            }
            situationText.text += "\n";
        }

        Control_ModeSelect.SetActive(false);
        Control_Situation.SetActive(true);
    }

    public void ReplaceDisplay()
    {
        var poke1 = Control_ReplaceSelect.transform.Find("Poke1");
        var poke2 = Control_ReplaceSelect.transform.Find("Poke2");
        poke1.gameObject.SetActive(true);
        poke2.gameObject.SetActive(true);

        for (var ip = 0; ip < 2; ip++)
        {
            Control_ReplaceSelect.transform.Find("Button_Back").gameObject.SetActive(true);

            Transform poke = Control_ReplaceSelect.transform.Find("Poke1");

            int rePoke = 0;

            switch (ip)
            {
                case 0:
                    poke = poke1;
                    if(player_PokeTeamNum == 0 || player_PokeTeamNum == 255 && BattleDatas_Default.user_OthersStatus[0].hp <= 0)
                    {
                        rePoke = 1;
                    }
                    else if(player_PokeTeamNum == 1 || player_PokeTeamNum == 255 && BattleDatas_Default.user_OthersStatus[1].hp <= 0)
                    {
                        rePoke = 0;
                    }
                    else if (player_PokeTeamNum == 2 || player_PokeTeamNum == 255 && BattleDatas_Default.user_OthersStatus[2].hp <= 0)
                    {
                        rePoke = 0;
                    }
                    break;
                case 1:
                    poke = poke2;
                    if (player_PokeTeamNum == 0 || player_PokeTeamNum == 255 && BattleDatas_Default.user_OthersStatus[0].hp <= 0)
                    {
                        rePoke = 2;
                    }
                    else if (player_PokeTeamNum == 1 || player_PokeTeamNum == 255 && BattleDatas_Default.user_OthersStatus[1].hp <= 0)
                    {
                        rePoke = 2;
                    }
                    else if (player_PokeTeamNum == 2 || player_PokeTeamNum == 255 && BattleDatas_Default.user_OthersStatus[2].hp <= 0)
                    {
                        rePoke = 1;
                    }
                    break;
            }

            var poke_Status = poke.transform.Find("poke_Status");
            poke.transform.Find("poke_Status").gameObject.SetActive(true);
            var Button_poke = poke.transform.Find("Button_poke");

            Button_poke.transform.Find("condition").gameObject.SetActive(false);
            if (BattleDatas_Default.user_OthersStatus[rePoke].condition != BattleEnum.condition.none)
            {
                var condition = Button_poke.transform.Find("condition");
                condition.gameObject.SetActive(true);
                Color conditionColor = new Color();
                string conditionName = "";
                switch (BattleDatas_Default.user_OthersStatus[rePoke].condition)
                {
                    case BattleEnum.condition.paralysis:
                        conditionColor = Color_Condition[0];
                        conditionName = "まひ";
                        break;
                    case BattleEnum.condition.ice:
                        conditionColor = Color_Condition[1];
                        conditionName = "こおり";
                        break;
                    case BattleEnum.condition.burn:
                        conditionColor = Color_Condition[2];
                        conditionName = "やけど";
                        break;
                    case BattleEnum.condition.poison:
                        conditionColor = Color_Condition[3];
                        conditionName = "どく";
                        break;
                    case BattleEnum.condition.veryPoisonous:
                        conditionColor = Color_Condition[3];
                        conditionName = "どく";
                        break;
                    case BattleEnum.condition.sleep:
                        conditionColor = Color_Condition[4];
                        conditionName = "ねむり";
                        break;
                    case BattleEnum.condition.dying:
                        conditionColor = Color_Condition[5];
                        conditionName = "ひんし";
                        break;
                }
                condition.gameObject.GetComponent<Image>().color = conditionColor;
                condition.Find("Text").GetComponent<Text>().text = conditionName;
            }

            Button_poke.GetComponent<Button>().onClick.RemoveAllListeners();
            var tmp = rePoke;
            Button_poke.GetComponent<Button>().onClick.AddListener(() => replaceScript.Confirmation(tmp));
            Button_poke.GetComponent<Button>().onClick.AddListener(() => Audio_SE.PlayOneShot(click));

            Control_Situation.transform.Find("Button_Front").gameObject.SetActive(true);
            Control_Situation.transform.Find("Button_BackGround").gameObject.SetActive(false);

            var thisPoke = BattleDatas_Default.user_PokemonData[rePoke];
            var thisPoke_OtherStatus = BattleDatas_Default.user_OthersStatus[rePoke];

            var type1 = poke_Status.Find("Type1");
            Color buttonColor = new Color();
            string typeName = "";
            switch (thisPoke.userP_Type1)
            {
                case Pokémon_Type.Type.Normal:
                    buttonColor = Color_Type[0];
                    typeName = "ノーマル";
                    break;
                case Pokémon_Type.Type.Fire:
                    buttonColor = Color_Type[1];
                    typeName = "ほのお";
                    break;
                case Pokémon_Type.Type.Water:
                    buttonColor = Color_Type[2];
                    typeName = "みず";
                    break;
                case Pokémon_Type.Type.Electric:
                    buttonColor = Color_Type[3];
                    typeName = "でんき";
                    break;
                case Pokémon_Type.Type.Grass:
                    buttonColor = Color_Type[4];
                    typeName = "くさ";
                    break;
                case Pokémon_Type.Type.Ice:
                    buttonColor = Color_Type[5];
                    typeName = "こおり";
                    break;
                case Pokémon_Type.Type.Fighting:
                    buttonColor = Color_Type[6];
                    typeName = "かくとう";
                    break;
                case Pokémon_Type.Type.Poison:
                    buttonColor = Color_Type[7];
                    typeName = "どく";
                    break;
                case Pokémon_Type.Type.Ground:
                    buttonColor = Color_Type[8];
                    typeName = "じめん";
                    break;
                case Pokémon_Type.Type.Flying:
                    buttonColor = Color_Type[9];
                    typeName = "ひこう";
                    break;
                case Pokémon_Type.Type.Psychic:
                    buttonColor = Color_Type[10];
                    typeName = "エスパー";
                    break;
                case Pokémon_Type.Type.Bug:
                    buttonColor = Color_Type[11];
                    typeName = "むし";
                    break;
                case Pokémon_Type.Type.Rock:
                    buttonColor = Color_Type[12];
                    typeName = "いわ";
                    break;
                case Pokémon_Type.Type.Ghost:
                    buttonColor = Color_Type[13];
                    typeName = "ゴースト";
                    break;
                case Pokémon_Type.Type.Dragon:
                    buttonColor = Color_Type[14];
                    typeName = "ドラゴン";
                    break;
                case Pokémon_Type.Type.Dark:
                    buttonColor = Color_Type[15];
                    typeName = "あく";
                    break;
                case Pokémon_Type.Type.Steel:
                    buttonColor = Color_Type[16];
                    typeName = "はがね";
                    break;
                case Pokémon_Type.Type.Fairy:
                    buttonColor = Color_Type[17];
                    typeName = "フェアリー";
                    break;
            }
            type1.GetComponent<Image>().color = buttonColor;
            type1.Find("Text").GetComponent<Text>().text = typeName;
            var type2 = poke_Status.Find("Type2");
            type2.gameObject.SetActive(true);
            switch (thisPoke.userP_Type2)
            {
                case Pokémon_Type.Type.None:
                    type2.gameObject.SetActive(false);
                    break;
                case Pokémon_Type.Type.Normal:
                    buttonColor = Color_Type[0];
                    typeName = "ノーマル";
                    break;
                case Pokémon_Type.Type.Fire:
                    buttonColor = Color_Type[1];
                    typeName = "ほのお";
                    break;
                case Pokémon_Type.Type.Water:
                    buttonColor = Color_Type[2];
                    typeName = "みず";
                    break;
                case Pokémon_Type.Type.Electric:
                    buttonColor = Color_Type[3];
                    typeName = "でんき";
                    break;
                case Pokémon_Type.Type.Grass:
                    buttonColor = Color_Type[4];
                    typeName = "くさ";
                    break;
                case Pokémon_Type.Type.Ice:
                    buttonColor = Color_Type[5];
                    typeName = "こおり";
                    break;
                case Pokémon_Type.Type.Fighting:
                    buttonColor = Color_Type[6];
                    typeName = "かくとう";
                    break;
                case Pokémon_Type.Type.Poison:
                    buttonColor = Color_Type[7];
                    typeName = "どく";
                    break;
                case Pokémon_Type.Type.Ground:
                    buttonColor = Color_Type[8];
                    typeName = "じめん";
                    break;
                case Pokémon_Type.Type.Flying:
                    buttonColor = Color_Type[9];
                    typeName = "ひこう";
                    break;
                case Pokémon_Type.Type.Psychic:
                    buttonColor = Color_Type[10];
                    typeName = "エスパー";
                    break;
                case Pokémon_Type.Type.Bug:
                    buttonColor = Color_Type[11];
                    typeName = "むし";
                    break;
                case Pokémon_Type.Type.Rock:
                    buttonColor = Color_Type[12];
                    typeName = "いわ";
                    break;
                case Pokémon_Type.Type.Ghost:
                    buttonColor = Color_Type[13];
                    typeName = "ゴースト";
                    break;
                case Pokémon_Type.Type.Dragon:
                    buttonColor = Color_Type[14];
                    typeName = "ドラゴン";
                    break;
                case Pokémon_Type.Type.Dark:
                    buttonColor = Color_Type[15];
                    typeName = "あく";
                    break;
                case Pokémon_Type.Type.Steel:
                    buttonColor = Color_Type[16];
                    typeName = "はがね";
                    break;
                case Pokémon_Type.Type.Fairy:
                    buttonColor = Color_Type[17];
                    typeName = "フェアリー";
                    break;
            }
            type2.GetComponent<Image>().color = buttonColor;
            type2.Find("Text").GetComponent<Text>().text = typeName;

            var realValue = poke_Status.Find("RealValue");
            realValue.Find("ValueText_Hp").GetComponent<Text>().text = thisPoke.userP_Real_Hp.ToString();
            realValue.Find("ValueText_Atk").GetComponent<Text>().text = thisPoke.userP_Real_A.ToString();
            realValue.Find("ValueText_Def").GetComponent<Text>().text = thisPoke.userP_Real_B.ToString();
            realValue.Find("ValueText_Sat").GetComponent<Text>().text = thisPoke.userP_Real_C.ToString();
            realValue.Find("ValueText_Sde").GetComponent<Text>().text = thisPoke.userP_Real_D.ToString();
            realValue.Find("ValueText_Spe").GetComponent<Text>().text = thisPoke.userP_Real_S.ToString();

            poke_Status.Find("Characteristic").Find("Text_Characteristic").GetComponent<Text>().text = thisPoke.userP_Characteristic;
            if (thisPoke_OtherStatus.b_item == "" || thisPoke_OtherStatus.b_item == null)
            {
                poke_Status.Find("Belongings").Find("Text_Belongings").GetComponent<Text>().text = " - ";
            }
            else
            {
                var itemName = b_ImageDatas.sheet.Find(x => x.imageId == thisPoke_OtherStatus.b_item);
                poke_Status.Find("Belongings").Find("Text_Belongings").GetComponent<Text>().text = itemName.displayName;
            }

            var technique = poke_Status.Find("Technique");
            for (var i = 0; i < 4; i++)
            {
                string name = "";
                int pp = 0;
                switch (i)
                {
                    case 0:
                        name = thisPoke.set_Technique1;
                        pp = thisPoke_OtherStatus.pp_Technique1;
                        break;
                    case 1:
                        name = thisPoke.set_Technique2;
                        pp = thisPoke_OtherStatus.pp_Technique2;
                        break;
                    case 2:
                        name = thisPoke.set_Technique3;
                        pp = thisPoke_OtherStatus.pp_Technique3;
                        break;
                    case 3:
                        name = thisPoke.set_Technique4;
                        pp = thisPoke_OtherStatus.pp_Technique4;
                        break;
                }
                var techniqueData = DataLists.titleData_Technique.Find(x => x.t_Name == name);

                var techniqueIcon = technique.Find($"Technique{i + 1}");

                buttonColor = new Color();
                switch (techniqueData.t_Type)
                {
                    case "ノーマル":
                        buttonColor = Color_Type[0];
                        break;
                    case "ほのお":
                        buttonColor = Color_Type[1];
                        break;
                    case "みず":
                        buttonColor = Color_Type[2];
                        break;
                    case "でんき":
                        buttonColor = Color_Type[3];
                        break;
                    case "くさ":
                        buttonColor = Color_Type[4];
                        break;
                    case "こおり":
                        buttonColor = Color_Type[5];
                        break;
                    case "かくとう":
                        buttonColor = Color_Type[6];
                        break;
                    case "どく":
                        buttonColor = Color_Type[7];
                        break;
                    case "じめん":
                        buttonColor = Color_Type[8];
                        break;
                    case "ひこう":
                        buttonColor = Color_Type[9];
                        break;
                    case "エスパー":
                        buttonColor = Color_Type[10];
                        break;
                    case "むし":
                        buttonColor = Color_Type[11];
                        break;
                    case "いわ":
                        buttonColor = Color_Type[12];
                        break;
                    case "ゴースト":
                        buttonColor = Color_Type[13];
                        break;
                    case "ドラゴン":
                        buttonColor = Color_Type[14];
                        break;
                    case "あく":
                        buttonColor = Color_Type[15];
                        break;
                    case "はがね":
                        buttonColor = Color_Type[16];
                        break;
                    case "フェアリー":
                        buttonColor = Color_Type[17];
                        break;
                }
                techniqueIcon.GetComponent<Image>().color = buttonColor;
                techniqueIcon.Find("Text_PP").GetComponent<Text>().text = $"{pp}/{techniqueData.t_PP}";
                techniqueIcon.Find("Text_TechniqueName").GetComponent<Text>().text = techniqueData.t_Name;
            }

            #region//上昇数値と下降数値に色を付けて数値を表示する
            var image = realValue.Find("Image");
            var realATK = image.Find("a").GetComponent<Text>();
            var realDEF = image.Find("b").GetComponent<Text>();
            var realSATK = image.Find("c").GetComponent<Text>();
            var realSDEF = image.Find("d").GetComponent<Text>();
            var realSPE = image.Find("s").GetComponent<Text>();

            realATK.color = defaultColor;
            realDEF.color = defaultColor;
            realSATK.color = defaultColor;
            realSDEF.color = defaultColor;
            realSPE.color = defaultColor;
            if (thisPoke.userP_Personality == 1 || thisPoke.userP_Personality == 2 || thisPoke.userP_Personality == 3 || thisPoke.userP_Personality == 4)
            {
                realATK.color = upStatusColor;
            }
            else if (thisPoke.userP_Personality == 5 || thisPoke.userP_Personality == 9 || thisPoke.userP_Personality == 13 || thisPoke.userP_Personality == 17)
            {
                realATK.color = downStatusColor;
            }
            if (thisPoke.userP_Personality == 5 || thisPoke.userP_Personality == 6 || thisPoke.userP_Personality == 7 || thisPoke.userP_Personality == 8)
            {
                realDEF.color = upStatusColor;
            }
            else if (thisPoke.userP_Personality == 1 || thisPoke.userP_Personality == 10 || thisPoke.userP_Personality == 14 || thisPoke.userP_Personality == 18)
            {
                realDEF.color = downStatusColor;
            }
            if (thisPoke.userP_Personality == 9 || thisPoke.userP_Personality == 10 || thisPoke.userP_Personality == 11 || thisPoke.userP_Personality == 12)
            {
                realSATK.color = upStatusColor;
            }
            else if (thisPoke.userP_Personality == 2 || thisPoke.userP_Personality == 6 || thisPoke.userP_Personality == 15 || thisPoke.userP_Personality == 19)
            {
                realSATK.color = downStatusColor;
            }
            if (thisPoke.userP_Personality == 13 || thisPoke.userP_Personality == 14 || thisPoke.userP_Personality == 15 || thisPoke.userP_Personality == 16)
            {
                realSDEF.color = upStatusColor;
            }
            else if (thisPoke.userP_Personality == 3 || thisPoke.userP_Personality == 7 || thisPoke.userP_Personality == 11 || thisPoke.userP_Personality == 20)
            {
                realSDEF.color = downStatusColor;
            }
            if (thisPoke.userP_Personality == 17 || thisPoke.userP_Personality == 18 || thisPoke.userP_Personality == 19 || thisPoke.userP_Personality == 20)
            {
                realSPE.color = upStatusColor;
            }
            else if (thisPoke.userP_Personality == 4 || thisPoke.userP_Personality == 8 || thisPoke.userP_Personality == 12 || thisPoke.userP_Personality == 16)
            {
                realSPE.color = downStatusColor;
            }
            #endregion

            var icon = Button_poke.Find("Image_Icon").GetComponent<Image>();
            if (thisPoke.isDifferentColors)
            {
                icon.sprite = p_ImageDatas.sheet.Find(x => x.p_Id == thisPoke.userP_Id).p_ImageHand_C;
            }
            else
            {
                icon.sprite = p_ImageDatas.sheet.Find(x => x.p_Id == thisPoke.userP_Id).p_ImageHand;
            }
            Button_poke.Find("Text_PokeName").GetComponent<Text>().text = thisPoke.userP_Name;
            Button_poke.Find("Text_Level").GetComponent<Text>().text = $"Lv.{thisPoke.userP_Level}";

            var slider = Button_poke.Find("Slider").GetComponent<Slider>();
            slider.minValue = Mathf.Floor(thisPoke.userP_Real_Hp / 36.4f * -1);
            slider.maxValue = thisPoke.userP_Real_Hp;
            slider.value = thisPoke_OtherStatus.hp;
            if (slider.value <= slider.maxValue / 5)
            {
                slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color_HpBer[2];
            }
            else if (slider.value <= slider.maxValue / 2)
            {
                slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color_HpBer[1];
            }
            else
            {
                slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color_HpBer[0];
            }
            Button_poke.Find("Text_HPValue").GetComponent<Text>().text = $"{thisPoke_OtherStatus.hp}/{thisPoke.userP_Real_Hp}";
            var image_Gender = Button_poke.Find("Image_gender").GetComponent<Image>();
            image_Gender.gameObject.SetActive(true);
            if (thisPoke.userP_gender == 0)
            {
                image_Gender.sprite = Image_Gender[0];
            }
            else if (thisPoke.userP_gender == 1)
            {
                image_Gender.sprite = Image_Gender[1];
            }
            else if (thisPoke.userP_gender == 2)
            {
                image_Gender.gameObject.SetActive(false);
            }

            if(BattleDatas_Default.user_OthersStatus[rePoke].hp <= 0)
            {
                poke.gameObject.SetActive(false);
            }
        }

        Control_ModeSelect.SetActive(false);
        Control_ReplaceSelect.SetActive(true);
    }

    public void BackModeSelect()
    {
        textDisplay.text = "行動を選択してください";
    }
}
