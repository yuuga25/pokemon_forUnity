using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

public class SelectPokémon : MonoBehaviour
{
    [SerializeField] private GameObject SelectObject;
    [SerializeField] private GameObject ConfirmationObject;
    [SerializeField] private ImageData_Pokémon imageDatas;

    [SerializeField] private GameObject InputObject_NickName;
    [SerializeField] private GameObject ConfirmationObject_NickName;
    [SerializeField] private GameObject ErrorObject_NickName;
    [SerializeField] private GameObject NextExplanationObject;

    [Header("設定データ_確認画面")]
    [SerializeField] private Color[] textColor;
    [SerializeField] private Image pokeImage;
    [SerializeField] private Text pokeName;

    [Header("ニックネーム_入力確認画面")]
    [SerializeField] private GameObject Text;
    [SerializeField] private GameObject Text_NickName;

    [SerializeField] private GameObject Button;
    [SerializeField] private GameObject Button_NickName;

    [Header("ニックネーム入力画面")]
    [SerializeField] private Image pokeImage_NickName;
    [SerializeField] private GameObject text_error;
    [SerializeField] private InputField inputField_NickName;
    [SerializeField] private Text text_Decision_NickName;

    private int pokeId;

    private const string PASSWORD_CHARS = "0123456789ABCDEFGHIJKLMNPQRSTUVWXYZ";

    public void SelectPokémonButton(int pokémonType)
    {
        ConfirmationObject.SetActive(true);
        SelectObject.SetActive(false);
        switch (pokémonType)
        {
            case 1 : //フシギダネ
                pokeImage.sprite = imageDatas.sheet[0].p_ImageFront;
                pokeName.color = textColor[0];
                pokeName.text = "フシギダネ";
                pokeId = 1;
                break;
            case 2 : //ヒトカゲ
                pokeImage.sprite = imageDatas.sheet[4].p_ImageFront;
                pokeName.color = textColor[1];
                pokeName.text = "ヒトカゲ";
                pokeId = 4;
                break;
            case 3 : //ゼニガメ
                pokeImage.sprite = imageDatas.sheet[7].p_ImageFront;
                pokeName.color = textColor[2];
                pokeName.text = "ゼニガメ";
                pokeId = 7;
                break;
        }
    }

    public void Decision_Input_NickName()
    {
        string nickName = inputField_NickName.text;
        bool isMatch = false;
        foreach(var i in Controller_AC.errorName)
        {
            if(Regex.IsMatch(nickName, i, RegexOptions.IgnoreCase))
            {
                isMatch = true;
            }
        }
        if (isMatch)
        {
            ErrorObject_NickName.SetActive(true);
            InputObject_NickName.SetActive(false);
        }
        else
        {
            if(nickName.Length > 0)
            {
                ConfirmationObject_NickName.SetActive(true);
                InputObject_NickName.SetActive(false);
                int imageId = 0;
                switch (DataLists.playerData.pokémonsList[0].userP_Id)
                {
                    case 1:
                        imageId = 0;
                        text_Decision_NickName.color = textColor[0];
                        break;
                    case 4:
                        imageId = 4;
                        text_Decision_NickName.color = textColor[1];
                        break;
                    case 7:
                        imageId = 7;
                        text_Decision_NickName.color = textColor[2];
                        break;
                }
                if (DataLists.playerData.pokémonsList[0].isDifferentColors)
                {
                    pokeImage_NickName.sprite = imageDatas.sheet[imageId].p_ImageFront_C;
                }
                else
                {
                    pokeImage_NickName.sprite = imageDatas.sheet[imageId].p_ImageFront;
                }
                text_Decision_NickName.text = nickName;
            }
            else
            {
                StartCoroutine(falseMessage());
                text_error.SetActive(true);
            }
        }
    }

    public void Decision_Confirmation_NickName()
    {
        DataLists.playerData.pokémonsList[0].userP_NickName = text_Decision_NickName.text;
        log();
        CloseObject();
        InputObject_NickName.SetActive(false);
        NextExplanationObject.SetActive(true);
    }

    public void CloseObject()
    {
        ConfirmationObject_NickName.SetActive(false);
        ErrorObject_NickName.SetActive(false);
        InputObject_NickName.SetActive(true);
    }
    private IEnumerator falseMessage()
    {
        yield return new WaitForSeconds(1.5f);
        text_error.SetActive(false);
    }

    public void CancelSelect()
    {
        SelectObject.SetActive(true);
        ConfirmationObject.SetActive(false);
    }

    public void ConfirmButton()
    {
        SetPokémon(pokeId);
        SetPokémon(25);
        SetPokémon(133);
        var r3 = Random.Range(0, 101);
        if(r3 < 90) { SetPokémon(447); }
        else { SetPokémon(443); }
        DataLists.playerData.teamDatas.pokémons[1] = DataLists.playerData.pokémonsList[1];
        DataLists.playerData.teamDatas.pokémons[2] = DataLists.playerData.pokémonsList[2];
        DataLists.playerData.teamDatas.pokémons[3] = DataLists.playerData.pokémonsList[3];
        Text.SetActive(false);
        Text_NickName.SetActive(true);
        Button.SetActive(false);
        Button_NickName.SetActive(true);
    }

    public void Input_NickName()
    {
        ConfirmationObject.SetActive(false);
        InputObject_NickName.SetActive(true);
    }
    public void Cansel_NickName()
    {
        InputObject_NickName.SetActive(false);
        ConfirmationObject.SetActive(false);
        NextExplanationObject.SetActive(true);
    }

    private void SetPokémon(int pokeNum)
    {
        var pokémon = new UserData_Pokémon();

        var provisional_pokeId = DataLists.titleData_Pokémon.Where(x => x.p_Id == pokeNum);
        foreach(var i in provisional_pokeId)
        {
            //ユニークID
            {
                var sb = new System.Text.StringBuilder(12);

                for(int o = 0; o < 12; o++)
                {
                    int pos = Random.Range(0 ,PASSWORD_CHARS.Length);
                    char c = PASSWORD_CHARS[pos];
                    sb.Append(c);
                }

                pokémon.unique_Id = sb.ToString();
                sb = null;
            }
            //IDセット
            pokémon.userP_Id = i.p_Id;
            //名前
            pokémon.userP_Name = i.p_Name;
            //ニックネーム
            pokémon.userP_NickName = i.p_Name;
            //入っているボール
            pokémon.userP_Ball = Pokémon_Ball.Ball.Poké;
            //性別
            var r1 = Random.Range(1.0f, 100.0f);
            {
                if(255 == i.p_MaleProbability)//性別不明
                {
                    pokémon.userP_gender = 2;
                }
                else if(r1 <= i.p_MaleProbability)//オス
                {
                    pokémon.userP_gender = 0;
                }
                else if(r1 > i.p_MaleProbability)//メス
                {
                    pokémon.userP_gender = 1;
                }
            }
            //タイプ
            {
                switch (i.p_Type1)//タイプ1
                {
                    #region
                    case "ノーマル":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Normal;
                        break;
                    case "ほのお":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Fire;
                        break;
                    case "みず":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Water;
                        break;
                    case "でんき":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Electric;
                        break;
                    case "くさ":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Grass;
                        break;
                    case "こおり":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Ice;
                        break;
                    case "かくとう":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Fighting;
                        break;
                    case "どく":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Poison;
                        break;
                    case "じめん":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Ground;
                        break;
                    case "ひこう":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Flying;
                        break;
                    case "エスパー":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Psychic;
                        break;
                    case "むし":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Bug;
                        break;
                    case "いわ":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Rock;
                        break;
                    case "ゴースト":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Ghost;
                        break;
                    case "ドラゴン":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Dragon;
                        break;
                    case "あく":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Dark;
                        break;
                    case "はがね":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Steel;
                        break;
                    case "フェアリー":
                        pokémon.userP_Type1 = Pokémon_Type.Type.Fairy;
                        break;
                    case "" :
                    case null:
                        Debug.LogError("タイプが設定されていません");
                        break;
                    default:
                        pokémon.userP_Type1 = Pokémon_Type.Type.Normal;
                        break;
                    #endregion
                }
                switch (i.p_Type2)//タイプ2
                {
                    #region
                    case "ノーマル":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Normal;
                        break;
                    case "ほのお":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Fire;
                        break;
                    case "みず":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Water;
                        break;
                    case "でんき":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Electric;
                        break;
                    case "くさ":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Grass;
                        break;
                    case "こおり":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Ice;
                        break;
                    case "かくとう":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Fighting;
                        break;
                    case "どく":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Poison;
                        break;
                    case "じめん":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Ground;
                        break;
                    case "ひこう":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Flying;
                        break;
                    case "エスパー":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Psychic;
                        break;
                    case "むし":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Bug;
                        break;
                    case "いわ":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Rock;
                        break;
                    case "ゴースト":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Ghost;
                        break;
                    case "ドラゴン":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Dragon;
                        break;
                    case "あく":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Dark;
                        break;
                    case "はがね":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Steel;
                        break;
                    case "フェアリー":
                        pokémon.userP_Type2 = Pokémon_Type.Type.Fairy;
                        break;
                    case "":
                        pokémon.userP_Type2 = Pokémon_Type.Type.None;
                        break;
                    case null:
                        pokémon.userP_Type2 = Pokémon_Type.Type.None;
                        break;
                    default:
                        pokémon.userP_Type1 = Pokémon_Type.Type.Normal;
                        break;
                    #endregion
                }
            }
            //レベル
            pokémon.userP_Level = 5;
            //性格
            var r2 = Random.Range(1, 26);
            {
                pokémon.userP_Personality = r2;
            }
            //所持経験値
            switch (i.p_ExperienceType)
            {
                case 60:
                    if (2 < pokémon.userP_Level && pokémon.userP_Level <= 50)
                    {
                        pokémon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level, 3) * (100 - pokémon.userP_Level) / 50);
                    }
                    else if (50 <= pokémon.userP_Level && pokémon.userP_Level <= 68)
                    {
                        pokémon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level, 3) * (150 - pokémon.userP_Level) / 100);
                    }
                    else if (68 <= pokémon.userP_Level && pokémon.userP_Level <= 98)
                    {
                        pokémon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level, 3) * Mathf.FloorToInt(637 - 10 * pokémon.userP_Level / 3) / 500);
                    }
                    else if (98 <= pokémon.userP_Level && pokémon.userP_Level <= 100)
                    {
                        pokémon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level, 3) * (160 - pokémon.userP_Level) / 100);
                    }
                    break;
                case 80:
                    pokémon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level, 3) * 0.8f);
                    break;
                case 100:
                    pokémon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level, 3));
                    break;
                case 105:
                    pokémon.userP_ExperiencePoint = Mathf.FloorToInt(1.2f * Mathf.Pow(pokémon.userP_Level, 3) - (15 * Mathf.Pow(pokémon.userP_Level, 2)) + (100 * pokémon.userP_Level - 140));
                    break;
                case 125:
                    pokémon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level, 3) * 1.25f);
                    break;
                case 164:
                    if (2 <= pokémon.userP_Level && pokémon.userP_Level <= 15)
                    {
                        pokémon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level, 3) * (24 + Mathf.FloorToInt((pokémon.userP_Level + 1) / 3)) / 50);
                    }
                    else if (15 <= pokémon.userP_Level && pokémon.userP_Level <= 36)
                    {
                        pokémon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level, 3) * (14 + pokémon.userP_Level) / 50);
                    }
                    else if (36 <= pokémon.userP_Level && pokémon.userP_Level <= 100)
                    {
                        pokémon.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level, 3) * (32 + Mathf.FloorToInt(pokémon.userP_Level / 2)) / 50);
                    }
                    break;
            }
            //次のレベルまで
            switch (i.p_ExperienceType)
            {
                case 60:
                    if (2 <= pokémon.userP_Level + 1 && pokémon.userP_Level + 1 <= 50)
                    {
                        pokémon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level + 1, 3) * (100 - (pokémon.userP_Level + 1)) / 50);
                    }
                    else if (50 <= pokémon.userP_Level + 1 && pokémon.userP_Level + 1 <= 68)
                    {
                        pokémon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level + 1, 3) * (150 - (pokémon.userP_Level + 1)) / 100);
                    }
                    else if (68 <= pokémon.userP_Level + 1 && pokémon.userP_Level + 1 <= 98)
                    {
                        pokémon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level + 1, 3) * Mathf.FloorToInt(637 - 10 * (pokémon.userP_Level + 1) / 3) / 500);
                    }
                    else if (98 <= pokémon.userP_Level + 1 && pokémon.userP_Level + 1 <= 100)
                    {
                        pokémon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level + 1, 3) * (160 - (pokémon.userP_Level + 1)) / 100);
                    }
                    break;
                case 80:
                    pokémon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level + 1, 3) * 0.8f);
                    break;
                case 100:
                    pokémon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level + 1, 3));
                    break;
                case 105:
                    pokémon.userP_UntilLevelUp = Mathf.FloorToInt(1.2f * Mathf.Pow(pokémon.userP_Level + 1, 3) - (15 * Mathf.Pow(pokémon.userP_Level + 1, 2)) + (100 * (pokémon.userP_Level + 1) - 140));
                    print(pokémon.userP_UntilLevelUp);
                    break;
                case 125:
                    pokémon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level + 1, 3) * 1.25f);

                    break;
                case 164:
                    if (2 <= pokémon.userP_Level + 1 && pokémon.userP_Level + 1 <= 15)
                    {
                        pokémon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level + 1, 3) * (24 + Mathf.FloorToInt(((pokémon.userP_Level + 1) + 1) / 3)) / 50);
                    }
                    else if (15 <= pokémon.userP_Level + 1 && pokémon.userP_Level + 1 <= 36)
                    {
                        pokémon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level + 1, 3) * (14 + (pokémon.userP_Level + 1)) / 50);
                    }
                    else if (36 <= pokémon.userP_Level + 1 && pokémon.userP_Level + 1 <= 100)
                    {
                        pokémon.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(pokémon.userP_Level + 1, 3) * (32 + Mathf.FloorToInt((pokémon.userP_Level + 1) / 2)) / 50);
                    }
                    break;
            }
            //特性
            pokémon.userP_Characteristic = i.p_Characteristic;
            //色違い確率
            var r3 = Random.Range(1, 4097);
            if(r3 == 152)
            {
                pokémon.isDifferentColors = true;
            }
            else
            {
                pokémon.isDifferentColors = false;
            }
            //個体値
            {
                pokémon.userP_Individual_Hp = 5;
                pokémon.userP_Individual_A = 5;
                pokémon.userP_Individual_B = 5;
                pokémon.userP_Individual_C = 5;
                pokémon.userP_Individual_D = 5;
                pokémon.userP_Individual_S = 5;

                //強化レベル
                pokémon.userP_ELevel = 1;
            }
            //努力値
            {
                pokémon.userP_Effort_Hp = 0;
                pokémon.userP_Effort_A = 0;
                pokémon.userP_Effort_B = 0;
                pokémon.userP_Effort_C = 0;
                pokémon.userP_Effort_D = 0;
                pokémon.userP_Effort_S = 0;
            }
            //実数値
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
            }
            //技
            {
                var rememberData = DataLists.titleData_Remember.Where(x => x.p_T_Id == i.p_Id);
                foreach(var ii in rememberData)
                {
                    pokémon.set_Technique1 = ii.Technique1;
                    pokémon.set_TechniqueID1 = ii.TechniqueID1;
                    pokémon.set_Technique3 = ii.Technique3;
                    pokémon.set_TechniqueID3 = ii.TechniqueID3;
                    pokémon.set_Technique2 = ii.Technique5;
                    pokémon.set_TechniqueID2 = ii.TechniqueID5;
                    pokémon.set_Technique4 = ii.Technique8;
                    pokémon.set_TechniqueID4 = ii.TechniqueID8;

                }
            }
        }

        DataLists.playerData.pokémonsList.Add(pokémon);
        DataLists.playerData.teamDatas.pokémons[0] = DataLists.playerData.pokémonsList[0];
        log();
    }

    private void log()
    {
        var i = DataLists.playerData.pokémonsList.Last(x => x.userP_Id > 0);
        Debug.Log($"ユニークID{i.unique_Id}\n"+
                  $"ポケモンID:{i.userP_Id}\n" +
                  $"ポケモン名:{i.userP_Name}\n" +
                  $"ニックネーム:{i.userP_NickName}\n" +
                  $"ボール:{i.userP_Ball}\n" +
                  $"性別:{i.userP_gender}\n" +
                  $"タイプ1:{i.userP_Type1}\n" +
                  $"タイプ2:{i.userP_Type2}\n" +
                  $"レベル:{i.userP_Level}\n" +
                  $"性格:{i.userP_Personality}\n" +
                  $"所持経験値:{i.userP_ExperiencePoint}\n" +
                  $"次のレベルまで:{i.userP_UntilLevelUp}\n" +
                  $"特性:{i.userP_Characteristic}\n" +
                  $"色違い:{i.isDifferentColors}\n" +
                  $"現在のHP:{i.userP_CurrentHp}\n" +
                  $"強化レベル:{i.userP_ELevel}\n" +
                  $"実数値HP:{i.userP_Real_Hp}\n" +
                  $"実数値攻撃:{i.userP_Real_A}\n" +
                  $"実数値防御:{i.userP_Real_B}\n" +
                  $"実数値特攻:{i.userP_Real_C}\n" +
                  $"実数値特防:{i.userP_Real_D}\n" +
                  $"実数値素早さ:{i.userP_Real_S}");

        Debug.Log($"編成:ポケモン1:{DataLists.playerData.teamDatas.pokémons[0]}");
    }
}
