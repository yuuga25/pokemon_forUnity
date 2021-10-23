using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Linq;
using System.Text.RegularExpressions;

public class GachaScript : MonoBehaviour
{
    public GameObject MenuButton_Static;
    public GameObject GachaObj;
    public GameObject ConfiObj;
    public GameObject GachaResultObj;

    public GameObject errorObj;

    public GameObject gacha_Unit;
    public GameObject parentObj;

    public GameObject closeButtonObj;

    public Menu_PokéController pokéController;

    public AudioClip open;

    private const string PASSWORD_CHARS = "0123456789ABCDEFGHIJKLMNPQRSTUVWXYZ";

    public void SetGacha()
    {
        GachaObj.SetActive(true);
        ConfiObj.SetActive(false);
        errorObj.SetActive(false);
        GachaResultObj.SetActive(false);

        MenuButton_Static.SetActive(true);

        pokéController.loading_Image.SetActive(true);
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        result =>
        {
            DataLists.player_Money = result.VirtualCurrency["GD"];
            DataLists.player_BattlePoint = result.VirtualCurrency["BP"];

            GachaObj.transform.Find("Display_GD").Find("Text_Value").GetComponent<Text>().text = $"{DataLists.player_Money}円";
            GachaObj.transform.Find("Display_BP").Find("Text_Value").GetComponent<Text>().text = $"{DataLists.player_BattlePoint}BP";

            GetUserTitleData();
        },
        error => { pokéController.error_Image.SetActive(true); });
    }

    private void GetUserTitleData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
        result =>
        {
            DataLists.playerData = PlayFabSimpleJson.DeserializeObject<PlayerData>(result.Data["PlayerData"].Value);
            pokéController.loading_Image.SetActive(false);
        }, error => { pokéController.error_Image.SetActive(true); });
    }

    public void Confirmation(int value)
    {
        if (150 * value > DataLists.player_BattlePoint)
        {
            errorObj.SetActive(true);
            errorObj.transform.Find("confirmationScreen").Find("Text").GetComponent<Text>().text = $"ガチャに必要なBPが\n足りていません。";
        }
        else if(DataLists.playerData.pokémonsList.Count >= 100)
        {
            errorObj.SetActive(true);
            errorObj.transform.Find("confirmationScreen").Find("Text").GetComponent<Text>().text = $"所持しているポケモンの数が\n最大数を超えています。";
        }
        else
        {
            ConfiObj.SetActive(true);

            ConfiObj.transform.Find("confirmationScreen").gameObject.transform.Find("Text_BP_Value").GetComponent<Text>().text = $"{150 * value}";
            ConfiObj.transform.Find("confirmationScreen").gameObject.transform.Find("Text_GachaCount").GetComponent<Text>().text = $"{value}";

            gachaValue = value;
        }
    }

    private int gachaValue;

    public void Gacha()
    {
        pokéController.loading_Image.SetActive(true);

        var ConsumptionValue = 150 * gachaValue;

        PlayFabClientAPI.SubtractUserVirtualCurrency(new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "BP",
            Amount = ConsumptionValue
        }, result =>
        {
            GachaObj.SetActive(false);
            ConfiObj.SetActive(false);
            GachaResultObj.SetActive(true);
            MenuButton_Static.SetActive(false);
            closeButtonObj.SetActive(false);

            foreach (Transform u in parentObj.transform)
            {
                Destroy(u.gameObject);
            }

            units = new List<GameObject>();

            for(int i = 0;i < gachaValue; i++)
            {
                GameObject unit = Instantiate(gacha_Unit, parentObj.transform);

                List<int> EmissionListNum = new List<int>();
                int ballNum = 0;
                #region ガチャ確率

                int r = Random.Range(0, 100);
                if(r >= 59)
                {
                    r = Random.Range(0, 100);
                    if (r >= 69)
                    {
                        r = Random.Range(0, 100);
                        if (r >= 79)
                        {
                            r = Random.Range(0, 100);
                            if (r >= 89)
                            {
                                EmissionListNum = EmissionListNum_5;
                                ballNum = 3;
                            }
                            else
                            {
                                EmissionListNum = EmissionListNum_4;
                                ballNum = 2;
                            }
                        }
                        else
                        {
                            EmissionListNum = EmissionListNum_3;
                            ballNum = 1;
                        }
                    }
                    else
                    {
                        EmissionListNum = EmissionListNum_2;
                        ballNum = 0;
                    }
                }
                else
                {
                    EmissionListNum = EmissionListNum_1;
                    ballNum = 0;
                }
                #endregion

                int r1 = Random.Range(0, EmissionListNum.Count);
                int pokeId = EmissionListNum[r1];
                var poke = new UserData_Pokémon();

                var ii = DataLists.titleData_Pokémon.Find(x => x.p_Id == pokeId);

                //ユニークID
                {
                    var sb = new System.Text.StringBuilder(12);

                    for(int o = 0; o < 12; o++)
                    {
                        int pos = Random.Range(0, PASSWORD_CHARS.Length);
                        char c = PASSWORD_CHARS[pos];
                        sb.Append(c);
                    }

                    poke.unique_Id = sb.ToString();
                    sb = null;
                }
                //IDセット
                poke.userP_Id = ii.p_Id;
                //名前
                poke.userP_Name = ii.p_Name;
                //ニックネーム
                poke.userP_NickName = ii.p_Name;
                //入っているボール
                switch (ballNum)
                {
                    case 0:
                        poke.userP_Ball = Pokémon_Ball.Ball.Poké;
                        break;
                    case 1:
                        poke.userP_Ball = Pokémon_Ball.Ball.Great;
                        break;
                    case 2:
                        poke.userP_Ball = Pokémon_Ball.Ball.Ultra;
                        break;
                    case 3:
                        poke.userP_Ball = Pokémon_Ball.Ball.Master;
                        break;
                }
                //性別
                var r2 = Random.Range(1.0f, 100.0f);
                {
                    if(255 == ii.p_MaleProbability)
                    {
                        poke.userP_gender = 2;
                    }
                    else if(r2 <= ii.p_MaleProbability)
                    {
                        poke.userP_gender = 0;
                    }
                    else if(r2 > ii.p_MaleProbability)
                    {
                        poke.userP_gender = 1;
                    }
                }
                //タイプ
                {
                    switch (ii.p_Type1)//タイプ1
                    {
                        #region
                        case "ノーマル":
                            poke.userP_Type1 = Pokémon_Type.Type.Normal;
                            break;
                        case "ほのお":
                            poke.userP_Type1 = Pokémon_Type.Type.Fire;
                            break;
                        case "みず":
                            poke.userP_Type1 = Pokémon_Type.Type.Water;
                            break;
                        case "でんき":
                            poke.userP_Type1 = Pokémon_Type.Type.Electric;
                            break;
                        case "くさ":
                            poke.userP_Type1 = Pokémon_Type.Type.Grass;
                            break;
                        case "こおり":
                            poke.userP_Type1 = Pokémon_Type.Type.Ice;
                            break;
                        case "かくとう":
                            poke.userP_Type1 = Pokémon_Type.Type.Fighting;
                            break;
                        case "どく":
                            poke.userP_Type1 = Pokémon_Type.Type.Poison;
                            break;
                        case "じめん":
                            poke.userP_Type1 = Pokémon_Type.Type.Ground;
                            break;
                        case "ひこう":
                            poke.userP_Type1 = Pokémon_Type.Type.Flying;
                            break;
                        case "エスパー":
                            poke.userP_Type1 = Pokémon_Type.Type.Psychic;
                            break;
                        case "むし":
                            poke.userP_Type1 = Pokémon_Type.Type.Bug;
                            break;
                        case "いわ":
                            poke.userP_Type1 = Pokémon_Type.Type.Rock;
                            break;
                        case "ゴースト":
                            poke.userP_Type1 = Pokémon_Type.Type.Ghost;
                            break;
                        case "ドラゴン":
                            poke.userP_Type1 = Pokémon_Type.Type.Dragon;
                            break;
                        case "あく":
                            poke.userP_Type1 = Pokémon_Type.Type.Dark;
                            break;
                        case "はがね":
                            poke.userP_Type1 = Pokémon_Type.Type.Steel;
                            break;
                        case "フェアリー":
                            poke.userP_Type1 = Pokémon_Type.Type.Fairy;
                            break;
                        case "":
                        case null:
                            Debug.LogError("タイプが設定されていません");
                            break;
                        default:
                            poke.userP_Type1 = Pokémon_Type.Type.Normal;
                            break;
                            #endregion
                    }
                    switch (ii.p_Type2)//タイプ2
                    {
                        #region
                        case "ノーマル":
                            poke.userP_Type2 = Pokémon_Type.Type.Normal;
                            break;
                        case "ほのお":
                            poke.userP_Type2 = Pokémon_Type.Type.Fire;
                            break;
                        case "みず":
                            poke.userP_Type2 = Pokémon_Type.Type.Water;
                            break;
                        case "でんき":
                            poke.userP_Type2 = Pokémon_Type.Type.Electric;
                            break;
                        case "くさ":
                            poke.userP_Type2 = Pokémon_Type.Type.Grass;
                            break;
                        case "こおり":
                            poke.userP_Type2 = Pokémon_Type.Type.Ice;
                            break;
                        case "かくとう":
                            poke.userP_Type2 = Pokémon_Type.Type.Fighting;
                            break;
                        case "どく":
                            poke.userP_Type2 = Pokémon_Type.Type.Poison;
                            break;
                        case "じめん":
                            poke.userP_Type2 = Pokémon_Type.Type.Ground;
                            break;
                        case "ひこう":
                            poke.userP_Type2 = Pokémon_Type.Type.Flying;
                            break;
                        case "エスパー":
                            poke.userP_Type2 = Pokémon_Type.Type.Psychic;
                            break;
                        case "むし":
                            poke.userP_Type2 = Pokémon_Type.Type.Bug;
                            break;
                        case "いわ":
                            poke.userP_Type2 = Pokémon_Type.Type.Rock;
                            break;
                        case "ゴースト":
                            poke.userP_Type2 = Pokémon_Type.Type.Ghost;
                            break;
                        case "ドラゴン":
                            poke.userP_Type2 = Pokémon_Type.Type.Dragon;
                            break;
                        case "あく":
                            poke.userP_Type2 = Pokémon_Type.Type.Dark;
                            break;
                        case "はがね":
                            poke.userP_Type2 = Pokémon_Type.Type.Steel;
                            break;
                        case "フェアリー":
                            poke.userP_Type2 = Pokémon_Type.Type.Fairy;
                            break;
                        case "":
                            poke.userP_Type2 = Pokémon_Type.Type.None;
                            break;
                        case null:
                            poke.userP_Type2 = Pokémon_Type.Type.None;
                            break;
                        default:
                            poke.userP_Type1 = Pokémon_Type.Type.Normal;
                            break;
                            #endregion
                    }
                }
                //レベル
                poke.userP_Level = Random.Range(1, 6);
                //性格
                var r3 = Random.Range(1, 26);
                {
                    poke.userP_Personality = r3;
                }
                //所持経験値
                switch (ii.p_ExperienceType)
                {
                    case 60:
                        if (2 < poke.userP_Level && poke.userP_Level <= 50)
                        {
                            poke.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level, 3) * (100 - poke.userP_Level) / 50);
                        }
                        else if (50 <= poke.userP_Level && poke.userP_Level <= 68)
                        {
                            poke.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level, 3) * (150 - poke.userP_Level) / 100);
                        }
                        else if (68 <= poke.userP_Level && poke.userP_Level <= 98)
                        {
                            poke.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level, 3) * Mathf.FloorToInt(637 - 10 * poke.userP_Level / 3) / 500);
                        }
                        else if (98 <= poke.userP_Level && poke.userP_Level <= 100)
                        {
                            poke.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level, 3) * (160 - poke.userP_Level) / 100);
                        }
                        break;
                    case 80:
                        poke.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level, 3) * 0.8f);
                        break;
                    case 100:
                        poke.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level, 3));
                        break;
                    case 105:
                        poke.userP_ExperiencePoint = Mathf.FloorToInt(1.2f * Mathf.Pow(poke.userP_Level, 3) - (15 * Mathf.Pow(poke.userP_Level, 2)) + (100 * poke.userP_Level - 140));
                        break;
                    case 125:
                        poke.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level, 3) * 1.25f);
                        break;
                    case 164:
                        if (2 <= poke.userP_Level && poke.userP_Level <= 15)
                        {
                            poke.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level, 3) * (24 + Mathf.FloorToInt((poke.userP_Level + 1) / 3)) / 50);
                        }
                        else if (15 <= poke.userP_Level && poke.userP_Level <= 36)
                        {
                            poke.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level, 3) * (14 + poke.userP_Level) / 50);
                        }
                        else if (36 <= poke.userP_Level && poke.userP_Level <= 100)
                        {
                            poke.userP_ExperiencePoint = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level, 3) * (32 + Mathf.FloorToInt(poke.userP_Level / 2)) / 50);
                        }
                        break;
                }
                if(poke.userP_Level == 1)
                {
                    poke.userP_ExperiencePoint = 0;
                }
                //次のレベルまで
                switch (ii.p_ExperienceType)
                {
                    case 60:
                        if (2 <= poke.userP_Level + 1 && poke.userP_Level + 1 <= 50)
                        {
                            poke.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level + 1, 3) * (100 - (poke.userP_Level + 1)) / 50);
                        }
                        else if (50 <= poke.userP_Level + 1 && poke.userP_Level + 1 <= 68)
                        {
                            poke.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level + 1, 3) * (150 - (poke.userP_Level + 1)) / 100);
                        }
                        else if (68 <= poke.userP_Level + 1 && poke.userP_Level + 1 <= 98)
                        {
                            poke.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level + 1, 3) * Mathf.FloorToInt(637 - 10 * (poke.userP_Level + 1) / 3) / 500);
                        }
                        else if (98 <= poke.userP_Level + 1 && poke.userP_Level + 1 <= 100)
                        {
                            poke.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level + 1, 3) * (160 - (poke.userP_Level + 1)) / 100);
                        }
                        break;
                    case 80:
                        poke.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level + 1, 3) * 0.8f);
                        break;
                    case 100:
                        poke.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level + 1, 3));
                        break;
                    case 105:
                        poke.userP_UntilLevelUp = Mathf.FloorToInt(1.2f * Mathf.Pow(poke.userP_Level + 1, 3) - (15 * Mathf.Pow(poke.userP_Level + 1, 2)) + (100 * (poke.userP_Level + 1) - 140));
                        print(poke.userP_UntilLevelUp);
                        break;
                    case 125:
                        poke.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level + 1, 3) * 1.25f);

                        break;
                    case 164:
                        if (2 <= poke.userP_Level + 1 && poke.userP_Level + 1 <= 15)
                        {
                            poke.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level + 1, 3) * (24 + Mathf.FloorToInt(((poke.userP_Level + 1) + 1) / 3)) / 50);
                        }
                        else if (15 <= poke.userP_Level + 1 && poke.userP_Level + 1 <= 36)
                        {
                            poke.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level + 1, 3) * (14 + (poke.userP_Level + 1)) / 50);
                        }
                        else if (36 <= poke.userP_Level + 1 && poke.userP_Level + 1 <= 100)
                        {
                            poke.userP_UntilLevelUp = Mathf.FloorToInt(Mathf.Pow(poke.userP_Level + 1, 3) * (32 + Mathf.FloorToInt((poke.userP_Level + 1) / 2)) / 50);
                        }
                        break;
                }
                //特性
                poke.userP_Characteristic = ii.p_Characteristic;
                //色違い確率
                var r4 = Random.Range(1, 4097);
                if(r4 == 152)
                {
                    poke.isDifferentColors = true;
                }
                else
                {
                    poke.isDifferentColors = false;
                }
                //個体値
                {
                    poke.userP_Individual_Hp = 5;
                    poke.userP_Individual_A = 5;
                    poke.userP_Individual_B = 5;
                    poke.userP_Individual_C = 5;
                    poke.userP_Individual_D = 5;
                    poke.userP_Individual_S = 5;

                    int r5;

                    #region 強化レベル
                    if (i == 9)
                    {
                        r5 = Random.Range(0, 100);
                        if(r5 >= 89)
                        {
                            r5 = Random.Range(0, 100);
                            if(r5 >= 89)
                            {
                                poke.userP_ELevel = 5;
                            }
                            else
                            {
                                poke.userP_ELevel = 4;
                            }
                        }
                        else
                        {
                            poke.userP_ELevel = 3;
                        }
                    }
                    else
                    {
                        r5 = Random.Range(0, 100);
                        if (r5 >= 59)
                        {
                            r5 = Random.Range(0, 100);
                            if (r5 >= 69)
                            {
                                r5 = Random.Range(0, 100);
                                if (r5 >= 79)
                                {
                                    r5 = Random.Range(0, 100);
                                    if (r5 >= 89)
                                    {
                                        poke.userP_ELevel = 5;
                                    }
                                    else
                                    {
                                        poke.userP_ELevel = 4;
                                    }
                                }
                                else
                                {
                                    poke.userP_ELevel = 3;
                                }
                            }
                            else
                            {
                                poke.userP_ELevel = 2;
                            }
                        }
                        else
                        {
                            poke.userP_ELevel = 1;
                        }
                    }
                    #endregion
                }
                //努力値
                {
                    poke.userP_Effort_Hp = 0;
                    poke.userP_Effort_A = 0;
                    poke.userP_Effort_B = 0;
                    poke.userP_Effort_C = 0;
                    poke.userP_Effort_D = 0;
                    poke.userP_Effort_S = 0;
                }
                //実数値
                {
                    //HP
                    poke.userP_Real_Hp = Mathf.FloorToInt((ii.race_H * 2 + poke.userP_Individual_Hp + poke.userP_Effort_Hp / 4) * poke.userP_Level / 100 + poke.userP_Level + 10);
                    poke.userP_CurrentHp = poke.userP_Real_Hp;
                    //攻撃
                    {
                        var poke_a = Mathf.FloorToInt((ii.race_A * 2 + poke.userP_Individual_A + poke.userP_Effort_A / 4) * poke.userP_Level / 100 + 5);
                        if (poke.userP_Personality == 1 || poke.userP_Personality == 2 || poke.userP_Personality == 3 || poke.userP_Personality == 4)
                        {
                            poke_a = Mathf.FloorToInt(poke_a * 1.1f);
                        }
                        else if (poke.userP_Personality == 5 || poke.userP_Personality == 9 || poke.userP_Personality == 13 || poke.userP_Personality == 17)
                        {
                            poke_a = Mathf.FloorToInt(poke_a * 0.9f);
                        }
                        poke.userP_Real_A = Mathf.FloorToInt(poke_a);
                    }
                    //防御
                    {
                        var poke_b = Mathf.FloorToInt((ii.race_B * 2 + poke.userP_Individual_B + poke.userP_Effort_B / 4) * poke.userP_Level / 100 + 5);
                        if (poke.userP_Personality == 5 || poke.userP_Personality == 6 || poke.userP_Personality == 7 || poke.userP_Personality == 8)
                        {
                            poke_b = Mathf.FloorToInt(poke_b * 1.1f);
                        }
                        else if (poke.userP_Personality == 1 || poke.userP_Personality == 10 || poke.userP_Personality == 14 || poke.userP_Personality == 18)
                        {
                            poke_b = Mathf.FloorToInt(poke_b * 0.9f);
                        }
                        poke.userP_Real_B = Mathf.FloorToInt(poke_b);
                    }
                    //特攻
                    {
                        var poke_c = Mathf.FloorToInt((ii.race_C * 2 + poke.userP_Individual_C + poke.userP_Effort_C / 4) * poke.userP_Level / 100 + 5);
                        if (poke.userP_Personality == 9 || poke.userP_Personality == 10 || poke.userP_Personality == 11 || poke.userP_Personality == 12)
                        {
                            poke_c = Mathf.FloorToInt(poke_c * 1.1f);
                        }
                        else if (poke.userP_Personality == 2 || poke.userP_Personality == 6 || poke.userP_Personality == 15 || poke.userP_Personality == 19)
                        {
                            poke_c = Mathf.FloorToInt(poke_c * 0.9f);
                        }
                        poke.userP_Real_C = Mathf.FloorToInt(poke_c);
                    }
                    //特防
                    {
                        var poke_d = Mathf.FloorToInt((ii.race_D * 2 + poke.userP_Individual_D + poke.userP_Effort_D / 4) * poke.userP_Level / 100 + 5);
                        if (poke.userP_Personality == 13 || poke.userP_Personality == 14 || poke.userP_Personality == 15 || poke.userP_Personality == 16)
                        {
                            poke_d = Mathf.FloorToInt(poke_d * 1.1f);
                        }
                        else if (poke.userP_Personality == 3 || poke.userP_Personality == 7 || poke.userP_Personality == 11 || poke.userP_Personality == 20)
                        {
                            poke_d = Mathf.FloorToInt(poke_d * 0.9f);
                        }
                        poke.userP_Real_D = Mathf.FloorToInt(poke_d);
                    }
                    //素早さ
                    {
                        var poke_s = Mathf.FloorToInt((ii.race_S * 2 + poke.userP_Individual_S + poke.userP_Effort_S / 4) * poke.userP_Level / 100 + 5);
                        if (poke.userP_Personality == 17 || poke.userP_Personality == 18 || poke.userP_Personality == 19 || poke.userP_Personality == 20)
                        {
                            poke_s = Mathf.FloorToInt(poke_s * 1.1f);
                        }
                        else if (poke.userP_Personality == 4 || poke.userP_Personality == 8 || poke.userP_Personality == 12 || poke.userP_Personality == 16)
                        {
                            poke_s = Mathf.FloorToInt(poke_s * 0.9f);
                        }
                        poke.userP_Real_S = Mathf.FloorToInt(poke_s);
                    }
                }
                //技
                {
                    var rememberData = DataLists.titleData_Remember.Where(x => x.p_T_Id == ii.p_Id);
                    foreach (var iii in rememberData)
                    {
                        poke.set_Technique1 = iii.Technique1;
                        poke.set_TechniqueID1 = iii.TechniqueID1;
                        poke.set_Technique3 = iii.Technique3;
                        poke.set_TechniqueID3 = iii.TechniqueID3;
                        poke.set_Technique2 = iii.Technique5;
                        poke.set_TechniqueID2 = iii.TechniqueID5;
                        poke.set_Technique4 = iii.Technique8;
                        poke.set_TechniqueID4 = iii.TechniqueID8;

                    }
                }

                DataLists.playerData.pokémonsList.Add(poke);
                for (int i1 = 0; i1 < DataLists.playerData.teamDatas.pokémons.Length; i1++)
                {
                    if (DataLists.playerData.teamDatas.pokémons[i1] == null)
                    {
                        DataLists.playerData.teamDatas.pokémons[i1] = poke;
                        break;
                    }
                }

                //背景色・タイプカラー
                int colorNum = 0;
                switch (poke.userP_Type1)
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
                unit.transform.Find("Image_FirstColor").gameObject.GetComponent<Image>().color = pokéController.typeColor[colorNum];
                switch (poke.userP_Type2)
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
                unit.transform.Find("Image_SecondColor").gameObject.GetComponent<Image>().color = pokéController.typeColor[colorNum];

                //手持ちポケモン画像
                var imageData_Hand = pokéController.imageData.sheet.Find(x => x.p_Id == poke.userP_Id);
                Sprite image = null;
                if (poke.isDifferentColors)
                {
                    image = imageData_Hand.p_ImageHand_C;
                }
                else if (!poke.isDifferentColors)
                {
                    image = imageData_Hand.p_ImageHand;
                }
                unit.transform.Find("Image_Poké").gameObject.GetComponent<Image>().sprite = image;

                //フレーム
                Color color1 = new Color();
                Color color2 = new Color();
                switch (poke.userP_ELevel)
                {
                    case 1:
                        color1 = pokéController.Color1_lv1;
                        color2 = pokéController.Color2_lv1;
                        break;
                    case 2:
                        color1 = pokéController.Color1_lv2;
                        color2 = pokéController.Color2_lv2;
                        break;
                    case 3:
                        color1 = pokéController.Color1_lv3;
                        color2 = pokéController.Color2_lv3;
                        break;
                    case 4:
                        color1 = pokéController.Color1_lv4;
                        color2 = pokéController.Color2_lv4;
                        break;
                    case 5:
                        color1 = pokéController.Color1_lv5;
                        color2 = pokéController.Color2_lv5;
                        break;
                }
                var Frame = unit.transform.Find("Frame").gameObject.GetComponent<UICornersGradient>();
                Frame.m_topLeftColor = color1;
                Frame.m_topRightColor = color2;
                Frame.m_bottomRightColor = color1;
                Frame.m_bottomLeftColor = color2;

                //レベル
                unit.transform.Find("Text_Level").gameObject.GetComponent<Text>().text = $"Lv.{poke.userP_Level}";

                //ボール
                int ballId = 0;
                switch (poke.userP_Ball)
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
                unit.transform.Find("Image").gameObject.GetComponent<Image>().sprite = pokéController.ballSprite[ballId];
                unit.transform.Find("Image_Ball").gameObject.GetComponent<Image>().sprite = pokéController.ballSprite[ballId];

                units.Add(unit);
            }

            UpdateUserData();
        },
        error => { pokéController.error_Image.SetActive(true); });
    }

    private void UpdateUserData()
    {
        PlayFabClientAPI.UpdateUserData(
        new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>()
            {
                {"PlayerData", PlayFabSimpleJson.SerializeObject(DataLists.playerData)}
            }
        }, result =>
        {
            pokéController.loading_Image.SetActive(false);
            StartCoroutine(display());
        }
        , error => { Debug.Log(error.GenerateErrorReport()); });
    }
    private List<GameObject> units;

    private IEnumerator display()
    {
        yield return new WaitForSeconds(0.2f);

        for(var i = 0; i < gachaValue; i++)
        {
            yield return new WaitForSeconds(0.5f);
            units[i].transform.Find("Image_FirstColor").gameObject.SetActive(true);
            units[i].transform.Find("Image_SecondColor").gameObject.SetActive(true);
            units[i].transform.Find("Image_FirstColorC").gameObject.SetActive(true);
            units[i].transform.Find("Image_Poké").gameObject.SetActive(true);
            units[i].transform.Find("Frame").gameObject.SetActive(true);
            units[i].transform.Find("Text_Level").gameObject.SetActive(true);
            units[i].transform.Find("Image_Ball").gameObject.SetActive(true);
            units[i].transform.Find("Image").gameObject.SetActive(false);
            pokéController.audioManager_SE.PlayOneShot(open);
        }

        closeButtonObj.SetActive(true);
    }

    private List<int> EmissionListNum_1 = new List<int>
    {
        1,4,7,1,4,7,280,447
    };
    private List<int> EmissionListNum_2 = new List<int>
    {
        25,133,2,5,8,443,281
    };
    private List<int> EmissionListNum_3 = new List<int>
    {
        444,444,3,6,9,
    };
    private List<int> EmissionListNum_4 = new List<int>
    {
        445,282,448,282,448,
    };
    private List<int> EmissionListNum_5 = new List<int>
    {
        644,1,4,7
    };
}
