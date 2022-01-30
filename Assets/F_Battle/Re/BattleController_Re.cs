using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PlayFab;

public class BattleController_Re : MonoBehaviour
{
    public TurnScript_Re turnScript;
    public ReplaceScript_Re replaceScript;

    #region データ（スクリプタブルオブジェクト）
    [Header("データ（スクリプタブルオブジェクト）")]
    public ImageData_Pokémon p_ImageDatas;
    public VoiceData_Pokémon p_VoiceDatas;
    public CompatibilityData c_Data;
    public ImageData_Belongings b_ImageData;
    #endregion
    #region publicデータ
    [Header("publicデータ")]
    public List<Sprite> image_Ball;
    public List<Sprite> image_Classification;
    public List<Sprite> image_Gender;
    public List<Color> color_Type;
    public List<Color> color_Gender;
    public List<Color> color_HPBer;
    public List<Color> color_Condition;
    public Color color_Default;
    public Color color_UpStatus;
    public Color color_DownStatus;
    #endregion
    #region オーディオ関連
    [Header("オーディオ関連")]
    public AudioSource Audio_SE;
    public AudioSource Audio_Voice;
    public AudioClip click;
    #endregion
    #region UIオブジェクト
    [Header("UIオブジェクト")]
    public GameObject control_Panel;        //親オブジェクト
    public GameObject control_Modet;        //行動選択画面
    public GameObject control_Situation;    //場に出ているポケモンのステータス確認画面
    public GameObject control_Technique;    //技選択画面
    public GameObject control_Replace;      //入れ替え選択画面
    public GameObject control_Surrender;    //降参確認画面
    #endregion
    #region ModeSelectのオブジェクト
    [Header("ModeSelectのオブジェクト")]
    public GameObject onHand_List;          //手持ちリスト
    #endregion
    #region バトル場のオブジェクト
    [Header("バトル場のオブジェクト")]
    public List<GameObject> obj_Pokemon;
    public List<Animator> anim_Pokemon;

    public List<Slider> slider_HP;

    public Text text_Display;               //メッセージを入力するテキスト
    #endregion

    public int[] num_TeamPoke = new int[2];
    public string[] name_TeamPoke = new string[2];

    private void Awake()
    {
        //未ログインでタイトルに戻る
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            print("ログインしていません");
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }

    public void Start()
    {
        BattleDatas.battleDatas = new BattleDatas_Default[2];

        control_Panel.SetActive(true);

        control_Modet.SetActive(false);
        control_Situation.SetActive(false);
        control_Technique.SetActive(false);
        control_Replace.SetActive(false);
        control_Surrender.SetActive(false);

        #region 手持ちのポケモン表示
        for(int h = 0; h < 2; h++)
        {
            var poke = BattleDatas.battleDatas[0];
            var name = $"P{h+1}";

            for (int i = 0; i < 3; i++)
            {
                var image_UserPoke = onHand_List.transform.Find($"{name}_poke{i + 1}").GetComponent<Image>();
                var data = p_ImageDatas.sheet.Find(x => x.p_Id == poke.pokemonData[i].userP_Id);
                if (poke.pokemonData[i].isDifferentColors)
                {
                    image_UserPoke.sprite = data.p_ImageHand_C;
                }
                else
                {
                    image_UserPoke.sprite = data.p_ImageHand;
                }
            }
        }
        #endregion

        text_Display.text = "";

        int[] poke_Speed = new int[2] { 0, 0 };
        string[] poke_Chara = new string[2] { "", "" };
        AudioClip[] voice = new AudioClip[2];

        #region ポケモンの表示・UI
        for(var i = 0; i < 2; i++)
        {
            var image_Poke = obj_Pokemon[i].transform.Find($"Image_Poke_Player{i + 1}").GetComponent<Image>();
            var poke = BattleDatas.battleDatas[i].pokemonData[0];
            var pokeData = p_ImageDatas.sheet.Where(x => x.p_Id == poke.userP_Id);
            ImageData_Pokémon.GenderType genderType = new ImageData_Pokémon.GenderType();
            foreach(var p in pokeData)
            {
                if (p.genderType == ImageData_Pokémon.GenderType.same)
                {
                    genderType = ImageData_Pokémon.GenderType.same;
                }
                else
                {
                    if (poke.userP_gender == 0)
                    {
                        genderType = ImageData_Pokémon.GenderType.maleOnly;
                    }
                    else if (poke.userP_gender == 1)
                    {
                        genderType = ImageData_Pokémon.GenderType.femeleOnly;
                    }
                }
                break;
            }
            var image = pokeData.Where(x => x.genderType == genderType);
            foreach(var pp in image)
            {
                if(i == 0)
                {
                    if (poke.isDifferentColors)
                    {
                        image_Poke.sprite = pp.p_ImageBack_C;
                    }
                    else
                    {
                        image_Poke.sprite = pp.p_ImageBack;
                    }
                }
                else
                {
                    if (poke.isDifferentColors)
                    {
                        image_Poke.sprite = pp.p_ImageFront_C;
                    }
                    else
                    {
                        image_Poke.sprite = pp.p_ImageFront;
                    }
                }
                break;
            }

            var hpBer = obj_Pokemon[i].transform.Find($"HPBer_Player{i + 1}");
            var slider = hpBer.Find("Slider").GetComponent<Slider>();
            slider.minValue = Mathf.Floor(poke.userP_Real_Hp / 36.4f - 1);
            slider.maxValue = poke.userP_Real_Hp;
            slider.value = BattleDatas.battleDatas[i].othersStatus[0].hp;

            hpBer.transform.Find("condition").gameObject.SetActive(false);

            var otherStatus = BattleDatas.battleDatas[i].othersStatus;
            int pokeTeamNum = num_TeamPoke[i];
            string pokeTeamName = name_TeamPoke[i];

            if(otherStatus[pokeTeamNum].condition != BattleEnum.condition.none)
            {
                var condition = hpBer.transform.Find("condition");
                condition.gameObject.SetActive(true);
                Color conditionColor = new Color();
                string conditionName = "";
                switch (otherStatus[pokeTeamNum].condition)
                {
                    case BattleEnum.condition.paralysis:
                        conditionColor = color_Condition[0];
                        conditionName = "まひ";
                        break;
                    case BattleEnum.condition.ice:
                        conditionColor = color_Condition[1];
                        conditionName = "こおり";
                        break;
                    case BattleEnum.condition.burn:
                        conditionColor = color_Condition[2];
                        conditionName = "やけど";
                        break;
                    case BattleEnum.condition.poison:
                        conditionColor = color_Condition[3];
                        conditionName = "どく";
                        break;
                    case BattleEnum.condition.veryPoisonous:
                        conditionColor = color_Condition[3];
                        conditionName = "どく";
                        break;
                    case BattleEnum.condition.sleep:
                        conditionColor = color_Condition[4];
                        conditionName = "ねむり";
                        break;
                    case BattleEnum.condition.dying:
                        conditionColor = color_Condition[5];
                        conditionName = "ひんし";
                        break;
                }
                condition.gameObject.GetComponent<Image>().color = conditionColor;
                condition.Find("Text").GetComponent<Text>().text = conditionName;
            }

            var text_PokeName = hpBer.Find("Text_PokeName").GetComponent<Text>();

            //プレイヤーとエネミーで違う部分
            if (i == 0)
            {
                text_PokeName.text = poke.userP_NickName;
                hpBer.Find("Text_HPValue").GetComponent<Text>().text = $"{otherStatus[pokeTeamNum].hp}/{poke.userP_Real_Hp}";

                name_TeamPoke[i] = poke.userP_NickName;
            }
            else
            {
                text_PokeName.text = poke.userP_Name;

                name_TeamPoke[i] = poke.userP_Name;
            }

            var genderText = hpBer.Find("Text_Gender").GetComponent<Text>();
            genderText.gameObject.SetActive(true);
            if(poke.userP_gender == 0)
            {
                genderText.text = "♂";
                genderText.color = color_Gender[0];
            }
            else if(poke.userP_gender == 1)
            {
                genderText.text = "♀";
                genderText.color = color_Gender[1];
            }
            else if(poke.userP_gender == 2)
            {
                genderText.gameObject.SetActive(false);
            }
            hpBer.Find("Text_Level").GetComponent<Text>().text = poke.userP_Level.ToString();

            voice[i] = p_VoiceDatas.sheet.Find(x => x.p_Id == poke.userP_Id).voiceData;

            poke_Speed[i] = poke.userP_Real_S;
            if(otherStatus[i].b_item == "こだわりスカーフ")
            {
                poke_Speed[i] = Mathf.FloorToInt(poke_Speed[i] * 1.5f);
            }

            poke_Chara[i] = poke.userP_Characteristic;
        }
        #endregion

        StartCoroutine(InPokemon());

        IEnumerator InPokemon()
        {
            yield return new WaitForSeconds(3);
            anim_Pokemon[1].SetBool("p_In", true);
            Audio_SE.PlayOneShot(turnScript.clip_INOUT[0]);
            yield return new WaitForSeconds(0.25f);
            text_Display.text = $"{name_TeamPoke[1]}があらわれた！";
            yield return new WaitForSeconds(0.75f);
            Audio_Voice.PlayOneShot(voice[1]);
            yield return new WaitForSeconds(3);
            anim_Pokemon[0].SetBool("p_In", true);
            Audio_SE.PlayOneShot(turnScript.clip_INOUT[0]);
            text_Display.text = $"行け！{name_TeamPoke[0]}！";
            yield return new WaitForSeconds(1);
            Audio_Voice.PlayOneShot(voice[0]);
            yield return new WaitForSeconds(2);

            var p1_Image_Characteristic = obj_Pokemon[0].transform.Find("Image_Characteristic").gameObject;
            var p2_Image_Characteristic = obj_Pokemon[1].transform.Find("Image_Characteristic").gameObject;

            var player1 = BattleDatas.battleDatas[0].pokemonData[num_TeamPoke[0]];
            var player2 = BattleDatas.battleDatas[1].pokemonData[num_TeamPoke[1]];

            var anim1 = anim_Pokemon[0];
            var anim2 = anim_Pokemon[1];

            if(poke_Speed[0] < poke_Speed[1])
            {
                (player1, player2) = (player2, player1);
                (p1_Image_Characteristic, p2_Image_Characteristic) = (p2_Image_Characteristic, p1_Image_Characteristic);
                (anim1, anim2) = (anim2, anim1);
            }
            else if(poke_Speed[0] == poke_Speed[1])
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
            }
        }
    }
}
